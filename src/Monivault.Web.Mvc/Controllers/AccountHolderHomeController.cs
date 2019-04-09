using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.Controllers;
using Monivault.TransactionLogs;
using Monivault.TransactionLogs.Dto;
using Monivault.Web.Models.AccountHolderHome;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.ViewAccountHolderDashboard)]
    public class AccountHolderHomeController : MonivaultControllerBase
    {
        private readonly ITransactionLogAppService _transactionLogAppService;

        public AccountHolderHomeController(
                ITransactionLogAppService transactionLogAppService
            )
        {
            _transactionLogAppService = transactionLogAppService;
        }
        
        public ViewResult Index()
        {
            return View();
        }

        public async Task<JsonResult> RecentTransactions()
        {
            var transactionLogs = await _transactionLogAppService.GetRecentTransactions(new LimitedResultRequestDto());

            return Json(transactionLogs);
        }
    }
}