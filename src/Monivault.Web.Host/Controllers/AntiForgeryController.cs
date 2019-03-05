using Microsoft.AspNetCore.Antiforgery;
using Monivault.Controllers;

namespace Monivault.Web.Host.Controllers
{
    public class AntiForgeryController : MonivaultControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
