using System;

namespace Monivault.Web.Models.Settings
{
    public class SettingViewModel
    {
        public bool StopTopUpSaving { get; set; }

        public bool StopSignUp { get; set; }

        public bool StopWithdrawal { get; set; }

        public decimal WithdrawalServiceCharge { get; set; }

        public string InterestType { get; set; }

        public bool InterestStatus { get; set; }

        public decimal InterestRate { get; set; }

        public int InterestDuration { get; set; }

        public DateTime InterestDurationStartDate { get; set; }

        public DateTime InterestDurationEndDate { get; set; }

        public decimal PenaltyDeduction { get; set; }
    }
}