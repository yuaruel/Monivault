using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Email
{
    public class TemporaryPasswordCreatedEmailJobArgs
    {
        public string UserEmail { get; set; }
        public string TemporaryPassword { get; set; }
    }
}
