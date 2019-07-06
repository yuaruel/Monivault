using Abp.Application.Services;
using Monivault.Tax.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monivault.Tax
{
    public interface ITaxAppService : IApplicationService
    {
        Task<TaxProfileDto> GetTaxProfile();
        Task UpdateTaxProfile(UpdateTaxProfileInput input);
        List<TaxTypeDto> GetTaxTypes();
        Task MakePayment(MakePaymentInput input);
    }
}
