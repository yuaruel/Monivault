using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Monivault.AppModels;

namespace Monivault.Services
{
    public class VerificationCodeService : MonivaultAppServiceBase, IVerificationCodeService
    {
        private readonly IRepository<VerificationCode> _verificationCodeRepository;
        private readonly SmsService _smsService;

        public VerificationCodeService(IRepository<VerificationCode> verificationCodeRepository,
                                        SmsService smsService)
        {
            _verificationCodeRepository = verificationCodeRepository;
            _smsService = smsService;
        }
        
        public async Task GenerateAndSendVerificationCode(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                //User is signed in, and just needs verification code to cary out transaction.
                var currentUserTask = await GetCurrentUserAsync();
                phoneNumber = currentUserTask.PhoneNumber;
            }
            
            var verificationCode = RandomHelper.GetRandom(10000, 99999);
            
            var signUpVerificationCode = new VerificationCode()
            {
                Id = verificationCode,
                PhoneNumber = phoneNumber
            };
            
            _verificationCodeRepository.Insert(signUpVerificationCode);
            
            _smsService.SendSms("Monivault VC: " + verificationCode, phoneNumber);
        }

        public async Task<VerificationCode> GetVerificationCode(int code, string phoneNumber)
        {
            var verificationCode = await _verificationCodeRepository.FirstOrDefaultAsync(p => p.Id == code && p.PhoneNumber == phoneNumber);

            return verificationCode;
        }
    }
}