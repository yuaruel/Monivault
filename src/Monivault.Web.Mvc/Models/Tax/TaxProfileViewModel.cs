using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Models.Tax
{
    public class TaxProfileViewModel
    {
        public string TaxProfileKey { get; set; }

        public string Tin { get; set; }

        public string FullName { get; set; }

        public string ReconcilliationPvNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
