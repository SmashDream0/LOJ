using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace LaboratoryOnlineJournal
{
    public partial class LoadDBUpdates_Form : Form
    {
        public LoadDBUpdates_Form(Update_struct[] Files)
        {
            InitializeComponent();

            this.Files = Files;

            Updates_Grid.RowCount = Files.Length;
        }

        public unsafe struct Update_struct
        {
            public Update_struct(string FilePath)
            {
                this.FilePath = FilePath;
                this.UserName = "Ошибка!";
                this.Loaded = true;
                this.Size = "";

                if (File.Exists(FilePath))
                {
                    using (var fs = new FileStream(FilePath, FileMode.Open))
                    {
                        uint ID = 0;
                        DateTime synchData;

                        var bytes = new byte[fs.Length];
                        fs.Read(bytes, 0, bytes.Length);

                        data.SynchPool.TryGetHeaderData(bytes, out ID, out synchData);

                        if ((bool)G.User.QUERRY().EXIST.WHERE.ID(ID).DO()[0].Value)
                        {
                            switch ((data.UType)T.User.Rows.Get_UnShow<uint>(ID, C.User.UType))
                            {
                                case data.UType.Employe:
                                case data.UType.Union:
                                    this.UserName = T.User.Rows.Get<string>(ID, C.User.Login) + "(" + T.User.Rows.Get<string>(ID, C.User.Podr, C.Podr.ShrName) + ")";
                                    break;
                                default:
                                    this.UserName = T.User.Rows.Get<string>(ID, C.User.Login);
                                    break;
                            }

                            this.Loaded = false;
                            this.Size = ((double)fs.Length / 1024).ToString("0.00") + " кб";
                        }
                    }
                }
            }

            public readonly string FilePath;
            public string Name
            { get { return Path.GetFileName(FilePath); } }

            public bool Loaded;
            public readonly string UserName;
            public string Size;

            public override string ToString()
            {
                return "Name=" + Name + ", Size=" + Size.ToString() + ", Loaded=" + Loaded.ToString();
            }
        }

        enum Columns : byte { Path, Author, Size, State };

        Update_struct[] Files;

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Updates_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            switch ((Columns)e.ColumnIndex)
            {
                case Columns.Path:
                    e.Value = Files[e.RowIndex].FilePath;
                    break;
                case Columns.State:
                    if (Files[e.RowIndex].Loaded)
                    { e.Value = "Загружено"; }
                    break;
                case Columns.Author:
                    e.Value = Files[e.RowIndex].UserName;
                    break;
                case Columns.Size:
                    e.Value = Files[e.RowIndex].Size;
                    break;
            }
        }

        private void Updates_Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex > -1 && (Columns)e.ColumnIndex == Columns.State)
            {
                var Result = LoadMessage(e.RowIndex, true);
                if (Result != null)
                { MessageBox.Show(this, "Файл \"" + Files[e.RowIndex].Name + "\"\n" + Result, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Question); }
                else
                { Updates_Grid.InvalidateCell(e.ColumnIndex, e.RowIndex); }
            }
        }

        string LoadMessage(int Index, bool UseProgressForm)
        {
            if (Files[Index].Loaded)
            { return null; }
            else
            {
                //if (!CounterUpdate()) return "Ошибка отправки, продолжение невозможно";

                byte[] Message;

                using (var fs = new FileStream(Files[Index].FilePath, FileMode.Open))
                {
                    Message = new byte[fs.Length];
                    fs.Read(Message, 0, Message.Length);
                }

                var Return = data.SynchPool.LoadCrypted(Message, UseProgressForm);

                if (Return == null)
                { Files[Index].Loaded = true; }

                return Return;
            }
        }

        private void LoadAll_button_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < Files.Length; i++)
            {
                var Result = LoadMessage(i, true);

                if (Result != null)
                {
                    if (i == Files.Length - 1 && MessageBox.Show(this, "Файл \"" + Files[i].Name + "\"\n" + Result + "\nПродолжить загрузку?", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                    { break; }
                    else
                    {
                        MessageBox.Show(this, "Файл \"" + Files[i].Name + "\", последний в списке.\n" + Result, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }

            Updates_Grid.InvalidateColumn((int)Columns.State);
        }

        private void LoadDBUpdates_Form_Load(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Произвести загрузку обновлений немедленно?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            { LoadAll_button_Click(null, null); }
        }
    }
}
