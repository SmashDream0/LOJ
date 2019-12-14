using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Serializer
{
    public class Row
    {
        public Row(uint ID, DataBase.State InUse, object[] Values)
        {
            this.ID = ID;
            this.InUse = InUse;
            this.Values = Values;
        }

        public readonly uint ID;
        public readonly DataBase.State InUse;
        public readonly object[] Values;
    }
}