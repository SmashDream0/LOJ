namespace LaboratoryOnlineJournal
{
    partial class MErrorEdit_Form
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
            this.Diap_Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.DO_button = new System.Windows.Forms.Button();
            this.Close_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Mark_Box = new System.Windows.Forms.TextBox();
            this.Volume_label = new System.Windows.Forms.Label();
            this.Volume_Box = new System.Windows.Forms.TextBox();
            this.Norm_Box = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Percent_check = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // Diap_Box
            // 
            this.Diap_Box.Location = new System.Drawing.Point(98, 61);
            this.Diap_Box.Name = "Diap_Box";
            this.Diap_Box.Size = new System.Drawing.Size(218, 20);
            this.Diap_Box.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Location = new System.Drawing.Point(6, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Диапазон";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // DO_button
            // 
            this.DO_button.Location = new System.Drawing.Point(6, 141);
            this.DO_button.Name = "DO_button";
            this.DO_button.Size = new System.Drawing.Size(75, 32);
            this.DO_button.TabIndex = 4;
            this.DO_button.UseVisualStyleBackColor = true;
            // 
            // Close_button
            // 
            this.Close_button.Location = new System.Drawing.Point(241, 141);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(75, 32);
            this.Close_button.TabIndex = 5;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // label3
            // 
            this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(86, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Показатель";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Mark_Box
            // 
            this.Mark_Box.Location = new System.Drawing.Point(98, 9);
            this.Mark_Box.Name = "Mark_Box";
            this.Mark_Box.Size = new System.Drawing.Size(218, 20);
            this.Mark_Box.TabIndex = 0;
            // 
            // Volume_label
            // 
            this.Volume_label.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Volume_label.Location = new System.Drawing.Point(6, 87);
            this.Volume_label.Name = "Volume_label";
            this.Volume_label.Size = new System.Drawing.Size(86, 43);
            this.Volume_label.TabIndex = 9;
            this.Volume_label.Text = "Погрешность процент";
            this.Volume_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Volume_Box
            // 
            this.Volume_Box.Location = new System.Drawing.Point(98, 87);
            this.Volume_Box.Name = "Volume_Box";
            this.Volume_Box.Size = new System.Drawing.Size(218, 20);
            this.Volume_Box.TabIndex = 3;
            this.Volume_Box.TextChanged += new System.EventHandler(this.Percent_Box_TextChanged);
            // 
            // Norm_Box
            // 
            this.Norm_Box.Location = new System.Drawing.Point(98, 35);
            this.Norm_Box.Name = "Norm_Box";
            this.Norm_Box.Size = new System.Drawing.Size(218, 20);
            this.Norm_Box.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Location = new System.Drawing.Point(6, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 20);
            this.label2.TabIndex = 11;
            this.label2.Text = "Норма";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Percent_check
            // 
            this.Percent_check.AutoSize = true;
            this.Percent_check.Checked = true;
            this.Percent_check.CheckState = System.Windows.Forms.CheckState.Checked;
            this.Percent_check.Location = new System.Drawing.Point(98, 113);
            this.Percent_check.Name = "Percent_check";
            this.Percent_check.Size = new System.Drawing.Size(88, 17);
            this.Percent_check.TabIndex = 12;
            this.Percent_check.Text = "Это процент";
            this.Percent_check.UseVisualStyleBackColor = true;
            this.Percent_check.CheckedChanged += new System.EventHandler(this.Percent_check_CheckedChanged);
            // 
            // MErrorEdit_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(321, 178);
            this.Controls.Add(this.Percent_check);
            this.Controls.Add(this.Norm_Box);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Volume_label);
            this.Controls.Add(this.Volume_Box);
            this.Controls.Add(this.Mark_Box);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.DO_button);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Diap_Box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MErrorEdit_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Добавить диапазон";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Diap_Box;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button DO_button;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Mark_Box;
        private System.Windows.Forms.Label Volume_label;
        private System.Windows.Forms.TextBox Volume_Box;
        private System.Windows.Forms.TextBox Norm_Box;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox Percent_check;
    }
}