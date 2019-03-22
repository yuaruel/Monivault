using System;
using System.Text;
using Abp;

namespace Monivault.Utils
{
    public static class RandomStringGeneratorUtil
    {
        public static string GenerateAgentTransactionId()
        {
            var transactionIdBuilder = new StringBuilder(15);
            var random = new Random();
            
            for (var cnt = 0; cnt < 3; cnt++)
            {
                transactionIdBuilder.Append(random.Next(10000, 99999));
            }
        
            return transactionIdBuilder.ToString();
        }
        
        public static string GenerateAccountHolderIdentity()
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