using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Monivault.AppModels;

namespace Monivault.DashboardService
{
    public class AccountHolderDashboardService : ITransientDependency
    {
        public IAbpSession AbpSession { get; set; }
        private readonly IRepository<AccountHolder> _accountHolderRepository;

        public AccountHolderDashboardService(
                IRepository<AccountHolder> accountHolderRepository
            )
        {
            AbpSession = NullAbpSession.Instance;
            _accountHolderRepository = accountHolderRepository;
        }

        public decimal GetUserAvailableBalance()
        {
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == AbpSession.UserId);

            return accountHolder.AvailableBalance;
        }
    }
}