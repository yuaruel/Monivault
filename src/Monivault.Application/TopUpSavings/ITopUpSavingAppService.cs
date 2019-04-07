using System.Threading.Tasks;
using Abp.Application.Services;

namespace Monivault.TopUpSavings
{
    public interface ITopUpSavingAppService : IApplicationService
    {
        Task<string> RedeemOneCardPin(string pinno, string comment, string requestPlatform, string platformSpecific);
    }
}