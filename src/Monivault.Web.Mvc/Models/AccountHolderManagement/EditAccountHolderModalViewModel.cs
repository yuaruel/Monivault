using Microsoft.AspNetCore.Mvc.Rendering;
using Monivault.AccountOfficers.Dto;
using Monivault.Users.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Monivault.Web.Models.AccountHolderManagement
{
    public class EditAccountHolderModalViewModel
    {
        public string AccountHolderKey { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string EmailAddress { get; set; }

        public string AccountOfficer { get; set; }

        public List<SelectListItem> AccountOfficers { get; set; }
    }
}
