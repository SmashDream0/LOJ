using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.CryptionProvider
{
    public class BlankEncryptionProvider : ICryptionProvider
    {
        public byte[] Decode(byte[] mass, Func<uint, RSACryptoServiceProvider> getRSA)
        {
            return mass;
        }

        public byte[] Encode(byte[] mass, RSACryptoServiceProvider rsa)
        {
            return mass;
        }
    }
}