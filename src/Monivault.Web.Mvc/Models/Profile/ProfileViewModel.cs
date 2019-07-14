using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monivault.AppModels;

namespace Monivault.Web.Models.Profile
{
    public class ProfileViewModel
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Bank { get; set; }
        
        public List<SelectListItem> Banks { get; set; }

        public string AccountNumber { get; set; }

        public string AccountName { get; set; }

        public string ProfilePictureUrl { get; set; }
    }
}