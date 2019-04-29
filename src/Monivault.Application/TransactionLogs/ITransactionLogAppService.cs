using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Monivault.TransactionLogs.Dto;

namespace Monivault.TransactionLogs
{
    public interface ITransactionLogAppService : IApplicationService
    {
        Task<List<RecentTransactionDto>> GetRecentTransactions(LimitedResultRequestDto requestDto);

        int GetTotalDeposits();
    }
}