using Abp.Application.Services;
using Monivault.BuyAirtime.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monivault.BuyAirtime
{
    public interface IAirtimePurchaseApiAppService : IApplicationService
    {
        Task BuyAirtime(AirtimePurchaseDto input);
    }
}
