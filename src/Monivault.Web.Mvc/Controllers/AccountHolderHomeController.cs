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
            var timeZoneInfos = TimeZoneInfo.GetSystemTimeZones();
            Logger.Info($"Time zone count: {timeZoneInfos.Count}");
            foreach (var timeZoneInfo in timeZoneInfos)
            {
                Logger.Info($"TimeZoneInfo: {timeZoneInfo.DaylightName}");
                Logger.Info($"Standard timeZone Name: {timeZoneInfo.StandardName}");
            }
            return View();
        }

        public async Task<JsonResult> RecentTransactions()
        {
            var transactionLogs = await _transactionLogAppService.GetRecentTransactions(new LimitedResultRequestDto());

            return Json(transactionLogs);
        }
    }
}