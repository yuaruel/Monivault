using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using EstelServicesServiceReference;
using Monivault.AppModels;
using Monivault.Exceptions;
using Monivault.Utils;

namespace Monivault.MoneyTransfers
{
    public class MoneyTransferAppService : MonivaultAppServiceBase, IMoneyTransferAppService
    {
        private readonly IRepository<OtpSession> _otpRepository;

        public MoneyTransferAppService(
                IRepository<OtpSession> otpRepository
            )
        {
            _otpRepository = otpRepository;
        }
        
        public async Task<string> GenerateBankAccountTransferOtp(decimal amount, string comment, string phoneNumber)
        {
            var activityProperty = new Dictionary<string, string>();
            activityProperty.Add("Amount", amount.ToString());
            activityProperty.Add("Comment", comment);

            var otp = RandomStringGeneratorUtil.GenerateOtp();
            
            var otpSession = new OtpSession
            {
                User = await GetCurrentUserAsync(),
                Id = otp,
                ActionProperty = activityProperty,
                PhoneNumberSentTo = phoneNumber
            };

            _otpRepository.Insert(otpSession);

            return otp.ToString();
        }

        public async Task TransferMoneyToBankAccount(string amount, string productCode, string accountNumber, string phoneNumber)
        {
            var estelClient = new EstelServicesClient();

            Logger.Info("amount to transfer: " + amount);

            var fundsTransferRequest = new FundsTransferRequest();

            fundsTransferRequest.amount = amount;
            fundsTransferRequest.mpin = "7F1359753577B274D717DC2E41BA1E51";
            fundsTransferRequest.agentCode = "APEX_PINRDM";
            fundsTransferRequest.destination = accountNumber;
            fundsTransferRequest.mobilenumber = phoneNumber;
            fundsTransferRequest.productCode = productCode;

            var fundsTransferResponse = (await estelClient.getFundsTransferAsync(fundsTransferRequest)).Body.getFundsTransferReturn;

            switch (fundsTransferResponse.resultcode)
            {
                case "0":
                    Logger.Info("destination: " + fundsTransferResponse.destination);
                    Logger.Info("bank code: "+ fundsTransferResponse.productcode);
                    Logger.Info("reason: " + fundsTransferResponse.reason);
                    Logger.Info("result code: " + fundsTransferResponse.resultcode);
                    Logger.Info("description: " + fundsTransferResponse.resultdescription);
                    break;
                
                default:
                    Logger.Info("destination: " + fundsTransferResponse.destination);
                    Logger.Info("bank code: "+ fundsTransferResponse.productcode);
                    Logger.Info("reason: " + fundsTransferResponse.reason);
                    Logger.Info("result code: " + fundsTransferResponse.resultcode);
                    Logger.Info("description: " + fundsTransferResponse.resultdescription);
                    throw new MoneyTransferException("Error completing request");
            }
        }
    }
}