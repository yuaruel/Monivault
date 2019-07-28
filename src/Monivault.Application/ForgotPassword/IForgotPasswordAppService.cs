using Abp.Application.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Monivault.ForgotPassword
{
    public interface IForgotPasswordAppService : IApplicationService
    {
        Task<bool> ValidateEmailOrPhoneNumber(string emailOrPhoneNumber);
        Task<bool> ResetPassword(string passwordResetToken);
    }
}
