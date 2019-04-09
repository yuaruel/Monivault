using Abp.Application.Services;
using Monivault.OtpSessions.Dto;

namespace Monivault.OtpSessions
{
    public interface IOtpSessionAppService : IApplicationService
    {
        OtpSessionDto GetOtpSession(string otp);
    }
}