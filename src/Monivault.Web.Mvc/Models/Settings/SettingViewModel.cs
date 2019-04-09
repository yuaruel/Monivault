namespace Monivault.Web.Models.Settings
{
    public class SettingViewModel
    {
        public bool StopDeposit { get; set; } = false;

        public bool StopSignUp { get; set; } = false;

        public bool StopWithdrawal { get; set; } = false;

        public decimal WithdrawalServiceCharge { get; set; }

        public string InterestType { get; set; }

        public bool InterestStatus { get; set; }
    }
}