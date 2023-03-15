using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace enovaApi.Proxy.Controllers
{
    [Route("api/MethodInvoker/InvokeServiceMethod")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        public ApiController(IConfiguration configuration)
        {
            enovaUrl = $"{configuration.GetValue<string>("enovaUrl")}";
            if (string.IsNullOrWhiteSpace(enovaUrl) )
            {
                throw new ArgumentException("enova url was not found in configuration.");
            }

            keys = configuration.Get<Keys>() ?? Keys.Empty;
        }

        private readonly HttpClient _httpClient = new();
        private readonly string enovaUrl;
        private readonly Keys keys;

        [HttpPost]
        public async Task<object> Post()
        {
            var body = await Request.Body.GetString();
            var jObject = JObject.Parse(body);
            var key = jObject["ApiKey"];
            if (key != null)
            {
                jObject.Remove("ApiKey");
                jObject.Remove("Operator");
                jObject.Remove("Password");

                var apiKey = keys.ApiKeys.FirstOrDefault(x => x.Key.Equals(key.Value<string>()));
                if (apiKey != null)
                {
                    jObject.Add("Operator", apiKey.Operator);
                    jObject.Add("Password", Cryptography.Decrypt(apiKey.Password));
                }
            }

            var content = new StringContent(jObject.ToString(), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(new Uri(new Uri(enovaUrl), "api/MethodInvoker/InvokeServiceMethod"), content);
            var json = await response.Content.ReadAsStringAsync();

            return json;
        }
    }
}
