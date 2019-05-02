﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Monivault.Profiles.Dto
{
    public class PersonalDetailDto
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
