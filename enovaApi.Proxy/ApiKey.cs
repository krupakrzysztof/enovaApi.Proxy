using System.Security.Cryptography;

namespace enovaApi.Proxy
{
    public class ApiKey
    {
        public ApiKey()
        {
            GenerateKey();
        }

        public string Key { get; set; } = string.Empty;
        public string Operator { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public void GenerateKey()
        {
            Key = Convert.ToBase64String(RandomNumberGenerator.GetBytes(32));
        }

        public override string ToString()
        {
            return $"{Operator} has key {Key}";
        }
    }


    public class Keys
    {
        public static Keys Empty { get; } = new Keys();

        public List<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();

        public override string ToString()
        {
            return $"Founded keys count: {ApiKeys.Count}";
        }
    }
}
