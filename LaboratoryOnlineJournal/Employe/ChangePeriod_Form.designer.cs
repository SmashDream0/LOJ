using LaboratoryOnlineJournal;

partial class PeriodChange_Form
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
            this.Date_Picker = new System.Windows.Forms.DateTimePicker();
            this.Continue_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.Next_button = new System.Windows.Forms.Button();
            this.Previous_button = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Date_Picker
            // 
            this.Date_Picker.CustomFormat = "MMMM:yyyy";
            this.Date_Picker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.Date_Picker.Location = new System.Drawing.Point(45, 7);
            this.Date_Picker.Name = "Date_Picker";
            this.Date_Picker.Size = new System.Drawing.Size(176, 20);
            this.Date_Picker.TabIndex = 0;
            // 
            // Continue_button
            // 
            this.Continue_button.Location = new System.Drawing.Point(5, 33);
            this.Continue_button.Name = "Continue_button";
            this.Continue_button.Size = new System.Drawing.Size(80, 31);
            this.Continue_button.TabIndex = 1;
            this.Continue_button.Text = "Продолжить";
            this.Continue_button.UseVisualStyleBackColor = true;
            this.Continue_button.Click += new System.EventHandler(this.Continue_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(181, 33);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(80, 31);
            this.Cancel_button.TabIndex = 2;
            this.Cancel_button.Text = "Отменить";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // Next_button
            // 
            this.Next_button.Location = new System.Drawing.Point(227, 4);
            this.Next_button.Name = "Next_button";
            this.Next_button.Size = new System.Drawing.Size(34, 26);
            this.Next_button.TabIndex = 3;
            this.Next_button.Text = ">>";
            this.Next_button.UseVisualStyleBackColor = true;
            this.Next_button.Click += new System.EventHandler(this.Next_button_Click);
            // 
            // Previous_button
            // 
            this.Previous_button.Location = new System.Drawing.Point(5, 4);
            this.Previous_button.Name = "Previous_button";
            this.Previous_button.Size = new System.Drawing.Size(34, 26);
            this.Previous_button.TabIndex = 4;
            this.Previous_button.Text = "<<";
            this.Previous_button.UseVisualStyleBackColor = true;
            this.Previous_button.Click += new System.EventHandler(this.Previous_button_Click);
            // 
            // PeriodChange_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 68);
            this.Controls.Add(this.Previous_button);
            this.Controls.Add(this.Next_button);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Continue_button);
            this.Controls.Add(this.Date_Picker);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PeriodChange_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Смена периода";
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.DateTimePicker Date_Picker;
    private System.Windows.Forms.Button Continue_button;
    private System.Windows.Forms.Button Cancel_button;
    private System.Windows.Forms.Button Next_button;
    private System.Windows.Forms.Button Previous_button;

}