using Microsoft.Extensions.Options;
using NotesApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace NotesApp.Extensions
{
    public class CryptoText
    {
        private readonly EncryptOptions encryptOptions;

        public CryptoText(IOptions<EncryptOptions> _encryptOptions)
        {
            encryptOptions = _encryptOptions.Value;
        }

        public string Encrypt(string text)
        {
            using(Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptOptions.Key);
                aes.IV = Encoding.UTF8.GetBytes(encryptOptions.IV);

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }
                    }
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string text)
        {
            using(Aes aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(encryptOptions.Key);
                aes.IV = Encoding.UTF8.GetBytes(encryptOptions.IV);

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using(MemoryStream ms = new MemoryStream(Convert.FromBase64String(text)))
                using(CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using(StreamReader sr = new StreamReader(cs))
                {
                    return sr.ReadToEnd();
                }
            }
        }
    }
}
