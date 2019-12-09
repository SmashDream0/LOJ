using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LaboratoryOnlineJournal;

public partial class PeriodChange_Form : Form
{
    public PeriodChange_Form()
    {
        InitializeComponent();

        Date_Picker.Value = ATMisc.GetDateTimeFromYM(Employe_Form.WorkYM);
    }

    private void Cancel_button_Click(object sender, EventArgs e)
    {
        this.Close();
    }

    private void Next_button_Click(object sender, EventArgs e)
    {
        int Year = Date_Picker.Value.Year,
            Month = Date_Picker.Value.Month;
        if (Month == 12)
        {
            Year++;
            Month = 1;
        }
        else
        { Month++; }

        Date_Picker.Value = new DateTime(Year, Month, 1);
    }

    private void Previous_button_Click(object sender, EventArgs e)
    {
        int Year = Date_Picker.Value.Year,
            Month = Date_Picker.Value.Month;
        if (Month == 1)
        {
            Year--;
            Month = 12;
        }
        else
        { Month--; }

        Date_Picker.Value = new DateTime(Year, Month, 1);
    }

    private void Continue_button_Click(object sender, EventArgs e)
    {
        int NewYM = ATMisc.GetYMFromYearMonth(Date_Picker.Value.Year, Date_Picker.Value.Month);

        if (Employe_Form.WorkYM != NewYM)
        {
            data.User<bool>(C.User.CNP, true);

            Employe_Form.WorkYM = NewYM;

            this.Close();
        }
    }
}