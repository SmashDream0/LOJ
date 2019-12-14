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

namespace LaboratoryOnlineJournal
{
    public static partial class Misc
    {
        public static void DataBaseLoad(DataBase db, StartupLogo_Form.Loading_class Loading)
        {
            if (Loading != null)
            { Loading.LoadingComment = "SPool"; }

            if (data.SynchPool != null)
            { data.SynchPool.Prepare(); }

            /*  
                река(речные нормы)
                питьевая(питьевые нормы)
                разводящая(питьевые нормы)
                промывочная(промывочные нормы)
                река выше стока(речные нормы)
                река ниже стока(речные нормы)
                поступило на очистку(пдк)
                очищенное в воду(нормы на очищенное)
             */

            if (!AddRemote(db, Loading, "Prfssn", "Професия", ref T.Prfssn, ref G.Prfssn,
                     newTable =>
                     {
                         newTable.Columns.AddString("Name", "Наименование", 85);
                         newTable.Columns.AddRelation(T.PnMean.GetColumn(C.PnMean.Name));

                         newTable.Columns.Add_Unique("Name");
                     }, false)) { return; }

            if (!AddRemote(db, Loading, "People", "Сотрудник", ref T.People, ref G.People,
                newTable =>
                {
                    newTable.Columns.AddString("Name1", "Фамилия", 30);
                    newTable.Columns.AddString("Name2", "Имя", 30);
                    newTable.Columns.AddString("Name3", "Отчество", 30);
                    newTable.Columns.AddRelation(T.Prfssn.GetColumn(C.Prfssn.Name));

                    //newTable.Columns.Add_Unique(C.People.name1, C.People.name2, C.People.name3, C.People.Prfssn);
                }, false)) { return; }

            if (!AddRemote(db, Loading, "PSGM", "Метод для группы", ref T.PSGM, ref G.PSGM,
                     newTable =>
                     {
                         newTable.Columns.AddInt32("YM", "Период начала использования");
                         newTable.Columns.AddRelation(T.PSG.GetColumn(C.PSG.Name));
                         newTable.Columns.AddString("Name", "Наименование", 200);
                         newTable.Columns.AddRelation(T.People.GetColumn(C.People.name1));

                         newTable.Columns.Add_Unique(C.PSGM.YM, C.PSGM.PSG);
                     }, false)) { return; }

            if (!AddRemote(db, Loading, "PodrPpl", "Сотрудник в подразделении", ref T.PodrPpl, ref G.PodrPpl,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.Podr.GetColumn(C.Podr.ShrName));   //
                    newTable.Columns.AddRelation(T.People.GetColumn(C.People.name1));   //

                    newTable.Columns.Add_Unique(C.PodrPpl.People, C.PodrPpl.Podr);
                }, false)) { return; }

            G.PodrPpl.Rows.SetEditForm(C.PodrPpl.GetEdit);
            G.PodrPpl.Rows.SetAddForm(C.PodrPpl.GetEdit);

            T.PodrPpl.Add("Сотрудник", 2, true, true,
                (R) => { return Misc.GetPWP(T.PodrPpl.Rows.Get_UnShow<uint>(R.ID, C.PodrPpl.People)); });
            T.PodrPpl.Add(C.PodrPpl.Podr);

