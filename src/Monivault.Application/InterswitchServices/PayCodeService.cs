using System;
using Abp.Dependency;
using Monivault.Utils;

namespace Monivault.InterswitchServices
{
    public class PayCodeService : MonivaultServiceBase, ITransientDependency
    {
        public void ProcessPayCode()
        {
            GenerateHeaders("https://sandbox.interswitchng.com/api/v1/pwm/subscribers/2348108309180/tokens");
        }

        private void GenerateHeaders(string tokenUrl)
        {
            Logger.Info($"Timestamp component: {DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}");
            Logger.Info($"Nonce: {RandomStringGeneratorUtil.GenerateNonce()}");
        }
    }
}