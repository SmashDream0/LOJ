using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Reflection;
using System.IO;
using LaboratoryOnlineJournal;
using LaboratoryOnlineJournal.Synch;
using LaboratoryOnlineJournal.SerializeProvider;

namespace LOJ_test
{
    [TestClass]
    public class OldTest
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

        /// <summary>Класс SPoints_class</summary>
        [TestMethod]
        public void Base()
        {
            var YM = ATMisc.GetYMFromYearMonth(2016, 7);

            var SPoints = new Employe_Form.SPoints_class(YM);

            uint PodrID = 19;

            SPoints.UpdateSPoints(PodrID, false, false);

            if (SPoints.YM != YM)
            { Exception("не верный номер периода - " + SPoints.YM.ToString()); }

            if (SPoints.SPointsCount != 4)
            { Exception("Не верное количество точек отбора - " + SPoints.SPointsCount.ToString()); }

            if (SPoints[0].SampleCount != 3)
            { Exception("не верное количество замеров у точки отбора 0 - " + SPoints[0].SampleCount.ToString()); }

            if (SPoints[1].SampleCount != 3)
            { Exception("не верное количество замеров у точки отбора 1 - " + SPoints[1].SampleCount.ToString()); }

            if (SPoints[2].SampleCount != 0)
            { Exception("не верное количество замеров у точки отбора 2 - " + SPoints[2].SampleCount.ToString()); }

            if (SPoints[3].SampleCount != 0)
            { Exception("не верное количество замеров у точки отбора 3 - " + SPoints[3].SampleCount.ToString()); }

            CheckSorts(SPoints);

            {
                var SPID = SPoints[0].SPointID;

                var sample = new List<uint>();
                var sms = new List<uint>();

                for (int i = 0; i < SPoints[0].SampleCount; i++)
                {
                    sample.Add(SPoints[0][i].SampleID);

                    for (int j = 0; j < SPoints[0][i].Marks.Length; j++)
                    {
                        if (SPoints[0][i].Marks[j].SMID > 0)
                        { sms.Add(SPoints[0][i].Marks[j].SMID); }
                    }
                }

                SPoints.DeleteSPoint(0);

                for (int i = 0; i < sample.Count; i++)
                {
                    if (T.Sample.Rows.Status(sample[i]) != DataBase.State.Deleted)
                    { Exception("Не верный статус записи замера после удаления точки отбора"); }
                }

                for (int i = 0; i < sms.Count; i++)
                {
                    if (T.SM.Rows.Status(sms[i]) != DataBase.State.Deleted)
                    { Exception("Не верный статус записи концентрации после удаления замера"); }
                }

                if (T.SPoint.Rows.Status(SPID) != DataBase.State.Deleted)
                { Exception("Удаленная запись имеет не верный статус"); }
            }

            if (SPoints[0].SampleCount != 3)
            { Exception("не верное количество замеров у точки отбора 0 - " + SPoints[0].SampleCount.ToString()); }

            if (SPoints[1].SampleCount != 0)
            { Exception("не верное количество замеров у точки отбора 1 - " + SPoints[1].SampleCount.ToString()); }

            if (SPoints[2].SampleCount != 0)
            { Exception("не верное количество замеров у точки отбора 2 - " + SPoints[2].SampleCount.ToString()); }

            CheckSorts(SPoints);

            {
                var SampleID = SPoints[0][0].SampleID;
                var sms = new List<uint>();

                for (int i = 0; i < SPoints[0][0].Marks.Length; i++)
                {
                    if (SPoints[0][0].Marks[i].SMID > 0)
                    { sms.Add(SPoints[0][0].Marks[i].SMID); }
                }

                SPoints[0].DeleteSample(0);

                if (T.Sample.Rows.Status(SampleID) != DataBase.State.Deleted)
                { Exception("Удаленная запись имеет не верный статус"); }

                for (int i = 0; i < sms.Count; i++)
                {
                    if (T.SM.Rows.Status(sms[i]) != DataBase.State.Deleted)
                    { Exception("Не верный статус записи концентрации после удаления замера"); }
                }
            }

            if (SPoints[0].SampleCount != 2)
            { Exception("не верное количество замеров у точки отбора 0 - " + SPoints[0].SampleCount.ToString()); }

            if (SPoints[1].SampleCount != 0)
            { Exception("не верное количество замеров у точки отбора 1 - " + SPoints[1].SampleCount.ToString()); }

            if (SPoints[2].SampleCount != 0)
            { Exception("не верное количество замеров у точки отбора 2 - " + SPoints[2].SampleCount.ToString()); }
        }

