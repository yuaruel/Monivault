using System;
using System.Text;
using Abp;

namespace Monivault.Utils
{
    public static class RandomStringGeneratorUtil
    {
        public static string GenerateAgentTransactionId()
        {
            var transactionIdBuilder = new StringBuilder(7);
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

        public static string GenerateNonce()
        {
            var randomizer = new Random();
            var randomCharacterBuilder = new StringBuilder(25);

            for (var i = 0; i < 25; i++)
            {
                var caseSelector = randomizer.Next(1, 4);

                switch (caseSelector)
                {
                    case 1:
                        randomCharacterBuilder.Append(Convert.ToChar(randomizer.Next(48, 58)));
                        break;
                    case 2:
                        randomCharacterBuilder.Append(Convert.ToChar(randomizer.Next(65, 91)));
                        break;
                    case 3:
                        randomCharacterBuilder.Append(Convert.ToChar(randomizer.Next(97, 123)));
                        break;
                }   
            }

            return randomCharacterBuilder.ToString();
        }

        public static string GenerateFakeEmail()
        {
            return Guid.NewGuid() + "@fakeemailforapp.com";
        }

        public static int GenerateOtp()
        {
            return RandomHelper.GetRandom(10000, 99999);
        }
    }
}