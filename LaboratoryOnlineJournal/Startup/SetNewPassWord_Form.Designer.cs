partial class SetNewPassWord_Form
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
            this.NewPassWord_Box = new System.Windows.Forms.TextBox();
            this.RepeateNewPassWord_Box = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Continue_button = new System.Windows.Forms.Button();
            this.Cancel_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // NewPassWord_Box
            // 
            this.NewPassWord_Box.Location = new System.Drawing.Point(12, 89);
            this.NewPassWord_Box.Name = "NewPassWord_Box";
            this.NewPassWord_Box.PasswordChar = '*';
            this.NewPassWord_Box.Size = new System.Drawing.Size(210, 20);
            this.NewPassWord_Box.TabIndex = 0;
            this.NewPassWord_Box.UseSystemPasswordChar = true;
            // 
            // RepeateNewPassWord_Box
            // 
            this.RepeateNewPassWord_Box.Location = new System.Drawing.Point(12, 135);
            this.RepeateNewPassWord_Box.Name = "RepeateNewPassWord_Box";
            this.RepeateNewPassWord_Box.PasswordChar = '*';
            this.RepeateNewPassWord_Box.Size = new System.Drawing.Size(210, 20);
            this.RepeateNewPassWord_Box.TabIndex = 1;
            this.RepeateNewPassWord_Box.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Введить новый пароль";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 112);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(210, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Повторите пароль";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Continue_button
            // 
            this.Continue_button.Location = new System.Drawing.Point(12, 173);
            this.Continue_button.Name = "Continue_button";
            this.Continue_button.Size = new System.Drawing.Size(87, 32);
            this.Continue_button.TabIndex = 4;
            this.Continue_button.Text = "Продолжить";
            this.Continue_button.UseVisualStyleBackColor = true;
            this.Continue_button.Click += new System.EventHandler(this.Continue_button_Click);
            // 
            // Cancel_button
            // 
            this.Cancel_button.Location = new System.Drawing.Point(135, 173);
            this.Cancel_button.Name = "Cancel_button";
            this.Cancel_button.Size = new System.Drawing.Size(87, 32);
            this.Cancel_button.TabIndex = 5;
            this.Cancel_button.Text = "Закрыть";
            this.Cancel_button.UseVisualStyleBackColor = true;
            this.Cancel_button.Click += new System.EventHandler(this.Cancel_button_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(13, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(213, 42);
            this.label3.TabIndex = 6;
            this.label3.Text = "Введите новый пароль для вашей учетной записи дважды, чтобы исключить возможность" +
    " опечатки.";
            // 
            // SetNewPassWord_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 209);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Cancel_button);
            this.Controls.Add(this.Continue_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RepeateNewPassWord_Box);
            this.Controls.Add(this.NewPassWord_Box);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SetNewPassWord_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Ввод нового пароля";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.TextBox NewPassWord_Box;
    private System.Windows.Forms.TextBox RepeateNewPassWord_Box;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Button Continue_button;
    private System.Windows.Forms.Button Cancel_button;
    private System.Windows.Forms.Label label3;
}