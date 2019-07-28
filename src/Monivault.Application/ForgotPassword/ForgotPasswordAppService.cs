using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Monivault.AppModels;
using Monivault.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Monivault.Configuration;
using Abp.BackgroundJobs;
using Monivault.BackgroundJobs.Email;
using Monivault.Authorization.Users;

namespace Monivault.ForgotPassword
{
    public class ForgotPasswordAppService : MonivaultAppServiceBase, IForgotPasswordAppService
    {
        private readonly IRepository<ResetPasswordToken, string> _passwordTokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly UserManager _userManager;

        public ForgotPasswordAppService(
                IRepository<ResetPasswordToken, string> passwordTokenRepository,
                IConfiguration configuration,
                IBackgroundJobManager backgroundJobManager,
                UserManager userManager
            )
        {
            _passwordTokenRepository = passwordTokenRepository;
            _configuration = configuration;
            _backgroundJobManager = backgroundJobManager;
            _userManager = userManager;
        }

        public async Task<bool> ResetPassword(string resetToken)
        {
            try
            {
                var passwordResetToken = _passwordTokenRepository.Get(resetToken);

                var user = await _userManager.FindByIdAsync(passwordResetToken.UserId.ToString());
                var tempPassword = RandomStringGeneratorUtil.GeneratePassword();
                await _userManager.ChangePasswordAsync(user, tempPassword);

                //Delete password reset token
                _passwordTokenRepository.Delete(passwordResetToken);

                await _userManager.UpdateAsync(user);

                //Schedule Email to user
                await _backgroundJobManager.EnqueueAsync<TemporaryPasswordCreatedEmailJob, TemporaryPasswordCreatedEmailJobArgs>(new TemporaryPasswordCreatedEmailJobArgs
                {
                    UserEmail = user.EmailAddress,
                    TemporaryPassword = tempPassword
                });
            }
            catch(EntityNotFoundException enfExc)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> ValidateEmailOrPhoneNumber(string emailOrPhoneNumber)
        {
            var user = await UserManager.FindByEmailAsync(emailOrPhoneNumber);

            if (null == user) throw new EntityNotFoundException("Invalid email");

            //Create resetpasswordtoken.
            var passwordResetToken = RandomStringGeneratorUtil.GeneratePasswordResetToken();
            var siteUrl = new UriBuilder(_configuration.GetSection("App").GetValue<string>("WebSiteRootAddress"));
            siteUrl.Path = "/ResetPassword";
            siteUrl.Query = $"ptk={passwordResetToken}";

            _passwordTokenRepository.Insert(new ResetPasswordToken
            {
                Id = passwordResetToken,
                UserId = user.Id,
                EmailOrPhoneNumber = emailOrPhoneNumber
            });
            var url = siteUrl;

            //Schedule email to user
            await _backgroundJobManager.EnqueueAsync<PasswordResetLinkEmailJob, PasswordResetLinkEmailJobArgs>(new PasswordResetLinkEmailJobArgs
            {
                Email = emailOrPhoneNumber,
                ResetLink = siteUrl.ToString()
            });

            return true;
        }
    }
}
