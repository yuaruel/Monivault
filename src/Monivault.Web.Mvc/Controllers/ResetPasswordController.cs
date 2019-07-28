using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Monivault.Controllers;
using Monivault.ForgotPassword;

namespace Monivault.Web.Mvc.Controllers
{
    public class ResetPasswordController : MonivaultControllerBase
    {
        private readonly IForgotPasswordAppService _forgotPasswordAppService;

        public ResetPasswordController(IForgotPasswordAppService forgotPasswordAppService)
        {
            _forgotPasswordAppService = forgotPasswordAppService;
        }

        public async Task<IActionResult> Index(string ptk)
        {
            var passwordReset = await _forgotPasswordAppService.ResetPassword(ptk);

            if (!passwordReset)
            {
                return RedirectToAction("InvalidPasswordReset");
            }
            return View();
        }

        public IActionResult InvalidPasswordReset()
        {
            return View();
        }
    }
}