        /// <summary>Кодирование-декодирование сообщения</summary>
        [TestMethod]
        public void CoDeMessage()
        {
            //обустройство бд
            var DataBase = new DataBase("CeDoTest");

            data.UserID = 1;

            T.User = DataBase.Tables.Add("User", "User");
            T.User.Columns.AddString("Login", "Логин", 25);

            G.User = T.User.CreateSubTable();
            G.User.Rows.Add(new object[] { "test" });

            T.SPool = DataBase.Tables.Add("SPool", "Пул синхронизаций");
            T.SPool.Columns.AddRelation(T.User.GetColumn(C.User.Login), "A", T.User.AlterName + " автор");
            T.SPool.Columns.AddRelation(T.User.GetColumn(C.User.Login), "S", T.User.AlterName + " отправитель");
            T.SPool.Columns.AddBool("local", "создано локально");
            T.SPool.Columns.AddDATE("StartDate", "Дата создания");
            T.SPool.AutoSave(false, DataBase.TypeOfTable.Remote);

            var Table1 = DataBase.Tables.Add("tb1", "tb1");
            Table1.Columns.AddString("str", "str", 5);

            var Table2 = DataBase.Tables.Add("tb2", "tb2");
            Table2.Columns.AddAutoUpdate("au", "au");
            Table2.Columns.AddBool("bl", "bl");
            Table2.Columns.AddByte("bt", "bt");
            Table2.Columns.AddDATE("dt", "dt");
            Table2.Columns.AddDecimal("dc", "dc");
            Table2.Columns.AddDOUBLE("db", "db");
            Table2.Columns.AddInt32("i32", "i32");
            Table2.Columns.AddInt64("i64", "i64");
            Table2.Columns.AddString("str", "str", 150);
            Table2.Columns.AddRelation(Table1.GetColumn(0));

            T.UTable = DataBase.Tables.Add("UTable", "UTable");
            T.UTable.Columns.AddString("Name", "Наименование", 15);
            T.UTable.Columns.AddBool("Add", "Добавление", DataBase.ColLocation.Local, true, true);
            T.UTable.Columns.AddBool("Update", "Изменение", DataBase.ColLocation.Local, true, true);
            T.UTable.Columns.AddBool("Delete", "Удаление", DataBase.ColLocation.Local, true, true);
            T.UTable.Columns.AddBool("Use", "Задействовать", DataBase.ColLocation.Local, true, true);

            //данные
            var serializeProviders = Misc.GetSerializeProviders(DataBase);
            var synch = new SynchPoolManager(DataBase, serializeProviders.First().Name, serializeProviders);

            synch.KeysContainer.Add(1, "<RSAKeyValue><Modulus>vSHFY3X54bKGI0lc8djFZhqOH/a86oORTH6ulnUBnvSlpj65CtOPTLpwOkmfnSlLu64qyazFMg7IrFuW3fkiSsiiTCVZInIh50De+V/dadYwjc96KDDb9qn9wb3JMikSBCWNQwqTyEgT1vCrUuHebhx8rjc6laGkRpFnzzrUJbtktYcDqYYU/xYUUmYb19HhtQV0GrQtudZ54oEHeGz+tZtopyqyVPm0ZEHUh3xf6THRWg9Jd1hnBAhyjlCgaTIwA9D25D7hJA74fArVolIRIPG0JMV+plBC9sfNRmGfPoT082l3AKIVWbO6/hb+3vxN1c+SoJpc9JD90JysC/Vkkw==</Modulus><Exponent>AQAB</Exponent><P>5Og3JEWla4H35QOq6Djt8eh1lpGVLZmEYTmrKIZoLXqOJR6NvMMlwpjDmnvZynLd8qIhGZBn7rsXBGHxNvfzwCxXcxzCmD2iDL/XK8NoA/tlZ4r70UvFyVZDjsr6XNL0gYegkTLklobW1ZFD0Typh9qboNYWydlACUzZWKvmnVM=</P><Q>04RipKn84c1yt0XfaKbe2Eg4bjx+g8ilv6vPaw5XlQlo27WCtHsD7MIrbzPLe5Be78ixdcEoKFIPucH2huSjtxxH/Y8oJ1b0VYgdk8445SKTCG6WcwJPYxmVY8Twz7hTDLoZzTSnPvSynM6PsBktuC+zPqrVU068gehPkRnR88E=</Q><DP>HFDpeVAwPVNPggHpI17feFxEJ4MMzB5AdPJ4TMQLoQyXBtp3uBD/28mf8L0/XL7G29vYclwdrzdving/KYiUm4IgszmsjL6bDC6zBFPgyxVPHvbfXa2c4uIL618Kh28FFfzcDPoZstEtRC/7DqgNZKPTOpshKIj6VewuurxRA8c=</DP><DQ>pd8RhFQSDfmRVowi8Oy7oRyxtDEYfbwhzzerBydOI4AnjPTAtUwq/cYfTatujU3gRWY7VD7PgR8pWeDztUEj6frxsbRMJt2X6mM93qVAFOCSMXCX50UOgIaVkpHkzuCbsEVY6oW6CjLWxwVtxQlZwzEU/bX2aMg8KBvIGeAHt4E=</DQ><InverseQ>sAnYc66Ze4RmwOdjqMUs2Z0NeALuI7b15ZroZ/m5qqOoaasNKsKphX3PM9GLWdw9NQd4r6AnCTWCz+2CdEftX6f7psFwrIVEJM7LjLJtSNzKVMhlz4edlxmRoM43rXxuVax++KWYrBPG0jw5zbH6e9NE+K6j125MpFnYZLvpssI=</InverseQ><D>X3zta4nk306C6s3fXztSbnp5xymLt9s1QKm0+8GXT+m0uHpyckTd1J9MiiEhtPdkhR0p/Sh9ZwiPyHV1dhySc69YQZmZpwp4k4jtCnqcDxNU8EQQKLqCU8b/lxF6wxh5QB61c2OjuTqqyZo45V+kLXO0f0DjEyjJB9fh0X6iHWng0fDz9WZLKyYnNFHD5/SqKub6CnX8UUdypkzbtdeAzI902+AlfJ8J6+sg/lhbev5uo9rFIM5BkSPjMtZBizuSiPA4E7hmOUlojGOaQFMxIE85W5ZBnso2VWnCgtzKjDQz5QHCKYhSNpiikUUB3HwYuuX/ib8v9tn8AwAu1cdgAQ==</D></RSAKeyValue>", "<RSAKeyValue><Modulus>vSHFY3X54bKGI0lc8djFZhqOH/a86oORTH6ulnUBnvSlpj65CtOPTLpwOkmfnSlLu64qyazFMg7IrFuW3fkiSsiiTCVZInIh50De+V/dadYwjc96KDDb9qn9wb3JMikSBCWNQwqTyEgT1vCrUuHebhx8rjc6laGkRpFnzzrUJbtktYcDqYYU/xYUUmYb19HhtQV0GrQtudZ54oEHeGz+tZtopyqyVPm0ZEHUh3xf6THRWg9Jd1hnBAhyjlCgaTIwA9D25D7hJA74fArVolIRIPG0JMV+plBC9sfNRmGfPoT082l3AKIVWbO6/hb+3vxN1c+SoJpc9JD90JysC/Vkkw==</Modulus><Exponent>AQAB</Exponent><P>5Og3JEWla4H35QOq6Djt8eh1lpGVLZmEYTmrKIZoLXqOJR6NvMMlwpjDmnvZynLd8qIhGZBn7rsXBGHxNvfzwCxXcxzCmD2iDL/XK8NoA/tlZ4r70UvFyVZDjsr6XNL0gYegkTLklobW1ZFD0Typh9qboNYWydlACUzZWKvmnVM=</P><Q>04RipKn84c1yt0XfaKbe2Eg4bjx+g8ilv6vPaw5XlQlo27WCtHsD7MIrbzPLe5Be78ixdcEoKFIPucH2huSjtxxH/Y8oJ1b0VYgdk8445SKTCG6WcwJPYxmVY8Twz7hTDLoZzTSnPvSynM6PsBktuC+zPqrVU068gehPkRnR88E=</Q><DP>HFDpeVAwPVNPggHpI17feFxEJ4MMzB5AdPJ4TMQLoQyXBtp3uBD/28mf8L0/XL7G29vYclwdrzdving/KYiUm4IgszmsjL6bDC6zBFPgyxVPHvbfXa2c4uIL618Kh28FFfzcDPoZstEtRC/7DqgNZKPTOpshKIj6VewuurxRA8c=</DP><DQ>pd8RhFQSDfmRVowi8Oy7oRyxtDEYfbwhzzerBydOI4AnjPTAtUwq/cYfTatujU3gRWY7VD7PgR8pWeDztUEj6frxsbRMJt2X6mM93qVAFOCSMXCX50UOgIaVkpHkzuCbsEVY6oW6CjLWxwVtxQlZwzEU/bX2aMg8KBvIGeAHt4E=</DQ><InverseQ>sAnYc66Ze4RmwOdjqMUs2Z0NeALuI7b15ZroZ/m5qqOoaasNKsKphX3PM9GLWdw9NQd4r6AnCTWCz+2CdEftX6f7psFwrIVEJM7LjLJtSNzKVMhlz4edlxmRoM43rXxuVax++KWYrBPG0jw5zbH6e9NE+K6j125MpFnYZLvpssI=</InverseQ><D>X3zta4nk306C6s3fXztSbnp5xymLt9s1QKm0+8GXT+m0uHpyckTd1J9MiiEhtPdkhR0p/Sh9ZwiPyHV1dhySc69YQZmZpwp4k4jtCnqcDxNU8EQQKLqCU8b/lxF6wxh5QB61c2OjuTqqyZo45V+kLXO0f0DjEyjJB9fh0X6iHWng0fDz9WZLKyYnNFHD5/SqKub6CnX8UUdypkzbtdeAzI902+AlfJ8J6+sg/lhbev5uo9rFIM5BkSPjMtZBizuSiPA4E7hmOUlojGOaQFMxIE85W5ZBnso2VWnCgtzKjDQz5QHCKYhSNpiikUUB3HwYuuX/ib8v9tn8AwAu1cdgAQ==</D></RSAKeyValue>");

            var t1 = Table1.CreateSubTable();
            var t2 = Table2.CreateSubTable();

            synch.AddNewSynch();
            synch.Invalidate(T.UTable.CreateSubTable());

            t1.Rows.Add(new object[] { "test1", (uint)0 });
            t1.Rows.Add(new object[] { "test2", (uint)0 });

            var uTables = synch.UTables.Where(x => new[] { "tb1", "tb2" }.Contains(x.Table.Parent.Name));

            foreach (var uTable in uTables)
            { uTable.Use = true; }

            {
                var Row = new object[t2.Parent.Columns.Count];

                Row[TestC.au] = false;
                Row[TestC.bl] = true;
                Row[TestC.bt] = (byte)1;
                Row[TestC.db] = (double)55;
                Row[TestC.dc] = (decimal)1687212.1234;
                Row[TestC.dt] = DateTime.Now;
                Row[TestC.i32] = (int)987;
                Row[TestC.i64] = (long)987;
                Row[TestC.rl] = (uint)1;
                Row[TestC.str] = "789123\n";
                t2.Rows.Add(Row);

                Row[TestC.au] = true;
                Row[TestC.bl] = false;
                Row[TestC.bt] = (byte)255;
                Row[TestC.db] = (double)110;
                Row[TestC.dc] = (decimal)4321.212786;
                Row[TestC.dt] = DateTime.Now;
                Row[TestC.i32] = (int)789;
                Row[TestC.i64] = (long)789;
                Row[TestC.rl] = (uint)2;
                Row[TestC.str] = "\n321987";
                t2.Rows.Add(Row);
            }

            var cmsg = synch.GetEncrypted();

            DeserializeResult dmsg;
            uint UID;
            DateTime DT;
            bool SPoolExisted;

            var msg = synch.GetValues(0, cmsg, out dmsg, out SPoolExisted);

            UID = dmsg.UserID;
            DT = dmsg.SynchDate;

            if (msg != null)
            { Exception(msg); }

            if (dmsg == null)
            { Exception("Декодированное сообщение пустое"); }

            if (dmsg.Tables.Length != 1)
            { Exception("Не верное количество таблиц в декодированном сообщении"); }

            if (dmsg.Tables[0].STable.Parent != Table2)
            { Exception("Не вертая таблица декодированного сообщения"); }

            if (dmsg.Tables[0].Rows.Length != t2.Rows.Count)
            { Exception("Не верное количество строк в декодированном сообщении"); }

            for (int i = 0; i < dmsg.Tables[0].Rows.Length; i++)
            {
                int ColumnCount = -1;

                var SPCl = t2.Parent.GetColumn(t2.Parent.Columns.Count - 1);

                if (SPCl.TypeCol != DataBase.Types.RIU32)
                { Exception("не верный тип колонки SPool"); }

                if (SPCl.RelatedTable != T.SPool)
                { Exception("не верная связь с таблицей"); }

                for (int j = 0; j < t2.Parent.Columns.Count - 1; j++)
                {
                    var Cl = t2.Parent.GetColumn(j);

                    if (!Cl.Protect)
                    {
                        ColumnCount++;

                        object tableValue;
                        object resultValue = dmsg.Tables[0].Rows[i].Values[ColumnCount];

                        switch (Cl.TypeCol)
                        {
                            case DataBase.Types.AutoStatus:
                                tableValue = t2.Rows.Get_UnShow<DataBase.AutoStatus>(i, ColumnCount);
                                break;
                            case DataBase.Types.Bool:
                                tableValue = t2.Rows.Get_UnShow<bool>(i, ColumnCount);
                                break;
                            case DataBase.Types.Byte:
                                tableValue = t2.Rows.Get_UnShow<byte>(i, ColumnCount);
                                break;
                            case DataBase.Types.DateTime:
                                tableValue = t2.Rows.Get_UnShow<DateTime>(i, ColumnCount);
                                break;
                            case DataBase.Types.Decimal:
                                tableValue = t2.Rows.Get_UnShow<Decimal>(i, ColumnCount);
                                break;
                            case DataBase.Types.Double:
                                tableValue = t2.Rows.Get_UnShow<Double>(i, ColumnCount);
                                break;
                            case DataBase.Types.Int32:
                                tableValue = t2.Rows.Get_UnShow<Int32>(i, ColumnCount);
                                break;
                            case DataBase.Types.Int64:
                                tableValue = t2.Rows.Get_UnShow<Int64>(i, ColumnCount);
                                break;
                            case DataBase.Types.RIU32:
                                tableValue = t2.Rows.Get_UnShow<RIU32>(i, ColumnCount);
                                break;
                            case DataBase.Types.String:
                                tableValue = t2.Rows.Get_UnShow<String>(i, ColumnCount);
                                break;
                            case DataBase.Types.UInt32:
                                tableValue = t2.Rows.Get_UnShow<UInt32>(i, ColumnCount);
                                break;
                            default:
                                throw new Exception("Неизвестный тип колонки");
                        }

                        Assert.AreEqual(tableValue, resultValue);
                    }
                }
            }
        }

