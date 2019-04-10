using System.Collections.Generic;
using Abp.Configuration;

namespace Monivault.Configuration
{
    public class AppSettingProvider : SettingProvider
    {
        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new[]
            {
                new SettingDefinition(AppSettingNames.UiTheme, "red", scopes: SettingScopes.Application | SettingScopes.Tenant | SettingScopes.User, isVisibleToClients: true),
                new SettingDefinition(AppSettingNames.StopDeposit, "false", scopes: SettingScopes.Tenant), 
                new SettingDefinition(AppSettingNames.StopSignUp, "false", scopes: SettingScopes.Tenant), 
                new SettingDefinition(AppSettingNames.StopWithdrawal, "false", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.WithdrawalServiceCharge, "100", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.InterestStatus, "true", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.InterestType, "SimpleInterest", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.InterestRate, "12", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.InterestDuration, "30", scopes: SettingScopes.Tenant),
                new SettingDefinition(AppSettingNames.PenaltyPercentageDeduction, "50", scopes: SettingScopes.Tenant), 
            };
        }
    }
}
