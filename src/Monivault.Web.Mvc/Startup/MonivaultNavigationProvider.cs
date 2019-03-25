using Abp.Application.Navigation;
using Abp.Localization;
using Monivault.Authorization;

namespace Monivault.Web.Startup
{
    /// <summary>
    /// This class defines menus for the application.
    /// </summary>
    public class MonivaultNavigationProvider : NavigationProvider
    {
        public override void SetNavigation(INavigationProviderContext context)
        {
            context.Manager.MainMenu
                .AddItem(
                    new MenuItemDefinition(
                        PageNames.AdminDashboard,
                        L("Dashboard"),
                        url: "",
                        icon: "flaticon-imac",
                        requiredPermissionName: PermissionNames.Pages_Admin_Dashboard
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.AccountHolderDashboard,
                        L("Dashboard"),
                        url: "AccountHolderHome",
                        icon: "flaticon-imac",
                        requiredPermissionName: PermissionNames.Pages_AccountHolder_Dashboard
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Transfers,
                        L("Transfers"),
                        url: "Transfers",
                        icon: "flaticon-diagram",
                        requiredPermissionName: PermissionNames.Pages_Transfer
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.BankAccount,
                            L("BankAccount"),
                            requiredPermissionName: PermissionNames.Pages_Transfer_BankAccount  
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.PayCode,
                            L("PayCode"),
                            requiredPermissionName: PermissionNames.Pages_Transfer_PayCode
                        )
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Utilities,
                        L("Utilities"),
                        url: "About",
                        icon: "flaticon-interface-6",
                        requiredPermissionName: PermissionNames.Pages_Utilities
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.TaxPayment,
                            L("TaxPayment"),
                            requiredPermissionName: PermissionNames.Pages_Utilities_TaxPayment
                        )
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.AccountHolders,
                        L("AccountHolders"),
                        url: "",
                        icon: "flaticon-profile-1",
                        requiredPermissionName: PermissionNames.Pages_AccountHolder_Management
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Settings,
                        L("Settings"),
                        url: "",
                        icon: "flaticon-cogwheel-2",
                        requiredPermissionName: PermissionNames.Pages_Settings
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.UserManagement,
                        L("UserManagement"),
                        url: "",
                        icon: "flaticon-users",
                        requiredPermissionName: PermissionNames.Pages_UserManagement
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.RoleManagement,
                        L("RoleManagement"),
                        url: "Roles",
                        icon: "flaticon-web",
                        requiredPermissionName: PermissionNames.Pages_RoleManagement
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MonivaultConsts.LocalizationSourceName);
        }
    }
}
