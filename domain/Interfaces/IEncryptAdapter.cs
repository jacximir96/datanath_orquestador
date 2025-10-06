using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace domain.Interfaces
{
    public interface IEncryptAdapter
    {
        string Encrypt(string text, string key);
        string Decrypt(string cipherText, string key);
    }
}
