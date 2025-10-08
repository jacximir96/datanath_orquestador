using domain.Interfaces;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace infrastructure.Adapters
{
    public class EncryptAdapter : IEncryptAdapter
    {
        public string Decrypt(string cipherText, string key)
        {
            StreamReader sr = null;
            string cipheredText=string.Empty;
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16]; // Must be same as used in Encrypt

                using var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var buffer = Convert.FromBase64String(cipherText);

                using var ms = new MemoryStream(buffer);
                using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
                using (sr = new StreamReader(cs))
                {
                    cipheredText=sr.ReadToEnd();
                } 
                
            }
            catch (Exception e) 
            { 
                e.Message.ToString();
            }
            return cipheredText;    
        }

        public string Encrypt(string text, string key)
        {
            MemoryStream ms = null;
            try
            {
                using var aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16];

                using var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (ms = new MemoryStream()) 
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    using (var sw = new StreamWriter(cs))
                    {
                        sw.Write(text);
                    }
                }              
            }
            catch (Exception e) 
            {
                e.Message.ToString();
            }
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}
