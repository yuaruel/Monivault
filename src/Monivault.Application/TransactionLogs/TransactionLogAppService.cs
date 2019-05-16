using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Monivault.AppModels;
using Monivault.TransactionLogs.Dto;

namespace Monivault.TransactionLogs
{
    public class TransactionLogAppService : MonivaultAppServiceBase, ITransactionLogAppService
    {
        private readonly IRepository<TransactionLog, long> _transactionRepository;
        private readonly IRepository<AccountHolder> _accountHolderRepository;

        public TransactionLogAppService(
                IRepository<TransactionLog, long> transactionRepository,
                IRepository<AccountHolder> accountHolderRepository
            )
        {
            _transactionRepository = transactionRepository;
            _accountHolderRepository = accountHolderRepository;
        }
        
        public async Task<List<RecentTransactionDto>> GetRecentTransactions(LimitedResultRequestDto requestDto)
        {
            var user = await GetCurrentUserAsync();
            var accountHolder = _accountHolderRepository.Single(p => p.UserId == user.Id);

            var transactionLogs = _transactionRepository.GetAllList(p => p.AccountHolderId == accountHolder.Id).OrderByDescending(p => p.CreationTime)
                                    .Take(requestDto.MaxResultCount).ToList();
            return ObjectMapper.Map<List<RecentTransactionDto>>(transactionLogs);
        }

        

        public int GetTotalDeposits()
        {
            //_transactionRepository.
            return 0;
        }

        public void LogTransaction()
        {
            throw new System.NotImplementedException();
        }
    }
}