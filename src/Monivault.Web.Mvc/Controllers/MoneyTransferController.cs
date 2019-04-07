using Microsoft.AspNetCore.Mvc;

namespace Monivault.Web.Controllers
{
    public class MoneyTransferController : Controller
    {
        // GET
        public IActionResult BankAccount()
        {
            return View();
        }
    }
}