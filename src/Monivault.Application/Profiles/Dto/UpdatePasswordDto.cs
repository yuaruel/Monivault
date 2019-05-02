using System.ComponentModel.DataAnnotations;

namespace Monivault.Profiles.Dto
{
    public class UpdatePasswordDto
    {
        [Required]
        public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}