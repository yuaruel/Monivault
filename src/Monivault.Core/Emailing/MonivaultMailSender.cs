using Abp.Dependency;
using Abp.Net.Mail;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.Emailing
{
    public class MonivaultMailSender : MonivaultServiceBase, IMonivaultMailSender, ITransientDependency
    {
        private readonly IEmailSender _mailSender;
        private readonly IMonivaultEmailTemplateProvider _monivaultEmailTemplateProvider;

        public MonivaultMailSender(
                IEmailSender mailSender,
                IMonivaultEmailTemplateProvider monivaultEmailTemplateProvider
            )
        {
            _mailSender = mailSender;
            _monivaultEmailTemplateProvider = monivaultEmailTemplateProvider;
        }

        public void SendUserAccountCreatedMail(string userEmail, string userName, string userPassword, string fullName)
        {
            try
            {
                var emailTemplate = new StringBuilder(_monivaultEmailTemplateProvider.GetUserAccountCreatedTemplate());
                emailTemplate.Replace("{FullName}", fullName);
                emailTemplate.Replace("{UserName}", userName);
                emailTemplate.Replace("{TemporaryPassword}", userPassword);

                _mailSender.Send(userEmail, "Monivault User Accounnt", emailTemplate.ToString());
            }catch(Exception exc)
            {
                Logger.Error($"Mail sending error: {exc.StackTrace}");
            }
        }
    }
}
