using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Security.Authentication;

namespace LaboratoryOnlineJournal
{
    public partial class Settings_Form : Form
    {
        public Settings_Form()
        {
            InitializeComponent();

            var nms = Enum.GetNames(typeof(DataBase.RemoteType));

            for (int i = 0; i < nms.Length; i++)
            { DataSource_combo.Items.Add(nms[i]); }
        }

        private void SaveBTN_Click(object sender, EventArgs e)
        {
            if (!CheckValue(SqlPass_label, SqlPass_Box) ||
                !CheckValue(SqlIp1_label, SqlIp1_Box) ||
                !CheckValue(SqlIp2_label, SqlIp2_Box) ||
                !CheckValue(SqlIpLast_label, SqlIpLast_Box) ||
                !CheckValue(SqlPort_label, SqlPort_Box) ||
                !CheckValue(SqlBdName_label, SqlBdName_Box) ||
                !CheckValue(eMaleLogin_label, eMaleLogin_Box) ||
                !CheckValue(eMalePass_label, eMalePass_Box) ||
                !CheckValue(DirectImapAdress_label, DirectImapAdress_Box) ||
                !CheckValue(SmtpAdress_label, SmtpAdress_Box) ||
                !CheckIP(SqlIp1_label, SqlIp1_Box.Text) ||
                !CheckIP(SqlIp2_label, SqlIp2_Box.Text) ||
                !CheckValue(DirectSMTPPort_label, DirectSMTPPort_Box) ||
                !CheckValue(DirectSMTPLogin_label, DirectSMTPLogin_Box) ||
                !CheckValue(DirectSMTPPass_label, DirectSMTPPass_Box) ||
                SqlIp_label.Text.Length > 0 && !CheckIP(label4, SqlIp_label.Text)) return;

            if (DataSource_combo.SelectedIndex != data.PrgSettings.Values[(int)data.Strings.UseSQL].Int)
            { MessageBox.Show("Некоторые настройки вступят в силу, после перезапуска программы"); }

            data.PrgSettings.Values[(int)data.Strings.SqlIp1].String = SqlIp1_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SqlIp2].String = SqlIp2_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SqlIpLast].String = SqlIpLast_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SqlIp].String = SqlIp_label.Text;
            data.PrgSettings.Values[(int)data.Strings.SqlLogin].String = SqlLogin_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SqlPort].String = SqlPort_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SqlPassword].String = SqlPass_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.DATABASE].String = SqlBdName_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.MailLogin].String = eMaleLogin_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.MailPass].String = eMalePass_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SMTPAdress].String = SmtpAdress_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SMTPPort].String = eMaleSmtpPort_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.SMTPUseSSL].Bool = eMaleUseSmtpSSL_Check.Checked;

            data.PrgSettings.Values[(int)data.Strings.DirectIMAPAdress].String = DirectImapAdress_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.DirectIMAPPort].String = DirecteMaleImapPort_Box.Text;

            data.PrgSettings.Values[(int)data.Strings.DirectSMTPAdress].String = DirectSMTPAdress_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.DirectSMTPPort].String = DirectSMTPPort_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.DirectSMTPCrypt].Bool = DirectSMTPCrypt_Check.Checked;
            data.PrgSettings.Values[(int)data.Strings.DirectMailLogin].String = DirectSMTPLogin_Box.Text;
            data.PrgSettings.Values[(int)data.Strings.DirectMailPass].String = DirectSMTPPass_Box.Text;

            switch (DirectEncryption_combo.SelectedIndex)
            {
                case 4:
                    data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int = 240;
                    break;
                case 0:
                    data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int = 0;
                    break;
                case 1:
                    data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int = 12;
                    break;
                case 2:
                    data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int = 48;
                    break;
                case 3:
                    data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int = 192;
                    break;
            }

            data.PrgSettings.Values[(int)data.Strings.UseSQL].Int = DataSource_combo.SelectedIndex;
            data.PrgSettings.Values[(int)data.Strings.AutoCrypto].Bool = ACrypto_check.Checked;

            this.Close();
        }

        static bool CheckValue(Label label, TextBox textbox)
        {
            if (textbox.Text.Length == 0)
            {
                MessageBox.Show("Значение в поле \"" + label.Text + "\" не может быть пустым");
                textbox.Focus();
                return false;
            }
            return true;
        }

        static bool CheckIP(Label label, string textbox)
        {
            if (textbox == "localhost")
                return true;

            IPAddress ip;
            if (!IPAddress.TryParse(textbox, out ip))
            {
                MessageBox.Show(label.Parent, "Значение в поле \"" + label.Text + "\" не является ip-адресом.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ExitBTN_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Settings_Form_Load(object sender, EventArgs e)
        {
            SqlIp_label.Text = data.PrgSettings.Values[(int)data.Strings.SqlIp].String;

            SqlLogin_Box.Text = data.PrgSettings.Values[(int)data.Strings.SqlLogin].String;
            SqlPort_Box.Text = data.PrgSettings.Values[(int)data.Strings.SqlPort].String;
            SqlPass_Box.Text = data.PrgSettings.Values[(int)data.Strings.SqlPassword].String;
            SqlBdName_Box.Text = data.PrgSettings.Values[(int)data.Strings.DATABASE].String;
            eMaleLogin_Box.Text = data.PrgSettings.Values[(int)data.Strings.MailLogin].String;
            eMalePass_Box.Text = data.PrgSettings.Values[(int)data.Strings.MailPass].String;
            SmtpAdress_Box.Text = data.PrgSettings.Values[(int)data.Strings.SMTPAdress].String;
            DirectImapAdress_Box.Text = data.PrgSettings.Values[(int)data.Strings.DirectIMAPAdress].String;
            eMaleSmtpPort_Box.Text = data.PrgSettings.Values[(int)data.Strings.SMTPPort].String;
            DirecteMaleImapPort_Box.Text = data.PrgSettings.Values[(int)data.Strings.DirectIMAPPort].String;

            DirectSMTPAdress_Box.Text = data.PrgSettings.Values[(int)data.Strings.DirectSMTPAdress].String;
            DirectSMTPPort_Box.Text = data.PrgSettings.Values[(int)data.Strings.DirectSMTPPort].String;
            DirectSMTPCrypt_Check.Checked = data.PrgSettings.Values[(int)data.Strings.DirectSMTPCrypt].Bool;
            DirectSMTPLogin_Box.Text = data.PrgSettings.Values[(int)data.Strings.DirectMailLogin].String;
            DirectSMTPPass_Box.Text = data.PrgSettings.Values[(int)data.Strings.DirectMailPass].String;

            switch ((SslProtocols)data.PrgSettings.Values[(int)data.Strings.DirectIMAPUseSSL].Int)
            {
                case SslProtocols.Default:
                    DirectEncryption_combo.SelectedIndex = 4;
                    break;
                case SslProtocols.None:
                    DirectEncryption_combo.SelectedIndex = 0;
                    break;
                case SslProtocols.Ssl2:
                    DirectEncryption_combo.SelectedIndex = 1;
                    break;
                case SslProtocols.Ssl3:
                    DirectEncryption_combo.SelectedIndex = 2;
                    break;
                case SslProtocols.Tls:
                    DirectEncryption_combo.SelectedIndex = 3;
                    break;
            }
            eMaleUseSmtpSSL_Check.Checked = data.PrgSettings.Values[(int)data.Strings.SMTPUseSSL].Bool;
            SqlIp1_Box.Text = data.PrgSettings.Values[(int)data.Strings.SqlIp1].String;
            SqlIp2_Box.Text = data.PrgSettings.Values[(int)data.Strings.SqlIp2].String;
            SqlIpLast_Box.Text = data.PrgSettings.Values[(int)data.Strings.SqlIpLast].String;
            DataSource_combo.SelectedIndex = data.PrgSettings.Values[(int)data.Strings.UseSQL].Int;

            ACrypto_check.Checked = data.PrgSettings.Values[(int)data.Strings.AutoCrypto].Bool;

            this.Location = data.PrgSettings.Forms[(int)data.Forms.Settings].Location;
        }

        private void Settings_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            data.PrgSettings.Forms[(int)data.Forms.Settings].Set(this);

            data.PrgSettings.Save();
        }

        private void EmalePort_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.NoABC_Int_Dinamic(sender as TextBox);
        }

        private void UseIp1_button_Click(object sender, EventArgs e)
        {
            SqlIp_label.Text = data.PrgSettings.Values[(int)data.Strings.SqlIp1].String;
        }

        private void UseIp2_button_Click(object sender, EventArgs e)
        {
            SqlIp_label.Text = data.PrgSettings.Values[(int)data.Strings.SqlIp2].String;
        }

        private void UseIp3_button_Click(object sender, EventArgs e)
        {
            SqlIp_label.Text = data.PrgSettings.Values[(int)data.Strings.SqlIpLast].String;
        }

        private void SqlPort_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.NoABC_Int_Dinamic((sender as TextBox));
        }

        private void eMaleImapPort_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.NoABC_Int_Dinamic((sender as TextBox));
        }

        private void GenerateSP_button_Click(object sender, EventArgs e)
        {
            var RsaKey = new RSACryptoServiceProvider(2048);

            OpenSP.Text = RsaKey.ToXmlString(false);
            CloseSP_Box.Text = RsaKey.ToXmlString(true);
        }

        private void eMalr_tab_Click(object sender, EventArgs e)
        {

        }

        private void ACrypto_check_CheckedChanged(object sender, EventArgs e)
        {
            DirectEncryption_combo.Enabled = !ACrypto_check.Checked;
        }
    }
}
