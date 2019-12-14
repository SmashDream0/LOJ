﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Cryption
{
    public interface ICryption
    {
        byte[] Decode(byte[] mass, Func<uint, RSACryptoServiceProvider> getRSA);
        byte[] Encode(byte[] mass, RSACryptoServiceProvider rsa);
    }
}