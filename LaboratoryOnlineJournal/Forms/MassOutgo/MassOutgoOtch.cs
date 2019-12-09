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
        public static bool OtchMassOutgo(uint PodrID, int YMFrom, Podrs_class.PeriodType pt, uint OTypeID, bool ShowErrorMessage = true, bool CreateNew = true, bool Open = true)
        {
            bool Returning = false;

            RCache.Podrs = new RCache.Podrs_class();
            var Podrs = new Podrs_class(PodrID, pt, YMFrom, OTypeID);

            int Year, Month;
            ATMisc.GetYearMonthFromYM(YMFrom, out Year, out Month);
            if (Podrs.MarksCount == 0)
            {
                if (ShowErrorMessage)
                { MessageBox.Show("Показатели не обнаружены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                return false;
            }

            {
                int tMCount = 0;

                for (int i = 0; i < Podrs.MarkCount; i++)
                {
                    if (Podrs.ShowMark(i))
                    { tMCount++; }
                }

                if (tMCount == 0)
                {
                    if (ShowErrorMessage)
                    { MessageBox.Show("Показатели не обнаружены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                    return false;
                }
            }

            G.OPType.QUERRY().SHOW.DO();

            var StylesEDType = new NPOI.SS.UserModel.ICellStyle[G.OPType.Rows.Count];

            if (!Directory.Exists(Application.StartupPath + "\\Отчеты"))
            { Directory.CreateDirectory(Application.StartupPath + "\\Отчеты"); }

            for (int i = 0; i < Podrs.Count; i++)   //подразделения
            {
                if (Podrs[i].MarksCount > 0)
                {
                    var NewFileName = Application.StartupPath + "\\Отчеты\\Расчет массы " + Podrs.PeriodName() + ' ' + Year.ToString() + ' ' + T.Podr.Rows.Get<string>(Podrs[i].PodrID, C.Podr.ShrName) + ".xls";

                    if (CreateNew || !File.Exists(NewFileName))
                    {
                        var WorkBook = ATMisc.GetGenericExcel("Расчет массы.xls");

                        if (WorkBook == null)
                        {
                            return false;
                        }

                        var Sheet1 = WorkBook.GetSheet("Таблица");

                        if (Sheet1 == null)
                        {
                            if (ShowErrorMessage)
                            {
                                MessageBox.Show("В шаблоне не найден лист \"Заголовок\", вывод невозможен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            return false;
                        }

                        var Exchanges = new CellExchange_Class(Sheet1);

                        NPOI.SS.UserModel.ICellStyle NumberStyle = null
                                                   , NameStyle = null
                                                   , EdTypeStyle = null
                                                   , CodeStyle = null
                                                   , FactStyle = null
                                                   , BackStyle = null
                                                   , CalcStyle = null
                                                   , MassStyle = null;

                        var Podr = RCache.Podrs[Podrs[i].PodrID];

                        Exchanges.AddExchange("{сооружение}", Podr.ShortName, 2);

                        for (int s = 0; s < Podrs[i].Count; s++)
                        {
                            if (Podrs[i][s].ObjectID > 0)
                            {
                                Exchanges.AddExchange("{объект}", T.Object.Rows.Get<string>(Podrs[i][s].ObjectID, C.Object.OType, C.OType.Name).ToLower(), 2);
                                break;
                            }
                        }

                        if (Podr.Ppls.Count == 0 || (data.PnMean)Podr.Ppls[0].PnMeanID != data.PnMean.Nachalnic)
                        {
                            if (ShowErrorMessage)
                            { MessageBox.Show("У сооружения " + Podr.ShortName + " нет начальника.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                            return false;
                        }

                        Exchanges.AddExchange("{начальник сооружения}", Podr.Ppls[0].PeopleName, 2);
                        Exchanges.AddExchange("{профессия начальника}", Podr.Ppls[0].Profession, 2);
                        {
                            var PeopleID = RCache.PSG.GetPeopleID(Podr.PSG);

                            Exchanges.AddExchange("{профессия ответственного}", T.People.Rows.Get<string>(PeopleID, C.People.Prfssn, C.Prfssn.Name), 2);
                            Exchanges.AddExchange("{ФИО ответственного}", Misc.GetShortFIO(PeopleID), 2);
                        }
                        Exchanges.AddExchange("{год}", Year.ToString(), 5);

                        int NumberIndex = -1, NameIndex = -1, EdTypeIndex = -1, CodeIndex = -1, FactIndex = -1, BackIndex = -1, CalcIndex = -1, MassIndex = -1, RowIndex = -1;
                        {
                            int NumberRowIndex = -1, NameRowIndex = -1, EdTypeRowIndex = -1, CodeRowIndex = -1, FactRowIndex = -1, BackRowIndex = -1, CalcRowIndex = -1, MassRowIndex = -1;

                            Exchanges.AddExchange("{выпуск}", Cell =>
                            {
                                NumberIndex = Cell.ColumnIndex;
                                NumberRowIndex = Cell.RowIndex;
                                NumberStyle = Cell.CellStyle;
                            }, 1);
                            Exchanges.AddExchange("{имя}",
                                Cell =>
                                {
                                    NameIndex = Cell.ColumnIndex;
                                    NameRowIndex = Cell.RowIndex;
                                    NameStyle = Cell.CellStyle;
                                }, 1);
                            Exchanges.AddExchange("{едизм}",
                                Cell =>
                                {
                                    EdTypeIndex = Cell.ColumnIndex;
                                    EdTypeRowIndex = Cell.RowIndex;
                                    EdTypeStyle = Cell.CellStyle;
                                }, 1);
                            Exchanges.AddExchange("{код}",
                                Cell =>
                                {
                                    CodeIndex = Cell.ColumnIndex;
                                    CodeRowIndex = Cell.RowIndex;
                                    CodeStyle = Cell.CellStyle;
                                }, 1);
                            Exchanges.AddExchange("{факт}",
                                Cell =>
                                {
                                    FactIndex = Cell.ColumnIndex;
                                    FactRowIndex = Cell.RowIndex;
                                    FactStyle = Cell.CellStyle;
                                }, 1);
                            Exchanges.AddExchange("{фон}",
                                Cell =>
                                {
                                    BackIndex = Cell.ColumnIndex;
                                    BackRowIndex = Cell.RowIndex;
                                    BackStyle = Cell.CellStyle;
                                }, 1);
                            Exchanges.AddExchange("{расчет}",
                                Cell =>
                                {
                                    CalcIndex = Cell.ColumnIndex;
                                    CalcRowIndex = Cell.RowIndex;
                                    CalcStyle = Cell.CellStyle;
                                }, 1);
                            Exchanges.AddExchange("{масса}",
                                Cell =>
                                {
                                    MassIndex = Cell.ColumnIndex;
                                    MassRowIndex = Cell.RowIndex;

                                    MassStyle = Cell.CellStyle;

                                    for (int j = 0; j < StylesEDType.Length; j++)
                                    {
                                        StylesEDType[j] = WorkBook.CreateCellStyle();
                                        CopyStyleFromCell(StylesEDType[j], Cell);
                                        StylesEDType[j].DataFormat = WorkBook.CreateDataFormat().GetFormat("#,##0.000\" " + G.OPType.Rows.Get<string>(j, C.OPType.EdTypeT, C.EdType.Name) + "\"");
                                    }
                                }, 1);

                            Exchanges.AddExchange("{период}", Podrs.PeriodName(), 2);

                            SetResp(Exchanges, Podr.ID, data.TResp.LaboratoryProtokol);

                            if (NameRowIndex == NumberRowIndex && NumberRowIndex == FactRowIndex && NumberRowIndex == BackRowIndex && BackRowIndex == CalcRowIndex && NumberRowIndex == MassRowIndex)
                            {
                                if (MassRowIndex == -1)
                                {
                                    if (ShowErrorMessage)
                                    {
                                        MessageBox.Show("Критически важные колонки таблицы не найдены(выпуск,наименование, факт, фон, расчет, масса).", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                    return false;
                                }
                            }
                            else
                            {
                                if (ShowErrorMessage)
                                { MessageBox.Show("Метки всех колонок должны находиться на одной строке.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                                return false;
                            }

                            RowIndex = NumberRowIndex;

                            Sheet1.ShiftRows(RowIndex + 1, Sheet1.LastRowNum, -1);
                        }

                        Sheet1.SetColumnHidden(BackIndex, !Podrs.AllowBack);
                        Sheet1.SetColumnHidden(CalcIndex, !Podrs.AllowBack);

                        if (!Podrs.AllowBack)
                        {
                            int NewWidth = Sheet1.GetColumnWidth(BackIndex) + Sheet1.GetColumnWidth(FactIndex) + Sheet1.GetColumnWidth(CalcIndex);
                            Sheet1.SetColumnWidth(FactIndex, NewWidth);
                        }

                        for (int j = 0; j < Podrs[i].Count; j++)   //выпуск
                        {
                            Sheet1.ShiftRows(RowIndex, Sheet1.LastRowNum, Podrs[i][j].Count + 2);

                            var LastRow = Sheet1.CreateRow(RowIndex + Podrs[i][j].Count);

                            if (EdTypeIndex > -1)
                            { ATMisc.SetValue(LastRow, "тыс.м3", EdTypeIndex, EdTypeStyle); }

                            ATMisc.SetValue(LastRow, T.Object.Rows.Get<string>(Podrs[i][j].ObjectID, C.Object.OLocationTo, C.OLocation.Name), MassIndex, MassStyle);

                            if (Podrs[i][j].Count > 0)
                            {
                                for (int k = 0; k < Podrs[i][j].Count; k++)
                                {
                                    var MarkID = Podrs[i][j].MarkID(k);

                                    var Row = Sheet1.CreateRow(RowIndex++);
                                    if (NameIndex > -1)
                                    { ATMisc.SetValue(Row, T.Mark.Rows.Get<string>(MarkID, C.Mark.Name), NameIndex, NameStyle); }
                                    if (EdTypeIndex > -1)
                                    { ATMisc.SetValue(Row, T.Mark.Rows.Get<string>(MarkID, C.Mark.EdType, C.EdType.Name), EdTypeIndex, EdTypeStyle); }
                                    if (CodeIndex > -1)
                                    { ATMisc.SetValue(Row, T.Mark.Rows.Get<int>(MarkID, C.Mark.Code), CodeIndex, CodeStyle); }

                                    double Front = RCache.Marks[MarkID].GetRound(Podrs[i][j][k].FMiddle)
                                         , Back = RCache.Marks[MarkID].GetRound(Podrs[i][j][k].BMiddle);

                                    ATMisc.SetValue(Row, Front, FactIndex, FactStyle);

                                    ATMisc.SetValue(Row, Back, BackIndex, BackStyle);

                                    ATMisc.SetFormula(Row, "if(" + GetColCharName(FactIndex) + RowIndex.ToString() + "<=" + GetColCharName(BackIndex) + RowIndex.ToString() + ", \"<=фон\", " + GetColCharName(FactIndex) + RowIndex.ToString() + "-" + GetColCharName(BackIndex) + RowIndex.ToString() + ")", CalcIndex, CalcStyle);
                                    ATMisc.SetFormula(Row, "if(" + GetColCharName(CalcIndex) + RowIndex.ToString() + "=\"<=фон\",\"\", " + GetColCharName(CalcIndex) + RowIndex.ToString() + "*" + GetColCharName(FactIndex) + (LastRow.RowNum + 1).ToString() + "*" + DTSF(RCache.Marks[MarkID].Multyply) + ")", MassIndex, StylesEDType[G.OPType.Rows.GetIndex(RCache.Marks[MarkID].OPTypeID)]);

                                    if (NumberIndex > -1)
                                    { ATMisc.SetValue(Row, "", NumberIndex, NumberStyle); }
                                }

                                if (NumberIndex > -1)
                                {
                                    ATMisc.SetValue(Sheet1.GetRow(RowIndex - Podrs[i][j].Count), Podrs[i][j].VShortName + " [" + (j + 1).ToString() + "]", NumberIndex, NumberStyle);
                                }

                                if (NumberIndex > -1)
                                { ATMisc.SetValue(LastRow, "", NumberIndex, NumberStyle); }
                            }
                            else
                            {
                                if (NumberIndex > -1)
                                {
                                    ATMisc.SetValue(LastRow, Podrs[i][j].VShortName + " [" + (j + 1).ToString() + "]", NumberIndex, NumberStyle);
                                }
                            }
                            RowIndex++;

                            ATMisc.SetValue(LastRow, Podrs[i][j].VolumeSumm, FactIndex, FactStyle);
                            ATMisc.SetValue(LastRow, "Объём сброшеных вод", NameIndex, NameStyle);

                            if (EdTypeIndex > -1)
                            { ATMisc.SetValue(LastRow, "тыс.м3", EdTypeIndex, EdTypeStyle); }

                            if (CodeIndex > -1)
                            { ATMisc.SetValue(LastRow, "", CodeIndex, CodeStyle); }

                            ATMisc.SetValue(LastRow, "", BackIndex, BackStyle);
                            ATMisc.SetValue(LastRow, "", CalcIndex, CalcStyle);

                            Sheet1.CreateRow(RowIndex++).Height = 50;
                        }

                        Returning = Returning || SaveExcel(WorkBook, NewFileName, Open);
                    }
                    else if (Open && File.Exists(NewFileName))
                    {
                        System.Diagnostics.Process.Start(NewFileName);
                        Returning = true;
                    }

                }
            }

            return Returning;
        }
    }
}