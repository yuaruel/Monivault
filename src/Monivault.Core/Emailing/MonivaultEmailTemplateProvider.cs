using Abp.Dependency;
using Abp.IO.Extensions;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Monivault.Emailing
{
    public class MonivaultEmailTemplateProvider : IMonivaultEmailTemplateProvider, ITransientDependency
    {
        public ILogger Logger { get; set; }

        public MonivaultEmailTemplateProvider()
        {
            Logger = NullLogger.Instance;
        }

        public string GetUserAccountCreatedTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Monivault.Emailing.EmailTemplates.userAccountCreated.html"))
            {
                try
                {
                    var bytes = stream.GetAllBytes();
                    var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

                    return template;
                }catch(Exception exc)
                {
                    throw exc;
                }
            }
        }

        public string GetPasswordResetTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Monivault.Emailing.EmailTemplates.accountPasswordReset.html"))
            {
                try
                {
                    var bytes = stream.GetAllBytes();
                    var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

                    return template;
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }
        }

        public string GetTemporaryPasswordTemplate()
        {
            using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Monivault.Emailing.EmailTemplates.accountTemporaryPassword.html"))
            {
                try
                {
                    var bytes = stream.GetAllBytes();
                    var template = Encoding.UTF8.GetString(bytes, 3, bytes.Length - 3);

                    return template;
                }
                catch (Exception exc)
                {
                    throw exc;
                }
            }
        }
    }
}
