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
using Monivault.SavingsInterests;
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
        private readonly SavingsInterestManager _interestManager;

        public AccountHolderHomeController(
                ITransactionLogAppService transactionLogAppService,
                PayCodeService payCodeService,
                SavingsInterestManager interestManager
            )
        {
            _transactionLogAppService = transactionLogAppService;
            _payCodeService = payCodeService;
            _interestManager = interestManager;
        }

        public async Task<ViewResult> Index()
        {
            await _interestManager.RunInterestForTheDay();
            await _payCodeService.ProcessPayCode();
            return View();
        }

        public async Task<JsonResult> RecentTransactions()
        {
            var transactionLogs = await _transactionLogAppService.GetRecentTransactions(new LimitedResultRequestDto());

            return Json(transactionLogs);
        }
    }
}