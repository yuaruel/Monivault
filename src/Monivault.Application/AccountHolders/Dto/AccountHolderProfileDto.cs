using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.AccountHolders.Dto
{
    public class AccountHolderProfileDto
    {
        public string AccountHolderKey { get; set; }

        public string IdentityNumber { get; set; }

        public string FullName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public DateTime DateJoined { get; set; }

        public DateTime LastSeen { get; set; }
    }
}
