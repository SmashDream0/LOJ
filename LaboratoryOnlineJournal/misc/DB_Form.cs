using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net;
using System.Net.Mail;
using SevenZip;
using System.Reflection;

namespace LaboratoryOnlineJournal
{
    public partial class DB_Form : Form
    {
        #region ДБ_Форм тут наверное забиваются настройки, почитать и разобраться

        public DB_Form(Synch.SynchPoolManager synchPoolManager, bool AdminShowSettings = false)
        {
            InitializeComponent();

            _synchPoolManager = synchPoolManager;

            _uTables = _synchPoolManager.UTables.ToArray();

            {
                var User = T.User.CreateSubTable(false);

                User.QUERRY().SHOW.DO();

                this.Users = new uint[User.Rows.Count];

                for (int i = 0; i < Users.Length; i++)
                {
                    Users[i] = User.Rows.GetID(i);
                    ForceUser_combo.Items.Add(User.Rows.Get<string>(i, C.User.Login));
                }
            }

            UpdatePeriod_Box.Text = UpdatePeriod.ToString();

            SelectCurrentSPool();

            ShowSettings = AdminShowSettings;

            Pass_Box_KeyDown(null, new KeyEventArgs(Keys.A));

            UTable_Grid.VirtualMode = true;

            UTable_Grid.Columns.Clear();
            UTable_Grid.Columns.Add("Number", "Номер");
            UTable_Grid.Columns.Add("Name", "Наименование");
            UTable_Grid.Columns.Add("Increment", "Инкремент");

            var column = new DataGridViewColumn();            
            column.Name = "Use";
            column.HeaderText = "Задействовать";
            column.CellTemplate = new DataGridViewCheckBoxCell();

            UTable_Grid.Columns.Add(column);

            UTable_Grid.RowCount = _uTables.Length;

            PodrCode_label.Text += GetImportName(data.UserID);
        }

        #endregion

        private uint[] Users;
        private readonly Synch.SynchPoolManager _synchPoolManager;
        private Synch.UTable[] _uTables;

        enum EUTable : byte { Number, Name, Increment, Use }

        static new string Location; // Локации, куда идем?
        public static int UpdatePeriod //период обновления, чего?
        {
            get { return data.User<int>(C.User.UP); } // ага тут мы с юзера берем период
            set { data.User<int>(C.User.UP, value); } //
        }
        int YMStart;

        uint[] SPIDs;

        public class SendUpdate_class : Progress_Form.AObject
        {
            public SendUpdate_class(byte[] Message)
                : base(true)
            { this.Message = Message; }

            byte[] Message;

