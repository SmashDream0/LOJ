namespace LaboratoryOnlineJournal
{
    partial class Table_Form
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
            this.Table_Grid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.Table_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Table_Grid
            // 
            this.Table_Grid.AllowUserToAddRows = false;
            this.Table_Grid.AllowUserToDeleteRows = false;
            this.Table_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Table_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Table_Grid.Location = new System.Drawing.Point(0, 2);
            this.Table_Grid.Name = "Table_Grid";
            this.Table_Grid.ReadOnly = true;
            this.Table_Grid.RowHeadersWidth = 22;
            this.Table_Grid.Size = new System.Drawing.Size(992, 323);
            this.Table_Grid.TabIndex = 0;
            this.Table_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Table_Grid_CellValueNeeded);
            this.Table_Grid.DoubleClick += new System.EventHandler(this.Table_Grid_DoubleClick);
            // 
            // Table_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(995, 327);
            this.Controls.Add(this.Table_Grid);
            this.Name = "Table_Form";
            this.Resize += new System.EventHandler(this.Table_Form_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.Table_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Table_Grid;
    }
}