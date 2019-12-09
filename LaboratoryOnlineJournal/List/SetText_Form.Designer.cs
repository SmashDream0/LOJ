namespace LaboratoryOnlineJournal
{
    partial class SetText_Form
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
            this.Text_Box = new System.Windows.Forms.TextBox();
            this.Continue_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Text_Box
            // 
            this.Text_Box.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Text_Box.Location = new System.Drawing.Point(12, 22);
            this.Text_Box.Name = "Text_Box";
            this.Text_Box.Size = new System.Drawing.Size(348, 20);
            this.Text_Box.TabIndex = 0;
            // 
            // Continue_button
            // 
            this.Continue_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Continue_button.Location = new System.Drawing.Point(12, 48);
            this.Continue_button.Name = "Continue_button";
            this.Continue_button.Size = new System.Drawing.Size(91, 28);
            this.Continue_button.TabIndex = 1;
            this.Continue_button.Text = "Продолжить";
            this.Continue_button.UseVisualStyleBackColor = true;
            this.Continue_button.Click += new System.EventHandler(this.Continue_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel_button.Location = new System.Drawing.Point(269, 48);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(91, 28);
            this.Cancel_button.TabIndex = 2;
            this.Cancel_button.Text = "Отмена";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(13, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(347, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "Введите наименование";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SetText_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 78);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Continue_button);
            this.Controls.Add(this.Text_Box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetText_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Text_Box;
        private System.Windows.Forms.Button Continue_button;
        private System.Windows.Forms.Button Cancel_button;
        private System.Windows.Forms.Label label1;
    }
}