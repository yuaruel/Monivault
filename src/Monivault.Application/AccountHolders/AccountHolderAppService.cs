using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Domain.Repositories;
using Monivault.AccountHolders.Dto;
using Monivault.AppModels;
using Monivault.Banks.Dto;
using Monivault.Exceptions;

namespace Monivault.AccountHolders
{
    [RemoteService(false)]
    public class AccountHolderAppService : MonivaultAppServiceBase, IAccountHolderAppService
    {
        private readonly IRepository<AccountHolder> _accountHolderRepository;
        private readonly IRepository<Bank> _bankRepository;
        private readonly IRepository<SavingsInterest, long> _savingInterestRepository;

        public AccountHolderAppService(
                IRepository<AccountHolder> accountHolderRepository,
                IRepository<Bank> bankRepository,
                IRepository<SavingsInterest, long> savingInterestRepository
            )
        {
            _accountHolderRepository = accountHolderRepository;
            _bankRepository = bankRepository;
            _savingInterestRepository = savingInterestRepository;
        }

        public BalanceDto GetAccountHolderBalance()
        {
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == AbpSession.UserId);

            var balanceDto = new BalanceDto
            {
                AvailableBalance = accountHolder.AvailableBalance,
                LedgerBalance = accountHolder.LedgerBalance
            };

            return balanceDto;
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

        public AccountHolderDto GetAccountHolderDetailByUserId(long userId)
        {
            var accountHolder = _accountHolderRepository.FirstOrDefault(p => p.UserId == userId);

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

        public decimal GetInterestAccrued()
        {
            var accruedInterest = 0.0m;
            try
            {
                var accountHolder = _accountHolderRepository.Single(p => p.UserId == AbpSession.UserId);
                var savingsInterest = _savingInterestRepository.FirstOrDefault(p => p.Status == SavingsInterest.StatusTypes.Running && p.AccountHolderId == accountHolder.Id);

                if(savingsInterest != null)
                {
                    accruedInterest = savingsInterest.InterestAccrued;
                }
            }
            catch(InvalidOperationException ioExc)
            {
                Logger.Error($"Interest accrued exception: {ioExc.StackTrace}");
            }

            return accruedInterest;
        }

        public int GetTotalNumberOfAccountHolders()
        {
            return _accountHolderRepository.Count();
        }

        /// <summary>
        /// Checks if the account holder available balance is more than the amount required for the transaction.
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task<bool> IsAvailableBalanceEnough(decimal amount)
        {
            var user = await GetCurrentUserAsync();
            var accountHolder = await _accountHolderRepository.SingleAsync(p => p.UserId == user.Id);

            if (accountHolder.AvailableBalance <= amount) throw new InsufficientBalanceException();

            return true;
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