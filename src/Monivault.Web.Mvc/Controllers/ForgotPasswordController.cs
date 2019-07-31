using Abp.Domain.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;
using Monivault.ForgotPassword;
using Monivault.Web.Models.ForgotPassword;
using System.Threading.Tasks;
using Monivault.Configuration;

namespace Monivault.Web.Controllers
{
    public class ForgotPasswordController : MonivaultControllerBase
    {
        private readonly IForgotPasswordAppService _forgotPasswordAppService;

        public ForgotPasswordController(IForgotPasswordAppService forgotPasswordAppService)
        {
            _forgotPasswordAppService = forgotPasswordAppService;
        }

        // GET
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(RequestPasswordResetViewModel viewModel)
        {
            try
            {
                var passwordValidated = await _forgotPasswordAppService.ValidateEmailOrPhoneNumber(viewModel.EmailOrPhoneNumber);
                if (passwordValidated)
                {
                    return RedirectToAction("ResetLinkSent");
                }

                //TODO Display the sent email in the form including the error shown.
                
            }catch(EntityNotFoundException enfExc)
            {
                ViewBag.ErrorMessage = enfExc.Message;
            }
            
            return View();
        }

        public ActionResult ResetLinkSent()
        {
            return View();
        }


    }
}