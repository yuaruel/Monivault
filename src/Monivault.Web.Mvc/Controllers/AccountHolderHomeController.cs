using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize(PermissionNames.Pages_Account_Holder_Dashboard)]
    public class AccountHolderHomeController : MonivaultControllerBase
    {
        public ViewResult Index()
        {
            return View();
        }
    }
}