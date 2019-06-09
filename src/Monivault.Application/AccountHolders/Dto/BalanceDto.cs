using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.AccountHolders.Dto
{
    public class BalanceDto
    {
        public decimal AvailableBalance { get; set; }
        public decimal LedgerBalance { get; set; }
    }
}
