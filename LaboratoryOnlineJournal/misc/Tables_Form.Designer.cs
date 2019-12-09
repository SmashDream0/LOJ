namespace LaboratoryOnlineJournal
{
    partial class Tables_Form
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
            this.Tables_Grid = new System.Windows.Forms.DataGridView();
            this.Descryption_label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Tables_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Tables_Grid
            // 
            this.Tables_Grid.AllowUserToAddRows = false;
            this.Tables_Grid.AllowUserToDeleteRows = false;
            this.Tables_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tables_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Tables_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Tables_Grid.Location = new System.Drawing.Point(3, 74);
            this.Tables_Grid.Name = "Tables_Grid";
            this.Tables_Grid.ReadOnly = true;
            this.Tables_Grid.RowHeadersWidth = 22;
            this.Tables_Grid.Size = new System.Drawing.Size(585, 205);
            this.Tables_Grid.TabIndex = 0;
            this.Tables_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Tables_Grid_CellValueNeeded);
            this.Tables_Grid.DoubleClick += new System.EventHandler(this.Tables_Grid_DoubleClick);
            // 
            // Descryption_label
            // 
            this.Descryption_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Descryption_label.Location = new System.Drawing.Point(3, 4);
            this.Descryption_label.Name = "Descryption_label";
            this.Descryption_label.Size = new System.Drawing.Size(585, 67);
            this.Descryption_label.TabIndex = 1;
            // 
            // Tables_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 281);
            this.Controls.Add(this.Descryption_label);
            this.Controls.Add(this.Tables_Grid);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Tables_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Таблицы";
            ((System.ComponentModel.ISupportInitialize)(this.Tables_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Tables_Grid;
        private System.Windows.Forms.Label Descryption_label;
    }
}