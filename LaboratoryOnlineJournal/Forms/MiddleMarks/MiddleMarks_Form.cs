using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaboratoryOnlineJournal
{
    public partial class MiddleMarks_Form : Form
    {
        public MiddleMarks_Form()
        {
            InitializeComponent();

            Marks_Grid.Columns.Add("Name", "Наименование"); Marks_Grid.Columns[0].Width = 130;
            Marks_Grid.Columns.Add("EdType", "Ед.изм"); Marks_Grid.Columns[1].Width = 50;
            Marks_Grid.Columns.Add("VGroup", "Выпуск"); Marks_Grid.Columns[2].Width = 60;

            Marks_Grid.Columns[(int)Columns.EdType].DividerWidth = 2;

            for (int i = 0; i < ATMisc.MonthesCount; i++)
            {
                Marks_Grid.Columns.Add("m" + i.ToString(), ATMisc.GetMonthName1(i + 1));
                Marks_Grid.Columns[3 + i].Width = 60;
            }

            Marks_Grid.Columns[(int)Columns.m12].DividerWidth = 2;

            Marks_Grid.Columns.Add("q1", "К1"); Marks_Grid.Columns[(int)Columns.q1].Width = 60;
            Marks_Grid.Columns.Add("q2", "К2"); Marks_Grid.Columns[(int)Columns.q2].Width = 60;
            Marks_Grid.Columns.Add("q3", "К3"); Marks_Grid.Columns[(int)Columns.q3].Width = 60;
            Marks_Grid.Columns.Add("q4", "К4"); Marks_Grid.Columns[(int)Columns.q4].Width = 60;

            Marks_Grid.Columns[(int)Columns.q4].DividerWidth = 2;

            Marks_Grid.Columns.Add("Year", "Год"); Marks_Grid.Columns[(int)Columns.Year].Width = 60;

            int Year, Month;
            ATMisc.GetYearMonthFromYM(Employe_Form.SPoints.YM, out  Year, out Month);

            CanDo = false;

            this.SPoint = T.SPoint.CreateSubTable(false);

            Podr_combo.Items.Add("Все");

            Year_Box.Text = Year.ToString();

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Employe)
            {
                this.PodrIDs = new uint[] { data.User<uint>(C.User.Podr) };
                Podr_combo.Items.Add(T.Podr.Rows.Get<string>(this.PodrIDs[0], C.Podr.ShrName));
                Podr_combo.Enabled = false;

                CanDo = true;
                Podr_combo.SelectedIndex = 1;
            }
            else
            {
                var PodrIDs = new List<uint>();

                var User = T.User.CreateSubTable(false);

                User.QUERRY().SHOW.WHERE.AC(C.User.Podr).More.BV<uint>(0).DO();

                for (int i = 0; i < User.Rows.Count; i++)
                {
                    if (PodrIDs.IndexOf(User.Rows.Get_UnShow<uint>(i, C.User.Podr)) < 0)
                    { PodrIDs.Add(User.Rows.Get_UnShow<uint>(i, C.User.Podr)); }
                }

                this.PodrIDs = PodrIDs.ToArray();

                for (int i = 0; i < this.PodrIDs.Length; i++)
                { Podr_combo.Items.Add(T.Podr.Rows.Get<string>(this.PodrIDs[i], C.Podr.ShrName)); }

                CanDo = true;
                Podr_combo.SelectedIndex = 0;
            }
        }

        DataBase.ISTable SPoint;
        uint[] PodrIDs;
        uint[] ObjectIDs;
        Misc.MiddleMarks_class Marks;

        enum Columns : byte { Name, EdType, VGindex, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, q1, q2, q3, q4, Year };

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        object GetMonth(int MarkIndex, int MonthIndex)
        {
            var Value = Marks.MonthSumm(MarkIndex, MonthIndex);

            switch (Marks.VarType(MarkIndex))
            {
                case data.VarType.Bool:
                    if (Value == 1)
                    { return "Да"; }
                    else if (Value == 0)
                    { return "Нет"; }
                    else
                    { return Value; }
                case data.VarType.dbl:
                case data.VarType.i32:
                    if (Value == 0)
                    { return Marks.MZero(MarkIndex); }
                    else
                    { return Value; }
                default: throw new Exception("неизвестный тип");
            }
        }

        object GetQuartal(int MarkIndex, int QuartalIndex)
        {
            var Value = Marks.QuartalSumm(MarkIndex, QuartalIndex);

            switch (Marks.VarType(MarkIndex))
            {
                case data.VarType.Bool:
                    if (Value == 1)
                    { return "Да"; }
                    else if (Value == 0)
                    { return "Нет"; }
                    else
                    { return Value; }
                case data.VarType.dbl:
                case data.VarType.i32:
                    if (Value == 0)
                    { return Marks.MZero(MarkIndex); }
                    else
                    { return Value; }
                default: throw new Exception("неизвестный тип");
            }
        }

        private void Marks_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < Marks.MarkCount)
            {
                switch ((Columns)e.ColumnIndex)
                {
                    case Columns.Name:
                        e.Value = Marks.MarkName(e.RowIndex);
                        break;
                    case Columns.EdType:
                        e.Value = Marks.EdName(e.RowIndex);
                        break;
                    case Columns.m1:
                        e.Value = GetMonth(e.RowIndex, 0);
                        break;
                    case Columns.m2:
                        e.Value = GetMonth(e.RowIndex, 1);
                        break;
                    case Columns.m3:
                        e.Value = GetMonth(e.RowIndex, 2);
                        break;
                    case Columns.m4:
                        e.Value = GetMonth(e.RowIndex, 3);
                        break;
                    case Columns.m5:
                        e.Value = GetMonth(e.RowIndex, 4);
                        break;
                    case Columns.m6:
                        e.Value = GetMonth(e.RowIndex, 5);
                        break;
                    case Columns.m7:
                        e.Value = GetMonth(e.RowIndex, 6);
                        break;
                    case Columns.m8:
                        e.Value = GetMonth(e.RowIndex, 7);
                        break;
                    case Columns.m9:
                        e.Value = GetMonth(e.RowIndex, 8);
                        break;
                    case Columns.m10:
                        e.Value = GetMonth(e.RowIndex, 9);
                        break;
                    case Columns.m11:
                        e.Value = GetMonth(e.RowIndex, 10);
                        break;
                    case Columns.m12:
                        e.Value = GetMonth(e.RowIndex, 11);
                        break;
                    case Columns.q1:
                        e.Value = GetQuartal(e.RowIndex, 0);
                        break;
                    case Columns.q2:
                        e.Value = GetQuartal(e.RowIndex, 1);
                        break;
                    case Columns.q3:
                        e.Value = GetQuartal(e.RowIndex, 2);
                        break;
                    case Columns.q4:
                        e.Value = GetQuartal(e.RowIndex, 3);
                        break;
                    case Columns.Year:
                        {
                            var Value = Marks.YearSumm(e.RowIndex);

                            switch (Marks.VarType(e.RowIndex))
                            {
                                case data.VarType.Bool:
                                    if (Value == 1)
                                    { e.Value = "Да"; }
                                    else if (Value == 0)
                                    { e.Value = "Нет"; }
                                    else
                                    { e.Value = Value; }
                                    break;
                                case data.VarType.dbl:
                                case data.VarType.i32:
                                    if (Value == 0)
                                    { e.Value = Marks.MZero(e.RowIndex); }
                                    else
                                    { e.Value = Value; }
                                    break;
                                default: throw new Exception("неизвестный тип");
                            }
                        }
                        break;
                }
            }
        }

        private void Marks_Grid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < Marks.MarkCount)
            {
                switch ((Columns)e.ColumnIndex)
                {
                    case Columns.Name:
                        e.ToolTipText = Marks.MarkName(e.RowIndex);
                        break;
                    case Columns.EdType:
                        e.ToolTipText = Marks.EdName(e.RowIndex);
                        break;
                    case Columns.m1:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 0);
                        break;
                    case Columns.m2:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 1);
                        break;
                    case Columns.m3:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 2);
                        break;
                    case Columns.m4:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 3);
                        break;
                    case Columns.m5:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 4);
                        break;
                    case Columns.m6:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 5);
                        break;
                    case Columns.m7:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 6);
                        break;
                    case Columns.m8:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 7);
                        break;
                    case Columns.m9:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 8);
                        break;
                    case Columns.m10:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 9);
                        break;
                    case Columns.m11:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 10);
                        break;
                    case Columns.m12:
                        e.ToolTipText = Marks.MonthDesc(e.RowIndex, 11);
                        break;
                    case Columns.q1:
                        e.ToolTipText = Marks.QuartalDesc(e.RowIndex, 0);
                        break;
                    case Columns.q2:
                        e.ToolTipText = Marks.QuartalDesc(e.RowIndex, 1);
                        break;
                    case Columns.q3:
                        e.ToolTipText = Marks.QuartalDesc(e.RowIndex, 2);
                        break;
                    case Columns.q4:
                        e.ToolTipText = Marks.QuartalDesc(e.RowIndex, 3);
                        break;
                    case Columns.Year:
                        e.ToolTipText = Marks.YearDesc(e.RowIndex);
                        break;
                }
            }
        }

        bool CanDo = true;

        private void Year_Box_TextChanged(object sender, EventArgs e)
        {
            if (CanDo)
            {
                CanDo = false;
                DataBase.NoABC_Int_Dinamic(Year_Box);

                uint PodrID, OID;

                if (Podr_combo.SelectedIndex < 1)
                {
                    PodrID = 0;
                    this.Text = "Средние показатели за " + Year_Box.Text + " г.";
                }
                else
                {
                    PodrID = PodrIDs[Podr_combo.SelectedIndex - 1];
                    this.Text = "Средние показатели за " + Year_Box.Text + " г. подразделение: " + T.Podr.Rows.Get<string>(PodrID, C.Podr.ShrName);
                }

                if (Obj_combo.SelectedIndex < 1)
                { OID = 0; }
                else
                {
                    OID = this.ObjectIDs[Obj_combo.SelectedIndex - 1];

                    this.Text += ", вода: " + T.Object.Rows.Get<string>(OID, C.Object.Name);
                }

                Marks = new Misc.MiddleMarks_class(Convert.ToInt16(Year_Box.Text), PodrID, OID);

                Marks_Grid.RowCount = Marks.MarkCount + 1;

                for (int i = 0; i < Marks.MarkCount; i++)
                {
                    switch((data.VarType)T.Mark.Rows.Get_UnShow<uint>(Marks.MarkID(i), C.Mark.VarType))
                    {
                        case data.VarType.Bool:
                            Marks_Grid.Rows[i].DefaultCellStyle.Format = "";
                            break;
                        case data.VarType.dbl:
                        case data.VarType.i32:
                            if (T.Mark.Rows.Get_UnShow<bool>(Marks.MarkID(i), C.Mark.Exp))
                            {
                                if (T.Mark.Rows.Get<int>(Marks.MarkID(i), C.Mark.Round) > 0)
                                { Marks_Grid.Rows[i].DefaultCellStyle.Format = 'E' + T.Mark.Rows.Get<string>(Marks.MarkID(i), C.Mark.Round); }
                                else
                                { Marks_Grid.Rows[i].DefaultCellStyle.Format = "E"; }
                            }
                            else
                            {
                                if (T.Mark.Rows.Get<int>(Marks.MarkID(i), C.Mark.Round) > 0)
                                { Marks_Grid.Rows[i].DefaultCellStyle.Format = 'F' + T.Mark.Rows.Get<string>(Marks.MarkID(i), C.Mark.Round); }
                                else
                                { Marks_Grid.Rows[i].DefaultCellStyle.Format = "F"; }
                            }                            
                            break;
                    }
                }

                Marks_Grid.Invalidate();

                CanDo = true;
            }
        }

        private void PerviousYear_button_Click(object sender, EventArgs e)
        {
            Year_Box.Text = (Convert.ToInt32(Year_Box.Text) - 1).ToString();
        }

        private void NextYear_button_Click(object sender, EventArgs e)
        {
            Year_Box.Text = (Convert.ToInt32(Year_Box.Text) + 1).ToString();
        }

        private void SaveToFile_button_Click(object sender, EventArgs e)
        {
            Misc.OtchMiddleMarks(Marks);
        }

        private void UseVGroup_check_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void VG_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CanDo)
            {
                Marks_Grid.Invalidate();
            }
        }

        private void Marks_Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void Marks_Grid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void Marks_Grid_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Marks_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < Marks.MarkCount && (Columns)e.ColumnIndex == Columns.Name)
            {
                Marks.SetMarkEnabled(e.RowIndex, !Marks.GetMarkEnabled(e.RowIndex));

                Marks_Grid[(int)Columns.Name, e.RowIndex].Style.BackColor = (Marks.GetMarkEnabled(e.RowIndex) ? Color.White : Color.LightGray);

                Marks_Grid.CurrentCell = Marks_Grid[(int)Columns.EdType, e.RowIndex];
            }
        }

        private void Podr_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ObjectIDs = new List<uint>();

            if (Podr_combo.SelectedIndex > 0)
            { this.SPoint.QUERRY().SHOW.WHERE.C(C.SPoint.Podr, PodrIDs[Podr_combo.SelectedIndex - 1]).DO(); }
            else
            {
                var PQ = this.SPoint.QUERRY().SHOW.WHERE.C(C.SPoint.Podr, PodrIDs[0]);

                for (int i = 1; i < PodrIDs.Length; i++)
                { PQ.OR.C(C.SPoint.Podr, PodrIDs[i]); }

                PQ.DO();
            }

            for (int i = 0; i < SPoint.Rows.Count; i++)
            {
                if (ObjectIDs.IndexOf(SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object)) < 0)
                { ObjectIDs.Add(SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object)); }
            }

            ObjectIDs.Sort((it1, it2) => T.Object.Rows.Get<string>(it1, C.Object.Name).CompareTo(T.Object.Rows.Get<string>(it2, C.Object.Name)));

            this.ObjectIDs = ObjectIDs.ToArray();

            Obj_combo.Items.Clear();
            Obj_combo.Items.Add("Все");

            for (int i = 0; i < this.ObjectIDs.Length; i++)
            { Obj_combo.Items.Add(T.Object.Rows.Get<string>(this.ObjectIDs[i], C.Object.Name)); }

            Obj_combo.SelectedIndex = 0;

            //Year_Box_TextChanged(null, null);
        }
    }

    /*public partial class MiddleMarks_Form : Form
    {
        public MiddleMarks_Form()
        {
            InitializeComponent();

            AllowToShowMark = new bool[RCache.Marks.Count];

            int Year;

            CanDo = false;

            Podr_combo.Items.Add("Все");
            for (int i = 0; i < G.Podr.Rows.Count; i++)
            { Podr_combo.Items.Add(G.Podr.Rows.Get<string>(i, C.Podr.ShrName) + " - " + G.Podr.Rows.Get<string>(i, C.Podr.PSG, C.PSG.Name)); }

            this.Object = T.Object.CreateSubTable(false);
            this.Object.QUERRY().SHOW.DO();
            this.Object.Get_Default();
            this.Object.Sort(C.Object.Name);

            Obj_combo.Items.Add("Все");
            for (int i = 0; i < this.Object.Rows.Count; i++)
            { Obj_combo.Items.Add(this.Object.Rows.Get<string>(i, C.Object.Name)); }

            Obj_combo.SelectedIndex = 0;

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.Employe)
            {
                Podr_combo.Enabled = false;
                Podr_combo.SelectedIndex = G.Podr.Rows.GetIndex(data.User<uint>(C.User.Podr)) + 1;
                Year = DateTime.Now.Year;
            }
            else
            {
                Podr_combo.SelectedIndex = 0;
                Year = ATMisc.GetDateTimeFromYM(Employe_Form.SPoints.YM).Year;
            }
            Marks = new Misc.MiddleMarks_class();
            Round_Box.Text = data.User<string>(C.User.Round);
            CanDo = true;

            Year_Box.Text = Year.ToString();
        }

        DataBase.ISTable Object;
        Misc.MiddleMarks_class Marks;

        bool[] AllowToShowMark;
        int ShowCount = 0;

        enum Columns : byte { Name, EdType, VGindex, m1, m2, m3, m4, m5, m6, m7, m8, m9, m10, m11, m12, q1, q2, q3, q4 };

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Marks_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < Marks.Length)
                switch ((Columns)e.ColumnIndex)
                {
                    case Columns.Name:
                        e.Value = Marks[e.RowIndex].MarkName;
                        break;
                    case Columns.EdType:
                        e.Value = Marks[e.RowIndex].EdType;
                        break;
                    case Columns.m1:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 0);
                        break;
                    case Columns.m2:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 1);
                        break;
                    case Columns.m3:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 2);
                        break;
                    case Columns.m4:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 3);
                        break;
                    case Columns.m5:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 4);
                        break;
                    case Columns.m6:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 5);
                        break;
                    case Columns.m7:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 6);
                        break;
                    case Columns.m8:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 7);
                        break;
                    case Columns.m9:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 8);
                        break;
                    case Columns.m10:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 9);
                        break;
                    case Columns.m11:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 10);
                        break;
                    case Columns.m12:
                        e.Value = Marks[e.RowIndex].GetMiddleMonthSumm(0, 11);
                        break;
                    case Columns.q1:
                        e.Value = Marks[e.RowIndex].GetMiddleQuartalSumm(0, 0);
                        break;
                    case Columns.q2:
                        e.Value = Marks[e.RowIndex].GetMiddleQuartalSumm(0, 1);
                        break;
                    case Columns.q3:
                        e.Value = Marks[e.RowIndex].GetMiddleQuartalSumm(0, 2);
                        break;
                    case Columns.q4:
                        e.Value = Marks[e.RowIndex].GetMiddleQuartalSumm(0, 3);
                        break;
                }
        }

        bool CanDo = true;
        private void Round_Box_TextChanged(object sender, EventArgs e)
        {
            if (CanDo)
            {
                CanDo = false;
                DataBase.NoABC_Int_Dinamic(sender as TextBox);

                data.User(C.User.Round, Convert.ToInt32(Round_Box.Text));

                Marks_Grid.Invalidate();
                CanDo = true;
            }
        }

        private void Year_Box_TextChanged(object sender, EventArgs e)
        {
            if (CanDo)
            {
                CanDo = false;
                DataBase.NoABC_Int_Dinamic(Year_Box);

                uint PodrID, OID;

                if (Podr_combo.SelectedIndex < 1)
                {
                    PodrID = 0;
                    this.Text = "Средние показатели за " + Year_Box.Text + " г.";
                }
                else
                {
                    PodrID = G.Podr.Rows.GetID(Podr_combo.SelectedIndex - 1);
                    this.Text = "Средние показатели за " + Year_Box.Text + " г. подразделение: " + G.Podr.Rows.Get<string>(Podr_combo.SelectedIndex - 1, C.Podr.ShrName);
                }

                if (Obj_combo.SelectedIndex < 1)
                { OID = 0; }
                else
                {
                    OID = this.Object.Rows.GetID(Obj_combo.SelectedIndex - 1);

                    this.Text += ", вода: " + this.Object.Rows.Get<string>(OID, C.Object.Name);
                }

                Marks.Reload(Convert.ToInt16(Year_Box.Text), PodrID, OID);

                Marks_Grid.Columns.Clear();

                Marks_Grid.Columns.Add("Name", "Наименование"); Marks_Grid.Columns[0].Width = 130;
                Marks_Grid.Columns.Add("EdType", "Ед.изм"); Marks_Grid.Columns[1].Width = 50;
                Marks_Grid.Columns.Add("VGroup", "Выпуск"); Marks_Grid.Columns[2].Width = 60;

                Marks_Grid.Columns[(int)Columns.EdType].DividerWidth = 2;

                for (int i = 0; i < ATMisc.MonthesCount; i++)
                {
                    Marks_Grid.Columns.Add("m" + i.ToString(), ATMisc.GetMonthName1(i]);
                    Marks_Grid.Columns[3 + i].Width = 60;
                }

                Marks_Grid.Columns[(int)Columns.m12].DividerWidth = 2;

                Marks_Grid.Columns.Add("q1", "К1"); Marks_Grid.Columns[(int)Columns.q1].Width = 60;
                Marks_Grid.Columns.Add("q2", "К2"); Marks_Grid.Columns[(int)Columns.q2].Width = 60;
                Marks_Grid.Columns.Add("q3", "К3"); Marks_Grid.Columns[(int)Columns.q3].Width = 60;
                Marks_Grid.Columns.Add("q4", "К4"); Marks_Grid.Columns[(int)Columns.q4].Width = 60;

                Marks_Grid.RowCount = Marks.Length + 1;

                ShowCount = 0;

                for (int i = 0; i < RCache.Marks.Count; i++)
                {
                    AllowToShowMark[i] = false;
                    Marks_Grid.Rows[i].Visible = Marks[i].AllowToShow;
                }

                CanDo = true;
            }
        }

        private void PerviousYear_button_Click(object sender, EventArgs e)
        {
            Year_Box.Text = (Convert.ToInt32(Year_Box.Text) - 1).ToString();
        }

        private void NextYear_button_Click(object sender, EventArgs e)
        {
            Year_Box.Text = (Convert.ToInt32(Year_Box.Text) + 1).ToString();
        }

        private void SaveToFile_button_Click(object sender, EventArgs e)
        {
            if (ShowCount > 0)
            {
                for (int i = 0; i < AllowToShowMark.Length; i++)
                {
                    var Mark = Marks[i];
                    Mark.AllowToShow = AllowToShowMark[i];
                    Marks[i] = Mark;
                }
            }
            else
            {
                for (int i = 0; i < AllowToShowMark.Length; i++)
                {
                    var Mark = Marks[i];
                    Mark.AllowToShow = Marks_Grid[0, i].Visible;
                    Marks[i] = Mark;
                }
            }
            Misc.OtchMiddleMarks(Marks);
        }

        private void Marks_Grid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1 && e.RowIndex < Marks.Length)
                switch ((Columns)e.ColumnIndex)
                {
                    case Columns.m1:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 0);
                        break;
                    case Columns.m2:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 1);
                        break;
                    case Columns.m3:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 2);
                        break;
                    case Columns.m4:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 3);
                        break;
                    case Columns.m5:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 4);
                        break;
                    case Columns.m6:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 5);
                        break;
                    case Columns.m7:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 6);
                        break;
                    case Columns.m8:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 7);
                        break;
                    case Columns.m9:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 8);
                        break;
                    case Columns.m10:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 9);
                        break;
                    case Columns.m11:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 10);
                        break;
                    case Columns.m12:
                        e.ToolTipText = Marks[e.RowIndex].MonthToolTip(0, 11);
                        break;
                    case Columns.q1:
                        e.ToolTipText = Marks[e.RowIndex].QuartalToolTip(0, 0);
                        break;
                    case Columns.q2:
                        e.ToolTipText = Marks[e.RowIndex].QuartalToolTip(0, 1);
                        break;
                    case Columns.q3:
                        e.ToolTipText = Marks[e.RowIndex].QuartalToolTip(0, 2);
                        break;
                    case Columns.q4:
                        e.ToolTipText = Marks[e.RowIndex].QuartalToolTip(0, 3);
                        break;
                }
        }

        private void UseVGroup_check_CheckedChanged(object sender, EventArgs e)
        {
        }

        private void VG_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CanDo)
            {
                Marks_Grid.Invalidate();
            }
        }

        private void Marks_Grid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void Marks_Grid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void Marks_Grid_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void Marks_Grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < AllowToShowMark.Length && (Columns)e.ColumnIndex == Columns.Name)
            {
                if (AllowToShowMark[e.RowIndex])
                {
                    ShowCount--;
                    Marks_Grid[(int)Columns.Name, e.RowIndex].Style.BackColor = Color.White;
                }
                else
                {
                    ShowCount++;
                    Marks_Grid[(int)Columns.Name, e.RowIndex].Style.BackColor = Color.LightGray;
                }

                AllowToShowMark[e.RowIndex] = !AllowToShowMark[e.RowIndex];

                Marks_Grid.CurrentCell = Marks_Grid[(int)Columns.EdType, e.RowIndex];
            }
        }
    }*/
}