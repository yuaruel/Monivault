using Abp.AspNetCore.Mvc.ViewComponents;

namespace Monivault.Web.Views
{
    public abstract class MonivaultViewComponent : AbpViewComponent
    {
        protected MonivaultViewComponent()
        {
            LocalizationSourceName = MonivaultConsts.LocalizationSourceName;
        }
    }
}
