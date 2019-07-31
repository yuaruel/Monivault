using System.ComponentModel.DataAnnotations;

namespace Monivault.Web.Models
{
    public class ContactDetailViewModel
    {
        [Required]
        public string PhoneNumber { get; set; }

        public string Email { get; set; }
    }
}