using Microsoft.AspNetCore.Mvc;
using Monivault.AccountHolders;
using Monivault.Controllers;
using Monivault.TransactionLogs;

namespace Monivault.Web.Controllers
{
    public class AdminHomeController : MonivaultControllerBase
    {
        private readonly IAccountHolderAppService _accountHolderAppService;
        private readonly ITransactionLogAppService _transactionLogAppService;

        public AdminHomeController(
                IAccountHolderAppService accountHolderAppService,
                ITransactionLogAppService transactionLogAppService
            )
        {
            _accountHolderAppService = accountHolderAppService;
            _transactionLogAppService = transactionLogAppService;
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

        public JsonResult TotalCreditAndDebit()
        {
            var (totalCredit, totalDebit) = _transactionLogAppService.GetTotalCreditAndDebit();

            return Json(new { totalCredit, totalDebit });
        }
    }
}