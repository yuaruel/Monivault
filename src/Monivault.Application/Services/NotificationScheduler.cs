using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Castle.Core.Logging;
using Hangfire;
using Monivault.BackgroundJobs;

namespace Monivault.ModelServices
{
    public class NotificationScheduler : ISingletonDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        public ILogger Logger { get; set; }
        
        public NotificationScheduler(IBackgroundJobManager backgroundJobManager)
        {
            _backgroundJobManager = backgroundJobManager;
            Logger = NullLogger.Instance;
        }
        
        public void ScheduleWelcomeMessage(string phoneNumber, string email, string accountIdentity)
        {
            Logger.Info("About to set sms notification job");
            ScheduleWelcomeText(phoneNumber, accountIdentity);
            ScheduleWelcomeEmail(email);
        }

        public void ScheduleOtp(string phoneNumber, string email, string otp)
        {
            ScheduleOtpText(phoneNumber, otp);
        }

        private void ScheduleWelcomeText(string phoneNumber, string accountIdentity)
        {
            var message = new StringBuilder();
            message.AppendLine("Welcome to Monivault.");
            message.AppendLine($"Your Account ID is {accountIdentity}.");
            
            Logger.Info($"user phone number: {phoneNumber}");
            
            var smsJobArg = new SmsJobArgs
            {
                Message = message.ToString(),
                Recipient = phoneNumber
            };

            _backgroundJobManager.EnqueueAsync<SmsJob, SmsJobArgs>(smsJobArg);
        }

        private void ScheduleOtpText(string phoneNumber, string otp)
        {
            var message = new StringBuilder();
            message.AppendLine($"Monivault OTP: {otp}");
            
            var smsJobArg = new SmsJobArgs
            {
                Message = message.ToString(),
                Recipient = phoneNumber
            };

            _backgroundJobManager.EnqueueAsync<SmsJob, SmsJobArgs>(smsJobArg);
        }

        private void ScheduleWelcomeEmail(string email)
        {
            
        }
    }
}