namespace LaboratoryOnlineJournal
{
    partial class UsersBlock_Form
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.User_Grid = new System.Windows.Forms.DataGridView();
            this.AddUser_button = new System.Windows.Forms.Button();
            this.RemoveUser_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Cause_Box = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Block_button = new System.Windows.Forms.Button();
            this.Close_button = new System.Windows.Forms.Button();
            this.AddAll_button = new System.Windows.Forms.Button();
            this.UnBlock_button = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Login = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.EnabledToUse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IsOnline = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.User_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // User_Grid
            // 
            this.User_Grid.AllowUserToAddRows = false;
            this.User_Grid.AllowUserToDeleteRows = false;
            this.User_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.User_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.User_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Login,
            this.UType,
            this.EnabledToUse,
            this.IsOnline});
            this.User_Grid.Location = new System.Drawing.Point(12, 39);
            this.User_Grid.Name = "User_Grid";
            this.User_Grid.ReadOnly = true;
            this.User_Grid.RowHeadersVisible = false;
            this.User_Grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.User_Grid.Size = new System.Drawing.Size(471, 150);
            this.User_Grid.TabIndex = 0;
            this.User_Grid.VirtualMode = true;
            this.User_Grid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.User_Grid_CellMouseDoubleClick);
            this.User_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.User_Grid_CellValueNeeded);
            this.User_Grid.CurrentCellChanged += new System.EventHandler(this.User_Grid_CurrentCellChanged);
            // 
            // AddUser_button
            // 
            this.AddUser_button.Location = new System.Drawing.Point(13, 196);
            this.AddUser_button.Name = "AddUser_button";
            this.AddUser_button.Size = new System.Drawing.Size(27, 23);
            this.AddUser_button.TabIndex = 1;
            this.AddUser_button.Text = "+";
            this.AddUser_button.UseVisualStyleBackColor = true;
            this.AddUser_button.Click += new System.EventHandler(this.AddUser_button_Click);
            // 
            // RemoveUser_button
            // 
            this.RemoveUser_button.Location = new System.Drawing.Point(46, 196);
            this.RemoveUser_button.Name = "RemoveUser_button";
            this.RemoveUser_button.Size = new System.Drawing.Size(27, 23);
            this.RemoveUser_button.TabIndex = 2;
            this.RemoveUser_button.Text = "-";
            this.RemoveUser_button.UseVisualStyleBackColor = true;
            this.RemoveUser_button.Click += new System.EventHandler(this.RemoveUser_button_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Учетные записи";
            // 
            // Cause_Box
            // 
            this.Cause_Box.Location = new System.Drawing.Point(12, 263);
            this.Cause_Box.Multiline = true;
            this.Cause_Box.Name = "Cause_Box";
            this.Cause_Box.Size = new System.Drawing.Size(468, 82);
            this.Cause_Box.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 247);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(113, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Причина блокировки";
            // 
            // Block_button
            // 
            this.Block_button.Location = new System.Drawing.Point(15, 358);
            this.Block_button.Name = "Block_button";
            this.Block_button.Size = new System.Drawing.Size(108, 35);
            this.Block_button.TabIndex = 6;
            this.Block_button.Text = "Блокировать";
            this.Block_button.UseVisualStyleBackColor = true;
            this.Block_button.Click += new System.EventHandler(this.Block_button_Click);
            // 
            // Close_button
            // 
            this.Close_button.Location = new System.Drawing.Point(372, 358);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(108, 35);
            this.Close_button.TabIndex = 7;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // AddAll_button
            // 
            this.AddAll_button.Location = new System.Drawing.Point(375, 196);
            this.AddAll_button.Name = "AddAll_button";
            this.AddAll_button.Size = new System.Drawing.Size(108, 23);
            this.AddAll_button.TabIndex = 8;
            this.AddAll_button.Text = "Добавить всех";
            this.AddAll_button.UseVisualStyleBackColor = true;
            this.AddAll_button.Click += new System.EventHandler(this.AddAll_button_Click);
            // 
            // UnBlock_button
            // 
            this.UnBlock_button.Location = new System.Drawing.Point(204, 358);
            this.UnBlock_button.Name = "UnBlock_button";
            this.UnBlock_button.Size = new System.Drawing.Size(108, 35);
            this.UnBlock_button.TabIndex = 9;
            this.UnBlock_button.Text = "Разблокировать";
            this.UnBlock_button.UseVisualStyleBackColor = true;
            this.UnBlock_button.Click += new System.EventHandler(this.UnBlock_button_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Login
            // 
            this.Login.HeaderText = "Имя";
            this.Login.Name = "Login";
            this.Login.ReadOnly = true;
            // 
            // UType
            // 
            this.UType.HeaderText = "Тип";
            this.UType.Name = "UType";
            this.UType.ReadOnly = true;
            // 
            // EnabledToUse
            // 
            this.EnabledToUse.HeaderText = "Блок";
            this.EnabledToUse.Name = "EnabledToUse";
            this.EnabledToUse.ReadOnly = true;
            // 
            // IsOnline
            // 
            this.IsOnline.HeaderText = "В сети";
            this.IsOnline.Name = "IsOnline";
            this.IsOnline.ReadOnly = true;
            // 
            // UsersBlock_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(495, 405);
            this.Controls.Add(this.UnBlock_button);
            this.Controls.Add(this.AddAll_button);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Block_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Cause_Box);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RemoveUser_button);
            this.Controls.Add(this.AddUser_button);
            this.Controls.Add(this.User_Grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UsersBlock_Form";
            this.Text = "Блокировка пользователей";
            ((System.ComponentModel.ISupportInitialize)(this.User_Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView User_Grid;
        private System.Windows.Forms.Button AddUser_button;
        private System.Windows.Forms.Button RemoveUser_button;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Cause_Box;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button Block_button;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button AddAll_button;
        private System.Windows.Forms.Button UnBlock_button;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Login;
        private System.Windows.Forms.DataGridViewTextBoxColumn UType;
        private System.Windows.Forms.DataGridViewTextBoxColumn EnabledToUse;
        private System.Windows.Forms.DataGridViewTextBoxColumn IsOnline;
    }
}