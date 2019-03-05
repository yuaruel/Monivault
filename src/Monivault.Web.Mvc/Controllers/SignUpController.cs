using System;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Monivault.Authorization.Users;
using Monivault.Controllers;
using Monivault.Services;
using Monivault.SignUp;
using Monivault.Web.Models.Login;

namespace Monivault.Web.Controllers
{
    public class SignUpController : MonivaultControllerBase
    {
        private readonly IRepository<SignUpVerificationCode> _verificationCodeRepository;
        private readonly SMSService _smsService;
        private readonly UserManager _userManager;
        
        public SignUpController(
            IRepository<SignUpVerificationCode> verificationCodeRepository,
            SMSService smsService,
            UserManager userManager)
        {
            _verificationCodeRepository = verificationCodeRepository;
            _smsService = smsService;
            _userManager = userManager;
        }
        // GET
        public IActionResult Index()
        {
            return
            View();
        }

        [HttpPost]
        public IActionResult SignUpAccountHolder()
        {
            return Json(new {});
        }

        public async Task<JsonResult> SendUpVerificationCode(ContactDetailViewModel model)
        {
            var signupUser = await _userManager.FindByPhoneNumber(model.PhoneNumber);
            if (signupUser == null)
            {
                //TODO Check if the user entered email address. If yes, check if the email has already been used.
            
                var verificationCode = RandomHelper.GetRandom(10000, 99999);
            
                var signupVerificationCode = new SignUpVerificationCode()
                {
                    Id = verificationCode,
                    PhoneNumber = model.PhoneNumber
                };
                _verificationCodeRepository.Insert(signupVerificationCode);
            
                _smsService.SendSms("Monivault VC: " + verificationCode, model.PhoneNumber);
            
                return Json(new { status = "Very Successful" });   
            }
            else
            {
                throw new UserFriendlyException(L("UserAlreadyExist"));
            }
        }
    }
}