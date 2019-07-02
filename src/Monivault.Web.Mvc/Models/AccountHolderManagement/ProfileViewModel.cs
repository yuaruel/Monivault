using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Models.AccountHolderManagement
{
    public class ProfileViewModel
    {
        public string IdentityNumber { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
