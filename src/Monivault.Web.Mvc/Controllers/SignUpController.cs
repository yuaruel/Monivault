using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monivault.Authorization.Users;
using Monivault.Controllers;
using Monivault.ModelManagers;
using Monivault.Services;
using Monivault.SignUp;
using Monivault.Web.Models.SignUp;

namespace Monivault.Web.Controllers
{
    public class SignUpController : MonivaultControllerBase
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly SmsService _smsService;
        private readonly UserSignUpManager _userSignUpManager;
        private readonly UserManager _userManager;
        private readonly AccountHolderManager _accountHolderManager;
        
        public SignUpController(
            IVerificationCodeService verificationCodeService,
            SmsService smsService,
            UserSignUpManager userSignUpManager,
            UserManager userManager,
            AccountHolderManager accountHolderManager)
        {
            _verificationCodeService = verificationCodeService;
            _smsService = smsService;
            _userSignUpManager = userSignUpManager;
            _userManager = userManager;
            _accountHolderManager = accountHolderManager;
        }
        // GET
        public IActionResult Index()
        {
            return
            View();
        }

        [HttpPost]
        public async Task<JsonResult> SignUpAccountHolder(SignUpAccountHolderViewModel model)
        {
            var verificationCode = await _verificationCodeService.GetVerificationCode(int.Parse(model.VerificationCode), model.PhoneNumber);
            if (verificationCode == null) throw new UserFriendlyException("Invalid verification code");

            var user = await _userSignUpManager.SignUpAsync(
                                                    model.FirstName,
                                           model.LastName,
                                       model.Email,
                                                    model.PhoneNumber,
                                                    model.UserName,
                                                    model.Password,
                                    true);

            //TODO Create account holder information.
            _accountHolderManager.CreateAccountHolder(user.Id);
            
            return Json(new AjaxResponse{TargetUrl = "/SignUp/SuccessfulSignUp"});
        }

        public async Task<StatusCodeResult> SendUpVerificationCode(ContactDetailViewModel model)
        {
            //Check if any user exist that already used the phone number.
            var signUpUser = await _userManager.FindByPhoneNumberAsync(model.PhoneNumber);
            
            //Check if user supplied email address, if yes, check if a user exist that already used the email.
            if (!string.IsNullOrEmpty(model.Email))
            {
                signUpUser = await _userManager.FindByEmailAsync(model.Email);
            }

            //If user exists with either phone number or email end verification sending.
            if (signUpUser != null) throw new UserFriendlyException(L("UserAlreadyExist"));

            //_verificationCodeService.GenerateAndSendVerificationCode(model.PhoneNumber);
            
            return StatusCode(200);

        }

        public ViewResult SuccessfulSignUp()
        {
            return View();
        }
    }
}