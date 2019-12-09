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
    public partial class Tables_Form : Form
    {
        public Tables_Form(SynchPool_class.Table_class[] Tables, uint UserID, DateTime DateOfCreate)
        {
            this.Tables = Tables;
            InitializeComponent();

            var SPoolID = (uint)G.SPool.QUERRY().GET.ID().WHERE.C(C.SPool.Date, DateOfCreate).DO()[0].Value;

            Descryption_label.Text = SPoolID.ToString() + " - " + DateOfCreate.ToString() + ' ' + T.User.Rows.Get<string>(UserID, C.User.Login);

            this.Tables_Grid.Columns.Add("Таблица", "Таблица");

            this.Tables_Grid.VirtualMode = true;
            this.Tables_Grid.RowCount = Tables.Length;
        }
        SynchPool_class.Table_class[] Tables;

        private void Tables_Grid_DoubleClick(object sender, EventArgs e)
        {
            if (Tables_Grid.CurrentCell == null) return;

            new Table_Form(Tables[Tables_Grid.CurrentCell.RowIndex]).ShowDialog();
        }

        private void Tables_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
                e.Value = Tables[e.RowIndex].Table.Parent.Name + "(" + Tables[e.RowIndex].Rows.Length + ")";
        }
    }
}
