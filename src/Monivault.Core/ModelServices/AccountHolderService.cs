using System;
using System.Text;
using System.Threading.Tasks;
using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
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
        
        public AccountHolder CreateAccountHolder(long userId)
        {
            try
            {
                var accountHolder = new AccountHolder
                {
                    AvailableBalance = Decimal.Zero,
                    LedgerBalance = Decimal.Zero,
                    UserId = userId
                };

                accountHolder = _accountHolderRepository.Insert(accountHolder);
                CurrentUnitOfWork.SaveChanges();
           
                return accountHolder;
            }
            catch (DbUpdateException duExc)
            {
                Logger.Error(duExc.InnerException.Message);
                throw new UserFriendlyException("Unable to complete your sign up at this time. Please try again later!");
            }
        }
        
    }
}