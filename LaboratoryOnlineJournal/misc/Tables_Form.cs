using LaboratoryOnlineJournal.Serializer;
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
        public Tables_Form(DeserializeResult Tables)
        {
            this.Tables = Tables;
            InitializeComponent();

            var SPoolID = (uint)G.SPool.QUERRY().GET.ID().WHERE.C(C.SPool.Date, Tables.SynchDate).DO()[0].Value;

            Descryption_label.Text = SPoolID.ToString() + " - " + Tables.SynchDate.ToString() + ' ' + T.User.Rows.Get<string>(Tables.UserID, C.User.Login);

            this.Tables_Grid.Columns.Add("Таблица", "Таблица");

            this.Tables_Grid.VirtualMode = true;
            this.Tables_Grid.RowCount = Tables.Tables.Count();
        }
        DeserializeResult Tables;

        private void Tables_Grid_DoubleClick(object sender, EventArgs e)
        {
            if (Tables_Grid.CurrentCell == null) return;

            new Table_Form(Tables.Tables[Tables_Grid.CurrentCell.RowIndex]).ShowDialog();
        }

        private void Tables_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1 && e.ColumnIndex > -1)
                e.Value = Tables.Tables[e.RowIndex].STable.Parent.Name + "(" + Tables.Tables[e.RowIndex].Rows.Count() + ")";
        }
    }
}
