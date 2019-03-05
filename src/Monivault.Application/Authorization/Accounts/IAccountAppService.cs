using System.Threading.Tasks;
using Abp.Application.Services;
using Monivault.Authorization.Accounts.Dto;

namespace Monivault.Authorization.Accounts
{
    public interface IAccountAppService : IApplicationService
    {
        Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input);

        Task<RegisterOutput> Register(RegisterInput input);
    }
}
