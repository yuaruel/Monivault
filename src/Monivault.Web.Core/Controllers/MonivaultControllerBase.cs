using Abp.AspNetCore.Mvc.Controllers;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;

namespace Monivault.Controllers
{
    public abstract class MonivaultControllerBase: AbpController
    {
        protected MonivaultControllerBase()
        {
            LocalizationSourceName = MonivaultConsts.LocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
