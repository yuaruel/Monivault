using Microsoft.AspNetCore.Mvc;
using Monivault.AccountHolders;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    public class AdminHomeController : MonivaultControllerBase
    {
        private readonly IAccountHolderAppService _accountHolderAppService;

        public AdminHomeController(
                IAccountHolderAppService accountHolderAppService
            )
        {
            _accountHolderAppService = accountHolderAppService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult TotalAccountHolders()
        {
            return Json(new { totalAccountHolders = _accountHolderAppService.GetTotalNumberOfAccountHolders() });
        }
    }
}