using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.Synch
{

    /// <summary>Таблица синхронизации</summary>
    public class UTable
    {
        public UTable(SynchPoolManager Parent, DataBase.ISTable Table)
        {
            this._parent = Parent;
            this.Table = Table;

            if (!RelationColumnExist(this.Table.Parent, "SPool"))
            { this.Table.Parent.Columns.AddRelation(T.SPool.GetColumn(C.SPool.Date), false, false); }

            this._id = 0;

            var tableName = Table.Parent.Name;

            if (_uTable == null)
            {
                _uTable = Parent.DataBase.Tables.First(x => x.Name == "UTable").CreateSubTable(false);
                _uTable.QUERRY().SHOW.DO();
            }

            for (int i = 0; i < _uTable.Rows.Count; i++)
            {
                var uTableName = _uTable.Rows.Get<string>(i, C.UTable.Name);

                if (String.Equals(tableName, uTableName, StringComparison.InvariantCultureIgnoreCase))
                {
                    this._id = _uTable.Rows.GetID(i);

                    if (this.Use)
                    { this.Table.Parent.Rows.SetValue += SetPoolAction; }

                    break;
                }
            }
        }

        private static DataBase.ISTable _uTable = null;

        private uint _id;
        private readonly SynchPoolManager _parent;

        public DataBase.ISTable Table 
        { get; private set; }

        /// <summary>Разрешено использовать</summary>
        public bool Use
        {
            get
            {
                if (this._id == 0)
                { return false; }
                else
                { return T.UTable.Rows.Get<bool>(_id, C.UTable.Use); }
            }
            set
            {
                if (this._id == 0)
                {
                    var Value = new object[_uTable.Parent.Columns.Count];

                    Value[C.UTable.Name] = Table.Parent.Name;
                    Value[C.UTable.Use] = value;

                    _uTable.Rows.Add(Value);

                    this._id = _uTable.Rows.GetID(_uTable.Rows.Count - 1);
                }
                else
                { T.UTable.Rows.Set<bool>(_id, C.UTable.Use, value); }

                Table.Parent.Rows.SetValue -= SetPoolAction;

                if (value)
                { Table.Parent.Rows.SetValue += SetPoolAction; }
            }
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
        private void SetPoolAction(DataBase.ITable table, DataBase.AddSet_class e)
        { e.Add(table.Columns.Count - 1, this._parent.LastPoolID); }

        public override string ToString()
        {
            return Table.Name.ToString();
        }
    }
}
