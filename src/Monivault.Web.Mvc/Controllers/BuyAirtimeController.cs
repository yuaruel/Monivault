using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Monivault.AppModels;
using Monivault.BuyAirtime.Dto;
using Monivault.Controllers;
using Monivault.TopUpAirtime;
using Monivault.Web.Models.BuyAirtime;
using Monivault.WebUtils;

namespace Monivault.Web.Mvc.Controllers
{
    public class BuyAirtimeController : MonivaultControllerBase
    {
        private readonly IBuyAirtimeAppService _buyAirtimeAppService;
        private readonly IRepository<AirtimeNetwork> _airtimeNetworkRepository;

        public BuyAirtimeController(
                IBuyAirtimeAppService buyAirtimeAppService,
                IRepository<AirtimeNetwork> airtimeNetworkRepository
            )
        {
            _buyAirtimeAppService = buyAirtimeAppService;
            _airtimeNetworkRepository = airtimeNetworkRepository;
        }

        public IActionResult Index()
        {
            var airtimeNetworks = _airtimeNetworkRepository.GetAllList();
            var airtimeNetworkItems = new List<SelectListItem>
            {
                new SelectListItem("Select Network", "-1")
            };

            foreach(var airtimeNetwork in airtimeNetworks)
            {
                airtimeNetworkItems.Add(new SelectListItem(airtimeNetwork.NetworkName, airtimeNetwork.OneCardAirtimePurchaseCode));
            }

            var viewModel = new BuyAirtimeViewModel
            {
                AirtimeNetworks = airtimeNetworkItems
            };

            return View(viewModel);
        }

        public async Task<JsonResult> PurchaseAirtime(BuyAirtimeViewModel viewModel)
        {
            var airtimePurchaseDto = new AirtimePurchaseDto
            {
                AirtimeNetwork = viewModel.AirtimeNetwork,
                Amount = viewModel.Amount,
                PhoneNumber = viewModel.PhoneNumber,
                RequestOriginatingPlatform = ClientRequestOriginatingPlatform.Web
            };

            await _buyAirtimeAppService.BuyAirtime(airtimePurchaseDto);

            return Json(new { });
        }
    }
}