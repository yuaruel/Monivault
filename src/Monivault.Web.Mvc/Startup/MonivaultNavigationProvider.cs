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
                        requiredPermissionName: PermissionNames.ViewAdminDashboard
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.AccountHolderDashboard,
                        L("Dashboard"),
                        url: "AccountHolderHome",
                        icon: "flaticon-imac",
                        requiredPermissionName: PermissionNames.ViewAccountHolderDashboard
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.MoneyTransfer,
                        L("MoneyTransfer"),
                        url: "Transfers",
                        icon: "flaticon-diagram"
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.BankAccount,
                            L("BankAccount"),
                            url: "MoneyTransfer/BankAccount",
                            requiredPermissionName: PermissionNames.DoBankAccountTransfer  
                        )
                    )//.AddItem(
                    //    new MenuItemDefinition(
                    //        PageNames.PayCode,
                    //        L("PayCode"),
                    //        requiredPermissionName: PermissionNames.DoPayCodeTransfer
                    //    )
                    //)
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Utilities,
                        L("Utilities"),
                        url: "About",
                        icon: "flaticon-interface-6"
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.Tax,
                            L(PageNames.Tax),
                            requiredPermissionName: PermissionNames.DoTaxPayment
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Profile,
                                L(PageNames.Profile),
                                url: "Tax/Profile",
                                icon: "flaticon-avatar"
                            )
                        ).AddItem(
                            new MenuItemDefinition(
                                PageNames.Payment,
                                L(PageNames.Payment),
                                url: "Tax/Payment",
                                icon: "flaticon-feed"
                            )
                        )
                    ).AddItem(
                        new MenuItemDefinition(
                            PageNames.BuyAirtime,
                            L("BuyAirtime"),
                            url: "BuyAirtime",
                            requiredPermissionName: PermissionNames.BuyAirtime
                        )
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.AccountHolders,
                        L("AccountHolders"),
                        url: "AccountHolderManagement",
                        icon: "flaticon-profile-1",
                        requiredPermissionName: PermissionNames.ViewAccountHolders
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.Settings,
                        L("Settings"),
                        url: "Settings",
                        icon: "flaticon-cogwheel-2",
                        requiredPermissionName: PermissionNames.ViewSettingsPage
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.UserManagement,
                        L("UserManagement"),
                        url: "Users",
                        icon: "flaticon-users",
                        requiredPermissionName: PermissionNames.ViewUsers
                    )
                ).AddItem(
                    new MenuItemDefinition(
                        PageNames.RoleManagement,
                        L("RoleManagement"),
                        url: "Roles",
                        icon: "flaticon-web",
                        requiredPermissionName: PermissionNames.ViewRoles
                    )
                );
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, MonivaultConsts.LocalizationSourceName);
        }
    }
}
