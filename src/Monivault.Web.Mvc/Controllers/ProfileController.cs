using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monivault.AccountHolders;
using Monivault.AppModels;
using Monivault.Controllers;
using Monivault.Profiles;
using Monivault.Profiles.Dto;
using Monivault.Web.Models.Profile;

namespace Monivault.Web.Controllers
{
    public class ProfileController : MonivaultControllerBase
    {
        private readonly IProfileAppService _profileAppService;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IAccountHolderAppService _accountHolderAppService;
        
        public ProfileController(
                IProfileAppService profileAppService,
                IRepository<Bank> bankRepository,
                IAccountHolderAppService accountHolderAppService
            )
        {
            _profileAppService = profileAppService;
            _bankRepository = bankRepository;
            _accountHolderAppService = accountHolderAppService;
        }
        
        // GET
        public async Task<IActionResult> Index()
        {
            var profileDto = await _profileAppService.GetUserProfileAsync();
            var accountHolderDto = await _accountHolderAppService.GetAccountHolderDetail();
            
            
            var banks = _bankRepository.GetAllList();
            var bankSelectItems = new List<SelectListItem>
            {
                new SelectListItem { Value = "-1", Text = "Select Bank" }
            };

            foreach (var bank in banks)
            {
                bankSelectItems.Add(new SelectListItem{Value = bank.BankKey.ToString(), Text = bank.Name});
            }

            var viewModel = new ProfileViewModel
            {
                Email = profileDto.Email,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                PhoneNumber = profileDto.PhoneNumber,
                Bank = "-1",
                Banks = bankSelectItems,
                AccountNumber = accountHolderDto.BankAccountNumber,
                AccountName = accountHolderDto.BankAccountName
            };
            
            if (accountHolderDto.Bank != null)
            {
                Logger.Info("account holder bank: " + accountHolderDto.Bank.BankKey);
                viewModel.Bank = accountHolderDto.Bank.BankKey.ToString();
            }
            
            return View(viewModel);
        }

        public JsonResult UpdateBankAccountDetail([FromBody]BankAccountViewModel viewModel)
        {
            _accountHolderAppService.UpdateBankDetails(viewModel.BankName, viewModel.AccountNumber,
                viewModel.AccountName);
            
            return Json(new { });
        }

        public async Task<StatusCodeResult> UpdatePersonalDetail(PersonalDetailViewModel viewModel)
        {
            await _profileAppService.UpdatePersonalDetail(new PersonalDetailDto
            {
                FirstName = viewModel.FirstName,
                LastName = viewModel.LastName,
                PhoneNumber = viewModel.PhoneNumber,
                Email = viewModel.Email
            });
            
            return StatusCode(200);
        }

        [DontWrapResult]
        public async Task<ActionResult> UpdatePassword(UpdatePasswordViewModel viewModel)
        {
            var updateResult = await _profileAppService.UpdatePassword(new UpdatePasswordDto
            {
                CurrentPassword = viewModel.CurrentPassword,
                NewPassword = viewModel.NewPassword
            });

            if (updateResult.Succeeded) return Ok();

            
            var errorDesc = "";
            foreach (var error in updateResult.Errors)
            {
                errorDesc = error.Description;
                //if (error.Code == "PasswordMismatch") throw new UserFriendlyException(error.Description);
            }

            return BadRequest(errorDesc);

            //return StatusCode(200);
        }
    }
}