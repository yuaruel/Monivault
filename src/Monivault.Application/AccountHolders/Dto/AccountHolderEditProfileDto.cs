using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.AccountHolders.Dto
{
    public class AccountHolderEditProfileDto
    {
        public string AccountHolderKey { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }
        
        public long? AccountOfficerId { get; set; }
    }
}
