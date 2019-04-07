using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Monivault.TransactionLogs.Dto;

namespace Monivault.Web.Models.AccountHolderHome
{
    public class AccountHolderDashboardViewModel
    {
        public List<RecentTransactionDto> Transactions { get; set; }
    }
}