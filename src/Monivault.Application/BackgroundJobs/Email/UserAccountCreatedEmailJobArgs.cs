using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Email
{
    public class UserAccountCreatedEmailJobArgs
    {
        public string UserEmail { get; set; }

        public string FullName { get; set; }

        public string UserPassword { get; set; }

        public string UserName { get; set; }
    }
}
