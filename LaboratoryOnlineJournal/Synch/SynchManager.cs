using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Synch
{
    public class SynchManager
    {
        public SynchManager(RSA rsa)
        {
            _rsa = rsa;

            SynchTables = new List<SynchTable>();
        }

        private static RSA _rsa;

        public IList<SynchTable> SynchTables
        { get; private set; }

        private IEnumerable<DataBase.ISTable> GetTables()
        {
            var tables = new List<DataBase.ISTable>();

            foreach (var synchTable in SynchTables)
            {
                if (synchTable.Use)
                {
                    var table = synchTable.Table;

                    table.QUERRY().SHOW.DO();

                    if (table.Rows.Count > 0)
                    { tables.Add(table); }
                }
            }

            return tables;
        }

        //public byte[] GetMessage(uint synchPoolID)
        //{ 
            
        //}
    }
}