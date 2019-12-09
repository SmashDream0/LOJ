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
        public static bool OtchProtokolAquaAurat_KOCA(Protokols_class.SGroup_class.Protokol_class Protokol, bool CreateNew = true, bool Open = true)
        {
            if (Protokol.SampleCount != 1)
            {
                MessageBox.Show("Не верное количество замеров в протоколе:" + Protokol.SampleCount.ToString() + ". Должен быть 1", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

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

                var Objects = Protokol[0].SPointName + ", " + Protokol.Objects;
                var ProbeType = T.PType.Rows.Get<string>(Protokol[0].PTypeID, C.PType.Name);
                var Location = Protokol.ObjectsLocations;

                var Cause = Protokol.SCause;
                string UnDeleteSheetName;
                var AddInSheetName = T.Podr.Rows.Get<string>(Protokol.PodrID, C.Podr.PSG, C.PSG.Name);

                //Location = T.Object.Rows.Get<string>(Protokol[0].ObjectID, C.Object.OLocationFrom, C.OLocation.Name);
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
                    if (NormID > 0) ExistColumn.AddExchange("{Норматив}", T.Norm.Rows.Get<string>(NormID, C.Norm.Name), 5);
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
                    { ATMisc.SetValue(Row, ++ONumber, NumberIndex.Index, NumberIndex.Style); }

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
                                var VIndex = RCache.Marks.Norms.GetVolumeIndex(Norm.NormID, T.SPoint.Rows.Get_UnShow<uint>(Protokol[0].SPointID, C.SPoint.Object, C.Object.OLocationFrom));
                                ATMisc.SetValue(Row, Norm.Volume(VIndex).Range, LimitIndex.Index, LimitIndex.Style);
                                break;
                        }

                        ATMisc.SetValue(Row, Protokol.GetMarkAmount(i), ResultIndex.Index, ResultIndex.Style);
                    }
                    else
                    {
                        if (Protokol.IsSpetialOut(i))
                        { ATMisc.SetValue(Row, Protokol.GetSpetialOut(i), ResultIndex.Index, ResultIndex.Style); }
                        else
                        { ATMisc.SetValue(Row, Protokol.GetMarkAmount(i), ResultIndex.Index, ResultIndex.Style); }
                    }

                    ATMisc.SetValue(Row, Protokol.GetMethod(i), OMethodIndex.Index, OMethodIndex.Style);
                    ATMisc.SetValue(Row, Protokol.GetMarkName(i), OMarkNameIndex.Index, OMarkNameIndex.Style);
                    ATMisc.SetValue(Row, Protokol.GetMarkEdType(i), OEdTypeIndex.Index, OEdTypeIndex.Style);

                    //Вывожу концентрацию как есть
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

                DateTime DT;
                {
                    int Year, Month;

                    ATMisc.GetYearMonthFromYM(Protokol.YM, out Year, out Month);

                    DT = new DateTime(Year, Month, Protokol.Day);
                }

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
                    Exchanges.AddExchange("{Номер протокола}", ProtokolNum, 5);
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

        enum EMiddleCol
        { Number, Name, NextCol }
    }
}