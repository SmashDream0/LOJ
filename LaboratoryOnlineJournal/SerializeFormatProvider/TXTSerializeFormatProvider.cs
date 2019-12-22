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
    public class TXTSerializeFormatProvider : SerializeFormatProvider
    {
        public TXTSerializeFormatProvider( DataBase dataBase)
            : base("TXT", new TXTFormatChecker(Encoding.GetEncoding(1251)), new TXTSerializeProvider(Encoding.GetEncoding(1251), dataBase), new BlankEncryptionProvider())
        { }
    }
}