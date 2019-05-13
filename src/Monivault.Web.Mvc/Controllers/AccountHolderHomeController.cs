using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Configuration;
using Estel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monivault.Authorization;
using Monivault.Configuration;
using Monivault.ConfigurationOptions;
using Monivault.Controllers;
using Monivault.InterswitchServices;
using Monivault.SavingsInterests;
using Monivault.TransactionLogs;
using Monivault.TransactionLogs.Dto;
using Monivault.Web.Models.AccountHolderHome;
using Newtonsoft.Json;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.ViewAccountHolderDashboard)]
    public class AccountHolderHomeController : MonivaultControllerBase
    {
        private readonly ITransactionLogAppService _transactionLogAppService;
        private readonly PayCodeService _payCodeService;
        private readonly SavingsInterestManager _interestManager;
        private readonly IConfiguration _tempConfig;
        //private readonly IOptions<AWSCredentialOptions> _credentialOptions;

        public AccountHolderHomeController(
                ITransactionLogAppService transactionLogAppService,
                PayCodeService payCodeService,
                SavingsInterestManager interestManager,
                IConfiguration tempConfig
                //IOptions<AWSCredentialOptions> credentialOptions
            )
        {
            _transactionLogAppService = transactionLogAppService;
            _payCodeService = payCodeService;
            _interestManager = interestManager;
            _tempConfig = tempConfig;
            //_credentialOptions = credentialOptions;
        }

        public async Task<ViewResult> Index()
        {
            var awsCredentialOptions = _tempConfig.GetSection("AWS").Get<AWSCredentialOptions>();
            Logger.Info($"Fetching from config. The AWS AccessKey: {awsCredentialOptions}");
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