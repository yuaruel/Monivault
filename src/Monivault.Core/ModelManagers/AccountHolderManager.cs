using System;
using System.Text;
using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Monivault.Models;

namespace Monivault.ModelManagers
{
    public class AccountHolderManager : DomainService, ITransientDependency
    {
        private readonly IRepository<AccountHolder> _accountHolderRepository;

        public AccountHolderManager(IRepository<AccountHolder> accountHolderRepository)
        {
            _accountHolderRepository = accountHolderRepository;
        }
        
        public void CreateAccountHolder(long userId)
        {
            var accountHolder = new AccountHolder
            {
                AccountHolderKey = Guid.NewGuid(),
                AccountIdentity = GenerateAccountHolderIdentity(),
                AvailableBalance = Decimal.Zero,
                LedgerBalance = Decimal.Zero,
                UserId = userId
            };

            _accountHolderRepository.Insert(accountHolder);
        }
        
        private string GenerateAccountHolderIdentity()
        {
            var identityBuilder = new StringBuilder(6);

            for(var alp = 0; alp < 2; alp++){
                identityBuilder.Append(Convert.ToChar(RandomHelper.GetRandom(97, 122)).ToString().ToUpper());
            }
            
            identityBuilder.Append(new Random().Next(1000, 9999));

            return identityBuilder.ToString();
        }
    }
}