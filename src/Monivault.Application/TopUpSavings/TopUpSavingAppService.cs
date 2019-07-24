using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Monivault.AppModels;
using Monivault.BackgroundJobs.Sms;
using Monivault.ModelServices;
//using Monivault.EstelOneCardServicesService;
using Monivault.Utils;
using ServiceReference;
using TimeZoneConverter;

namespace Monivault.TopUpSavings
{
    public class TopUpSavingAppService : MonivaultAppServiceBase, ITopUpSavingAppService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<TransactionLog, long> _transactionLogRepository;
        private readonly IRepository<OneCardPinRedeemLog, long> _pinRedeemLogRepository;
        private readonly NotificationScheduler _notificationScheduler;
        
        public TopUpSavingAppService(
            IConfiguration configuration,
            IRepository<AccountHolder> accountHolderRepository,
            IRepository<TransactionLog, long> transactionLogRepository,
            IRepository<OneCardPinRedeemLog, long> pinRedeemLogRepository,
            NotificationScheduler notificationScheduler
        )
        {
            _configuration = configuration;
            _accountHolderRepository = accountHolderRepository;
            _transactionLogRepository = transactionLogRepository;
            _pinRedeemLogRepository = pinRedeemLogRepository;
            _notificationScheduler = notificationScheduler;
            AbpSession = NullAbpSession.Instance;
        }
        
        public async Task<string> RedeemOneCardPin(string pinno, string comment, string requestPlatform, string platformSpecific)
        {
            var user = await GetCurrentUserAsync();

            var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);

            var oneCardServiceClient = new EstelOneCardServicesClient();

            var config = _configuration.GetSection("OneCardProperties");

            var agentTransactionId = RandomStringGeneratorUtil.GenerateAgentTransactionId();

            var pinRedeemRequest = new PinRedeemRequest
            {
                pin = config.GetValue<string>("AgentPin"),
                agentcode = config.GetValue<string>("AgentCode"),
                pinno = pinno,
                agenttransid = agentTransactionId,
                serviceid = PinRedeemServiceIds.SavingsTopUp,
                comments = comment
            };

            var pinRedeemResponse = await oneCardServiceClient.getPinRedeemAsync(pinRedeemRequest);

            //Check if the PinRedeem service call was successful, and update account holder alance.

            var pinRedeemLog = new OneCardPinRedeemLog();

            try
            {
                //Log OneCardPinRedeem. Whether successful or not
                
                pinRedeemLog.Amount = decimal.Parse(string.IsNullOrEmpty(pinRedeemResponse.amount) ? decimal.Zero.ToString() : pinRedeemResponse.amount);
                pinRedeemLog.AccountHolder = accountHolder;
                pinRedeemLog.Comments = comment;
                pinRedeemLog.PinNo = pinno;
                pinRedeemLog.SerialNo = pinRedeemResponse.serialno;
                pinRedeemLog.ServiceId = PinRedeemServiceIds.SavingsTopUp;
                pinRedeemLog.VendorCode = pinRedeemResponse.vendorcode;
                pinRedeemLog.ProductCode = pinRedeemResponse.productcode;
                pinRedeemLog.AgentTransactionId = agentTransactionId;
                pinRedeemLog.ResultCode = pinRedeemResponse.resultcode;
                pinRedeemLog.ResultDescription = pinRedeemResponse.resultdescription;

                pinRedeemLog = _pinRedeemLogRepository.Insert(pinRedeemLog);
                CurrentUnitOfWork.SaveChanges();
            }
            catch(Exception exc)
            {
                Logger.Error($"Error in log entity: {exc.StackTrace}");
                //Send an error log email to my inbox. For Monitoring.
            }

            if ("0".Trim().Equals(pinRedeemResponse.resultcode)){

                var pinAmount = decimal.Parse(pinRedeemResponse.amount);
                var currentBalance = accountHolder.AvailableBalance + pinAmount;

                try
                {
                    //Update AccountHolders balance.
                    accountHolder.AvailableBalance = currentBalance;
                    _accountHolderRepository.Update(accountHolder);
                    CurrentUnitOfWork.SaveChanges();

                    //SendSms.
                    var currentDate = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Unspecified);
                    var transctionDate = new DateTimeOffset(currentDate, TZConvert.GetTimeZoneInfo("Africa/Lagos").BaseUtcOffset);
                    _notificationScheduler.SchedulePinRedeemMessage(accountHolder.Id, pinAmount, transctionDate.ToString("dd-MM-yyyy HH:mm:ss"), user.PhoneNumber);

                    //Log transaction
                    var transactionLog = new TransactionLog();
                    transactionLog.Amount = pinAmount;
                    transactionLog.BalanceAfterTransaction = currentBalance;
                    transactionLog.AccountHolder = accountHolder;
                    transactionLog.TransactionType = TransactionLog.TransactionTypes.Credit;
                    transactionLog.TransactionService = TransactionServiceNames.OneCardPinRedeem;
                    transactionLog.RequestOriginatingPlatform = requestPlatform;
                    transactionLog.PlatformSpecificDetail = platformSpecific;
                    transactionLog.Description = comment ?? string.Empty;

                    transactionLog = _transactionLogRepository.Insert(transactionLog);

                    //Update OneCardPinRedeemLog with the successful transaction.
                    pinRedeemLog.TransactionLog = transactionLog;
                    _pinRedeemLogRepository.Update(pinRedeemLog);

                    
                }
                catch (Exception exc)
                {
                    Logger.Error(exc.StackTrace);
                }

                //Send Sms Receipt
                //Send Email Receipt.
            }

            return pinRedeemResponse.resultcode;


        }
        
        public void LogPinRedeemTransaction()
        {
            
        }
    }
    
    public class PinRedeemServiceIds
    {
        public const string SavingsTopUp = "Savings Top Up";
    }
}