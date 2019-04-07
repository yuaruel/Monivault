using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Monivault.AccountHolders.Dto;
using Monivault.AppModels;
using Monivault.Banks.Dto;

namespace Monivault.AccountHolders
{
    [RemoteService(false)]
    public class AccountHolderAppService : MonivaultAppServiceBase, IAccountHolderAppService
    {
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<Bank> _bankRepository;

        public AccountHolderAppService(
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<Bank> bankRepository
            )
        {
            _accountHolderRepository = accountHolderRepository;
            _bankRepository = bankRepository;
        }
        
        public async Task<AccountHolderDto> GetAccountHolderDetail()
        {
            var user = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.FirstOrDefault(p => p.UserId == user.Id);

            if (accountHolder == null)
            {
                return null;
            }

            var accountHolderDto = ObjectMapper.Map<AccountHolderDto>(accountHolder);
            if (accountHolder.BankId.HasValue)
            {
                var bank = _bankRepository.Single(p => p.Id == accountHolder.BankId);
                var bankDto = ObjectMapper.Map<BankDto>(bank);

                accountHolderDto.Bank = bankDto;
            }

            return accountHolderDto;
        }

        public async Task UpdateBankDetails(string bankKey, string accountNumber, string accountName)
        {
            var bankGuidKey = Guid.Parse(bankKey);
            var bank = _bankRepository.Single(p => p.BankKey == bankGuidKey);

            var user = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);

            accountHolder.Bank = bank;
            accountHolder.BankAccountNumber = accountNumber;
            accountHolder.BankAccountName = accountName;
        }
    }
}