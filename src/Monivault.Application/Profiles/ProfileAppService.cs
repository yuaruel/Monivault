using System.Threading.Tasks;
using Monivault.Profiles.Dto;

namespace Monivault.Profiles
{
    public class ProfileAppService : MonivaultAppServiceBase, IProfileAppService
    {
        public ProfileAppService()
        {
            
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
    }
}