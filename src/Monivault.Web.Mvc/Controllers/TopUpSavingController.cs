using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.Controllers;
using Monivault.ModelServices;
using Monivault.Web.Models.TopUpSaving;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.TopUpSaving)]
    public class TopUpSavingController : MonivaultControllerBase
    {
        private readonly TopUpSavingService _topUpSavingService;

        public TopUpSavingController(
            TopUpSavingService topUpSavingService
        )
        {
            _topUpSavingService = topUpSavingService;
        }
        
        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ProcessOneCardPin([FromBody]OneCardPinViewModel model)
        {
            _topUpSavingService.RedeemOneCardPin(model.Pin, model.Comment, model.RequestOriginatingPlatform, model.PlatformSpecificDetail);
            return StatusCode(200);
        }
    }
}