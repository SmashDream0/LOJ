namespace LaboratoryOnlineJournal
{
    partial class PSGMethods_Form
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
            this.Income_Box = new System.Windows.Forms.TextBox();
            this.Outgo_Box = new System.Windows.Forms.TextBox();
            this.Income_label = new System.Windows.Forms.Label();
            this.Outgo_label = new System.Windows.Forms.Label();
            this.Save_button = new System.Windows.Forms.Button();
            this.Close_button = new System.Windows.Forms.Button();
            this.IncomeD_label = new System.Windows.Forms.Label();
            this.OutgoD_label = new System.Windows.Forms.Label();
            this.DelIncome_button = new System.Windows.Forms.Button();
            this.DelOutgo_button = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.IncomePeople_label = new System.Windows.Forms.Label();
            this.OutgoPeople_label = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Income_Box
            // 
            this.Income_Box.Location = new System.Drawing.Point(113, 9);
            this.Income_Box.Name = "Income_Box";
            this.Income_Box.Size = new System.Drawing.Size(421, 20);
            this.Income_Box.TabIndex = 0;
            // 
            // Outgo_Box
            // 
            this.Outgo_Box.Location = new System.Drawing.Point(113, 35);
            this.Outgo_Box.Name = "Outgo_Box";
            this.Outgo_Box.Size = new System.Drawing.Size(421, 20);
            this.Outgo_Box.TabIndex = 1;
            // 
            // Income_label
            // 
            this.Income_label.Location = new System.Drawing.Point(12, 9);
            this.Income_label.Name = "Income_label";
            this.Income_label.Size = new System.Drawing.Size(100, 20);
            this.Income_label.TabIndex = 2;
            this.Income_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Outgo_label
            // 
            this.Outgo_label.Location = new System.Drawing.Point(12, 34);
            this.Outgo_label.Name = "Outgo_label";
            this.Outgo_label.Size = new System.Drawing.Size(100, 20);
            this.Outgo_label.TabIndex = 3;
            this.Outgo_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Save_button
            // 
            this.Save_button.Location = new System.Drawing.Point(11, 73);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(97, 29);
            this.Save_button.TabIndex = 4;
            this.Save_button.Text = "Сохранить";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Close_button
            // 
            this.Close_button.Location = new System.Drawing.Point(855, 73);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(97, 29);
            this.Close_button.TabIndex = 5;
            this.Close_button.Text = "Отмена";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // IncomeD_label
            // 
            this.IncomeD_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IncomeD_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IncomeD_label.Location = new System.Drawing.Point(748, 8);
            this.IncomeD_label.Name = "IncomeD_label";
            this.IncomeD_label.Size = new System.Drawing.Size(134, 20);
            this.IncomeD_label.TabIndex = 6;
            this.IncomeD_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // OutgoD_label
            // 
            this.OutgoD_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutgoD_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OutgoD_label.Location = new System.Drawing.Point(748, 33);
            this.OutgoD_label.Name = "OutgoD_label";
            this.OutgoD_label.Size = new System.Drawing.Size(134, 20);
            this.OutgoD_label.TabIndex = 7;
            this.OutgoD_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // DelIncome_button
            // 
            this.DelIncome_button.Location = new System.Drawing.Point(888, 8);
            this.DelIncome_button.Name = "DelIncome_button";
            this.DelIncome_button.Size = new System.Drawing.Size(74, 20);
            this.DelIncome_button.TabIndex = 8;
            this.DelIncome_button.Text = "удалить";
            this.DelIncome_button.UseVisualStyleBackColor = true;
            this.DelIncome_button.Click += new System.EventHandler(this.DelIncome_button_Click);
            // 
            // DelOutgo_button
            // 
            this.DelOutgo_button.Location = new System.Drawing.Point(888, 34);
            this.DelOutgo_button.Name = "DelOutgo_button";
            this.DelOutgo_button.Size = new System.Drawing.Size(74, 20);
            this.DelOutgo_button.TabIndex = 9;
            this.DelOutgo_button.Text = "удалить";
            this.DelOutgo_button.UseVisualStyleBackColor = true;
            this.DelOutgo_button.Click += new System.EventHandler(this.DelOutgo_button_Click);
            // 
            // IncomePeople_label
            // 
            this.IncomePeople_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.IncomePeople_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.IncomePeople_label.Location = new System.Drawing.Point(540, 8);
            this.IncomePeople_label.Name = "IncomePeople_label";
            this.IncomePeople_label.Size = new System.Drawing.Size(202, 20);
            this.IncomePeople_label.TabIndex = 10;
            this.IncomePeople_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.IncomePeople_label.Click += new System.EventHandler(this.IncomePeople_label_Click);
            // 
            // OutgoPeople_label
            // 
            this.OutgoPeople_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.OutgoPeople_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.OutgoPeople_label.Location = new System.Drawing.Point(540, 34);
            this.OutgoPeople_label.Name = "OutgoPeople_label";
            this.OutgoPeople_label.Size = new System.Drawing.Size(202, 20);
            this.OutgoPeople_label.TabIndex = 11;
            this.OutgoPeople_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.OutgoPeople_label.Click += new System.EventHandler(this.OutgoPeople_label_Click);
            // 
            // PSGMethods_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 109);
            this.Controls.Add(this.OutgoPeople_label);
            this.Controls.Add(this.IncomePeople_label);
            this.Controls.Add(this.DelOutgo_button);
            this.Controls.Add(this.DelIncome_button);
            this.Controls.Add(this.OutgoD_label);
            this.Controls.Add(this.IncomeD_label);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Outgo_label);
            this.Controls.Add(this.Income_label);
            this.Controls.Add(this.Outgo_Box);
            this.Controls.Add(this.Income_Box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PSGMethods_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Методы";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Income_Box;
        private System.Windows.Forms.TextBox Outgo_Box;
        private System.Windows.Forms.Label Income_label;
        private System.Windows.Forms.Label Outgo_label;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label IncomeD_label;
        private System.Windows.Forms.Label OutgoD_label;
        private System.Windows.Forms.Button DelIncome_button;
        private System.Windows.Forms.Button DelOutgo_button;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label IncomePeople_label;
        private System.Windows.Forms.Label OutgoPeople_label;
    }
}