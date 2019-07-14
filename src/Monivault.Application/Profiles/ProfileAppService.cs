using System;
using System.IO;
using System.Threading.Tasks;
using Abp.IdentityFramework;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.AspNetCore.Http;
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
                PhoneNumber = user.PhoneNumber,
                ProfileImageUrl = user.ProfileImageUrl
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

        public async Task UpdateProfilePicture(IFormFile imageFile)
        {
            var currentUser = await GetCurrentUserAsync();

            try
            {
                var profilePictureName = $"profilepicture_{DateTime.UtcNow.Ticks}";
                using (var fileStream = new FileStream(Path.Combine($"wwwroot{Path.DirectorySeparatorChar}", profilePictureName), FileMode.Create))
                {
                    imageFile.CopyTo(fileStream);
                }

                var bucketName = "monivault-temp";
                var regionEndpoint = RegionEndpoint.EUWest1;

                var transferUtitlityRequest = new TransferUtilityUploadRequest();
                transferUtitlityRequest.BucketName = bucketName;
                transferUtitlityRequest.Key = profilePictureName;
                transferUtitlityRequest.CannedACL = S3CannedACL.PublicRead;
                transferUtitlityRequest.FilePath = Path.Combine($"wwwroot{Path.DirectorySeparatorChar}", profilePictureName);

                var transferUtility = new TransferUtility("AKIAXWH2F4YHV3LLGB4X", "WEo4Eh76oFff3OzKopCe7qqtwbfX0R62LMx3dv4a", regionEndpoint);
                transferUtility.Upload(transferUtitlityRequest);

                //Using the various properties, I formed the image URL, based on the AWS format.
                var uploadedFileUrl = $"https://{bucketName}.s3-{regionEndpoint.SystemName}.amazonaws.com/{profilePictureName}";

                currentUser.ProfileImageUrl = uploadedFileUrl;
            }
            catch(Exception exc)
            {
                Logger.Error($"AWS Profile picture upload error: {exc.StackTrace}");
            }

        }
    }
}