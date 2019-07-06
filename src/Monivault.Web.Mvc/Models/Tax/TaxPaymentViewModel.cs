using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Models.Tax
{
    public class TaxPaymentViewModel
    {
        public string TaxIdentificationNumber { get; set; }

        public string FullName { get; set; }

        public int TaxType { get; set; }

        public List<SelectListItem> TaxTypes { get; set; }

        public DateTime TaxPeriod { get; set; }

        public decimal Amount { get; set; }
    }
}
