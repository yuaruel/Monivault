using System;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Hangfire;
using Monivault.Configuration;
using Monivault.SavingsInterests;

namespace Monivault.Web.Startup
{
    [DependsOn(typeof(MonivaultWebCoreModule))]
    [DependsOn(typeof(AbpHangfireAspNetCoreModule))]
    public class MonivaultWebMvcModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MonivaultWebMvcModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            Configuration.Navigation.Providers.Add<MonivaultNavigationProvider>();
            Configuration.BackgroundJobs.UseHangfire();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MonivaultWebMvcModule).GetAssembly());
        }
    }
}
