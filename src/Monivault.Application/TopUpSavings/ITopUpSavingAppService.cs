using Abp.Application.Services;

namespace Monivault.TopUpSavings
{
    public interface ITopUpSavingAppService : IApplicationService
    {
        void RedeemOneCardPin(string pinno, string comment, string requestPlatform, string platformSpecific);
    }
}