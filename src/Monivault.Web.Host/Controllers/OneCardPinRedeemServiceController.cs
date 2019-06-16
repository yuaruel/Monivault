using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;
using Monivault.Models;
using Monivault.TopUpSavings;
using Monivault.WebUtils;

namespace Monivault.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class OneCardPinRedeemServiceController : MonivaultControllerBase
    {
        private readonly ITopUpSavingAppService _topUpSavingAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public OneCardPinRedeemServiceController(
                ITopUpSavingAppService topUpSavingAppService,
                IHttpContextAccessor httpContextAccessor
            )
        {
            _topUpSavingAppService = topUpSavingAppService;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<JsonResult> RedeemPin([FromBody] OneCardPinViewModel model)
        {
            Logger.Info("Got into the controller to redeem pin");
            Logger.Info($"Pin: {model.Pin}");
            var resultCode = await _topUpSavingAppService.RedeemOneCardPin(model.Pin, model.Comment, model.RequestOriginatingPlatform, model.PlatformSpecificDetail);
            Logger.Info($"result code: {resultCode}");
            switch (resultCode)
            {
                case "0":

                    return Json(new { });

                case "60":
                case "62":
                case "63":
                case "115":
                    Logger.Info("this is result code: " + resultCode);
                    throw new UserFriendlyException("Invalid PIN");

                default:
                    throw new UserFriendlyException("One card internal server error!");
            }
        }
    }
}