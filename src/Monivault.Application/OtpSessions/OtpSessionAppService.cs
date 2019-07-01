using Abp.Domain.Repositories;
using Monivault.AppModels;
using Monivault.Exceptions;
using Monivault.OtpSessions.Dto;
using System.Threading.Tasks;

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

        public async Task<OtpSession> ValidateOtp(string otp)
        {
            var user = await GetCurrentUserAsync();

            var otpSession = _otpSessionRepository.Get(int.Parse(otp));
            _otpSessionRepository.Delete(otpSession);

            if (otpSession.PhoneNumberSentTo != user.PhoneNumber) throw new InvalidOtpException();

            return otpSession;
        }
    }
}