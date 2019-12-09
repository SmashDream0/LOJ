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
        public static bool OtchProtokolType1(Protokols_class.SGroup_class.Protokol_class Protokol, bool CreateNew = true, bool Open = true)
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

                ATMisc.GetYearMonthFromYM(Protokol.Parent.YM, out Year, out Month);

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
                var WorkBook = ATMisc.GetGenericExcel("Протокол испытаний тип 1.xls");

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

                string UnDeleteSheetName;
                var AddInSheetName = T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.PSG, C.PSG.Name);
                var NormID = Protokol[0].NormID;

                var ObjectsLocations = Protokol.ObjectsLocations;

                switch (Protokol.SGroup)
                {
                    case data.SGroup.Middle1:

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

                        var Exchange = new CellExchange_Class(ByPodrSheet);
                        {
                            var NumC = new ATMisc.CellMark_struct("{номер}");
                            var NameC = new ATMisc.CellMark_struct("{имя точки отбора}");
                            var MarkC = new ATMisc.CellMark_struct("{показатели}");
                            var SMC = new ATMisc.CellMark_struct("{концентрации}");

                            Exchange.AddExchange(NumC.Mark, (Cell) => { NumC.ColumnIndex = Cell.ColumnIndex; NumC.RowIndex = Cell.RowIndex; }, 1);
                            Exchange.AddExchange(NameC.Mark, (Cell) => { NameC.ColumnIndex = Cell.ColumnIndex; NameC.RowIndex = Cell.RowIndex; }, 1);
                            Exchange.AddExchange(MarkC.Mark, (Cell) => { MarkC.ColumnIndex = Cell.ColumnIndex; MarkC.RowIndex = Cell.RowIndex; }, 1);
                            Exchange.AddExchange(SMC.Mark, (Cell) => { SMC.ColumnIndex = Cell.ColumnIndex; SMC.RowIndex = Cell.RowIndex; }, 1);

                            Exchange.Exchange();

                            if (!NumC.Check() || !NameC.Check() || !MarkC.Check() || !SMC.Check())
                            { return false; }

                            if (MarkC.ColumnIndex != SMC.ColumnIndex)
                            {
                                MessageBox.Show("Метки показатели и концентрации должны распологаться в одной колонке", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }

                            if (MarkC.RowIndex > SMC.RowIndex)
                            {
                                MessageBox.Show("Метка показателей добжна распологаться выше метки концентраций", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }

                            if (NumC.RowIndex != NameC.RowIndex || NameC.RowIndex != SMC.RowIndex)
                            {
                                MessageBox.Show("Метки номер, имя точки отбора и концентрации, должны распологаться на одной строке", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return false;
                            }

                            if (Protokol.MarkCount > 1)
                            {
                                var MRow = ByPodrSheet.GetRow(MarkC.RowIndex);
                                var SMRow = ByPodrSheet.GetRow(SMC.RowIndex);

                                var MStyle = MRow.GetCell(MarkC.ColumnIndex).CellStyle;
                                var SMStyle = SMRow.GetCell(SMC.ColumnIndex).CellStyle;

                                ATMisc.BreakColumn(ByPodrSheet, MarkC.ColumnIndex, Protokol.MarkCount);

                                for (int i = 0; i < ByPodrSheet.NumMergedRegions; i++)
                                {
                                    var mrg = ByPodrSheet.GetMergedRegion(i);

                                    if ((mrg.FirstRow <= SMC.RowIndex && mrg.LastRow >= SMC.RowIndex
                                        || (mrg.FirstRow <= MarkC.RowIndex && mrg.LastRow >= MarkC.RowIndex)) &&
                                        mrg.FirstColumn <= MarkC.ColumnIndex && mrg.LastColumn >= MarkC.ColumnIndex)
                                    {
                                        ByPodrSheet.RemoveMergedRegion(i);
                                        i--;
                                    }
                                }


                                for (int i = 0; i < Protokol.MarkCount; i++)
                                {
                                    {
                                        var Cell = MRow.GetCell(MarkC.ColumnIndex + i);

                                        if (Cell == null)
                                        { Cell = MRow.CreateCell(MarkC.ColumnIndex + i); }

                                        Cell.CellStyle = MStyle;
                                    }
                                    {
                                        var Cell = SMRow.GetCell(SMC.ColumnIndex + i);

                                        if (Cell == null)
                                        { Cell = SMRow.CreateCell(SMC.ColumnIndex + i); }

                                        Cell.CellStyle = MStyle;
                                    }
                                }
                            }

                            Exchange.AddColumn(NumC.RowIndex, NumC.ColumnIndex);
                            Exchange.AddColumn(NameC.RowIndex, NameC.ColumnIndex);

                            var Row = ByPodrSheet.GetRow(MarkC.RowIndex);

                            for (int i = 0; i < Protokol.MarkCount; i++)
                            {
                                Row.GetCell(MarkC.ColumnIndex + i).SetCellValue(Protokol.GetMarkName(i) + ", " + Protokol.GetMarkEdType(i));

                                Exchange.AddColumn(SMC.RowIndex, SMC.ColumnIndex + i);
                            }

                            Exchange.CheckTableColumns();
                        }

                        for (int i = 0; i < Protokol.TableCount; i++)
                        {
                            var Values = new object[2 + Protokol.MarkCount];

                            Values[0] = i + 1;
                            Values[1] = Protokol.GetTableName(i);

                            for (int j = 0; j < Protokol.MarkCount; j++)
                            {
                                if (Protokol.IsSpetialOut(j, i, false))
                                { Values[2 + j] = Protokol.GetSpetialOut(j, i); }
                                else
                                { Values[2 + j] = Protokol.GetMarkAmount(j, i); }
                            }

                            Exchange.SetRow(Values);
                        }
                        break;
                    case data.SGroup.NotGroup1:
                        {
                            ObjectsLocations = Protokol[0].SPointName;

                            UnDeleteSheetName = "Изначальный";
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

                            var OEdTypeIndex = new SColumn_struct(-1, null);
                            var OMethodIndex = new SColumn_struct(-1, null);
                            var OMarkNameIndex = new SColumn_struct(-1, null);
                            var NumberIndex = new SColumn_struct(-1, null);
                            var ResultIndex = new SColumn_struct(-1, null);
                            var LimitIndex = new SColumn_struct(-1, null);

                            int RowIndex = -1;
                            {
                                var OEdTypeRowIndex = -1;
                                var OMethodRowIndex = -1;
                                var OMarkNameRowIndex = -1;
                                var NumberRowIndex = -1;
                                var ResultRowIndex = -1;
                                var LimitRowIndex = -1;

                                var ExistColumn = new CellExchange_Class(ByPodrSheet);

                                ExistColumn.AddExchange("{Метка пробы}", "№ " + T.Sample.Rows.Get<string>(Protokol[0].SampleID, C.Sample.Number), 5);
                                if (NormID > 0)
                                { ExistColumn.AddExchange("{Норматив}", T.Norm.Rows.Get<string>(NormID, C.Norm.Name), 5); }
                                ExistColumn.AddExchange("{номер п/п}", (Cell) =>
                                {
                                    NumberRowIndex = Cell.RowIndex;
                                    NumberIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{показатель}", (Cell) =>
                                {
                                    OMarkNameRowIndex = Cell.RowIndex;
                                    OMarkNameIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{ед.изм.}", (Cell) =>
                                {
                                    OEdTypeRowIndex = Cell.RowIndex;
                                    OEdTypeIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{методика}", (Cell) =>
                                {
                                    OMethodRowIndex = Cell.RowIndex;
                                    OMethodIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{результат}", (Cell) =>
                                {
                                    ResultRowIndex = Cell.RowIndex;
                                    ResultIndex = new SColumn_struct(Cell);

                                    CopyStyleFromCell(ExpStyle, Cell);
                                }, 5);
                                ExistColumn.AddExchange("{лимит}", (Cell) =>
                                {
                                    LimitRowIndex = Cell.RowIndex;
                                    LimitIndex = new SColumn_struct(Cell);
                                }, 5);


                                ExistColumn.Exchange(0, 25, 0, 25);

                                if (NumberRowIndex == -1 || OMarkNameRowIndex == -1 || OEdTypeRowIndex == -1 || OMethodRowIndex == -1 || ResultRowIndex == -1)
                                {
                                    MessageBox.Show("Не все табличные метки найдены.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }

                                if (LimitRowIndex > -1 && LimitRowIndex != NumberRowIndex || NumberRowIndex != OMarkNameRowIndex || OMarkNameRowIndex != OEdTypeRowIndex || OEdTypeRowIndex != OMethodRowIndex || OMethodRowIndex != ResultRowIndex)
                                {
                                    MessageBox.Show("Все табличные метки должны распологаться в одной строке.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }

                                if (NormID == 0 && LimitRowIndex > -1)
                                {
                                    ByPodrSheet.SetColumnWidth(ResultIndex.Index, ByPodrSheet.GetColumnWidth(ResultIndex.Index) + ByPodrSheet.GetColumnWidth(LimitIndex.Index));
                                    ByPodrSheet.SetColumnHidden(LimitIndex.Index, true);
                                }

                                RowIndex = NumberRowIndex;
                            }

                            ByPodrSheet.ShiftRows(RowIndex, ByPodrSheet.LastRowNum, Protokol.MarkCount - 1);

                            int ONumber = 0;

                            for (int i = 0; i < Protokol.MarkCount; i++)
                            {
                                var Row = ByPodrSheet.CreateRow(RowIndex++);

                                if (RowIndex > -1)
                                {
                                    ATMisc.SetValue(Row, ++ONumber, NumberIndex.Index, NumberIndex.Style);
                                }

                                if (LimitIndex.Index > -1)
                                {
                                    var Norm = RCache.Marks[Protokol.GetMarkID(i)].GetNorm(Protokol[0].NormID);

                                    switch (Norm.NType)
                                    {
                                        case data.NType.Mark:
                                            ATMisc.SetValue(Row, Norm.Range.Range, LimitIndex.Index, LimitIndex.Style);
                                            break;
                                        case data.NType.PodrV:
                                        case data.NType.PodrK:
                                        case data.NType.PodrAll:
                                            var PIndex = RCache.Marks.Norms.GetPodrIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Protokol[0].SPointID, C.SPoint.Podr));
                                            ATMisc.SetValue(Row, Norm.Station(PIndex).Range, LimitIndex.Index, LimitIndex.Style);
                                            break;
                                        case data.NType.Volume:
                                            var VIndex = RCache.Marks.Norms.GetVolumeIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Protokol[0].SPointID, C.SPoint.Object, C.Object.OLocationFrom));
                                            ATMisc.SetValue(Row, Norm.Volume(VIndex).Range, LimitIndex.Index, LimitIndex.Style);
                                            break;
                                    }
                                }

                                if (Protokol.IsSpetialOut(i))
                                { ATMisc.SetValue(Row, Protokol.GetSpetialOut(i), ResultIndex.Index, ResultIndex.Style); }
                                else
                                { ATMisc.SetValue(Row, Protokol.GetMarkAmount(i), ResultIndex.Index, ResultIndex.Style); }

                                ATMisc.SetValue(Row, Protokol.GetMethod(i), OMethodIndex.Index, OMethodIndex.Style);
                                ATMisc.SetValue(Row, Protokol.GetMarkName(i), OMarkNameIndex.Index, OMarkNameIndex.Style);
                                ATMisc.SetValue(Row, Protokol.GetMarkEdType(i), OEdTypeIndex.Index, OEdTypeIndex.Style);
                            }
                        }
                        break;
                    case data.SGroup.Group1:
                        {
                            UnDeleteSheetName = "Изначальный";
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

                            var OEdTypeIndex = new SColumn_struct(-1, null);
                            var OMethodIndex = new SColumn_struct(-1, null);
                            var OMarkNameIndex = new SColumn_struct(-1, null);
                            var NumberIndex = new SColumn_struct(-1, null);
                            var ResultIndex = new SColumn_struct(-1, null);
                            var ProbeIndex = new SColumn_struct(-1, null);
                            var LimitIndex = new SColumn_struct(-1, null);
                            int ProbeRowIndex = -1;
                            int RowIndex = -1;
                            {
                                var OEdTypeRowIndex = -1;
                                var OMethodRowIndex = -1;
                                var OMarkNameRowIndex = -1;
                                var NumberRowIndex = -1;
                                var ResultRowIndex = -1;
                                var LimitRowIndex = -1;

                                var ExistColumn = new CellExchange_Class(ByPodrSheet);

                                if (NormID > 0)
                                { ExistColumn.AddExchange("{Норматив}", T.Norm.Rows.Get<string>(NormID, C.Norm.Name), 5); }
                                ExistColumn.AddExchange("{Метка пробы}", (Cell) =>
                                {
                                    ProbeRowIndex = Cell.RowIndex;
                                    ProbeIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{номер п/п}", (Cell) =>
                                {
                                    NumberRowIndex = Cell.RowIndex;
                                    NumberIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{показатель}", (Cell) =>
                                {
                                    OMarkNameRowIndex = Cell.RowIndex;
                                    OMarkNameIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{ед.изм.}", (Cell) =>
                                {
                                    OEdTypeRowIndex = Cell.RowIndex;
                                    OEdTypeIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{методика}", (Cell) =>
                                {
                                    OMethodRowIndex = Cell.RowIndex;
                                    OMethodIndex = new SColumn_struct(Cell);
                                }, 5);
                                ExistColumn.AddExchange("{результат}", (Cell) =>
                                {
                                    ResultRowIndex = Cell.RowIndex;
                                    ResultIndex = new SColumn_struct(Cell);

                                    CopyStyleFromCell(ExpStyle, Cell);

                                    ExpStyle.DataFormat = WorkBook.CreateDataFormat().GetFormat("0.00E+00");

                                }, 5);
                                ExistColumn.AddExchange("{лимит}", (Cell) =>
                                {
                                    LimitRowIndex = Cell.RowIndex;
                                    LimitIndex = new SColumn_struct(Cell);
                                }, 5);

                                ExistColumn.Exchange(0, 25, 0, 25);

                                if (ProbeRowIndex == -1 || NumberRowIndex == -1 || OMarkNameRowIndex == -1 || OEdTypeRowIndex == -1 || OMethodRowIndex == -1 || ResultRowIndex == -1)
                                {
                                    MessageBox.Show("Не все табличные метки найдены.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }

                                if (LimitRowIndex > -1 && LimitRowIndex != NumberRowIndex || NumberRowIndex != OMarkNameRowIndex || OMarkNameRowIndex != OEdTypeRowIndex || OEdTypeRowIndex != OMethodRowIndex || OMethodRowIndex != ResultRowIndex)
                                {
                                    MessageBox.Show("Все табличные метки должны распологаться в одной строке.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    return false;
                                }

                                if (LimitRowIndex > -1)
                                {
                                    ByPodrSheet.SetColumnWidth(ResultIndex.Index, ByPodrSheet.GetColumnWidth(ResultIndex.Index) + ByPodrSheet.GetColumnWidth(LimitIndex.Index));
                                    ByPodrSheet.SetColumnHidden(LimitIndex.Index, true);
                                }

                                RowIndex = NumberRowIndex;
                            }

                            ByPodrSheet.ShiftRows(RowIndex, ByPodrSheet.LastRowNum, Protokol.MarkCount - 1);

                            int Width = ByPodrSheet.GetColumnWidth(ResultIndex.Index) / Protokol.SampleCount;
                            ByPodrSheet.AddMergedRegion(new CellRangeAddress(RowIndex - 2, RowIndex - 2, ResultIndex.Index, ResultIndex.Index + Protokol.SampleCount - 1));

                            {
                                var Row = ByPodrSheet.GetRow(ProbeRowIndex);

                                //проверяю одинаковые ли типы воды, если да, тогда проверяю одинаковые ли места, если нет то пишу места, если да, то пишу тип

                                for (int j = 1; j < Protokol.SampleCount; j++)
                                {
                                    if (T.Object.Rows.Get_UnShow<uint>(Protokol[j].ObjectID, C.Object.OType) != T.Object.Rows.Get_UnShow<uint>(Protokol[0].ObjectID, C.Object.OType))
                                    { goto UseOType; }
                                }

                                for (int j = 1; j < Protokol.SampleCount; j++)
                                {
                                    if (T.Object.Rows.Get_UnShow<uint>(Protokol[j].ObjectID, C.Object.OLocationTo) == T.Object.Rows.Get_UnShow<uint>(Protokol[0].ObjectID, C.Object.OLocationTo))
                                    { goto UseOType; }
                                }

                                for (int j = 0; j < Protokol.SampleCount; j++)
                                {
                                    ByPodrSheet.SetColumnWidth(ResultIndex.Index + j, Width);
                                    ATMisc.SetValue(Row, T.Object.Rows.Get<string>(Protokol[j].ObjectID, C.Object.OLocationTo, C.OLocation.Name), ProbeIndex.Index + j, ProbeIndex.Style);
                                }

                                goto IFilled;

                            UseOType: ;

                                for (int j = 0; j < Protokol.SampleCount; j++)
                                {
                                    ByPodrSheet.SetColumnWidth(ResultIndex.Index + j, Width);
                                    ATMisc.SetValue(Row, T.Object.Rows.Get<string>(Protokol[j].ObjectID, C.Object.OType, C.OType.Name) + " №" + Protokol[j].Number.ToString(), ProbeIndex.Index + j, ProbeIndex.Style);
                                }

                            IFilled: ;

                                {   //костыльности вот прям тут. т.к. не особенно общая вещь делается: клепается объединение ячеек там где сейчас "результаты испытаний"
                                    var PrevRow = ByPodrSheet.GetRow(RowIndex - 2);

                                    PrevRow.GetCell(ResultIndex.Index).CellStyle = StyleLTD_CC;

                                    for (int j = Protokol.SampleCount - 2; j > 0; j--)
                                    { PrevRow.CreateCell(ResultIndex.Index + j).CellStyle = StyleTD_CC; }

                                    PrevRow.CreateCell(ResultIndex.Index + Protokol.SampleCount - 1).CellStyle = StyleRTD_CC;
                                }

                                int MergeCount = ByPodrSheet.NumMergedRegions;
                                for (int i = 0; i < MergeCount; i++)
                                {
                                    var Merge = ByPodrSheet.GetMergedRegion(i);

                                    if (Merge.LastColumn == ResultIndex.Index)
                                    {
                                        ByPodrSheet.RemoveMergedRegion(i);

                                        MergeCount--; i--;

                                        ByPodrSheet.AddMergedRegion(new CellRangeAddress(Merge.FirstRow, Merge.LastRow, Merge.FirstColumn, ResultIndex.Index + Protokol.SampleCount - 1));
                                    }
                                }
                            }

                            int TNumber = 0;

                            for (int i = 0; i < Protokol.MarkCount; i++)
                            {
                                var Row = ByPodrSheet.CreateRow(RowIndex++);

                                if (LimitIndex.Index > -1)
                                {
                                    var Norm = RCache.Marks[Protokol.GetMarkID(i)].GetNorm(Protokol.GetNormID(i));

                                    switch (Norm.NType)
                                    {
                                        case data.NType.Mark:
                                            ATMisc.SetValue(Row, Norm.Range.Range, LimitIndex.Index, LimitIndex.Style);
                                            break;
                                        case data.NType.PodrV:
                                        case data.NType.PodrK:
                                        case data.NType.PodrAll:
                                            var PIndex = RCache.Marks.Norms.GetPodrIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Protokol[0].SPointID, C.SPoint.Podr));
                                            ATMisc.SetValue(Row, Norm.Station(PIndex).Range, LimitIndex.Index, LimitIndex.Style);
                                            break;
                                        case data.NType.Volume:
                                            var VIndex = RCache.Marks.Norms.GetVolumeIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Protokol[0].SPointID, C.SPoint.Object, C.Object.OLocationTo));
                                            ATMisc.SetValue(Row, Norm.Volume(VIndex).Range, LimitIndex.Index, LimitIndex.Style);
                                            break;
                                    }

                                    for (int j = 0; j < Protokol.TableCount; j++)
                                    {
                                        if (Protokol.IsSpetialOut(i, j, true))
                                        { ATMisc.SetValue(Row, Protokol.GetSpetialOut(i, j), ResultIndex.Index + j, ResultIndex.Style); }
                                        else
                                        { ATMisc.SetValue(Row, Protokol.GetMarkAmount(i, j), ResultIndex.Index + j, ResultIndex.Style); }
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < Protokol.TableCount; j++)
                                    {
                                        if (Protokol.IsSpetialOut(i, j))
                                        { ATMisc.SetValue(Row, Protokol.GetSpetialOut(i, j), ResultIndex.Index + j, ResultIndex.Style); }
                                        else
                                        { ATMisc.SetValue(Row, Protokol.GetMarkAmount(i, j), ResultIndex.Index + j, ResultIndex.Style); }
                                    }
                                }


                                ATMisc.SetValue(Row, ++TNumber, NumberIndex.Index, NumberIndex.Style);
                                ATMisc.SetValue(Row, Protokol.GetMethod(i), OMethodIndex.Index, OMethodIndex.Style);
                                ATMisc.SetValue(Row, Protokol.GetMarkName(i), OMarkNameIndex.Index, OMarkNameIndex.Style);
                                ATMisc.SetValue(Row, Protokol.GetMarkEdType(i), OEdTypeIndex.Index, OEdTypeIndex.Style);
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

                string DateO;
                string DateP;
                var DTMin = T.Sample.Rows.Get<int>(Protokol[0].SampleID, C.Sample.CYMD);
                for (int i = 1; i < Protokol.SampleCount; i++)
                {
                    if (DTMin > T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.CYMD))
                    { DTMin = T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.CYMD); }
                }

                var DTMax = T.Sample.Rows.Get<int>(Protokol[0].SampleID, C.Sample.CYMD);
                for (int i = 1; i < Protokol.SampleCount; i++)
                {
                    if (DTMax < T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.CYMD))
                    { DTMax = T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.CYMD); }
                }
                if (DTMax == DTMin)
                { DateO = ATMisc.GetDateTime(DTMax).ToShortDateString(); }
                else
                { DateO = ATMisc.GetDateTime(DTMin).ToShortDateString() + " - " + ATMisc.GetDateTime(DTMax).ToShortDateString(); }

                DTMin = T.Sample.Rows.Get<int>(Protokol[0].SampleID, C.Sample.AYMD);
                for (int i = 1; i < Protokol.SampleCount; i++)
                {
                    if (DTMin > T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.AYMD))
                    { DTMin = T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.AYMD); }
                }

                DTMax = T.Sample.Rows.Get<int>(Protokol[0].SampleID, C.Sample.AYMD);
                for (int i = 1; i < Protokol.SampleCount; i++)
                {
                    if (DTMax < T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.AYMD))
                    { DTMax = T.Sample.Rows.Get<int>(Protokol[i].SampleID, C.Sample.AYMD); }
                }
                if (DTMax == DTMin)
                { DateP = ATMisc.GetDateTime(DTMax).ToShortDateString(); }
                else
                { DateP = ATMisc.GetDateTime(DTMin).ToShortDateString() + " - " + ATMisc.GetDateTime(DTMax).ToShortDateString(); }

                var DT = Protokol.Date;

                GetProtokolsExchanges(TitleSheet
                                    , ATMisc.GetDateTimeFromYM(Protokol.Parent.YM).Year
                                    , Protokol.Number.ToString() + "-" + T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.ShrName) + " - " + DT.Month.ToString() + "/" + DT.Year.ToString()
                                    , Protokol.Objects
                                    , Protokol.Objects
                                    , Protokol.PTypes
                                    , ObjectsLocations
                                    , DateO
                                    , DateP
                                    , Protokol.StrTime
                                    , PeopleNames
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
                                    , Protokol.Act).Exchange(0, 25, 0, 25);
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