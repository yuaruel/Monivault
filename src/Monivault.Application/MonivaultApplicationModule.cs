using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Monivault.Authorization;

namespace Monivault
{
    [DependsOn(
        typeof(MonivaultCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class MonivaultApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<MonivaultAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(MonivaultApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddProfiles(thisAssembly)
            );
        }
    }
}
