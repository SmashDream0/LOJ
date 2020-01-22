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
        delegate void AddCols_delegate(DataBase.ITable Table);

        /// <summary>Номер версии изменений</summary>
        public const int Number = 4;
        /// <summary>менять нельзя</summary>
        public static readonly byte[] smth = { 48, 86, 3, 24, 125, 123, 55, 4, 5, 8, 89, 84, 89, 8, 23, 32, 12, 10, 32, 158, 250, 1, 2, 15, 0, 0, 5, 18, 51, 81, 68, 8, 97, 87, 8, 9, 78, 78, 78, 42, 1, 25, 4, 54, 8, 4, 56, 5, 24, 5, 45, 4, 8, 4, 255, 2 };
        /// <summary>менять нельзя</summary>
        public const int pass = 2048;
        /// <summary>
        /// Проверять ли наличие таблицы, при её добавлении
        /// </summary>
        public static bool CheckTablesExist = true;

        public static byte Round { get { return data.User<byte>(C.User.Round); } }
        /// <summary>Получить краткое фио сотрудника, с его профессией</summary>
        /// <param name="PeopleID">Уникальный идентификатор сотрудника</param>
        public static string GetPWP(uint PeopleID)
        { return T.People.Rows.Get<string>(PeopleID, C.People.Prfssn, C.Prfssn.Name) + ' ' + GetShortFIO(PeopleID); }
        public static string GetShortFIO(DataBase.ITable Table, uint ID, params int[] ColIndex)
        {
            string temp = Table.Rows.Get<string>(ID, ColIndex[0]) + ' '
                 , temp1;

            for (int i = 1; i < ColIndex.Length; i++)
            {
                temp1 = Table.Rows.Get<string>(ID, ColIndex[i]);

                if (temp1.Length == 0)
                { temp += '.'; }
                else
                { temp += temp1.Substring(0, 1).ToUpper() + '.'; }
            }

            return temp;
        }
        public static string GetShortFIO(uint PeopleID)
        {
            return GetShortFIO(T.People, PeopleID, C.People.name1, C.People.name2, C.People.name3);
        }

        public static bool SaveExcel(NPOI.SS.UserModel.IWorkbook WorkBook, string FileName, bool OpenAfterSave, bool ShowErrorMessage = true)
        {
            try
            {
                using (var fs = new System.IO.FileStream(FileName, FileMode.Create, System.IO.FileAccess.Write))
                { WorkBook.Write(fs); }
            }
            catch (Exception ex)
            {
                if (ShowErrorMessage) MessageBox.Show("При попытке сохранения файла печатной формы возникла ошибка:\n" + ex.ToString(), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }

            if (OpenAfterSave)
            { System.Diagnostics.Process.Start(FileName); }
            return true;
        }

        /// <summary>Получить ответственных по типу ответственности для подразделения</summary>
        static void SetResp(CellExchange_Class Exchange, uint PodrID, data.TResp RespType)
        {
            var Prffn = new SColumn_struct(-1, null);
            var FIO = new SColumn_struct(-1, null);
            int RowIndex;

            {
                int PrffnRowIndex = -1;
                int FIORowIndex = -1;

                Exchange.AddExchange("{Профессии ответственных по подразделению}", Cell =>
                {
                    Prffn = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    PrffnRowIndex = Cell.RowIndex;
                }, 5);
                Exchange.AddExchange("{ФИО ответственных по подразделению}", Cell =>
                {
                    FIO = new SColumn_struct(Cell.ColumnIndex, Cell.CellStyle);
                    FIORowIndex = Cell.RowIndex;
                }, 5);
                Exchange.Exchange(0, 50, 0, Exchange.sheet.LastRowNum + 1);

                if (PrffnRowIndex == FIORowIndex && FIORowIndex < 0)
                {
                    return;
                }

                if (PrffnRowIndex > -1 && FIORowIndex > -1 && FIORowIndex != PrffnRowIndex)
                {
                    return;
                }

                RowIndex = PrffnRowIndex;
            }

            CellRangeAddress FIOMerge = null;
            CellRangeAddress PrffnMerge = null;

            G.Resp.QUERRY()
                .SHOW.WHERE
                    .ARC(C.Resp.PodrPpl, C.PodrPpl.Podr).EQUI.BV(PodrID)
                    .AND.C(C.Resp.TResp, (uint)RespType)
                .DO();

            if (G.Resp.Rows.Count > 0)
            {
                for (int i = 0; i < Exchange.sheet.NumMergedRegions; i++)
                {
                    var Merge = Exchange.sheet.GetMergedRegion(i);
                    if (Merge.FirstRow <= RowIndex && Merge.LastRow >= RowIndex)
                    {
                        if (Merge.FirstColumn <= FIO.Index && Merge.LastColumn >= FIO.Index)
                        { FIOMerge = Merge; }
                        else if (Merge.FirstColumn <= Prffn.Index && Merge.LastColumn >= Prffn.Index)
                        { PrffnMerge = Merge; }
                    }
                }

                Exchange.sheet.ShiftRows(RowIndex + 1, Exchange.sheet.LastRowNum, G.Resp.Rows.Count - 1);

                G.Resp.Sort((it1, it2) =>
                    {
                        var ret1 = T.Resp.Rows.Get_UnShow<uint>(it1.ID, C.Resp.PodrPpl, C.PodrPpl.People, C.People.Prfssn, C.Prfssn.PnMean)
                            .CompareTo(T.Resp.Rows.Get_UnShow<uint>(it2.ID, C.Resp.PodrPpl, C.PodrPpl.People, C.People.Prfssn, C.Prfssn.PnMean));

                        return (ret1 == 0 ? it1.ID.CompareTo(it2.ID) : ret1);
                    });

                if (Prffn.Index > -1)
                {
                    var Row = Exchange.sheet.GetRow(RowIndex);
                    var Cell = Row.GetCell(Prffn.Index);

                    Cell.SetCellValue(G.Resp.Rows.Get<string>(0, C.Resp.PodrPpl, C.PodrPpl.People, C.People.Prfssn, C.Prfssn.Name));
                }

                if (FIO.Index > -1)
                {
                    var Row = Exchange.sheet.GetRow(RowIndex);
                    var Cell = Row.GetCell(FIO.Index);

                    Cell.SetCellValue(Misc.GetShortFIO(G.Resp.Rows.Get_UnShow<uint>(0, C.Resp.PodrPpl, C.PodrPpl.People)));
                }

                for (int i = 1; i < G.Resp.Rows.Count; i++)
                {
                    var Row = Exchange.sheet.CreateRow(RowIndex + i);

                    if (Prffn.Index > -1)
                    {
                        var Cell = Row.CreateCell(Prffn.Index);
                        Cell.CellStyle = Prffn.Style;
                        Cell.SetCellValue(G.Resp.Rows.Get<string>(i, C.Resp.PodrPpl, C.PodrPpl.People, C.People.Prfssn, C.Prfssn.Name));

                        if (PrffnMerge != null)
                        {
                            Exchange.sheet.AddMergedRegion(new CellRangeAddress(Row.RowNum, Row.RowNum, PrffnMerge.FirstColumn, PrffnMerge.LastColumn));
                        }
                    }

                    if (FIO.Index > -1)
                    {
                        var Cell = Row.CreateCell(FIO.Index);
                        Cell.CellStyle = FIO.Style;
                        Cell.SetCellValue(Misc.GetShortFIO(G.Resp.Rows.Get_UnShow<uint>(i, C.Resp.PodrPpl, C.PodrPpl.People)));

                        if (FIOMerge != null)
                        {
                            Exchange.sheet.AddMergedRegion(new CellRangeAddress(Row.RowNum, Row.RowNum, FIOMerge.FirstColumn, FIOMerge.LastColumn));
                        }
                    }
                }
            }
            else
            {
                Exchange.sheet.ShiftRows(RowIndex + 1, Exchange.sheet.LastRowNum, G.Resp.Rows.Count - 1);
            }
        }

        class Grp_class
        {
            public Grp_class(List<Protokols_class.SGroup_class.Protokol_class.Sample_class> Samples, int TotalWidth)
            {
                this.Samples = Samples;
                this.Cells = new Cell_struct[Samples.Count];

                this.DefaultCellWidth = TotalWidth / Samples.Count;

                for (int i = 0; i < this.Cells.Length; i++)
                { this.Cells[i] = new Cell_struct(DefaultCellWidth); }
            }

            struct Cell_struct
            {
                public Cell_struct(int Length)
                {
                    this.Length = Length;
                    //this.ColIndex = ColIndex;
                    this.SetIn = true;
                }
                public Cell_struct(int Length, bool SetIn)
                {
                    this.Length = Length;
                    //this.ColIndex = ColIndex;
                    this.SetIn = SetIn;
                }
                /// <summary>Концентрации заносить здесь</summary>
                public bool SetIn;
                /*/// <summary>Номер ячейки</summary>
                public int ColIndex;*/
                /// <summary>Длина ячейки</summary>
                public int Length;

                public override string ToString()
                {
                    return "SetIn=" + SetIn.ToString() + ", Length=" + Length.ToString();
                }
            }

            public readonly int DefaultCellWidth;
            Cell_struct[] Cells;
            List<Protokols_class.SGroup_class.Protokol_class.Sample_class> Samples;

            public int CellCount { get { return Cells.Length; } }
            public int SampleCount { get { return Samples.Count; } }
            public int CellWidth(int Index) { return Cells[Index].Length; }
            public bool GetSetIn(int Index) { return Cells[Index].SetIn; }

            public Protokols_class.SGroup_class.Protokol_class.Sample_class GetSample(int Index)
            { return Samples[Index]; }
            public void CellSplit(int ColIndex, int FirstLength)
            {
                if (Cells[ColIndex].Length > FirstLength)
                {
                    Array.Resize(ref Cells, Cells.Length + 1);

                    if (ColIndex < Cells.Length - 1)
                    { Array.Copy(Cells, ColIndex, Cells, ColIndex + 1, Cells.Length - ColIndex - 1); }

                    Cells[ColIndex + 1] = new Cell_struct(Cells[ColIndex].Length - FirstLength, false);
                    Cells[ColIndex].Length = FirstLength;
                }
            }
            public string OLocationName { get { return T.OLocation.Rows.Get<string>(Samples[0].OLocationToID, C.OLocation.Name); } }

            public override string ToString()
            {
                return "CCount=" + Cells.Length.ToString() + ", SCount=" + Samples.Count.ToString() + ", DLength=" + DefaultCellWidth.ToString();
            }
        }

        static int NewColumnIndex(int ColIndex, int OldCount, int NewCount)
        {
            if (ColIndex == 0)
            { return 0; }
            else if (ColIndex == OldCount - 1)
            { return NewCount - 1; }
            else
            { return (int)((NewCount * ((double)(ColIndex + 1) / OldCount)) - 1); }
        }

        static void CopyStyleFromCell(NPOI.SS.UserModel.ICellStyle To, NPOI.SS.UserModel.ICell Cell)
        {
            To.Alignment = Cell.CellStyle.Alignment;
            To.BorderBottom = Cell.CellStyle.BorderBottom;
            To.BorderLeft = Cell.CellStyle.BorderLeft;
            To.BorderRight = Cell.CellStyle.BorderRight;
            To.BorderTop = Cell.CellStyle.BorderTop;
            To.BottomBorderColor = Cell.CellStyle.BottomBorderColor;
            To.FillBackgroundColor = Cell.CellStyle.FillBackgroundColor;
            To.FillForegroundColor = Cell.CellStyle.FillForegroundColor;
            To.FillPattern = Cell.CellStyle.FillPattern;
            To.SetFont(Cell.CellStyle.GetFont(Cell.Row.Sheet.Workbook));
            To.IsHidden = Cell.CellStyle.IsHidden;
            To.IsLocked = Cell.CellStyle.IsLocked;
            To.LeftBorderColor = Cell.CellStyle.LeftBorderColor;
            To.RightBorderColor = Cell.CellStyle.RightBorderColor;
            To.Rotation = Cell.CellStyle.Rotation;
            To.ShrinkToFit = Cell.CellStyle.ShrinkToFit;
            To.TopBorderColor = Cell.CellStyle.TopBorderColor;
            To.VerticalAlignment = Cell.CellStyle.VerticalAlignment;
            To.WrapText = Cell.CellStyle.WrapText;
            To.DataFormat = Cell.CellStyle.DataFormat;
        }

        struct SColumn_struct
        {
            public SColumn_struct(int ColumnIndex, NPOI.SS.UserModel.ICellStyle Style)
            {
                this.Index = ColumnIndex;
                this.Style = Style;
            }
            public SColumn_struct(NPOI.SS.UserModel.ICell Cell)
            {
                this.Index = Cell.ColumnIndex;
                this.Style = Cell.CellStyle;
            }
            public int Index;
            public NPOI.SS.UserModel.ICellStyle Style;
        }
        /// <summary>Перевожу двоичный double в десятичный вид</summary>
        static string DTSF(double Value)
        {
            var IntValue = (int)Value;
            if (Value == (double)IntValue)
            { return Value.ToString(); }
            else
            {
                int rnd = 1;

                while (Value != (double)(int)Value)
                {
                    Value *= 10;
                    rnd *= 10;
                }

                return "(" + Value.ToString() + "/" + rnd.ToString() + ")";
            }
        }

        public static string GetColCharName(int Index)
        {
            var One = new byte[10];
            int i, j = 0, Register = 65, NextCount = NextCount = Index / 26;
            for (i = 0; i < NextCount; i++)
            { One[j++] = (byte)Register; }

            One[j++] = (byte)(Index - (NextCount * 26) + Register);

            return Encoding.Default.GetString(One, 0, j);
        }

        static bool AddSynch(DataBase db, StartupLogo_Form.Loading_class Loading, string Name, string AlterName, ref DataBase.ITable Table, ref DataBase.ISTable SubTable, AddCols_delegate AddCols, bool Dedicate)
        {
            if (Loading != null)
            { Loading.LoadingComment = Name; }

            if (CheckTablesExist && Table != null)
            { throw new Exception("Таблица уже существует!"); }

            Table = db.Tables.Add(Encoding.GetEncoding(866), Name, AlterName);
            AddCols(Table);

            Table.AutoSave(Dedicate, DataBase.TypeOfTable.Combine);

            if (Table.Parent.DataSourceEnabled || Table.Parent.type == DataBase.RemoteType.Local)
            {
                SubTable = Table.CreateSubTable();

                SubTable.QUERRY().SHOW.DO();
                return true;
            }
            else
            {
                if (Loading != null)
                { Loading.LoadingComment = "Ошибка"; }
                return false;
            }
        }

        static bool AddRemote(DataBase db, StartupLogo_Form.Loading_class Loading, string Name, string AlterName, ref DataBase.ITable Table, ref DataBase.ISTable SubTable, AddCols_delegate AddCols, bool Dedicate)
        {
            if (Loading != null)
            { Loading.LoadingComment = Name; }

            if (CheckTablesExist && Table != null)
            { throw new Exception("Таблица уже существует!"); }

            Table = db.Tables.Add(Encoding.GetEncoding(866), Name, AlterName);

            AddCols(Table);

            Table.AutoSave(Dedicate, DataBase.TypeOfTable.Remote);

            if (Table.Parent.DataSourceEnabled || Table.Parent.type == DataBase.RemoteType.Local)
            {
                SubTable = Table.CreateSubTable();
                return true;
            }
            else
            {
                if (Loading != null)
                { Loading.LoadingComment = "Ошибка"; }
                return false;
            }
        }

        public static Form SelectForm()
        {
            switch ((data.UType)data.User<uint>(C.User.UType)) //смотрю тип пользователя и в зависимости от этого использую интерфейс
            {
                case data.UType.Admin: //админ

                    G.User.QUERRY().SHOW.DO();

                    for (int i = 0; i < G.User.Rows.Count; i++)
                    {
                        var UID = G.User.Rows.GetID(i);
                        var Key = G.User.Rows.Get<string>(i, C.User.ck1);

                        if (Key.Length > 0)
                        { data.SynchPool.KeysContainer.Add(UID, T.User.Rows.Get<string>(UID, C.User.ok1), T.User.Rows.Get<string>(UID, C.User.ck1)); }
                    }

                    return new AdminPanel();
                case data.UType.Employe: //Сотрудник.Перефирия
                case data.UType.MainEmploye: //Сотрудник.Основной
                case data.UType.Union: //Сотрудник.Отдел пром.стоков

                    G.User.QUERRY().SHOW.DO();

                    for (int i = 0; i < G.User.Rows.Count; i++)
                    {
                        var UID = G.User.Rows.GetID(i);
                        var Key = G.User.Rows.Get<string>(i, C.User.ck1);

                        if (Key.Length > 0)
                        { data.SynchPool.KeysContainer.Add(UID, T.User.Rows.Get<string>(UID, C.User.ok1), T.User.Rows.Get<string>(UID, C.User.ck1)); }
                    }

                    return new Employe_Form();
                default:
                    throw new Exception("Не извесный тип пользователя");
            }
        }
    }
}