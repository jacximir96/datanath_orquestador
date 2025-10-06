using application.Interfaces;
using domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace application.Services
{
    public class EncryptService : IEncryptService
    {
        private readonly IEncryptAdapter _encryptAdapter; 
        
        public EncryptService(IEncryptAdapter encryptAdapter) 
        { 
            _encryptAdapter = encryptAdapter;
        }

        public string Decrypt(string text)
        {
            string decrypted=string.Empty;
            try
            {
                decrypted = _encryptAdapter.Decrypt(text, Resources.GeneralKeyToEncrypt);
            }
            catch (Exception e)
            {
                e.Message.ToString();
            }
            return decrypted;   
        }

        public string Encrypt(string text)
        {
            string encrypted=string.Empty;  
            try
            {
                encrypted=_encryptAdapter.Encrypt(text, Resources.GeneralKeyToEncrypt);
            }
            catch (Exception e)
            {

            }
            return encrypted;
        }
    }
}
