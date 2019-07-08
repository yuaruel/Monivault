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

        public void SendSampleEmail()
        {
            try
            {
                var emailTemplate = new StringBuilder(_monivaultEmailTemplateProvider.GetSampleEmail());

                _mailSender.Send("henryezeanya@gmail.com", "Sample Email", emailTemplate.ToString());
            }catch(Exception exc)
            {
                Logger.Error($"Mail sending error: {exc.StackTrace}");
            }
        }
    }
}
