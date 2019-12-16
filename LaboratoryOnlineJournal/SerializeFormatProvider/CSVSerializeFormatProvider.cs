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
    public class CSVSerializeFormatProvider : SerializeFormatProvider
    {
        public CSVSerializeFormatProvider(Encoding encoding, DataBase dataBase)
            : base("CSV", new CSVFormatChecker(encoding), new CSVSerializeProvider(encoding, dataBase), new BlankEncryptionProvider())
        { }
    }
}