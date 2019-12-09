using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NPOI.HSSF.UserModel;
//using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using NPOI.SS.Util;

namespace LaboratoryOnlineJournal
{
    public static partial class Misc
    {
        public static bool OtchProtokolToxicity1(Protokols_class.SGroup_class.Protokol_class Protokol, bool CreateNew = true, bool Open = true)
        {
            if (Protokol.SampleCount != 1)
            {
                MessageBox.Show("Не верное количество замеров в протоколе:" + Protokol.SampleCount.ToString() + ". Должен быть 1", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            string NewFileName;
            {
                int Month, Year;

                ATMisc.GetYearMonthFromYM(Employe_Form.SPoints.YM, out Year, out Month);

                {
                    NewFileName = Application.StartupPath + "\\Отчеты\\";

                    if (!Directory.Exists(NewFileName))
                    { Directory.CreateDirectory(NewFileName); }

                    NewFileName += T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName) + "\\";

                    if (!Directory.Exists(NewFileName))
                    { Directory.CreateDirectory(NewFileName); }

                    NewFileName += ATMisc.GetMonthName1(Month) + "\\";

                    if (!Directory.Exists(NewFileName))
                    { Directory.CreateDirectory(NewFileName); }
                }

                NewFileName += ProtokolFileName(Protokol);
            }

            if (CreateNew || !File.Exists(NewFileName))
            {
                var WorkBook = ATMisc.GetGenericExcel(Protokol.TemplateFileName());

                if (WorkBook == null)
                { return false; }

                var TitleSheet = WorkBook.GetSheet("Заголовок");
                NPOI.SS.UserModel.ISheet ByPodrSheet;

                if (TitleSheet == null)
                {
                    MessageBox.Show("В шаблоне не найден лист \"Заголовок\", вывод невозможен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (Protokol.MarkCount == 0)
                {
                    MessageBox.Show("Заполненые показатели не найдены.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                if (Protokol.MarkCount != 2)
                {
                    MessageBox.Show("Не верное количество показателей.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                var Font = WorkBook.CreateFont();
                Font.Color = NPOI.HSSF.Util.HSSFColor.OliveGreen.Index;
                Font.IsStrikeout = false;
                Font.FontHeightInPoints = 11;
                Font.FontName = "Times New Roman";
                Font.Color = short.MaxValue;

                var StyleLRTD_CC = WorkBook.CreateCellStyle();
                StyleLRTD_CC.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_CC.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_CC.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_CC.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleLRTD_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleLRTD_CC.SetFont(Font);
                StyleLRTD_CC.WrapText = true;

                var ExpStyle = WorkBook.CreateCellStyle();
                ExpStyle.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                ExpStyle.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                ExpStyle.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                ExpStyle.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                ExpStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                ExpStyle.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                ExpStyle.SetFont(Font);
                ExpStyle.WrapText = true;
                ExpStyle.DataFormat = WorkBook.CreateDataFormat().GetFormat("0.00E+00");

                var StyleLRTD_LC = WorkBook.CreateCellStyle();
                StyleLRTD_LC.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_LC.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_LC.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_LC.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRTD_LC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Left;
                StyleLRTD_LC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleLRTD_LC.SetFont(Font);
                StyleLRTD_LC.WrapText = true;

                var StyleLRD_CC = WorkBook.CreateCellStyle();
                StyleLRD_CC.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRD_CC.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRD_CC.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRD_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleLRD_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleLRD_CC.SetFont(Font);
                StyleLRD_CC.WrapText = true;

                var StyleLRT_CC = WorkBook.CreateCellStyle();
                StyleLRT_CC.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRT_CC.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRT_CC.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLRT_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleLRT_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleLRT_CC.SetFont(Font);
                StyleLRT_CC.WrapText = true;

                var StyleRTD_CC = WorkBook.CreateCellStyle();
                StyleRTD_CC.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleRTD_CC.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleRTD_CC.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleRTD_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleRTD_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleRTD_CC.SetFont(Font);
                StyleRTD_CC.WrapText = true;

                var StyleLTD_CC = WorkBook.CreateCellStyle();
                StyleLTD_CC.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLTD_CC.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLTD_CC.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLTD_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleLTD_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleLTD_CC.SetFont(Font);
                StyleLTD_CC.WrapText = true;

                var StyleTD_CC = WorkBook.CreateCellStyle();
                StyleTD_CC.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleTD_CC.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleTD_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleTD_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleTD_CC.SetFont(Font);
                StyleTD_CC.WrapText = true;

                var Objects = Protokol.Objects;
                var ProbeType = T.PType.Rows.Get<string>(Protokol[0].PTypeID, C.PType.Name);
                var Location = Protokol.ObjectsLocations;

                var Cause = Protokol.SCause;
                string UnDeleteSheetName;
                var AddInSheetName = T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.PSG, C.PSG.Name);

                Location = T.SPoint.Rows.Get<string>(Protokol[0].SPointID, C.SPoint.Name);

                var Probes = Protokol.Numbers;
                var NormID = Protokol[0].NormID;

                UnDeleteSheetName = "Концентрации";
                {
                    int Index = WorkBook.GetSheetIndex(UnDeleteSheetName);
                    ByPodrSheet = WorkBook.GetSheetAt(Index);
                    WorkBook.SetSheetName(Index, "Концентрации");
                }

                if (ByPodrSheet == null)
                {
                    MessageBox.Show("В шаблоне не найден лист \"" + UnDeleteSheetName + "\", вывод невозможен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }

                var Cells = new ATMisc.CellMark_struct[Protokol.MarkCount];

                var Exchange = new CellExchange_Class(ByPodrSheet);

                /*for (int i = 0; i < Protokol.MarkCount; i++)
                {
                    Cells[i] = new ATMisc.CellMark_struct("{Оценка " + Protokol.GetMarkName(i) + '}');

                    int Index = i;
                    Exchange.AddExchange(Cells[i].Mark, (Cell) =>
                    { Cells[Index].ColumnIndex = Cell.ColumnIndex; Cells[Index].RowIndex = Cell.RowIndex; }, 1);
                }*/

                Cells[0] = new ATMisc.CellMark_struct("{4 дня " + Protokol.GetMarkName(0) + '}');

                Exchange.AddExchange(Cells[0].Mark, (Cell) =>
                { Cells[0].ColumnIndex = Cell.ColumnIndex; Cells[0].RowIndex = Cell.RowIndex; }, 1);

                Cells[1] = new ATMisc.CellMark_struct("{3 дня " + Protokol.GetMarkName(1) + '}');

                Exchange.AddExchange(Cells[1].Mark, (Cell) =>
                { Cells[1].ColumnIndex = Cell.ColumnIndex; Cells[1].RowIndex = Cell.RowIndex; }, 1);

                if (Exchange.Exchange())
                {
                    for (int i = 0; i < Cells.Length; i++)
                    {
                        if (!Cells[i].Check())
                        { return false; }
                    }

                    var tmpDT = ATMisc.GetDateTime(Protokol[0].AYMD);

                    var Tm = new DataBase.Time(Protokol.Time);

                    tmpDT = tmpDT.AddDays(4).AddHours(Tm.Hours).AddMinutes((Protokol.SGroup == data.SGroup.Toxicity2 ? Tm.Minutes + 4 * 60 : Tm.Minutes));

                    //if (tmpDT.DayOfWeek == DayOfWeek.Saturday || tmpDT.DayOfWeek == DayOfWeek.Sunday)
                    //{ throw new Exception("День окончания проведения испытаний приходится на выходной."); }

                    ByPodrSheet.GetRow(Cells[0].RowIndex).GetCell(Cells[0].ColumnIndex).SetCellValue(ATMisc.GetDateTime(Protokol[0].AYMD).ToShortDateString() + '-' + tmpDT.ToShortDateString() + ' ' + (tmpDT.Hour > 9 ? tmpDT.Hour.ToString() : '0' + tmpDT.Hour.ToString()) + ':' + (tmpDT.Minute > 9 ? tmpDT.Minute.ToString() : '0' + tmpDT.Minute.ToString()));

                    tmpDT = ATMisc.GetDateTime(Protokol[0].AYMD).AddDays(3).AddHours(Tm.Hours).AddMinutes((Protokol.SGroup == data.SGroup.Toxicity2 ? Tm.Minutes + 4 * 60 : Tm.Minutes));

                    //if (tmpDT.DayOfWeek == DayOfWeek.Saturday || tmpDT.DayOfWeek == DayOfWeek.Sunday)
                    //{ throw new Exception("День окончания проведения испытаний приходится на выходной."); }

                    ByPodrSheet.GetRow(Cells[1].RowIndex).GetCell(Cells[1].ColumnIndex).SetCellValue(ATMisc.GetDateTime(Protokol[0].AYMD).ToShortDateString() + '-' + tmpDT.ToShortDateString() + ' ' + (tmpDT.Hour > 9 ? tmpDT.Hour.ToString() : '0' + tmpDT.Hour.ToString()) + ':' + (tmpDT.Minute > 9 ? tmpDT.Minute.ToString() : '0' + tmpDT.Minute.ToString()));
                }

                for (int i = WorkBook.NumberOfSheets - 1; i > -1; i--)
                {
                    if (WorkBook.GetSheetAt(i).SheetName.ToLower() != "заголовок" && WorkBook.GetSheetAt(i).SheetName.ToLower() != "концентрации")
                    { WorkBook.RemoveSheetAt(i); }
                }

                string PeopleNames;

                {
                    var Ppls = new List<uint>();

                    for (int i = 0; i < Protokol.SampleCount; i++)
                    {
                        for (int j = 0; j < Ppls.Count; j++)
                        {
                            if (Ppls[j] == Protokol[i].PeopleID)
                            { goto Finded; }
                        }

                        Ppls.Add(Protokol[i].PeopleID);

                    Finded: ;
                    }

                    PeopleNames = Misc.GetShortFIO(Ppls[0]);

                    for (int i = 1; i < Ppls.Count; i++)
                    { PeopleNames += ", " + Misc.GetShortFIO(Ppls[i]); }
                }

                var DT = Protokol.Date;

                var ProtokolNum = ProtokolNumber(Protokol);

                GetProtokolsExchanges(TitleSheet
                                    , DT.Year
                                    , ProtokolNum
                                    , Objects
                                    , Objects
                                    , ProbeType
                                    , Location
                                    , Protokol.DateOstr
                                    , Protokol.DateP
                                    , Protokol.StrTime
                                    , PeopleNames
                                    , Cause
                                    , Probes
                                    , DT.Day.ToString()
                                    , ATMisc.GetMonthName2(DT.Month)
                                    , DT.Month.ToString()
                                    , RCache.PSG.GetMethodName(data.PSG.Income)
                                    , T.PaPoS.Rows.Get<string>(Protokol.PaPoSID, C.PaPoS.Name)
                                    , T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.FllName)
                                    , T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName)
                                    , T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.Contact)
                                    , Protokol.Act).Exchange(0, 25, 0, 25);

                var Exchanges = new CellExchange_Class(TitleSheet);

                Exchanges.ClearExchanges();

                Exchanges.AddColumn("{имя свойства}");
                Exchanges.AddColumn("{ед. свойства}");
                Exchanges.AddColumn("{значение свойства}");

                if (Exchanges.CheckTableColumns())
                {
                    for (int i = 0; i < Protokol[0].TCSCount; i++)
                    {
                        var Values = new object[3];

                        Values[0] = Protokol[0].TCSName(i);
                        Values[1] = Protokol[0].TCSEdType(i);
                        Values[2] = Protokol[0].TCSValue(i);

                        Exchanges.SetRow(Values);
                    }
                }

                Exchanges = new CellExchange_Class(ByPodrSheet);

                Exchanges.AddExchange("{должность ответственного}", T.People.Rows.Get<string>(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Protokol[0].PodrID, C.Podr.PSG)), C.People.Prfssn, C.Prfssn.Name), 5);
                Exchanges.AddExchange("{ФИО ответственного}", Misc.GetShortFIO(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Protokol[0].PodrID, C.Podr.PSG))), 5);
                Exchanges.AddExchange("{Номер протокола}", ProtokolNum, 5);
                Exchanges.AddExchange("{Дата}", DT.ToShortDateString(), 5);

                SetResp(Exchanges, Protokol.PodrID, data.TResp.LaboratoryProtokol);

                return SaveExcel(WorkBook, NewFileName, Open);
            }
            else
            {
                if (Open)
                { System.Diagnostics.Process.Start(NewFileName); }

                return true;
            }
        }
    }
}