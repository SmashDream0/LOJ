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
        byte[] Decode(byte[] mass, GetRsaDelegate getRSA);
        byte[] Encode(byte[] mass, RSACryptoServiceProvider rsa);
    }

    public delegate bool GetRsaDelegate(uint id, out RSACryptoServiceProvider rsa);
}