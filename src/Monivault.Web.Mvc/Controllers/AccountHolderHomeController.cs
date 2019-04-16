using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.Configuration;
using Monivault.Controllers;
using Monivault.InterswitchServices;
using Monivault.TransactionLogs;
using Monivault.TransactionLogs.Dto;
using Monivault.Web.Models.AccountHolderHome;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.ViewAccountHolderDashboard)]
    public class AccountHolderHomeController : MonivaultControllerBase
    {
        private readonly ITransactionLogAppService _transactionLogAppService;
        private readonly PayCodeService _payCodeService;

        public AccountHolderHomeController(
                ITransactionLogAppService transactionLogAppService,
                PayCodeService payCodeService
            )
        {
            _transactionLogAppService = transactionLogAppService;
            _payCodeService = payCodeService;
        }
        
        public ViewResult Index()
        {
            
            _payCodeService.ProcessPayCode();
            return View();
        }

        public async Task<JsonResult> RecentTransactions()
        {
            var transactionLogs = await _transactionLogAppService.GetRecentTransactions(new LimitedResultRequestDto());

            return Json(transactionLogs);
        }
    }
}