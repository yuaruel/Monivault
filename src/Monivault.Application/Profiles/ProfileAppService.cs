using System.Threading.Tasks;
using Abp.IdentityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Monivault.Authorization.Users;
using Monivault.Profiles.Dto;
using Monivault.Utils;

namespace Monivault.Profiles
{
    public class ProfileAppService : MonivaultAppServiceBase, IProfileAppService
    {
        private readonly UserManager _userManager;
        
        public ProfileAppService(
                UserManager userManager
            )
        {
            _userManager = userManager;
        }
        
        public async Task<ProfileDto> GetUserProfileAsync()
        {
            var user = await GetCurrentUserAsync();

            var profileDto = new ProfileDto
            {
                Email = user.RealEmailAddress,
                FirstName = user.Name,
                LastName = user.Surname,
                PhoneNumber = user.PhoneNumber
            };

            return profileDto;
        }

        public async Task UpdatePersonalDetail(PersonalDetailDto input)
        {
            var user = await GetCurrentUserAsync();
            user.Name = input.FirstName;
            user.Surname = input.LastName;
            user.PhoneNumber = input.PhoneNumber;
            user.EmailAddress = input.Email ?? RandomStringGeneratorUtil.GenerateFakeEmail();

            await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> UpdatePassword(UpdatePasswordDto input)
        {
            var user = await GetCurrentUserAsync();

            var updateResult = await _userManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword);
            
            foreach (var error in updateResult.Errors)
            {
                Logger.Info($"Error Code: {error.Code}");
                Logger.Info($"Error Description: {error.Description}");
            }

            return updateResult;
        }
    }
}