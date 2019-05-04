using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Security.Cryptography;
using System.ServiceModel.Security.Tokens;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using Abp.Dependency;
using Abp.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Monivault.Utils;
using Newtonsoft.Json;
using System.Security.Policy;

namespace Monivault.InterswitchServices
{
    public class PayCodeService : MonivaultServiceBase, ITransientDependency
    {
        private readonly IConfiguration _config;

        public PayCodeService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public async Task ProcessPayCode()
        {
            await GenerateHeaders("https://sandbox.interswitch.com/api/v1/pwm/subscribers/2348022901232/tokens");
        }

        private async Task GenerateHeaders(string tokenUrl)
        {
            //var oneCardProps = _config.GetSection("OneCardProperties");
            //Logger.Info($"Connection string: {oneCardProps.GetValue<string>("AgentCode")}");
            
            var timeStamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
            Logger.Info($"Timestamp component: {timeStamp}");
            var nonce = RandomStringGeneratorUtil.GenerateNonce();//.NewGuid().ToString().Replace("-", "");
            Logger.Info($"Nonce: {nonce}");
            const string clientId = "IKIA36A24822E7E30C61AAE5112A0F4608ADC2CA0997";
                             
            Logger.Info($"ClientId: {clientId}");
            const string clientSecret = "i7BZAcxnUFNxmcP9FkOOVP3PpJSKcpeS3XsK6eYHHCQ=";
            Logger.Info($"ClientSecret: {clientSecret}");

            var authorization = Convert.ToBase64String(Encoding.UTF8.GetBytes(clientId));
            Logger.Info($"Authorization: Interswitch Auth {authorization}");
            //const string generateTokenUrl = "https://saturn.interswitchng.com/api/v1/pwm/subscribers/2348022901232/tokens";
            var encodedUrl = HttpUtility.UrlEncode(tokenUrl, Encoding.UTF8);

            var stringBuilder = new StringBuilder("Post");
            //stringBuilder.Append(HttpMethod.Post.Method);
            //stringBuilder.Append("Post");
            stringBuilder.Append("&");
            stringBuilder.Append(encodedUrl);
            stringBuilder.Append("&");
            stringBuilder.Append(timeStamp);
            stringBuilder.Append("&");
            stringBuilder.Append(nonce);
            stringBuilder.Append("&");
            stringBuilder.Append(clientId);
            stringBuilder.Append("&");
            stringBuilder.Append(clientSecret);

            var signatureCipher = stringBuilder.ToString();
            Logger.Info($"Signature Cipher: {signatureCipher}");
            var sha = new SHA1Managed();

            var signatureBytes = sha.ComputeHash(Encoding.UTF8.GetBytes(signatureCipher));
            var signature = Convert.ToBase64String(signatureBytes);
            //var encodedSignature = Encoding.UTF8.GetBytes(signatureCipher);
            //var sha1Digest = new SHA1Managed();
            //sha1Digest.BlockUpdate(encodedSignature, 0, encodedSignature.Length);
            //var signatureByte = new byte[sha1Digest.GetDigestSize()];
            //var signature = Convert.ToBase64String(signatureByte);
            
            Logger.Info($"Signature: {signature}");
            Logger.Info("Signature Method: SHA1");
            Logger.Info("Terminal Id: 3NRX0001");
            Logger.Info("Frontend Partner: APEX");

            Logger.Info("After FrontEnd Apex...");
            var stringContent = JsonConvert.SerializeObject(new
            {
                scope = "profile",
                grant_type = "client_credentials"
            });
            Logger.Info("About creating content");
            var httpContent = new StringContent(stringContent);
            var headers = httpContent.Headers;
            httpContent.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            Logger.Info("Preparing authorization header...");
            try
            {
                
                using (var httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri("https://saturn.interswitchng.com/");
                    var basicAuthCredentials = $"{clientId}:{clientSecret}";
                    var basicAuthEnc = Convert.ToBase64String(Encoding.UTF8.GetBytes(basicAuthCredentials));
                    var authValue = $"Basic {basicAuthEnc}";
                    Logger.Info($"Auth value: {authValue}");
                    httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(authValue);

                    Logger.Info("Lets get into HttpClient section...");
                    var response = await httpClient.PostAsync("passport/oauth/token", httpContent);
                    Logger.Info("About to check status code");
                    Logger.Info($"status code: {response.StatusCode}");
                    Logger.Info($"response: {response.Content.ReadAsStringAsync()}");
                    if (response.IsSuccessStatusCode)
                    {
                        var responseStr = await response.Content.ReadAsStringAsync();
                        Logger.Info($"Access Token String: {responseStr}");
                    }
                    else
                    {
                        Logger.Info("This thing is false...");
                    }
                }
            }
            catch (Exception exc)
            {
                Logger.Error(exc.Message);
                Logger.Error(exc.StackTrace);
            }
        }
    }
}