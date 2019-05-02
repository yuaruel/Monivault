using System;
using System.ComponentModel.DataAnnotations;

namespace Monivault.Web.Models.Profile
{
    public class PersonalDetailViewModel
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Email { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
