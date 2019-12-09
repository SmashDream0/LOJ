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
        public static bool OtchMiddleMarks(MiddleMarks_class Marks, bool ShowErrorMessage = true, bool Open = true)
        {
            var WorkBook = ATMisc.GetGenericExcel("Средние концентрации за год.xls");

            if (WorkBook == null) return false;

            var Sheet = WorkBook.GetSheet("печать");

            if (Sheet == null)
            {
                if (ShowErrorMessage)
                { MessageBox.Show("В шаблоне не найден лист с именем \"печать\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                return false;
            }

            var Exchange = new CellExchange_Class(Sheet);

            if (Marks.PodrID == 0)
            { Exchange.AddExchange("{подразделение}", "Все подразделения", 5); }
            else
            { Exchange.AddExchange("{подразделение}", T.Podr.Rows.Get<string>(Marks.PodrID, C.Podr.ShrName), 5); }

            var ObjectName = (Marks.ObjectID == 0 ? T.Object.Rows.Get<string>(Marks.ObjectID, C.Object.Name) : "Все");

            Exchange.AddExchange("{объект}", ObjectName, 5);

            Exchange.AddExchange("{год}", Marks.Year.ToString(), 5);

            var MarkNameIndex = new SColumn_struct(-1, null);
            var VGIndex = new SColumn_struct(-1, null);
            var EdTypeIndex = new SColumn_struct(-1, null);
            var MiddlesMonthIndexes = new SColumn_struct[12];
            var MiddlesQIndexes = new SColumn_struct[4];
            var MiddleYearMarkIndex = new SColumn_struct(-1, null);

            int MarkRowIndex = -1;
            int VGRowIndex = -1;

            {
                int MarkNameRowIndex = -1;
                int EdTypeRowIndex = -1;
                var MiddlesMonthRowIndexes = new int[12];
                var MiddlesQRowIndexes = new int[4];
                int MiddleYearMarkRowIndex = -1;

                var MiddlesMonthVGRowIndexes = new int[12];
                var MiddlesQVGRowIndexes = new int[4];

                Exchange.AddExchange("{показатель}", Cell =>
                {
                    MarkNameIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MarkNameRowIndex = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{выпуск}", Cell =>
                {
                    VGIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    VGRowIndex = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{едизм}", Cell =>
                {
                    EdTypeIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    EdTypeRowIndex = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Январь}", Cell =>
                {
                    MiddlesMonthIndexes[0] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[0] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Февраль}", Cell =>
                {
                    MiddlesMonthIndexes[1] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[1] = Cell.RowIndex;

                }, 1);

                Exchange.AddExchange("{средний за Март}", Cell =>
                {
                    MiddlesMonthIndexes[2] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[2] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 1 кв.}", Cell =>
                {
                    MiddlesQIndexes[0] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[0] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Апрель}", Cell =>
                {
                    MiddlesMonthIndexes[3] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[3] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Май}", Cell =>
                {
                    MiddlesMonthIndexes[4] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[4] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Июнь}", Cell =>
                {
                    MiddlesMonthIndexes[5] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[5] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 2 кв.}", Cell =>
                {
                    MiddlesQIndexes[1] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[1] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Июль}", Cell =>
                {
                    MiddlesMonthIndexes[6] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[6] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Август}", Cell =>
                {
                    MiddlesMonthIndexes[7] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[7] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Сентябрь}", Cell =>
                {
                    MiddlesMonthIndexes[8] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[8] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 3 кв.}", Cell =>
                {
                    MiddlesQIndexes[2] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[2] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Октябрь}", Cell =>
                {
                    MiddlesMonthIndexes[9] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[9] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Ноябрь}", Cell =>
                {
                    MiddlesMonthIndexes[10] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[10] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Декабрь}", Cell =>
                {
                    MiddlesMonthIndexes[11] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[11] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 4 кв.}", Cell =>
                {
                    MiddlesQIndexes[3] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[3] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за За год}", Cell =>
                {
                    MiddleYearMarkIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddleYearMarkRowIndex = Cell.RowIndex;
                }, 1);

                if (Marks.PodrID > 0)
                {
                    Exchange.AddExchange("{должность ответственного}", T.People.Rows.Get<string>(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Marks.PodrID, C.Podr.PSG)), C.People.Prfssn, C.Prfssn.Name), 5);
                    Exchange.AddExchange("{ФИО ответственного}", Misc.GetShortFIO(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Marks.PodrID, C.Podr.PSG))), 5);
                }
                else
                {
                    Exchange.AddExchange("{должность ответственного}", "", 5);
                    Exchange.AddExchange("{ФИО ответственного}", "", 5);
                }

                SetResp(Exchange, Marks.PodrID, data.TResp.LaboratoryProtokol);

                if (MarkNameRowIndex == EdTypeRowIndex
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[0]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[1]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[2]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[3]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[4]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[5]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[6]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[7]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[8]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[9]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[10]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[11]
                 && EdTypeRowIndex == MiddlesQRowIndexes[0]
                 && EdTypeRowIndex == MiddlesQRowIndexes[1]
                 && EdTypeRowIndex == MiddlesQRowIndexes[2]
                 && EdTypeRowIndex == MiddlesQRowIndexes[3]
                 && EdTypeRowIndex == MiddleYearMarkRowIndex)
                { MarkRowIndex = EdTypeRowIndex; }
                else
                {
                    if (ShowErrorMessage) MessageBox.Show("Не все колонки показателей были найдены или не все из них расположены на одной строке.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            Sheet.ShiftRows(MarkRowIndex + 1, Sheet.LastRowNum, -1);  //затираю метки

            int RowIndex = MarkRowIndex;

            for (int i = 0; i < Marks.MarkCount; i++)
            {
                var Mark = RCache.Marks[Marks.MarkID(i)];

                if (Marks.GetMarkEnabled(i) && (Marks.YearSumm(i) > 0 || Mark.ShowZero))
                {
                    Sheet.ShiftRows(RowIndex, Sheet.LastRowNum, 1);

                    var Row = Sheet.CreateRow(RowIndex++);
                    //выпуск
                    ATMisc.SetValue(Row, "", VGIndex.Index, VGIndex.Style);
                    //наименование
                    ATMisc.SetValue(Row, Marks.MarkName(i), MarkNameIndex.Index, MarkNameIndex.Style);
                    //едизм
                    ATMisc.SetValue(Row, Marks.EdName(i), EdTypeIndex.Index, EdTypeIndex.Style);
                    //месяцы
                    for (byte j = 0; j < MiddlesMonthIndexes.Length; j++)
                    { ATMisc.SetValue(Row, Mark.GetRoundedVolume(Marks.MonthSumm(i, j)), MiddlesMonthIndexes[j].Index, MiddlesMonthIndexes[j].Style); }
                    //кварталы
                    for (byte j = 0; j < MiddlesQIndexes.Length; j++)
                    { ATMisc.SetValue(Row, Mark.GetRoundedVolume(Marks.QuartalSumm(i, j)), MiddlesQIndexes[j].Index, MiddlesQIndexes[j].Style); }
                    //за год
                    ATMisc.SetValue(Row, Mark.GetRoundedVolume(Marks.YearSumm(i)), MiddleYearMarkIndex.Index, MiddleYearMarkIndex.Style);
                }
            }

            string PodrName;

            if (Marks.PodrID > 0)
            { PodrName = ' ' + T.Podr.Rows.Get<string>(Marks.PodrID, C.Podr.ShrName); }
            else
            { PodrName = "все"; }

            if (!Directory.Exists(Application.StartupPath + "\\Отчеты"))
            { Directory.CreateDirectory(Application.StartupPath + "\\Отчеты"); }
            
            return SaveExcel(WorkBook, Application.StartupPath + "\\Отчеты\\СП " + PodrName + ", " + ObjectName + ", " + Marks.Year.ToString() + ".xls", Open, ShowErrorMessage);
        }

        /*public static bool OtchMiddleMarks(MiddleMarks_class Marks, bool ShowErrorMessage = true, bool Open = true)
        {
            var WorkBook = ATMisc.GetGenericExcel("Средние концентрации за год.xls");

            if (WorkBook == null) return false;

            var Sheet = WorkBook.GetSheet("печать");

            if (Sheet == null)
            {
                if (ShowErrorMessage)
                { MessageBox.Show("В шаблоне не найден лист с именем \"печать\"", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                return false;
            }

            var Exchange = new CellExchange_Class(Sheet);

            if (Marks.PodrID == 0)
            { Exchange.AddExchange("{подразделение}", "Все подразделения", 5); }
            else
            { Exchange.AddExchange("{подразделение}", T.Podr.Rows.Get<string>(Marks.PodrID, C.Podr.ShrName), 5); }

            var ObjectName = "";

            for (int i = 0; i < Marks.OIDs.Length; i++)
            {
                var OTN = T.Object.Rows.Get<string>(Marks.OIDs[i], C.Object.Name);

                if (OTN.Length > 0)
                { ObjectName += ", " + OTN; }
            }

            ObjectName = ObjectName.Substring(1);

            Exchange.AddExchange("{объект}", ObjectName, 5);

            Exchange.AddExchange("{год}", Marks.ShowYear.ToString(), 5);

            var MarkNameIndex = new SColumn_struct(-1, null);
            var VGIndex = new SColumn_struct(-1, null);
            var EdTypeIndex = new SColumn_struct(-1, null);
            var MiddlesMonthIndexes = new SColumn_struct[12];
            var MiddlesQIndexes = new SColumn_struct[4];
            var MiddleYearMarkIndex = new SColumn_struct(-1, null);

            int MarkRowIndex = -1;
            int VGRowIndex = -1;

            {
                int MarkNameRowIndex = -1;
                int EdTypeRowIndex = -1;
                var MiddlesMonthRowIndexes = new int[12];
                var MiddlesQRowIndexes = new int[4];
                int MiddleYearMarkRowIndex = -1;

                var MiddlesMonthVGRowIndexes = new int[12];
                var MiddlesQVGRowIndexes = new int[4];

                Exchange.AddExchange("{показатель}", Cell =>
                {
                    MarkNameIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MarkNameRowIndex = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{выпуск}", Cell =>
                {
                    VGIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    VGRowIndex = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{едизм}", Cell =>
                {
                    EdTypeIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    EdTypeRowIndex = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Январь}", Cell =>
                {
                    MiddlesMonthIndexes[0] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[0] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Февраль}", Cell =>
                {
                    MiddlesMonthIndexes[1] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[1] = Cell.RowIndex;

                }, 1);

                Exchange.AddExchange("{средний за Март}", Cell =>
                {
                    MiddlesMonthIndexes[2] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[2] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 1 кв.}", Cell =>
                {
                    MiddlesQIndexes[0] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[0] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Апрель}", Cell =>
                {
                    MiddlesMonthIndexes[3] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[3] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Май}", Cell =>
                {
                    MiddlesMonthIndexes[4] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[4] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Июнь}", Cell =>
                {
                    MiddlesMonthIndexes[5] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[5] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 2 кв.}", Cell =>
                {
                    MiddlesQIndexes[1] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[1] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Июль}", Cell =>
                {
                    MiddlesMonthIndexes[6] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[6] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Август}", Cell =>
                {
                    MiddlesMonthIndexes[7] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[7] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Сентябрь}", Cell =>
                {
                    MiddlesMonthIndexes[8] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[8] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 3 кв.}", Cell =>
                {
                    MiddlesQIndexes[2] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[2] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Октябрь}", Cell =>
                {
                    MiddlesMonthIndexes[9] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[9] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Ноябрь}", Cell =>
                {
                    MiddlesMonthIndexes[10] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[10] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за Декабрь}", Cell =>
                {
                    MiddlesMonthIndexes[11] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesMonthRowIndexes[11] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за 4 кв.}", Cell =>
                {
                    MiddlesQIndexes[3] = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddlesQRowIndexes[3] = Cell.RowIndex;
                }, 1);

                Exchange.AddExchange("{средний за За год}", Cell =>
                {
                    MiddleYearMarkIndex = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    MiddleYearMarkRowIndex = Cell.RowIndex;
                }, 1);

                if (Marks.PodrID > 0)
                {
                    Exchange.AddExchange("{должность ответственного}", T.People.Rows.Get<string>(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Marks.PodrID, C.Podr.PSG)), C.People.Prfssn, C.Prfssn.Name), 5);
                    Exchange.AddExchange("{ФИО ответственного}", Misc.GetShortFIO(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Marks.PodrID, C.Podr.PSG))), 5);
                }
                else
                {
                    Exchange.AddExchange("{должность ответственного}", "", 5);
                    Exchange.AddExchange("{ФИО ответственного}", "", 5);
                }

                SetResp(Exchange, Marks.PodrID, data.TResp.LaboratoryProtokol);

                if (MarkNameRowIndex == EdTypeRowIndex
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[0]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[1]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[2]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[3]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[4]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[5]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[6]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[7]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[8]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[9]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[10]
                 && EdTypeRowIndex == MiddlesMonthRowIndexes[11]
                 && EdTypeRowIndex == MiddlesQRowIndexes[0]
                 && EdTypeRowIndex == MiddlesQRowIndexes[1]
                 && EdTypeRowIndex == MiddlesQRowIndexes[2]
                 && EdTypeRowIndex == MiddlesQRowIndexes[3]
                 && EdTypeRowIndex == MiddleYearMarkRowIndex)
                {
                    MarkRowIndex = EdTypeRowIndex;
                }
                else
                {
                    if (ShowErrorMessage) MessageBox.Show("Не все колонки показателей были найдены или не все из них расположены на одной строке.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            Sheet.ShiftRows(MarkRowIndex + 1, Sheet.LastRowNum, -1);  //затираю метки

            int RowIndex = MarkRowIndex;
            int VGCount = 0;

            for (int k = 0; k < Marks.VGCount; k++)
            {
                int FRowIndex = RowIndex;

                for (int i = 0; i < Marks.Length; i++)
                {
                    if (Marks[i].AllowToShow && (Marks[i].YearSumm(k) > 0 || RCache.Marks[i].ShowZero))
                    {
                        Sheet.ShiftRows(RowIndex, Sheet.LastRowNum, 1);

                        var Row = Sheet.CreateRow(RowIndex++);
                        //выпуск
                        ATMisc.SetValue(Row, "", VGIndex.Index, VGIndex.Style);
                        //наименование
                        ATMisc.SetValue(Row, Marks[i].MarkName, MarkNameIndex.Index, MarkNameIndex.Style);
                        //едизм
                        ATMisc.SetValue(Row, Marks[i].EdType, EdTypeIndex.Index, EdTypeIndex.Style);
                        //месяцы
                        for (byte j = 0; j < MiddlesMonthIndexes.Length; j++)
                        { ATMisc.SetValue(Row, RCache.Marks[i].GetRoundedVolume(Marks[i].GetMiddleMonthSumm(k, j)), MiddlesMonthIndexes[j].Index, MiddlesMonthIndexes[j].Style); }
                        //кварталы
                        for (byte j = 0; j < MiddlesQIndexes.Length; j++)
                        { ATMisc.SetValue(Row, RCache.Marks[i].GetRoundedVolume(Marks[i].GetMiddleQuartalSumm(k, j)), MiddlesQIndexes[j].Index, MiddlesQIndexes[j].Style); }
                        //за год
                        ATMisc.SetValue(Row, RCache.Marks[i].GetRoundedVolume(Marks[i].YearSumm(k)), MiddleYearMarkIndex.Index, MiddleYearMarkIndex.Style);
                    }
                }

                if (Marks.VGID(k) > 0)
                { ATMisc.SetValue(Sheet.GetRow(FRowIndex), Marks.VGShortName(k) + "[" + (++VGCount).ToString() + "]", VGIndex.Index, VGIndex.Style); }
                else
                { ATMisc.SetValue(Sheet.GetRow(FRowIndex), Marks.VGShortName(k), VGIndex.Index, VGIndex.Style); }
            }

            string PodrName;

            if (Marks.PodrID > 0)
            { PodrName = " " + T.Podr.Rows.Get<string>(Marks.PodrID, C.Podr.ShrName); }
            else
            { PodrName = "все"; }

            if (!Directory.Exists(Application.StartupPath + "\\Отчеты"))
            { Directory.CreateDirectory(Application.StartupPath + "\\Отчеты"); }

            if (Marks.OIDs.Length == G.Object.Rows.Count)
            { ObjectName = "Все"; }
            else
            {
                ObjectName = Marks.OIDs.Length.ToString() + " из " + G.Object.Rows.Count.ToString();
            }

            return SaveExcel(WorkBook, Application.StartupPath + "\\Отчеты\\СП " + PodrName + ", " + ObjectName + ", " + Marks.ShowYear.ToString() + ".xls", Open, ShowErrorMessage);
        }*/
    }
}