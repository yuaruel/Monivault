using Abp.BackgroundJobs;
using Abp.Dependency;
using Monivault.Emailing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Email
{
    public class UserAccountCreatedEmailJob : BackgroundJob<UserAccountCreatedEmailJobArgs>, ITransientDependency
    {
        private readonly IMonivaultMailSender _monivaultailSender;

        public UserAccountCreatedEmailJob(
                IMonivaultMailSender monivaultMailSender
            )
        {
            _monivaultailSender = monivaultMailSender;
        }

        public override void Execute(UserAccountCreatedEmailJobArgs args)
        {
            _monivaultailSender.SendUserAccountCreatedMail(args.UserEmail, args.UserName, args.UserPassword, args.FullName);
        }
    }
}
