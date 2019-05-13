using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Monivault.BuyAirtime.Dto
{
    public class AirtimePurchaseDto
    {
        public string AirtimeNetwork { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
