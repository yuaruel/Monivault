using System;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.ServiceModel.Channels;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monivault.AppModels;
using Monivault.EstelOneCardServicesService;
using Monivault.Utils;
using ILogger = Castle.Core.Logging.ILogger;

namespace Monivault.ModelServices
{
    public class TopUpSavingService : ITransientDependency
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<TransactionLog, long> _transactionLogRepository;
        private readonly IRepository<OneCardPinRedeemLog, long> _pinRedeemLogRepository;
        public IAbpSession AbpSession { get; set; }
        public ILogger Logger { set; get; }

        public TopUpSavingService(
                IConfiguration configuration,
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<TransactionLog, long> transactionLogRepository,
                IRepository<OneCardPinRedeemLog, long> pinRedeemLogRepository
            )
        {
            _configuration = configuration;
            _accountHolderRepository = accountHolderRepository;
            _transactionLogRepository = transactionLogRepository;
            _pinRedeemLogRepository = pinRedeemLogRepository;
            AbpSession = NullAbpSession.Instance;
            Logger = NullLogger.Instance;
        }

        public void RedeemOneCardPin(string pinno, string comment, string requestPlatform, string platformSpecific)
        {
            try
            {
                var accountHolder = _accountHolderRepository.Single(p => p.UserId == AbpSession.UserId);

                var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
                //var customBinding = new CustomBinding(binding);
                
                var factoryHandlerBehavior = new HttpMessageHandlerBehavior();
                
                factoryHandlerBehavior.OnSending = (message, token) => {
                    message.Headers.Add("SOAPAction", "");

                    return null;
                };
                
                //binding.CreateBindingElements().Add();
                var endpointAddress = new EndpointAddress("http://180.179.201.98/EstelOneCardServices/services");
                var factory = new ChannelFactory<EstelOneCardServices>(binding, endpointAddress);
                factory.Endpoint.EndpointBehaviors.Add(factoryHandlerBehavior);
                var client = factory.CreateChannel();
                
                
                //var oneCardServiceClient = new EstelOneCardServicesClient(binding, endpointAddress);
               
                
                var config = _configuration.GetSection("OneCardProperties");

                var agentTransactionId = RandomStringGeneratorUtil.GenerateAgentTransactionId();
                Logger.Info("Pin no: " + pinno);
                var pinRedeemRequest = new PinRedeemRequest
                {
                    //pin = "APEX_PINRDM",
                    pin = "TPR_AAL_1",
                    //agentcode = "7F1359753577B274D717DC2E41BA1E51",
                    agentcode = "38EEC49BBE3E155E8E3DCF7FBAB6B6D2",
                    pinno = pinno,
                    agenttransid = agentTransactionId,
                    serviceid = PinRedeemServiceIds.SavingsTopUp,
                    comments = "just do it"
                };

                var pinRedeemResponse = client.getPinRedeem(pinRedeemRequest);
                
                //Log OneCardPinRedeem. Whether successful or not
                var pinRedeemLog = new OneCardPinRedeemLog();
                pinRedeemLog.Amount = decimal.Parse(pinRedeemResponse.amount);
                pinRedeemLog.AccountHolder = accountHolder;
                pinRedeemLog.Comments = comment;
                pinRedeemLog.PinNo = pinno;
                pinRedeemLog.SerialNo = pinRedeemResponse.serialno;
                pinRedeemLog.ServiceId = PinRedeemServiceIds.SavingsTopUp;
                pinRedeemLog.VendorCode = pinRedeemResponse.vendorcode;
                pinRedeemLog.ProductCode = pinRedeemResponse.productcode;
                pinRedeemLog.AgentTransactionId = agentTransactionId;

                pinRedeemLog = _pinRedeemLogRepository.Insert(pinRedeemLog);

                switch (pinRedeemResponse.resultcode)
                {
                    case "0":

                        var pinAmount = decimal.Parse(pinRedeemResponse.amount);
                        var currentBalance = accountHolder.AvailableBalance + pinAmount;

                        //Update AccountHolders balance.
                        accountHolder.AvailableBalance = currentBalance;
                        _accountHolderRepository.Update(accountHolder);
                        
                        //Log transaction
                        var transactionLog = new TransactionLog();
                        transactionLog.Amount = pinAmount;
                        transactionLog.AccountHolder = accountHolder;
                        transactionLog.TransactionType = TransactionLog.TransactionTypes.Credit;
                        transactionLog.TransactionService = TransactionServiceNames.OneCardPinRedeem;
                        transactionLog.RequestOriginatingPlatform = requestPlatform;
                        transactionLog.PlatformSpecificDetail = platformSpecific;
                        transactionLog.Description = comment;

                        transactionLog = _transactionLogRepository.Insert(transactionLog);
                        
                        //Update OneCardPinRedeemLog with the successful transaction.
                        pinRedeemLog.TransactionLog = transactionLog;
                        _pinRedeemLogRepository.Update(pinRedeemLog);
                        
                        //Send Sms Receipt
                        //Send Email Receipt.

                        break;
                    
                    case "60":
                    case "63":
                    case "115":
                        throw new UserFriendlyException("Invalid Pin No");
                       
                    default:
                        throw new UserFriendlyException("One card system error. Try again later!");
                }
            }
            catch (InvalidOperationException ioExc)
            {
                //An exception needs to be thrown to notify the user, because the user is not an account holder.
                Logger.Error(ioExc.StackTrace);
            }
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