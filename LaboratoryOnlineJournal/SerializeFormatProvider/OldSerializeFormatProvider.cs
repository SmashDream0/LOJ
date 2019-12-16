using LaboratoryOnlineJournal.CryptionProvider;
using LaboratoryOnlineJournal.FormatChecker;
using LaboratoryOnlineJournal.SerializeProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeFormatProvider
{
    public class OldSerializeFormatProvider : SerializeFormatProvider
    {
        public OldSerializeFormatProvider(Encoding encoding, DataBase dataBase)
            : base("Old", new OldFormatChecker(encoding), new OldByteSerializeProvider(encoding, dataBase), new OldEncryptionProvider())
        { }
    }
}