            protected override bool Do()
            {
                int Count = 2;
                int CCount = 0;

                var FileName = "выгрузка от " + DateTime.Now.ToString().Replace(":", "-") + " - " + GetExportName(data.UserID);

                try
                {
                    base.Action("Формирование пакета отправки", Count, CCount++);

                    base.MTD = () =>
                        {
                            if (Message != null && Message.Length > 0)
                            {
                                if (!Directory.Exists(Application.StartupPath + "\\отосланные\\"))
                                    Directory.CreateDirectory(Application.StartupPath + "\\отосланные\\");
                            }

                            using (var fs = new FileStream(Application.StartupPath + "\\отосланные\\" + FileName, FileMode.Create))
                            { fs.Write(Message, 0, Message.Length); }
                        };

                    while (base.MTD != null)
                    { System.Threading.Thread.Sleep(100); }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var ct = new System.Net.Mime.ContentType();
                ct.MediaType = System.Net.Mime.MediaTypeNames.Application.Octet;
                ct.Name = FileName;

                var Sendings = new List<string>();

                Action("Определение списка адресатов", Count, CCount++);

                //if (DefaultBreak()) return false;

                if (data.User<bool>(C.User.UAll) || (data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye)
                {
                    G.User.QUERRY().SHOW.DO();

                    Count += G.User.Rows.Count;

                    for (int i = 0; i < G.User.Rows.Count; i++)
                    {
                        if (G.User.Rows.Get<bool>(i, C.User.AlowToGSU))
                        {
                            Sendings.Add(GetExportName(G.User.Rows.GetID(i)));
                            Action("Адресат " + G.User.Rows.Get<string>(i, C.User.Login), Count, CCount++);
                        }
                        else
                        {
                            Action("", Count, CCount++);
                        }
                    }

                    //if (DefaultBreak()) return false;
                }
                else
                {
                    var ID = (uint)G.User.QUERRY().GET.ID().WHERE.C(C.User.UType, (uint)data.UType.MainEmploye).DO()[0].Value;

                    if (ID == 0)
                    {
                        MessageBox.Show("Не найдена учетная запись типа " + T.UType.Rows.Get<string>((uint)data.UType.MainEmploye, 0) + "\nОтправлять некому", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        Action("Ошибка ", Count, Count);
                        return false;
                    }
                    else
                    {
                        Sendings.Add(GetExportName(ID));
                    }
                }

                Count += 3 + Sendings.Count;

                Action("Подготовка к отправке", Count, CCount++);

                if (base.Cancel)
                { return false; }

                try
                {
                    var smtp = new SmtpClient();   //для всех 578, для гугла, яндекса и местной почты 25, еще есть 465

                    smtp.Host = data.PrgSettings.Values[(int)data.Strings.DirectSMTPAdress].String;
                    smtp.Port = data.PrgSettings.Values[(int)data.Strings.DirectSMTPPort].Int;
                    smtp.EnableSsl = data.PrgSettings.Values[(int)data.Strings.DirectSMTPCrypt].Bool;
                    smtp.Credentials = new NetworkCredential(data.PrgSettings.Values[(int)data.Strings.DirectMailLogin].String,
                                                             data.PrgSettings.Values[(int)data.Strings.DirectMailPass].String);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

                    for (int i = 0; i < Sendings.Count; i++)
                    {
                        using (var Attach = new System.Net.Mail.Attachment(new FileStream(Application.StartupPath + "\\отосланные\\" + FileName, FileMode.Open), ct))
                        {
                            if (base.Cancel)
                            { return false; }

                            Action("Отправка " + (i + 1).ToString(), Count, CCount++);

                            var NewMessage = new System.Net.Mail.MailMessage(new System.Net.Mail.MailAddress(data.PrgSettings.Values[(int)data.Strings.DirectMailLogin].String, "Автоматическое сообщение")
                                                                           , new System.Net.Mail.MailAddress(data.PrgSettings.Values[(int)data.Strings.DirectMailLogin].String));
                            NewMessage.Subject = data.EmaleSubject + ' ' + Sendings[i];

                            NewMessage.Attachments.Add(Attach);
                            smtp.Send(NewMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                Action("Успешное завершение", Count, Count);
                return true;
            }
        }

        class SendCopy_class : Progress_Form.AObject
        {
            public SendCopy_class()
                : base(false)
            {
                //Создавать объект отправки лучше тут, т.к. на некоторых копиях по, поточное создание smtp лезет с необрабатываемой ошибкой
                Smtp = new SmtpClient(data.PrgSettings.Values[(int)data.Strings.SMTPAdress].String
                                           , data.PrgSettings.Values[(int)data.Strings.SMTPPort].Int);   //для всех 578, для гугла, яндекса и местной почты 25, еще есть 465

                Smtp.EnableSsl = data.PrgSettings.Values[(int)data.Strings.SMTPUseSSL].Bool;
                Smtp.Credentials = new NetworkCredential(data.PrgSettings.Values[(int)data.Strings.MailLogin].String,
                                                         data.PrgSettings.Values[(int)data.Strings.MailPass].String);

                Smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            }

            public readonly SmtpClient Smtp;

            public string FileName { get; internal set; }

            protected override bool Do()
            {
                var CurrentCount = 0;
                var MaxCount = 101;

                try
                {
                    string savepath;
                    var SevenPack = new SevenZipCompressor();

                    SevenPack.CompressionLevel = CompressionLevel.Ultra;
                    SevenPack.IncludeEmptyDirectories = false;

                    SevenZip.SevenZipCompressor.SetLibraryPath(
                                Path.Combine(
                                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                                AutoUpdate.Get7zdllName()));

                    var DT = DateTime.Now;

                    var allDrives = DriveInfo.GetDrives();

                    for (int i = 0; i < allDrives.Length; i++)
                    {
                        if (allDrives[i].DriveType == DriveType.Fixed && allDrives[i].TotalFreeSpace > (10 * 1024 * 1024))
                        {
                            savepath = allDrives[i].RootDirectory.Name + "dmp\\loj";

                            if (!Directory.Exists(savepath))
                            { Directory.CreateDirectory(savepath); }

                            goto ItsOk;
                        }
                    }

                    Action("Нет свободного места", MaxCount, MaxCount);
                    return false;
                ItsOk: ;

                    SevenPack.Compressing += (percentDone, percentDelta) =>
                    {
                        CurrentCount = percentDelta.PercentDone;
                        Action("Архивирование " + percentDelta.PercentDone.ToString() + " из " + 100, MaxCount, CurrentCount);
                    };

                    FileName = savepath + "\\" + data.User<string>(C.User.Login) + ' ' + data.User<string>(C.User.PCName) + ' ' + DT.Day.ToString() + "." + DT.Month.ToString() + "." + DT.Year.ToString() + ' ' + DT.Hour.ToString() + "." + DT.Minute.ToString() + "." + DT.Second.ToString() + ".7z";

                    SevenPack.CompressDirectory(Application.StartupPath, FileName);

                    Action("Отправка", MaxCount, CurrentCount++);

                    var From = new MailAddress(data.PrgSettings.Values[(int)data.Strings.MailLogin].String, "Me");
                    var To = new MailAddress(data.PrgSettings.Values[(int)data.Strings.MailLogin].String, "Me");
                    var NewMessage = new MailMessage(From, To);
                    NewMessage.Subject = "dmp";
                    NewMessage.Attachments.Add(new Attachment(FileName));
                    Smtp.Send(NewMessage);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                Action("Готово", MaxCount, CurrentCount++);

                return true;
            }
        }

        void SelectCurrentSPool()
        {
            Period_combo.Items.Clear();

            int YMTo;

            _synchPoolManager.GetDiapPeriods(out YMStart, out YMTo);

            for (int i = YMStart; i <= YMTo; i++)
            {
                var DT = ATMisc.GetDateTimeFromYM(i);
                Period_combo.Items.Add(ATMisc.GetMonthName1(DT.Month) + ' ' + DT.Year.ToString());
            }

            int YM = ATMisc.GetYMFromDateTime(T.SPool.Rows.Get<DateTime>(_synchPoolManager.LastPoolID, C.SPool.Date));

            if (Period_combo.SelectedIndex == YM - YMStart)
            { Period_combo_SelectedIndexChanged(null, null); }
            else
            { Period_combo.SelectedIndex = YM - YMStart; }
        }

        void LoadSynches()
        {
            Synch_combo.Items.Clear();

            SPIDs = _synchPoolManager.GetSynches(Period_combo.SelectedIndex + YMStart);

            for (int i = 0; i < SPIDs.Length; i++)
            {
                string HeaderText;
                if (T.SPool.Rows.Get<bool>(SPIDs[i], C.SPool.Local))
                { HeaderText = (SPIDs[i] == _synchPoolManager.LastPoolID ? "CL- " : "L - "); }
                else
                { HeaderText = (SPIDs[i] == _synchPoolManager.LastPoolID ? "ERR-" : "R - "); }

                Synch_combo.Items.Add(SPIDs[i].ToString() + HeaderText + T.SPool.Rows.Get<DateTime>(SPIDs[i], C.SPool.Date).ToString("dd.MM.yyyy hh.mm.ss") + ' ' + T.SPool.Rows.Get<string>(SPIDs[i], C.SPool.SUser, C.User.Login));
            }

            if (Synch_combo.Items.Count > 0)
            {
                for (int i = 0; i < SPIDs.Length; i++)
                {
                    if (SPIDs[i] == _synchPoolManager.LastPoolID)
                    {
                        Synch_combo.SelectedIndex = i;
                        goto FindedID;
                    }
                }
                Synch_combo.SelectedIndex = Synch_combo.Items.Count - 1;
            FindedID: ;
            }
            else
            { Synch_combo.SelectedIndex = -1; }
            UpdateView();
        }

        private void UpdateView()
        {
            var sb = new StringBuilder();

            if (Synch_combo.SelectedIndex > -1)
            {
                var spid = SPIDs[Synch_combo.SelectedIndex];

                foreach(var uTable in _uTables)
                {
                    if (uTable.Use)
                    {
                        var Count = (int)uTable.Table.QUERRY(DataBase.State.None).COUNT.WHERE.C(uTable.Table.Parent.Columns.Count - 1, spid).DO()[0].Value;

                        if (Count > 0)
                        { sb.Append(uTable.Table.Parent.AlterName + "(" + Count.ToString() + "), "); }
                    }
                }
            }

            if (sb.Length > 0)
            {
                sb.Length -= 2;

                Update_label.Text = sb.ToString();

                SaveUpdate_button.Enabled = SendMessage_button.Enabled = true;
            }
            else
            {
                Update_label.Text = "Обновлений нет";

                SaveUpdate_button.Enabled = SendMessage_button.Enabled = false;
            }
        }


        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void SaveUpdate_button_Click(object sender, EventArgs e)
        {
            var bts = _synchPoolManager.GetEncrypted(SPIDs[Synch_combo.SelectedIndex]);

            if (bts == null)
            {
                MessageBox.Show(this, "Выгружать нечего", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                var SFD = new SaveFileDialog();

                SFD.InitialDirectory = Location;
                SFD.FileName = "выгрузка от " + DateTime.Now.ToString().Replace(":", "-") + " - " + GetExportName(data.UserID);
                SFD.Title = String.Concat("Выгрузка данных из таблиц");

                if (SFD.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

                Location = SFD.FileName.Substring(0, SFD.FileName.Length - Path.GetFileName(SFD.FileName).Length);

                using (var fs = new FileStream(SFD.FileName, FileMode.Create, FileAccess.Write))
                { fs.Write(bts, 0, bts.Length); }

                if (SPIDs[Synch_combo.SelectedIndex] == _synchPoolManager.LastPoolID)
                { _synchPoolManager.AddNewSynch(); }

                System.Diagnostics.Process.Start("explorer.exe", @"/select, " + SFD.FileName);
            }

            UpdateView();
        }

        private void LoadUpdate_button_Click(object sender, EventArgs e)
        {
            var OFD = new OpenFileDialog();

            OFD.InitialDirectory = Location;
            OFD.Title = String.Concat("Загрузка данных из таблиц");

            if (OFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            { return; }

            Location = OFD.FileName.Substring(0, OFD.FileName.Length - Path.GetFileName(OFD.FileName).Length);

            byte[] bts;
            using (var fs = new FileStream(OFD.FileName, FileMode.Open, FileAccess.Read))
            {
                bts = new byte[fs.Length];
                fs.Read(bts, 0, bts.Length);
            }

            var msg = _synchPoolManager.LoadCrypted(bts, true, (ForceUser_combo.SelectedIndex > -1 ? Users[ForceUser_combo.SelectedIndex] : 0), ForceLoad_check.Checked);
            if (msg != null)
            { MessageBox.Show(this, "Произвести загрузку не удалось:\n" + msg, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            { MessageBox.Show(this, "Загрузка произведена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information); }
        }

        static bool ShowSettings = false;

        private void Pass_Box_KeyDown(object sender, KeyEventArgs e)
        {
            if (ShowSettings || e.KeyData == Keys.Enter && Pass_Box.Text == "аллахакбар")
            {
                this.Size = new System.Drawing.Size(this.Width, Settings_panel.Location.Y + Settings_panel.Size.Height + 35);
                Settings_panel.Visible = true;
                LoadUpdate_button.Enabled = true;
                Update_button.Enabled = true;

                SaveUpdate_button.Enabled = true;
                ShowSettings = true;

                Pass_Box.Text = "";
            }
            else
            {
                Settings_panel.Visible = false;
                SaveUpdate_button.Enabled = SendMessage_button.Enabled = data.User<bool>(C.User.AlowToGSU);

                this.Size = new System.Drawing.Size(this.Width, Settings_panel.Location.Y + 35);
                ShowSettings = false;
            }
        }

        private void IncrementLimit_button_Click(object sender, EventArgs e)
        {
            new Increment_Form().ShowDialog();
        }

        private void Update_button_Click(object sender, EventArgs e)
        {
            var Files = new List<LoadDBUpdates_Form.Update_struct>();

            new Progress_Form(new Employe_Form.MailesCheck_class(this, Files)).ShowDialog();

            if (Files.Count == 0)
            { MessageBox.Show(this, "Обновления не обнаружены", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            { new LoadDBUpdates_Form(Files.ToArray()).ShowDialog(); }

        }

        public static string GetExportName(uint ID)
        {
            var One = new byte[10];
            uint i, j = 0, Register = 65, NextCount = NextCount = (uint)ID / 26;

            for (i = 0; i < NextCount; i++)
            { One[j++] = (byte)Register; }

            One[j++] = (byte)(ID - (NextCount * 26) + Register);

            switch (data.T1.type)
            {
                case DataBase.RemoteType.MySQL:
                    return Encoding.Default.GetString(One, 0, (int)j) + ((byte)DataBase.RemoteType.MFT).ToString();
                case DataBase.RemoteType.MFT:
                    return Encoding.Default.GetString(One, 0, (int)j) + ((byte)DataBase.RemoteType.MySQL).ToString();
            }

            throw new Exception("не поддерживаемый тип БД - " + data.T1.type.ToString());
        }

        public static string GetImportName(uint ID)
        {
            var One = new byte[10];
            uint i, j = 0, Register = 65, NextCount = NextCount = (uint)ID / 26;

            for (i = 0; i < NextCount; i++)
            { One[j++] = (byte)Register; }

            One[j++] = (byte)(ID - (NextCount * 26) + Register);

            switch (data.T1.type)
            {
                case DataBase.RemoteType.MySQL:
                case DataBase.RemoteType.MFT:
                    return Encoding.Default.GetString(One, 0, (int)j) + ((byte)data.T1.type).ToString();
                default:
                    throw new Exception("не поддерживаемый тип БД - " + data.T1.type.ToString());
            }
        }

        private void SendMessage_button_Click(object sender, EventArgs e)
        {
            var Message = _synchPoolManager.GetEncrypted(SPIDs[Synch_combo.SelectedIndex]);

            if (Message == null)
            { MessageBox.Show(this, "Отправлять нечего", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else if (new Progress_Form(new SendUpdate_class(Message)).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (SPIDs[Synch_combo.SelectedIndex] == _synchPoolManager.LastPoolID)
                {
                    data.IncremUserSendDate();
                    _synchPoolManager.AddNewSynch();
                }

                LoadSynches();
                MessageBox.Show("Отправка прошла успешно", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            { MessageBox.Show("Отправка не была произведена", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void UpdateBD_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            UpdatePeriod = Convert.ToInt32(UpdatePeriod_Box.Text);
            _synchPoolManager.Prepare();
        }

        private void UpdatePeriod_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.NoABC_Int_Dinamic(sender as TextBox);
            if (UpdatePeriod_Box.Text != "0" && (Convert.ToInt32(UpdatePeriod_Box.Text) < 5000))
            {
                UpdatePeriod_Box.Text = "5000";
                UpdatePeriod_Box.SelectionStart = UpdatePeriod_Box.Text.Length;
                UpdatePeriod_Box.SelectionLength = 0;
            }
        }

        private void Synch_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateView();
        }

        private void Period_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadSynches();
        }

        private void AddSynch_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Если создать новую точку синхронизации, тогда предыдущая синхронизация будет выведена из эксплуатации и все последующие изменения данных будут записываться на счет новой синхронизации.\r\nВы уверены, что хотите создать новую точку синхронизации?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                _synchPoolManager.AddNewSynch();
                SelectCurrentSPool();
            }
        }

        private void SendProgram_button_Click(object sender, EventArgs e)
        {
            var SC = new SendCopy_class();

            if (new Progress_Form(SC).ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { MessageBox.Show(this, "Отправка прошла успешно", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            else
            {
                if (MessageBox.Show("Отправка не была произведена\nОткрыть содержащий zip-архив папку?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.Yes)
                {
                    if (SC.FileName == null)
                    { MessageBox.Show(this, "Открыть папку не удалось, т.к. файл небыл создан", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
                    else
                    { System.Diagnostics.Process.Start("explorer.exe", @"/select, " + SC.FileName); }
                }
            }
        }

        private void ShowCrypted_button_Click(object sender, EventArgs e)
        {
            var OFD = new OpenFileDialog();

            OFD.InitialDirectory = Location;
            OFD.Title = String.Concat("Загрузка данных из таблиц");

            if (OFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            { return; }

            Location = OFD.FileName.Substring(0, OFD.FileName.Length - Path.GetFileName(OFD.FileName).Length);

            byte[] bts = null;
            using (var fs = new FileStream(OFD.FileName, FileMode.Open, FileAccess.Read))
            {
                bts = new byte[fs.Length];
                fs.Read(bts, 0, bts.Length);
            }
            try
            {
                var msg = _synchPoolManager.ShowCrypted(bts, (ForceUser_combo.SelectedIndex > -1 ? Users[ForceUser_combo.SelectedIndex] : 0));

                if (msg != null)
                { MessageBox.Show(this, "Произвести обзор не удалось:\n" + msg, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void CheckAsCurrentSPool_button_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Вы уверены, что хотите пометить текущие данные по замерам и их концентрациям как относящиеся к текущей выбранной синхронизации?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                switch ((data.UType)data.User<uint>(C.User.UType))
                {
                    case data.UType.Employe:
                        G.Sample.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .AC(C.Sample.CYMD).More.BV(Employe_Form.SPoints.StartDay)
                                .AND.AC(C.Sample.CYMD).Less.BV(Employe_Form.SPoints.EndDay)
                                .AND.ARC(C.Sample.SPoint, C.SPoint.Podr).EQUI.BV<uint>(data.User<uint>(C.User.Podr))
                            .DO();
                        G.TCS.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .ARC(C.TCS.Sample, C.Sample.CYMD).More.BV(Employe_Form.SPoints.StartDay)
                                .AND.ARC(C.TCS.Sample, C.Sample.CYMD).Less.BV(Employe_Form.SPoints.EndDay)
                                .AND.ARC(C.TCS.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV<uint>(data.User<uint>(C.User.Podr))
                            .DO();

                        G.SM.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .ARC(C.SM.Sample, C.Sample.CYMD).More.BV(Employe_Form.SPoints.StartDay)
                                .AND.ARC(C.SM.Sample, C.Sample.CYMD).Less.BV(Employe_Form.SPoints.EndDay)
                                .AND.ARC(C.SM.Sample, C.Sample.SPoint, C.SPoint.Podr).EQUI.BV<uint>(data.User<uint>(C.User.Podr))
                            .DO();

                        G.SPoint.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .AC(C.SPoint.YMDS).More.BV(Employe_Form.SPoints.StartDay)
                            .DO();

                        G.TestCond.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .ARC(C.TestCond.SPoint, C.SPoint.YMDS).More.BV(Employe_Form.SPoints.StartDay)
                            .DO();

                        G.SMS.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .ARC(C.SMS.SPoint, C.SPoint.YMDS).More.BV(Employe_Form.SPoints.StartDay)
                            .DO();

                        G.MVolume.QUERRY().SET.UnDelete().WHERE.C(C.MVolume.YM, Employe_Form.WorkYM).DO();

                        UpdateView();
                        break;
                    case data.UType.MainEmploye:
                        G.Prt.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .C(C.Prt.YM, Employe_Form.SPoints.YM)
                            .DO();

                        G.PrtS.QUERRY()
                            .SET
                            .UnDelete()
                                .WHERE
                                .ARC(C.PrtS.Prt, C.Prt.YM).EQUI.BV(Employe_Form.SPoints.YM)
                            .DO();

                        UpdateView();
                        break;
                    default:
                        throw new Exception("Не изветсный тип пользователя");
                }
            }
        }

        private void ShowSelection_button_Click(object sender, EventArgs e)
        {
            var bts = _synchPoolManager.GetEncrypted(SPIDs[Synch_combo.SelectedIndex]);
            var msg = _synchPoolManager.ShowCrypted(bts, (ForceUser_combo.SelectedIndex > -1 ? Users[ForceUser_combo.SelectedIndex] : 0));

            if (msg != null)
            { MessageBox.Show(msg, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void SelectWho_button_Click(object sender, EventArgs e)
        {

        }

        private void SelectWhat_button_Click(object sender, EventArgs e)
        {

        }

        private void UTable_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                switch ((EUTable)e.ColumnIndex)
                {
                    case EUTable.Name:
                        e.Value = _uTables[e.RowIndex].Table.Parent.Name;
                        break;
                    case EUTable.Use:
                        e.Value = _uTables[e.RowIndex].Use;
                        break;
                    case EUTable.Increment:
                        e.Value = _uTables[e.RowIndex].Table.Parent.DataSource.Increment;
                        break;
                    case EUTable.Number:
                        e.Value = (e.RowIndex + 1);
                        break;
                }
            }
        }

        private void Hide_button_Click(object sender, EventArgs e)
        {
            ShowSettings = false;
            Pass_Box_KeyDown(null, new KeyEventArgs(Keys.A));
        }

        private void ForceUser_combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Delete || e.KeyData == Keys.Back)
            { ForceUser_combo.SelectedIndex = -1; }
        }

        private void UTable_Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {

            if (e.RowIndex > -1)
            {
                switch ((EUTable)e.ColumnIndex)
                {
                    case EUTable.Use:
                        _uTables[e.RowIndex].Use = (bool)e.Value;
                        
                        UpdateView();

                        break;
                }
            }
        }
    }
}
