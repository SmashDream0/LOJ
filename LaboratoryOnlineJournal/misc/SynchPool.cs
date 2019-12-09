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

namespace LaboratoryOnlineJournal
{
    /// <summary>Синхронизация БД</summary>
    public unsafe class SynchPool_class
    {
        public SynchPool_class()
        {
            SPool = T.SPool.CreateSubTable(false);
        }

        /// <summary>Таблица синхронизации</summary>
        struct UTable_struct
        {
            public UTable_struct(SynchPool_class Parent, DataBase.ISTable Table)
            {
                this.Parent = Parent;
                this.Table = Table;
                this.Table.Parent.Columns.AddRelation(T.SPool.GetColumn(C.SPool.Date), false, false);

                var LCTable = this.Table.Parent.Name.ToLower();
                this.ID = 0;

                for (int i = 0; i < Parent._UTable.Rows.Count; i++)
                {
                    if (LCTable == Parent._UTable.Rows.Get<string>(i, C.UTable.Name).ToLower())
                    {
                        this.ID = Parent._UTable.Rows.GetID(i);

                        if (this.Use)
                        { this.Table.Parent.Rows.SetValue += Parent.SetPoolAction; }

                        break;
                    }
                }
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
        }

        /// <summary>Обновить список таблиц синхронизации</summary>
        /// <param name="UTable"></param>
        public void Invalidate(DataBase.ISTable UTable)
        {
            UTable.QUERRY().SHOW.DO();

            var UTableList = new List<UTable_struct>();
            _UTable = UTable;

            for (int i = 0; i < data.T1.Tables.Count; i++)
            {
                if (data.T1.Tables[i].Name.ToLower() != SPool.Parent.Name.ToLower()
                    && data.T1.Tables[i].Name.ToLower() != UTable.Parent.Name.ToLower()
                    && (data.T1.Tables[i].RemoteType != DataBase.RemoteType.Local || data.T1.type == DataBase.RemoteType.Local))
                { UTableList.Add(new UTable_struct(this, data.T1.Tables[i].CreateSubTable(false))); }
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

            throw new Exception("Неизвестный идентфиикатор");
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

        struct crypt_struct
        {
            public crypt_struct(Random rnd, int MinCount)
            {
                this.mass = new byte[rnd.Next(MinCount, MinCount * 2)];
                rnd.NextBytes(mass);
                this.index = 0;
            }
            public crypt_struct(Random rnd, int MinCount, int MaxCount)
            {
                this.mass = new byte[rnd.Next(MinCount, MaxCount)];
                rnd.NextBytes(mass);
                this.index = 0;
            }
            public crypt_struct(byte[] MassFrom)
            {
                this.mass = MassFrom;
                this.index = 0;
            }
            public crypt_struct(string Pass)
            {
                this.mass = Encoding.UTF32.GetBytes(Pass);
                this.index = 0;
            }

            public byte Encrypt(byte b)
            {
                if (index > mass.Length - 1)
                { index = 0; }

                unchecked
                {
                    return (byte)(b + mass[index++] - index);
                }
            }

            public byte Decrypt(byte b)
            {
                if (index > mass.Length - 1)
                { index = 0; }

                unchecked
                {
                    return (byte)(b - mass[index++] + index);
                }
            }

            public readonly byte[] mass;
            int index;

            public string pass { get { return Encoding.UTF32.GetString(mass); } }
        }

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
        /// <summary>Шифровать массив</summary>
        /// <param name="UserID">Идентификатор пользователя</param>
        /// <param name="mass">Массив для шифрования</param>
        void Encode(uint UserID, ref byte[] mass)
        {
            var rnd = new Random();

            var mrpass = new crypt_struct(rnd, mass.Length / 2);   //внутренний пароль смещения   
            var urbytes = new crypt_struct(rnd, 200, 200);    //внешний пароль смещения

            var passPos = mass.Length;
            int enckeyLength;

            {
                //шифрую шифр
                var rsa = new RSACryptoServiceProvider();

                var uk = GetUserKey(UserID);

                if (uk.EncodeKey == null)
                { throw new Exception("Обновление не может быть сформировано, т.к. шифрующий код не задан."); }

                {
                    var k = ((DataBase.table)T.User).EncodeType.GetString(uk.EncodeKey);

                    rsa.FromXmlString(k);
                }

                var tmp = ByteString(urbytes.mass);

                var enckey = rsa.Encrypt(urbytes.mass, true); //шифрованый шифр
                
                tmp = ByteString(enckey);

                enckeyLength = enckey.Length;   //длина шифрованного пароля

                //новая длина, с учетом длины паролей
                Array.Resize(ref mass, mass.Length + mrpass.mass.Length + enckey.Length + 8);   //+8 - длина внешнего пароля 4(позиция 0) + длина внутреннего пароля(позиция 4)

                //сдвигаю на указатели длин паролей и длины внутреннего + внешнего паролей. id автора не шифруется
                Array.Copy(mass, 0, mass, 8 + enckey.Length + mrpass.mass.Length, passPos);
                fixed (void* numPtr = &mass[0])
                { *(int*)numPtr = enckey.Length; } //длина внешнего пароля
                fixed (void* numPtr = &mass[4])
                { *(int*)numPtr = mrpass.mass.Length; } //длина внутреннего пароля

                //копирую внешний пароль
                Array.Copy(enckey, 0, mass, 8, enckey.Length);
                //копирую внутренний пароль
                Array.Copy(mrpass.mass, 0, mass, enckey.Length + 8, mrpass.mass.Length);
            }

            //шифрую данные первым шифром
            {
                int Start = mrpass.mass.Length + enckeyLength + 8;
                int End = mass.Length - 4;
                for (int i = Start; i < End; i++)
                { mass[i] = mrpass.Encrypt(mass[i]); }
            }
            //шифрую данные и первый пароль вторым шифром(иначе - все, кроме указателя длины внешнего пароля и id пользователя)
            {
                int Start = 8 + enckeyLength;
                int End = mass.Length - 4;
                for (int i = Start; i < End; i++)
                { mass[i] = urbytes.Encrypt(mass[i]); }
            }
        }

        static string ByteString(byte[] array)
        {
            var sb = new StringBuilder();

            for (int i = 0; i < array.Length; i++)
            { sb.Append(array[i]).Append(','); }

            return sb.ToString();
        }
        /// <summary>Расшифровать массив</summary>
        /// <param name="mass">Массив шифрации</param>
        /// <param name="ForceUserID">Идентификатор пользователя. Не обязательно</param>
        /// <returns></returns>
        bool Decode(ref byte[] mass, uint ForceUserID = 0)    //работает правильно
        {
            uint UserID;
            fixed (void* numPtr = &mass[mass.Length - 4])
            { UserID = *(uint*)numPtr; }  //id пользователя

            crypt_struct urbytes, mrpass;

            int enckeyLength;

            {
                //длина внешнего пароля
                fixed (void* numPtr = &mass[0])
                { enckeyLength = *(int*)numPtr; }
                var tmass = new byte[enckeyLength];
                Array.Copy(mass, 8, tmass, 0, enckeyLength);

                int m1ln, m2ln;
                fixed (void* numPtr = &mass[0])
                { m1ln = *(int*)numPtr; }
                fixed (void* numPtr = &mass[4])
                { m2ln = *(int*)numPtr; }

                var uk = GetUserKey((ForceUserID == 0 ? UserID : ForceUserID));

                if (uk.DecodeKey == null)
                { throw new Exception("Обновление не может быть сформировано, т.к. шифрующий код не задан."); }

                var rsa = new RSACryptoServiceProvider();
                {
                    var k = ((DataBase.table)T.User).EncodeType.GetString(uk.EncodeKey);

                    rsa.FromXmlString(k);
                }

                var tmp = ByteString(tmass);

                urbytes = new crypt_struct(rsa.Decrypt(tmass, true));    //дешифрованный шифр   //тут все правильно!
            }

            //дешифрую основной и первый пароль вторым шифром
            {
                int Start = 8 + enckeyLength;
                int End = mass.Length - 4;
                for (int i = Start; i < End; i++)
                { mass[i] = urbytes.Decrypt(mass[i]); }
            }

            {
                int mrpLength;   //длина внутреннего пароля
                fixed (void* numPtr = &mass[4])
                { mrpLength = *(int*)numPtr; }

                var tmass = new byte[mrpLength];
                Array.Copy(mass, 8 + enckeyLength, tmass, 0, tmass.Length);

                mrpass = new crypt_struct(tmass);    //внутренний пароль смещения
            }

            //дешифрую основной набор первым шифром
            {
                int Start = mrpass.mass.Length + enckeyLength + 8;
                int End = mass.Length - 4;
                for (int i = Start; i < End; i++)
                { mass[i] = mrpass.Decrypt(mass[i]); }
            }
            {
                int MassFrom = mrpass.mass.Length + enckeyLength + 8;
                int MassLength = mass.Length - MassFrom;
                Array.Copy(mass, MassFrom, mass, 0, MassLength);
                Array.Resize(ref mass, MassLength);
            }
            var mark = Encoding.UTF32.GetString(mass, 0, MarkByteCount);

            return mark == Mark;
        }

        /// <summary>получить шифрованое сообщение</summary>
        public unsafe byte[] GetEncrypted(uint SPoolID)
        {
            var msg = GetMessage(SPoolID);

            if (msg == null)
            { return null; }

            Encode(T.SPool.Rows.Get_UnShow<uint>(SPoolID, C.SPool.AUser), ref msg);

            return msg;
        }
        /// <summary>Получить шифрованное сообщение</summary>
        /// <returns></returns>
        public unsafe byte[] GetEncrypted()
        {
            return GetEncrypted(LastPoolID);
        }
        /// <summary>Получить не шифрованое сообщение</summary>
        public unsafe byte[] GetMessage(uint SPoolID)
        {
            byte[] message;
            var UTables = new List<DataBase.ISTable>();

            {
                int Length = MarkByteCount + sizeof(int) + sizeof(uint) + sizeof(long); //метка + количество таблиц+userID+дата создания пула

                for (int i = 0; i < UTable.Length; i++)
                {
                    if (UTable[i].Use)
                    {
                        UTable[i].Table.QUERRY(DataBase.State.None).SHOW.WHERE.C(UTable[i].Table.Parent.Columns.Count - 1, SPoolID).DO();

                        if (UTable[i].Table.Rows.Count > 0)
                        {
                            UTables.Add(UTable[i].Table);
                            Length += sizeof(int) * 3 + Encoding.UTF32.GetByteCount(UTable[i].Table.Parent.Name); //длина имени + кол-во записей + общая длина данных по записям + имя таблицы

                            int RowLength = sizeof(int) + sizeof(int) + sizeof(byte);  //длина впереди идущей записи + идентификатор + метка об удалении

                            for (int j = 0; j < UTable[i].Table.Parent.Columns.Count - 1; j++)
                            {
                                if (!UTable[i].Table.Parent.GetColumn(j).Protect)
                                { RowLength += UTable[i].Table.Parent.GetColumn(j).BinarLength * 3; }
                            }

                            Length += RowLength * UTable[i].Table.Rows.Count;
                        }
                    }
                }

                if (UTables.Count == 0)
                { return null; }
                else
                { message = new byte[Length]; }
            }

            //Метка
            var SPos = 0;
            Encoding.UTF32.GetBytes(Mark, 0, Mark.Length, message, SPos);   //запись
            SPos += MarkByteCount;

            //кол-во таблиц
            fixed (void* numPtr = &message[SPos])
            { *(int*)numPtr = UTables.Count; }  //запись
            SPos += sizeof(int);

            for (int i = 0; i < UTables.Count; i++)
            {
                //длина имени таблицы
                fixed (void* numPtr = &message[SPos])
                { *(int*)numPtr = Encoding.UTF32.GetByteCount(UTables[i].Parent.Name); }  //запись
                SPos += sizeof(int);
                //имя таблицы
                Encoding.UTF32.GetBytes(UTables[i].Parent.Name, 0, UTables[i].Parent.Name.Length, message, SPos);   //запись
                SPos += Encoding.UTF32.GetByteCount(UTables[i].Parent.Name);

                //количество записей
                var TR = UTables[i].Rows;
                fixed (void* numPtr = &message[SPos])
                { *(int*)numPtr = TR.Count; }  //запись
                SPos += sizeof(int);

                //длина записей таблицы
                int RowsDataLengthPos = SPos;
                SPos += sizeof(int);

                for (int j = 0; j < TR.Count; j++)
                {
                    //длина записи таблицы
                    int RowDataLengthPos = SPos;
                    SPos += sizeof(int);

                    //идентификатор
                    fixed (void* numPtr = &message[SPos])
                    { *(uint*)numPtr = TR.GetID(j); }  //запись
                    SPos += sizeof(uint);

                    //метка об удалении
                    fixed (void* numPtr = &message[SPos])
                    { *(DataBase.State*)numPtr = TR.GetStatus(j); }  //запись
                    SPos += sizeof(DataBase.State);

                    for (int k = 0; k < UTables[i].Parent.Columns.Count - 1; k++)
                    {
                        if (!UTables[i].Parent.GetColumn(k).Protect)
                        {
                            switch (UTables[i].Parent.GetColumn(k).TypeCol)
                            {
                                case DataBase.Types.Bool:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(bool*)numPtr = TR.Get_UnShow<bool>(j, k); }  //запись
                                    SPos += sizeof(bool);
                                    break;
                                case DataBase.Types.AutoStatus:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(bool*)numPtr = TR.Get_UnShow<bool>(j, k); }  //запись
                                    SPos += sizeof(bool);
                                    break;
                                case DataBase.Types.Byte:
                                    message[SPos] = TR.Get_UnShow<byte>(j, k);
                                    SPos += sizeof(byte);
                                    break;
                                case DataBase.Types.DateTime:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(long*)numPtr = TR.Get_UnShow<DateTime>(j, k).Ticks; }  //запись
                                    SPos += sizeof(long);
                                    break;
                                case DataBase.Types.Double:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(double*)numPtr = TR.Get_UnShow<double>(j, k); }   //запись
                                    SPos += sizeof(double);
                                    break;
                                case DataBase.Types.Decimal:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(decimal*)numPtr = TR.Get_UnShow<decimal>(j, k); }   //запись
                                    SPos += sizeof(decimal);
                                    break;
                                case DataBase.Types.Int64:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(long*)numPtr = TR.Get_UnShow<long>(j, k); }  //запись
                                    SPos += sizeof(long);
                                    break;
                                case DataBase.Types.Int32:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(int*)numPtr = TR.Get_UnShow<int>(j, k); }   //запись
                                    SPos += sizeof(int);
                                    break;
                                case DataBase.Types.RIU32:
                                    fixed (void* numPtr = &message[SPos])
                                    { *(uint*)numPtr = TR.Get_UnShow<uint>(j, k); }  //запись
                                    SPos += sizeof(uint);
                                    break;
                                case DataBase.Types.String://тут надо бдительничать!
                                    var Value = TR.Get_UnShow<string>(j, k);

                                    int StrLength = Encoding.UTF32.GetByteCount(Value); //реальное кол-во байтов

                                    fixed (void* numPtr = &message[SPos])
                                    { *(int*)numPtr = StrLength; }  //запись
                                    SPos += sizeof(int);

                                    Encoding.UTF32.GetBytes(Value, 0, Value.Length, message, SPos);   //запись
                                    SPos += StrLength;
                                    break;
                                default: throw new Exception("не поддерживаемый тип");
                            }
                        }
                    }
                    //длина записи таблицы
                    fixed (void* numPtr = &message[RowDataLengthPos])
                    { *(int*)numPtr = SPos - RowDataLengthPos - sizeof(int); }  //без учета длины RowDataLengthPos
                }

                //длина записей таблицы
                fixed (void* numPtr = &message[RowsDataLengthPos])
                { *(int*)numPtr = SPos - RowsDataLengthPos - sizeof(int); }   //без учета длины RowsDataLengthPos

            }

            Array.Resize(ref message, SPos + 4 + 8);

            fixed (void* numPtr = &message[SPos])
            { *(long*)numPtr = T.SPool.Rows.Get<DateTime>(SPoolID, C.SPool.Date).Ticks; }   //запись
            SPos += sizeof(long);

            fixed (void* numPtr = &message[SPos])
            { *(uint*)numPtr = T.SPool.Rows.Get_UnShow<uint>(SPoolID, C.SPool.AUser); }  //запись

            return message;
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

                public bool SaveToDB()
                {
                    //искать и заменять по индексерам полей неполучится, т.к. записи приходят уже измененные и невозможно определить чем оно там было до изменения
                    //измененная запись придет в своём измененном виде
                    //если происходят многократные изменения ключевых полей, тогда хреново, т.к. будут приходить многократные дубликаты с измененными ключевыми полями

                    var RowExist = (bool)Parent.Table.QUERRY(DataBase.State.None).EXIST.WHERE.ID(this.ID).DO()[0].Value;

                    if (RowExist)
                    {
                        var Set = Parent.Table.QUERRY(DataBase.State.None).SET;

                        switch (InUse)
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

                            for (int i = 0; i < Values.Length; i++)
                            {
                                if (Values[i] != null)
                                { Set.C(i, Values[i]); }
                            } 
                            
                            Set.WHERE.ID(ID).DO();
                        }
                        catch (Exception ex)
                        {
                            DataBase.ATSettings.AllowQuerryAutoConvertTypes = false;

                            if (MessageBox.Show("В процессе добавления записи номер " + ID.ToString() + " возникла ошибка:" + ex.Message.ToString() + "\nВы хотите продолжить загрузку?", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                            { return false; }
                        }

                        DataBase.ATSettings.AllowQuerryAutoConvertTypes = false;
                    }
                    else
                    {
                        try
                        { return Parent.Table.Rows.Add(ID, Values, InUse); }
                        catch (Exception ex)
                        {
                            if (MessageBox.Show("В процессе добавления записи номер " + ID.ToString() + " возникла ошибка:" + ex.Message.ToString() + "\nВы хотите продолжить загрузку?", "Ошибка", MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                            { return false; }
                        }
                    }

                    return true;
                }

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

        /// <summary>Загрущить шифрованное сообщение</summary>
        /// <param name="mass">Шифрованный массив</param>
        /// <param name="UseProgressForm">Использовать визульную форму загрузки</param>
        /// <param name="ForceUserID">Использовать идентификатор пользователя насильно</param>
        /// <param name="ForceLoad">Загрузить не смотря на ограничения</param>
        /// <returns></returns>
        public unsafe string LoadCrypted(byte[] mass, bool UseProgressForm, uint ForceUserID = 0, bool ForceLoad = false)
        {
            Table_class[] Tables;
            uint UserID;
            DateTime DT;
            bool SPoolExisted;
            var Returned = this.GetValues(ForceUserID, mass, out Tables, out UserID, out DT, out SPoolExisted, true);

            if (Returned == null)
            {
                if (SPoolExisted && !ForceLoad)
                { return "Метка синхронизаций уже существует(" + UserID.ToString() + ", " + DT.ToString() + ")"; }

                if (UseProgressForm)
                {
                    var loading = new LoadSPool_class(Tables);

                    new Progress_Form(loading).ShowDialog();

                    if (loading.Returning != null)
                    { return loading.Returning; }
                }
                else
                {
                    for (int i = 0; i < Tables.Length; i++)
                    {
                        Tables[i].Table.Parent.Rows.CanUseEvents = Tables[i].Table.Parent.Rows.AllowGridUpdateEvents = false;

                        for (int j = 0; j < Tables[i].Count; j++)
                        {
                            if (!Tables[i][j].SaveToDB())
                            { return "Загрузка остановлена пользователем"; }
                        }

                        Tables[i].Table.Parent.Rows.CanUseEvents = Tables[i].Table.Parent.Rows.AllowGridUpdateEvents = true;
                    }
                }

                return null;
            }
            else
            { return Returned; }
        }

        class LoadSPool_class : Progress_Form.AObject
        {
            public LoadSPool_class(Table_class[] Tables) :
                base(false)
            { this.Tables = Tables; }

            Table_class[] Tables;
            public string Returning = null;

            protected override bool Do()
            {
                var MaxCount = 0;
                var CurCount = 0;

                for (int i = 0; i < Tables.Length; i++)
                { MaxCount += Tables[i].Rows.Length; }

                Action(MaxCount, 0);

                for (int i = 0; i < Tables.Length; i++)
                {
                    Action(Tables[i].Table.Name, MaxCount, CurCount);

                    Tables[i].Table.Parent.Rows.CanUseEvents = Tables[i].Table.Parent.Rows.AllowGridUpdateEvents = false;

                    for (int j = 0; j < Tables[i].Count; j++)
                    {
                        Action(MaxCount, ++CurCount);

                        if (!Tables[i][j].SaveToDB())
                        {
                            Returning = "Загрузка остановлена пользователем";
                            return false;
                        }
                    }

                    Tables[i].Table.Parent.Rows.CanUseEvents = Tables[i].Table.Parent.Rows.AllowGridUpdateEvents = true;
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
        public unsafe string GetValues(uint ForceUserID, byte[] mass, out Table_class[] Tables, out uint UserID, out DateTime SynchDT, out bool SPoolExisted, bool CheckSPoolExist = false)
        {
            SPoolExisted = false;

            if (mass == null)
            {
                UserID = 0;
                SynchDT = new DateTime();
                Tables = null;
                return "Сообщение не найдено";
            }
            else if (Decode(ref mass, ForceUserID))
            {
                int SPos = MarkByteCount;

                uint SPoolID = 0;

                {
                    fixed (void* numPtr = &mass[mass.Length - 4])
                    { UserID = *(uint*)numPtr; }

                    fixed (void* numPtr = &mass[mass.Length - 8 - 4])
                    { SynchDT = new DateTime(*(long*)numPtr); }

                    if (CheckSPoolExist)
                    {
                        //такой пул уже существует, загрузка не требуется
                        SPoolExisted = (bool)SPool.QUERRY().EXIST.WHERE.C(C.SPool.SUser, UserID).AND.C(C.SPool.Date, SynchDT).DO()[0].Value;

                        if (!SPoolExisted)
                        {
                            SPool.QUERRY()
                                .ADD
                                    .C(C.SPool.AUser, UserID)
                                    .C(C.SPool.SUser, UserID)
                                    .C(C.SPool.Local, false)
                                    .C(C.SPool.Date, SynchDT)
                                .DO();
                            SPoolID = SPool.Rows.GetID(SPool.Rows.Count - 1);
                        }
                    }
                }

                {
                    int TableCount;
                    fixed (void* numPtr = &mass[SPos])
                    { TableCount = *(int*)numPtr; } //количество таблиц
                    SPos += 4;
                    Tables = new Table_class[TableCount];
                }

                //гружу записи
                for (int i = 0; i < Tables.Length; i++)
                {
                    DataBase.ISTable Table;
                    DataBase.ITable Tbl;
                    {
                        string TableName;
                        int TableNameLength;
                        fixed (void* numPtr = &mass[SPos])
                        { TableNameLength = *(int*)numPtr; } //длина имени таблицы
                        SPos += sizeof(int);
                        TableName = Encoding.UTF32.GetString(mass, SPos, TableNameLength);  //имя таблицы
                        SPos += TableNameLength;

                        Tbl = data.T1.Tables[TableName];
                        if (Tbl == null)
                        { return "Таблица не найдена: \"" + TableName + "\""; }

                        Table = Tbl.GetSubTable[Tbl.GetSubTable.Count - 1];
                    }
                    int RowCount;
                    fixed (void* numPtr = &mass[SPos])
                    { RowCount = *(int*)numPtr; }
                    SPos += sizeof(int);

                    int RowsDataLength, CheckRowsDataLength = 0;
                    fixed (void* numPtr = &mass[SPos])
                    { RowsDataLength = *(int*)numPtr; } //длина данных
                    SPos += sizeof(int);

                    Tables[i] = new Table_class(Table, SPoolID, RowCount);

                    for (int j = 0; j < RowCount; j++)
                    {
                        CheckRowsDataLength += sizeof(uint) + sizeof(int) + sizeof(DataBase.State);   //id+метка об удалении + длина записи
                        int RowDataLength, CheckRowDataLength = sizeof(uint) + sizeof(DataBase.State);  //id+метка об удалении

                        fixed (void* numPtr = &mass[SPos])
                        { RowDataLength = *(int*)numPtr; }  //длина следующей записи
                        SPos += sizeof(int);

                        uint ID;
                        fixed (void* numPtr = &mass[SPos])
                        { ID = *(uint*)numPtr; }  //идентификатор
                        SPos += sizeof(uint);

                        DataBase.State InUse;
                        fixed (void* numPtr = &mass[SPos])
                        { InUse = *(DataBase.State*)numPtr; }  //метка об удалении
                        SPos += sizeof(DataBase.State);

                        object[] Values;
                        int ColumnIndex = 0;

                        Values = new object[Tbl.Columns.Count];
                        //Values[Values.Length - 1] = SPoolID;

                        Tables[i][j] = new Table_class.Row_class(Tables[i], ID, InUse, Values);

                        //тут важно, чтобы у всех таблиц были колонки relation spool и располагались в конце
                        for (int k = 0; k < Tbl.Columns.Count - 1; k++)
                        {
                            if (!Tbl.GetColumn(k).Protect)
                            {
                                switch (Tbl.GetColumn(k).TypeCol)
                                {
                                    case DataBase.Types.Bool:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = *(bool*)numPtr; }
                                        SPos += sizeof(bool);
                                        CheckRowDataLength += sizeof(bool);
                                        CheckRowsDataLength += sizeof(bool);
                                        break;
                                    case DataBase.Types.AutoStatus:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = (*(bool*)numPtr ? DataBase.AutoStatus.Used : DataBase.AutoStatus.UnUse); }
                                        SPos += sizeof(bool);
                                        CheckRowDataLength += sizeof(bool);
                                        CheckRowsDataLength += sizeof(bool);
                                        break;
                                    case DataBase.Types.Byte:
                                        Values[k] = mass[SPos];
                                        SPos += sizeof(byte);
                                        CheckRowDataLength += sizeof(byte);
                                        CheckRowsDataLength += sizeof(byte);
                                        break;
                                    case DataBase.Types.DateTime:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = new DateTime(*(long*)numPtr); }
                                        SPos += sizeof(long);
                                        CheckRowDataLength += sizeof(long);
                                        CheckRowsDataLength += sizeof(long);
                                        break;
                                    case DataBase.Types.Double:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = *(double*)numPtr; }
                                        SPos += sizeof(double);
                                        CheckRowDataLength += sizeof(double);
                                        CheckRowsDataLength += sizeof(double);
                                        break;
                                    case DataBase.Types.Decimal:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = *(decimal*)numPtr; }
                                        SPos += sizeof(decimal);
                                        CheckRowDataLength += sizeof(decimal);
                                        CheckRowsDataLength += sizeof(decimal);
                                        break;
                                    case DataBase.Types.Int64:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = *(long*)numPtr; }
                                        SPos += sizeof(long);
                                        CheckRowDataLength += sizeof(long);
                                        CheckRowsDataLength += sizeof(long);
                                        break;
                                    case DataBase.Types.Int32:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = *(int*)numPtr; }
                                        SPos += sizeof(int);
                                        CheckRowDataLength += sizeof(int);
                                        CheckRowsDataLength += sizeof(int);
                                        break;
                                    case DataBase.Types.RIU32:
                                        fixed (void* numPtr = &mass[SPos])
                                        { Values[k] = new RIU32(*(uint*)numPtr); }
                                        SPos += sizeof(uint);
                                        CheckRowDataLength += sizeof(uint);
                                        CheckRowsDataLength += sizeof(uint);
                                        break;
                                    case DataBase.Types.String://тут надо бдительничать!
                                        int StrLength;
                                        fixed (void* numPtr = &mass[SPos])
                                        { StrLength = *(int*)numPtr; }
                                        SPos += sizeof(int);
                                        CheckRowDataLength += sizeof(int);
                                        CheckRowsDataLength += sizeof(int);

                                        Values[k] = Encoding.UTF32.GetString(mass, SPos, StrLength);
                                        SPos += StrLength;
                                        CheckRowDataLength += StrLength;
                                        CheckRowsDataLength += StrLength;
                                        break;
                                    default: throw new Exception("не поддерживаемый тип");
                                }
                                ColumnIndex++;
                            }
                        }

                        if (CheckRowDataLength != RowDataLength)
                        { return "Сообщение повреждено: " + Tables[i].Table.Name + ' ' + (j + 1).ToString() + "-" + ID.ToString() + " CheckRowDataLength != RowDataLength(" + CheckRowDataLength.ToString() + "!=" + RowDataLength.ToString() + ")"; }
                    }

                    if (CheckRowsDataLength != RowsDataLength)
                    { return "Сообщение повреждено: " + Tables[i].Table.Name + " CheckRowsDataLength != RowsDataLength(" + CheckRowsDataLength.ToString() + "!=" + RowsDataLength.ToString() + ")"; }
                }

                return null;
            }
            else
            {
                CheckSPoolExist = false;
                UserID = 0;
                SynchDT = new DateTime();
                Tables = null;
                return "Не удалось декодировать сообщение";
            }
        }

        /// <summary>Получить шифрованное сообщение</summary>
        /// <param name="mass">Шифруемое сообещение</param>
        /// <param name="ForceUserID">Использовать идентификатор пользователя насильно</param>
        /// <returns></returns>
        public unsafe string ShowCrypted(byte[] mass, uint ForceUserID = 0)
        {
            Table_class[] Tables;
            uint UserID;
            DateTime DT;
            bool SPoolExisted;
            var Returned = this.GetValues(ForceUserID, mass, out Tables, out UserID, out DT, out SPoolExisted, false);

            if (Returned == null)
            {
                new Tables_Form(Tables, UserID, DT).ShowDialog();
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
