using System;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Abp.Configuration;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Monivault.AppModels;
using Monivault.Configuration;
using Monivault.Profiles.Dto;
using ILogger = Castle.Core.Logging.ILogger;

namespace Monivault.SavingsInterests
{
    public class SavingsInterestManager : MonivaultServiceBase, ISingletonDependency
    {
        private const string SavingsInterestJobId = "Monivault-SavingInterestProcessor";
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
            var savingsInterests = _savingsInterestRepository.Query(x => x.Where(p => p.Status == SavingsInterest.StatusTypes.Running)
                                                                .Include(p => p.AccountHolder)
                                                                .ThenInclude(p => p.User).ToList());
            Logger.Info($"Savings interest size: {savingsInterests.Count}");
                foreach (var savingsInterest in savingsInterests)
                {
                    var accountHolder1 = savingsInterest.AccountHolder;
                    Logger.Info($"the account holder Id: {accountHolder1.AccountIdentity}");
                    var user = accountHolder1.User;
                    Logger.Info($"The user firstname: {user.Name}");
                    Logger.Info($"Calculating interest for: {savingsInterest.AccountHolder.User.Name}");
                    //Check if user performed transaction the previous day. This is because interest calculation runs at 00.05am.
                    var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TimeZoneInfo.FindSystemTimeZoneById("Africa/Lagos"));

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
                                Logger.Info($"Credit log: {log.Amount}");
                                savingsInterest.InterestPrincipal = savingsInterest.InterestPrincipal + log.Amount;
                                break;
                            case TransactionLog.TransactionTypes.Debit:
                                Logger.Info($"Debit log: {log.Amount}");
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
                        Logger.Info($"Rate calculation: {rateCalc}");
                        var timeCalc = Decimal.Divide(1, 365);
                        Logger.Info($"Time Calculation: {timeCalc}");
                        var timeRateCalc = rateCalc * timeCalc;
                        Logger.Info($"TimeRateCalc: {timeRateCalc}");

                        var todayInterest = savingsInterest.InterestPrincipal * timeRateCalc;
                        Logger.Info($"Today Interest: {todayInterest}");
                        var newAccruedInterest = savingsInterest.InterestAccrued + todayInterest;
                        Logger.Info($"New Accroued Interest: {newAccruedInterest}");

                        savingsInterestDetail.TodayInterest = todayInterest;
                        savingsInterestDetail.PrincipalBeforeTodayCalculation = savingsInterest.InterestPrincipal;

                        var interestType = await SettingManager.GetSettingValueAsync(AppSettingNames.InterestType);
                        Logger.Info($"Interest type: {interestType}");
                        switch (interestType)
                        {
                            case SavingsInterestDetail.InterestTypes.SimpleInterest:
                                Logger.Info("Interest type is just simple");
                                savingsInterest.InterestAccrued = newAccruedInterest;
                                savingsInterestDetail.PrincipalAfterTodayCalculation =
                                    savingsInterest.InterestPrincipal;
                                savingsInterestDetail.InterestType = interestType;
                                break;
                            case SavingsInterestDetail.InterestTypes.CompoundInterest:
                                Logger.Info("Interest type is just compound");
                                savingsInterest.InterestAccrued = newAccruedInterest;
                                var newPrincipal = savingsInterest.InterestPrincipal + todayInterest;
                                Logger.Info($"New principal: {newPrincipal}");

                                savingsInterest.InterestPrincipal = newPrincipal;
                                savingsInterestDetail.PrincipalAfterTodayCalculation = newPrincipal;
                                savingsInterestDetail.InterestType = interestType;
                                break;
                        }
                    }

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

        /*public async Task CheckSavingsInterestProcessingStatus()
        {
            Logger.Info("About to check if InterestStatus is running...");
            var savingsInterestStatus = await SettingManager.GetSettingValueForApplicationAsync<bool>(AppSettingNames.InterestStatus);
            Logger.Info("Interest status: " + savingsInterestStatus);
            if (savingsInterestStatus)
            {
                Logger.Info("Interest status is running");
                Logger.Info("Savings interest processing about to be started...");
                RecurringJob.AddOrUpdate<SavingsInterestManager>(SavingsInterestJobId, sm => sm.RunInterestForTheDay(), Cron.Daily(0, 5), 
                    TimeZoneInfo.FindSystemTimeZoneById("Africa/Lagos"));
                Logger.Info("Savings interest processing started.");
            }
        }*/

        public static void StartSavingsInterestProcessing()
        {
            //Startup Hangfire RecurringJob that processes SavingInterest calculation every midnight
            RecurringJob.AddOrUpdate<SavingsInterestManager>(SavingsInterestJobId, sm => sm.RunInterestForTheDay(), Cron.Daily(0, 10), 
                TimeZoneInfo.FindSystemTimeZoneById("Africa/Lagos"));
        }

        public static void StopSavingsInterestProcessing()
        {
            RecurringJob.RemoveIfExists(SavingsInterestJobId);
        }
    }
}