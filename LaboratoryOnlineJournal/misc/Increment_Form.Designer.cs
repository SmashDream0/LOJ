namespace LaboratoryOnlineJournal
{
    partial class Increment_Form
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
            this.Increment_Grid = new System.Windows.Forms.DataGridView();
            this.TableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Increment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Multiply = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Limit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Close_button = new System.Windows.Forms.Button();
            this.Increment_button = new System.Windows.Forms.Button();
            this.Decrement_button = new System.Windows.Forms.Button();
            this.Description_label = new System.Windows.Forms.Label();
            this.Clear_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Increment_Grid)).BeginInit();
            this.SuspendLayout();
            // 
            // Increment_Grid
            // 
            this.Increment_Grid.AllowUserToAddRows = false;
            this.Increment_Grid.AllowUserToDeleteRows = false;
            this.Increment_Grid.AllowUserToResizeColumns = false;
            this.Increment_Grid.AllowUserToResizeRows = false;
            this.Increment_Grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Increment_Grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.Increment_Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Increment_Grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.TableName,
            this.Increment,
            this.Multiply,
            this.Limit});
            this.Increment_Grid.Location = new System.Drawing.Point(13, 2);
            this.Increment_Grid.Name = "Increment_Grid";
            this.Increment_Grid.RowHeadersVisible = false;
            this.Increment_Grid.Size = new System.Drawing.Size(790, 219);
            this.Increment_Grid.TabIndex = 0;
            this.Increment_Grid.VirtualMode = true;
            this.Increment_Grid.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Increment_Grid_CellValueNeeded);
            this.Increment_Grid.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.Increment_Grid_CellValuePushed);
            // 
            // TableName
            // 
            this.TableName.HeaderText = "Таблица";
            this.TableName.Name = "TableName";
            // 
            // Increment
            // 
            this.Increment.HeaderText = "Инкремент";
            this.Increment.Name = "Increment";
            // 
            // Multiply
            // 
            this.Multiply.HeaderText = "Множитель";
            this.Multiply.Name = "Multiply";
            // 
            // Limit
            // 
            this.Limit.HeaderText = "Осталось";
            this.Limit.Name = "Limit";
            // 
            // Close_button
            // 
            this.Close_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Close_button.Location = new System.Drawing.Point(713, 270);
            this.Close_button.Name = "Close_button";
            this.Close_button.Size = new System.Drawing.Size(90, 34);
            this.Close_button.TabIndex = 1;
            this.Close_button.Text = "Закрыть";
            this.Close_button.UseVisualStyleBackColor = true;
            this.Close_button.Click += new System.EventHandler(this.Close_button_Click);
            // 
            // Increment_button
            // 
            this.Increment_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Increment_button.Location = new System.Drawing.Point(13, 227);
            this.Increment_button.Name = "Increment_button";
            this.Increment_button.Size = new System.Drawing.Size(129, 34);
            this.Increment_button.TabIndex = 2;
            this.Increment_button.Text = "Инкрементировать";
            this.Increment_button.UseVisualStyleBackColor = true;
            this.Increment_button.Click += new System.EventHandler(this.Increment_button_Click);
            // 
            // Decrement_button
            // 
            this.Decrement_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Decrement_button.Location = new System.Drawing.Point(12, 267);
            this.Decrement_button.Name = "Decrement_button";
            this.Decrement_button.Size = new System.Drawing.Size(129, 34);
            this.Decrement_button.TabIndex = 3;
            this.Decrement_button.Text = "Декрементировать";
            this.Decrement_button.UseVisualStyleBackColor = true;
            this.Decrement_button.Click += new System.EventHandler(this.Decrement_button_Click);
            // 
            // Description_label
            // 
            this.Description_label.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Description_label.Location = new System.Drawing.Point(353, 227);
            this.Description_label.Name = "Description_label";
            this.Description_label.Size = new System.Drawing.Size(192, 74);
            this.Description_label.TabIndex = 4;
            // 
            // Clear_button
            // 
            this.Clear_button.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.Clear_button.Location = new System.Drawing.Point(218, 227);
            this.Clear_button.Name = "Clear_button";
            this.Clear_button.Size = new System.Drawing.Size(129, 34);
            this.Clear_button.TabIndex = 5;
            this.Clear_button.Text = "Сбросить";
            this.Clear_button.UseVisualStyleBackColor = true;
            this.Clear_button.Click += new System.EventHandler(this.Clear_button_Click);
            // 
            // Increment_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(815, 313);
            this.Controls.Add(this.Clear_button);
            this.Controls.Add(this.Description_label);
            this.Controls.Add(this.Decrement_button);
            this.Controls.Add(this.Increment_button);
            this.Controls.Add(this.Close_button);
            this.Controls.Add(this.Increment_Grid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "Increment_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            ((System.ComponentModel.ISupportInitialize)(this.Increment_Grid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView Increment_Grid;
        private System.Windows.Forms.Button Close_button;
        private System.Windows.Forms.Button Increment_button;
        private System.Windows.Forms.Button Decrement_button;
        private System.Windows.Forms.Label Description_label;
        private System.Windows.Forms.Button Clear_button;
        private System.Windows.Forms.DataGridViewTextBoxColumn TableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Increment;
        private System.Windows.Forms.DataGridViewTextBoxColumn Multiply;
        private System.Windows.Forms.DataGridViewTextBoxColumn Limit;
    }
}