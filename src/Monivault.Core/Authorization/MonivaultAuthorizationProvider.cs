using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using Monivault.MultiTenancy;

namespace Monivault.Authorization
{
    public class MonivaultAuthorizationProvider : AuthorizationProvider
    {
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            context.CreatePermission(PermissionNames.ViewAdminDashboard, L(PermissionNames.ViewAdminDashboard), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.ViewAdminReports, L(PermissionNames.ViewAdminReports), multiTenancySides: MultiTenancySides.Tenant);
            
            context.CreatePermission(PermissionNames.ViewUsers, L(PermissionNames.ViewUsers), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.CreateUsers, L(PermissionNames.CreateUsers), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.EditUsers, L(PermissionNames.CreateUsers), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.DeleteUsers, L(PermissionNames.DeleteUsers), multiTenancySides: MultiTenancySides.Tenant);
            
 
            context.CreatePermission(PermissionNames.ViewRoles, L(PermissionNames.ViewRoles), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.CreateRoles, L(PermissionNames.CreateRoles), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.EditRoles, L(PermissionNames.EditRoles), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.DeleteRoles, L(PermissionNames.DeleteRoles), multiTenancySides: MultiTenancySides.Tenant);

            context.CreatePermission(PermissionNames.ViewAccountHolderDashboard, L(PermissionNames.ViewAccountHolderDashboard), multiTenancySides: MultiTenancySides.Tenant);

            context.CreatePermission(PermissionNames.DoPayCodeTransfer, L(PermissionNames.DoPayCodeTransfer), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.DoBankAccountTransfer, L(PermissionNames.DoBankAccountTransfer), multiTenancySides: MultiTenancySides.Tenant);

            context.CreatePermission(PermissionNames.DoTaxPayment, L(PermissionNames.DoTaxPayment), multiTenancySides: MultiTenancySides.Tenant);

            context.CreatePermission(PermissionNames.ViewAccountHolders, L(PermissionNames.ViewAccountHolders), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.ViewSettingsPage, L(PermissionNames.ViewSettingsPage), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.TopUpSaving, L(PermissionNames.TopUpSaving), multiTenancySides: MultiTenancySides.Tenant);

            context.CreatePermission(PermissionNames.BuyAirtime, L(PermissionNames.BuyAirtime), multiTenancySides: MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MonivaultConsts.LocalizationSourceName);
        }
    }
}
