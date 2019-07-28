using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Models.ForgotPassword
{
    public class RequestPasswordResetViewModel
    {
        public string EmailOrPhoneNumber { get; set; }
    }
}
