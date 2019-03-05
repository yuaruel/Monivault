using System.Collections.Generic;
using Monivault.Roles.Dto;
using Monivault.Users.Dto;

namespace Monivault.Web.Models.Users
{
    public class UserListViewModel
    {
        public IReadOnlyList<UserDto> Users { get; set; }

        public IReadOnlyList<RoleDto> Roles { get; set; }
    }
}
