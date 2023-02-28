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
        public string CreateKey(string oper, string password)
        {
            var key = new ApiKey()
            {
                Key = Guid.NewGuid().ToString("N"),
                Operator = oper,
                Password = Cryptography.Encrypt(password)
            };
            keys.ApiKeys.Add(key);

            System.IO.File.WriteAllText("keys.json", JObject.FromObject(keys).ToString(), Encoding.UTF8);

            return key.Key;
        }
    }
}
