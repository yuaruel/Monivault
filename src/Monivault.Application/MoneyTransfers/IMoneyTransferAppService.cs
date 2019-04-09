using System.Threading.Tasks;
using Abp.Application.Services;

namespace Monivault.MoneyTransfers
{
    public interface IMoneyTransferAppService : IApplicationService
    {
        Task<string> GenerateBankAccountTransferOtp(decimal amount, string comment, string phoneNumber);

        Task TransferMoneyToBankAccount(string amount, string productCode, string accountNumber, string phoneNumber);
    }
}