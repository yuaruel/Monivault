using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monivault.Controllers;
using Monivault.Tax;
using Monivault.Tax.Dto;
using Monivault.Web.Models.Tax;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Monivault.Web.Controllers
{
    public class TaxController : MonivaultControllerBase
    {
        private readonly ITaxAppService _taxAppService;

        public TaxController(
                ITaxAppService taxAppService
            )
        {
            _taxAppService = taxAppService;
        }

        public async Task<ActionResult> Profile()
        {
            var taxProfile = await _taxAppService.GetTaxProfile();

            var viewModel = ObjectMapper.Map<TaxProfileViewModel>(taxProfile);

            return View(viewModel);
        }

        public async Task<JsonResult> UpdateProfile(TaxProfileViewModel viewModel)
        {
            var taxProfileInput = ObjectMapper.Map<UpdateTaxProfileInput>(viewModel);

            await _taxAppService.UpdateTaxProfile(taxProfileInput);

            return Json(new { });
        }

        public ActionResult Payment()
        {
            return View();
        }

        public async Task<PartialViewResult> TaxPaymentModal()
        {
            var taxProfile = await _taxAppService.GetTaxProfile();
            var taxTypes = _taxAppService.GetTaxTypes();

            var taxTypeSelectItems = new List<SelectListItem>();
            
            foreach(var taxType in taxTypes)
            {
                taxTypeSelectItems.Add(new SelectListItem { Text = taxType.Name, Value = taxType.Id.ToString() });
            }

            var viewModel = new TaxPaymentViewModel
            {
                TaxIdentificationNumber = taxProfile.Tin,
                FullName = taxProfile.FullName,
                TaxTypes = taxTypeSelectItems
            };

            return PartialView("_TaxPaymentModal", viewModel);
        }

        public async Task<JsonResult> MakePayment(TaxPaymentViewModel viewModel)
        {
            var paymentInput = new MakePaymentInput
            {
                Amount = viewModel.Amount,
                FullName = viewModel.FullName,
                TaxIdentificationNumber = viewModel.TaxIdentificationNumber,
                TaxPeriod = viewModel.TaxPeriod,
                TaxType = viewModel.TaxType
            };

            await _taxAppService.MakePayment(paymentInput);

            return Json(new { });
        }

        public async Task<JsonResult> GetTaxPaymentList()
        {
            var taxPayments = await _taxAppService.GetTaxPayments();

            return Json(taxPayments);
        }
    }
}
