using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Monivault.Exceptions;
using Monivault.ModelServices;

namespace Monivault.BackgroundJobs
{
    public class SmsJob : AsyncBackgroundJob<SmsJobArgs>, ITransientDependency
    {
        private readonly SmsService _smsService;

        public SmsJob(SmsService smsService)
        {
            _smsService = smsService;
        }
        
        protected override async Task ExecuteAsync(SmsJobArgs args) => await _smsService.SendSms(args.Message, args.Recipient);
    }
}