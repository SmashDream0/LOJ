using LaboratoryOnlineJournal.FormatChecker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeProvider
{
    public interface ISerializeProvider
    {
        byte[] Serialize(IEnumerable<DataBase.ISTable> tables, DateTime date, uint userID);
        DeserializeResult Deserialize(byte[] mass);
    }
}