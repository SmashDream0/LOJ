using LaboratoryOnlineJournal.FormatChecker;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LaboratoryOnlineJournal.SerializeProvider
{
    public class TXTSerializeProvider : TXTFormatChecker, ISerializeProvider
    {
        static TXTSerializeProvider()
        {
            _replaceVariants = new ReplaceVariant[]
                {
                    new ReplaceVariant(@"\\", "\\"),
                    new ReplaceVariant(@"\r", "\r"),
                    new ReplaceVariant(@"\n", "\n"),
                    new ReplaceVariant("<¬>", "┐"),
                    new ReplaceVariant("<¦>", "│"),
                    //new ReplaceVariant("•", ","),
                };
        }

        public TXTSerializeProvider(Encoding encoding, DataBase dataBase) : base(encoding)
        {
            _dataBase = dataBase;
        }

        private readonly String _separator = "\t";
        private readonly DataBase _dataBase;
        public String Name => "TXT";

        private static readonly IEnumerable<ReplaceVariant> _replaceVariants;

        public unsafe byte[] Serialize(IEnumerable<DataBase.ISTable> tables, DateTime date, uint userID)
        {
            var txt = SerializeToString(tables, date, userID);

            var result = Encoding.GetBytes(txt);

            return result;
        }

        public String SerializeToString(IEnumerable<DataBase.ISTable> tables, DateTime date, uint userID)
        {
            var sb = new StringBuilder();

            sb.AppendLine(Mark);

            sb.Append(userID); sb.AppendLine();
            sb.Append(date); sb.AppendLine();

            foreach (var table in tables)
            {
                if (table.Rows.Count > 0)
                {
                    sb.AppendLine(table.Parent.Name);

                    var columns = new List<DataBase.IColumn>();

                    for (int i = 0; i < table.Parent.Columns.Count; i++)
                    {
                        var column = table.Parent.GetColumn(i);

                        if (!column.Protect)
                        { columns.Add(column); }
                    }

                    sb.Append("ID"); sb.Append(_separator);
                    sb.Append("Status"); sb.Append(_separator);

                    foreach (var column in columns)
                    {
                        sb.Append(column.Name); sb.Append(_separator);
                    }

                    sb.Length -= _separator.Length;
                    sb.AppendLine();

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        sb.Append(table.Rows.GetID(i)); sb.Append(_separator);
                        sb.Append(table.Rows.GetStatus(i)); sb.Append(_separator);

                        foreach (var column in columns)
                        {
                            if (!column.Protect)
                            {
                                object value;

                                switch (column.TypeCol)
                                {
                                    case DataBase.Types.Bool:
                                        value = table.Rows.Get_UnShow<bool>(i, column.Index);
                                        break;
                                    case DataBase.Types.AutoStatus:
                                        value = (table.Rows.Get_UnShow<bool>(i, column.Index) ? DataBase.AutoStatus.Used : DataBase.AutoStatus.UnUse);
                                        break;
                                    case DataBase.Types.Byte:
                                        value = table.Rows.Get_UnShow<byte>(i, column.Index);
                                        break;
                                    case DataBase.Types.DateTime:
                                        var dt = table.Rows.Get_UnShow<DateTime>(i, column.Index);

                                        value = dt.ToString("dd-MM-yyyy HH.mm.ss.fffffff");
                                        break;
                                    case DataBase.Types.Double:
                                        value = table.Rows.Get_UnShow<double>(i, column.Index);
                                        value = ReplaceFront(value.ToString());
                                        break;
                                    case DataBase.Types.Decimal:
                                        value = table.Rows.Get_UnShow<decimal>(i, column.Index);
                                        value = ReplaceFront(value.ToString());
                                        break;
                                    case DataBase.Types.Int64:
                                        value = table.Rows.Get_UnShow<long>(i, column.Index);
                                        break;
                                    case DataBase.Types.Int32:
                                        value = table.Rows.Get_UnShow<int>(i, column.Index);
                                        break;
                                    case DataBase.Types.RIU32:
                                        value = table.Rows.Get_UnShow<uint>(i, column.Index);
                                        break;
                                    case DataBase.Types.String:
                                        value = table.Rows.Get_UnShow<string>(i, column.Index);

                                        value = ReplaceFront(value.ToString());
                                        break;
                                    default: throw new Exception("не поддерживаемый тип");
                                }

                                sb.Append(value); sb.Append(_separator);
                            }
                        }

                        sb.Length -= _separator.Length;
                        sb.AppendLine();
                    }

                    sb.AppendLine();
                }
            }

            var result = sb.ToString();

            return result;
        }

        public unsafe DeserializeResult Deserialize(byte[] mass)
        {
            var tables = new List<Table>();
            uint userID;
            DateTime synchDT;

            var txt = Encoding.GetString(mass);
            using (var stringReader = new StringReader(txt))
            {
                var mark = stringReader.ReadLine().Trim();

                var userIDStr = stringReader.ReadLine().Trim();
                if (!uint.TryParse(userIDStr, out userID))
                { throw new Exception($"не удалось получить User ID"); }

                var synchDateStr = stringReader.ReadLine().Trim();
                if (!DateTime.TryParse(synchDateStr, out synchDT))
                { throw new Exception($"не удалось получить Synch Date"); }

                while (stringReader.Peek() >= 0)
                {
                    var tableName = stringReader.ReadLine().Trim();

                    var table = _dataBase.Tables[tableName].CreateSubTable(false);
                    if (table == null)
                    { throw new Exception($"Таблица не найдена: \"{tableName}\""); }

                    var columns = ParseColumns(stringReader);

                    var rows = ParseRows(stringReader, columns, table);

                    tables.Add(new Table() { STable = table, Rows = rows });
                }
            }

            return new DeserializeResult() { Tables = tables.ToArray(), SynchDate = synchDT, UserID = userID };
        }

        private IEnumerable<string> ParseColumns(StringReader stringReader)
        {
            var line = stringReader.ReadLine();

            var result = line.Split(new string[] { _separator }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray();

            return result;
        }

        private Row[] ParseRows(StringReader stringReader, IEnumerable<string> columns, DataBase.ISTable table)
        {
            var rows = new List<Row>();

            var columnArray = columns.ToArray();

            while (stringReader.Peek() >= 0)
            {
                var line = stringReader.ReadLine();

                var items = line.Split(new string[] { _separator }, StringSplitOptions.None);

                if (!items.Any() || items.Length < 3)
                {
                    break; 
                }

                var values = new object[table.Parent.Columns.Count];

                var id = uint.Parse(items[0]);
                var inUse = (DataBase.State)Enum.Parse(typeof(DataBase.State), items[1], true);

                for (int i = 2; i < columnArray.Length; i++)
                {
                    var columnName = columnArray[i];

                    var column = table.Parent.GetColumn(columnName);

                    var item = items[i];

                    var value = ParseField(column, item);

                    values[column.Index] = value;
                }

                rows.Add(new Row(id, inUse, values));
            }

            return rows.ToArray();
        }

        private object ParseField(DataBase.IColumn column, string txt)
        {
            object result = null;

            switch (column.TypeCol)
            {
                case DataBase.Types.Bool:
                    result = bool.Parse(txt);
                    break;
                case DataBase.Types.AutoStatus:
                    {
                        bool boolResult;
                        if (bool.TryParse(txt, out boolResult))
                        { result = (boolResult ? DataBase.AutoStatus.Used : DataBase.AutoStatus.UnUse); }
                        else
                        { result = Enum.Parse(typeof(DataBase.AutoStatus), txt, true); }
                    }
                    break;
                case DataBase.Types.Byte:
                    result = byte.Parse(txt);
                    break;
                case DataBase.Types.DateTime:
                    result = DateTime.ParseExact(txt, "dd-MM-yyyy HH.mm.ss.fffffff", CultureInfo.InvariantCulture);
                    break;
                case DataBase.Types.Double:
                    {
                        var str = ReplaceBackward(txt);
                        result = Double.Parse(str);
                    }
                    break;
                case DataBase.Types.Decimal:
                    {
                        var str = ReplaceBackward(txt);
                        result = Decimal.Parse(str);
                    }
                    break;
                case DataBase.Types.Int64:
                    result = Int64.Parse(txt);
                    break;
                case DataBase.Types.Int32:
                    result = Int32.Parse(txt);
                    break;
                case DataBase.Types.RIU32:
                    result = new RIU32(UInt32.Parse(txt));
                    break;
                case DataBase.Types.String:
                    result = ReplaceBackward(txt);
                    break;
                default: throw new Exception("не поддерживаемый тип");
            }

            return result;
        }

        private static String ReplaceBackward(String txt)
        {
            var sb = new StringBuilder(txt);

            foreach (var replaceValiant in _replaceVariants)
            { sb.Replace(replaceValiant.From, replaceValiant.To); }

            var result = sb.ToString();

            return result;
        }

        private static String ReplaceFront(String txt)
        {
            var sb = new StringBuilder(txt);

            foreach (var replaceValiant in _replaceVariants)
            { sb.Replace(replaceValiant.To, replaceValiant.From); }

            return sb.ToString();
        }

        private class ReplaceVariant
        {
            public ReplaceVariant(String from, String to)
            {
                From = from;
                To = to;
            }

            public String From { get; private set; }
            public String To { get; private set; }
        }
    }
}