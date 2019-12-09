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
        public static bool OtchProtokolType4(Protokols_class.SGroup_class.Protokol_class Protokol, bool CreateNew = true, bool Open = true)
        {
            {
                var MsgErr = "";

                for (int i = 0; i < Protokol.MarkCount; i++)
                {
                    var PAMIndex = -1;

                    for (int j = 0; j < Protokol.SampleCount; j++)
                    {
                        if (Protokol[j][i].LocalAlow && Protokol[j][i].Method.Length > 0)
                        {
                            if (PAMIndex < 0)
                            { PAMIndex = j; }
                            else if (Protokol[j][i].Method != Protokol[PAMIndex][i].Method)
                            {
                                MsgErr += '\n' + Protokol[j][i].Mark + " имеет различные методы у нормативов " + T.Object.Rows.Get<string>(Protokol[j].ObjectID, C.Object.Norm, C.Norm.Name) + " и " + T.Object.Rows.Get<string>(Protokol[PAMIndex].ObjectID, C.Object.Norm, C.Norm.Name);
                            }
                        }
                    }
                }

                if (MsgErr.Length > 0)
                {
                    MessageBox.Show(MsgErr, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return false;
                }
            }

            string NewFileName;
            {
                int Month, Year;

                ATMisc.GetYearMonthFromYM(Employe_Form.SPoints.YM, out Year, out Month);

                {
                    NewFileName = Application.StartupPath + "\\Отчеты\\";

                    if (!Directory.Exists(NewFileName)) Directory.CreateDirectory(NewFileName);

                    NewFileName += T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName) + "\\";

                    if (!Directory.Exists(NewFileName)) Directory.CreateDirectory(NewFileName);

                    NewFileName += ATMisc.GetMonthName1(Month) + "\\";

                    if (!Directory.Exists(NewFileName)) Directory.CreateDirectory(NewFileName);
                }

                NewFileName += ProtokolFileName(Protokol);
            }

            if (CreateNew || !File.Exists(NewFileName))
            {
                var WorkBook = ATMisc.GetGenericExcel("Протокол испытаний тип 4.xls");

                if (WorkBook == null) return false;

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

                var StyleLR_CC = WorkBook.CreateCellStyle();
                StyleLR_CC.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLR_CC.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                StyleLR_CC.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                StyleLR_CC.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
                StyleLR_CC.SetFont(Font);
                StyleLR_CC.WrapText = true;

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

                string UnDeleteSheetName;
                string AddInSheetName = T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.PSG, C.PSG.Name);

                switch (Protokol.SGroup)
                {
                    case data.SGroup.Middle4:

                        UnDeleteSheetName = "Усредненный";
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

                        //ищу метку
                        int TableLocation = -1;
                        int ColumnsCount = 0;
                        for (int i = 0; i < ByPodrSheet.LastRowNum + 1; i++)
                        {
                            var R = ByPodrSheet.GetRow(i);

                            if (R != null)
                            {
                                if (ColumnsCount < R.LastCellNum) ColumnsCount = R.LastCellNum;

                                for (int j = 0; j < R.LastCellNum + 1; j++)
                                {
                                    var Col = R.GetCell(j);
                                    if (Col != null)
                                    {
                                        if (Col.CellType == NPOI.SS.UserModel.CellType.String && Col.StringCellValue.ToLower() == "{таблица}")
                                        {
                                            if (j > 0)
                                            {
                                                MessageBox.Show("В листе \"" + UnDeleteSheetName + "\", положение метки таблицы не находится в первой ячейке", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                                return false;
                                            }
                                            TableLocation = i;

                                            goto FindIt;
                                        }
                                    }
                                }
                            }
                        }

                        MessageBox.Show("В листе \"" + UnDeleteSheetName + "\", не найдена метка таблицы", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return false;

                    FindIt: ;

                        var SGrps = new List<List<Protokols_class.SGroup_class.Protokol_class.Sample_class>>();
                        SGrps.Add(new List<Protokols_class.SGroup_class.Protokol_class.Sample_class>());
                        SGrps[0].Add(Protokol[0]);

                        for (int i = 1; i < Protokol.SampleCount; i++)
                        {
                            for (int j = 0; j < SGrps.Count; j++)
                            {
                                if (SGrps[j][0].OLocationToID == Protokol[i].OLocationToID)
                                {
                                    SGrps[j].Add(Protokol[i]);
                                    goto FindedIt;
                                }
                            }

                            SGrps.Add(new List<Protokols_class.SGroup_class.Protokol_class.Sample_class>());
                            SGrps[SGrps.Count - 1].Add(Protokol[i]);

                        FindedIt: ;
                        }

                        var Grps = new Grp_class[SGrps.Count];

                        for (int i = 0; i < Grps.Length; i++)
                        { Grps[i] = new Grp_class(SGrps[i], ByPodrSheet.GetColumnWidth(4)); }
                        {   //Разбиватель ячеек
                            //Нужно посчитать длины и объединения такого количество ячеек, чтобы их объединения умещались друг под другом как цельное кол-во ячеек
                            //к примеру 1 и 3: первую нужно распилить на три одинаковые по длине
                            /*
                            |__.___._|
                            |__|__|__|
                            2 и 3
                            |__._|_.__|
                            |__|_._|__|
                            при условии ограниченной суммарной длины ячеек и равной длины ячеек каждой группы
                             */
                            //в принципе алгоритм обобщенный, можно задавать их произвольное количество

                            int StartCell = 0;

                            while (true)
                            {
                                int MinWidth = 0;
                                for (int i = 1; i < Grps.Length; i++)
                                {
                                    if (Grps[i].CellWidth(StartCell) < Grps[MinWidth].CellWidth(StartCell))
                                    { MinWidth = i; }
                                }
                                for (int i = 0; i < Grps.Length; i++)
                                    if (Grps[i].CellCount - 1 != StartCell)
                                    { goto TryNext; }
                                break;
                            TryNext: ;

                                for (int i = 0; i < Grps.Length; i++)
                                {
                                    if (i != MinWidth)
                                    { Grps[i].CellSplit(StartCell, Grps[MinWidth].CellWidth(StartCell)); }
                                }

                                StartCell++;
                            }
                        }
                        {
                            //заношу полученные длины колонок

                            for (int i = 0; i < Grps[0].CellCount; i++)
                            { ByPodrSheet.SetColumnWidth(4 + i, Grps[0].CellWidth(i)); }

                            {
                                int LastColumnIndex = 4 + Grps[0].CellCount;

                                for (int i = ByPodrSheet.NumMergedRegions - 1; i > -1; i--)
                                {
                                    var Merged = ByPodrSheet.GetMergedRegion(i);

                                    if (Merged.FirstColumn > 3 && Merged.LastColumn > 3)
                                    {
                                        ByPodrSheet.AddMergedRegion(
                                               new CellRangeAddress(Merged.FirstRow
                                                                  , Merged.LastRow
                                                                  , NewColumnIndex(Merged.FirstColumn, ColumnsCount, LastColumnIndex)
                                                                  , NewColumnIndex(Merged.LastColumn, ColumnsCount, LastColumnIndex)));

                                        ByPodrSheet.RemoveMergedRegion(i);
                                    }
                                    else if (Merged.LastColumn > 3)
                                    {
                                        ByPodrSheet.AddMergedRegion(
                                               new CellRangeAddress(Merged.FirstRow
                                                                  , Merged.LastRow
                                                                  , Merged.FirstColumn
                                                                  , NewColumnIndex(Merged.LastColumn, ColumnsCount, LastColumnIndex)));

                                        ByPodrSheet.RemoveMergedRegion(i);
                                    }
                                }

                                for (int i = 0; i < ByPodrSheet.LastRowNum + 1; i++)
                                {
                                    var R = ByPodrSheet.GetRow(i);

                                    if (R != null)
                                    {
                                        for (int j = R.LastCellNum - 1; j > 3; j--)
                                        {
                                            var Col = R.GetCell(j);

                                            if (Col != null)
                                            {
                                                int NewIndex = NewColumnIndex(Col.ColumnIndex, ColumnsCount, LastColumnIndex);

                                                if (NewIndex != Col.ColumnIndex)
                                                {
                                                    var TCell = R.GetCell(NewIndex);  //баг в библиотеке
                                                    if (TCell == null)
                                                    { TCell = R.CreateCell(NewIndex); }

                                                    R.RemoveCell(TCell);

                                                    R.MoveCell(Col, NewIndex);
                                                }
                                            }
                                        }
                                    }
                                }

                                ByPodrSheet.ShiftRows(TableLocation, ByPodrSheet.LastRowNum, Protokol.MarkCount * SGrps.Count + 3 * SGrps.Count + SGrps.Count - 2);  //количество показателей в таблицах + заголовки и расстояния между таблицами
                            }

                            int StartRowIndex = TableLocation;

                            for (int i = 0; i < Grps.Length; i++)
                            {
                                var Row = ByPodrSheet.CreateRow(StartRowIndex);
                                //Основной заголовок
                                ATMisc.SetValue(Row, "№ п/п", 0, StyleLRT_CC);
                                ByPodrSheet.AddMergedRegion(new CellRangeAddress(StartRowIndex, StartRowIndex + 2, 0, 0));

                                ATMisc.SetValue(Row, "Показатели", 1, StyleLRT_CC);
                                ByPodrSheet.AddMergedRegion(new CellRangeAddress(StartRowIndex, StartRowIndex + 2, 1, 1));

                                ATMisc.SetValue(Row, "Ед.изм.", 2, StyleLRT_CC);
                                ByPodrSheet.AddMergedRegion(new CellRangeAddress(StartRowIndex, StartRowIndex + 2, 2, 2));

                                ATMisc.SetValue(Row, "НД на методику", 3, StyleLRT_CC);
                                ByPodrSheet.AddMergedRegion(new CellRangeAddress(StartRowIndex, StartRowIndex + 2, 3, 3));

                                ATMisc.SetValue(Row, "Результаты испытаний", 4, Grps[i].CellCount + 3, StyleLTD_CC, StyleTD_CC, StyleRTD_CC);

                                Row = ByPodrSheet.CreateRow(++StartRowIndex);
                                ATMisc.SetValue(Row, "", 0, StyleLR_CC);
                                ATMisc.SetValue(Row, "", 1, StyleLR_CC);
                                ATMisc.SetValue(Row, "", 2, StyleLR_CC);
                                ATMisc.SetValue(Row, "", 3, StyleLR_CC);
                                ATMisc.SetValue(Row, Grps[i].OLocationName, 4, Grps[i].CellCount + 3, StyleLTD_CC, StyleTD_CC, StyleRTD_CC);

                                Row = ByPodrSheet.CreateRow(++StartRowIndex);
                                ATMisc.SetValue(Row, "", 0, StyleLR_CC);
                                ATMisc.SetValue(Row, "", 1, StyleLR_CC);
                                ATMisc.SetValue(Row, "", 2, StyleLR_CC);
                                ATMisc.SetValue(Row, "", 3, StyleLR_CC);

                                {//Заголовки замеров
                                    int s = 0;

                                    for (int k = 0; k < Grps[i].SampleCount; k++)
                                    {
                                        int KNum = s;

                                        for (s = KNum + 1; s < Grps[i].CellCount; s++)
                                        {
                                            if (Grps[i].GetSetIn(s))
                                            { break; }
                                        }

                                        if (KNum < s - 1)
                                        { ATMisc.SetValue(Row, "№ " + Grps[i].GetSample(k).Number.ToString(), 4 + KNum, 3 + s, StyleLTD_CC, StyleTD_CC, StyleRTD_CC); }
                                        else
                                        { ATMisc.SetValue(Row, "№ " + Grps[i].GetSample(k).Number.ToString(), 4 + KNum, StyleLRTD_CC); }
                                    }
                                }
                                int MarkNumber = 0;

                                for (int j = 0; j < Protokol.TotalMarkCount; j++)
                                {
                                    if (Protokol.GetTotalAlowedMark(j))
                                    {
                                        MarkNumber++;
                                        Row = ByPodrSheet.CreateRow(++StartRowIndex);
                                        int s = 0;

                                        for (int k = 0; k < Grps[i].SampleCount; k++)
                                        {
                                            //заношу данные показателя
                                            ATMisc.SetValue(Row, MarkNumber, 0, StyleLRTD_CC);
                                            ATMisc.SetValue(Row, Protokol.GetTotalMarkName(j), 1, StyleLRTD_CC);
                                            ATMisc.SetValue(Row, Protokol.GetTotalMarkEdType(j), 2, StyleLRTD_CC);
                                            ATMisc.SetValue(Row, RCache.Marks[Protokol.GetTotalMarkID(j)].GetNorm(Grps[i].GetSample(k).NormID).MethodName, 3, StyleLRTD_CC);

                                            //заношу данные концентраций
                                            int KNum = s;

                                            for (s = KNum + 1; s < Grps[i].CellCount; s++)
                                            {
                                                if (Grps[i].GetSetIn(s))
                                                { break; }
                                            }

                                            if (KNum < s - 1)
                                            { ATMisc.SetValue(Row, Grps[i].GetSample(k)[j].Amount, 4 + KNum, 3 + s, StyleLTD_CC, StyleTD_CC, StyleRTD_CC); }
                                            else
                                            { ATMisc.SetValue(Row, Grps[i].GetSample(k)[j].Amount, 4 + KNum, StyleLRTD_CC); }
                                        }
                                    }
                                }

                                StartRowIndex += 2;
                            }
                        }

                        break;
                    default: throw new Exception("Неизвестный тип протокола");
                }

                for (int i = 0; i < WorkBook.NumberOfSheets; i++)
                {
                    if (WorkBook.GetSheetAt(i).SheetName.ToLower() != "заголовок" && WorkBook.GetSheetAt(i).SheetName.ToLower() != "концентрации")
                    {
                        WorkBook.RemoveSheetAt(i);
                        i--;
                    }
                }

                var DT = Protokol.Date;

                GetProtokolsExchanges(TitleSheet
                                    , ATMisc.GetDateTimeFromYM(Employe_Form.SPoints.YM).Year
                                    , Protokol.Number.ToString() + "-" + T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName) + " - " + DT.Month.ToString() + "/" + DT.Year.ToString()
                                    , Protokol.Objects
                                    , Protokol.Objects
                                    , Protokol.PTypes
                                    , Protokol.ObjectsLocations
                                    , Protokol.DateOstr
                                    , Protokol.DateP
                                    , Protokol.StrTime
                                    , Protokol.Peoples
                                    , Protokol.Causes
                                    , Protokol.Numbers
                                    , DT.Day.ToString()
                                    , ATMisc.GetMonthName2(DT.Month)
                                    , DT.Month.ToString()
                                    , RCache.PSG.GetMethodName(Protokol.PodrID)
                                    , T.PaPoS.Rows.Get<string>(Protokol.PaPoSID, C.PaPoS.Name)
                                    , T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.FllName)
                                    , T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName)
                                    , T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.Contact)
                                    , 0).Exchange(0, 25, 0, 25);
                {
                    var Exchanges = new CellExchange_Class(TitleSheet);

                    Exchanges.ClearExchanges();

                    Exchanges.AddColumn("{имя свойства}");
                    Exchanges.AddColumn("{ед. свойства}");
                    Exchanges.AddColumn("{значение свойства}");
                }

                {
                    var Exchanges = new CellExchange_Class(ByPodrSheet);

                    Exchanges.AddExchange("{должность ответственного}", T.People.Rows.Get<string>(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Protokol[0].PodrID, C.Podr.PSG)), C.People.Prfssn, C.Prfssn.Name), 5);
                    Exchanges.AddExchange("{ФИО ответственного}", Misc.GetShortFIO(RCache.PSG.GetPeopleID((data.PSG)T.Podr.Rows.Get_UnShow<uint>(Protokol[0].PodrID, C.Podr.PSG))), 5);
                    Exchanges.AddExchange("{Номер протокола}", Protokol.Number.ToString() + "-" + T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName) + " - " + DT.Month.ToString() + "/" + DT.Year.ToString(), 5);
                    Exchanges.AddExchange("{Дата}", DT.ToShortDateString(), 5);

                    SetResp(Exchanges, Protokol.PodrID, data.TResp.LaboratoryProtokol);
                }

                return SaveExcel(WorkBook, NewFileName, Open);
            }
            else
            {
                if (Open) System.Diagnostics.Process.Start(NewFileName);

                return true;
            }
        }
    }
}