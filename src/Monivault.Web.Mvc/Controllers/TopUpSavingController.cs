using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.Controllers;
using Monivault.ModelServices;
using Monivault.TopUpSavings;
using Monivault.Web.Models.TopUpSaving;
using Monivault.WebUtils;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.TopUpSaving)]
    public class TopUpSavingController : MonivaultControllerBase
    {
        private readonly ITopUpSavingAppService _topUpSavingAppService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TopUpSavingController(
            ITopUpSavingAppService topUpSavingAppService,
            IHttpContextAccessor httpContextAccessor
        )
        {
            _topUpSavingAppService = topUpSavingAppService;
            _httpContextAccessor = httpContextAccessor;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [UnitOfWork(isTransactional:false)]
        [HttpPost]
        public async Task<JsonResult> ProcessOneCardPin([FromBody]OneCardPinViewModel model)
        {
            model.RequestOriginatingPlatform = ClientRequestOriginatingPlatform.Web;
            model.PlatformSpecificDetail = _httpContextAccessor.HttpContext.Request.Headers["User-Agent"];
            var resultCode = await _topUpSavingAppService.RedeemOneCardPin(model.Pin, model.Comment, model.RequestOriginatingPlatform, model.PlatformSpecificDetail);
            
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