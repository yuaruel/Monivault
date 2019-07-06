using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Tax.Dto
{
    public class TaxProfileDto
    {
        public string TaxProfileKey { get; set; }

        public string Tin { get; set; }

        public string FullName { get; set; }

        public string ReconcilliationPvNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
