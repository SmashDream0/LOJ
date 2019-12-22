using LaboratoryOnlineJournal.SerializeProvider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Synch
{
    public class LoadSPool : Progress_Form.AObject
    {
        public LoadSPool(DeserializeResult Tables, Func<DataBase.ISTable, uint, DataBase.State, object[], bool> saveToDBFunc) :
            base(false)
        { 
            this.Tables = Tables;
            _saveToDBFunc = saveToDBFunc;
        }

        private readonly Func<DataBase.ISTable, uint, DataBase.State, object[], bool> _saveToDBFunc;
        private readonly DeserializeResult Tables;
        public string Returning = null;

        protected override bool Do()
        {
            var MaxCount = 0;
            var CurCount = 0;

            for (int i = 0; i < Tables.Tables.Length; i++)
            { MaxCount += Tables.Tables[i].Rows.Length; }

            Action(MaxCount, 0);

            for (int i = 0; i < Tables.Tables.Length; i++)
            {
                var table = Tables.Tables[i];

                Action(Tables.Tables[i].STable.Name, MaxCount, CurCount);

                Tables.Tables[i].STable.Parent.Rows.CanUseEvents = Tables.Tables[i].STable.Parent.Rows.AllowGridUpdateEvents = false;

                for (int j = 0; j < Tables.Tables[i].Rows.Length; j++)
                {
                    Action(MaxCount, ++CurCount);
                    var row = Tables.Tables[i].Rows[j];

                    if (!_saveToDBFunc(table.STable, row.ID, row.InUse, row.Values))
                    {
                        Returning = "Загрузка остановлена пользователем";
                        return false;
                    }
                }

                Tables.Tables[i].STable.Parent.Rows.CanUseEvents = Tables.Tables[i].STable.Parent.Rows.AllowGridUpdateEvents = true;
            }

            return true;
        }

        public void Do_public()
        { Do(); }
    }
}
