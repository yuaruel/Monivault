using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Monivault.AccountHolders;
using Monivault.DashboardService;
using System.Threading.Tasks;

namespace Monivault.Controllers
{
    public class AccountHolderDashboardServiceController : MonivaultControllerBase
    {
        private readonly AccountHolderDashboardService _accountHolderDashboardService;
        private readonly IAccountHolderAppService _accountHolderAppService;

        public AccountHolderDashboardServiceController(
            AccountHolderDashboardService accountHolderDashboardService,
            IAccountHolderAppService accountHolderAppService
        )
        {
            _accountHolderDashboardService = accountHolderDashboardService;
            _accountHolderAppService = accountHolderAppService;
        }
        
        // GET
        
        public JsonResult AvailableBalance()
        {
            var availableBalance = _accountHolderDashboardService.GetUserAvailableBalance();

            return Json(new { AvailableBalance = availableBalance});
        }

        public async Task<JsonResult> AvailableBalanceAndReceivedInterest()
        {
            var receivedInterest = await _accountHolderAppService.GetInterestReceivedForCurrentYear();
            var availableBalance = _accountHolderDashboardService.GetUserAvailableBalance();

            return Json(new { RecievedInterest = receivedInterest, AvailableBalance = availableBalance });
        }
    }
}