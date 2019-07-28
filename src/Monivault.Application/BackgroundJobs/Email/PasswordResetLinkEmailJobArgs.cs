using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Email
{
    public class PasswordResetLinkEmailJobArgs
    {
        public string Email { get; set; }
        public string ResetLink { get; set; }
    }
}
