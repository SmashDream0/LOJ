using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;

using NPOI.HSSF.UserModel;
//using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;
using LaboratoryOnlineJournal.SerializeProvider;
using LaboratoryOnlineJournal.SerializeFormatProvider;
using LaboratoryOnlineJournal.Synch;

namespace LaboratoryOnlineJournal
{
    public static partial class Misc
    {
        public static void DataBaseLoadFT(DataBase db, StartupLogo_Form.Loading_class Loading)
        {
            if (Loading != null)
            { Loading.LoadingComment = "Виртуальные таблицы"; }
            {
                T.UType = db.Tables.Add(Encoding.GetEncoding(1251), "UType", "Тип учетной записи");
                T.UType.Columns.AddString("Name", "Наименование", 15);

                G.UType = T.UType.CreateSubTable();

                G.UType.Rows.Add(new object[] { "Админ" });
                G.UType.Rows.Add(new object[] { "Сооружение" });
                G.UType.Rows.Add(new object[] { "Центр" });
                G.UType.Rows.Add(new object[] { "Объединение" });

                T.VarType = db.Tables.Add(Encoding.GetEncoding(1251), "VarType", "Тип значения");
                T.VarType.Columns.AddString("Name", "Наименование", 15);

                G.VarType = T.VarType.CreateSubTable();

                G.VarType.Rows.Add(new object[] { "Десятичное" });
                G.VarType.Rows.Add(new object[] { "Целое" });
                G.VarType.Rows.Add(new object[] { "Булево" });

                T.TResp = db.Tables.Add(Encoding.GetEncoding(1251), "TResp", "Тип ответственности");
                T.TResp.Columns.AddString("Name", "Наименование", 20);

                G.TResp = T.TResp.CreateSubTable();

                G.TResp.Rows.Add(new object[] { "Материальная" });
                G.TResp.Rows.Add(new object[] { "Лабор.анализы" });
                G.TResp.Rows.Add(new object[] { "Пробоотбор" });

                T.PnMean = db.Tables.Add(Encoding.GetEncoding(1251), "PnMean", "Значение профессии");
                T.PnMean.Columns.AddString("Name", "Наименование", 15);
                G.PnMean = T.PnMean.CreateSubTable();

                G.PnMean.Rows.Add(new object[] { "Начальник" });
                G.PnMean.Rows.Add(new object[] { "Заместитель" });
                G.PnMean.Rows.Add(new object[] { "Технолог сооружения" });
                G.PnMean.Rows.Add(new object[] { "Сотрудник" });

                T.SGroup = db.Tables.Add(Encoding.GetEncoding(1251), "SGroup", "Тип протокола");
                T.SGroup.Columns.AddString("Name", "Наименование", 30);
                T.SGroup.Columns.AddString("ShrName", "Краткое наименование", 5);
                G.SGroup = T.SGroup.CreateSubTable();

                G.SGroup.Rows.Add(1, new object[] { "Усредненный", "" }); //1
                G.SGroup.Rows.Add(2, new object[] { "Сводный", "" });      //2
                G.SGroup.Rows.Add(3, new object[] { "Единичный", "" });   //3
                G.SGroup.Rows.Add(5, new object[] { "Атмосферный воздух", "АВ" });      //5
                G.SGroup.Rows.Add(8, new object[] { "Рабочая зона", "РЗ" });      //8
                G.SGroup.Rows.Add(10, new object[] { "Колодец\\Шламонакопитель", "" }); //10
                G.SGroup.Rows.Add(11, new object[] { "АКВА-АУРАТ", "Р" }); //11
                G.SGroup.Rows.Add(12, new object[] { "Кагулянт сульфат алюминия", "Р" }); //12
                G.SGroup.Rows.Add(13, new object[] { "Токсичность1.Очищеная вода", "Т" }); //13
                G.SGroup.Rows.Add(4, new object[] { "Токсичность2", "Т" }); //4

                T.PSG = db.Tables.Add(Encoding.GetEncoding(1251), "PSG", "Группа складов"); //Sample point type
                T.PSG.Columns.AddString("Name", "Наименование", 15);
                G.PSG = T.PSG.CreateSubTable();

                G.PSG.Rows.Add(new object[] { "Стандарт" });
                G.PSG.Rows.Add(new object[] { "Водоснабжение" });
                G.PSG.Rows.Add(new object[] { "Водоотведение" });

                T.NType = db.Tables.Add(Encoding.GetEncoding(1251), "NType", "Тип нормы"); //Sample point type
                T.NType.Columns.AddString("Name", "Наименование", 22);
                G.NType = T.NType.CreateSubTable();

                G.NType.Rows.Add(new object[] { "Показатель" });
                G.NType.Rows.Add(new object[] { "Сооружение водопровода" });
                G.NType.Rows.Add(new object[] { "Сооружение канализации" });
                G.NType.Rows.Add(new object[] { "Сооружение любое" });
                G.NType.Rows.Add(new object[] { "Выпуск" });
            }

            if (!AddRemote(db, Loading, "Podr", "Подразделение", ref T.Podr, ref G.Podr,
            newTable =>
            {
                newTable.Columns.AddRelation(T.PSG, "Name");
                newTable.Columns.AddString("ShrName", "Короткое наименование", 25);
                newTable.Columns.AddString("FllName", "Полное наименование", 95);
                newTable.Columns.AddString("Contact", "Контакты", 95);
                newTable.Columns.AddRelation(T.Podr.GetColumn(C.Podr.ShrName));
                newTable.Columns.AddBool("ShowP", "Скрыть/Показать");
                newTable.Columns.AddInt32("Xloc", "Положение X");
                //newTable.Columns.AddInt32("Zloc", "Положение Z");
                newTable.Columns.AddInt32("Yloc", "Положение Y");

                newTable.Columns.Add_Unique("ShrName");
                newTable.Columns.Add_Unique("FllName");
            }, false)) { return; }

            G.Podr.Add(C.Podr.PSG);
            G.Podr.Add(C.Podr.ShrName);
            G.Podr.Add(C.Podr.FllName);
            G.Podr.Add(C.Podr.ShowP);

            G.Podr.Rows.SetAddForm(C.Podr.GetEdit);
            G.Podr.Rows.SetEditForm(C.Podr.GetEdit);

            if (!AddRemote(db, Loading, "User", "Пользователь", ref T.User, ref G.User,
                newTable =>
                {
                    newTable.Columns.AddString("Login", "Логин", 25);
                    newTable.Columns.AddString("Pass", "Пароль", 25, DataBase.ColLocation.Remote, true, "", true);
                    newTable.Columns.AddRelation(T.UType, "Name");  //User Type
                    newTable.Columns.AddString("PCName", "Имя компьютера", 50, true); //
                    newTable.Columns.AddString("PCUser", "Имя пользователя", 50, true); //
                    newTable.Columns.AddAutoUpdate("IsHere", "Используется");
                    newTable.Columns.AddString("Mail", "Почта", 50);
                    newTable.Columns.AddInt32("YM", "Период");
                    newTable.Columns.AddRelation(T.Podr.GetColumn(C.Podr.ShrName));  //Podr
                    newTable.Columns.AddInt32("Round", "Точность", 9, DataBase.ColLocation.Local, true, 5, false);

                    newTable.Columns.AddInt32("SPC1", "Колонка 1", 20, DataBase.ColLocation.Local, true, 80);   //Column 1
                    newTable.Columns.AddInt32("SPC2", "Колонка 2", 20, DataBase.ColLocation.Local, true, 80);   //Column 2
                    newTable.Columns.AddInt32("SPC3", "Колонка 3", 20, DataBase.ColLocation.Local, true, 80);   //Column 3
                    newTable.Columns.AddInt32("SPC4", "Колонка 4", 20, DataBase.ColLocation.Local, true, 80);   //Column 4
                    newTable.Columns.AddInt32("SPC5", "Колонка 5", 20, DataBase.ColLocation.Local, true, 80);   //Column 5
                    newTable.Columns.AddInt32("SPC6", "Колонка 6", 20, DataBase.ColLocation.Local, true, 80);   //Column 6

                    newTable.Columns.AddInt32("SC1", "Колонка 1", 20, DataBase.ColLocation.Local, true, 80);   //Column 1
                    newTable.Columns.AddInt32("SC2", "Колонка 2", 20, DataBase.ColLocation.Local, true, 80);   //Column 2
                    newTable.Columns.AddInt32("SC3", "Колонка 3", 20, DataBase.ColLocation.Local, true, 80);   //Column 3
                    newTable.Columns.AddInt32("SC4", "Колонка 4", 20, DataBase.ColLocation.Local, true, 80);   //Column 4
                    newTable.Columns.AddInt32("SC5", "Колонка 5", 20, DataBase.ColLocation.Local, true, 80);   //Column 5
                    newTable.Columns.AddInt32("SC6", "Колонка 6", 20, DataBase.ColLocation.Local, true, 80);   //Column 6
                    newTable.Columns.AddInt32("SCM", "Колонки показателей", 20, DataBase.ColLocation.Local, true, 80);   //Column Marks
                    newTable.Columns.AddInt32("SpntH", "Высота записей точек отбора", 20, DataBase.ColLocation.Local, true, 20);   //Row Height
                    newTable.Columns.AddInt32("SplH", "Высота записей замеров", 20, DataBase.ColLocation.Local, true, 20);   //Row Height

                    newTable.Columns.AddInt32("SplDist", "Дистанция разделения", 20, DataBase.ColLocation.Local, true, 350);   //Row Height
                    newTable.Columns.AddString("ok1", "открыто", 1000, DataBase.ColLocation.Remote, false, "", true);
                    newTable.Columns.AddString("ck1", "закрыто", 1800, DataBase.ColLocation.Remote, false, "", true);

                    newTable.Columns.AddBool("CNP", "Создавать новый протокол");

                    newTable.Columns.AddInt32("YMDSND", "День отправки");
                    newTable.Columns.AddInt32("Period", "Периодичность, в днях");
                    newTable.Columns.AddBool("AlowToGSU", "Разрешить отправку/приём");  //Alow to Get/Send Updates
                    newTable.Columns.AddBool("UAll", "Обновлять всем");   //Update All
                    newTable.Columns.AddInt32("UP", "Период обновления");   //Update Period

                    newTable.Columns.AddBool("Enabled", "Разрешено использовать", DataBase.ColLocation.Remote, false, true);
                    newTable.Columns.AddString("Cause", "Причина закрытия", 55, DataBase.ColLocation.Remote, false, "");

                    newTable.Columns.Add_Unique("Login");
                }, false)) { return; }

            G.User.Rows.SetAddForm(C.User.GetEdit);
            G.User.Rows.SetEditForm(C.User.GetEdit);

            G.User.Add(C.User.Login);
            G.User.Add(C.User.UType);
            G.User.Add(C.User.Podr);
            G.User.Add(C.User.Mail);

            //вынужденная мера, чтоб автотестами поддерживалось
            T.UTable = db.Tables.Add("UTable", "Выгружаемые таблицы");
            T.UTable.Columns.AddString("Name", "Наименование", 15);
            T.UTable.Columns.AddBool("Add", "Добавление", DataBase.ColLocation.Local, true, true);
            T.UTable.Columns.AddBool("Update", "Изменение", DataBase.ColLocation.Local, true, true);
            T.UTable.Columns.AddBool("Delete", "Удаление", DataBase.ColLocation.Local, true, true);
            T.UTable.Columns.AddBool("Use", "Задействовать", DataBase.ColLocation.Local, true, true);
            T.UTable.AutoSave(false, DataBase.TypeOfTable.Remote);
            G.UTable = T.UTable.CreateSubTable(false);

            T.SPool = db.Tables.Add("SPool", "Пул синхронизаций");
            T.SPool.Columns.AddRelation(T.User.GetColumn(C.User.Login), "A", T.User.AlterName + " автор");
            T.SPool.Columns.AddRelation(T.User.GetColumn(C.User.Login), "S", T.User.AlterName + " отправитель");
            T.SPool.Columns.AddBool("local", "создано локально");
            T.SPool.Columns.AddDATE("StartDate", "Дата создания");
            T.SPool.AutoSave(false, DataBase.TypeOfTable.Remote);
            G.SPool = T.SPool.CreateSubTable(false);

            var serializeProviders = GetSerializeProviders(db);
            data.SynchPool = new SynchPoolManager(db, serializeProviders.Last().Name, serializeProviders);
        }

        public static ISerializeFormatProvider[] GetSerializeProviders(DataBase dataBase)
        {
            var oldProvider = new OldSerializeFormatProvider(dataBase);
            var csvProvider = new TXTSerializeFormatProvider(dataBase);

            return new ISerializeFormatProvider[] { oldProvider, csvProvider };
        }
    }
}