using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Monivault.AppModels;
using Monivault.Authorization.Users;
using Monivault.Configuration;
using Monivault.Controllers;
using Monivault.ModelServices;
using Monivault.SavingsInterests;
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
        private readonly AccountHolderService _accountHolderService;
        private readonly NotificationScheduler _notificationScheduler;
        private readonly SavingsInterestManager _savingsInterestManager;
        
        public SignUpController(
            IVerificationCodeService verificationCodeService,
            SmsService smsService,
            UserSignUpManager userSignUpManager,
            UserManager userManager,
            AccountHolderService accountHolderService,
            NotificationScheduler notificationScheduler,
            SavingsInterestManager savingsInterestManager)
        {
            _verificationCodeService = verificationCodeService;
            _smsService = smsService;
            _userSignUpManager = userSignUpManager;
            _userManager = userManager;
            _accountHolderService = accountHolderService;
            _notificationScheduler = notificationScheduler;
            _savingsInterestManager = savingsInterestManager;
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
            
            //Delete verificationCode.
            _verificationCodeService.ClearVerificationCode(verificationCode);

            var user = await _userSignUpManager.SignUpAsync(
                                                    model.FirstName,
                                           model.LastName,
                                       model.Email,
                                                    model.PhoneNumber,
                                                    model.UserName,
                                                    model.Password,
                                    true);

            var accountHolder = _accountHolderService.CreateAccountHolder(user.Id);
            
            //Check if InterestStatus is running and bootstrap Interest for accountholder.
            var interestStatus = await SettingManager.GetSettingValueAsync(AppSettingNames.InterestStatus);
            if (SavingsInterest.StatusTypes.Running.Equals(interestStatus))
            {
                await _savingsInterestManager.BootstrapNewSavingsInterestForAccountHolder(accountHolder.Id);
            }
            
            //Send a welcome text and email, with AccountHolder Identity.
            await _notificationScheduler.ScheduleWelcomeMessage(user.PhoneNumber, user.EmailAddress, accountHolder.AccountIdentity);

            return Json(new AjaxResponse{TargetUrl = "/SignUp/SuccessfulSignUp"});
        }

        public async Task<JsonResult> SendUpVerificationCode(ContactDetailViewModel model)
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

            await _verificationCodeService.GenerateAndSendVerificationCode(model.PhoneNumber);

            return Json(new { });

        }

        public ViewResult SuccessfulSignUp()
        {
            return View();
        }
    }
}