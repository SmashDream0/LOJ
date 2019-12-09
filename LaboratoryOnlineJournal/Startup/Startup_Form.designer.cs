
partial class Startup_Form
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
            this.components = new System.ComponentModel.Container();
            this.UserNames_combo = new System.Windows.Forms.ComboBox();
            this.UserPass_Box = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Start_button = new System.Windows.Forms.Button();
            this.Exit_button = new System.Windows.Forms.Button();
            this.Mail_Label = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.Version_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.CanUse_timer = new System.Windows.Forms.Timer(this.components);
            this.Timer_timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserNames_combo
            // 
            this.UserNames_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.UserNames_combo.FormattingEnabled = true;
            this.UserNames_combo.Location = new System.Drawing.Point(12, 46);
            this.UserNames_combo.Name = "UserNames_combo";
            this.UserNames_combo.Size = new System.Drawing.Size(361, 21);
            this.UserNames_combo.TabIndex = 5;
            this.UserNames_combo.SelectedIndexChanged += new System.EventHandler(this.UserNames_combo_SelectedIndexChanged);
            this.UserNames_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserNames_combo_KeyDown);
            // 
            // UserPass_Box
            // 
            this.UserPass_Box.AsciiOnly = true;
            this.UserPass_Box.Location = new System.Drawing.Point(12, 93);
            this.UserPass_Box.Name = "UserPass_Box";
            this.UserPass_Box.PromptChar = '+';
            this.UserPass_Box.Size = new System.Drawing.Size(361, 20);
            this.UserPass_Box.TabIndex = 6;
            this.UserPass_Box.UseSystemPasswordChar = true;
            this.UserPass_Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserPass_Box_KeyDown);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Имя пользователя";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(361, 19);
            this.label2.TabIndex = 9;
            this.label2.Text = "Пароль пользователя";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Start_button
            // 
            this.Start_button.Location = new System.Drawing.Point(15, 136);
            this.Start_button.Name = "Start_button";
            this.Start_button.Size = new System.Drawing.Size(88, 37);
            this.Start_button.TabIndex = 8;
            this.Start_button.Text = "Вход";
            this.Start_button.UseVisualStyleBackColor = true;
            this.Start_button.Click += new System.EventHandler(this.Start_button_Click);
            // 
            // Exit_button
            // 
            this.Exit_button.Location = new System.Drawing.Point(285, 136);
            this.Exit_button.Name = "Exit_button";
            this.Exit_button.Size = new System.Drawing.Size(88, 37);
            this.Exit_button.TabIndex = 10;
            this.Exit_button.Text = "Выход";
            this.Exit_button.UseVisualStyleBackColor = true;
            this.Exit_button.Click += new System.EventHandler(this.Exit_button_Click);
            // 
            // Mail_Label
            // 
            this.Mail_Label.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Mail_Label.Font = new System.Drawing.Font("Times New Roman", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Underline))));
            this.Mail_Label.ForeColor = System.Drawing.Color.Blue;
            this.Mail_Label.Location = new System.Drawing.Point(12, 116);
            this.Mail_Label.Name = "Mail_Label";
            this.Mail_Label.Size = new System.Drawing.Size(361, 16);
            this.Mail_Label.TabIndex = 11;
            this.Mail_Label.Text = "Проблема с учетной записью ?";
            this.Mail_Label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Mail_Label.Visible = false;
            this.Mail_Label.Click += new System.EventHandler(this.Mail_Label_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Version_Strip});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(385, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // Version_Strip
            // 
            this.Version_Strip.Name = "Version_Strip";
            this.Version_Strip.Size = new System.Drawing.Size(58, 20);
            this.Version_Strip.Text = "Версия";
            this.Version_Strip.Click += new System.EventHandler(this.Version_Strip_Click);
            // 
            // CanUse_timer
            // 
            this.CanUse_timer.Interval = 10000;
            this.CanUse_timer.Tick += new System.EventHandler(this.CanUse_timer_Tick);
            // 
            // Timer_timer
            // 
            this.Timer_timer.Interval = 1000;
            this.Timer_timer.Tick += new System.EventHandler(this.Timer_timer_Tick);
            // 
            // Startup_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(385, 178);
            this.Controls.Add(this.Mail_Label);
            this.Controls.Add(this.Exit_button);
            this.Controls.Add(this.Start_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UserPass_Box);
            this.Controls.Add(this.UserNames_combo);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Startup_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Добро пожаловать";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Startup_Form_FormClosed);
            this.Load += new System.EventHandler(this.Startup_Form_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.ComboBox UserNames_combo;
    private System.Windows.Forms.MaskedTextBox UserPass_Box;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button Start_button;
    private System.Windows.Forms.Button Exit_button;
    private System.Windows.Forms.Label Mail_Label;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem Version_Strip;
    private System.Windows.Forms.Timer CanUse_timer;
    private System.Windows.Forms.Timer Timer_timer;

}


