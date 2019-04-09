using Newtonsoft.Json.Serialization;

namespace Monivault.Configuration
{
    public static class AppSettingNames
    {
        public const string UiTheme = "App.UiTheme";
        public const string StopWithdrawal = "StopWithdrawal";
        public const string StopDeposit = "StopDeposit";
        public const string StopSignUp = "StopSignUp";
        public const string WithdrawalServiceCharge = "100";
        public const string InterestStatus = "true";
        public const string InterestType = "Simple";
        public const string InterestRate = "12";
        public const string InterestDuration = "30";
        public const string PenaltyPercentageDeduction = "50";
    }
}
