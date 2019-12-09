namespace LaboratoryOnlineJournal
{
    partial class TimeLessPass_Form
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
            this.Pass_Box = new System.Windows.Forms.TextBox();
            this.Continue_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Pass_Box
            // 
            this.Pass_Box.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Pass_Box.Location = new System.Drawing.Point(7, 12);
            this.Pass_Box.Name = "Pass_Box";
            this.Pass_Box.Size = new System.Drawing.Size(253, 26);
            this.Pass_Box.TabIndex = 0;
            // 
            // Continue_button
            // 
            this.Continue_button.Location = new System.Drawing.Point(7, 44);
            this.Continue_button.Name = "Continue_button";
            this.Continue_button.Size = new System.Drawing.Size(87, 26);
            this.Continue_button.TabIndex = 1;
            this.Continue_button.Text = "Продолжить";
            this.Continue_button.UseVisualStyleBackColor = true;
            this.Continue_button.Click += new System.EventHandler(this.Continue_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(173, 44);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(87, 26);
            this.Cancel_button.TabIndex = 2;
            this.Cancel_button.Text = "Отмена";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // TimeLessPass_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(272, 78);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Continue_button);
            this.Controls.Add(this.Pass_Box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "TimeLessPass_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Введите временный пароль";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Pass_Box;
        private System.Windows.Forms.Button Continue_button;
        private System.Windows.Forms.Button Cancel_button;
    }
}