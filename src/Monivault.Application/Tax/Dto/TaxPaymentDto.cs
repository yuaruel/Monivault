using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Tax.Dto
{
    public class TaxPaymentDto
    {
        public decimal Amount { get; set; }

        public string TaxType { get; set; }

        public string TaxPeriod { get; set; }

        public string DatePaid { get; set; }
    }
}
