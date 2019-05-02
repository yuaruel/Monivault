using System.Threading.Tasks;
using Abp.Application.Services;
using Microsoft.AspNetCore.Identity;
using Monivault.Profiles.Dto;

namespace Monivault.Profiles
{
    public interface IProfileAppService : IApplicationService
    {
        Task<ProfileDto> GetUserProfileAsync();

        Task UpdatePersonalDetail(PersonalDetailDto input);

        Task<IdentityResult> UpdatePassword(UpdatePasswordDto input);
    }
}