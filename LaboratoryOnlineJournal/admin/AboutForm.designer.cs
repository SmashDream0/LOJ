namespace LaboratoryOnlineJournal
{
    partial class AboutForm
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
            this.Descryption = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Descryptions_button = new System.Windows.Forms.Button();
            this.DocChanges_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Descryption
            // 
            this.Descryption.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.Descryption.Location = new System.Drawing.Point(10, 10);
            this.Descryption.Name = "Descryption";
            this.Descryption.Size = new System.Drawing.Size(355, 75);
            this.Descryption.TabIndex = 0;
            this.Descryption.Click += new System.EventHandler(this.Descryption_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(10, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(355, 35);
            this.label1.TabIndex = 1;
            this.label1.Text = "  В программе использовались свободно распространяемые библиотеки:";
            this.label1.Click += new System.EventHandler(this.Descryption_Click);
            // 
            // label2
            // 
            this.label2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(11, 125);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "NPOI: http://npoi.codeplex.com/";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(11, 150);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(180, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "IMAPX http://imapx.codeplex.com/";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.Cursor = System.Windows.Forms.Cursors.Hand;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Italic | System.Drawing.FontStyle.Underline))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(11, 175);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(290, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "MYSQL http://dev.mysql.com/downloads/connector/net/";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // Descryptions_button
            // 
            this.Descryptions_button.Location = new System.Drawing.Point(13, 207);
            this.Descryptions_button.Name = "Descryptions_button";
            this.Descryptions_button.Size = new System.Drawing.Size(352, 36);
            this.Descryptions_button.TabIndex = 5;
            this.Descryptions_button.Text = "Показать инструкции";
            this.Descryptions_button.UseVisualStyleBackColor = true;
            this.Descryptions_button.Click += new System.EventHandler(this.Descryptions_button_Click);
            // 
            // DocChanges_button
            // 
            this.DocChanges_button.Location = new System.Drawing.Point(14, 249);
            this.DocChanges_button.Name = "DocChanges_button";
            this.DocChanges_button.Size = new System.Drawing.Size(352, 36);
            this.DocChanges_button.TabIndex = 7;
            this.DocChanges_button.Text = "Показать список изменений";
            this.DocChanges_button.UseVisualStyleBackColor = true;
            this.DocChanges_button.Click += new System.EventHandler(this.DocChanges_button_Click);
            // 
            // AboutForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(376, 290);
            this.Controls.Add(this.DocChanges_button);
            this.Controls.Add(this.Descryptions_button);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Descryption);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "О программе";
            this.Load += new System.EventHandler(this.AboutForm_Load);
            this.Click += new System.EventHandler(this.Descryption_Click);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label Descryption;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Descryptions_button;
        private System.Windows.Forms.Button DocChanges_button;
    }
}