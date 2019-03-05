using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Monivault.Configuration;

namespace Monivault.Web.Host.Startup
{
    [DependsOn(
       typeof(MonivaultWebCoreModule))]
    public class MonivaultWebHostModule: AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public MonivaultWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(MonivaultWebHostModule).GetAssembly());
        }
    }
}
