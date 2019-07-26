using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Json;
using Castle.Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Monivault.Exceptions;
using Newtonsoft.Json;
using ILogger = Castle.Core.Logging.ILogger;

namespace Monivault.ModelServices
{
    public class SmsService : ISingletonDependency
    {
        private readonly IConfiguration _configuration;
        private readonly string SmsApiToken = string.Empty;

        public ILogger Logger { get; set; }

        private const string SenderName = "MONIVAULT";
        private const string DndNumber = "5";


        public SmsService(IConfiguration configuration)
        {
            _configuration = configuration;
            SmsApiToken = _configuration.GetValue<string>("SmsApiToken");
            Logger = NullLogger.Instance;
        }

        public async Task SendSms(string message, string recipient)
        {
            //Check that recipient is a valid phone GSM phone number.
            await SendMessage(message, recipient);
        }

        public async Task SendCreditMessage(decimal creditAmount, decimal newBalance, string recipientPhone, string creditType, string transactionDate, int accountHolderId)
        {
            string amountStr = creditAmount.ToString("C2", new CultureInfo("ig-NG"));
            string newBalanceStr = 'N'+ newBalance.ToString("C2", new CultureInfo("ig-NG"));
            var message = $"A credit of N{amountStr} was made to your account on {transactionDate}. {creditType}. Balance: {newBalanceStr}";

            await SendMessage(message, recipientPhone);

            //Log sms usage for account holder.
        }

        public async Task SendDebitMessage(decimal amount, string recipientPhone, string debitType, string transactionDate, int accountHolderId)
        {
            string amountStr = amount.ToString("C2", new CultureInfo("ig-NG"));
            var message = $"A debit of N{amountStr} was done on your account on {transactionDate}. {debitType}";

            await SendMessage(message, recipientPhone);

            //Log sms usage for account holder.
        }

        private async Task SendMessage(string message, string recipientPhone)
        {
            var query = new Dictionary<string, string>
            {
                { "api_token",  SmsApiToken},
                { "from", SenderName },
                { "to", recipientPhone },
                { "body", message },
                { "dnd", DndNumber }
            };

            var content = new FormUrlEncodedContent(query);
            //throw new SmsException("Just throw");
            var httpClient = new HttpClient();

            var response = await httpClient.PostAsync("https://www.bulksmsnigeria.com/api/v1/sms/create", content);

            var responseString = await response.Content.ReadAsStringAsync();

            var respObj = JsonConvert.DeserializeObject<IDictionary<string, object>>(responseString);

            if (respObj.ContainsKey("error"))
            {
                var errorMsg =
                    JsonConvert.DeserializeObject<IDictionary<string, string>>(respObj["error"].ToJsonString())[
                        "message"];
                throw new SmsException(errorMsg);
            }
        }
    }
}