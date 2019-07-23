using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Sms
{
    public enum SmsType
    {
        NonTransactionalSms,
        CreditSms,
        DebitSms
    }
}
