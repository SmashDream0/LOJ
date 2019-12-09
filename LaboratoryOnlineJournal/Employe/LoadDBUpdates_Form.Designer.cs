namespace LaboratoryOnlineJournal
{
    partial class LoadDBUpdates_Form
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
            this.Updates_Grid = new System.Windows.Forms.DataGridView();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Size = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.State = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LoadAll_button = new System.Windows.Forms.Button();
            this.Close_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Updates_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Updates_Grid
            // 
            this.Updates_Grid.AllowUserToAddRows = false;
            this.Updates_Grid.AllowUserToDeleteRows = false;
            this.Updates_Grid.AllowUserToResizeRows = false;
            this.Updates_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Updates_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Updates_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FilePath,
            this.User,
            this.Size,
            this.State});
            this.Updates_Grid.Location = new System.Drawing.Point(3, 3);
            this.Updates_Grid.Name = "Updates_Grid";
            this.Updates_Grid.ReadOnly = true;
            this.Updates_Grid.RowHeadersVisible = false;
            this.Updates_Grid.Size = new System.Drawing.Size(810, 284);
            this.Updates_Grid.TabIndex = 0;
            this.Updates_Grid.VirtualMode = true;
            this.Updates_Grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Updates_Grid_CellDoubleClick);
            this.Updates_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Updates_Grid_CellValueNeeded);
            // 
            // FilePath
            // 
            this.FilePath.HeaderText = "Файл";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            // 
            // User
            // 
            this.User.HeaderText = "Автор";
            this.User.Name = "User";
            this.User.ReadOnly = true;
            // 
            // Size
            // 
            this.Size.HeaderText = "Размер";
            this.Size.Name = "Size";
            this.Size.ReadOnly = true;
            // 
            // State
            // 
            this.State.FillWeight = 30.45685F;
            this.State.HeaderText = "Статус";
            this.State.Name = "State";
            this.State.ReadOnly = true;
            // 
            // LoadAll_button
            // 
            this.LoadAll_button.Location = new System.Drawing.Point(4, 292);
            this.LoadAll_button.Name = "LoadAll_button";
            this.LoadAll_button.Size = new System.Drawing.Size(103, 32);
            this.LoadAll_button.TabIndex = 1;
            this.LoadAll_button.Text = "Загрузить все";
            this.LoadAll_button.UseVisualStyleBackColor = true;
            this.LoadAll_button.Click += new System.EventHandler(this.LoadAll_button_Click);
            // 
            // Close_button
            // 
            this.Close_button.Location = new System.Drawing.Point(710, 292);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(103, 32);
            this.Close_button.TabIndex = 2;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // LoadDBUpdates_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 328);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.LoadAll_button);
            this.Controls.Add(this.Updates_Grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LoadDBUpdates_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Загрузить обновления";
            this.Load += new System.EventHandler(this.LoadDBUpdates_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Updates_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Updates_Grid;
        private System.Windows.Forms.Button LoadAll_button;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Size;
        private System.Windows.Forms.DataGridViewTextBoxColumn State;
    }
}