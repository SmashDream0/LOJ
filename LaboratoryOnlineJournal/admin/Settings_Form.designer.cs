namespace LaboratoryOnlineJournal
{
    partial class Settings_Form
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.SaveBTN = new System.Windows.Forms.Button();
            this.ExitBTN = new System.Windows.Forms.Button();
            this.Settings_Tab = new System.Windows.Forms.TabControl();
            this.NySql_tab = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.DataSource_combo = new System.Windows.Forms.ComboBox();
            this.SqlPort_label = new System.Windows.Forms.Label();
            this.SqlPort_Box = new System.Windows.Forms.TextBox();
            this.SqlLogin_label = new System.Windows.Forms.Label();
            this.SqlLogin_Box = new System.Windows.Forms.TextBox();
            this.UseIpLast_button = new System.Windows.Forms.Button();
            this.UseIp2_button = new System.Windows.Forms.Button();
            this.UseIp1_button = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.SqlIp_label = new System.Windows.Forms.Label();
            this.SqlIp1_label = new System.Windows.Forms.Label();
            this.SqlIp1_Box = new System.Windows.Forms.TextBox();
            this.SqlIp2_label = new System.Windows.Forms.Label();
            this.SqlIp2_Box = new System.Windows.Forms.TextBox();
            this.SqlBdName_label = new System.Windows.Forms.Label();
            this.SqlPass_label = new System.Windows.Forms.Label();
            this.SqlIpLast_label = new System.Windows.Forms.Label();
            this.SqlBdName_Box = new System.Windows.Forms.TextBox();
            this.SqlIpLast_Box = new System.Windows.Forms.TextBox();
            this.SqlPass_Box = new System.Windows.Forms.TextBox();
            this.eMalr_tab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.eMaleUseSmtpSSL_Check = new System.Windows.Forms.CheckBox();
            this.eMaleSmtpPort_label = new System.Windows.Forms.Label();
            this.eMaleSmtpPort_Box = new System.Windows.Forms.TextBox();
            this.eMalePass_label = new System.Windows.Forms.Label();
            this.eMalePass_Box = new System.Windows.Forms.TextBox();
            this.eMaleLogin_label = new System.Windows.Forms.Label();
            this.eMaleLogin_Box = new System.Windows.Forms.TextBox();
            this.SmtpAdress_label = new System.Windows.Forms.Label();
            this.SmtpAdress_Box = new System.Windows.Forms.TextBox();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.CloseSP_Box = new System.Windows.Forms.TextBox();
            this.OpenSP = new System.Windows.Forms.TextBox();
            this.GenerateSP_button = new System.Windows.Forms.Button();
            this.SP_label = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ACrypto_check = new System.Windows.Forms.CheckBox();
            this.DirectEncryption_combo = new System.Windows.Forms.ComboBox();
            this.DirecteMaleImapPort_label = new System.Windows.Forms.Label();
            this.DirecteMaleImapPort_Box = new System.Windows.Forms.TextBox();
            this.DirectImapAdress_label = new System.Windows.Forms.Label();
            this.DirectImapAdress_Box = new System.Windows.Forms.TextBox();
            this.DirectEncryption_label = new System.Windows.Forms.Label();
            this.DirectSMTPPass_label = new System.Windows.Forms.Label();
            this.DirectSMTPPass_Box = new System.Windows.Forms.TextBox();
            this.DirectSMTPLogin_label = new System.Windows.Forms.Label();
            this.DirectSMTPLogin_Box = new System.Windows.Forms.TextBox();
            this.DirectSMTPCrypt_label = new System.Windows.Forms.Label();
            this.DirectSMTPCrypt_Check = new System.Windows.Forms.CheckBox();
            this.DirectSMTPPort_label = new System.Windows.Forms.Label();
            this.DirectSMTPPort_Box = new System.Windows.Forms.TextBox();
            this.DirectSMTPAdress_label = new System.Windows.Forms.Label();
            this.DirectSMTPAdress_Box = new System.Windows.Forms.TextBox();
            this.Settings_Tab.SuspendLayout();
            this.NySql_tab.SuspendLayout();
            this.eMalr_tab.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveBTN
            // 
            this.SaveBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveBTN.Location = new System.Drawing.Point(12, 284);
            this.SaveBTN.Name = "SaveBTN";
            this.SaveBTN.Size = new System.Drawing.Size(133, 29);
            this.SaveBTN.TabIndex = 1;
            this.SaveBTN.Text = "Сохранить";
            this.SaveBTN.UseVisualStyleBackColor = true;
            this.SaveBTN.Click += new System.EventHandler(this.SaveBTN_Click);
            // 
            // ExitBTN
            // 
            this.ExitBTN.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ExitBTN.Location = new System.Drawing.Point(315, 284);
            this.ExitBTN.Name = "ExitBTN";
            this.ExitBTN.Size = new System.Drawing.Size(133, 29);
            this.ExitBTN.TabIndex = 2;
            this.ExitBTN.Text = "Отменить";
            this.ExitBTN.UseVisualStyleBackColor = true;
            this.ExitBTN.Click += new System.EventHandler(this.ExitBTN_Click);
            // 
            // Settings_Tab
            // 
            this.Settings_Tab.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Settings_Tab.Controls.Add(this.NySql_tab);
            this.Settings_Tab.Controls.Add(this.eMalr_tab);
            this.Settings_Tab.Controls.Add(this.tabPage1);
            this.Settings_Tab.Controls.Add(this.tabPage2);
            this.Settings_Tab.Location = new System.Drawing.Point(12, 8);
            this.Settings_Tab.Name = "Settings_Tab";
            this.Settings_Tab.SelectedIndex = 0;
            this.Settings_Tab.Size = new System.Drawing.Size(436, 270);
            this.Settings_Tab.TabIndex = 0;
            // 
            // NySql_tab
            // 
            this.NySql_tab.Controls.Add(this.label3);
            this.NySql_tab.Controls.Add(this.DataSource_combo);
            this.NySql_tab.Controls.Add(this.SqlPort_label);
            this.NySql_tab.Controls.Add(this.SqlPort_Box);
            this.NySql_tab.Controls.Add(this.SqlLogin_label);
            this.NySql_tab.Controls.Add(this.SqlLogin_Box);
            this.NySql_tab.Controls.Add(this.UseIpLast_button);
            this.NySql_tab.Controls.Add(this.UseIp2_button);
            this.NySql_tab.Controls.Add(this.UseIp1_button);
            this.NySql_tab.Controls.Add(this.label4);
            this.NySql_tab.Controls.Add(this.SqlIp_label);
            this.NySql_tab.Controls.Add(this.SqlIp1_label);
            this.NySql_tab.Controls.Add(this.SqlIp1_Box);
            this.NySql_tab.Controls.Add(this.SqlIp2_label);
            this.NySql_tab.Controls.Add(this.SqlIp2_Box);
            this.NySql_tab.Controls.Add(this.SqlBdName_label);
            this.NySql_tab.Controls.Add(this.SqlPass_label);
            this.NySql_tab.Controls.Add(this.SqlIpLast_label);
            this.NySql_tab.Controls.Add(this.SqlBdName_Box);
            this.NySql_tab.Controls.Add(this.SqlIpLast_Box);
            this.NySql_tab.Controls.Add(this.SqlPass_Box);
            this.NySql_tab.Location = new System.Drawing.Point(4, 22);
            this.NySql_tab.Name = "NySql_tab";
            this.NySql_tab.Padding = new System.Windows.Forms.Padding(3);
            this.NySql_tab.Size = new System.Drawing.Size(428, 244);
            this.NySql_tab.TabIndex = 1;
            this.NySql_tab.Text = "Подключение";
            this.NySql_tab.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(166, 20);
            this.label3.TabIndex = 32;
            this.label3.Text = "Тип БД";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DataSource_combo
            // 
            this.DataSource_combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DataSource_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DataSource_combo.FormattingEnabled = true;
            this.DataSource_combo.Location = new System.Drawing.Point(181, 7);
            this.DataSource_combo.Name = "DataSource_combo";
            this.DataSource_combo.Size = new System.Drawing.Size(241, 21);
            this.DataSource_combo.TabIndex = 31;
            // 
            // SqlPort_label
            // 
            this.SqlPort_label.Location = new System.Drawing.Point(11, 115);
            this.SqlPort_label.Name = "SqlPort_label";
            this.SqlPort_label.Size = new System.Drawing.Size(166, 20);
            this.SqlPort_label.TabIndex = 30;
            this.SqlPort_label.Text = "Порт";
            // 
            // SqlPort_Box
            // 
            this.SqlPort_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlPort_Box.Location = new System.Drawing.Point(181, 115);
            this.SqlPort_Box.Name = "SqlPort_Box";
            this.SqlPort_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlPort_Box.TabIndex = 7;
            this.SqlPort_Box.TextChanged += new System.EventHandler(this.SqlPort_Box_TextChanged);
            // 
            // SqlLogin_label
            // 
            this.SqlLogin_label.Location = new System.Drawing.Point(11, 141);
            this.SqlLogin_label.Name = "SqlLogin_label";
            this.SqlLogin_label.Size = new System.Drawing.Size(166, 20);
            this.SqlLogin_label.TabIndex = 28;
            this.SqlLogin_label.Text = "Логин";
            // 
            // SqlLogin_Box
            // 
            this.SqlLogin_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlLogin_Box.Location = new System.Drawing.Point(181, 141);
            this.SqlLogin_Box.Name = "SqlLogin_Box";
            this.SqlLogin_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlLogin_Box.TabIndex = 8;
            // 
            // UseIpLast_button
            // 
            this.UseIpLast_button.Location = new System.Drawing.Point(155, 89);
            this.UseIpLast_button.Name = "UseIpLast_button";
            this.UseIpLast_button.Size = new System.Drawing.Size(21, 20);
            this.UseIpLast_button.TabIndex = 5;
            this.UseIpLast_button.Text = "=";
            this.UseIpLast_button.UseVisualStyleBackColor = true;
            this.UseIpLast_button.Click += new System.EventHandler(this.UseIp3_button_Click);
            // 
            // UseIp2_button
            // 
            this.UseIp2_button.Location = new System.Drawing.Point(155, 62);
            this.UseIp2_button.Name = "UseIp2_button";
            this.UseIp2_button.Size = new System.Drawing.Size(21, 20);
            this.UseIp2_button.TabIndex = 3;
            this.UseIp2_button.Text = "=";
            this.UseIp2_button.UseVisualStyleBackColor = true;
            this.UseIp2_button.Click += new System.EventHandler(this.UseIp2_button_Click);
            // 
            // UseIp1_button
            // 
            this.UseIp1_button.Location = new System.Drawing.Point(155, 37);
            this.UseIp1_button.Name = "UseIp1_button";
            this.UseIp1_button.Size = new System.Drawing.Size(21, 20);
            this.UseIp1_button.TabIndex = 1;
            this.UseIp1_button.Text = "=";
            this.UseIp1_button.UseVisualStyleBackColor = true;
            this.UseIp1_button.Click += new System.EventHandler(this.UseIp1_button_Click);
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 221);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(163, 20);
            this.label4.TabIndex = 23;
            this.label4.Text = "Используемый IP";
            // 
            // SqlIp_label
            // 
            this.SqlIp_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlIp_label.Location = new System.Drawing.Point(178, 221);
            this.SqlIp_label.Name = "SqlIp_label";
            this.SqlIp_label.Size = new System.Drawing.Size(659, 20);
            this.SqlIp_label.TabIndex = 22;
            // 
            // SqlIp1_label
            // 
            this.SqlIp1_label.Location = new System.Drawing.Point(11, 37);
            this.SqlIp1_label.Name = "SqlIp1_label";
            this.SqlIp1_label.Size = new System.Drawing.Size(166, 20);
            this.SqlIp1_label.TabIndex = 21;
            this.SqlIp1_label.Text = "Ip-адрес 1";
            // 
            // SqlIp1_Box
            // 
            this.SqlIp1_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlIp1_Box.Location = new System.Drawing.Point(181, 37);
            this.SqlIp1_Box.Name = "SqlIp1_Box";
            this.SqlIp1_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlIp1_Box.TabIndex = 2;
            // 
            // SqlIp2_label
            // 
            this.SqlIp2_label.Location = new System.Drawing.Point(11, 63);
            this.SqlIp2_label.Name = "SqlIp2_label";
            this.SqlIp2_label.Size = new System.Drawing.Size(166, 20);
            this.SqlIp2_label.TabIndex = 19;
            this.SqlIp2_label.Text = "Ip-адрес 2";
            // 
            // SqlIp2_Box
            // 
            this.SqlIp2_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlIp2_Box.Location = new System.Drawing.Point(181, 63);
            this.SqlIp2_Box.Name = "SqlIp2_Box";
            this.SqlIp2_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlIp2_Box.TabIndex = 4;
            // 
            // SqlBdName_label
            // 
            this.SqlBdName_label.Location = new System.Drawing.Point(11, 193);
            this.SqlBdName_label.Name = "SqlBdName_label";
            this.SqlBdName_label.Size = new System.Drawing.Size(166, 20);
            this.SqlBdName_label.TabIndex = 17;
            this.SqlBdName_label.Text = "Название базы данных";
            // 
            // SqlPass_label
            // 
            this.SqlPass_label.Location = new System.Drawing.Point(11, 167);
            this.SqlPass_label.Name = "SqlPass_label";
            this.SqlPass_label.Size = new System.Drawing.Size(166, 20);
            this.SqlPass_label.TabIndex = 16;
            this.SqlPass_label.Text = "Пароль";
            // 
            // SqlIpLast_label
            // 
            this.SqlIpLast_label.Location = new System.Drawing.Point(11, 89);
            this.SqlIpLast_label.Name = "SqlIpLast_label";
            this.SqlIpLast_label.Size = new System.Drawing.Size(166, 20);
            this.SqlIpLast_label.TabIndex = 14;
            this.SqlIpLast_label.Text = "Ip-адрес 3";
            // 
            // SqlBdName_Box
            // 
            this.SqlBdName_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlBdName_Box.Location = new System.Drawing.Point(181, 193);
            this.SqlBdName_Box.Name = "SqlBdName_Box";
            this.SqlBdName_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlBdName_Box.TabIndex = 10;
            // 
            // SqlIpLast_Box
            // 
            this.SqlIpLast_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlIpLast_Box.Location = new System.Drawing.Point(181, 89);
            this.SqlIpLast_Box.Name = "SqlIpLast_Box";
            this.SqlIpLast_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlIpLast_Box.TabIndex = 6;
            // 
            // SqlPass_Box
            // 
            this.SqlPass_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SqlPass_Box.Location = new System.Drawing.Point(181, 167);
            this.SqlPass_Box.Name = "SqlPass_Box";
            this.SqlPass_Box.PasswordChar = '*';
            this.SqlPass_Box.Size = new System.Drawing.Size(241, 20);
            this.SqlPass_Box.TabIndex = 9;
            this.SqlPass_Box.UseSystemPasswordChar = true;
            // 
            // eMalr_tab
            // 
            this.eMalr_tab.Controls.Add(this.label1);
            this.eMalr_tab.Controls.Add(this.eMaleUseSmtpSSL_Check);
            this.eMalr_tab.Controls.Add(this.eMaleSmtpPort_label);
            this.eMalr_tab.Controls.Add(this.eMaleSmtpPort_Box);
            this.eMalr_tab.Controls.Add(this.eMalePass_label);
            this.eMalr_tab.Controls.Add(this.eMalePass_Box);
            this.eMalr_tab.Controls.Add(this.eMaleLogin_label);
            this.eMalr_tab.Controls.Add(this.eMaleLogin_Box);
            this.eMalr_tab.Controls.Add(this.SmtpAdress_label);
            this.eMalr_tab.Controls.Add(this.SmtpAdress_Box);
            this.eMalr_tab.Location = new System.Drawing.Point(4, 22);
            this.eMalr_tab.Name = "eMalr_tab";
            this.eMalr_tab.Padding = new System.Windows.Forms.Padding(3);
            this.eMalr_tab.Size = new System.Drawing.Size(428, 244);
            this.eMalr_tab.TabIndex = 0;
            this.eMalr_tab.Text = "Настройки почты";
            this.eMalr_tab.UseVisualStyleBackColor = true;
            this.eMalr_tab.Click += new System.EventHandler(this.eMalr_tab_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 60);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 20);
            this.label1.TabIndex = 33;
            this.label1.Text = "Использовать Smtp SSL";
            // 
            // eMaleUseSmtpSSL_Check
            // 
            this.eMaleUseSmtpSSL_Check.AutoSize = true;
            this.eMaleUseSmtpSSL_Check.Location = new System.Drawing.Point(183, 59);
            this.eMaleUseSmtpSSL_Check.Name = "eMaleUseSmtpSSL_Check";
            this.eMaleUseSmtpSSL_Check.Size = new System.Drawing.Size(15, 14);
            this.eMaleUseSmtpSSL_Check.TabIndex = 3;
            this.eMaleUseSmtpSSL_Check.UseVisualStyleBackColor = true;
            // 
            // eMaleSmtpPort_label
            // 
            this.eMaleSmtpPort_label.Location = new System.Drawing.Point(13, 33);
            this.eMaleSmtpPort_label.Name = "eMaleSmtpPort_label";
            this.eMaleSmtpPort_label.Size = new System.Drawing.Size(166, 20);
            this.eMaleSmtpPort_label.TabIndex = 27;
            this.eMaleSmtpPort_label.Text = "Smtp порт";
            // 
            // eMaleSmtpPort_Box
            // 
            this.eMaleSmtpPort_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eMaleSmtpPort_Box.Location = new System.Drawing.Point(183, 33);
            this.eMaleSmtpPort_Box.Name = "eMaleSmtpPort_Box";
            this.eMaleSmtpPort_Box.Size = new System.Drawing.Size(239, 20);
            this.eMaleSmtpPort_Box.TabIndex = 2;
            this.eMaleSmtpPort_Box.TextChanged += new System.EventHandler(this.EmalePort_Box_TextChanged);
            // 
            // eMalePass_label
            // 
            this.eMalePass_label.Location = new System.Drawing.Point(13, 109);
            this.eMalePass_label.Name = "eMalePass_label";
            this.eMalePass_label.Size = new System.Drawing.Size(166, 20);
            this.eMalePass_label.TabIndex = 23;
            this.eMalePass_label.Text = "Пароль";
            // 
            // eMalePass_Box
            // 
            this.eMalePass_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eMalePass_Box.Location = new System.Drawing.Point(183, 109);
            this.eMalePass_Box.Name = "eMalePass_Box";
            this.eMalePass_Box.PasswordChar = '*';
            this.eMalePass_Box.Size = new System.Drawing.Size(239, 20);
            this.eMalePass_Box.TabIndex = 8;
            this.eMalePass_Box.UseSystemPasswordChar = true;
            // 
            // eMaleLogin_label
            // 
            this.eMaleLogin_label.Location = new System.Drawing.Point(13, 83);
            this.eMaleLogin_label.Name = "eMaleLogin_label";
            this.eMaleLogin_label.Size = new System.Drawing.Size(166, 20);
            this.eMaleLogin_label.TabIndex = 21;
            this.eMaleLogin_label.Text = "Логин";
            // 
            // eMaleLogin_Box
            // 
            this.eMaleLogin_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.eMaleLogin_Box.Location = new System.Drawing.Point(183, 83);
            this.eMaleLogin_Box.Name = "eMaleLogin_Box";
            this.eMaleLogin_Box.Size = new System.Drawing.Size(239, 20);
            this.eMaleLogin_Box.TabIndex = 7;
            // 
            // SmtpAdress_label
            // 
            this.SmtpAdress_label.Location = new System.Drawing.Point(13, 6);
            this.SmtpAdress_label.Name = "SmtpAdress_label";
            this.SmtpAdress_label.Size = new System.Drawing.Size(166, 20);
            this.SmtpAdress_label.TabIndex = 19;
            this.SmtpAdress_label.Text = "Smtp-адрес";
            // 
            // SmtpAdress_Box
            // 
            this.SmtpAdress_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SmtpAdress_Box.Location = new System.Drawing.Point(183, 6);
            this.SmtpAdress_Box.Name = "SmtpAdress_Box";
            this.SmtpAdress_Box.Size = new System.Drawing.Size(239, 20);
            this.SmtpAdress_Box.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.CloseSP_Box);
            this.tabPage1.Controls.Add(this.OpenSP);
            this.tabPage1.Controls.Add(this.GenerateSP_button);
            this.tabPage1.Controls.Add(this.SP_label);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(428, 244);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "Генератор";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // CloseSP_Box
            // 
            this.CloseSP_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseSP_Box.Location = new System.Drawing.Point(6, 115);
            this.CloseSP_Box.Multiline = true;
            this.CloseSP_Box.Name = "CloseSP_Box";
            this.CloseSP_Box.Size = new System.Drawing.Size(416, 89);
            this.CloseSP_Box.TabIndex = 6;
            this.CloseSP_Box.UseSystemPasswordChar = true;
            // 
            // OpenSP
            // 
            this.OpenSP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OpenSP.Location = new System.Drawing.Point(6, 22);
            this.OpenSP.Multiline = true;
            this.OpenSP.Name = "OpenSP";
            this.OpenSP.Size = new System.Drawing.Size(416, 87);
            this.OpenSP.TabIndex = 5;
            this.OpenSP.UseSystemPasswordChar = true;
            // 
            // GenerateSP_button
            // 
            this.GenerateSP_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.GenerateSP_button.Location = new System.Drawing.Point(299, 210);
            this.GenerateSP_button.Name = "GenerateSP_button";
            this.GenerateSP_button.Size = new System.Drawing.Size(123, 28);
            this.GenerateSP_button.TabIndex = 4;
            this.GenerateSP_button.Text = "Создать ключ";
            this.GenerateSP_button.UseVisualStyleBackColor = true;
            this.GenerateSP_button.Click += new System.EventHandler(this.GenerateSP_button_Click);
            // 
            // SP_label
            // 
            this.SP_label.AutoSize = true;
            this.SP_label.Location = new System.Drawing.Point(6, 3);
            this.SP_label.Name = "SP_label";
            this.SP_label.Size = new System.Drawing.Size(112, 13);
            this.SP_label.TabIndex = 2;
            this.SP_label.Text = "Генерировать ключи";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ACrypto_check);
            this.tabPage2.Controls.Add(this.DirectEncryption_combo);
            this.tabPage2.Controls.Add(this.DirecteMaleImapPort_label);
            this.tabPage2.Controls.Add(this.DirecteMaleImapPort_Box);
            this.tabPage2.Controls.Add(this.DirectImapAdress_label);
            this.tabPage2.Controls.Add(this.DirectImapAdress_Box);
            this.tabPage2.Controls.Add(this.DirectEncryption_label);
            this.tabPage2.Controls.Add(this.DirectSMTPPass_label);
            this.tabPage2.Controls.Add(this.DirectSMTPPass_Box);
            this.tabPage2.Controls.Add(this.DirectSMTPLogin_label);
            this.tabPage2.Controls.Add(this.DirectSMTPLogin_Box);
            this.tabPage2.Controls.Add(this.DirectSMTPCrypt_label);
            this.tabPage2.Controls.Add(this.DirectSMTPCrypt_Check);
            this.tabPage2.Controls.Add(this.DirectSMTPPort_label);
            this.tabPage2.Controls.Add(this.DirectSMTPPort_Box);
            this.tabPage2.Controls.Add(this.DirectSMTPAdress_label);
            this.tabPage2.Controls.Add(this.DirectSMTPAdress_Box);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(428, 244);
            this.tabPage2.TabIndex = 3;
            this.tabPage2.Text = "Отправка/Получение";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ACrypto_check
            // 
            this.ACrypto_check.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ACrypto_check.AutoSize = true;
            this.ACrypto_check.Location = new System.Drawing.Point(248, 134);
            this.ACrypto_check.Name = "ACrypto_check";
            this.ACrypto_check.Size = new System.Drawing.Size(174, 17);
            this.ACrypto_check.TabIndex = 66;
            this.ACrypto_check.Text = "Определить тип шифрования";
            this.ACrypto_check.UseVisualStyleBackColor = true;
            this.ACrypto_check.CheckedChanged += new System.EventHandler(this.ACrypto_check_CheckedChanged);
            // 
            // DirectEncryption_combo
            // 
            this.DirectEncryption_combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectEncryption_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DirectEncryption_combo.FormattingEnabled = true;
            this.DirectEncryption_combo.Items.AddRange(new object[] {
            "Отключено",
            "SSL2",
            "SSL3",
            "TSL",
            "Default"});
            this.DirectEncryption_combo.Location = new System.Drawing.Point(183, 132);
            this.DirectEncryption_combo.Name = "DirectEncryption_combo";
            this.DirectEncryption_combo.Size = new System.Drawing.Size(59, 21);
            this.DirectEncryption_combo.TabIndex = 65;
            // 
            // DirecteMaleImapPort_label
            // 
            this.DirecteMaleImapPort_label.Location = new System.Drawing.Point(13, 105);
            this.DirecteMaleImapPort_label.Name = "DirecteMaleImapPort_label";
            this.DirecteMaleImapPort_label.Size = new System.Drawing.Size(166, 20);
            this.DirecteMaleImapPort_label.TabIndex = 64;
            this.DirecteMaleImapPort_label.Text = "Imap порт";
            // 
            // DirecteMaleImapPort_Box
            // 
            this.DirecteMaleImapPort_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirecteMaleImapPort_Box.Location = new System.Drawing.Point(183, 105);
            this.DirecteMaleImapPort_Box.Name = "DirecteMaleImapPort_Box";
            this.DirecteMaleImapPort_Box.Size = new System.Drawing.Size(239, 20);
            this.DirecteMaleImapPort_Box.TabIndex = 61;
            // 
            // DirectImapAdress_label
            // 
            this.DirectImapAdress_label.Location = new System.Drawing.Point(13, 79);
            this.DirectImapAdress_label.Name = "DirectImapAdress_label";
            this.DirectImapAdress_label.Size = new System.Drawing.Size(166, 20);
            this.DirectImapAdress_label.TabIndex = 63;
            this.DirectImapAdress_label.Text = "Imap-адрес";
            // 
            // DirectImapAdress_Box
            // 
            this.DirectImapAdress_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectImapAdress_Box.Location = new System.Drawing.Point(183, 79);
            this.DirectImapAdress_Box.Name = "DirectImapAdress_Box";
            this.DirectImapAdress_Box.Size = new System.Drawing.Size(239, 20);
            this.DirectImapAdress_Box.TabIndex = 60;
            // 
            // DirectEncryption_label
            // 
            this.DirectEncryption_label.Location = new System.Drawing.Point(13, 132);
            this.DirectEncryption_label.Name = "DirectEncryption_label";
            this.DirectEncryption_label.Size = new System.Drawing.Size(166, 20);
            this.DirectEncryption_label.TabIndex = 62;
            this.DirectEncryption_label.Text = "Шифрование Imap";
            // 
            // DirectSMTPPass_label
            // 
            this.DirectSMTPPass_label.Location = new System.Drawing.Point(13, 185);
            this.DirectSMTPPass_label.Name = "DirectSMTPPass_label";
            this.DirectSMTPPass_label.Size = new System.Drawing.Size(166, 20);
            this.DirectSMTPPass_label.TabIndex = 59;
            this.DirectSMTPPass_label.Text = "Пароль";
            // 
            // DirectSMTPPass_Box
            // 
            this.DirectSMTPPass_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectSMTPPass_Box.Location = new System.Drawing.Point(183, 185);
            this.DirectSMTPPass_Box.Name = "DirectSMTPPass_Box";
            this.DirectSMTPPass_Box.PasswordChar = '*';
            this.DirectSMTPPass_Box.Size = new System.Drawing.Size(239, 20);
            this.DirectSMTPPass_Box.TabIndex = 57;
            this.DirectSMTPPass_Box.UseSystemPasswordChar = true;
            // 
            // DirectSMTPLogin_label
            // 
            this.DirectSMTPLogin_label.Location = new System.Drawing.Point(13, 159);
            this.DirectSMTPLogin_label.Name = "DirectSMTPLogin_label";
            this.DirectSMTPLogin_label.Size = new System.Drawing.Size(166, 20);
            this.DirectSMTPLogin_label.TabIndex = 58;
            this.DirectSMTPLogin_label.Text = "Логин";
            // 
            // DirectSMTPLogin_Box
            // 
            this.DirectSMTPLogin_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectSMTPLogin_Box.Location = new System.Drawing.Point(183, 159);
            this.DirectSMTPLogin_Box.Name = "DirectSMTPLogin_Box";
            this.DirectSMTPLogin_Box.Size = new System.Drawing.Size(239, 20);
            this.DirectSMTPLogin_Box.TabIndex = 56;
            // 
            // DirectSMTPCrypt_label
            // 
            this.DirectSMTPCrypt_label.Location = new System.Drawing.Point(13, 60);
            this.DirectSMTPCrypt_label.Name = "DirectSMTPCrypt_label";
            this.DirectSMTPCrypt_label.Size = new System.Drawing.Size(166, 20);
            this.DirectSMTPCrypt_label.TabIndex = 55;
            this.DirectSMTPCrypt_label.Text = "Использовать Smtp SSL";
            // 
            // DirectSMTPCrypt_Check
            // 
            this.DirectSMTPCrypt_Check.AutoSize = true;
            this.DirectSMTPCrypt_Check.Location = new System.Drawing.Point(183, 59);
            this.DirectSMTPCrypt_Check.Name = "DirectSMTPCrypt_Check";
            this.DirectSMTPCrypt_Check.Size = new System.Drawing.Size(15, 14);
            this.DirectSMTPCrypt_Check.TabIndex = 44;
            this.DirectSMTPCrypt_Check.UseVisualStyleBackColor = true;
            // 
            // DirectSMTPPort_label
            // 
            this.DirectSMTPPort_label.Location = new System.Drawing.Point(13, 33);
            this.DirectSMTPPort_label.Name = "DirectSMTPPort_label";
            this.DirectSMTPPort_label.Size = new System.Drawing.Size(166, 20);
            this.DirectSMTPPort_label.TabIndex = 53;
            this.DirectSMTPPort_label.Text = "Smtp порт";
            // 
            // DirectSMTPPort_Box
            // 
            this.DirectSMTPPort_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectSMTPPort_Box.Location = new System.Drawing.Point(183, 33);
            this.DirectSMTPPort_Box.Name = "DirectSMTPPort_Box";
            this.DirectSMTPPort_Box.Size = new System.Drawing.Size(239, 20);
            this.DirectSMTPPort_Box.TabIndex = 43;
            // 
            // DirectSMTPAdress_label
            // 
            this.DirectSMTPAdress_label.Location = new System.Drawing.Point(13, 6);
            this.DirectSMTPAdress_label.Name = "DirectSMTPAdress_label";
            this.DirectSMTPAdress_label.Size = new System.Drawing.Size(166, 20);
            this.DirectSMTPAdress_label.TabIndex = 50;
            this.DirectSMTPAdress_label.Text = "Smtp-адрес";
            // 
            // DirectSMTPAdress_Box
            // 
            this.DirectSMTPAdress_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.DirectSMTPAdress_Box.Location = new System.Drawing.Point(183, 6);
            this.DirectSMTPAdress_Box.Name = "DirectSMTPAdress_Box";
            this.DirectSMTPAdress_Box.Size = new System.Drawing.Size(239, 20);
            this.DirectSMTPAdress_Box.TabIndex = 42;
            // 
            // Settings_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(457, 319);
            this.Controls.Add(this.Settings_Tab);
            this.Controls.Add(this.ExitBTN);
            this.Controls.Add(this.SaveBTN);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(450, 357);
            this.Name = "Settings_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Настройки";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Settings_Form_FormClosed);
            this.Load += new System.EventHandler(this.Settings_Form_Load);
            this.Settings_Tab.ResumeLayout(false);
            this.NySql_tab.ResumeLayout(false);
            this.NySql_tab.PerformLayout();
            this.eMalr_tab.ResumeLayout(false);
            this.eMalr_tab.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveBTN;
        private System.Windows.Forms.Button ExitBTN;
        private System.Windows.Forms.TabControl Settings_Tab;
        private System.Windows.Forms.TabPage eMalr_tab;
        private System.Windows.Forms.Label SmtpAdress_label;
        private System.Windows.Forms.TextBox SmtpAdress_Box;
        private System.Windows.Forms.TabPage NySql_tab;
        private System.Windows.Forms.Label SqlBdName_label;
        private System.Windows.Forms.Label SqlPass_label;
        private System.Windows.Forms.Label SqlIpLast_label;
        private System.Windows.Forms.TextBox SqlBdName_Box;
        private System.Windows.Forms.TextBox SqlIpLast_Box;
        private System.Windows.Forms.TextBox SqlPass_Box;
        private System.Windows.Forms.Label SqlIp2_label;
        private System.Windows.Forms.TextBox SqlIp2_Box;
        private System.Windows.Forms.Label SqlIp1_label;
        private System.Windows.Forms.TextBox SqlIp1_Box;
        private System.Windows.Forms.Label eMaleSmtpPort_label;
        private System.Windows.Forms.TextBox eMaleSmtpPort_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox eMaleUseSmtpSSL_Check;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label SqlIp_label;
        private System.Windows.Forms.Button UseIpLast_button;
        private System.Windows.Forms.Button UseIp2_button;
        private System.Windows.Forms.Button UseIp1_button;
        private System.Windows.Forms.Label SqlLogin_label;
        private System.Windows.Forms.TextBox SqlLogin_Box;
        private System.Windows.Forms.Label SqlPort_label;
        private System.Windows.Forms.TextBox SqlPort_Box;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label SP_label;
        private System.Windows.Forms.TextBox OpenSP;
        private System.Windows.Forms.Button GenerateSP_button;
        private System.Windows.Forms.TextBox CloseSP_Box;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox DataSource_combo;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label DirectSMTPCrypt_label;
        private System.Windows.Forms.CheckBox DirectSMTPCrypt_Check;
        private System.Windows.Forms.Label DirectSMTPPort_label;
        private System.Windows.Forms.TextBox DirectSMTPPort_Box;
        private System.Windows.Forms.Label DirectSMTPAdress_label;
        private System.Windows.Forms.TextBox DirectSMTPAdress_Box;
        private System.Windows.Forms.Label DirectSMTPPass_label;
        private System.Windows.Forms.TextBox DirectSMTPPass_Box;
        private System.Windows.Forms.Label DirectSMTPLogin_label;
        private System.Windows.Forms.TextBox DirectSMTPLogin_Box;
        private System.Windows.Forms.Label eMalePass_label;
        private System.Windows.Forms.TextBox eMalePass_Box;
        private System.Windows.Forms.Label eMaleLogin_label;
        private System.Windows.Forms.TextBox eMaleLogin_Box;
        private System.Windows.Forms.ComboBox DirectEncryption_combo;
        private System.Windows.Forms.Label DirecteMaleImapPort_label;
        private System.Windows.Forms.TextBox DirecteMaleImapPort_Box;
        private System.Windows.Forms.Label DirectImapAdress_label;
        private System.Windows.Forms.TextBox DirectImapAdress_Box;
        private System.Windows.Forms.Label DirectEncryption_label;
        private System.Windows.Forms.CheckBox ACrypto_check;
    }
}