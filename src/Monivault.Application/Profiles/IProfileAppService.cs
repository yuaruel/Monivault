using System.Threading.Tasks;
using Abp.Application.Services;
using Monivault.Profiles.Dto;

namespace Monivault.Profiles
{
    public interface IProfileAppService : IApplicationService
    {
        Task<ProfileDto> GetUserProfileAsync();
    }
}