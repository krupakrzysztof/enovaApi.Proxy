using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Text;

namespace enovaApi.Proxy.Controllers
{
    [Route("api/Keys")]
    [ApiController]
    public class KeysController : ControllerBase
    {
        public KeysController(IConfiguration configuration)
        {
            keys = configuration.Get<Keys>() ?? Keys.Empty;
        }

        private readonly Keys keys;

        [HttpPost]
        public string CreateKey(string oper, string password, bool regenerate)
        {
            ApiKey apiKey = keys.ApiKeys.FirstOrDefault(x => x.Operator == oper);
            if (apiKey != null)
            {
                if (regenerate)
                {
                    apiKey.GenerateKey();
                }
                else
                {
                    return new JObject(new JProperty("Message", $"Api key for {oper} was generated. Set regenerate parameter to true to regenerate key.")).ToString();
                }
            }
            else
            {
                apiKey = new ApiKey()
                {
                    Operator = oper,
                    Password = Cryptography.Encrypt(password)
                };
                keys.ApiKeys.Add(apiKey);
            }

            System.IO.File.WriteAllText("keys.json", JObject.FromObject(keys).ToString(), Encoding.UTF8);

            return new JObject(new JProperty("Key", apiKey.Key)).ToString();
        }
    }
}
