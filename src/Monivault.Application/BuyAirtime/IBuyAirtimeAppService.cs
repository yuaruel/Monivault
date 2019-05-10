using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace Monivault.TopUpAirtime
{
    public interface IBuyAirtimeAppService : IApplicationService
    {
        void BuyAirtime();
    }
}
