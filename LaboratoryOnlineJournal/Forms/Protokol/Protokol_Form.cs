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
    public partial class Protok_Form : Form
    {
        public Protok_Form(int YM, uint PodrID, bool Union, bool IsInPeriod, bool AllowGeneration)
        {
            InitializeComponent();

            this.IsInPeriod = IsInPeriod;
            this.AllowGeneration = AllowGeneration;

            Save_button.Enabled = IsInPeriod && AllowGeneration;
            CanMoveByButtons_check.Enabled = AllowGeneration;

            this.Protoks_Grid.ContextMenuStrip = contextMenuStrip1;

            this.Protokols = new Protokols_class(YM, Save_button.Enabled, PodrID, Union);

            Protoks_Grid.RowCount = this.Protokols.Count;

            CanMoveByButtons_check.Enabled = Save_button.Enabled;

            Protoks_Grid.ReadOnly = !IsInPeriod || !AllowGeneration;
        }

        struct Columns
        {
            public const byte Number = 0;
            public const byte DateCr = Number + 1;
            public const byte Time = DateCr + 1;
            public const byte Act = Time + 1;
            public const byte Type = Act + 1;
            public const byte Date = Type + 1;
            public const byte Podr = Date + 1;
            public const byte Samples = Podr + 1;
            public const byte ID = Samples + 1;
        }

        Protokols_class Protokols;
        readonly bool IsInPeriod;
        readonly bool AllowGeneration;

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Add_button_Click(object sender, EventArgs e)
        {

        }

        private void Protoks_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case Columns.ID:
                    e.Value = Protokols[e.RowIndex].PrtID;
                    break;
                case Columns.Act:
                    if (Protokols[e.RowIndex].Act > 0)
                    { e.Value = Protokols[e.RowIndex].Act; }
                    break;
                case Columns.Time:
                    e.Value = Protokols[e.RowIndex].StrTime;
                    break;
                case Columns.Samples:
                    e.Value = Protokols[e.RowIndex].SampleCount;
                    break;
                case Columns.Date:
                    e.Value = Protokols[e.RowIndex].DateOstr;
                    break;
                case Columns.DateCr:
                    e.Value = ATMisc.GetDateTimeFromYM(Protokols[e.RowIndex].YM).AddDays(Protokols[e.RowIndex].Day-1).ToShortDateString();
                    break;
                case Columns.Type:
                    e.Value = Protokols[e.RowIndex].ProtoType;
                    break;
                case Columns.Podr:
                    e.Value = T.Podr.Rows.Get<string>(Protokols[e.RowIndex].PodrID, C.Podr.ShrName);
                    break;
                case Columns.Number:
                    if (Protokols[e.RowIndex].Number == 0)
                    { Protoks_Grid[Columns.Number, e.RowIndex].Style.BackColor = Color.Red; }
                    else
                    { Protoks_Grid[Columns.Number, e.RowIndex].Style.BackColor = Color.White; }
                    
                    if (Protokols[e.RowIndex].PrtID == 0)
                    { Protoks_Grid[Columns.Samples, e.RowIndex].Style.BackColor = Protoks_Grid[Columns.ID, e.RowIndex].Style.BackColor = Color.LightGreen; }
                    else
                    { Protoks_Grid[Columns.Samples, e.RowIndex].Style.BackColor = Protoks_Grid[Columns.ID, e.RowIndex].Style.BackColor = Color.White; }

                    e.Value = Protokols[e.RowIndex].Number;
                    break;
            }
        }

        private void Protoks_Grid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                switch (e.ColumnIndex)
                {
                    case Columns.Samples:

                        var sb = new StringBuilder("Номера замеров:");

                        int SCount = Protokols[e.RowIndex].SampleCount;
                        var EndVar = "";

                        if (SCount > 25)
                        {
                            SCount = 25;
                            EndVar = "\n(" + (Protokols[e.RowIndex].SampleCount - SCount).ToString() + ")...";
                        }

                        for (int i = 0; i < SCount; i++)
                        {
                            sb.Append("\n" + Protokols[e.RowIndex][i].SPointName + ". №" + Protokols[e.RowIndex][i].Number.ToString() + ", показателей " + Protokols[e.RowIndex][i].MarkCount.ToString());
                        }

                        sb.Append(EndVar);

                        e.ToolTipText = sb.ToString();
                        break;
                    case Columns.Type:
                        e.ToolTipText = Protokols[e.RowIndex].Discribe;
                        break;
                }
            }
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Вы уверены, что хотите сохранить все протоколы?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
            { return; }

            for (int i = 0; i < Protokols.Count; i++)
            { Protokols[i].SaveChanges(); }

            Protoks_Grid.Invalidate();
        }

        private void Protoks_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex > -1 && e.RowIndex > -1 && e.ColumnIndex != Columns.Time)
            { Misc.OtchProtokol(Protokols[e.RowIndex], false); }
        }

        private void Protok_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void Save_Strip_Click(object sender, EventArgs e)
        {
            if (this.Protoks_Grid.CurrentCell != null)
            {
                if (Protokols[this.Protoks_Grid.CurrentCell.RowIndex].PrtID == 0)
                {
                    Protokols[this.Protoks_Grid.CurrentCell.RowIndex].SaveChanges();
                    this.Protoks_Grid.Invalidate();
                }
                else
                {
                    MessageBox.Show(this, "Протокол уже сохранен", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void Delete_Strip_Click(object sender, EventArgs e)
        {
            if (this.Protoks_Grid.CurrentCell != null)
            {
                if (Protokols[this.Protoks_Grid.CurrentCell.RowIndex].PrtID > 0)
                {
                    //if (this.Protoks_Grid.CurrentCell.RowIndex + 1 < this.Protokols.Count && this.Protokols[this.Protoks_Grid.CurrentCell.RowIndex + 1].PrtID > 0) return;

                    switch(MessageBox.Show(this, "Ниже следующий протокол сохранен. Если удалить текущий протокол, то нужно ли менять номера последующих протоколов?", "Вопрос", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case System.Windows.Forms.DialogResult.Yes:
                            Protokols[this.Protoks_Grid.CurrentCell.RowIndex].Delete(true);
                            break;
                        case System.Windows.Forms.DialogResult.No:
                            Protokols[this.Protoks_Grid.CurrentCell.RowIndex].Delete(false);
                            break;
                        case System.Windows.Forms.DialogResult.Cancel:
                            return;
                    }

                    this.Protoks_Grid.RowCount = Protokols.Count;
                    this.Protoks_Grid.Invalidate();
                }
                else
                {
                    MessageBox.Show(this, "Протокол нельзя удалить, т.к. он не сохранен", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void Up_Strip_Click(object sender, EventArgs e)
        {
            if (this.Protoks_Grid.CurrentCell != null)
            {
                if (Protokols[this.Protoks_Grid.CurrentCell.RowIndex].Move(-1))
                {
                    this.Protoks_Grid.CurrentCell = this.Protoks_Grid[this.Protoks_Grid.CurrentCell.ColumnIndex, this.Protoks_Grid.CurrentCell.RowIndex - 1];
                    this.Protoks_Grid.Invalidate();
                }
                else
                {
                    MessageBox.Show(this, "Протокол переместить, т.к. он или последующий протокол не сохранен или является крайним.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void Down_Strip_Click(object sender, EventArgs e)
        {
            if (this.Protoks_Grid.CurrentCell != null)
            {
                if (Protokols[this.Protoks_Grid.CurrentCell.RowIndex].Move(1))
                {
                    this.Protoks_Grid.CurrentCell = this.Protoks_Grid[this.Protoks_Grid.CurrentCell.ColumnIndex, this.Protoks_Grid.CurrentCell.RowIndex + 1];
                    this.Protoks_Grid.Invalidate();
                }
                else
                {
                    MessageBox.Show(this, "Не удалось произвести перемещение, т.к. перемещаемый или последующий протокол не сохранен или является крайним в таблице.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }

        private void Protoks_Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (IsInPeriod && AllowGeneration &&
                CanMoveByButtons_check.Checked)
            {
                switch (e.KeyData)
                {
                    case Keys.Up:
                        Up_Strip_Click(null, null);
                        e.SuppressKeyPress = true;
                        break;
                    case Keys.Down:
                        Down_Strip_Click(null, null);
                        e.SuppressKeyPress = true;
                        break;
                }
            }
        }

        private void CanMoveByButtons_check_CheckedChanged(object sender, EventArgs e)
        {
            if (CanMoveByButtons_check.Checked && MessageBox.Show(this, "Вы уверены, что хотите двигать протоколы стрелками?\nИх перемещение нарушит изначальную нумерацию.", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            { CanMoveByButtons_check.BackColor = Color.Red; }
            else
            {
                CanMoveByButtons_check.Checked = false;
                CanMoveByButtons_check.BackColor = this.BackColor;
            }
        }

        private void Protoks_Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case Columns.DateCr:
                    if (e.Value != null)
                    {
                        DateTime DateCr;

                        if (DateTime.TryParse((string)e.Value, out DateCr))
                        { 
                            Protokols[e.RowIndex].YM = ATMisc.GetYMFromDateTime(DateCr);
                            Protokols[e.RowIndex].Day = DateCr.Day;
                        }
                    }
                    break;
                case Columns.Act:
                    if (e.Value != null)
                    {
                        int ActNumber;

                        if (int.TryParse((string)e.Value, out ActNumber))
                        { Protokols[e.RowIndex].Act = ActNumber; }
                    }
                    break;
                case Columns.Time:
                    if (e.Value != null)
                    {
                        var value = ((string)e.Value).Replace('-', ':');

                        Protokols[e.RowIndex].StrTime = value;
                    }
                    break;
                case Columns.Number:
                    if (IsInPeriod && AllowGeneration)
                    {
                        if (e.Value != null)
                        {
                            int Number;

                            if (int.TryParse((string)e.Value, out Number))
                            {
                                if (Number == 0)
                                {
                                    MessageBox.Show(this, "Номер протокола должен быть больше нуля.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else
                                { Protokols[e.RowIndex].Number = Number; }
                            }
                        }
                    }
                    break;
            }
        }

        private void Protok_Form_Load(object sender, EventArgs e)
        {

        }

        private void Open_Strip_Click(object sender, EventArgs e)
        {
            if (Protoks_Grid.CurrentCell != null)
            { Misc.OtchProtokol(Protokols[Protoks_Grid.CurrentCell.RowIndex], false); }
        }

        private void ReSaveAndOpen_Strip_Click(object sender, EventArgs e)
        {
            if (Protoks_Grid.CurrentCell != null)
            { Misc.OtchProtokol(Protokols[Protoks_Grid.CurrentCell.RowIndex], true); }
        }

        private void Protoks_Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case Columns.Number:
                case Columns.Time:
                case Columns.Act:
                    if (IsInPeriod)
                    { e.Cancel = Protokols[e.RowIndex].PrtID == 0; }
                    break;
                case Columns.DateCr:
                    e.Cancel = (data.UType)data.User<uint>(C.User.UType) != data.UType.Admin;
                    break;
                default:
                    e.Cancel = true;
                    break;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            Down_Strip.Enabled = Up_Strip.Enabled = Delete_Strip.Enabled = Save_Strip.Enabled = Save_button.Enabled;
        }
    }
}
