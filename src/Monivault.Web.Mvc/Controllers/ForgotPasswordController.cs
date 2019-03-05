using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;

namespace Monivault.Web.Controllers
{
    public class ForgotPasswordController : MonivaultControllerBase
    {
        // GET
        public IActionResult Index()
        {
            return View();
        }
    }
}