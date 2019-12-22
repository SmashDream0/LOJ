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
        public OldSerializeFormatProvider(DataBase dataBase)
            : base("Old", new OldFormatChecker(Encoding.UTF32), new OldByteSerializeProvider(Encoding.UTF32, dataBase), new OldEncryptionProvider())
        { }
    }
}