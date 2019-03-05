using Abp.Authorization;
using Monivault.Authorization.Roles;
using Monivault.Authorization.Users;

namespace Monivault.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {
        }
    }
}
