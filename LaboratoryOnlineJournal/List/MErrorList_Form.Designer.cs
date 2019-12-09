namespace LaboratoryOnlineJournal
{
    partial class MErrorList_Form
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
            this.Add_button = new System.Windows.Forms.Button();
            this.Close_button = new System.Windows.Forms.Button();
            this.Delete_button = new System.Windows.Forms.Button();
            this.MError_Grid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.MError_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Add_button
            // 
            this.Add_button.Location = new System.Drawing.Point(4, 223);
            this.Add_button.Name = "Add_button";
            this.Add_button.Size = new System.Drawing.Size(75, 34);
            this.Add_button.TabIndex = 1;
            this.Add_button.Text = "Добавить";
            this.Add_button.UseVisualStyleBackColor = true;
            this.Add_button.Click += new System.EventHandler(this.Add_button_Click);
            // 
            // Close_button
            // 
            this.Close_button.Location = new System.Drawing.Point(401, 223);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(75, 34);
            this.Close_button.TabIndex = 2;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Delete_button
            // 
            this.Delete_button.Location = new System.Drawing.Point(203, 223);
            this.Delete_button.Name = "Delete_button";
            this.Delete_button.Size = new System.Drawing.Size(75, 34);
            this.Delete_button.TabIndex = 3;
            this.Delete_button.Text = "Удалить";
            this.Delete_button.UseVisualStyleBackColor = true;
            this.Delete_button.Click += new System.EventHandler(this.Delete_button_Click);
            // 
            // MError_Grid
            // 
            this.MError_Grid.AllowUserToAddRows = false;
            this.MError_Grid.AllowUserToDeleteRows = false;
            this.MError_Grid.AllowUserToResizeColumns = false;
            this.MError_Grid.AllowUserToResizeRows = false;
            this.MError_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.MError_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MError_Grid.Location = new System.Drawing.Point(4, 5);
            this.MError_Grid.MultiSelect = false;
            this.MError_Grid.Name = "MError_Grid";
            this.MError_Grid.Size = new System.Drawing.Size(472, 212);
            this.MError_Grid.TabIndex = 0;
            this.MError_Grid.VirtualMode = true;
            // 
            // MErrorList_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(479, 263);
            this.Controls.Add(this.Delete_button);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Add_button);
            this.Controls.Add(this.MError_Grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "MErrorList_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Редактировать точность";
            ((System.ComponentModel.ISupportInitialize)(this.MError_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Add_button;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button Delete_button;
        private System.Windows.Forms.DataGridView MError_Grid;
    }
}