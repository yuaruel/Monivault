using Abp.Dependency;
using Abp.Domain.Repositories;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Monivault.AppModels;
using Monivault.Configuration;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using TimeZoneConverter;

namespace Monivault.SavingsInterests
{
    public class SavingsInterestManager : MonivaultServiceBase, ISingletonDependency
    {
        public const string SavingsInterestJobId = "Monivault-SavingInterestProcessor";
        private readonly IRepository<SavingsInterest, long> _savingsInterestRepository;
        private readonly IRepository<TransactionLog, long> _transactionLogRepository;
        private readonly IRepository<SavingsInterestDetail, long> _savingInterestDetailRepository;
        private readonly IRepository<AccountHolder> _accountHolderRepository;

        public SavingsInterestManager(
                IRepository<SavingsInterest, long> savingsInterestRepository,
                IRepository<TransactionLog, long> transactionLogRepository,
                IRepository<SavingsInterestDetail, long> savingInterestDetailRepository,
                IRepository<AccountHolder> accountHolderRepository
            )
        {
            _savingsInterestRepository = savingsInterestRepository;
            _transactionLogRepository = transactionLogRepository;
            _savingInterestDetailRepository = savingInterestDetailRepository;
            _accountHolderRepository = accountHolderRepository;
        }
        
       
        public async Task RunInterestForTheDay()
        {
            var savingsInterestStatus = bool.Parse(await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.InterestStatus));

            if (savingsInterestStatus)
            {
                Logger.Info($"Running interest calculation for the day at {DateTimeOffset.UtcNow}");
                var savingsInterests = _savingsInterestRepository.Query(x => x.Where(p => p.Status == SavingsInterest.StatusTypes.Running)
                                                    .Include(p => p.AccountHolder)
                                                    .ThenInclude(p => p.User).ToList());

                foreach (var savingsInterest in savingsInterests)
                {
                    var accountHolder1 = savingsInterest.AccountHolder;

                    var user = accountHolder1.User;

                    //Check if user performed transaction the previous day. This is because interest calculation runs at 00.05am.
                    var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TZConvert.GetTimeZoneInfo("Africa/Lagos"));

                    var yesterday = today.AddHours(-2);
                    var yesterdaysTransactions = _transactionLogRepository.Query(x =>
                        x.Where(p => p.AccountHolderId == savingsInterest.AccountHolderId)
                            .Where(p => p.CreationTime.Date == yesterday.Date).ToList());

                    var principalBeforeCalculation = savingsInterest.InterestPrincipal;
                    yesterdaysTransactions.ForEach(log =>
                    {
                        switch (log.TransactionType)
                        {
                            case TransactionLog.TransactionTypes.Credit:
                                savingsInterest.InterestPrincipal = savingsInterest.InterestPrincipal + log.Amount;
                                break;
                            case TransactionLog.TransactionTypes.Debit:
                                savingsInterest.InterestPrincipal = savingsInterest.InterestPrincipal - log.Amount;
                                savingsInterest.IsTransactionDebit = true;
                                break;
                        }
                    });

                    var savingsInterestDetail = new SavingsInterestDetail
                    {
                        AccruedInterestBeforeToday = savingsInterest.InterestAccrued
                    };

                    if (savingsInterest.IsTransactionDebit)
                    {
                        var penaltyChargeRate = decimal.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.PenaltyPercentageDeduction)) / 100;
                        var penaltyCharge = savingsInterest.InterestAccrued * penaltyChargeRate;
                        var newInterest = savingsInterest.InterestAccrued - penaltyCharge;


                        savingsInterestDetail.PenaltyCharge = penaltyCharge;
                        savingsInterestDetail.PrincipalAfterTodayCalculation = savingsInterest.InterestPrincipal;
                        savingsInterestDetail.PrincipalBeforeTodayCalculation = principalBeforeCalculation;

