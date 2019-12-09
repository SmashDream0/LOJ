namespace LaboratoryOnlineJournal
{
    partial class MarksCompanyEdit_Form
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
                Marks.Dispose();
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
            this.Norm_button = new System.Windows.Forms.Button();
            this.Show_check = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(758, 370);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(131, 31);
            this.Close_button.TabIndex = 3;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Norm_button
            // 
            this.Norm_button.Location = new System.Drawing.Point(12, 12);
            this.Norm_button.Name = "Norm_button";
            this.Norm_button.Size = new System.Drawing.Size(90, 26);
            this.Norm_button.TabIndex = 4;
            this.Norm_button.Text = "Нормы";
            this.Norm_button.UseVisualStyleBackColor = true;
            this.Norm_button.Click += new System.EventHandler(this.Norm_button_Click);
            // 
            // Show_check
            // 
            this.Show_check.AutoSize = true;
            this.Show_check.Checked = true;
            this.Show_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Show_check.Location = new System.Drawing.Point(133, 18);
            this.Show_check.Name = "Show_check";
            this.Show_check.Size = new System.Drawing.Size(75, 17);
            this.Show_check.TabIndex = 5;
            this.Show_check.Text = "Показать";
            this.Show_check.UseVisualStyleBackColor = true;
            this.Show_check.CheckedChanged += new System.EventHandler(this.Show_check_CheckedChanged);
            // 
            // MarksCompanyEdit_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 412);
            this.Controls.Add(this.Show_check);
            this.Controls.Add(this.Norm_button);
            this.Controls.Add(this.Close_button);
            this.Name = "MarksCompanyEdit_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактировать показатели";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MarksCompanyEdit_Form_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button Norm_button;
        private System.Windows.Forms.CheckBox Show_check;
    }
}