            if (!AddRemote(db, Loading, "Resp", "Ответственность", ref T.Resp, ref G.Resp,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.PodrPpl.GetColumn(C.PodrPpl.People));   //
                    newTable.Columns.AddRelation(T.TResp.GetColumn(C.TResp.Name));   //

                    newTable.Columns.Add_Unique(C.Resp.PodrPpl, C.Resp.TResp);
                }, false)) { return; }

            T.Resp.Add("Подразделение", (R) => { return T.Resp.Rows.Get<string>(R.ID, C.Resp.PodrPpl, C.PodrPpl.Podr, C.Podr.ShrName); });
            T.Resp.Add("Сотрудник", (R) => { return Misc.GetShortFIO(T.Resp.Rows.Get_UnShow<uint>(R.ID, C.Resp.PodrPpl, C.PodrPpl.People)); });
            T.Resp.Add(C.Resp.TResp);

            if (!AddSynch(db, Loading, "EdType", "Единица измерений", ref T.EdType, ref G.EdType,
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 25);
                    newTable.Columns.AddString("MZero", "Значит пусто", 25);

                    newTable.Columns.Add_Unique(C.EdType.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "BackGrd", "Группа фона", ref T.BackGrd, ref G.BackGrd,
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 25);

                    newTable.Columns.Add_Unique(C.BackGrd.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "OPType", "Единица измерений вывода", ref T.OPType, ref G.OPType,
                newTable =>
                {
                    newTable.Columns.AddDOUBLE("Multy", "Множитель");
                    newTable.Columns.AddRelation(T.EdType.GetColumn(C.EdType.Name), "T", "Единица измерений ввода");
                    newTable.Columns.AddRelation(T.EdType.GetColumn(C.EdType.Name), "F", "Единица измерений вывода");

                    newTable.Columns.Add_Unique(C.OPType.EdTypeT, C.OPType.EdTypeF);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "Mark", "Показатель", ref T.Mark, ref G.Mark,
                newTable =>
                {
                    newTable.Columns.AddInt32("Code", "Код");
                    newTable.Columns.AddString("Name", "Наименование", 55);
                    newTable.Columns.AddByte("Round", "Округление", 1);
                    newTable.Columns.AddBool("Exp", "Экспотенциальный");
                    newTable.Columns.AddRelation(T.VarType.GetColumn(C.VarType.Name));
                    newTable.Columns.AddRelation(T.EdType.GetColumn(C.EdType.Name));
                    newTable.Columns.AddRelation(T.OPType.GetColumn(C.OPType.EdTypeF));
                    newTable.Columns.AddByte("Number", "Номер");   //позиция показателя
                    newTable.Columns.AddBool("ShowZr", "Показывать ноль");   //Show Zero

                    newTable.Columns.Add_Unique(C.Mark.Name);
                }, false)) { return; }

            T.Mark.Rows.SetAddForm(C.Mark.GetEdit);
            T.Mark.Rows.SetEditForm(C.Mark.GetEdit);

            if (!AddSynch(db, Loading, "PType", "Тип пробы", ref T.PType, ref G.PType, //Probe Type
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 50);

                    newTable.Columns.Add_Unique(C.PType.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "SCause", "Цель испытания", ref T.SCause, ref G.SCause, //Probe Cause
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 200);

                    newTable.Columns.Add_Unique(C.SCause.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "Norm", "Норматив", ref T.Norm, ref G.Norm,
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 50);
                    newTable.Columns.AddDATE("DFrom", "Действует от");
                    newTable.Columns.AddDATE("DTo", "Действует до");
                    newTable.Columns.AddRelation(T.NType.GetColumn(C.NType.Name));
                    newTable.Columns.AddBool("Enabled", "Задействовать");
                    newTable.Columns.AddBool("Show", "Развернуто");

                    newTable.Columns.Add_Unique(C.Norm.Name);
                }, false)) { return; }

            T.Norm.Add(C.Norm.Name);
            T.Norm.Add(C.Norm.NType);
            T.Norm.Add(C.Norm.DFrom);
            T.Norm.Add(T.Norm.GetColumn(C.Norm.DTo).AlterName, (R) =>
                {
                    if (T.Norm.Rows.Get<DateTime>(R.ID, C.Norm.DTo).Ticks == 0)
                    { return ""; }
                    else
                    { return T.Norm.Rows.Get<string>(R.ID, C.Norm.DTo); }
                });
            T.Norm.Add(C.Norm.Enabled);

            if (!AddSynch(db, Loading, "Method", "Метод", ref T.Method, ref G.Method,
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 55);
                    newTable.Columns.AddRelation(T.Norm.GetColumn(C.Norm.Name));
                    newTable.Columns.AddRelation(T.Mark.GetColumn(C.Mark.Name));

                    newTable.Columns.Add_Unique(C.Method.Norm, C.Method.Mark);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "MError", "Точность измерений", ref T.MError, ref G.MError,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.Norm.GetColumn(C.Norm.Name));
                    newTable.Columns.AddRelation(T.Mark.GetColumn(C.Mark.Name));
                    newTable.Columns.AddDOUBLE("From", "Значение от");
                    newTable.Columns.AddDOUBLE("To", "Значение до");
                    newTable.Columns.AddDOUBLE("Volume", "Значение");
                    newTable.Columns.AddBool("Percent", "Это процент");

                    newTable.Columns.Add_Unique(C.MError.Norm, C.MError.Mark, C.MError.From, C.MError.To);
                }, false)) { return; }
            G.MError.Get_Default();

            if (!AddSynch(db, Loading, "OLocation", "Место", ref T.OLocation, ref G.OLocation,   //Object location
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 50);
                    newTable.Columns.AddString("ShrName", "Краткое наименование", 5);
                    newTable.Columns.AddBool("Volumed", "Использовать объём");

                    newTable.Columns.Add_Unique(C.OLocation.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "OType", "Тип объекта", ref T.OType, ref G.OType,   //Object Type
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 25);

                    newTable.Columns.Add_Unique(C.OType.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "Object", "Объект испытания", ref T.Object, ref G.Object,   //Object Type
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 100);
                    newTable.Columns.AddRelation(T.OType.GetColumn(C.OType.Name));
                    newTable.Columns.AddRelation(T.OLocation.GetColumn(C.OLocation.Name), "From", "Источник");
                    newTable.Columns.AddRelation(T.OLocation.GetColumn(C.OLocation.Name), "To", "Сброс");
                    newTable.Columns.AddRelation(T.Norm.GetColumn(C.Norm.Name));

                    newTable.Columns.Add_Unique(C.Object.Name);
                    newTable.Columns.Add_Unique(C.Object.Name, C.Object.OLocationFrom, C.Object.OLocationTo);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "PMNorm", "Значение нормы", ref T.PMNorm, ref G.PMNorm, //Podr Mark
                newTable =>
                {
                    newTable.Columns.AddRelation(T.Norm.GetColumn(C.Norm.Name));
                    newTable.Columns.AddRelation(T.Mark.GetColumn(C.Mark.Name));
                    newTable.Columns.AddRelation(T.Podr.GetColumn(C.Podr.ShrName));
                    newTable.Columns.AddRelation(T.OLocation.GetColumn(C.OLocation.ShrName));
                    newTable.Columns.AddDOUBLE("LFrom", "От");
                    newTable.Columns.AddDOUBLE("LTo", "До");
                    //newTable.Columns.AddBool("BckGrnd", "+ Фон");

                    newTable.Columns.Add_Unique(C.PMNorm.Norm, C.PMNorm.Mark, C.PMNorm.Podr, C.PMNorm.OLocation);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "Area", "Район", ref T.Area, ref G.Area,
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 50);

                    newTable.Columns.Add_Unique(C.Area.Name);
                }, false)) { return; }

            if (!AddSynch(db, Loading, "MVolume", "Объём выпуска", ref T.MVolume, ref G.MVolume, //Month Group
                newTable =>
                {
                    newTable.Columns.AddRelation(T.OLocation.GetColumn(C.OLocation.Name));
                    newTable.Columns.AddInt32("YM", "Номер периода");
                    newTable.Columns.AddDOUBLE("Volume", "Объём");

                    newTable.Columns.Add_Unique(C.MVolume.OLocation, C.MVolume.YM);
                }, false)) { return; }

            T.MVolume.Rows.SetEditForm(C.MVolume.GetEdit);
            T.MVolume.Rows.SetAddForm(C.MVolume.GetEdit);

            T.MVolume.Add(C.MVolume.OLocation);
            T.MVolume.Add(T.MVolume.GetColumn(C.MVolume.YM).AlterName, (R) => ATMisc.GetDateTimeFromYM(G.MVolume.Rows.Get<int>(R, C.MVolume.YM)).ToShortDateString());
            T.MVolume.Add(C.MVolume.Volume);

            if (!AddSynch(db, Loading, "PaPoS", "План и место отбора образцов", ref T.PaPoS, ref G.PaPoS, //Plan and place of sampling
                newTable =>
                {
                    newTable.Columns.AddString("Name", "Наименование", 75);

                    newTable.Columns.Add_Unique(C.PaPoS.Name);
                }, false)) { return; }

            if (!AddRemote(db, Loading, "SPoint", "Точка отбора", ref T.SPoint, ref G.SPoint,    //Sample point
                newTable =>
                {//
                    newTable.Columns.AddInt32("Number", "Номер");
                    newTable.Columns.AddString("Name", "Наименование", 500);
                    newTable.Columns.AddRelation(T.Area.GetColumn(C.Area.Name));
                    newTable.Columns.AddBool("BckGnd", "Фоновый");//Background
                    newTable.Columns.AddBool("UsBckGnd", "Применять фон");//Use Background
                    newTable.Columns.AddRelation(T.PType.GetColumn(C.PType.Name));
                    newTable.Columns.AddRelation(T.Object.GetColumn(C.Object.Name));
                    newTable.Columns.AddRelation(T.Podr.GetColumn(C.Podr.ShrName));
                    newTable.Columns.AddInt32("YMDS", "Начало действия");    //Year Month Day Start
                    newTable.Columns.AddInt32("YMDE", "Окончание действия");    //Year Month Day End
                    newTable.Columns.AddBool("IMLst", "Игнорировать список показателей");    //Ignore Mark List
                    newTable.Columns.AddRelation(T.SGroup.GetColumn(C.SGroup.Name));    //One of Two
                    newTable.Columns.AddInt32("SGNum", "Номер группы");    //Sample Point Group Name
                    newTable.Columns.AddRelation(T.BackGrd.GetColumn(C.BackGrd.Name));
                    newTable.Columns.AddBool("Union", "Объединена");    //Union
                    newTable.Columns.AddRelation(T.PaPoS.GetColumn(C.PaPoS.Name));
                    //newTable.Columns.AddBool("CanChange", "Можно редактировать", DataBase.ColLocation.Local, true, true);

                    //newTable.Columns.Add_Unique(C.SPoint.Union, C.SPoint.Name, C.SPoint.Podr, C.SPoint.YMDS);
                }, false)) { return; }
            T.SPoint.Rows.SetEditForm(C.SPoint.GetEdit);
            T.SPoint.Rows.SetAddForm(C.SPoint.GetEdit);

            if (!AddRemote(db, Loading, "Sample", "Замер", ref T.Sample, ref G.Sample,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.SPoint.GetColumn(C.SPoint.Name));
                    newTable.Columns.AddInt32("Number", "Номер"); //
                    newTable.Columns.AddInt32("Loc", "Порядок создания"); //
                    newTable.Columns.AddInt32("CYMD", "Дата отбора"); //Creation YearMonthDay
                    newTable.Columns.AddInt32("AYMD", "Дата(начала) испытаний"); //Analis YearMonthDay
                    newTable.Columns.AddRelation(T.Resp.GetColumn(C.Resp.PodrPpl), "", "Пробоотборщик");
                    newTable.Columns.AddRelation(T.SCause.GetColumn(C.SCause.Name));

                    //newTable.Columns.Add_Unique(C.Sample.SPoint, C.Sample.Loc);
                }, false)) { return; }
            
            if (!AddSynch(db, Loading, "TestCond", "Тип условия испытания", ref T.TestCond, ref G.TestCond, //Test condition
                newTable =>
                {
                    newTable.Columns.AddRelation(T.SPoint.GetColumn(C.SPoint.Name));
                    newTable.Columns.AddString("Name", "Наименование", 20);
                    newTable.Columns.AddRelation(T.EdType.GetColumn(C.EdType.Name));

                    newTable.Columns.Add_Unique(C.TestCond.SPoint, C.TestCond.Name);
                }, false)) { return; }

            T.TestCond.Rows.SetAddForm(C.TestCond.GetEdit);
            T.TestCond.Rows.SetEditForm(C.TestCond.GetEdit);

            if (!AddSynch(db, Loading, "TCS", "Условие испытания", ref T.TCS, ref G.TCS, //Test condition sample
                newTable =>
                {
                    newTable.Columns.AddRelation(T.Sample.GetColumn(C.Sample.CYMD));
                    newTable.Columns.AddRelation(T.TestCond.GetColumn(C.TestCond.Name));
                    newTable.Columns.AddString("Value", "Значение свойства", 100);

                    newTable.Columns.Add_Unique(C.TCS.Sample, C.TCS.TestCond);
                }, false)) { return; }

            T.TCS.Rows.SetAddForm(C.TCS.GetEdit);
            T.TCS.Rows.SetEditForm(C.TCS.GetEdit);

            if (!AddRemote(db, Loading, "SMS", "Показатель к точке отбора", ref T.SMS, ref G.SMS,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.SPoint.GetColumn(C.SPoint.Name));
                    newTable.Columns.AddRelation(T.Mark.GetColumn(C.Mark.Name));

                    newTable.Columns.Add_Unique(C.SMS.SPoint, C.SMS.Mark);
                }, false)) { return; }

            T.Sample.Rows.SetAddForm(C.Sample.GetEdit);
            T.Sample.Rows.SetEditForm(C.Sample.GetEdit);

            if (!AddRemote(db, Loading, "SM", "Концентрация к показателю", ref T.SM, ref G.SM,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.Sample.GetColumn(C.Sample.AYMD));
                    newTable.Columns.AddRelation(T.Mark.GetColumn(C.Mark.Name));
                    newTable.Columns.AddDOUBLE("Amount", "Значение");

                    newTable.Columns.Add_Unique(C.SM.Sample, C.SM.Mark);
                }, false)) { return; }

            T.SM.Add(T.Sample.AlterName, (R) => R.ID);
            T.SM.Add(C.SM.Mark);
            T.SM.Add(C.SM.Amount);

            G.SMMiddle = T.SM.CreateSubTable();

            if (!AddRemote(db, Loading, "Prt", "Протокол", ref T.Prt, ref G.Prt,
                newTable =>
                {
                    newTable.Columns.AddInt32("YM", "Месяц");
                    newTable.Columns.AddInt32("Time", "Время");
                    newTable.Columns.AddInt32("Number", "Номер протокола");
                    newTable.Columns.AddBool("Union", "Объединение");
                    newTable.Columns.AddRelation(T.OLocation.GetColumn(C.OLocation.Name));
                    newTable.Columns.AddRelation(T.Podr.GetColumn(C.Podr.ShrName));
                    newTable.Columns.AddRelation(T.Area.GetColumn(C.Area.Name));
                    newTable.Columns.AddRelation(T.Object.GetColumn(C.Object.Name));
                    newTable.Columns.AddInt32("SCount", "Количество за период");
                    newTable.Columns.AddRelation(T.SGroup.GetColumn(C.SGroup.Name));
                    newTable.Columns.AddRelation(T.PaPoS.GetColumn(C.PaPoS.Name));
                    newTable.Columns.AddInt32("Taos", "Акт отбора");  //the act of selection
                    newTable.Columns.AddByte("Day", "День сохранения");  //

                    //newTable.Columns.Add_Unique(C.Prt.YM, C.Prt.Number);
                }, false)) { return; }

            if (!AddRemote(db, Loading, "PrtS", "Отборы протокола", ref T.PrtS, ref G.PrtS,
                newTable =>
                {
                    newTable.Columns.AddRelation(T.Prt.GetColumn(C.Prt.Number));
                    newTable.Columns.AddRelation(T.Sample.GetColumn(C.Sample.CYMD));

                    newTable.Columns.Add_Unique(C.PrtS.Prt, C.PrtS.Sample);
                }, false)) { return; }

            T.PrtS.Add(T.Prt.GetColumn(C.Prt.Number).AlterName, (R) => T.PrtS.Rows.Get<int>(R.ID, C.PrtS.Prt, C.Prt.Number));
            T.PrtS.Add(T.Prt.GetColumn(C.Prt.YM).AlterName, (R) => ATMisc.GetDateTimeFromYM(T.PrtS.Rows.Get<int>(R.ID, C.PrtS.Prt, C.Prt.YM)).ToShortDateString());
            T.PrtS.Add(T.Podr.AlterName, (R) => T.PrtS.Rows.Get<string>(R.ID, C.PrtS.Prt, C.Prt.Podr, C.Podr.ShrName));
            T.PrtS.Add(T.SPoint.AlterName, (R) => T.PrtS.Rows.Get<string>(R.ID, C.PrtS.Sample, C.Sample.SPoint, C.SPoint.Name));
            T.PrtS.Add("Номер отбора", (R) => T.PrtS.Rows.Get<string>(R.ID, C.PrtS.Sample, C.Sample.Number));
            T.PrtS.Add(T.Sample.GetColumn(C.Sample.CYMD).AlterName, (R) => ATMisc.GetDateTime(T.PrtS.Rows.Get<int>(R.ID, C.PrtS.Sample, C.Sample.CYMD)).ToShortDateString());

            if (data.SynchPool != null)
            { data.SynchPool.Invalidate(G.UTable); }
        }
    }
}