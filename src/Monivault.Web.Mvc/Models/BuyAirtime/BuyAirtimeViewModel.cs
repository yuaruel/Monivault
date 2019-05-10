using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Models.BuyAirtime
{
    public class BuyAirtimeViewModel
    {
        public List<SelectListItem> AirtimeNetworks { get; set; }

        public string AirtimeNetwork { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public decimal Amount { get; set; }
    }
}
