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
    public partial class PSGMethods_Form : Form
    {
        public PSGMethods_Form()
        {
            InitializeComponent();

            Income_label.Text = T.PSG.Rows.Get<string>((uint)data.PSG.Income, C.PSG.Name);
            Outgo_label.Text = T.PSG.Rows.Get<string>((uint)data.PSG.Outgo, C.PSG.Name);

            Income_Box.MaxLength =
            Outgo_Box.MaxLength = T.PSGM.GetColumn(C.PSGM.Method).Length;

            UpdateIncome();
            UpdateOutgo();
        }

        uint IncomePeopleID, OutgoPeopleID;

        void UpdateIncome()
        {
            if (RCache.PSG.GetIncomeID() == 0)
                IncomeD_label.Text = "Добавление";
            else
                IncomeD_label.Text = "Редактирование";

            Income_Box.Text = RCache.PSG.GetMethodName(data.PSG.Income);
            IncomePeopleID = RCache.PSG.GetPeopleID(data.PSG.Income);
            IncomePeople_label.Text = Misc.GetShortFIO(IncomePeopleID);
            var DT=RCache.PSG.DateOfCreateIncome();
            toolTip1.SetToolTip(Income_Box, "Дата создания - " + ATMisc.GetMonthName1(DT.Month) + ' ' + DT.Year.ToString());
        }
        void UpdateOutgo()
        {
            if (RCache.PSG.GetOutgoID() == 0)
                OutgoD_label.Text = "Добавление";
            else
                OutgoD_label.Text = "Редактирование";

            Outgo_Box.Text = RCache.PSG.GetMethodName(data.PSG.Outgo);
            OutgoPeopleID = RCache.PSG.GetPeopleID(data.PSG.Outgo);
            OutgoPeople_label.Text = Misc.GetShortFIO(OutgoPeopleID);
            var DT = RCache.PSG.DateOfCreateOutgo();
            toolTip1.SetToolTip(Outgo_Box, "Дата создания - " + ATMisc.GetMonthName1(DT.Month) + ' ' + DT.Year.ToString());
        }

        private void Save_button_Click(object sender, EventArgs e)
        {
            if (IncomePeopleID == 0)
            {
                MessageBox.Show(this, "Ответственного за водоснабжение нужно указать.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (OutgoPeopleID == 0)
            {
                MessageBox.Show(this, "Ответственного за водоотведение нужно указать.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            RCache.PSG.SetMethod(data.PSG.Income, Income_Box.Text, IncomePeopleID);
            RCache.PSG.SetMethod(data.PSG.Outgo, Outgo_Box.Text, OutgoPeopleID);
            this.Close();
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void DelIncome_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Вы уверены?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                RCache.PSG.DeleteIncome();
                UpdateIncome();
            }
        }

        private void DelOutgo_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Вы уверены?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                RCache.PSG.DeleteOutgo();
                UpdateOutgo();
            }
        }

        private void IncomePeople_label_Click(object sender, EventArgs e)
        {
            G.People.QUERRY().SHOW.DO();
            G.People.GetAutoForm(RowIndex =>
                {
                    IncomePeopleID = G.People.Rows.GetID(RowIndex);
                    IncomePeople_label.Text = Misc.GetShortFIO(IncomePeopleID);
                    return true;
                }, AutoForm.ShowType.User).ShowDialog();
        }

        private void OutgoPeople_label_Click(object sender, EventArgs e)
        {
            G.People.QUERRY().SHOW.DO();
            G.People.GetAutoForm(RowIndex =>
            {
                OutgoPeopleID = G.People.Rows.GetID(RowIndex);
                OutgoPeople_label.Text = Misc.GetShortFIO(OutgoPeopleID);
                return true;
            }, AutoForm.ShowType.User).ShowDialog();
        }
    }
}
