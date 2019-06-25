using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Monivault.AccountHolders;
using Monivault.Controllers;
using Monivault.Net.MimeTypes;

namespace Monivault.Web.Mvc.Controllers
{
    public class AccountHolderManagementController : MonivaultControllerBase
    {
        private readonly IFileProvider _fileProvider;
        private readonly IAccountHolderAppService _accountHolderAppService;

        public AccountHolderManagementController(
                IFileProvider fileProvider,
                IAccountHolderAppService accountHolderAppService
            )
        {
            _fileProvider = fileProvider;
            _accountHolderAppService = accountHolderAppService;
        }

        public IActionResult Index()
        {
            return View();
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
    }
}