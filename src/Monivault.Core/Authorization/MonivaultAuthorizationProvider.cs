using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;

namespace Monivault.Authorization
{
    public class MonivaultAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.Pages_Admin_Dashboard, L("AdminDashboard"), multiTenancySides: MultiTenancySides.Tenant);
            
            var userPermission = context.CreatePermission(PermissionNames.Pages_UserManagement, L("UserManagement"), multiTenancySides: MultiTenancySides.Tenant);
            userPermission.CreateChildPermission(PermissionNames.Pages_UserManagement_Create, L("UserManagementCreate"));
            userPermission.CreateChildPermission(PermissionNames.Pages_UserManagement_Edit, L("UserManagementEdit"));
            userPermission.CreateChildPermission(PermissionNames.Pages_UserManagement_Delete, L("UserManagementDelete"));
            
            var rolePermission = context.CreatePermission(PermissionNames.Pages_RoleManagement, L("RoleManagement"), multiTenancySides: MultiTenancySides.Tenant);
            rolePermission.CreateChildPermission(PermissionNames.Pages_RoleManagement_Create, L("RoleManagementCreate"));
            rolePermission.CreateChildPermission(PermissionNames.Pages_RoleManagement_Edit, L("RoleManagementEdit"));
            rolePermission.CreateChildPermission(PermissionNames.Pages_RoleManagement_Delete, L("RoleManagementDelete"));

            context.CreatePermission(PermissionNames.Pages_Account_Holder_Dashboard, L("AccountHolderDashboard"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MonivaultConsts.LocalizationSourceName);
        }
    }
}
