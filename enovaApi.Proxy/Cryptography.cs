using System.Security.Cryptography;
using System.Text;

namespace enovaApi.Proxy
{
    public static class Cryptography
    {
        private static Aes Aes { get; } = Aes.Create();

        public static void SetIV(string iv)
        {
            Aes.IV = Convert.FromBase64String(iv);
        }

        public static void SetKey(string key)
        {
            Aes.Key = Convert.FromBase64String(key);
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
