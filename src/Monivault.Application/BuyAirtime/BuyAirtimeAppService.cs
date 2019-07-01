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
        private readonly IRepository<OneCardTopupLog, long> _oneCardTopupLogRepository;

        public BuyAirtimeAppService(
                IConfiguration configuration,
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<OneCardTopupLog, long> oneCardTopupLogRepository
            )
        {
            _configuration = configuration;
            _accountHolderRepository = accountHolderRepository;
            _oneCardTopupLogRepository = oneCardTopupLogRepository;
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

                //Log airtime purchase
                var topupLog = new OneCardTopupLog()
                {
                    OneCardTopupLogKey = Guid.NewGuid(),
                    Amount = input.Amount,
                    AgentTransactionId = topupResponse.agenttransid,
                    Destination = topupResponse.destination,
                    MobileNumber = topupResponse.mobilenumber,
                    ProductCode = topupResponse.productcode,
                    AccountHolderId = accountHolder.Id,
                    ResultCode = topupResponse.resultcode,
                    ResultDescription = topupResponse.resultdescription,
                    RequestCts = topupResponse.requestcts,
                    Type = topupResponse.type,
                    ResponseCts = topupResponse.responsects,
                    ResponseValue = topupResponse.responseValue,
                };

                topupLog = _oneCardTopupLogRepository.Insert(topupLog);

                switch (int.Parse(topupResponse.resultcode))
                {
                    case 0:
                        //I used UnitOfWorkManager to update the account holder's available balance, since airtime has been processed successfully.
                        //This is in case there is an error during transaction log, causing a roll back including the available balance update. This means the account holder's
                        //available balance wont be updated, and account holder gets a free airtime. So quickly update account holders available balance.
                        using(var _unitOfWork = UnitOfWorkManager.Begin())
                        {
                            //Subtract the airtime amount from users avaialble balance.
                            accountHolder.AvailableBalance -= input.Amount;

                            _unitOfWork.Complete();
                        }

                        //Log transaction
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