                        savingsInterest.InterestAccrued = newInterest;
                    }
                    else
                    {
                        var rateCalc = decimal.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.InterestRate)) / 100;
                        var timeCalc = Decimal.Divide(1, 365);
                        var timeRateCalc = rateCalc * timeCalc;

                        var todayInterest = savingsInterest.InterestPrincipal * timeRateCalc;
                        var newAccruedInterest = savingsInterest.InterestAccrued + todayInterest;

                        savingsInterestDetail.TodayInterest = todayInterest;
                        savingsInterestDetail.PrincipalBeforeTodayCalculation = savingsInterest.InterestPrincipal;

                        var interestType = await SettingManager.GetSettingValueAsync(AppSettingNames.InterestType);

                        switch (interestType)
                        {
                            case SavingsInterestDetail.InterestTypes.SimpleInterest:
                                savingsInterest.InterestAccrued = newAccruedInterest;
                                savingsInterestDetail.PrincipalAfterTodayCalculation =
                                    savingsInterest.InterestPrincipal;
                                savingsInterestDetail.InterestType = interestType;
                                break;
                            case SavingsInterestDetail.InterestTypes.CompoundInterest:
                                savingsInterest.InterestAccrued = newAccruedInterest;
                                var newPrincipal = savingsInterest.InterestPrincipal + todayInterest;

                                savingsInterest.InterestPrincipal = newPrincipal;
                                savingsInterestDetail.PrincipalAfterTodayCalculation = newPrincipal;
                                savingsInterestDetail.InterestType = interestType;
                                break;
                        }
                    }

                    //Update SavingsInterest
                    _savingsInterestRepository.Update(savingsInterest);

                    savingsInterestDetail.SavingsInterestId = savingsInterest.Id;
                    _savingInterestDetailRepository.Insert(savingsInterestDetail);

                    //Check if Interest EndDate has reached, for payout.
                    if (DateTime.UtcNow.Date.CompareTo(savingsInterest.EndDate.Date) < 0) continue;

                    var accountHolder = savingsInterest.AccountHolder;
                    accountHolder.AvailableBalance += savingsInterest.InterestAccrued;

                    savingsInterest.Status = SavingsInterest.StatusTypes.Completed;
                    CurrentUnitOfWork.SaveChanges();

                    //Create a new savings Interest
                    await BootstrapNewSavingsInterestForAccountHolder(accountHolder.Id);
                }
            }
            else
            {
                Logger.Info("Interest calculation is on pause.");
                RecurringJob.RemoveIfExists(SavingsInterestJobId);
            }
        }

        public async Task BootstrapNewSavingsInterestForAllAccountHolders()
        {
            var accountHolders = _accountHolderRepository.GetAllList();

            foreach (var accountHolder in accountHolders)
            {
                //Check if accountHolder already has a savingsInterest running.
                var savingInterest = _savingsInterestRepository.Query(x =>
                    x.Where(p => p.AccountHolderId == accountHolder.Id)
                        .Where(p => p.Status == SavingsInterest.StatusTypes.Running)).FirstOrDefault();

                if (savingInterest == null)
                {
                    //Logger.Info("About to bootstrap the account holder");
                    await BootstrapNewSavingsInterestForAccountHolder(accountHolder.Id);   
                }
            }
        }

        public async Task BootstrapNewSavingsInterestForAccountHolder(int accountHolderId)
        {
            var accountHolder = _accountHolderRepository.Get(accountHolderId);
            var interestDuration = int.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.InterestDuration));

            var interestEndDate = DateTime.UtcNow.Date.AddDays(interestDuration);
            var savingsInterest = new SavingsInterest
            {
                InterestPrincipal = accountHolder.AvailableBalance,
                AccountHolderId = accountHolderId,
                Status = SavingsInterest.StatusTypes.Running,
                EndDate = interestEndDate
            };

            _savingsInterestRepository.Insert(savingsInterest);
            CurrentUnitOfWork.SaveChanges();
        }

        public async Task CheckSavingsInterestProcessingStatus()
        {
            //Logger.Info("About to check if InterestStatus is running...");
            var savingsInterestStatus = bool.Parse(await SettingManager.GetSettingValueForApplicationAsync(AppSettingNames.InterestStatus));
            //Logger.Info("Interest status: " + savingsInterestStatus);
            if (savingsInterestStatus)
            {
                //Logger.Info("Interest status is running");
                //Logger.Info("Savings interest processing about to be started...");
                RecurringJob.AddOrUpdate<SavingsInterestManager>(SavingsInterestJobId, sm => sm.RunInterestForTheDay(), Cron.Daily(0, 5), 
                    TZConvert.GetTimeZoneInfo("Africa/Lagos"));
                //Logger.Info("Savings interest processing started.");
            }
        }

        public static void StartSavingsInterestProcessing()
        {
            //Startup Hangfire RecurringJob that processes SavingInterest calculation every midnight
            RecurringJob.AddOrUpdate<SavingsInterestManager>(SavingsInterestJobId, sm => sm.RunInterestForTheDay(), Cron.Daily(0, 5),
                TZConvert.GetTimeZoneInfo("Africa/Lagos"));
        }

        public static void StopSavingsInterestProcessing()
        {
            RecurringJob.RemoveIfExists(SavingsInterestJobId);
        }
    }
}