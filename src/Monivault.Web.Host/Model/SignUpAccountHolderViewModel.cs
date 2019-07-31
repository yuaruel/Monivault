using System.ComponentModel.DataAnnotations;

namespace Monivault.Web.Models
{
    public class SignUpAccountHolderViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        [Required]
        public string VerificationCode { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}