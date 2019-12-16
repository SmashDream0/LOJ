using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeProvider
{
    public class DeserializeResult
    {
        /// <summary>
        /// Список таблиц
        /// </summary>
        public Table[] Tables { get; set; }

        /// <summary>
        /// Дата точки синхронизации
        /// </summary>
        public DateTime SynchDate { get; set; }

        /// <summary>
        /// Ключ пользователя
        /// </summary>
        public uint UserID { get; set; }
    }
}
