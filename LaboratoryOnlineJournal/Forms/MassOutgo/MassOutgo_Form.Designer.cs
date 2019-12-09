namespace LaboratoryOnlineJournal
{
    partial class MassOutgo_Form
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
            this.Month_combo = new System.Windows.Forms.ComboBox();
            this.Quartal_combo = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Show_button = new System.Windows.Forms.Button();
            this.Close_button = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.Podr_combo = new System.Windows.Forms.ComboBox();
            this.Year_button = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.OType_combo = new System.Windows.Forms.ComboBox();
            this.From_Picker = new System.Windows.Forms.DateTimePicker();
            this.To_Picker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Existence_label = new System.Windows.Forms.Label();
            this.CreateNew_check = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Month_combo
            // 
            this.Month_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Month_combo.FormattingEnabled = true;
            this.Month_combo.Location = new System.Drawing.Point(183, 81);
            this.Month_combo.Name = "Month_combo";
            this.Month_combo.Size = new System.Drawing.Size(121, 21);
            this.Month_combo.TabIndex = 0;
            this.Month_combo.SelectedIndexChanged += new System.EventHandler(this.Month_combo_SelectedIndexChanged_1);
            // 
            // Quartal_combo
            // 
            this.Quartal_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Quartal_combo.FormattingEnabled = true;
            this.Quartal_combo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.Quartal_combo.Location = new System.Drawing.Point(183, 137);
            this.Quartal_combo.Name = "Quartal_combo";
            this.Quartal_combo.Size = new System.Drawing.Size(121, 21);
            this.Quartal_combo.TabIndex = 1;
            this.Quartal_combo.SelectedIndexChanged += new System.EventHandler(this.Quartal_combo_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(183, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Месяцы";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(183, 114);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 20);
            this.label4.TabIndex = 7;
            this.label4.Text = "Кварталы";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Show_button
            // 
            this.Show_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Show_button.Location = new System.Drawing.Point(6, 253);
            this.Show_button.Name = "Show_button";
            this.Show_button.Size = new System.Drawing.Size(121, 32);
            this.Show_button.TabIndex = 8;
            this.Show_button.Text = "Вывести";
            this.Show_button.UseVisualStyleBackColor = true;
            this.Show_button.Click += new System.EventHandler(this.Show_button_Click);
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(183, 253);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(121, 32);
            this.Close_button.TabIndex = 9;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(6, 58);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Сооружения";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Podr_combo
            // 
            this.Podr_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Podr_combo.FormattingEnabled = true;
            this.Podr_combo.Location = new System.Drawing.Point(6, 81);
            this.Podr_combo.Name = "Podr_combo";
            this.Podr_combo.Size = new System.Drawing.Size(121, 21);
            this.Podr_combo.TabIndex = 10;
            this.Podr_combo.SelectedIndexChanged += new System.EventHandler(this.CheckExist);
            // 
            // Year_button
            // 
            this.Year_button.Enabled = false;
            this.Year_button.Location = new System.Drawing.Point(183, 164);
            this.Year_button.Name = "Year_button";
            this.Year_button.Size = new System.Drawing.Size(88, 32);
            this.Year_button.TabIndex = 12;
            this.Year_button.Text = "Весь год";
            this.Year_button.UseVisualStyleBackColor = true;
            this.Year_button.Click += new System.EventHandler(this.Year_button_Click);
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(6, 114);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Тип объекта";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OType_combo
            // 
            this.OType_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OType_combo.FormattingEnabled = true;
            this.OType_combo.Location = new System.Drawing.Point(6, 137);
            this.OType_combo.Name = "OType_combo";
            this.OType_combo.Size = new System.Drawing.Size(121, 21);
            this.OType_combo.TabIndex = 13;
            this.OType_combo.SelectedIndexChanged += new System.EventHandler(this.CheckExist);
            // 
            // From_Picker
            // 
            this.From_Picker.CustomFormat = "MMMM yyyy";
            this.From_Picker.Enabled = false;
            this.From_Picker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.From_Picker.Location = new System.Drawing.Point(73, 9);
            this.From_Picker.Name = "From_Picker";
            this.From_Picker.Size = new System.Drawing.Size(117, 20);
            this.From_Picker.TabIndex = 2;
            this.From_Picker.ValueChanged += new System.EventHandler(this.From_Picker_ValueChanged);
            // 
            // To_Picker
            // 
            this.To_Picker.CustomFormat = "MMMM yyyy";
            this.To_Picker.Enabled = false;
            this.To_Picker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.To_Picker.Location = new System.Drawing.Point(73, 35);
            this.To_Picker.Name = "To_Picker";
            this.To_Picker.Size = new System.Drawing.Size(117, 20);
            this.To_Picker.TabIndex = 4;
            this.To_Picker.ValueChanged += new System.EventHandler(this.To_Picker_ValueChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(9, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "До";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "От";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Existence_label
            // 
            this.Existence_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Existence_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Existence_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Existence_label.Location = new System.Drawing.Point(9, 199);
            this.Existence_label.Name = "Existence_label";
            this.Existence_label.Size = new System.Drawing.Size(295, 20);
            this.Existence_label.TabIndex = 15;
            this.Existence_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // CreateNew_check
            // 
            this.CreateNew_check.AutoSize = true;
            this.CreateNew_check.Location = new System.Drawing.Point(9, 230);
            this.CreateNew_check.Name = "CreateNew_check";
            this.CreateNew_check.Size = new System.Drawing.Size(129, 17);
            this.CreateNew_check.TabIndex = 16;
            this.CreateNew_check.Text = "Перезаписать отчет";
            this.CreateNew_check.UseVisualStyleBackColor = true;
            // 
            // MassOutgo_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(310, 294);
            this.Controls.Add(this.CreateNew_check);
            this.Controls.Add(this.Existence_label);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.OType_combo);
            this.Controls.Add(this.Year_button);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Podr_combo);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Show_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.To_Picker);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.From_Picker);
            this.Controls.Add(this.Quartal_combo);
            this.Controls.Add(this.Month_combo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(326, 299);
            this.Name = "MassOutgo_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Расчет массы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Month_combo;
        private System.Windows.Forms.ComboBox Quartal_combo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Show_button;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox Podr_combo;
        private System.Windows.Forms.Button Year_button;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox OType_combo;
        private System.Windows.Forms.DateTimePicker From_Picker;
        private System.Windows.Forms.DateTimePicker To_Picker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Existence_label;
        private System.Windows.Forms.CheckBox CreateNew_check;

    }
}