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
    public partial class MErrorEdit_Form : Form
    {
        MErrorList_Form.Diaps_class Diaps;
        int EditIndex = -1;
        /// <summary>Добавить</summary>
        public MErrorEdit_Form(MErrorList_Form.Diaps_class Diaps)
        {
            InitializeComponent();

            this.Diaps = Diaps;

            Mark_Box.Text = RCache.Marks[Diaps.NM.Mark.ID].Name;
            Mark_Box.Enabled = false;

            Norm_Box.Text = T.Norm.Rows.Get<string>(Diaps.NM.NormID, C.Norm.Name);
            Norm_Box.Enabled = false;

            if (Diaps.Count > 0)
            {
                Percent_check.Checked = Diaps[Diaps.Count - 1].Percent;
                Diap_Box.Text = Diaps[Diaps.Count - 1].To.ToString() + "-";
                Diap_Box.SelectionStart = Diap_Box.TextLength;
                Diap_Box.SelectionLength = 0;
            }

            DO_button.Text = "Добавить";
            DO_button.Click += Add_button_Click;
        }
        /// <summary>Изменить</summary>
        public MErrorEdit_Form(MErrorList_Form.Diaps_class Diaps, int EditIndex)
        {
            InitializeComponent();

            this.Diaps = Diaps;
            this.EditIndex = EditIndex;

            Mark_Box.Text = RCache.Marks[Diaps.NM.Mark.ID].Name;
            Mark_Box.Enabled = false;

            Norm_Box.Text = T.Norm.Rows.Get<string>(Diaps.NM.NormID, C.Norm.Name);
            Norm_Box.Enabled = false;

            Percent_check.Checked = Diaps[EditIndex].Percent;
            Diap_Box.Text = Diaps[EditIndex].Range;
            Volume_Box.Text = Diaps[EditIndex].Volume.ToString();

            DO_button.Text = "Изменить";
            DO_button.Click += Edit_button_Click;
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        bool Checks()
        {
            if (Diap_Box.TextLength == 0)
            {
                MessageBox.Show(this, "Необходимо указать диапазон.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Diap_Box.Focus();
                return false;
            }

            double To, From;
            RCache.Marks_class.Mark_class.SetRanges(out From, out To, Diap_Box.Text);

            Diap_Box.Text = RCache.Marks_class.Mark_class.GetRange(From, To);

            if (To <= From)
            {
                MessageBox.Show(this, "Конец диапазона не может быть меньше или равен его началу.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Diap_Box.Focus();
                return false;
            }

            int PrevIndex;
            if (EditIndex > -1)
                PrevIndex = EditIndex - 1;
            else
                PrevIndex = Diaps.Count - 1;

            if (PrevIndex > -1)
            {
                if (EditIndex > 0 && From != Diaps[PrevIndex].To)
                {
                    MessageBox.Show(this, "Начало этого диапазона должно быть равно концу предыдущего.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Diap_Box.Focus();
                    return false;
                }

                if (EditIndex > -1 && EditIndex < Diaps.Count - 1 && To != Diaps[PrevIndex].From)
                {
                    MessageBox.Show(this, "Окончание этого диапазона должно быть равно началу последующего(" + Diaps[EditIndex + 1].From.ToString() + ").", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Diap_Box.Focus();
                    return false;
                }
            }

            if (Volume_Box.TextLength == 0)
            {
                MessageBox.Show(this, "Необходимо указать погрешность.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Volume_Box.Focus();
                return false;
            }

            return true;
        }

        private void Add_button_Click(object sender, EventArgs e)
        {
            if (!Checks()) return;

            double From, To, Percent;
            RCache.Marks_class.Mark_class.SetRanges(out From, out To, Diap_Box.Text);
            Percent = Convert.ToDouble(DataBase.NoAbc_Double_Static(Volume_Box.Text));

            if(Diaps.Add(From, To, Percent, Percent_check.Checked)) this.Close();
        }

        private void Edit_button_Click(object sender, EventArgs e)
        {
            if (!Checks()) return;

            double From, To, Percent;
            RCache.Marks_class.Mark_class.SetRanges(out From, out To, Diap_Box.Text);
            Percent = Convert.ToDouble(DataBase.NoAbc_Double_Static(Volume_Box.Text));

            if(Diaps.Change(EditIndex, From, To, Percent, Percent_check.Checked))this.Close();
        }

        private void Percent_Box_TextChanged(object sender, EventArgs e)
        {
            if (Percent_check.Checked)
            {
                DataBase.NoABC_Int_Dinamic(sender as TextBox);
                var Percent = Convert.ToInt32(Volume_Box.Text);
                if (Percent > 100)
                {
                    Volume_Box.Text = "100";
                    Volume_Box.SelectionStart = 3;
                    Volume_Box.SelectionLength = 0;
                }
            }
            else
            {
                DataBase.NoABC_Double_Dinamic(sender as TextBox);
            }
        }

        private void Percent_check_CheckedChanged(object sender, EventArgs e)
        {
            if (Percent_check.Checked)
                Volume_label.Text = "Погрешность процент";
            else
                Volume_label.Text = "Погрешность абсолютная";
        }
    }
}
