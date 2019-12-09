using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;

namespace LaboratoryOnlineJournal
{
    public partial class Mail_Form : Form
    {
        uint ID;
        public Mail_Form(uint ID)
        {
            InitializeComponent();
            this.ID = ID;

            Descryption_label.Text = "1)Нажмите \"Изменить пароль\", если хотите изменить пароль от вашей учетной записи\n\n2)Нажмите \"Сбросить учетную запись\", чтобы зайти из под вашей учетной записи, после сбоя питания и/или сбоя работы серверной части.";
        }

        bool Checks()
        {
            if (T.User.Rows.Get<string>(ID, C.User.Mail).Length == 0)
            {
                MessageBox.Show(this, "Почтовый ящик не указан, отправлять некуда.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            int SMTPPort;
            if (!int.TryParse(data.PrgSettings.Values[(int)data.Strings.SMTPPort].String, out SMTPPort))
            {
                MessageBox.Show(this, "Указанный порт не соответствует требованиям. Отправка невозможна", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (data.PrgSettings.Values[(int)data.Strings.MailLogin].String.Length == 0)
            {
                MessageBox.Show(this, "Логин почты отправителя не указан. Отправка невозможна", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            if (data.PrgSettings.Values[(int)data.Strings.MailPass].String.Length == 0)
            {
                MessageBox.Show(this, "Пароль почты отправителя не указан. Отправка невозможна", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            return true;
        }

        private void Send_button_Click(object sender, EventArgs e)
        {
            if (SetMail("Изменение пароля"))
            {
                if ((new SetNewPassWord_Form(ID)).ShowDialog() == System.Windows.Forms.DialogResult.Yes)
                {
                    MessageBox.Show(this, "Пароль успешно изменен.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                { MessageBox.Show(this, "Изменение пароля отменено.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information); }
            }
        }

        private void Cancel_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Repair_button_Click(object sender, EventArgs e)
        {
            if (SetMail("Восстановление учетной записи"))
            {
                T.User.Rows.Set(ID, C.User.IsHere, DataBase.AutoStatus.UnUse);
                MessageBox.Show(this, "Восстановление учетной записи прошло успешно.", "Сообщение", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
        }

        bool SetMail(string Message)
        {
            if (!Checks())
            { return false; }

            var Pass = new byte[6];
            var rnd = new Random();

            for (int i = 0; i < Pass.Length; i++)
            { Pass[i] = (byte)rnd.Next(48, 58); }

            var TimeLessPass = Encoding.Default.GetString(Pass);

            try
            {
                SmtpClient Smtp = new SmtpClient(data.PrgSettings.Values[(int)data.Strings.SMTPAdress].String, int.Parse(data.PrgSettings.Values[(int)data.Strings.SMTPPort].String));   //для всех 578, для гугла 25 и местной, еще есть 465

                if (data.PrgSettings.Values[(int)data.Strings.SMTPUseSSL].String.ToLower() == "true")
                { Smtp.EnableSsl = true; }
                else
                { Smtp.EnableSsl = false; }

                Smtp.Timeout = 5000;
                Smtp.Credentials = new NetworkCredential(data.PrgSettings.Values[(int)data.Strings.MailLogin].String
                                                         , data.PrgSettings.Values[(int)data.Strings.MailPass].String);

                MailAddress From = new MailAddress(data.PrgSettings.Values[(int)data.Strings.MailLogin].String, "Автоматическое сообщение"),
                            To = new MailAddress(T.User.Rows.Get<string>(ID, C.User.Mail), "Программа");
                var NewMessage = new MailMessage(From, To);
                NewMessage.Subject = Message;
                NewMessage.Body = String.Concat("Здравствуйте ", T.User.Rows.Get<string>(ID, C.User.Login), " ваш временный пароль ", TimeLessPass);
                Smtp.Send(NewMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "При отправке сообщения возникла ошибка:\n" + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            MessageBox.Show(this, "Письмо успешно отправлено, проверьте вашу почту.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);

            return new TimeLessPass_Form(TimeLessPass).ShowDialog() == System.Windows.Forms.DialogResult.Yes;
        }
    }
}
