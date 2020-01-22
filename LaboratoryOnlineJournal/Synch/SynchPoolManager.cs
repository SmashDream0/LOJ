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
using LaboratoryOnlineJournal.SerializeProvider;
using LaboratoryOnlineJournal.SerializeFormatProvider;

namespace LaboratoryOnlineJournal.Synch
{
    /// <summary>Синхронизация БД</summary>
    public unsafe class SynchPoolManager
    {
        public SynchPoolManager(DataBase db, String defaultSerializerName, IEnumerable<ISerializeFormatProvider> formatProvider)
        {
            DataBase = db;
            _sPool = T.SPool.CreateSubTable(false);
            _formatProviders = formatProvider;
            _defaultSerializerName = defaultSerializerName;

            KeysContainer = new KeysContainer(((DataBase.table)_sPool.Parent).EncodeType);
        }

        public IEnumerable<UTable> UTables 
        { get; private set; }

        public KeysContainer KeysContainer
        { get; private set; }
        public DataBase DataBase
        { get; private set; }

        private DataBase.ISTable _sPool;
        private DataBase.ISTable _uTable;

        private readonly String _defaultSerializerName;
        private readonly IEnumerable<ISerializeFormatProvider> _formatProviders;

        private ISerializeFormatProvider GetSerializeProvider()
        {
            var result = _formatProviders.First(x=> String.Equals(x.Name, _defaultSerializerName));

            return result;
        }

        /// <summary>Обновить список таблиц синхронизации</summary>
        /// <param name="UTable"></param>
        public void Invalidate(DataBase.ISTable UTable)
        {
            UTable.QUERRY().SHOW.DO();

            var UTableList = new List<UTable>();
            _uTable = UTable;

            for (int i = 0; i < DataBase.Tables.Count; i++)
            {
                if (DataBase.Tables[i].Name.ToLower() != _sPool.Parent.Name.ToLower()
                    && DataBase.Tables[i].Name.ToLower() != UTable.Parent.Name.ToLower()
                    && (DataBase.Tables[i].RemoteType != DataBase.RemoteType.Local || DataBase.type == DataBase.RemoteType.Local))
                { UTableList.Add(new UTable(this, DataBase.Tables[i].CreateSubTable(false))); }
            }

            this.UTables = UTableList.ToArray();
        }
        /// <summary>Идентификатор последней синхронизации этой копии БД</summary>
        public uint LastPoolID { get; internal set; }

        public uint[] GetSynches(int YM)
        {
            var DTFrom = ATMisc.GetDateTimeFromYM(YM).AddMilliseconds(-1);
            var DTTo = ATMisc.GetDateTimeFromYM(YM + 1).AddMilliseconds(1);

            _sPool.QUERRY().SHOWL(C.SPool.Date).WHERE
                .AC(C.SPool.Date).More.BV(DTFrom)
                .AND.AC(C.SPool.Date).Less.BV(DTTo)
                .DO();

            var Synches = new uint[_sPool.Rows.Count];

            for (int i = 0; i < _sPool.Rows.Count; i++)
            { Synches[i] = _sPool.Rows.GetID(i); }

            return Synches;
        }

        public void GetDiapPeriods(out int YMFrom, out int YMTo)
        {
            var DTMax = (DateTime)_sPool.QUERRY().GET.C(C.SPool.Date).Max(C.SPool.Date).By().DO()[0].Value;
            var DTMin = (DateTime)_sPool.QUERRY().GET.C(C.SPool.Date).Min(C.SPool.Date).By().DO()[0].Value;

            YMFrom = ATMisc.GetYMFromDateTime(DTMin);
            YMTo = ATMisc.GetYMFromDateTime(DTMax);
        }

        public void Prepare()
        {
            //сортировка по колонке Date, сортировка назад, лимит по количеству
            //внимание - КАСТЫЛЬ!
            var ID = (uint)_sPool.QUERRY().GET.ID().Max(C.SPool.Date).By().WHERE.C(C.SPool.Local, true).DO()[0].Value;

            if (ID == 0)
            { AddNewSynch(); }
            else
            { LastPoolID = ID; }
        }

        private void SetPoolAction(DataBase.ITable _Table, DataBase.AddSet_class Vals)
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

