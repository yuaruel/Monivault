using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Monivault.Authorization;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : MonivaultControllerBase
    {
        public ActionResult Index()
        {
            if (PermissionChecker.IsGranted(PermissionNames.Pages_Admin_Dashboard))
            {
                return RedirectToAction("Index", "AdminHome");
            }else if (PermissionChecker.IsGranted(PermissionNames.Pages_AccountHolder_Dashboard))
            {
                return RedirectToAction("Index", "AccountHolderHome");
            }
            return View();
        }
	}
}
