using Abp.BackgroundJobs;
using Abp.Dependency;
using Monivault.Emailing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Email
{
    public class PasswordResetLinkEmailJob : BackgroundJob<PasswordResetLinkEmailJobArgs>, ITransientDependency
    {
        private readonly IMonivaultMailSender _monivaultMailSender;

        public PasswordResetLinkEmailJob(IMonivaultMailSender monivaultMailSender)
        {
            _monivaultMailSender = monivaultMailSender;
        }

        public override void Execute(PasswordResetLinkEmailJobArgs args)
        {
            //
            _monivaultMailSender.SendResetPasswordLink(args.Email, args.ResetLink);
        }
    }
}
