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
    public partial class SetText_Form : Form
    {
        public SetText_Form(string HeaderText)
        {
            InitializeComponent();

            this.Text = HeaderText;
        }

        public string ResultText
        {
            get { return Text_Box.Text; }
            set { Text_Box.Text = value; }
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void Continue_button_Click(object sender, EventArgs e)
        {
            if (Text_Box.Text.Length == 0)
            {
                MessageBox.Show(this, "Введине наименование", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            { DialogResult = System.Windows.Forms.DialogResult.OK; }
        }
    }
}
