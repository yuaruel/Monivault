﻿using Abp.Runtime.Security;
using Microsoft.AspNetCore.Mvc;
using Monivault.AccountHolders;
using Monivault.AccountHolders.Dto;
using Monivault.Controllers;
using Monivault.DashboardService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountHolderServiceController : MonivaultControllerBase
    {
        private readonly AccountHolderDashboardService _accountHolderDashboardService;
        private readonly IAccountHolderAppService _accountHolderAppService;

        public AccountHolderServiceController(
                AccountHolderDashboardService accountHolderDashboardService,
                IAccountHolderAppService accountHolderAppService
            )
        {
            _accountHolderDashboardService = accountHolderDashboardService;
            _accountHolderAppService = accountHolderAppService;
        }

        [HttpGet]
        public JsonResult AvailableBalance()
        {
            var balance = _accountHolderAppService.GetAccountHolderBalance();

            return Json(new { balance.AvailableBalance, balance.LedgerBalance });
        }

        [HttpGet]
        public JsonResult AccruedInterest()
        {
            var interest = _accountHolderAppService.GetInterestAccrued();

            return Json(new { AccruedInterest = interest });
        }
    }
}