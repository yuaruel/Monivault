using Abp.AutoMapper;
using Monivault.AppModels;
using System;

namespace Monivault.TransactionLogs.Dto
{
    [AutoMap(typeof(TransactionLog))]
    public class ProfileViewTransactionDto
    {
        public Guid TransactionKey { get; set; }

        public decimal Amount { get; set; }

        public string TransactionType { get; set; }

        public string Description { get; set; }

        public decimal BalanceAfterTransaction { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
