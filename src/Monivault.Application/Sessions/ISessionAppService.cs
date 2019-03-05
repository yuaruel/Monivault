using System.Threading.Tasks;
using Abp.Application.Services;
using Monivault.Sessions.Dto;

namespace Monivault.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
