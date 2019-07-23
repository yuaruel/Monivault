using Abp.Domain.Repositories;
using Abp.UI;
using Estel;
using Microsoft.Extensions.Configuration;
using Monivault.AppModels;
using Monivault.BuyAirtime.Dto;
using Monivault.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monivault.TopUpAirtime
{
    public class BuyAirtimeAppService : MonivaultAppServiceBase, IBuyAirtimeAppService
    {
        private readonly IConfiguration _configuration;
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<TransactionLog, long> _transactionLogRepository;

        public BuyAirtimeAppService(
                IConfiguration configuration,
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<TransactionLog, long> transactionLogRepository
            )
        {
            _configuration = configuration;
            _accountHolderRepository = accountHolderRepository;
            _transactionLogRepository = transactionLogRepository;
        }

        public async Task BuyAirtime(AirtimePurchaseDto input)
        {
            try
            {
                //Check if account holder has enough balance to buy airtime.
                var user = await GetCurrentUserAsync();

                var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);
                if (accountHolder.AvailableBalance < input.Amount) throw new UserFriendlyException("Insufficient balance");

                var estelClient = new EstelServicesClient();

                var topupRequest = new TopupRequest
                {
                    agentCode = _configuration.GetSection("OneCardProperties").GetValue<string>("AgentCode"),// "TPR_AAL_1";
                    mpin = _configuration.GetSection("OneCardProperties").GetValue<string>("AgentPin"),
                    destination = input.PhoneNumber,
                    mobilenumber = input.PhoneNumber,
                    amount = input.Amount.ToString(),
                    agenttransid = RandomStringGeneratorUtil.GenerateAgentTransactionId(),
                    productCode = input.AirtimeNetwork,
                    type = "TOPUP"
                };

                var topupResponseAsync = await estelClient.getTopupAsync(topupRequest);
                var topupResponse = topupResponseAsync.Body.getTopupReturn;
                
                switch (int.Parse(topupResponse.resultcode))
                {
                    case 0:
                        //I used UnitOfWorkManager to update the account holder's available balance, since airtime has been processed successfully.
                        //This is in case there is an error during transaction log, causing a roll back including the available balance update. This means the account holder's
                        //available balance wont be updated, and account holder gets a free airtime. So quickly update account holders available balance.

                        accountHolder.AvailableBalance -= input.Amount;
                        CurrentUnitOfWork.SaveChanges();

                        //Send SMS notification.

                        //Send Email notification

                        var transactionLog = new TransactionLog
                        {
                            AccountHolderId = accountHolder.Id,
                            Amount = input.Amount,
                            BalanceAfterTransaction = accountHolder.AvailableBalance,
                            TransactionType = TransactionLog.TransactionTypes.Debit,
                            TransactionService = TransactionServiceNames.OneCardAirtimeTopUp,
                            RequestOriginatingPlatform = input.RequestOriginatingPlatform,
                            PlatformSpecificDetail = string.Empty,

                            //TODO Get the network name and add it to the description
                            Description = "Airtime purchase"
                        };

                        _transactionLogRepository.Insert(transactionLog);
                        break;

                    default:
                        throw new UserFriendlyException("Unable to complete your transaction. Please try again.");
                }
                

            }
            catch (InvalidOperationException exc)
            {
                Logger.Error(exc.StackTrace);
                throw new UserFriendlyException("Unable to complete your transaction. Please contact administrator");
            }catch(Exception exc)
            {
                Logger.Error(exc.StackTrace);
                throw new UserFriendlyException("Unable to complete your transaction.");
            }
        }
    }
}
