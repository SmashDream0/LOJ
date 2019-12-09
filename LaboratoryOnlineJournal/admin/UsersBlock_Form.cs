using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaboratoryOnlineJournal
{
    public partial class UsersBlock_Form : Form
    {
        public UsersBlock_Form()
        {
            InitializeComponent();
            
            Cause_Box.MaxLength = T.User.GetColumn(C.User.Cause).Length;
        }

        struct Columns
        {
            public const byte Name = 0;
            public const byte UType = 1;
            public const byte Block = 2;
            public const byte IsOnline = 3;
        }

        struct User_struct
        {
            public User_struct(uint ID)
            { this.ID = ID; }
            public readonly uint ID;
            public string Name { get { return T.User.Rows.Get<string>(ID, C.User.Login); } }
            public string UType { get { return T.User.Rows.Get<string>(ID, C.User.UType); } }
            public string Block
            {
                get
                {
                    if (T.User.Rows.Get<bool>(ID, C.User.Enabled))
                        return "Нет";
                    else
                        return "Да";
                }
            }
            public string IsHere
            {
                get
                {
                    if (T.User.Rows.Get<bool>(ID, C.User.IsHere))
                        return "Да";
                    else
                        return "Нет";
                }
            }
            public bool Enabled 
            {
                get { return T.User.Rows.Get<bool>(ID, C.User.Enabled); } 
                set { T.User.Rows.Set(ID, C.User.Enabled, value); } 
            }
            public string Cause { get { return T.User.Rows.Get<string>(ID, C.User.Cause); } }

            public void SetEC(string Cause)
            {
                G.User.QUERRY().SET.C(C.User.Enabled, !Enabled).C(C.User.Cause, Cause).WHERE.ID(this.ID).DO();
            }
        }

        List<User_struct> UserID = new List<User_struct>(G.User.Rows.Count);

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddUser_button_Click(object sender, EventArgs e)
        {
            G.User.QUERRY().SHOW.DO();
            G.User.GetAutoForm((RowIndex) =>
                {
                    var newUserID = G.User.Rows.GetID(RowIndex);

                    for (int i = 0; i < UserID.Count; i++)
                        if (newUserID == UserID[i].ID)
                        {
                            MessageBox.Show(this, "Пользователь уже в списке", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return false;
                        }

                    UserID.Add(new User_struct(newUserID));
                    User_Grid.RowCount++;

                    return true;
                }, AutoForm.ShowType.Admin).ShowDialog();
        }

        private void RemoveUser_button_Click(object sender, EventArgs e)
        {
            if (User_Grid.SelectedRows.Count == 0) return;

            var DeleteIndexes = new int[User_Grid.SelectedRows.Count];

            for (int i = 0; i < User_Grid.SelectedRows.Count; i++)
                DeleteIndexes[i] = User_Grid.SelectedRows[i].Index;

            User_Grid.RowCount -= User_Grid.SelectedRows.Count;

            Array.Sort(DeleteIndexes);

            for (int i = 0; i < DeleteIndexes.Length; i++)
                UserID.RemoveAt(DeleteIndexes[i] - i);

            if (User_Grid.CurrentCell != null)
            {
                if (User_Grid.CurrentCell.RowIndex - DeleteIndexes.Length > -1)
                    User_Grid.CurrentCell = User_Grid[0, User_Grid.CurrentCell.RowIndex - DeleteIndexes.Length];
                else
                    User_Grid.CurrentCell = User_Grid[0, 0];
            }
        }

        private void User_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                //Логин
                case Columns.Name:
                    e.Value = UserID[e.RowIndex].Name;
                    break;
                //Тип
                case Columns.UType:
                    e.Value = UserID[e.RowIndex].UType;
                    break;
                case Columns.Block:
                    e.Value = UserID[e.RowIndex].Block;
                    break;
                case Columns.IsOnline:
                    e.Value = UserID[e.RowIndex].IsHere;
                    break;
            }
        }

        private void AddAll_button_Click(object sender, EventArgs e)
        {
            UserID.Clear();
            G.User.QUERRY().SHOW.DO();

            for (int i = 0; i < G.User.Rows.Count; i++)
                UserID.Add(new User_struct(G.User.Rows.GetID(i)));

            User_Grid.RowCount = UserID.Count;
        }

        private void UnBlock_button_Click(object sender, EventArgs e)
        {
            if (User_Grid.SelectedRows.Count == 0 || UserID.Count == 0)
            {
                MessageBox.Show(this, "Список пуст.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var Set = G.User.QUERRY().SET.C(C.User.Enabled, true).WHERE.ID(UserID[User_Grid.SelectedRows[0].Index].ID);

            for (int i = 1; i < User_Grid.SelectedRows.Count; i++)
                Set.OR.ID(UserID[User_Grid.SelectedRows[i].Index].ID);

            Set.DO();

            User_Grid.Invalidate();
        }

        private void Block_button_Click(object sender, EventArgs e)
        {
            if (User_Grid.SelectedRows.Count == 0 || UserID.Count == 0)
            {
                MessageBox.Show(this, "Список пуст.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            var Set = G.User.QUERRY().SET.C(C.User.Enabled, false).C(C.User.Cause, Cause_Box.Text).WHERE.ID(UserID[User_Grid.SelectedRows[0].Index].ID);

            for (int i = 1; i < User_Grid.SelectedRows.Count; i++)
                Set.OR.ID(UserID[User_Grid.SelectedRows[i].Index].ID);

            Set.DO();

            User_Grid.Invalidate();
        }

        private void User_Grid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            switch (e.ColumnIndex)
            {
                case Columns.Block:
                    UserID[e.RowIndex].SetEC(Cause_Box.Text);
                    User_Grid.InvalidateCell(Columns.Block, e.RowIndex);
                    break;
                default:
                    G.User.Rows.GetEditRow_Form(UserID[e.RowIndex].ID).ShowDialog();
                    User_Grid.InvalidateRow(e.RowIndex);
                    break;
            }
        }

        private void User_Grid_CurrentCellChanged(object sender, EventArgs e)
        {
            if (User_Grid.CurrentCell != null)
                Cause_Box.Text = UserID[User_Grid.CurrentCell.RowIndex].Cause;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            User_Grid.InvalidateColumn(Columns.IsOnline);
        }
    }
}
