using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace InventoryApp.Services
{
    public static class CryptoHelper
    {
        private static readonly byte[] Key = Encoding.UTF8.GetBytes("InventoryApp2024Key123456789012345");
        private static readonly byte[] IV = Encoding.UTF8.GetBytes("InventoryApp2024!");

        public static string Encrypt(string plainText)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    
                    using (var encryptor = aes.CreateEncryptor())
                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var writer = new StreamWriter(cs))
                    {
                        writer.Write(plainText);
                        writer.Close();
                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string Decrypt(string cipherText)
        {
            try
            {
                using (var aes = Aes.Create())
                {
                    aes.Key = Key;
                    aes.IV = IV;
                    
                    using (var decryptor = aes.CreateDecryptor())
                    using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var reader = new StreamReader(cs))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}