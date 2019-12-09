partial class StartupLogo_Form
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
            this.Label_label = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // Label_label
            // 
            this.Label_label.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Label_label.Image = global::LaboratoryOnlineJournal.Properties.Resources.logotip01_mini;
            this.Label_label.Location = new System.Drawing.Point(12, 9);
            this.Label_label.Name = "Label_label";
            this.Label_label.Size = new System.Drawing.Size(275, 271);
            this.Label_label.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 285);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(275, 23);
            this.label1.TabIndex = 1;
            this.label1.Text = "Загрузка ...";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 750;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // StartupLogo_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ClientSize = new System.Drawing.Size(299, 315);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Label_label);
            this.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "StartupLogo_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "StartupLogo_Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StartupLogo_Form_FormClosing);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label Label_label;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Timer timer1;
}