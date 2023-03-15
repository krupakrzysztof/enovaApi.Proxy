using System.Security.Cryptography;
using System.Text;

namespace enovaApi.Proxy
{
    public static class Cryptography
    {
        private static Aes Aes { get; } = Aes.Create();

        internal static void Configure()
        {
            Aes.BlockSize = 128;
            Aes.KeySize = 256;
        }

        public static void SetIV(string iv)
        {
            byte[] encodedIv;
            try
            {
                encodedIv = Convert.FromBase64String(iv);
            }
            catch
            {
                // add some log
                throw;
            }
            Aes.IV = encodedIv;
        }

        public static void SetKey(string key)
        {
            byte[] encodedKey;
            try
            {
                encodedKey = Convert.FromBase64String(key);
            }
            catch
            {
                // add some log
                throw;
            }
            Aes.Key = encodedKey;
        }

        public static string Decrypt(string base64cipher)
        {
            var bytes = Convert.FromBase64String(base64cipher);
            ICryptoTransform transform = Aes.CreateDecryptor();
            byte[] decryptedValue = transform.TransformFinalBlock(bytes, 0, bytes.Length);
            return Encoding.Unicode.GetString(decryptedValue);
        }

        public static string Encrypt(string plain)
        {
            ICryptoTransform encryptor = Aes.CreateEncryptor();
            byte[] cipher = Encoding.Unicode.GetBytes(plain);
            byte[] encryptedValue = encryptor.TransformFinalBlock(cipher, 0, cipher.Length);
            return Convert.ToBase64String(encryptedValue);
        }

        public static string GetKey()
        {
            return Convert.ToBase64String(Aes.Key);
        }

        public static string GetIV()
        {
            return Convert.ToBase64String(Aes.IV);
        }
    }
}
