using Abp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Monivault.AppModels
{
    public class AirtimeNetwork : Entity
    {
        [Required]
        public string NetworkName { get; set; }

        [Required]
        public string OneCardAirtimePurchaseCode { get; set; }
    }
}
