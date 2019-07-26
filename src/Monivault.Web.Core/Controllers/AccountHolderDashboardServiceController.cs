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

        public JsonResult AvailableBalanceAndReceivedInterest()
        {
            var receivedInterest = _accountHolderAppService.GetInterestAccrued();
            var availableBalance = _accountHolderDashboardService.GetUserAvailableBalance();

            return Json(new { ReceivedInterest = receivedInterest, AvailableBalance = availableBalance });
        }
    }
}