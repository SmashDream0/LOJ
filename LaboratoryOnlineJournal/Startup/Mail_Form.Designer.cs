namespace LaboratoryOnlineJournal
{
    partial class Mail_Form
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
            this.Send_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.Descryption_label = new System.Windows.Forms.Label();
            this.Repair_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Send_button
            // 
            this.Send_button.Location = new System.Drawing.Point(13, 132);
            this.Send_button.Name = "Send_button";
            this.Send_button.Size = new System.Drawing.Size(114, 41);
            this.Send_button.TabIndex = 0;
            this.Send_button.Text = "Изменить пароль";
            this.Send_button.UseVisualStyleBackColor = true;
            this.Send_button.Click += new System.EventHandler(this.Send_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(300, 132);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(114, 41);
            this.Cancel_button.TabIndex = 1;
            this.Cancel_button.Text = "Отмена";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // Descryption_label
            // 
            this.Descryption_label.Location = new System.Drawing.Point(13, 13);
            this.Descryption_label.Name = "Descryption_label";
            this.Descryption_label.Size = new System.Drawing.Size(401, 116);
            this.Descryption_label.TabIndex = 2;
            // 
            // Repair_button
            // 
            this.Repair_button.Location = new System.Drawing.Point(156, 132);
            this.Repair_button.Name = "Repair_button";
            this.Repair_button.Size = new System.Drawing.Size(114, 41);
            this.Repair_button.TabIndex = 3;
            this.Repair_button.Text = "Сбросить учетную запись";
            this.Repair_button.UseVisualStyleBackColor = true;
            this.Repair_button.Click += new System.EventHandler(this.Repair_button_Click);
            // 
            // Mail_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(426, 180);
            this.Controls.Add(this.Repair_button);
            this.Controls.Add(this.Descryption_label);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Send_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Mail_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Форма отправки";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Send_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.Label Descryption_label;
        private System.Windows.Forms.Button Repair_button;
    }
}