        struct TestC
        {
            /// <summary>AutoUpdate</summary>
            public const byte au = 0;
            /// <summary>Bool</summary>
            public const byte bl = au + 1;
            /// <summary>Byte</summary>
            public const byte bt = bl + 1;
            /// <summary>DateTime</summary>
            public const byte dt = bt + 1;
            /// <summary>Decimal</summary>
            public const byte dc = dt + 1;
            /// <summary>Double</summary>
            public const byte db = dc + 1;
            /// <summary>Integer</summary>
            public const byte i32 = db + 1;
            /// <summary>Long</summary>
            public const byte i64 = i32 + 1;
            /// <summary>String</summary>
            public const byte str = i64 + 1;
            /// <summary>Relation</summary>
            public const byte rl = str + 1;
        }

        void CheckSorts(Employe_Form.SPoints_class SPoints)
        {
            for (int i = 1; i < SPoints.SPointsCount; i++)
            {
                if (SPoints[i - 1].Number > SPoints[i].Number)
                { Exception("Точка отбора " + i.ToString() + " расположена не на своем месте"); }
            }

            for (int i = 0; i < SPoints.SPointsCount; i++)
            {
                if (T.SPoint.Rows.Status(SPoints[i].SPointID) != DataBase.State.Normal)
                { Exception("Запись точки отбора номер " + SPoints[i].SPointID.ToString() + " имеет не верный статус"); }

                for (int j = 0; j < SPoints[i].SampleCount; j++)
                {
                    if (T.Sample.Rows.Status(SPoints[i][j].SampleID) != DataBase.State.Normal)
                    { Exception("Запись замера номер " + SPoints[i][j].SampleID.ToString() + " имеет не верный статус"); }

                    if (SPoints[i][j].Loc != (j + 1))
                    { Exception("Не верный порядковый номер замера " + j.ToString() + " точки отбора " + i.ToString()); }
                }
            }
        }

        void Exception(string text)
        { throw new Exception(text); }
    }
}