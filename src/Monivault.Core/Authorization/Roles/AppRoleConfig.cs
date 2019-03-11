using Abp.MultiTenancy;
using Abp.Zero.Configuration;

namespace Monivault.Authorization.Roles
{
    public static class AppRoleConfig
    {
        public static void Configure(IRoleManagementConfig roleManagementConfig)
        {
            // Static host roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Host.Admin,
                    MultiTenancySides.Host,
                    false
                )
            );

            // Static tenant roles

            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.Admin,
                    MultiTenancySides.Tenant,
                    false
                )
            );
            roleManagementConfig.StaticRoles.Add(
                new StaticRoleDefinition(
                    StaticRoleNames.Tenants.AccountHolder,
                    MultiTenancySides.Tenant,
                    false
                )
            );
        }
    }
}
