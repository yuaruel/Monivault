using Estel;
using Microsoft.Extensions.Configuration;
using Monivault.BuyAirtime.Dto;
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

        public BuyAirtimeAppService(
                IConfiguration configuration
            )
        {
            _configuration = configuration;
        }

        public async Task BuyAirtime(AirtimePurchaseDto input)
        {
            var estelClient = new EstelServicesClient();

            //var balanceRequest = new BalanceRequest();
            //balanceRequest.agentCode = "TPR_AAL_1";
            //balanceRequest.mpin = "14287490BC5A9662D60DFCD3333F723B";

            var topupRequest = new TopupRequest();
            topupRequest.agentCode = _configuration.GetSection("OneCardProperties").GetValue<string>("AgentCode");// "TPR_AAL_1";
            topupRequest.mpin = _configuration.GetSection("OneCardProperties").GetValue<string>("AgentPin");
            topupRequest.destination = input.PhoneNumber;
            topupRequest.mobilenumber = input.PhoneNumber;
            topupRequest.amount = input.Amount.ToString();
            topupRequest.agenttransid = "34634733654774334";
            topupRequest.productCode = input.AirtimeNetwork;
            topupRequest.type = "TOPUP";

            var topupResponseAsync = await estelClient.getTopupAsync(topupRequest);
            var topupResponse = topupResponseAsync.Body.getTopupReturn;

            Logger.Info($"Topup response result code: {topupResponse.resultcode}");
            Logger.Info($"Result description: {topupResponse.resultdescription}");
            //var fundsTransferRequest = new FundsTransferRequest();
            //fundsTransferRequest.agentCode = "TPR_AAL_1";
            //fundsTransferRequest.mpin = "14287490BC5A9662D60DFCD3333F723B";
            //fundsTransferRequest.amount = "1000";
            //fundsTransferRequest.destination = "9201876211";
            //fundsTransferRequest.agenttransid = "34634733654774334";
            //fundsTransferRequest.mobilenumber = "08032808912";
            //fundsTransferRequest.productCode = "SIBTC";

            //var fundsTransferResponse = await estelClient.getFundsTransferAsync(fundsTransferRequest);
            //Logger.Info($"fundstranfer response: {JsonConvert.SerializeObject(fundsTransferResponse.Body)}");
        }
    }
}
