using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Abp.Dependency;
using Abp.Domain.Services;
using Abp.Json;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Monivault.Exceptions;
using Newtonsoft.Json;
using ILogger = Castle.Core.Logging.ILogger;

namespace Monivault.ModelServices
{
    public class SmsService : ISingletonDependency
    {
        public ILogger Logger { get; set; }
        
        public SmsService()
        {
            Logger = NullLogger.Instance;
        }

        public async Task SendSms(string message, string recipient)
        {
            /*var urlBuilder = new UriBuilder("ttps://www.bulksmsnigeria.com/api/v1/sms/create");

            var query = HttpUtility.ParseQueryString(urlBuilder.Query);
            query["api_token"] = "YUqX8aO6YPsdYwcidhnVTkzSyYbTZPdlhVzLhBpqmApQNnEKH9vYVgacNBpI";
            query["from"] = "MONIVAULT";
            query["to"] = recipient;
            query["body"] = message;
            query["dnd"] = "5";

            urlBuilder.Query = query.ToString();*/
            
            var query = new Dictionary<string, string>();
            query.Add("api_token", "YUqX8aO6YPsdYwcidhnVTkzSyYbTZPdlhVzLhBpqmApQNnEKH9vYVgacNBpI");
            query.Add("from", "MONIVAULT");
            query.Add("to", recipient);
            query.Add("body", message);
            query.Add("dnd", "5");

            var content = new FormUrlEncodedContent(query);
            //throw new SmsException("Just throw");
            var httpClient = new HttpClient();
            Logger.Info("Sending sms...");
            var response = await httpClient.PostAsync("https://www.bulksmsnigeria.com/api/v1/sms/create", content);
            Logger.Info($"Sms send response status: {response.StatusCode}");
            var responseString = await response.Content.ReadAsStringAsync();
            Logger.Info($"response string: {responseString}");
            var respObj = JsonConvert.DeserializeObject<IDictionary<string, object>>(responseString);

            if (respObj.ContainsKey("error"))
            {
                Logger.Error("An error occurred sending SMS");
                var errorMsg =
                    JsonConvert.DeserializeObject<IDictionary<string, string>>(respObj["error"].ToJsonString())[
                        "message"];
                throw new SmsException(errorMsg);
            }

            Logger.Info("Sms sent!");
        }
    }
}