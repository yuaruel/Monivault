using System;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Monivault.AppModels;
using Monivault.Utils;

namespace Monivault.ModelServices
{
    public class AccountHolderService : DomainService, ITransientDependency
    {
        private readonly IRepository<AccountHolder> _accountHolderRepository;

        public AccountHolderService(IRepository<AccountHolder> accountHolderRepository)
        {
            _accountHolderRepository = accountHolderRepository;
        }
        
        public async Task<AccountHolder> CreateAccountHolder(long userId)
        {
            var accountHolder = new AccountHolder
            {
                AvailableBalance = Decimal.Zero,
                LedgerBalance = Decimal.Zero,
                UserId = userId
            };

            accountHolder = await _accountHolderRepository.InsertAsync(accountHolder);

            return accountHolder;
        }
        
    }
}