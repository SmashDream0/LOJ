using LaboratoryOnlineJournal;
using LaboratoryOnlineJournal.Serializer;
using LaboratoryOnlineJournal.Synch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LOJ_test
{
    [TestClass]
    public class OldSerializeProviderTest
    {
        private static DataBase DataBase;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Misc.CheckTablesExist = false;

            DataBase = new DataBase("loj", Encoding.GetEncoding(866));

            DataBase.UseMFT(".");

            Misc.DataBaseLoadFT(DataBase, null);
            data.SynchPool = null;
            Misc.DataBaseLoad(DataBase, null);

            var tables = new List<DataBase.ISTable>();

            var uTable = DataBase.Tables.First(x => x.Name == "UTable").CreateSubTable(false);

            uTable.QUERRY().SHOW.DO();

            foreach (var table in DataBase.Tables)
            {
                if (IsInUTable(uTable, table.Name))
                {
                    var sTable = table.CreateSubTable(false);

                    new SynchTable(sTable);
                }
            }

            DataBase.UseLocal();
        }

        [ClassCleanup]
        public static void Clean()
        {
            T.Clear();
            G.Clear();

            DataBase = null;
        }

        private IEnumerable<DataBase.ISTable> GetSTables()
        {
            var tables = new List<DataBase.ISTable>();

            foreach (var table in DataBase.Tables)
            {
                var lastColumn = table.Column.Last();

                if (lastColumn.Name == "SPool")
                {
                    var sTable = table.CreateSubTable(false);

                    new SynchTable(sTable);

                    sTable.QUERRY(DataBase.State.None).SHOW.DO();

                    if (sTable.Rows.Count > 0)
                    { tables.Add(sTable); }
                }
            }

            return tables.ToArray();
        }

        private static bool IsInUTable(DataBase.ISTable uTable, string tableName)
        {
            bool result = false;

            for (int i = 0; i < uTable.Rows.Count; i++)
            {
                var uTableName = uTable.Rows.Get<String>(i, C.UTable.Name);

                if (String.Equals(uTableName, tableName, StringComparison.InvariantCultureIgnoreCase))
                {
                    result = uTable.Rows.Get<bool>(i, C.UTable.Use);
                    break;
                }
            }

            return result;
        }

        private void CheckTables(IEnumerable<DataBase.ISTable> sTables, DeserializeResult deserializeResult)
        {
            Assert.AreEqual(sTables.Count(), deserializeResult.Tables.Count());

            var compares = Enumerable.Zip(deserializeResult.Tables, sTables, (x1, x2) => new { DTable = x1, STable = x2 });

            foreach (var compare in compares)
            {
                var rows = compare.DTable.Rows.ToArray();

                for (int i = 0; i < compare.STable.Rows.Count; i++)
                {
                    for (int j = 0; j < compare.STable.Parent.Columns.Count - 1; j++)
                    {
                        var column = compare.STable.Parent.GetColumn(j);

                        if (!column.Protect)
                        {
                            var row = compare.STable.Rows.Get_Row(i);

                            var rowValue = rows[i].Values[j];
                            var tableValue = compare.STable.Parent.Rows.GetObject_UnShow(row.ID, j);

                            if (rowValue != null && rowValue.GetType() == typeof(RIU32))
                            {
                                rowValue = (uint)(RIU32)rowValue;
                            }

                            try
                            {
                                Assert.AreEqual(rowValue, tableValue);
                            }
                            catch (Exception ex)
                            {
                                throw ex;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Проверяю работу нового объекта [де]сериализации, включающий старый алгоритм
        /// </summary>
        [TestMethod]
        public void OldSerializerTest()
        {
            var oldSerializerProvider = new OldByteSerializeProvider(Encoding.UTF32, DataBase);

            var tables = GetSTables();

            //var str = String.Join("\r\n", tables.Select(x => x.ToString()));

            var date = new DateTime(2019, 12, 11);
            uint userID = 5;

            var bytes = oldSerializerProvider.Serialize(tables, date, userID);

            var result = oldSerializerProvider.Deserialize(bytes);

            CheckTables(tables, result);
        }

        /// <summary>
        /// Сравниваю работу старого алгоритма из нового и старого объектов
        /// </summary>
        [TestMethod]
        public void OldSerializerCompareTest()
        {
            var tables = GetSTables();

            //var sPool = DataBase.Tables.First(x => x.Name == "SPool");

            //uint sPoolID;
            //{
            //    var values = new object[sPool.Columns.Count];
            //    values[C.SPool.AUser] = (uint)1;
            //    values[C.SPool.Date] = new DateTime(2019, 12, 14);
            //    values[C.SPool.Local] = true;

            //    sPoolID = sPool.DataSource.AddRow(405, values).ID;
            //}

            //foreach (var table in tables)
            //{
            //    for (int j = 0; j < table.Rows.Count; j++)
            //    {
            //        var id = table.Rows.GetID(j);

            //        table.Parent.Rows.Set(id, table.Parent.Columns.Count - 1, new RIU32(sPoolID));
            //    }
            //}

            //var uTable = DataBase.Tables.First(x => x.Name == "UTable");

            //var synchPool = new SynchPool_class(DataBase);

            //synchPool.Invalidate(uTable.CreateSubTable(false));

            //var bytes = synchPool.GetMessage(sPoolID);

            //var str = String.Join(", ", bytes);

            var date = new DateTime(2019, 12, 14);
            uint userID = 1;

            var oldSerializerProvider = new OldByteSerializeProvider(Encoding.UTF32, DataBase);

            var testBytes = oldSerializerProvider.Serialize(tables, date, userID);

            CollectionAssert.AreEqual(TestData1.MessageBytes, testBytes);
        }
    }
}