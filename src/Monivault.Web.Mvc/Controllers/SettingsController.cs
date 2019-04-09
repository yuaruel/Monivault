using System.Threading.Tasks;
using Abp.Configuration;
using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    public class SettingsController : MonivaultControllerBase
    {
        // GET
        public async Task<IActionResult> Index()
        {
            await SettingManager.ChangeSettingForApplicationAsync("StopWithdrawal", "true");
            return View();
        }

        public JsonResult UpdateGeneralSettings()
        {
            return Json(new { });
        }

        public JsonResult UpdateWithdrawalSettings()
        {
            return Json(new { });
        }

        public JsonResult UpdateInterestSettings()
        {
            return Json(new { });
        }
    }
}