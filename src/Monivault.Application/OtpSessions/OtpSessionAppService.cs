using Abp.Domain.Repositories;
using Monivault.AppModels;
using Monivault.OtpSessions.Dto;

namespace Monivault.OtpSessions
{
    public class OtpSessionAppService : MonivaultAppServiceBase, IOtpSessionAppService
    {
        private readonly IRepository<OtpSession> _otpSessionRepository;

        public OtpSessionAppService(
                IRepository<OtpSession> otpSessionRepository
            )
        {
            _otpSessionRepository = otpSessionRepository;
        }
        
        public OtpSessionDto GetOtpSession(string otp)
        {
            var otpSession = _otpSessionRepository.Get(int.Parse(otp));

            //Delete OtpSession immediately.
            _otpSessionRepository.Delete(otpSession);
            
            return ObjectMapper.Map<OtpSessionDto>(otpSession);
        }
    }
}