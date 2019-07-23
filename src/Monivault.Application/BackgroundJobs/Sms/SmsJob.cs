using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Monivault.Exceptions;
using Monivault.ModelServices;

namespace Monivault.BackgroundJobs.Sms
{
    public class SmsJob : AsyncBackgroundJob<SmsJobArgs>, ITransientDependency
    {
        private readonly SmsService _smsService;

        public SmsJob(SmsService smsService)
        {
            _smsService = smsService;
        }

        protected override async Task ExecuteAsync(SmsJobArgs args)
        {
            switch (args.SmsType)
            {
                case SmsType.NonTransactionalSms:
                    await _smsService.SendSms(args.Message, args.RecipientPhone);
                    break;

                case SmsType.CreditSms:
                    await _smsService.SendCreditMessage(args.Amount, args.RecipientPhone, args.TransactionServiceName, args.TransactionDate, args.AccountHolderId);
                    break;

                case SmsType.DebitSms:

                    await _smsService.SendDebitMessage(args.Amount, args.RecipientPhone, args.TransactionServiceName, args.TransactionDate, args.AccountHolderId);
                    break;
            }
            
        }
    }
}