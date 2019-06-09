using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Abp.Authorization;
using Monivault.Authorization.Roles;
using Microsoft.Extensions.Options;

namespace Monivault.Authorization.Users
{
    public class UserClaimsPrincipalFactory : AbpUserClaimsPrincipalFactory<User, Role>
    {
        public UserClaimsPrincipalFactory(
            UserManager userManager,
            RoleManager roleManager,
            IOptions<IdentityOptions> optionsAccessor)
            : base(
                  userManager,
                  roleManager,
                  optionsAccessor)
        {
        }
    }
}
