using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.CryptionProvider
{
    public interface ICryptionProvider
    {
        byte[] Decode(byte[] mass, Func<uint, RSACryptoServiceProvider> getRSA);
        byte[] Encode(byte[] mass, RSACryptoServiceProvider rsa);
    }
}