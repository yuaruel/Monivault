using Abp.Application.Services;
using Monivault.AppModels;
using Monivault.OtpSessions.Dto;
using System.Threading.Tasks;

namespace Monivault.OtpSessions
{
    public interface IOtpSessionAppService : IApplicationService
    {
        OtpSessionDto GetOtpSession(string otp);

        Task<OtpSession> ValidateOtp(string otp);
    }
}