using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web;
using Abp.Dependency;
using Abp.Domain.Services;
using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using ILogger = Castle.Core.Logging.ILogger;

namespace Monivault.Services
{
    public class SmsService : ISingletonDependency
    {
        private readonly ILogger _Logger;
        
        public SmsService()
        {
            _Logger = NullLogger.Instance;
        }

        public async void SendSms(string message, string recipient)
        {
            try
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
                
                var httpClient = new HttpClient();
                _Logger.Info("Sending sms...");
                var response = await httpClient.PostAsync("https://www.bulksmsnigeria.com/api/v1/sms/create", content);
                _Logger.Info("Sms sent!");
            }
            catch (Exception exc)
            {
                _Logger.Error(exc.StackTrace);
            }
        }
    }
}