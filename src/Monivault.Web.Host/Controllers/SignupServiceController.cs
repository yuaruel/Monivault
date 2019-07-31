using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Monivault.AppModels;
using Monivault.Authorization.Users;
using Monivault.Configuration;
using Monivault.Controllers;
using Monivault.Exceptions;
using Monivault.ModelServices;
using Monivault.SavingsInterests;
using Monivault.Web.Models;

namespace Monivault.Web.Host.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SignUpServiceController : MonivaultControllerBase
    {
        private readonly IVerificationCodeService _verificationCodeService;
        private readonly SmsService _smsService;
        private readonly UserSignUpManager _userSignUpManager;
        private readonly UserManager _userManager;
        private readonly AccountHolderService _accountHolderService;
        private readonly NotificationScheduler _notificationScheduler;
        private readonly SavingsInterestManager _savingsInterestManager;

        public SignUpServiceController(
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

        [HttpPost]
        public async Task<JsonResult> SignUpAccountHolder([FromBody] SignUpAccountHolderViewModel model)
        {
            var signupDisabled = bool.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.StopSignUp));

            if (signupDisabled) throw new UserFriendlyException("SignupDisbaled");

            var verificationCode = await _verificationCodeService.GetVerificationCode(int.Parse(model.VerificationCode), model.PhoneNumber);
            if (verificationCode == null) throw new UserFriendlyException("InvalidVerificationCode");

            //Delete verificationCode.
            _verificationCodeService.ClearVerificationCode(verificationCode);

            try
            {
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
            }
            catch(DuplicateUserNameException dunExc)
            {
                throw new UserFriendlyException("DuplicateUserName");
            }

            return Json(new { });
        }

        [HttpPost]
        public async Task<JsonResult> SendVerificationCode([FromBody] ContactDetailViewModel model)
        {
            var signupDisabled = bool.Parse(await SettingManager.GetSettingValueAsync(AppSettingNames.StopSignUp));

            if (signupDisabled) throw new UserFriendlyException("SignupDisbaled");

            //TODO I'll move this code into a central place later, so that the web and the API use same logic.
            //Check if any user exist that already used the phone number.
            var signUpUser = await _userManager.FindByPhoneNumberAsync(model.PhoneNumber);
            if (signUpUser != null) throw new UserFriendlyException("DuplicatePhone");

            //Check if user supplied email address, if yes, check if a user exist that already used the email.
            if (!string.IsNullOrEmpty(model.Email))
            {
                signUpUser = await _userManager.FindByEmailAsync(model.Email);
                if (signUpUser != null) throw new UserFriendlyException("DuplicateEmail");
            }

            //If user exists with either phone number or email end verification sending.
            //if (signUpUser != null) throw new UserFriendlyException(L("UserAlreadyExist"));

            await _verificationCodeService.GenerateAndSendVerificationCode(model.PhoneNumber);

            return Json(new { });

        }
    }
}