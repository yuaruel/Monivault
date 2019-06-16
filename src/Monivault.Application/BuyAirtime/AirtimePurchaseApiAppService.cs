using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monivault.BuyAirtime.Dto;

namespace Monivault.BuyAirtime
{
    public class AirtimePurchaseApiAppService : MonivaultAppServiceBase, IAirtimePurchaseApiAppService
    {
        public async Task BuyAirtime(AirtimePurchaseDto input)
        {
            Logger.Info($"The current user Id: {(await GetCurrentUserAsync()).Id}");
        }
    }
}
