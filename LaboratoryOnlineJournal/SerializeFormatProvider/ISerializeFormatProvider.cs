using LaboratoryOnlineJournal.SerializeProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeFormatProvider
{
    public interface ISerializeFormatProvider
    {
        String Name { get; }
        bool TryDecodeData(Func<uint, RSACryptoServiceProvider> getRSA, byte[] bytes, out DeserializeResult deserializeResult);
        byte[] EncodeData(RSACryptoServiceProvider rsa, IEnumerable<DataBase.ISTable> tables, DateTime date, uint userID);
    }
}