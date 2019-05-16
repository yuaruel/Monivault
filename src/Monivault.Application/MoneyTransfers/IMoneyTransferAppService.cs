using System.Threading.Tasks;
using Abp.Application.Services;
using Monivault.MoneyTransfers.Dto;

namespace Monivault.MoneyTransfers
{
    public interface IMoneyTransferAppService : IApplicationService
    {
        Task<string> GenerateBankAccountTransferOtp(decimal amount, string comment, string phoneNumber);

        Task TransferMoneyToAccountHolderBankAccount(TransferMoneyToAccountInput input);
    }
}