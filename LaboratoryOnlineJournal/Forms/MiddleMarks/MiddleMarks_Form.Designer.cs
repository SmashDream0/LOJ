namespace LaboratoryOnlineJournal
{
    partial class MiddleMarks_Form
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
            this.Marks_Grid = new System.Windows.Forms.DataGridView();
            this.Close_button = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Year_Box = new System.Windows.Forms.TextBox();
            this.NextYear_button = new System.Windows.Forms.Button();
            this.PerviousYear_button = new System.Windows.Forms.Button();
            this.Podr_combo = new System.Windows.Forms.ComboBox();
            this.Obj_combo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveToFile_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Marks_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Marks_Grid
            // 
            this.Marks_Grid.AllowUserToAddRows = false;
            this.Marks_Grid.AllowUserToDeleteRows = false;
            this.Marks_Grid.AllowUserToResizeRows = false;
            this.Marks_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Marks_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Marks_Grid.Location = new System.Drawing.Point(3, 48);
            this.Marks_Grid.Name = "Marks_Grid";
            this.Marks_Grid.ReadOnly = true;
            this.Marks_Grid.RowHeadersVisible = false;
            this.Marks_Grid.RowHeadersWidth = 10;
            this.Marks_Grid.Size = new System.Drawing.Size(787, 386);
            this.Marks_Grid.TabIndex = 1;
            this.Marks_Grid.VirtualMode = true;
            this.Marks_Grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Marks_Grid_CellClick);
            this.Marks_Grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Marks_Grid_CellMouseClick);
            this.Marks_Grid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Marks_Grid_CellMouseUp);
            this.Marks_Grid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.Marks_Grid_CellToolTipTextNeeded);
            this.Marks_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Marks_Grid_CellValueNeeded);
            this.Marks_Grid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Marks_Grid_MouseClick);
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(692, 439);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(98, 33);
            this.Close_button.TabIndex = 3;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(37, 2);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "Год";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Year_Box
            // 
            this.Year_Box.Location = new System.Drawing.Point(37, 22);
            this.Year_Box.MaxLength = 4;
            this.Year_Box.Name = "Year_Box";
            this.Year_Box.Size = new System.Drawing.Size(65, 20);
            this.Year_Box.TabIndex = 38;
            this.Year_Box.Text = "1";
            this.Year_Box.TextChanged += new System.EventHandler(this.Year_Box_TextChanged);
            // 
            // NextYear_button
            // 
            this.NextYear_button.Location = new System.Drawing.Point(106, 22);
            this.NextYear_button.Name = "NextYear_button";
            this.NextYear_button.Size = new System.Drawing.Size(30, 20);
            this.NextYear_button.TabIndex = 39;
            this.NextYear_button.Text = ">";
            this.NextYear_button.UseVisualStyleBackColor = true;
            this.NextYear_button.Click += new System.EventHandler(this.NextYear_button_Click);
            // 
            // PerviousYear_button
            // 
            this.PerviousYear_button.Location = new System.Drawing.Point(3, 22);
            this.PerviousYear_button.Name = "PerviousYear_button";
            this.PerviousYear_button.Size = new System.Drawing.Size(30, 20);
            this.PerviousYear_button.TabIndex = 42;
            this.PerviousYear_button.Text = "<";
            this.PerviousYear_button.UseVisualStyleBackColor = true;
            this.PerviousYear_button.Click += new System.EventHandler(this.PerviousYear_button_Click);
            // 
            // Podr_combo
            // 
            this.Podr_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Podr_combo.FormattingEnabled = true;
            this.Podr_combo.Location = new System.Drawing.Point(142, 22);
            this.Podr_combo.Name = "Podr_combo";
            this.Podr_combo.Size = new System.Drawing.Size(180, 21);
            this.Podr_combo.TabIndex = 43;
            this.Podr_combo.SelectedIndexChanged += new System.EventHandler(this.Podr_combo_SelectedIndexChanged);
            // 
            // Obj_combo
            // 
            this.Obj_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Obj_combo.FormattingEnabled = true;
            this.Obj_combo.Location = new System.Drawing.Point(328, 22);
            this.Obj_combo.Name = "Obj_combo";
            this.Obj_combo.Size = new System.Drawing.Size(243, 21);
            this.Obj_combo.TabIndex = 44;
            this.Obj_combo.SelectedIndexChanged += new System.EventHandler(this.Year_Box_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(146, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(176, 16);
            this.label1.TabIndex = 45;
            this.label1.Text = "Подразделение";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(328, 2);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 16);
            this.label2.TabIndex = 46;
            this.label2.Text = "Объект";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveToFile_button
            // 
            this.SaveToFile_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveToFile_button.Location = new System.Drawing.Point(3, 439);
            this.SaveToFile_button.Name = "SaveToFile_button";
            this.SaveToFile_button.Size = new System.Drawing.Size(98, 33);
            this.SaveToFile_button.TabIndex = 47;
            this.SaveToFile_button.Text = "В файл";
            this.SaveToFile_button.UseVisualStyleBackColor = true;
            this.SaveToFile_button.Click += new System.EventHandler(this.SaveToFile_button_Click);
            // 
            // MiddleMarks_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 476);
            this.Controls.Add(this.SaveToFile_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Obj_combo);
            this.Controls.Add(this.Podr_combo);
            this.Controls.Add(this.PerviousYear_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Year_Box);
            this.Controls.Add(this.NextYear_button);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Marks_Grid);
            this.Name = "MiddleMarks_Form";
            this.Text = "Средние показатели";
            ((System.ComponentModel.ISupportInitialize)(this.Marks_Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Marks_Grid;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox Year_Box;
        private System.Windows.Forms.Button NextYear_button;
        private System.Windows.Forms.Button PerviousYear_button;
        private System.Windows.Forms.ComboBox Podr_combo;
        private System.Windows.Forms.ComboBox Obj_combo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SaveToFile_button;
    }

    /*partial class MiddleMarks_Form
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
            this.Marks_Grid = new System.Windows.Forms.DataGridView();
            this.Close_button = new System.Windows.Forms.Button();
            this.Round_Box = new System.Windows.Forms.TextBox();
            this.Round_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.Year_Box = new System.Windows.Forms.TextBox();
            this.NextYear_button = new System.Windows.Forms.Button();
            this.PerviousYear_button = new System.Windows.Forms.Button();
            this.Podr_combo = new System.Windows.Forms.ComboBox();
            this.Obj_combo = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveToFile_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Marks_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Marks_Grid
            // 
            this.Marks_Grid.AllowUserToAddRows = false;
            this.Marks_Grid.AllowUserToDeleteRows = false;
            this.Marks_Grid.AllowUserToResizeRows = false;
            this.Marks_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Marks_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Marks_Grid.Location = new System.Drawing.Point(6, 68);
            this.Marks_Grid.Name = "Marks_Grid";
            this.Marks_Grid.ReadOnly = true;
            this.Marks_Grid.RowHeadersVisible = false;
            this.Marks_Grid.RowHeadersWidth = 10;
            this.Marks_Grid.Size = new System.Drawing.Size(775, 366);
            this.Marks_Grid.TabIndex = 1;
            this.Marks_Grid.VirtualMode = true;
            this.Marks_Grid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.Marks_Grid_CellClick);
            this.Marks_Grid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Marks_Grid_CellMouseClick);
            this.Marks_Grid.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.Marks_Grid_CellMouseUp);
            this.Marks_Grid.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.Marks_Grid_CellToolTipTextNeeded);
            this.Marks_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Marks_Grid_CellValueNeeded);
            this.Marks_Grid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Marks_Grid_MouseClick);
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(683, 440);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(98, 33);
            this.Close_button.TabIndex = 3;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Round_Box
            // 
            this.Round_Box.Location = new System.Drawing.Point(156, 30);
            this.Round_Box.MaxLength = 1;
            this.Round_Box.Name = "Round_Box";
            this.Round_Box.Size = new System.Drawing.Size(51, 20);
            this.Round_Box.TabIndex = 5;
            this.Round_Box.TextChanged += new System.EventHandler(this.Round_Box_TextChanged);
            // 
            // Round_label
            // 
            this.Round_label.Location = new System.Drawing.Point(153, 10);
            this.Round_label.Name = "Round_label";
            this.Round_label.Size = new System.Drawing.Size(54, 16);
            this.Round_label.TabIndex = 6;
            this.Round_label.Text = "Точность";
            this.Round_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(25, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 16);
            this.label3.TabIndex = 41;
            this.label3.Text = "Год";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Year_Box
            // 
            this.Year_Box.Location = new System.Drawing.Point(39, 30);
            this.Year_Box.MaxLength = 4;
            this.Year_Box.Name = "Year_Box";
            this.Year_Box.Size = new System.Drawing.Size(65, 20);
            this.Year_Box.TabIndex = 38;
            this.Year_Box.Text = "1";
            this.Year_Box.TextChanged += new System.EventHandler(this.Year_Box_TextChanged);
            // 
            // NextYear_button
            // 
            this.NextYear_button.Location = new System.Drawing.Point(110, 30);
            this.NextYear_button.Name = "NextYear_button";
            this.NextYear_button.Size = new System.Drawing.Size(30, 20);
            this.NextYear_button.TabIndex = 39;
            this.NextYear_button.Text = ">";
            this.NextYear_button.UseVisualStyleBackColor = true;
            this.NextYear_button.Click += new System.EventHandler(this.NextYear_button_Click);
            // 
            // PerviousYear_button
            // 
            this.PerviousYear_button.Location = new System.Drawing.Point(3, 30);
            this.PerviousYear_button.Name = "PerviousYear_button";
            this.PerviousYear_button.Size = new System.Drawing.Size(30, 20);
            this.PerviousYear_button.TabIndex = 42;
            this.PerviousYear_button.Text = "<";
            this.PerviousYear_button.UseVisualStyleBackColor = true;
            this.PerviousYear_button.Click += new System.EventHandler(this.PerviousYear_button_Click);
            // 
            // Podr_combo
            // 
            this.Podr_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Podr_combo.FormattingEnabled = true;
            this.Podr_combo.Location = new System.Drawing.Point(213, 29);
            this.Podr_combo.Name = "Podr_combo";
            this.Podr_combo.Size = new System.Drawing.Size(180, 21);
            this.Podr_combo.TabIndex = 43;
            this.Podr_combo.SelectedIndexChanged += new System.EventHandler(this.Year_Box_TextChanged);
            // 
            // Obj_combo
            // 
            this.Obj_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Obj_combo.FormattingEnabled = true;
            this.Obj_combo.Location = new System.Drawing.Point(399, 29);
            this.Obj_combo.Name = "Obj_combo";
            this.Obj_combo.Size = new System.Drawing.Size(243, 21);
            this.Obj_combo.TabIndex = 44;
            this.Obj_combo.SelectedIndexChanged += new System.EventHandler(this.Year_Box_TextChanged);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(213, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 16);
            this.label1.TabIndex = 45;
            this.label1.Text = "Подразделение";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(399, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(243, 16);
            this.label2.TabIndex = 46;
            this.label2.Text = "Объект";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SaveToFile_button
            // 
            this.SaveToFile_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveToFile_button.Location = new System.Drawing.Point(6, 440);
            this.SaveToFile_button.Name = "SaveToFile_button";
            this.SaveToFile_button.Size = new System.Drawing.Size(98, 33);
            this.SaveToFile_button.TabIndex = 47;
            this.SaveToFile_button.Text = "В файл";
            this.SaveToFile_button.UseVisualStyleBackColor = true;
            this.SaveToFile_button.Click += new System.EventHandler(this.SaveToFile_button_Click);
            // 
            // MiddleMarks_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(793, 476);
            this.Controls.Add(this.SaveToFile_button);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Obj_combo);
            this.Controls.Add(this.Podr_combo);
            this.Controls.Add(this.PerviousYear_button);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Year_Box);
            this.Controls.Add(this.NextYear_button);
            this.Controls.Add(this.Round_label);
            this.Controls.Add(this.Round_Box);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Marks_Grid);
            this.Name = "MiddleMarks_Form";
            this.Text = "Средние показатели";
            ((System.ComponentModel.ISupportInitialize)(this.Marks_Grid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView Marks_Grid;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.TextBox Round_Box;
        private System.Windows.Forms.Label Round_label;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox Year_Box;
        private System.Windows.Forms.Button NextYear_button;
        private System.Windows.Forms.Button PerviousYear_button;
        private System.Windows.Forms.ComboBox Podr_combo;
        private System.Windows.Forms.ComboBox Obj_combo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SaveToFile_button;
    }*/
}