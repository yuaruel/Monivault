using System.Threading.Tasks;
using Abp.Application.Services;
using Monivault.AccountHolders.Dto;

namespace Monivault.AccountHolders
{
    public interface IAccountHolderAppService : IApplicationService
    {
        Task<AccountHolderDto> GetAccountHolderDetail();

        AccountHolderDto GetAccountHolderDetailByUserId(long userId);

        Task UpdateBankDetails(string bankKey, string accountNumber, string accountName);

        int GetTotalNumberOfAccountHolders();

        Task<bool> IsAvailableBalanceEnough(decimal amount);

        decimal GetInterestAccrued();

        BalanceDto GetAccountHolderBalance();
    }
}