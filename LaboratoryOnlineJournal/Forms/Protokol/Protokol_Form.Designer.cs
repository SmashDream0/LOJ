namespace LaboratoryOnlineJournal
{
    partial class Protok_Form
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
            this.Close_button = new System.Windows.Forms.Button();
            this.Save_button = new System.Windows.Forms.Button();
            this.Protoks_Grid = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Open_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.ReSaveAndOpen_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.Save_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.Delete_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.Up_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.Down_Strip = new System.Windows.Forms.ToolStripMenuItem();
            this.CanMoveByButtons_check = new System.Windows.Forms.CheckBox();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CrDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Taos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Podr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Marks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.Protoks_Grid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(657, 344);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(109, 34);
            this.Close_button.TabIndex = 8;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Save_button
            // 
            this.Save_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Save_button.Location = new System.Drawing.Point(12, 344);
            this.Save_button.Name = "Save_button";
            this.Save_button.Size = new System.Drawing.Size(109, 34);
            this.Save_button.TabIndex = 11;
            this.Save_button.Text = "Сохранить все";
            this.Save_button.UseVisualStyleBackColor = true;
            this.Save_button.Click += new System.EventHandler(this.Save_button_Click);
            // 
            // Protoks_Grid
            // 
            this.Protoks_Grid.AllowUserToAddRows = false;
            this.Protoks_Grid.AllowUserToDeleteRows = false;
            this.Protoks_Grid.AllowUserToResizeColumns = false;
            this.Protoks_Grid.AllowUserToResizeRows = false;
            this.Protoks_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Protoks_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Protoks_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Protoks_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.CrDate,
            this.Time,
            this.Taos,
            this.Type,
            this.Date,
            this.Podr,
            this.Marks,
            this.ID});
            this.Protoks_Grid.Location = new System.Drawing.Point(12, 12);
            this.Protoks_Grid.Name = "Protoks_Grid";
            this.Protoks_Grid.RowHeadersVisible = false;
            this.Protoks_Grid.Size = new System.Drawing.Size(754, 295);
            this.Protoks_Grid.TabIndex = 12;
            this.Protoks_Grid.VirtualMode = true;
            this.Protoks_Grid.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.Protoks_Grid_CellBeginEdit);
            this.Protoks_Grid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Protoks_Grid_CellDoubleClick);
            this.Protoks_Grid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.Protoks_Grid_CellToolTipTextNeeded);
            this.Protoks_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Protoks_Grid_CellValueNeeded);
            this.Protoks_Grid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Protoks_Grid_CellValuePushed);
            this.Protoks_Grid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Protoks_Grid_KeyDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Open_Strip,
            this.ReSaveAndOpen_Strip,
            this.Save_Strip,
            this.Delete_Strip,
            this.Up_Strip,
            this.Down_Strip});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(217, 136);
            this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
            // 
            // Open_Strip
            // 
            this.Open_Strip.Name = "Open_Strip";
            this.Open_Strip.Size = new System.Drawing.Size(216, 22);
            this.Open_Strip.Text = "Открыть";
            this.Open_Strip.Click += new System.EventHandler(this.Open_Strip_Click);
            // 
            // ReSaveAndOpen_Strip
            // 
            this.ReSaveAndOpen_Strip.Name = "ReSaveAndOpen_Strip";
            this.ReSaveAndOpen_Strip.Size = new System.Drawing.Size(216, 22);
            this.ReSaveAndOpen_Strip.Text = "Сформировать и открыть";
            this.ReSaveAndOpen_Strip.Click += new System.EventHandler(this.ReSaveAndOpen_Strip_Click);
            // 
            // Save_Strip
            // 
            this.Save_Strip.Name = "Save_Strip";
            this.Save_Strip.Size = new System.Drawing.Size(216, 22);
            this.Save_Strip.Text = "Сохранить";
            this.Save_Strip.Click += new System.EventHandler(this.Save_Strip_Click);
            // 
            // Delete_Strip
            // 
            this.Delete_Strip.Name = "Delete_Strip";
            this.Delete_Strip.Size = new System.Drawing.Size(216, 22);
            this.Delete_Strip.Text = "Удалить";
            this.Delete_Strip.Click += new System.EventHandler(this.Delete_Strip_Click);
            // 
            // Up_Strip
            // 
            this.Up_Strip.Name = "Up_Strip";
            this.Up_Strip.Size = new System.Drawing.Size(216, 22);
            this.Up_Strip.Text = "Вверх";
            this.Up_Strip.Click += new System.EventHandler(this.Up_Strip_Click);
            // 
            // Down_Strip
            // 
            this.Down_Strip.Name = "Down_Strip";
            this.Down_Strip.Size = new System.Drawing.Size(216, 22);
            this.Down_Strip.Text = "Вниз";
            this.Down_Strip.Click += new System.EventHandler(this.Down_Strip_Click);
            // 
            // CanMoveByButtons_check
            // 
            this.CanMoveByButtons_check.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CanMoveByButtons_check.AutoSize = true;
            this.CanMoveByButtons_check.Location = new System.Drawing.Point(12, 313);
            this.CanMoveByButtons_check.Name = "CanMoveByButtons_check";
            this.CanMoveByButtons_check.Size = new System.Drawing.Size(207, 17);
            this.CanMoveByButtons_check.TabIndex = 14;
            this.CanMoveByButtons_check.Text = "Разрешить перемещать стрелками";
            this.CanMoveByButtons_check.UseVisualStyleBackColor = true;
            this.CanMoveByButtons_check.CheckedChanged += new System.EventHandler(this.CanMoveByButtons_check_CheckedChanged);
            // 
            // Number
            // 
            this.Number.FillWeight = 69.56133F;
            this.Number.HeaderText = "Номер";
            this.Number.Name = "Number";
            // 
            // CrDate
            // 
            this.CrDate.HeaderText = "Дата создания";
            this.CrDate.Name = "CrDate";
            // 
            // Time
            // 
            this.Time.FillWeight = 76.75127F;
            this.Time.HeaderText = "Время";
            this.Time.Name = "Time";
            // 
            // Taos
            // 
            this.Taos.FillWeight = 76.75127F;
            this.Taos.HeaderText = "Акт";
            this.Taos.Name = "Taos";
            // 
            // Type
            // 
            this.Type.FillWeight = 164.4358F;
            this.Type.HeaderText = "Тип";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // Date
            // 
            this.Date.FillWeight = 76.75127F;
            this.Date.HeaderText = "Дата";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // Podr
            // 
            this.Podr.FillWeight = 46.75204F;
            this.Podr.HeaderText = "Подразделение";
            this.Podr.Name = "Podr";
            this.Podr.ReadOnly = true;
            // 
            // Marks
            // 
            this.Marks.FillWeight = 50F;
            this.Marks.HeaderText = "Замеры";
            this.Marks.Name = "Marks";
            this.Marks.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.FillWeight = 50F;
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // Protok_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 384);
            this.Controls.Add(this.CanMoveByButtons_check);
            this.Controls.Add(this.Protoks_Grid);
            this.Controls.Add(this.Save_button);
            this.Controls.Add(this.Close_button);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Protok_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Протоколы за месяц";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Protok_Form_FormClosed);
            this.Load += new System.EventHandler(this.Protok_Form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.Protoks_Grid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button Save_button;
        private System.Windows.Forms.DataGridView Protoks_Grid;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem Save_Strip;
        private System.Windows.Forms.ToolStripMenuItem Delete_Strip;
        private System.Windows.Forms.ToolStripMenuItem Up_Strip;
        private System.Windows.Forms.ToolStripMenuItem Down_Strip;
        private System.Windows.Forms.CheckBox CanMoveByButtons_check;
        private System.Windows.Forms.ToolStripMenuItem Open_Strip;
        private System.Windows.Forms.ToolStripMenuItem ReSaveAndOpen_Strip;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn CrDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn Taos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Podr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Marks;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    }
}