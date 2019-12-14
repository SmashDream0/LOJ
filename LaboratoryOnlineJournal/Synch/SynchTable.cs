using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Synch
{
    public class SynchTable
    {
        public SynchTable(DataBase.ISTable Table)
        {
            this.Table = Table;

            if (!RelationColumnExist(Table.Parent, "SPool"))
            { this.Table.Parent.Columns.AddRelation(T.SPool.GetColumn(C.SPool.Date), false, false); }
        }

        private static bool RelationColumnExist(DataBase.ITable table, string columnName)
        {
            var result = false;

            for (int i = 0; i < table.Columns.Count; i++)
            {
                var column = table.GetColumn(i);

                if (column.Name == columnName)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public DataBase.ISTable Table 
        { get; private set; }

        /// <summary>Разрешено использовать</summary>
        public bool Use
        { get; set; }
    }
}