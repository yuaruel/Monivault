using System.Text;
using System.Threading.Tasks;
using Abp.BackgroundJobs;
using Abp.Dependency;
using Castle.Core.Logging;
using Hangfire;
using Monivault.BackgroundJobs;
using Monivault.BackgroundJobs.Sms;

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

        public void ScheduleCreditMessage(int accountHolderId, decimal amount, decimal newBalance, string transactionDate, string recipientPhone, string transactionServiceName)
        {
            _backgroundJobManager.EnqueueAsync<SmsJob, SmsJobArgs>(new SmsJobArgs
            {
                SmsType = SmsType.CreditSms,
                TransactionServiceName = transactionServiceName,
                AccountHolderId = accountHolderId,
                CreditAmount = amount,
                NewBalance = newBalance,
                TransactionDate = transactionDate,
                RecipientPhone = recipientPhone
            });
        }

        public async Task ScheduleOtp(string phoneNumber, string email, string otp)
        {
            await ScheduleOtpText(phoneNumber, otp);
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
                RecipientPhone = phoneNumber
            };

            _backgroundJobManager.EnqueueAsync<SmsJob, SmsJobArgs>(smsJobArg);
        }

        private async Task ScheduleOtpText(string phoneNumber, string otp)
        {
            var message = new StringBuilder();
            message.AppendLine($"Monivault OTP: {otp}");
            
            var smsJobArg = new SmsJobArgs
            {
                Message = message.ToString(),
                RecipientPhone = phoneNumber
            };

            await _backgroundJobManager.EnqueueAsync<SmsJob, SmsJobArgs>(smsJobArg);
        }

        private void ScheduleWelcomeEmail(string email)
        {
            
        }
    }
}