using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

//[assembly: AssemblyVersion("1.1.0.0")]

namespace LaboratoryOnlineJournal
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void Descryption_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
            Descryption.Text = "Электронный журнал замеров лаборатории.\r\n" +
                               "Версия " + Assembly.GetExecutingAssembly().GetName().Version +
                               ", МУП г.Астрахани \"Астрводоканал\", Отдел ИТиТ, Круглов Павел\r\n" +
                               ((DataBase.RemoteType)data.PrgSettings.Values[(int)data.Strings.UseSQL].Int).ToString() + " - " + data.PrgSettings.Values[(int)data.Strings.DATABASE].String;

            switch ((DataBase.RemoteType)data.PrgSettings.Values[(int)data.Strings.UseSQL].Int)
            {
                case DataBase.RemoteType.MySQL:
                    Descryption.Text += "\r\nip: " + data.PrgSettings.Values[(int)data.Strings.SqlIp].String + ";  login: " + data.PrgSettings.Values[(int)data.Strings.SqlLogin].String;
                    break;
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://dev.mysql.com/downloads/connector/net/");
        }

        private void label3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://imapx.codeplex.com/");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://npoi.codeplex.com/");
        }

        private void Descryptions_button_Click(object sender, EventArgs e)
        {
            var FilePath = Application.StartupPath + "\\Инструкции\\";
            {
                string[] Files;
                if (Directory.Exists(FilePath) && (Files = Directory.GetFiles(FilePath)).Length > 0)
                {
                    if (Files.Length == 1)
                    { System.Diagnostics.Process.Start(Files[0]); }
                    else
                    { System.Diagnostics.Process.Start(FilePath); }
                }
                else
                {
                    MessageBox.Show(this, "Инструкции не найдены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void DocChanges_button_Click(object sender, EventArgs e)
        {
            Startup_Form.CheckDocChanges();
        }
    }
}
