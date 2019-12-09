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
    public partial class MassOutgo_Form : Form
    {
        public MassOutgo_Form(uint PodrID, uint OTypeID)
        {
            InitializeComponent();

            CreateNew_check.Checked = Employe_Form.SPoints.YM == Employe_Form.WorkYM;

            for (int i = 0; i < ATMisc.MonthesCount; i++)
            { Month_combo.Items.Add(ATMisc.GetMonthName1(i + 1)); }
            
            int Month;

            ATMisc.GetYearMonthFromYM(Employe_Form.SPoints.YM, out Year, out Month);

            Month_combo.SelectedIndex = Month - 1;

            From_Picker.MinDate = To_Picker.MinDate = new DateTime(Year, 1, 1);
            From_Picker.MaxDate = To_Picker.MaxDate = new DateTime(Year, 12, DateTime.DaysInMonth(Year, 12));

            Podr_combo.Items.Add("Все");

            for (int i = 0; i < G.Podr.Rows.Count; i++)
            { Podr_combo.Items.Add(G.Podr.Rows.Get<string>(i, C.Podr.ShrName)); }

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye)
            { Podr_combo.Enabled = true; }
            else
            {
                Podr_combo.SelectedIndex = G.Podr.Rows.GetIndex(data.User<uint>(C.User.Podr)) + 1;
                Podr_combo.Enabled = false;
            }

            for (int i = 0; i < G.OType.Rows.Count; i++)
            { OType_combo.Items.Add(G.OType.Rows.Get<string>(i, C.OType.Name)); }

            Podr_combo.SelectedIndex = G.Podr.Rows.GetIndex(PodrID) + 1;
            OType_combo.SelectedIndex = G.OType.Rows.GetIndex(OTypeID);

            CheckExist(null, null);
        }

        int Year;

        Misc.Podrs_class.PeriodType period;
     
        private void Quartal_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Quartal_combo.SelectedIndex > -1)
            {
                period = Misc.Podrs_class.PeriodType.Quartal;
                Month_combo.SelectedIndex = -1;

                From_Picker.Value = new DateTime(Year, Quartal_combo.SelectedIndex * 3 + 1, 1);
                To_Picker.Value = From_Picker.Value.AddMonths(2);

                CheckExist(null, null);
            }
        }

        private void Month_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Month_combo.SelectedIndex > -1)
            {
                period = Misc.Podrs_class.PeriodType.Month;
                Quartal_combo.SelectedIndex = -1;

                From_Picker.Value = new DateTime(Year, Month_combo.SelectedIndex + 1, 1);
                To_Picker.Value = new DateTime(Year, Month_combo.SelectedIndex + 1, DateTime.DaysInMonth(Year, Month_combo.SelectedIndex + 1));

                CheckExist(null, null);
            }
        }

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Show_button_Click(object sender, EventArgs e)
        {
            if (period == Misc.Podrs_class.PeriodType.None)
            { MessageBox.Show(this, "Не указан тип периода", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            {
                if (OType_combo.SelectedIndex > -1)
                {
                    if (Podr_combo.SelectedIndex == 0)
                    { Misc.OtchMassOutgo(0, ATMisc.GetYMFromDateTime(From_Picker.Value), period, G.OType.Rows.GetID(OType_combo.SelectedIndex), true, CreateNew_check.Checked); }
                    else
                    {
                        Misc.OtchMassOutgo(G.Podr.Rows.GetID(Podr_combo.SelectedIndex - 1), ATMisc.GetYMFromDateTime(From_Picker.Value), period, G.OType.Rows.GetID(OType_combo.SelectedIndex), true, CreateNew_check.Checked);
                    }
                }
                else
                { MessageBox.Show(this, "Не указан тип воды", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }            
        }

        private void From_Picker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void To_Picker_ValueChanged(object sender, EventArgs e)
        {

        }

        private void Year_button_Click(object sender, EventArgs e)
        {
            period = Misc.Podrs_class.PeriodType.Year;

            Quartal_combo.SelectedIndex = -1;
            Month_combo.SelectedIndex = -1;

            From_Picker.Value = new DateTime(Year, 1, 1);
            To_Picker.Value = From_Picker.Value.AddMonths(11);
            CheckExist(null, null);
        }

        private void CheckExist(object sender, EventArgs e)
        {
            Show_button.Enabled = false;

            uint PodrID;
            uint OTypeID;
            int DayFrom, DayTo;

            if (OType_combo.SelectedIndex > -1)
            {
                OTypeID = G.OType.Rows.GetID(OType_combo.SelectedIndex);

                if (Podr_combo.SelectedIndex == 0)
                { PodrID = 0; }
                else
                { PodrID = G.Podr.Rows.GetID(Podr_combo.SelectedIndex - 1); }

                DayFrom = ATMisc.GetYMDFromDateTime(From_Picker.Value.AddDays(-1));
                DayTo = ATMisc.GetYMDFromDateTime(To_Picker.Value.AddMonths(1));

                var CheckSM = G.SM.QUERRY()
                            .EXIST
                            .WHERE
                                .AC(C.SM.Amount).More.BV<double>(0)
                                .AND.ARC(C.SM.Sample, C.Sample.Number).More.BV<int>(0)
                                .AND.ARC(C.SM.Sample, C.Sample.CYMD).More.BV(DayFrom)
                                .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(DayTo)
                                .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDS).Less.BV(DayTo)
                                .AND.OB()
                                    .ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).More.BV(DayFrom)
                                    .OR.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.YMDE).EQUI.BV(0)
                                .CB()
                                .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                                .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Object, C.Object.OType).EQUI.BV(OTypeID);

                if (PodrID > 0)
                { CheckSM.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV(PodrID); }
                else
                { CheckSM.AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).More.BV<uint>(0); }

                if ((bool)CheckSM.DO()[0].Value)
                {
                    Existence_label.Text = "Концентрации обнаружены";
                    Show_button.Enabled = true;
                }
                else
                {
                    Existence_label.Text = "Концентрации НЕ обнаружены";
                    Show_button.Enabled = false;
                }
            }
            else
            { Existence_label.Text = "Заполните поля"; }
        }
        private void Month_combo_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (Month_combo.SelectedIndex > -1)
            {
                period = Misc.Podrs_class.PeriodType.Month;
                Quartal_combo.SelectedIndex = -1;

                From_Picker.Value = new DateTime(Year, Month_combo.SelectedIndex + 1, 1);
                To_Picker.Value = From_Picker.Value;

                CheckExist(null, null);
            }
        }
    }
}
