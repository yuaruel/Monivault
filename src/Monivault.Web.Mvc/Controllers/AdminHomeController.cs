using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    public class AdminHomeController : MonivaultControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}