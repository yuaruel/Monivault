using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Host.Model
{
    public class PurchaseAirtimeModel
    {
        public string ProductCode { get; set; }
        public decimal AirtimeValue { get; set; }
        public string PhoneNumber { get; set; }
        public string PlatformSpecific { get; set; }
        public string RequestOriginatingPlatform { get; set; }
    }
}
