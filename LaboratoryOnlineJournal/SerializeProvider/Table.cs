using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeProvider
{
    public class Table
    {
        /// <summary>
        /// Таблица
        /// </summary>
        public DataBase.ISTable STable { get; set; }

        /// <summary>
        /// Список данных строк
        /// </summary>
        public Row[] Rows { get; set; }
    }
}