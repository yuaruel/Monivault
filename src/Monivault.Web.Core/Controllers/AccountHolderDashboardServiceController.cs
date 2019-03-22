using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Monivault.DashboardService;

namespace Monivault.Controllers
{
    public class AccountHolderDashboardServiceController : MonivaultControllerBase
    {
        private readonly AccountHolderDashboardService _accountHolderDashboardService;

        public AccountHolderDashboardServiceController(
            AccountHolderDashboardService accountHolderDashboardService
        )
        {
            _accountHolderDashboardService = accountHolderDashboardService;
        }
        
        // GET
        
        public JsonResult AvailableBalance()
        {
            var availableBalance = _accountHolderDashboardService.GetUserAvailableBalance();

            return Json(new { AvailableBalance = availableBalance});
        }
    }
}