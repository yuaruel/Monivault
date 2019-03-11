using System.Threading.Tasks;
using JetBrains.Annotations;
using Monivault.AppModels;

namespace Monivault.Services
{
    public interface IVerificationCodeService
    {
        Task GenerateAndSendVerificationCode(string phoneNumber);
        Task<VerificationCode> GetVerificationCode(int code, string phoneNumber);
    }
}