using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace Monivault.Web.Models.Login
{
    public class LoginViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [DisableAuditing]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
