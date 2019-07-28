using Abp.BackgroundJobs;
using Abp.Dependency;
using Monivault.Emailing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.BackgroundJobs.Email
{
    public class TemporaryPasswordCreatedEmailJob : BackgroundJob<TemporaryPasswordCreatedEmailJobArgs>, ITransientDependency
    {
        private readonly IMonivaultMailSender _monivaultailSender;

        public TemporaryPasswordCreatedEmailJob(
                IMonivaultMailSender monivaultMailSender
            )
        {
            _monivaultailSender = monivaultMailSender;
        }

        public override void Execute(TemporaryPasswordCreatedEmailJobArgs args)
        {
            _monivaultailSender.SendTemporaryPassword(args.UserEmail, args.TemporaryPassword);
        }
    }
}
