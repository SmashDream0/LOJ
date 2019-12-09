using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaboratoryOnlineJournal
{
    public partial class Table_Form : Form
    {
        public Table_Form(SynchPool_class.Table_class Table)
        {
            this.Table = Table;
            InitializeComponent();
            this.Text = "Таблица \"" + Table.Table.Parent.Name + "\"";

            this.Table_Grid.Columns.Add("", "ID");
            this.Table_Grid.Columns.Add("", "InUse");
            for (int i = 0; i < Table.Table.Parent.Columns.Count - 1; i++)
            { this.Table_Grid.Columns.Add("", Table.Table.Parent.GetColumn(i).Name); }

            this.Table_Grid.VirtualMode = true;
            this.Table_Grid.RowCount = Table.Rows.Length;

            LWidth = this.Table_Grid.Location.X;
            RWidth = this.Width - this.Table_Grid.Size.Width - this.Table_Grid.Location.X;
            UWidth = this.Table_Grid.Location.Y;
            DWidth = this.Height - this.Table_Grid.Size.Height - this.Table_Grid.Location.Y;
        }

        int LWidth, RWidth;
        int UWidth, DWidth;

        SynchPool_class.Table_class Table;

        private void Table_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                if (e.ColumnIndex == 0)
                {
                    if ((bool)Table.Table.QUERRY(DataBase.State.None).EXIST.WHERE.ID(Table.Rows[e.RowIndex].ID).DO()[0].Value)
                    { e.Value = Table.Rows[e.RowIndex].ID + "(Существует)"; }
                    else
                    { e.Value = Table.Rows[e.RowIndex].ID; }
                }
                else if (e.ColumnIndex == 1)
                { e.Value = Table.Rows[e.RowIndex].InUse; }
                else if (e.ColumnIndex > 1)
                { e.Value = Table.Rows[e.RowIndex].Values[e.ColumnIndex - 2]; }
            }
        }

        private void Table_Grid_DoubleClick(object sender, EventArgs e)
        {
            if (Table_Grid.CurrentCell == null)
            { return; }

            Table.Table.QUERRY(DataBase.State.None).SHOW.WHERE.ID(Table.Rows[Table_Grid.CurrentCell.RowIndex].ID).DO();

            if (Table.Table.Rows.Count > 0)
            { Table.Table.GetAutoForm(AutoForm.ShowType.Admin).ShowDialog(); }
        }

        private void Table_Form_Resize(object sender, EventArgs e)
        {
            this.Table_Grid.Size = new Size(this.Width - LWidth - RWidth, this.Height - UWidth - DWidth);
        }
    }
}
