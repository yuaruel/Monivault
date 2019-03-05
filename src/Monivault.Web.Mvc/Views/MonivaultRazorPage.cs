using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;

namespace Monivault.Web.Views
{
    public abstract class MonivaultRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected MonivaultRazorPage()
        {
            LocalizationSourceName = MonivaultConsts.LocalizationSourceName;
        }
    }
}
