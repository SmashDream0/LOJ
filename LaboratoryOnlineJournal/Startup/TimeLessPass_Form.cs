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
    public partial class TimeLessPass_Form : Form
    {
        string Pass;
        byte Nums = 0;
        public TimeLessPass_Form(string Pass)
        {
            InitializeComponent();
            this.Pass = Pass;
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
            this.Close();
        }

        private void Continue_button_Click(object sender, EventArgs e)
        {
            if (Pass == Pass_Box.Text)
            { this.DialogResult = System.Windows.Forms.DialogResult.Yes; }
            else
            {
                if (Nums > 1)
                {
                    MessageBox.Show(this, "Пароль не верный, попыток неосталось", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.DialogResult = System.Windows.Forms.DialogResult.No;
                }
                else
                { MessageBox.Show(this, "Пароль не верный, осталось попыток: " +(3 - ++Nums), "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }
        }
    }
}
