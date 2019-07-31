using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Estel;
using Microsoft.Extensions.Configuration;
using Monivault.AppModels;
using Monivault.Configuration;
using Monivault.Exceptions;
using Monivault.MoneyTransfers.Dto;
using Monivault.Utils;

namespace Monivault.MoneyTransfers
{
    public class MoneyTransferAppService : MonivaultAppServiceBase, IMoneyTransferAppService
    {
        private readonly IRepository<OtpSession> _otpRepository;
        private readonly IConfiguration _configuration;
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<OneCardFundsTransferLog, long> _fundsTransferLogRepository;
        private readonly IRepository<TransactionLog, long> _transactionLogRepository;
        private readonly IRepository<Bank> _bankRepository;

        public MoneyTransferAppService(
                IRepository<OtpSession> otpRepository,
                IConfiguration configuration,
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<OneCardFundsTransferLog, long> fundsTransferLogRepository,
                IRepository<TransactionLog, long> transactionLogRepository,
                IRepository<Bank> bankRepository
            )
        {
            _otpRepository = otpRepository;
            _configuration = configuration;
            _accountHolderRepository = accountHolderRepository;
            _fundsTransferLogRepository = fundsTransferLogRepository;
            _transactionLogRepository = transactionLogRepository;
            _bankRepository = bankRepository;
        }
        
        public async Task<string> GenerateBankAccountTransferOtp(decimal amount, string comment, string phoneNumber)
        {
            var activityProperty = new Dictionary<string, string>();
            activityProperty.Add("Amount", amount.ToString());
            activityProperty.Add("Comment", comment);

            var otp = RandomStringGeneratorUtil.GenerateOtp();
            
            var otpSession = new OtpSession
            {
                User = await GetCurrentUserAsync(),
                Id = otp,
                ActionProperty = activityProperty,
                PhoneNumberSentTo = phoneNumber
            };

            _otpRepository.Insert(otpSession);

            return otp.ToString();
        }

        public async Task TransferMoneyToAccountHolderBankAccount(TransferMoneyToAccountInput input)
        {
            var user = await GetCurrentUserAsync();

            var accountHolder = await _accountHolderRepository.SingleAsync(p => p.UserId == user.Id);
            var estelClient = new EstelServicesClient();

            var agentTransactionId = RandomStringGeneratorUtil.GenerateAgentTransactionId();
            var bank = await  _bankRepository.SingleAsync(p => p.Id == accountHolder.BankId.Value);
            var bankCode = bank.OneCardBankCode;

            var fundsTransferRequest = new FundsTransferRequest
            {
                amount = input.Amount.ToString(),
                agentCode = _configuration.GetSection("OneCardProperties").GetValue<string>("AgentCode"),
                mpin = _configuration.GetSection("OneCardProperties").GetValue<string>("AgentPin"),
                destination = accountHolder.BankAccountNumber,
                mobilenumber = user.PhoneNumber,
                productCode = bank.OneCardBankCode,
                agenttransid = agentTransactionId,
                action = "NF"
            };

            var fundsTransferResponseReturn = await estelClient.getFundsTransferAsync(fundsTransferRequest);

            var fundsTransferResponse = fundsTransferResponseReturn.Body.getFundsTransferReturn;

            //ToDo Log OneCardFundsTransfer
            var oneCardFundsTransferLog = new OneCardFundsTransferLog
            {
                OneCardFundsTransferLogKey = Guid.NewGuid(),
                AccountNumber = accountHolder.BankAccountNumber,
                Action = "NF",
                AccountHolderId = accountHolder.Id,
                AgentTransactionId = agentTransactionId,
                BankCode = bank.OneCardBankCode,
                BankId = bank.Id,
                ResultCode = fundsTransferResponse.resultcode,
                ResultDescription = fundsTransferResponse.resultdescription,
                Responsects = fundsTransferResponse.responsects,
                ResponseValue = fundsTransferResponse.responseValue
            };

            //Watch this statement and see if it will update the entity object when inserted. (Temporary).
            _fundsTransferLogRepository.Insert(oneCardFundsTransferLog);
            switch (fundsTransferResponse.resultcode)
            {
                case "0":
                    var currentBalance = accountHolder.AvailableBalance;
                    using(var unitOfWork = UnitOfWorkManager.Begin()) {
                        var transferCharge = decimal.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.WithdrawalServiceCharge));
                        accountHolder.AvailableBalance -= (input.Amount + transferCharge);

                        unitOfWork.Complete();
                    }

                    var transactionLog = new TransactionLog
                    {
                        AccountHolderId = accountHolder.Id,
                        Amount = input.Amount,
                        BalanceAfterTransaction = accountHolder.AvailableBalance,
                        TransactionKey = Guid.NewGuid(),
                        TransactionType = TransactionLog.TransactionTypes.Debit,
                        TransactionService = TransactionLog.TransactionServices.OneCardFundsTransferService,
                        RequestOriginatingPlatform = input.RequestOrigintingPlatform,
                        PlatformSpecificDetail = input.PlatformSpecificDetail
                    };

                    var transactionLogId = await _transactionLogRepository.InsertAndGetIdAsync(transactionLog);
                    oneCardFundsTransferLog.TransactionLogId = transactionLogId;

                    break;
                
                default:
                    
                    throw new MoneyTransferException("Error completing request");
            }
        }
    }
}