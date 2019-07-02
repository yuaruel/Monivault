using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;
using Monivault.TransactionLogs;

namespace Monivault.Web.Mvc.Controllers
{
    public class TransactionController : MonivaultControllerBase
    {
        private readonly ITransactionLogAppService _transactionLogAppService;

        public TransactionController(
                ITransactionLogAppService transactionLogAppService
            )
        {
            _transactionLogAppService = transactionLogAppService;
        }

        public JsonResult AccountHolderTransactions(string id)
        {
            var transactions = _transactionLogAppService.GetTransactionsForProfileView(id);

            return Json(transactions);
        }
    }
}