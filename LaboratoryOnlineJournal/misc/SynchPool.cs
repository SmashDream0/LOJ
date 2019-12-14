using System;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Security.Authentication;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using LaboratoryOnlineJournal.Serializer;
using LaboratoryOnlineJournal.SerializeFormatProvider;

namespace LaboratoryOnlineJournal
{
    /// <summary>Синхронизация БД</summary>
    public unsafe class SynchPool_class
    {
        public SynchPool_class(DataBase db, String defaultSerializerName, IEnumerable<ISerializeFormatProvider> formatProvider)
        {
            _db = db;
            SPool = T.SPool.CreateSubTable(false);
            _formatProviders = formatProvider;
            _defaultSerializerName = defaultSerializerName;
        }

        private readonly String _defaultSerializerName;
        private readonly DataBase _db;
        private readonly IEnumerable<ISerializeFormatProvider> _formatProviders;

        private ISerializeFormatProvider GetSerializeProvider()
        {
            var result = _formatProviders.First(x=> String.Equals(x.Name, _defaultSerializerName));

            return result;
        }

        /// <summary>Таблица синхронизации</summary>
        struct UTable_struct
        {
            public UTable_struct(SynchPool_class Parent, DataBase.ISTable Table)
            {
                this.Parent = Parent;
                this.Table = Table;

                if (!RelationColumnExist(this.Table.Parent, "SPool"))
                { this.Table.Parent.Columns.AddRelation(T.SPool.GetColumn(C.SPool.Date), false, false); }

                this.ID = 0;

                var tableName = Table.Parent.Name;

                for (int i = 0; i < Parent._UTable.Rows.Count; i++)
                {
                    var uTableName = Parent._UTable.Rows.Get<string>(i, C.UTable.Name);

                    if (String.Equals(tableName, uTableName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        this.ID = Parent._UTable.Rows.GetID(i);

                        if (this.Use)
                        { this.Table.Parent.Rows.SetValue += Parent.SetPoolAction; }

                        break;
                    }
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

            uint ID;
            public readonly DataBase.ISTable Table;
            readonly SynchPool_class Parent;

            /// <summary>Разрешено использовать</summary>
            public bool Use
            {
                get
                {
                    if (this.ID == 0)
                    { return false; }
                    else
                    { return T.UTable.Rows.Get<bool>(ID, C.UTable.Use); }
                }
                set
                {
                    if (this.ID == 0)
                    {
                        var Value = new object[Parent._UTable.Parent.Columns.Count];

                        Value[C.UTable.Name] = Table.Parent.Name;
                        Value[C.UTable.Use] = value;

                        Parent._UTable.Rows.Add(Value);

                        this.ID = Parent._UTable.Rows.GetID(Parent._UTable.Rows.Count - 1);
                    }
                    else
                    { T.UTable.Rows.Set<bool>(ID, C.UTable.Use, value); }

                    Table.Parent.Rows.SetValue -= Parent.SetPoolAction;

                    if (value)
                    { Table.Parent.Rows.SetValue += Parent.SetPoolAction; }
                }
            }

            public override string ToString()
            {
                return Table.Name.ToString();
            }
        }

        /// <summary>Обновить список таблиц синхронизации</summary>
        /// <param name="UTable"></param>
        public void Invalidate(DataBase.ISTable UTable)
        {
            UTable.QUERRY().SHOW.DO();

            var UTableList = new List<UTable_struct>();
            _UTable = UTable;

            for (int i = 0; i < _db.Tables.Count; i++)
            {
                if (_db.Tables[i].Name.ToLower() != SPool.Parent.Name.ToLower()
                    && _db.Tables[i].Name.ToLower() != UTable.Parent.Name.ToLower()
                    && (_db.Tables[i].RemoteType != DataBase.RemoteType.Local || _db.type == DataBase.RemoteType.Local))
                { UTableList.Add(new UTable_struct(this, _db.Tables[i].CreateSubTable(false))); }
            }

            this.UTable = UTableList.ToArray();
        }

        /// <summary>Добавить ключ шифрования</summary>
        /// <param name="UserID">Идентификатор пользователя</param>
        /// <param name="ok">открытый ключ</param>
        /// <param name="ck">закрытый ключ</param>
        public void AddKey(uint UserID, string ok, string ck)
        {
            for (int i = 0; i < UK.Length; i++)
            {
                if (UK[i].UserID == UserID)
                { throw new Exception("Такой идентификатор уже существует");}
            }

            Array.Resize(ref UK, UK.Length + 1);

            UK[UK.Length - 1] = new UserKey_struct(UserID, Encoding.UTF8.GetBytes(ok), Encoding.UTF8.GetBytes(ck));
        }

        UTable_struct[] UTable = null;

        DataBase.ISTable SPool;
        DataBase.ISTable _UTable;

        /// <summary>Метка версии</summary>
        static readonly string Mark = "MZђ\u0002\u0000\u0000\u0000\u0000\u0000\u0000Н!ё!LН!This program cannot b\u0000 \u1057 ";
        static readonly int MarkByteCount = Encoding.UTF32.GetByteCount(Mark);
        /// <summary>ненужная хня</summary>
        static string sign = "Hey! My dear friend! Its really me!";

        /// <summary>Ключ шифрования</summary>
        struct UserKey_struct
        {
            public UserKey_struct(uint UserID, byte[] EncodeKey, byte[] DecodeKey)
            {
                this.UserID = UserID;
                this.EncodeKey = EncodeKey;
                this.DecodeKey = DecodeKey;
            }

            public readonly uint UserID;
            public readonly byte[] EncodeKey;
            public readonly byte[] DecodeKey;
        }

        UserKey_struct[] UK = new UserKey_struct[0];

        UserKey_struct GetUserKey(uint UID)
        {
            for (int i = 0; i < UK.Length; i++)
            {
                if (UK[i].UserID == UID)
                { return UK[i]; }
            }

            throw new Exception("Неизвестный идентификатор");
        }
        /// <summary>Идентификатор последней синхронизации этой копии БД</summary>
        public uint LastPoolID { get; internal set; }
        public int UCount { get { return UTable.Length; } }
        public DataBase.ISTable GetUTable(int Index) { return UTable[Index].Table; }
        public bool GetUTableUse(int Index) { return UTable[Index].Use; }
        public bool SetUTableUse(int Index, bool Use) { return UTable[Index].Use = Use; }

        public uint[] GetSynches(int YM)
        {
            var DTFrom = ATMisc.GetDateTimeFromYM(YM).AddMilliseconds(-1);
            var DTTo = ATMisc.GetDateTimeFromYM(YM + 1).AddMilliseconds(1);

            SPool.QUERRY().SHOWL(C.SPool.Date).WHERE
                .AC(C.SPool.Date).More.BV(DTFrom)
                .AND.AC(C.SPool.Date).Less.BV(DTTo)
                .DO();

            var Synches = new uint[SPool.Rows.Count];

            for (int i = 0; i < SPool.Rows.Count; i++)
            { Synches[i] = SPool.Rows.GetID(i); }

            return Synches;
        }

        public void GetDiapPeriods(out int YMFrom, out int YMTo)
        {
            var DTMax = (DateTime)SPool.QUERRY().GET.C(C.SPool.Date).Max(C.SPool.Date).By().DO()[0].Value;
            var DTMin = (DateTime)SPool.QUERRY().GET.C(C.SPool.Date).Min(C.SPool.Date).By().DO()[0].Value;

            YMFrom = ATMisc.GetYMFromDateTime(DTMin);
            YMTo = ATMisc.GetYMFromDateTime(DTMax);
        }

        public void Prepare()
        {
            //сортировка по колонке Date, сортировка назад, лимит по количеству
            //внимание - КАСТЫЛЬ!
            var ID = (uint)SPool.QUERRY().GET.ID().Max(C.SPool.Date).By().WHERE.C(C.SPool.Local, true).DO()[0].Value;

            if (ID == 0)
            { AddNewSynch(); }
            else
            { LastPoolID = ID; }
        }

        void SetPoolAction(DataBase.ITable _Table, DataBase.AddSet_class Vals)
        { Vals.Add(_Table.Columns.Count - 1, LastPoolID); }

        /// <summary>Создать новую точку синхронизации</summary>
        public void AddNewSynch()
        {
            if (LastPoolID > 0)
            { T.SPool.Rows.Set(LastPoolID, C.SPool.SUser, data.UserID); }

            var Values = new object[T.SPool.Columns.Count];
            Values[C.SPool.AUser] = data.UserID;
            Values[C.SPool.Date] = DateTime.Now;
            Values[C.SPool.Local] = true;

            if (SPool.Rows.Add(Values))
            { LastPoolID = SPool.Rows.GetID(SPool.Rows.Count - 1); }
            else
            { throw new Exception("Не удалось создать новую точку синхронизации"); }
        }

        static string ByteString(byte[] array)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            { sb.Append(array[i]).Append(','); }

            return sb.ToString();
        }

        /// <summary>получить шифрованое сообщение</summary>
        public unsafe byte[] GetEncrypted(uint SPoolID)
        {
            byte[] message;
            var UTables = new List<DataBase.ISTable>();

            for (int i = 0; i < UTable.Length; i++)
            {
                if (UTable[i].Use)
                {
                    UTable[i].Table.QUERRY(DataBase.State.None).SHOW.WHERE.C(UTable[i].Table.Parent.Columns.Count - 1, SPoolID).DO();

                    if (UTable[i].Table.Rows.Count > 0)
                    {
                        UTables.Add(UTable[i].Table);
                    }
                }
            }
            var date = T.SPool.Rows.Get<DateTime>(SPoolID, C.SPool.Date);
            var aUserID = T.SPool.Rows.Get_UnShow<uint>(SPoolID, C.SPool.AUser);

            var formatProvider = GetSerializeProvider();

            var rsa = GetRSA(aUserID);

            message = formatProvider.EncodeData(rsa, UTables, date, aUserID);

            return message;
        }
        /// <summary>Получить шифрованное сообщение</summary>
        /// <returns></returns>
        public unsafe byte[] GetEncrypted()
        {
            return GetEncrypted(LastPoolID);
        }

        /// <summary>Список таблиц и записей к шифрованию</summary>
        public class Table_class
        {
            public Table_class(DataBase.ISTable Table, uint SPoolID, int RowCount)
            {
                this.Table = Table;
                this.Rows = new Row_class[RowCount];
                this.SPoolID = SPoolID;
            }

            uint SPoolID;

            public class Row_class
            {
                public Row_class(Table_class Parent, uint ID, DataBase.State InUse, object[] Values)
                {
                    this.ID = ID;
                    this.InUse = InUse;
                    this.Values = Values;
                    this.Parent = Parent;
                    this.Values[Values.Length - 1] = Parent.SPoolID;
                }
                Table_class Parent;
                public readonly uint ID;
                public readonly DataBase.State InUse;
                public readonly object[] Values;

                public override string ToString()
                {
                    var srt = ID.ToString() + "," + InUse.ToString() + ". ";

                    for (int i = 0; i < Values.Length; i++)
                    {
                        if (Values[i] != null)
                        { srt += Values[i].ToString() + "|"; }
                        else
                        { srt += "|"; }
                    }

                    return srt.Substring(0, srt.Length - 1);
                }
            }

            public readonly DataBase.ISTable Table;
            public Row_class[] Rows;

            public Row_class this[int Index]
            {
                get { return Rows[Index]; }
                set { Rows[Index] = value; }
            }
            public int Count { get { return Rows.Length; } }

            public override string ToString()
            {
                return Table.Name + ";RC" + Rows.Length.ToString();
            }
        }
        
        public static bool SaveToDB(DataBase.ISTable table, uint id, DataBase.State state, object[] values)
        {
            //искать и заменять по индексерам полей неполучится, т.к. записи приходят уже измененные и невозможно определить чем оно там было до изменения
            //измененная запись придет в своём измененном виде
            //если происходят многократные изменения ключевых полей, тогда хреново, т.к. будут приходить многократные дубликаты с измененными ключевыми полями

            var RowExist = (bool)table.QUERRY(DataBase.State.None).EXIST.WHERE.ID(id).DO()[0].Value;

            if (RowExist)
            {
                var Set = table.QUERRY(DataBase.State.None).SET;

                switch (state)
                {
                    case DataBase.State.Deleted:
                        Set.Delete();
                        break;
                    case DataBase.State.Normal:
                        Set.UnDelete();
                        break;
                }

                try
                {
                    DataBase.ATSettings.AllowQuerryAutoConvertTypes = true;

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (values[i] != null)
                        { Set.C(i, values[i]); }
                    }

                    Set.WHERE.ID(id).DO();
                }
                catch (Exception ex)
                {
                    DataBase.ATSettings.AllowQuerryAutoConvertTypes = false;

                    if (MessageBox.Show("В процессе добавления записи номер " + id.ToString() + " возникла ошибка:" + ex.Message.ToString() + "\nВы хотите продолжить загрузку?", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    { return false; }
                }

                DataBase.ATSettings.AllowQuerryAutoConvertTypes = false;
            }
            else
            {
                try
                { return table.Rows.Add(id, values, state); }
                catch (Exception ex)
                {
                    if (MessageBox.Show("В процессе добавления записи номер " + id.ToString() + " возникла ошибка:" + ex.Message.ToString() + "\nВы хотите продолжить загрузку?", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    { return false; }
                }
            }

            return true;
        }

        /// <summary>Загрущить шифрованное сообщение</summary>
        /// <param name="mass">Шифрованный массив</param>
        /// <param name="UseProgressForm">Использовать визульную форму загрузки</param>
        /// <param name="ForceUserID">Использовать идентификатор пользователя насильно</param>
        /// <param name="ForceLoad">Загрузить не смотря на ограничения</param>
        /// <returns></returns>
        public unsafe string LoadCrypted(byte[] mass, bool UseProgressForm, uint ForceUserID = 0, bool ForceLoad = false)
        {
            DeserializeResult Tables;
            uint UserID;
            DateTime DT;
            bool SPoolExisted;
            var Returned = this.GetValues(ForceUserID, mass, out Tables, out SPoolExisted, true);

            if (Returned == null)
            {
                if (SPoolExisted && !ForceLoad)
                { return "Метка синхронизаций уже существует(" + Tables.UserID.ToString() + ", " + Tables.SynchDate.ToString() + ")"; }

                if (UseProgressForm)
                {
                    var loading = new LoadSPool_class(Tables);

                    new Progress_Form(loading).ShowDialog();

                    if (loading.Returning != null)
                    { return loading.Returning; }
                }
                else
                {
                    for (int i = 0; i < Tables.Tables.Length; i++)
                    {
                        var table = Tables.Tables[i];

                        Tables.Tables[i].STable.Parent.Rows.CanUseEvents = Tables.Tables[i].STable.Parent.Rows.AllowGridUpdateEvents = false;

                        for (int j = 0; j < Tables.Tables[i].Rows.Length; j++)
                        {
                            var row = table.Rows[i];

                            if (!SaveToDB(table.STable, row.ID, row.InUse, row.Values))
                            { return "Загрузка остановлена пользователем"; }
                        }

                        Tables.Tables[i].STable.Parent.Rows.CanUseEvents = Tables.Tables[i].STable.Parent.Rows.AllowGridUpdateEvents = true;
                    }
                }

                return null;
            }
            else
            { return Returned; }
        }

        class LoadSPool_class : Progress_Form.AObject
        {
            public LoadSPool_class(Serializer.DeserializeResult Tables) :
                base(false)
            { this.Tables = Tables; }

            Serializer.DeserializeResult Tables;
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

                        if (!SaveToDB(table.STable, row.ID, row.InUse, row.Values))
                        {
                            Returning = "Загрузка остановлена пользователем";
                            return false;
                        }
                    }

                    Tables.Tables[i].STable.Parent.Rows.CanUseEvents = Tables.Tables[i].STable.Parent.Rows.AllowGridUpdateEvents = true;
                }

                return true;
            }
        }

        /// <summary>Декодировать сообщение в читаемый вид</summary>
        /// <param name="mass">Шифрованное сообщение</param>
        /// <param name="Tables">Таблицы</param>
        /// <param name="UserID">Полученный идентификатор пользователя</param>
        /// <param name="SynchDT">Полученная дата создания синхронизации</param>
        /// <returns></returns>
        public unsafe string GetValues(uint ForceUserID, byte[] mass, out DeserializeResult Tables, out bool SPoolExisted, bool CheckSPoolExist = false)
        {
            SPoolExisted = false;

            if (mass == null)
            {
                Tables = null;
                return "Сообщение не найдено";
            }
            else
            {
                var serializerProvider = GetSerializeProvider();

                DeserializeResult result = null;

                foreach (var formatFrovider in _formatProviders)
                {
                    if (formatFrovider.TryDecodeData(GetRSA, mass, out result))
                    { break; }
                }

                if (result == null)
                {
                    CheckSPoolExist = false;
                    Tables = null;
                    return "Не удалось декодировать сообщение";
                }

                Tables = result;

                uint SPoolID = 0;
                if (CheckSPoolExist)
                {
                    //такой пул уже существует, загрузка не требуется
                    SPoolExisted = (bool)SPool.QUERRY().EXIST.WHERE.C(C.SPool.SUser, Tables.UserID).AND.C(C.SPool.Date, Tables.SynchDate).DO()[0].Value;

                    if (!SPoolExisted)
                    {
                        SPool.QUERRY()
                            .ADD
                                .C(C.SPool.AUser, Tables.UserID)
                                .C(C.SPool.SUser, Tables.UserID)
                                .C(C.SPool.Local, false)
                                .C(C.SPool.Date, Tables.SynchDate)
                            .DO();
                        SPoolID = SPool.Rows.GetID(SPool.Rows.Count - 1);
                    }
                }

                return null;
            }
        }

        private RSACryptoServiceProvider GetRSA(uint userID)
        {
            var rsa = new RSACryptoServiceProvider();

            var uk = GetUserKey(userID);

            if (uk.EncodeKey == null)
            { throw new Exception("Обновление не может быть сформировано, т.к. шифрующий код не задан."); }

            {
                var k = ((DataBase.table)T.User).EncodeType.GetString(uk.DecodeKey);

                rsa.FromXmlString(k);
            }

            return rsa;
        }

        /// <summary>Получить шифрованное сообщение</summary>
        /// <param name="mass">Шифруемое сообещение</param>
        /// <param name="ForceUserID">Использовать идентификатор пользователя насильно</param>
        /// <returns></returns>
        public unsafe string ShowCrypted(byte[] mass, uint ForceUserID = 0)
        {
            DeserializeResult Tables;
            uint UserID;
            DateTime DT;
            bool SPoolExisted;
            var Returned = this.GetValues(ForceUserID, mass, out Tables, out SPoolExisted, false);

            if (Returned == null)
            {
                new Tables_Form(Tables).ShowDialog();
                return null;
            }
            else
            { return Returned; }
        }

        unsafe void LoadFromMessage(byte[] Message)
        {

        }

        public override string ToString()
        {
            return "PullCount=" + SPool.Rows.Count + "; PullID=" + LastPoolID.ToString() + "; TableCount=" + UTable.Length.ToString();
        }
    }
}
