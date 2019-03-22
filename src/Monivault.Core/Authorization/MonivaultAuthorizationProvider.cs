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
            context.CreatePermission(PermissionNames.Pages_Admin_Dashboard, L("AdminDashboard"), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.Pages_Admin_Reports, L("AdminReports"), multiTenancySides: MultiTenancySides.Tenant);
            
            var userPermission = context.CreatePermission(PermissionNames.Pages_UserManagement, L("UserManagement"), multiTenancySides: MultiTenancySides.Tenant);
            userPermission.CreateChildPermission(PermissionNames.Pages_UserManagement_Create, L("UserManagementCreate"));
            userPermission.CreateChildPermission(PermissionNames.Pages_UserManagement_Edit, L("UserManagementEdit"));
            userPermission.CreateChildPermission(PermissionNames.Pages_UserManagement_Delete, L("UserManagementDelete"));
            
            var rolePermission = context.CreatePermission(PermissionNames.Pages_RoleManagement, L("RoleManagement"), multiTenancySides: MultiTenancySides.Tenant);
            rolePermission.CreateChildPermission(PermissionNames.Pages_RoleManagement_Create, L("RoleManagementCreate"));
            rolePermission.CreateChildPermission(PermissionNames.Pages_RoleManagement_Edit, L("RoleManagementEdit"));
            rolePermission.CreateChildPermission(PermissionNames.Pages_RoleManagement_Delete, L("RoleManagementDelete"));

            context.CreatePermission(PermissionNames.Pages_AccountHolder_Dashboard, L("AccountHolderDashboard"), multiTenancySides: MultiTenancySides.Tenant);

            var transferPermission = context.CreatePermission(PermissionNames.Pages_Transfer, L("Transfer"), multiTenancySides: MultiTenancySides.Tenant);
            transferPermission.CreateChildPermission(PermissionNames.Pages_Transfer_PayCode, L("TransferPayCode"));
            transferPermission.CreateChildPermission(PermissionNames.Pages_Transfer_BankAccount, L("TransferBankAccount"));

            var utilitiesPermission = context.CreatePermission(PermissionNames.Pages_Utilities, L("Utilities"), multiTenancySides: MultiTenancySides.Tenant);
            utilitiesPermission.CreateChildPermission(PermissionNames.Pages_Utilities_TaxPayment, L("UtilitiesTaxPayment"));

            context.CreatePermission(PermissionNames.Pages_AccountHolder_Management, L("AccountHoldersManagement"), multiTenancySides: MultiTenancySides.Tenant);
            context.CreatePermission(PermissionNames.Pages_Settings, L("Settings"),
                multiTenancySides: MultiTenancySides.Tenant);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MonivaultConsts.LocalizationSourceName);
        }
    }
}
