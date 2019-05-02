using System.ComponentModel.DataAnnotations;

namespace Monivault.Web.Models.Profile
{
    public class UpdatePasswordViewModel
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}