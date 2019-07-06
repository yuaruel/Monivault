using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Monivault.Tax.Dto
{
    public class UpdateTaxProfileInput
    {
        [Required]
        public string Tin { get; set; }

        [Required]
        public string FullName { get; set; }

        public string ReconcilliationPvNumber { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
