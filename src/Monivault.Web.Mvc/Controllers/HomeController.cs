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
            if (PermissionChecker.IsGranted(PermissionNames.ViewAdminDashboard))
            {
                return RedirectToAction("Index", "AdminHome");
            }
            if (PermissionChecker.IsGranted(PermissionNames.ViewAccountHolderDashboard))
            {
                return RedirectToAction("Index", "AccountHolderHome");
            }

            return View();
        }
	}
}
