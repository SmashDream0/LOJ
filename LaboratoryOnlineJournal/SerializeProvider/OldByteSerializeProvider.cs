using LaboratoryOnlineJournal.FormatChecker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeProvider
{
    public class OldByteSerializeProvider : OldFormatChecker, ISerializeProvider
    {
        public OldByteSerializeProvider(Encoding encoding, DataBase dataBase) : base(encoding)
        {
            _dataBase = dataBase;
        }

        private readonly DataBase _dataBase;

        public String Name => "Old";

        /// <summary>Получить не шифрованое сообщение</summary>
        public unsafe byte[] Serialize(IEnumerable<DataBase.ISTable> tables, DateTime date, uint userID)
        {
            if (!tables.Any())
            { return null; }

            byte[] message;

            {
                int Length = Encoding.GetByteCount(Mark) + sizeof(int) + sizeof(uint) + sizeof(long); //метка + количество таблиц+userID+дата создания пула

                var lengths = new StringBuilder();

                foreach (var uTable in tables)
                {
                    if (uTable.Rows.Count > 0)
                    {
                        Length += sizeof(int) * 3 + Encoding.GetByteCount(uTable.Parent.Name); //длина имени + кол-во записей + общая длина данных по записям + имя таблицы

                        int RowLength = sizeof(int) + sizeof(int) + sizeof(byte);  //длина впереди идущей записи + идентификатор + метка об удалении

                        for (int j = 0; j < uTable.Parent.Columns.Count - 1; j++)
                        {
                            if (!uTable.Parent.GetColumn(j).Protect)
                            { RowLength += uTable.Parent.GetColumn(j).BinarLength * 3; }
                        }

                        var totalRowLengths = RowLength * uTable.Rows.Count;

                        lengths.AppendLine(uTable.Parent.Name + " " + totalRowLengths);

                        Length += totalRowLengths;
                    }
                }

                var str = lengths.ToString();

                message = new byte[Length];
            }

            //Метка
            var SPos = 0;
            Encoding.GetBytes(Mark, 0, Mark.Length, message, SPos);   //запись
            SPos += Encoding.GetByteCount(Mark);

            //кол-во таблиц
            fixed (void* numPtr = &message[SPos])
            { *(int*)numPtr = tables.Count(); }  //запись
            SPos += sizeof(int);

            foreach (var uTable in tables)
            {
                //длина имени таблицы
                fixed (void* numPtr = &message[SPos])
                { *(int*)numPtr = Encoding.GetByteCount(uTable.Parent.Name); }  //запись
                SPos += sizeof(int);
                //имя таблицы
                Encoding.GetBytes(uTable.Parent.Name, 0, uTable.Parent.Name.Length, message, SPos);   //запись
                SPos += Encoding.GetByteCount(uTable.Parent.Name);

                //количество записей
                var TR = uTable.Rows;
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

                    for (int k = 0; k < uTable.Parent.Columns.Count - 1; k++)
                    {
                        var column = uTable.Parent.GetColumn(k);

                        if (!column.Protect)
                        {
                            switch (column.TypeCol)
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

                                    int StrLength = Encoding.GetByteCount(Value); //реальное кол-во байтов

                                    fixed (void* numPtr = &message[SPos])
                                    { *(int*)numPtr = StrLength; }  //запись
                                    SPos += sizeof(int);

                                    Encoding.GetBytes(Value, 0, Value.Length, message, SPos);   //запись
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
            { *(long*)numPtr = date.Ticks; }   //запись
            SPos += sizeof(long);

            fixed (void* numPtr = &message[SPos])
            { *(uint*)numPtr = userID; }  //запись

            return message;
        }

        public unsafe DeserializeResult Deserialize(byte[] mass)
        {
            Table[] tables;
            uint userID;
            DateTime synchDT;
            int SPos = Encoding.GetByteCount(Mark);

            {
                fixed (void* numPtr = &mass[mass.Length - 4])
                { userID = *(uint*)numPtr; }

                fixed (void* numPtr = &mass[mass.Length - 8 - 4])
                { synchDT = new DateTime(*(long*)numPtr); }
            }

            {
                int TableCount;
                fixed (void* numPtr = &mass[SPos])
                { TableCount = *(int*)numPtr; } //количество таблиц
                SPos += 4;
                tables = new Table[TableCount];
            }

            //гружу записи
            for (int i = 0; i < tables.Length; i++)
            {
                DataBase.ISTable Table;
                DataBase.ITable Tbl;
                {
                    string TableName;
                    int TableNameLength;
                    fixed (void* numPtr = &mass[SPos])
                    { TableNameLength = *(int*)numPtr; } //длина имени таблицы
                    SPos += sizeof(int);
                    TableName = Encoding.GetString(mass, SPos, TableNameLength);  //имя таблицы
                    SPos += TableNameLength;

                    Tbl = _dataBase.Tables[TableName];
                    if (Tbl == null)
                    { throw new Exception($"Таблица не найдена: \"{TableName}\""); }

                    Table = Tbl.CreateSubTable(false);
                }

                int RowCount;
                fixed (void* numPtr = &mass[SPos])
                { RowCount = *(int*)numPtr; }
                SPos += sizeof(int);

                int RowsDataLength, CheckRowsDataLength = 0;
                fixed (void* numPtr = &mass[SPos])
                { RowsDataLength = *(int*)numPtr; } //длина данных
                SPos += sizeof(int);

                var rows = new Row[RowCount];

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

                    rows[j] = new Row(ID, InUse, Values);

                    //тут важно, чтобы у всех таблиц были колонки relation spool и располагались в конце
                    for (int k = 0; k < Tbl.Columns.Count - 1; k++)
                    {
                        var column = Tbl.GetColumn(k);

                        if (!column.Protect)
                        {
                            switch (column.TypeCol)
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

                                    Values[k] = Encoding.GetString(mass, SPos, StrLength);
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
                    { throw new Exception($"Сообщение повреждено: {tables[i].STable.Name} {(j + 1).ToString()}-{ID} CheckRowDataLength != RowDataLength({CheckRowDataLength}!={RowDataLength})"); }
                }

                tables[i] = new Table() { STable = Table, Rows = rows };

                if (CheckRowsDataLength != RowsDataLength)
                { throw new Exception($"Сообщение повреждено: {tables[i].STable.Name} CheckRowsDataLength != RowsDataLength({CheckRowsDataLength}!={RowsDataLength})"); }
            }

            return new DeserializeResult() { Tables = tables, SynchDate = synchDT, UserID = userID };
        }
    }
}