using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;
using Monivault.TransactionLogs;

namespace Monivault.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TransactionServiceController : MonivaultControllerBase
    {
        private readonly ITransactionLogAppService _transactionLogAppService;

        public TransactionServiceController(ITransactionLogAppService transactionLogAppService)
        {
            _transactionLogAppService = transactionLogAppService;
        }

        [HttpGet]
        public async Task<IActionResult> RecentTransactions()
        {
            var transactionLogs = await _transactionLogAppService.GetRecentTransactions(new LimitedResultRequestDto());
            var transactionList = new List<object>();

            foreach(var transactionLog in transactionLogs)
            {
                transactionList.Add(new
                {
                    Amount = transactionLog.Amount.ToString(),
                    transactionLog.Description,
                    TransactionDate = transactionLog.CreationTime.ToShortDateString()
                });
            }

            return Json(transactionList);
        }
    }
}