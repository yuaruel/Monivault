using System;

namespace Monivault.BackgroundJobs
{
    [Serializable]
    public class SmsJobArgs
    {
        public string Message { get; set; }
        public string Recipient { get; set; }
    }
}