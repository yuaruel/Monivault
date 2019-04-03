using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.Controllers;
using Monivault.ModelServices;
using Monivault.TopUpSavings;
using Monivault.Web.Models.TopUpSaving;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.TopUpSaving)]
    public class TopUpSavingController : MonivaultControllerBase
    {
        private readonly ITopUpSavingAppService _topUpSavingAppService;

        public TopUpSavingController(
            ITopUpSavingAppService topUpSavingAppService
        )
        {
            _topUpSavingAppService = topUpSavingAppService;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<JsonResult> ProcessOneCardPin([FromBody]OneCardPinViewModel model)
        {
            await _topUpSavingAppService.RedeemOneCardPin(model.Pin, model.Comment, model.RequestOriginatingPlatform, model.PlatformSpecificDetail);
            
            return Json(new {});
        }
    }
}