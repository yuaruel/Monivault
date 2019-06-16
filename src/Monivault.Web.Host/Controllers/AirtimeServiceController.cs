using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monivault.BuyAirtime;
using Monivault.BuyAirtime.Dto;
using Monivault.Controllers;
using Monivault.TopUpAirtime;
using Monivault.Web.Host.Model;

namespace Monivault.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AirtimeServiceController : MonivaultControllerBase
    {
        private readonly IBuyAirtimeAppService _airtimePurchaseApiAppService;

        public AirtimeServiceController(
                IBuyAirtimeAppService airtimePurchaseApiAppService
            )
        {
            _airtimePurchaseApiAppService = airtimePurchaseApiAppService;
        }

        [HttpPost]
        public async Task<JsonResult> PurchaseAirtime([FromBody] PurchaseAirtimeModel model)
        {
            Logger.Info("About to process airtime purchase...");
            Logger.Info($"Phone Number: {model.PhoneNumber}");
            Logger.Info($"Airtime Network: {model.ProductCode}");
            Logger.Info($"Airtime Value: {model.AirtimeValue}");
            var inputDto = new AirtimePurchaseDto
            {
                AirtimeNetwork = model.ProductCode,
                Amount = model.AirtimeValue,
                PhoneNumber = model.PhoneNumber,
                RequestOriginatingPlatform = model.RequestOriginatingPlatform
            };

            await _airtimePurchaseApiAppService.BuyAirtime(inputDto);

            return Json(new { });
        }
    }
}