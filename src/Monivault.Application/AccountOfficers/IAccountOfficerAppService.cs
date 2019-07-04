using Abp.Application.Services;
using Monivault.AccountOfficers.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monivault.AccountOfficers
{
    public interface IAccountOfficerAppService : IApplicationService
    {
        Task<List<AccountOfficerDto>> GetAccountOfficers();
    }
}
