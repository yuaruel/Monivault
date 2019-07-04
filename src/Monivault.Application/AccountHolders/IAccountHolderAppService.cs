using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Microsoft.AspNetCore.Http;
using Monivault.AccountHolders.Dto;

namespace Monivault.AccountHolders
{
    public interface IAccountHolderAppService : IApplicationService
    {
        Task<AccountHolderDto> GetAccountHolderDetail();

        AccountHolderDto GetAccountHolderDetailByUserId(long userId);

        Task<AccountHolderProfileDto> GetAccountHolderProfile(string key);

        AccountHolderEditProfileDto GetAccountHolderProfileForEdit(string key);

        Task UpdateBankDetails(string bankKey, string accountNumber, string accountName);

        void UpdateAccountHolder(AccountHolderEditProfileDto profileDto);

        int GetTotalNumberOfAccountHolders();

        Task<bool> IsAvailableBalanceEnough(decimal amount);

        decimal GetInterestAccrued();

        BalanceDto GetAccountHolderBalance();

        Task UploadAccountHolders(IFormFile uploadFile);

        Task<List<AccountHolderListDto>> GetAccountHolderList();

        Task<decimal> GetInterestReceivedForCurrentYear();
    }
}