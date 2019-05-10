using Estel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.TopUpAirtime
{
    public class BuyAirtimeAppService : MonivaultAppServiceBase, IBuyAirtimeAppService
    {
        public void BuyAirtime()
        {
            var estelClient = new EstelServicesClient();

            var balanceRequest = new BalanceRequest();
            balanceRequest.agentCode = "TPR_AAL_1";
            balanceRequest.mpin = "14287490BC5A9662D60DFCD3333F723B";

            var topupRequest = new TopupRequest();
            topupRequest.agentCode = "TPR_AAL_1";
            topupRequest.mpin = "14287490BC5A9662D60DFCD3333F723B";
            topupRequest.destination = "08032808912";
            topupRequest.mobilenumber = "08032808912";
            topupRequest.amount = "200";
            topupRequest.agenttransid = "34634733654774334";
            topupRequest.productCode = "MTN";
            topupRequest.type = "TOPUP";

            var fundsTransferRequest = new FundsTransferRequest();
            fundsTransferRequest.agentCode = "TPR_AAL_1";
            fundsTransferRequest.mpin = "14287490BC5A9662D60DFCD3333F723B";
            fundsTransferRequest.amount = "1000";
            fundsTransferRequest.destination = "9201876211";
            fundsTransferRequest.agenttransid = "34634733654774334";
            fundsTransferRequest.mobilenumber = "08032808912";
            fundsTransferRequest.productCode = "SIBTC";

            Logger.Info("about to do money trasnfer...");
            //var fundsTransferResponse = await estelClient.getFundsTransferAsync(fundsTransferRequest);
            //Logger.Info($"fundstranfer response: {JsonConvert.SerializeObject(fundsTransferResponse.Body)}");
        }
    }
}
