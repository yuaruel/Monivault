using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.AccountHolders.Dto
{
    public class AccountHolderListDto
    {
        public Guid AccountHolderKey { get; set; }

        public string AccountIdentity { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}
