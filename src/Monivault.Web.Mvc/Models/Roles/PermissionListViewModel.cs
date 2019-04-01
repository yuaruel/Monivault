using System.Collections.Generic;
using Monivault.Roles.Dto;

namespace Monivault.Web.Models.Roles
{
    public class PermissionListViewModel
    {
        public IReadOnlyList<PermissionDto> Permissions { get; set; }
    }
}