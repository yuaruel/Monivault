using System.Threading.Tasks;
using JetBrains.Annotations;
using Monivault.Models;

namespace Monivault.Services
{
    public interface IVerificationCodeService
    {
        Task GenerateAndSendVerificationCode(string phoneNumber);
        Task<VerificationCode> GetVerificationCode(int code, string phoneNumber);
    }
}