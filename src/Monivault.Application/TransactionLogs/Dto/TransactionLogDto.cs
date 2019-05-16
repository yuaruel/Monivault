using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.TransactionLogs.Dto
{
    public class TransactionLogDto
    {
        public int AccountHolderId { get; set; }

        public decimal Amount { get; set; }

        public decimal BalanceAfterTransaction { get; set; }

        public string TransactionType { get; set; }

        public string Description { get; set; }

        public string RequestOriginatingPlatform { get; set; }

        public string PlatformSpecificDetail { get; set; }

        public string TransactionService { get; set; }

        public DateTime CreationTime { get; set; } = new DateTime();
    }
}
