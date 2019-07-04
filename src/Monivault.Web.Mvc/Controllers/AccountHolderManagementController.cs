using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileProviders;
using Monivault.AccountHolders;
using Monivault.AccountHolders.Dto;
using Monivault.AccountOfficers;
using Monivault.Authorization;
using Monivault.Authorization.Roles;
using Monivault.Controllers;
using Monivault.Net.MimeTypes;
using Monivault.Web.Models.AccountHolderManagement;

namespace Monivault.Web.Mvc.Controllers
{
    public class AccountHolderManagementController : MonivaultControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly IAccountHolderAppService _accountHolderAppService;
        private readonly IAccountOfficerAppService _accountOfficerAppService;

        public AccountHolderManagementController(
                IFileProvider fileProvider,
                IAccountHolderAppService accountHolderAppService,
                IAccountOfficerAppService accountOfficerAppService
            )
        {
            _fileProvider = fileProvider;
            _accountHolderAppService = accountHolderAppService;
            _accountOfficerAppService = accountOfficerAppService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Profile(string id)
        {
            var accountHolderProfile = await _accountHolderAppService.GetAccountHolderProfile(id);

            var viewModel = new ProfileViewModel
            {
                AccountHolderKey = accountHolderProfile.AccountHolderKey,
                IdentityNumber = accountHolderProfile.IdentityNumber,
                FullName = accountHolderProfile.FullName,
                PhoneNumber = accountHolderProfile.PhoneNumber,
                EmailAddress = accountHolderProfile.EmailAddress,
                DateJoined = accountHolderProfile.DateJoined,
                AccountOfficerName = accountHolderProfile.AccountOfficerName
            };

            return View(viewModel);
        }

        public FileResult DownloadAccountHolderUploadSampleFile()
        {
            var fileInfo = _fileProvider.GetFileInfo("wwwroot/AccountHolderUploadSample.xlsx");

            var physicalFileResult = new PhysicalFileResult(fileInfo.PhysicalPath, MimeTypeNames.ApplicationOctetStream)
            {
                FileDownloadName = "AccountHolderUploadFileSample.xlsx"
            };

            return physicalFileResult;
        }

        [HttpPost]
        public async Task<IActionResult> UploadAccountHolders(IFormFile uploadedFile)
        {

            await _accountHolderAppService.UploadAccountHolders(uploadedFile);
            return Json(new { });
        }

        public async Task<JsonResult> AccountHolderList()
        {
            var accountHolderList = await _accountHolderAppService.GetAccountHolderList();

            return Json(accountHolderList);
        }

        [AbpMvcAuthorize(PermissionNames.ChangeAccountOfficer)]
        public async Task<ActionResult> EditAccountHolderModal(string accountHolderKey)
        {
            var profileDto = _accountHolderAppService.GetAccountHolderProfileForEdit(accountHolderKey);
            var accountOfficers = await _accountOfficerAppService.GetAccountOfficers();

            var viewModel = new EditAccountHolderModalViewModel
            {
                AccountHolderKey = profileDto.AccountHolderKey,
                FirstName = profileDto.FirstName,
                LastName = profileDto.LastName,
                PhoneNumber = profileDto.PhoneNumber,
                EmailAddress = profileDto.EmailAddress,
                AccountOfficer = profileDto.AccountOfficerId?.ToString(),
                AccountOfficers = new List<SelectListItem>()
            };

            //Add a default SelectListItem
            viewModel.AccountOfficers.Add(new SelectListItem { Text = "Select Account Officer", Value = "-1" });
            foreach(var accountOfficer in accountOfficers)
            {
                viewModel.AccountOfficers.Add(new SelectListItem { Text = $"{accountOfficer.FirstName} {accountOfficer.LastName}", Value = accountOfficer.AccountOfficerId.ToString() });
            }

            return PartialView("_EditAccountHolderModal", viewModel);
        }

        public JsonResult UpdateAccountHolder(EditAccountHolderModalViewModel viewModel)
        {
            var profileDto = new AccountHolderEditProfileDto
            {
                AccountHolderKey = viewModel.AccountHolderKey,
                AccountOfficerId = long.Parse(viewModel.AccountOfficer)
            };

            _accountHolderAppService.UpdateAccountHolder(profileDto);

            return Json(new { });
        }
    }
}