namespace LaboratoryOnlineJournal
{
    partial class DB_Form
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
            this.Close_button = new System.Windows.Forms.Button();
            this.Update_button = new System.Windows.Forms.Button();
            this.SaveUpdate_button = new System.Windows.Forms.Button();
            this.SendMessage_button = new System.Windows.Forms.Button();
            this.LoadUpdate_button = new System.Windows.Forms.Button();
            this.ForceLoad_check = new System.Windows.Forms.CheckBox();
            this.IncrementLimit_button = new System.Windows.Forms.Button();
            this.Update_label = new System.Windows.Forms.Label();
            this.Pass_Box = new System.Windows.Forms.TextBox();
            this.UpdatePeriod_Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Period_combo = new System.Windows.Forms.ComboBox();
            this.Synch_combo = new System.Windows.Forms.ComboBox();
            this.AddSynch_button = new System.Windows.Forms.Button();
            this.SendProgram_button = new System.Windows.Forms.Button();
            this.ShowCrypted_button = new System.Windows.Forms.Button();
            this.CheckAsCurrentSPool_button = new System.Windows.Forms.Button();
            this.Settings_panel = new System.Windows.Forms.Panel();
            this.PodrCode_label = new System.Windows.Forms.Label();
            this.ForceUser_combo = new System.Windows.Forms.ComboBox();
            this.Hide_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.UTable_Grid = new System.Windows.Forms.DataGridView();
            this.SelectWhat_button = new System.Windows.Forms.Button();
            this.SelectWho_button = new System.Windows.Forms.Button();
            this.ShowSelection_button = new System.Windows.Forms.Button();
            this.Settings_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UTable_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(372, 663);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(73, 33);
            this.Close_button.TabIndex = 0;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Update_button
            // 
            this.Update_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Update_button.Location = new System.Drawing.Point(256, 199);
            this.Update_button.Name = "Update_button";
            this.Update_button.Size = new System.Drawing.Size(192, 27);
            this.Update_button.TabIndex = 3;
            this.Update_button.Text = "Проверить обновление";
            this.Update_button.UseVisualStyleBackColor = true;
            this.Update_button.Click += new System.EventHandler(this.Update_button_Click);
            // 
            // SaveUpdate_button
            // 
            this.SaveUpdate_button.Location = new System.Drawing.Point(7, 166);
            this.SaveUpdate_button.Name = "SaveUpdate_button";
            this.SaveUpdate_button.Size = new System.Drawing.Size(192, 31);
            this.SaveUpdate_button.TabIndex = 4;
            this.SaveUpdate_button.Text = "Выгрузить обновление";
            this.SaveUpdate_button.UseVisualStyleBackColor = true;
            this.SaveUpdate_button.Click += new System.EventHandler(this.SaveUpdate_button_Click);
            // 
            // SendMessage_button
            // 
            this.SendMessage_button.Location = new System.Drawing.Point(7, 198);
            this.SendMessage_button.Name = "SendMessage_button";
            this.SendMessage_button.Size = new System.Drawing.Size(192, 31);
            this.SendMessage_button.TabIndex = 5;
            this.SendMessage_button.Text = "Выслать обновление";
            this.SendMessage_button.UseVisualStyleBackColor = true;
            this.SendMessage_button.Click += new System.EventHandler(this.SendMessage_button_Click);
            // 
            // LoadUpdate_button
            // 
            this.LoadUpdate_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.LoadUpdate_button.Location = new System.Drawing.Point(256, 166);
            this.LoadUpdate_button.Name = "LoadUpdate_button";
            this.LoadUpdate_button.Size = new System.Drawing.Size(192, 31);
            this.LoadUpdate_button.TabIndex = 6;
            this.LoadUpdate_button.Text = "Загрузить обновление";
            this.LoadUpdate_button.UseVisualStyleBackColor = true;
            this.LoadUpdate_button.Click += new System.EventHandler(this.LoadUpdate_button_Click);
            // 
            // ForceLoad_check
            // 
            this.ForceLoad_check.AutoSize = true;
            this.ForceLoad_check.Location = new System.Drawing.Point(3, 3);
            this.ForceLoad_check.Name = "ForceLoad_check";
            this.ForceLoad_check.Size = new System.Drawing.Size(144, 17);
            this.ForceLoad_check.TabIndex = 7;
            this.ForceLoad_check.Text = "Форсировать загрузку";
            this.ForceLoad_check.UseVisualStyleBackColor = true;
            // 
            // IncrementLimit_button
            // 
            this.IncrementLimit_button.Location = new System.Drawing.Point(0, 26);
            this.IncrementLimit_button.Name = "IncrementLimit_button";
            this.IncrementLimit_button.Size = new System.Drawing.Size(192, 31);
            this.IncrementLimit_button.TabIndex = 9;
            this.IncrementLimit_button.Text = "Лимит записей";
            this.IncrementLimit_button.UseVisualStyleBackColor = true;
            this.IncrementLimit_button.Click += new System.EventHandler(this.IncrementLimit_button_Click);
            // 
            // Update_label
            // 
            this.Update_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Update_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Update_label.Location = new System.Drawing.Point(7, 29);
            this.Update_label.Name = "Update_label";
            this.Update_label.Size = new System.Drawing.Size(441, 63);
            this.Update_label.TabIndex = 10;
            // 
            // Pass_Box
            // 
            this.Pass_Box.Location = new System.Drawing.Point(7, 287);
            this.Pass_Box.Name = "Pass_Box";
            this.Pass_Box.Size = new System.Drawing.Size(192, 20);
            this.Pass_Box.TabIndex = 8;
            this.Pass_Box.UseSystemPasswordChar = true;
            this.Pass_Box.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Pass_Box_KeyDown);
            // 
            // UpdatePeriod_Box
            // 
            this.UpdatePeriod_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UpdatePeriod_Box.Location = new System.Drawing.Point(123, 143);
            this.UpdatePeriod_Box.Name = "UpdatePeriod_Box";
            this.UpdatePeriod_Box.Size = new System.Drawing.Size(325, 20);
            this.UpdatePeriod_Box.TabIndex = 11;
            this.UpdatePeriod_Box.TextChanged += new System.EventHandler(this.UpdatePeriod_Box_TextChanged);
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(7, 143);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 20);
            this.label1.TabIndex = 12;
            this.label1.Text = "Период обновлений";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(7, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(441, 23);
            this.label2.TabIndex = 13;
            this.label2.Text = "Список выгружаемых записей:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(7, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 48);
            this.label4.TabIndex = 15;
            this.label4.Text = "Синхронизация:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Period_combo
            // 
            this.Period_combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Period_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Period_combo.FormattingEnabled = true;
            this.Period_combo.Location = new System.Drawing.Point(123, 95);
            this.Period_combo.Name = "Period_combo";
            this.Period_combo.Size = new System.Drawing.Size(246, 21);
            this.Period_combo.TabIndex = 16;
            this.Period_combo.SelectedIndexChanged += new System.EventHandler(this.Period_combo_SelectedIndexChanged);
            // 
            // Synch_combo
            // 
            this.Synch_combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Synch_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Synch_combo.Font = new System.Drawing.Font("DejaVu Sans Mono", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Synch_combo.FormattingEnabled = true;
            this.Synch_combo.Location = new System.Drawing.Point(123, 116);
            this.Synch_combo.Name = "Synch_combo";
            this.Synch_combo.Size = new System.Drawing.Size(246, 18);
            this.Synch_combo.TabIndex = 17;
            this.Synch_combo.SelectedIndexChanged += new System.EventHandler(this.Synch_combo_SelectedIndexChanged);
            // 
            // AddSynch_button
            // 
            this.AddSynch_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddSynch_button.Location = new System.Drawing.Point(375, 95);
            this.AddSynch_button.Name = "AddSynch_button";
            this.AddSynch_button.Size = new System.Drawing.Size(73, 42);
            this.AddSynch_button.TabIndex = 18;
            this.AddSynch_button.Text = "Создать";
            this.AddSynch_button.UseVisualStyleBackColor = true;
            this.AddSynch_button.Click += new System.EventHandler(this.AddSynch_button_Click);
            // 
            // SendProgram_button
            // 
            this.SendProgram_button.Location = new System.Drawing.Point(7, 235);
            this.SendProgram_button.Name = "SendProgram_button";
            this.SendProgram_button.Size = new System.Drawing.Size(192, 31);
            this.SendProgram_button.TabIndex = 19;
            this.SendProgram_button.Text = "Выслать копию программы";
            this.SendProgram_button.UseVisualStyleBackColor = true;
            this.SendProgram_button.Click += new System.EventHandler(this.SendProgram_button_Click);
            // 
            // ShowCrypted_button
            // 
            this.ShowCrypted_button.Location = new System.Drawing.Point(0, 63);
            this.ShowCrypted_button.Name = "ShowCrypted_button";
            this.ShowCrypted_button.Size = new System.Drawing.Size(192, 31);
            this.ShowCrypted_button.TabIndex = 20;
            this.ShowCrypted_button.Text = "Посмотреть содержимое";
            this.ShowCrypted_button.UseVisualStyleBackColor = true;
            this.ShowCrypted_button.Click += new System.EventHandler(this.ShowCrypted_button_Click);
            // 
            // CheckAsCurrentSPool_button
            // 
            this.CheckAsCurrentSPool_button.Location = new System.Drawing.Point(0, 144);
            this.CheckAsCurrentSPool_button.Name = "CheckAsCurrentSPool_button";
            this.CheckAsCurrentSPool_button.Size = new System.Drawing.Size(192, 34);
            this.CheckAsCurrentSPool_button.TabIndex = 21;
            this.CheckAsCurrentSPool_button.Text = "Пометить текущий период текущей синхронизацией";
            this.CheckAsCurrentSPool_button.UseVisualStyleBackColor = true;
            this.CheckAsCurrentSPool_button.Click += new System.EventHandler(this.CheckAsCurrentSPool_button_Click);
            // 
            // Settings_panel
            // 
            this.Settings_panel.Controls.Add(this.PodrCode_label);
            this.Settings_panel.Controls.Add(this.ForceUser_combo);
            this.Settings_panel.Controls.Add(this.Hide_button);
            this.Settings_panel.Controls.Add(this.label3);
            this.Settings_panel.Controls.Add(this.UTable_Grid);
            this.Settings_panel.Controls.Add(this.SelectWhat_button);
            this.Settings_panel.Controls.Add(this.SelectWho_button);
            this.Settings_panel.Controls.Add(this.ShowSelection_button);
            this.Settings_panel.Controls.Add(this.CheckAsCurrentSPool_button);
            this.Settings_panel.Controls.Add(this.ForceLoad_check);
            this.Settings_panel.Controls.Add(this.ShowCrypted_button);
            this.Settings_panel.Controls.Add(this.IncrementLimit_button);
            this.Settings_panel.Location = new System.Drawing.Point(7, 313);
            this.Settings_panel.Name = "Settings_panel";
            this.Settings_panel.Size = new System.Drawing.Size(346, 383);
            this.Settings_panel.TabIndex = 22;
            // 
            // PodrCode_label
            // 
            this.PodrCode_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PodrCode_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PodrCode_label.Location = new System.Drawing.Point(198, 100);
            this.PodrCode_label.Name = "PodrCode_label";
            this.PodrCode_label.Size = new System.Drawing.Size(145, 31);
            this.PodrCode_label.TabIndex = 23;
            this.PodrCode_label.Text = "Код подразделения:";
            this.PodrCode_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ForceUser_combo
            // 
            this.ForceUser_combo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ForceUser_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ForceUser_combo.FormattingEnabled = true;
            this.ForceUser_combo.Location = new System.Drawing.Point(198, 3);
            this.ForceUser_combo.Name = "ForceUser_combo";
            this.ForceUser_combo.Size = new System.Drawing.Size(145, 21);
            this.ForceUser_combo.TabIndex = 23;
            this.ForceUser_combo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ForceUser_combo_KeyDown);
            // 
            // Hide_button
            // 
            this.Hide_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Hide_button.Location = new System.Drawing.Point(198, 144);
            this.Hide_button.Name = "Hide_button";
            this.Hide_button.Size = new System.Drawing.Size(145, 33);
            this.Hide_button.TabIndex = 23;
            this.Hide_button.Text = "Скрыть";
            this.Hide_button.UseVisualStyleBackColor = true;
            this.Hide_button.Click += new System.EventHandler(this.Hide_button_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(3, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(340, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Выгружаемые таблицы";
            // 
            // UTable_Grid
            // 
            this.UTable_Grid.AllowUserToAddRows = false;
            this.UTable_Grid.AllowUserToDeleteRows = false;
            this.UTable_Grid.AllowUserToOrderColumns = true;
            this.UTable_Grid.AllowUserToResizeColumns = false;
            this.UTable_Grid.AllowUserToResizeRows = false;
            this.UTable_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.UTable_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UTable_Grid.Location = new System.Drawing.Point(0, 205);
            this.UTable_Grid.Name = "UTable_Grid";
            this.UTable_Grid.RowHeadersVisible = false;
            this.UTable_Grid.Size = new System.Drawing.Size(343, 174);
            this.UTable_Grid.TabIndex = 25;
            this.UTable_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.UTable_Grid_CellValueNeeded);
            this.UTable_Grid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.UTable_Grid_CellValuePushed);
            // 
            // SelectWhat_button
            // 
            this.SelectWhat_button.Location = new System.Drawing.Point(198, 63);
            this.SelectWhat_button.Name = "SelectWhat_button";
            this.SelectWhat_button.Size = new System.Drawing.Size(145, 31);
            this.SelectWhat_button.TabIndex = 24;
            this.SelectWhat_button.Text = "Что отослать";
            this.SelectWhat_button.UseVisualStyleBackColor = true;
            this.SelectWhat_button.Click += new System.EventHandler(this.SelectWhat_button_Click);
            // 
            // SelectWho_button
            // 
            this.SelectWho_button.Location = new System.Drawing.Point(198, 26);
            this.SelectWho_button.Name = "SelectWho_button";
            this.SelectWho_button.Size = new System.Drawing.Size(145, 31);
            this.SelectWho_button.TabIndex = 23;
            this.SelectWho_button.Text = "Кому отослать";
            this.SelectWho_button.UseVisualStyleBackColor = true;
            this.SelectWho_button.Click += new System.EventHandler(this.SelectWho_button_Click);
            // 
            // ShowSelection_button
            // 
            this.ShowSelection_button.Location = new System.Drawing.Point(0, 100);
            this.ShowSelection_button.Name = "ShowSelection_button";
            this.ShowSelection_button.Size = new System.Drawing.Size(192, 31);
            this.ShowSelection_button.TabIndex = 22;
            this.ShowSelection_button.Text = "Посмотреть выбранный период";
            this.ShowSelection_button.UseVisualStyleBackColor = true;
            this.ShowSelection_button.Click += new System.EventHandler(this.ShowSelection_button_Click);
            // 
            // DB_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 701);
            this.Controls.Add(this.Pass_Box);
            this.Controls.Add(this.Settings_panel);
            this.Controls.Add(this.SendProgram_button);
            this.Controls.Add(this.AddSynch_button);
            this.Controls.Add(this.Synch_combo);
            this.Controls.Add(this.Period_combo);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UpdatePeriod_Box);
            this.Controls.Add(this.Update_label);
            this.Controls.Add(this.LoadUpdate_button);
            this.Controls.Add(this.SendMessage_button);
            this.Controls.Add(this.SaveUpdate_button);
            this.Controls.Add(this.Update_button);
            this.Controls.Add(this.Close_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DB_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "База данных";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.UpdateBD_Form_FormClosed);
            this.Settings_panel.ResumeLayout(false);
            this.Settings_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UTable_Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button Update_button;
        private System.Windows.Forms.Button SaveUpdate_button;
        private System.Windows.Forms.Button SendMessage_button;
        private System.Windows.Forms.Button LoadUpdate_button;
        private System.Windows.Forms.CheckBox ForceLoad_check;
        private System.Windows.Forms.Button IncrementLimit_button;
        private System.Windows.Forms.Label Update_label;
        private System.Windows.Forms.TextBox Pass_Box;
        private System.Windows.Forms.TextBox UpdatePeriod_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Period_combo;
        private System.Windows.Forms.ComboBox Synch_combo;
        private System.Windows.Forms.Button AddSynch_button;
        private System.Windows.Forms.Button SendProgram_button;
        private System.Windows.Forms.Button ShowCrypted_button;
        private System.Windows.Forms.Button CheckAsCurrentSPool_button;
        private System.Windows.Forms.Panel Settings_panel;
        private System.Windows.Forms.Button ShowSelection_button;
        private System.Windows.Forms.Button SelectWhat_button;
        private System.Windows.Forms.Button SelectWho_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataGridView UTable_Grid;
        private System.Windows.Forms.Button Hide_button;
        private System.Windows.Forms.ComboBox ForceUser_combo;
        private System.Windows.Forms.Label PodrCode_label;
    }
}