            if (_sPool.Rows.Add(Values))
            { LastPoolID = _sPool.Rows.GetID(_sPool.Rows.Count - 1); }
            else
            { throw new Exception("Не удалось создать новую точку синхронизации"); }
        }

        /// <summary>получить шифрованое сообщение</summary>
        public unsafe byte[] GetEncrypted(uint SPoolID)
        {
            var message = new byte[0];
            var uTables = new List<DataBase.ISTable>();

            foreach(var uTable in UTables)
            {
                if (uTable.Use)
                {
                    uTable.Table.QUERRY(DataBase.State.None).SHOW.WHERE.C(uTable.Table.Parent.Columns.Count - 1, SPoolID).DO();

                    if (uTable.Table.Rows.Count > 0)
                    { uTables.Add(uTable.Table); }
                }
            }

            if (uTables.Any())
            {
                var date = T.SPool.Rows.Get<DateTime>(SPoolID, C.SPool.Date);
                var aUserID = T.SPool.Rows.Get_UnShow<uint>(SPoolID, C.SPool.AUser);

                var key = KeysContainer.GetEncodeKey(aUserID);

                if (!String.IsNullOrEmpty(key))
                {
                    var rsa = new RSACryptoServiceProvider();
                    rsa.FromXmlString(key);

                    var formatProvider = GetSerializeProvider();
                    message = formatProvider.EncodeData(rsa, uTables, date, aUserID);
                }
            }

            return message;
        }

        /// <summary>
        /// Получить заголовок пакета данных
        /// </summary>
        /// <param name="data">Входящие данные</param>
        /// <param name="userID">Ключ пользователя</param>
        /// <param name="synchDate">Дата синхронизации</param>
        /// <returns></returns>
        public bool TryGetHeaderData(byte[] data, out uint userID, out DateTime synchDate)
        {
            var serializerProvider = GetSerializeProvider();

            DeserializeResult output = null;

            foreach (var formatFrovider in _formatProviders)
            {
                if (formatFrovider.TryDecodeData(GetRSA, data, out output))
                { break; }
            }

            var result = output != null;

            if (result)
            {
                userID = output.UserID;
                synchDate = output.SynchDate;
            }
            else
            {
                userID = default(uint);
                synchDate = default(DateTime);
            }

            return result;
        }

        /// <summary>Получить шифрованное сообщение</summary>
        /// <returns></returns>
        public unsafe byte[] GetEncrypted()
        {
            return GetEncrypted(LastPoolID);
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
            bool SPoolExisted;
            var Returned = this.GetValues(ForceUserID, mass, out Tables, out SPoolExisted, true);

            if (Returned == null)
            {
                if (SPoolExisted && !ForceLoad)
                { return "Метка синхронизаций уже существует(" + Tables.UserID.ToString() + ", " + Tables.SynchDate.ToString() + ")"; }

                var loading = new LoadSPool(Tables, SaveToDB);

                if (UseProgressForm)
                { new Progress_Form(loading).ShowDialog(); }
                else
                { loading.Do_public(); }

                if (loading.Returning != null)
                { Returned = loading.Returning; }
            }
            
            return Returned;
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
                    SPoolExisted = (bool)_sPool.QUERRY().EXIST.WHERE.C(C.SPool.SUser, Tables.UserID).AND.C(C.SPool.Date, Tables.SynchDate).DO()[0].Value;

                    if (!SPoolExisted)
                    {
                        _sPool.QUERRY()
                            .ADD
                                .C(C.SPool.AUser, Tables.UserID)
                                .C(C.SPool.SUser, Tables.UserID)
                                .C(C.SPool.Local, false)
                                .C(C.SPool.Date, Tables.SynchDate)
                            .DO();
                        SPoolID = _sPool.Rows.GetID(_sPool.Rows.Count - 1);
                    }
                }

                return null;
            }
        }

        private bool GetRSA(uint userID, out RSACryptoServiceProvider rsa)
        {
            var result = false;
            rsa = null;

            var key = KeysContainer.GetDecodeKey(userID);

            if (!String.IsNullOrEmpty(key))
            {
                rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(key);

                result = true;
            }

            return result;
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

        public override string ToString()
        {
            return "PullCount=" + _sPool.Rows.Count + "; PullID=" + LastPoolID.ToString() + "; TableCount=" + UTables.Count().ToString();
        }
    }
}
