using LaboratoryOnlineJournal;
using LaboratoryOnlineJournal.SerializeFormatProvider;
using LaboratoryOnlineJournal.SerializeProvider;
using LaboratoryOnlineJournal.Synch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LOJ_test
{
    [TestClass]
    public class OldFormatProviderTest
    {
        private static DataBase DataBase;

        [ClassInitialize]
        public static void Initialize(TestContext testContext)
        {
            Misc.CheckTablesExist = false;

            DataBase = new DataBase("TestData\\loj", Encoding.GetEncoding(866));

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

        private RSACryptoServiceProvider GetRsa(uint userID)
        {
            var rsa = new RSACryptoServiceProvider();

            var ck = TestData.ClosedKeys.Users[userID];

            rsa.FromXmlString(ck);

            return rsa;
        }

        [TestMethod]
        public void OldFormatProviderFilesCheck()
        {
            var files = System.IO.Directory.GetFiles("TestData\\TestFiles");

            var oldSerializerFormatProvider = new OldSerializeFormatProvider(Encoding.UTF32, DataBase);

            Assert.IsTrue(files.Any(), "there are no test files");

            foreach (var file in files)
            {
                var fileInfo = new System.IO.FileInfo(file);

                var bytes = new Byte[fileInfo.Length];

                using (var stream = fileInfo.OpenRead())
                { stream.Read(bytes, 0, bytes.Length); }

                DeserializeResult deserializeResult;

                var result = oldSerializerFormatProvider.TryDecodeData(GetRsa, bytes, out deserializeResult);

                Assert.IsTrue(result);

                //var resultBytes = oldSerializerFormatProvider.
            }
        }
    }
}