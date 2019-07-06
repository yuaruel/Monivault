using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Tax.Dto
{
    public class MakePaymentInput
    {
        public string TaxIdentificationNumber { get; set; }

        public string FullName { get; set; }

        public int TaxType { get; set; }

        public DateTime TaxPeriod { get; set; }

        public decimal Amount { get; set; }
    }
}
