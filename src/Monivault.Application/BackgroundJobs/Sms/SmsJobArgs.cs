using System;

namespace Monivault.BackgroundJobs.Sms
{
    [Serializable]
    public class SmsJobArgs
    {
        public string Message { get; set; }
        public string RecipientPhone { get; set; }
        public SmsType SmsType { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal NewBalance { get; set; }
        public string TransactionServiceName { get; set; }
        public string TransactionDate { get; set; }
        public int AccountHolderId { get; set; }
    }
}