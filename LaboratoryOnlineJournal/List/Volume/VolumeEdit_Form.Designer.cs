namespace LaboratoryOnlineJournal
{
    partial class VolumeEdit_Form
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
            this.Volumes_panel = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(633, 425);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(97, 28);
            this.Close_button.TabIndex = 0;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Volumes_panel
            // 
            this.Volumes_panel.AutoScroll = true;
            this.Volumes_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Volumes_panel.Location = new System.Drawing.Point(12, 13);
            this.Volumes_panel.Name = "Volumes_panel";
            this.Volumes_panel.Size = new System.Drawing.Size(717, 406);
            this.Volumes_panel.TabIndex = 2;
            // 
            // VolumeEdit_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 461);
            this.Controls.Add(this.Volumes_panel);
            this.Controls.Add(this.Close_button);
            this.Name = "VolumeEdit_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактировать объёмы";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.VolumeEdit_Form_FormClosed);
            this.Resize += new System.EventHandler(this.VolumeEdit_Form_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Panel Volumes_panel;
    }
}