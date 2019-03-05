using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : MonivaultControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
	}
}
