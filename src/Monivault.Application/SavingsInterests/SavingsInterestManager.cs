using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Monivault.AppModels;
using Monivault.Configuration;
using Monivault.ModelServices;
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
        private readonly NotificationScheduler _notificationScheduler;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private static int interestRunCount = 1;

        public SavingsInterestManager(
                IRepository<SavingsInterest, long> savingsInterestRepository,
                IRepository<TransactionLog, long> transactionLogRepository,
                IRepository<SavingsInterestDetail, long> savingInterestDetailRepository,
                IRepository<AccountHolder> accountHolderRepository,
                NotificationScheduler notificationScheduler,
                IUnitOfWorkManager unitOfWorkManager
            )
        {
            _savingsInterestRepository = savingsInterestRepository;
            _transactionLogRepository = transactionLogRepository;
            _savingInterestDetailRepository = savingInterestDetailRepository;
            _accountHolderRepository = accountHolderRepository;
            _notificationScheduler = notificationScheduler;
            _unitOfWorkManager = unitOfWorkManager;
        }
        
       
        public async Task RunInterestForTheDay()
        {
            var savingsInterestStatus = bool.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.InterestStatus));

            if (savingsInterestStatus)
            {
                Logger.Info($"Running interest calculation for the day at {DateTimeOffset.UtcNow}");
                var savingsInterests = _savingsInterestRepository.Query(x => x.Where(p => p.Status == SavingsInterest.StatusTypes.Running)
                                                    .Include(p => p.AccountHolder)
                                                    .ThenInclude(p => p.User).ToList());
                Logger.Info($"Interest run No. {interestRunCount++}");
                foreach (var savingsInterest in savingsInterests)
                {
                    var today = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, TZConvert.GetTimeZoneInfo("Africa/Lagos"));

                    //Check if this current savings interest has been modifided today.
                    if (savingsInterest.LastModificationTime.HasValue && (savingsInterest.LastModificationTime.Value.Date.CompareTo(today.Date) == 0)) continue;

                    using(var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        Logger.Info($"SavingsInterest accoundholderId: {savingsInterest.AccountHolderId}");
                        //TODO Add logic to check the Saving Interest Detail, to know if an interest calculation has been done for an account holder for the day.

                        //Check if user performed transaction the previous day. This is because interest calculation runs at 00.05am.

                        Logger.Info($"Today date: {today}");
                        var yesterday = today.AddHours(-2);
                        Logger.Info($"Yesterday date: {yesterday}");
                        var yesterdaysTransactions = _transactionLogRepository.Query(x =>
                            x.Where(p => p.AccountHolderId == savingsInterest.AccountHolderId).Where(p => p.CreationTime.Date == yesterday.Date).ToList());
                        Logger.Info($"Yesterday transaction size: {yesterdaysTransactions.Count}");
                        var principalBeforeCalculation = savingsInterest.InterestPrincipal;
                        Logger.Info($"Principal before calculation: {principalBeforeCalculation}");
                        yesterdaysTransactions.ForEach(log =>
                        {
                            Logger.Info($"log type: {log.TransactionType}");
                            switch (log.TransactionType)
                            {
                                case TransactionLog.TransactionTypes.Credit:
                                    savingsInterest.InterestPrincipal = savingsInterest.InterestPrincipal + log.Amount;
                                    Logger.Info($"interest principal after credit transaction: {savingsInterest.InterestPrincipal}");
                                    break;
                                case TransactionLog.TransactionTypes.Debit:
                                    savingsInterest.InterestPrincipal = savingsInterest.InterestPrincipal - log.Amount;
                                    Logger.Info($"interest principal after debit transaction: {savingsInterest.InterestPrincipal}");
                                    savingsInterest.IsTransactionDebit = true;
                                    break;
                            }
                        });

                        var savingsInterestDetail = new SavingsInterestDetail
                        {
                            AccruedInterestBeforeToday = savingsInterest.InterestAccrued
                        };

                        //Calculate todays interest.
                        var rateCalc = decimal.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.InterestRate)) / 100;
                        Logger.Info($"rate calculation: {rateCalc}");
                        var timeCalc = decimal.Divide(1, 365);
                        Logger.Info($"time calculation: {timeCalc}");
                        var timeRateCalc = rateCalc * timeCalc;
                        Logger.Info($"time rate calc: {timeRateCalc}");

                        Logger.Info($"interest principal after transaction adjusment: {savingsInterest.InterestPrincipal}");
                        var todayInterest = savingsInterest.InterestPrincipal * timeRateCalc;
                        Logger.Info($"today interest: {todayInterest}");
                        var newAccruedInterest = savingsInterest.InterestAccrued + todayInterest;
                        Logger.Info($"New Accrued interest: {newAccruedInterest}");

                        savingsInterestDetail.TodayInterest = todayInterest;
                        savingsInterestDetail.PrincipalBeforeTodayCalculation = principalBeforeCalculation;

                        var interestType = await SettingManager.GetSettingValueAsync(AppSettingNames.InterestType);

                        switch (interestType)
                        {
                            case SavingsInterestDetail.InterestTypes.SimpleInterest:
                                savingsInterest.InterestAccrued = newAccruedInterest;
                                savingsInterestDetail.PrincipalAfterTodayCalculation = savingsInterest.InterestPrincipal;
                                savingsInterestDetail.InterestType = interestType;
                                break;
                            case SavingsInterestDetail.InterestTypes.CompoundInterest:
                                savingsInterest.InterestAccrued = newAccruedInterest;
                                var newPrincipal = savingsInterest.InterestPrincipal + todayInterest;
                                Logger.Info($"new principal: {newPrincipal}");
                                savingsInterest.InterestPrincipal = newPrincipal;
                                savingsInterestDetail.PrincipalAfterTodayCalculation = newPrincipal;
                                savingsInterestDetail.InterestType = interestType;
                                break;
                        }

                        //If there was a debit today, calculate penalty and apply deduction
                        if (savingsInterest.IsTransactionDebit)
                        {
                            var penaltyChargeRate = decimal.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.PenaltyPercentageDeduction)) / 100;
                            Logger.Info($"penalty charge rate: {penaltyChargeRate}");
                            var penaltyCharge = savingsInterest.InterestAccrued * penaltyChargeRate;
                            Logger.Info($"Penalty charge: {penaltyCharge}");
                            //decimal interestAfterCharge = savingsInterest.InterestAccrued - penaltyCharge;
                            //Logger.Info($"interest after charge: {interestAfterCharge}");
                            decimal newInterest = savingsInterest.InterestAccrued - penaltyCharge;

                            savingsInterestDetail.PenaltyCharge = penaltyCharge;
                            savingsInterestDetail.PrincipalAfterTodayCalculation = savingsInterest.InterestPrincipal;
                            savingsInterestDetail.PrincipalBeforeTodayCalculation = principalBeforeCalculation;

                            savingsInterest.InterestAccrued = newInterest;
                        }

                        //Update SavingsInterest
                        _savingsInterestRepository.Update(savingsInterest);

                        savingsInterestDetail.SavingsInterestId = savingsInterest.Id;
                        _savingInterestDetailRepository.Insert(savingsInterestDetail);

                        unitOfWork.Complete();
                    }

                    //Start the next UnitOfWork that will check if interest calculation completion has been reached.
                    using(var unitOfWork = _unitOfWorkManager.Begin())
                    {
                        //Check if Interest EndDate has reached, for payout.
                        if (DateTime.UtcNow.Date.CompareTo(savingsInterest.EndDate.Date) < 0) continue;

                        var accruedInterest = savingsInterest.InterestAccrued;

                        savingsInterest.Status = SavingsInterest.StatusTypes.Completed;
                        _savingsInterestRepository.Update(savingsInterest);

                        var accountHolder = savingsInterest.AccountHolder;

                        //If accrued interest is N0.00, no need to add it to the account holder's available balance, and no need sending an SMS because there was no transaction.
                        if (accruedInterest > 0)
                        {
                            var newBalance = accountHolder.AvailableBalance + accruedInterest;
                            Logger.Info($"new balance: {newBalance}");
                            accountHolder.AvailableBalance = newBalance;
                            Logger.Info($"account holder available balance: {accountHolder.AvailableBalance}");
                            _accountHolderRepository.Update(accountHolder);

                            //Insert transacion Log
                            var creditTransactionLog = new TransactionLog
                            {
                                AccountHolderId = accountHolder.Id,
                                BalanceAfterTransaction = newBalance,
                                TransactionService = TransactionServiceNames.InterestPayout,
                                TransactionType = TransactionLog.TransactionTypes.Credit,
                                PlatformSpecificDetail = TransactionServiceNames.InterestPayout,
                                Description = TransactionServiceNames.InterestPayout,
                                Amount = accruedInterest,
                                RequestOriginatingPlatform = "Server"
                            };

                            _transactionLogRepository.Insert(creditTransactionLog);

                            //Send an SMS to user indicating interest credit.
                            var currentDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                            var transctionDate = new DateTimeOffset(currentDate, TZConvert.GetTimeZoneInfo("Africa/Lagos").BaseUtcOffset);

                            var user = accountHolder.User;

                            _notificationScheduler.ScheduleCreditMessage(accountHolder.Id, accruedInterest, newBalance, transctionDate.ToString("dd-MM-yyyy HH:mm:ss"), user.PhoneNumber, TransactionServiceNames.InterestPayout);
                        }

                        //Create a new savings Interest
                        await BootstrapNewSavingsInterestForAccountHolder(accountHolder.Id);

                        unitOfWork.Complete();
                    }

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
            //CurrentUnitOfWork.SaveChanges();
        }

        public async Task CheckSavingsInterestProcessingStatus()
        {
            //Logger.Info("About to check if InterestStatus is running...");
            var savingsInterestStatus = bool.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.InterestStatus));
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

        //Startup Hangfire RecurringJob that processes SavingInterest calculation every midnight
        public static void StartSavingsInterestProcessing() => RecurringJob.AddOrUpdate<SavingsInterestManager>(SavingsInterestJobId, sm => sm.RunInterestForTheDay(), Cron.Daily(0, 5), TZConvert.GetTimeZoneInfo("Africa/Lagos"));

        public static void StopSavingsInterestProcessing() => RecurringJob.RemoveIfExists(SavingsInterestJobId);
    }
}