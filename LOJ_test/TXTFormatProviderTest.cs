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
    public class TXTFormatProviderTest
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

        private bool GetRsa(uint userID, out RSACryptoServiceProvider rsa)
        {
            var result = false;
            rsa = null;

            if (!TestData.ClosedKeys.Users.ContainsKey(userID))
            {
                rsa = new RSACryptoServiceProvider();
                var ck = TestData.ClosedKeys.Users[userID];

                rsa.FromXmlString(ck);
            }

            return result;
        }

        [TestMethod]
        public void NewFormatProviderFilesCheck()
        {
            var oldSerializerFormatProvider = new TXTSerializeFormatProvider(DataBase);

            var fileInfo = new System.IO.FileInfo("TestData\\TXTTestFiles\\testData.txt");

            var bytes = new Byte[fileInfo.Length];

            using (var stream = fileInfo.OpenRead())
            { stream.Read(bytes, 0, bytes.Length); }

            DeserializeResult deserializeResult;

            var result = oldSerializerFormatProvider.TryDecodeData(GetRsa, bytes, out deserializeResult);

            Assert.IsTrue(result);
            Assert.IsNotNull(deserializeResult);
        }

        /// <summary>
        /// Проверяю, что новый провайдер формата будет выдавать отрицательный результат, при попытке понять, что файл старого формата ему не подходит
        /// </summary>
        [TestMethod]
        public void TXTFormatProviderFalseFilesCheck()
        {
            var files = System.IO.Directory.GetFiles("TestData\\OldTestFiles");

            var oldSerializerFormatProvider = new TXTSerializeFormatProvider(DataBase);

            Assert.IsTrue(files.Any(), "there are no test files");

            foreach (var file in files)
            {
                var fileInfo = new System.IO.FileInfo(file);

                var bytes = new Byte[fileInfo.Length];

                using (var stream = fileInfo.OpenRead())
                { stream.Read(bytes, 0, bytes.Length); }

                DeserializeResult deserializeResult;

                var result = oldSerializerFormatProvider.TryDecodeData(GetRsa, bytes, out deserializeResult);

                Assert.IsFalse(result);
                Assert.IsNull(deserializeResult);
            }
        }
    }
}