using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace LaboratoryOnlineJournal
{
    public partial class AdminPanel : Form
    {
        public AdminPanel()
        {
            InitializeComponent();

            RCache.Marks = new RCache.Marks_class();
            RCache.Marks.Update();

            RCache.PSG = new RCache.PSG_class();
            RCache.Volumes = new RCache.Volumes_class();

            SubTables = new DataBase.ISTable[data.T1.Tables.Count];

            for (int i = 0; i < SubTables.Length; i++)
            {
                var Table = data.T1.Tables[i];

                if (Table.GetSubTable.Count == 0)
                { SubTables[i] = Table.CreateSubTable(); }
                else
                { SubTables[i] = Table.GetSubTable[0]; }
            }

            Array.Sort(SubTables, (it1, it2) =>
            {
                var ret1 = it1.Parent.DataSource.Type.CompareTo(it2.Parent.DataSource.Type);

                if (ret1 == 0)
                { return it1.Name.CompareTo(it2.Name); }
                else
                { return ret1; }
            });

            {
                int Y, M;
                ATMisc.GetYearMonthFromYM(data.User<int>(C.User.YM), out Y, out M);
                Period_Box.Text = M.ToString() + '.' + Y.ToString();
            }
        }

        DataBase.ISTable[] SubTables;

        void GetAutoForm(DataBase.ISTable SubTable)
        {
            if (LoadNew_check.Checked)
            {
                if (DeletedToo_check.Checked)
                { SubTable.QUERRY(DataBase.State.None).SHOW.DO(); }
                else
                { SubTable.QUERRY().SHOW.DO(); }
            }

            SubTable.GetAutoForm(AutoForm.ShowType.Admin).ShowDialog();
        }

        private void AdminPanel_Load(object sender, EventArgs e)
        {
            this.Location = data.PrgSettings.Forms[(int)data.Forms.AdminPanel].Location;

            for (int i = 0; i < data.T1.Tables.Count; i++)
            { AnyTable_combo.Items.Add("(" + SubTables[i].Parent.DataSource.Type.ToString() + ")" + SubTables[i].Name); }
        }

        private void UsersEditor_button_Click(object sender, EventArgs e)
        {
            GetAutoForm(G.User);
        }

        private void Settings_button_Click(object sender, EventArgs e)
        {
            (new Settings_Form()).ShowDialog();
        }

        private void AdminPanel_FormClosing(object sender, FormClosingEventArgs e)
        {
            data.PrgSettings.Forms[(int)data.Forms.AdminPanel].Set(this);
        }

        private void DownloadSpeed_button_Click(object sender, EventArgs e)
        {
            long Summ = 0;
            var SB = new StringBuilder();

            for (int i = 0; i < data.T1.Tables.Count; i++)
            {
                SB.Append(data.T1.Tables[i].Name + ", Initial=" + ((double)((DataBase.table)data.T1.Tables[i]).InitilalTime / 10000000).ToString() + ", ShowRows=" + ((double)((DataBase.table)data.T1.Tables[i]).ShowRowsTime / 10000000).ToString() + ", Download=" + ((double)((DataBase.table)data.T1.Tables[i]).DownloadTime / 10000000).ToString() + "\n");
                Summ += ((DataBase.table)data.T1.Tables[i]).InitilalTime;
            }

            SB.Append("Summ Initial=" + ((double)Summ / 10000000).ToString() + ", ").Append("Middle Initial=" + ((double)Summ / data.T1.Tables.Count / 10000000).ToString() + ", ");

            MessageBox.Show(SB.ToString());
        }

        private void UsersBock_button_Click(object sender, EventArgs e)
        {
            new UsersBlock_Form().ShowDialog();
        }

        private void StorageEditor_button_Click(object sender, EventArgs e)
        {
            new Nodes_Form(G.Podr, C.Podr.Xloc, C.Podr.Yloc, C.Podr .PFrom, C.Podr.ShrName).ShowDialog();
        }

        private void PodrPplEditor_button_Click(object sender, EventArgs e)
        {
            G.Podr.QUERRY(DataBase.State.None).SHOW.DO();
            G.Prfssn.QUERRY(DataBase.State.None).SHOW.DO();
            G.People.QUERRY(DataBase.State.None).SHOW.DO();
            GetAutoForm(G.PodrPpl);
        }

        private void PeopleEditor_button_Click(object sender, EventArgs e)
        {
            G.Prfssn.QUERRY(DataBase.State.None).SHOW.DO();
            G.People.QUERRY(DataBase.State.None).SHOW.DO();
            GetAutoForm(G.People);
        }

        private void SampleEditor_button_Click(object sender, EventArgs e)
        {
            GetAutoForm(G.Sample);
        }

        private void MarkEditor_button_Click(object sender, EventArgs e)
        {
            GetAutoForm(G.Mark);
        }

        private void SPointEdit_button_Click(object sender, EventArgs e)
        {
            GetAutoForm(G.SPoint);            
        }

        private void PMNormEdit_button_Click(object sender, EventArgs e)
        {
            GetAutoForm(G.PMNorm); 
        }

        private void SPoolEdit_button_Click(object sender, EventArgs e)
        {
            GetAutoForm(G.SPool); 
        }

        private void AnyTable_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void AnyTableEdit_button_Click(object sender, EventArgs e)
        {
            if (AnyTable_combo.SelectedIndex > -1)
            { GetAutoForm(SubTables[AnyTable_combo.SelectedIndex]); }
        }

        private void Increments_button_Click(object sender, EventArgs e)
        {
            new Increment_Form().ShowDialog();
        }

        private void DB_button_Click(object sender, EventArgs e)
        {
            new DB_Form(data.SynchPool, true).ShowDialog();
        }

        private void Period_Box_TextChanged(object sender, EventArgs e)
        {
            DataBase.DateYM_Only_Dinamic((TextBox)sender);
        }

        private void Protoks_button_Click(object sender, EventArgs e)
        {
            int YM;
            if (Period_Box.Text.Length == 7)
            {
                int Y, M;
                Y = Convert.ToInt32(Period_Box.Text.Substring(3, 4));
                M = Convert.ToInt32(Period_Box.Text.Substring(0, 2));

                YM = ATMisc.GetYMFromYearMonth(Y, M);

                new Protok_Form(YM, 0, false, true, true).ShowDialog();
            }
        }

        private void EmplueeEdit_button_Click(object sender, EventArgs e)
        {
            new Employe_Form().ShowDialog();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            var SFD = new SaveFileDialog();

            SFD.InitialDirectory = DataBase.table.SubTable.SavePath;
            SFD.FileName = "Таблицы.xls";
            SFD.Title = String.Concat("Выгрузка данных из таблиц");

            if (SFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            { return; }

            var path = SFD.FileName.Substring(0, SFD.FileName.Length - Path.GetFileName(SFD.FileName).Length);

            for (int i = 0; i < data.T1.Tables.Count; i++)
            {
                if (data.T1.Tables[i].RemoteType != DataBase.RemoteType.Local)
                {
                    var SubTable = data.T1.Tables[i].CreateSubTable(false);
                    SubTable.QUERRY(DataBase.State.None).SHOW.DO();

                    SaveTable_Form.SaveXLS(SubTable, path + SubTable.Parent.Name + ".xls", 1, SubTable.Rows.Count, true);
                }
            }
        }
        struct LoadTable
        {
            public LoadTable(DataBase.ISTable Table, string FileName)
            {
                this.Table = Table;
                this.FileName = FileName;
            }

            public readonly DataBase.ISTable Table;
            public readonly string FileName;
        }
        private void LoadButton_Click(object sender, EventArgs e)
        {
            var OFD = new OpenFileDialog();

            OFD.InitialDirectory = DataBase.table.SubTable.SavePath;
            OFD.Title = String.Concat("Загрузка данных из таблиц");

            if (OFD.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            { return; }

            var path = OFD.FileName.Substring(0, OFD.FileName.Length - Path.GetFileName(OFD.FileName).Length);

            var Tables = new LoadTable[data.T1.Tables.Count];

            for (int i = 0; i < data.T1.Tables.Count; i++)
            {
                var SubTable = data.T1.Tables[i].CreateSubTable(false);

                if (SubTable.Parent.RemoteType != DataBase.RemoteType.Local)
                {
                    if (File.Exists(path + SubTable.Parent.Name + ".xls"))
                    { Tables[i] = new LoadTable(SubTable, path + SubTable.Parent.Name + ".xls"); }
                    else if (File.Exists(path + SubTable.Parent.Name + ".xlsx"))
                    { Tables[i] = new LoadTable(SubTable, path + SubTable.Parent.Name + ".xlsx"); }
                    else
                    {
                        MessageBox.Show("Файл " + path + SubTable.Parent.Name + ".xls/.xlsx не существует");
                        return;
                    }
                }
            }

            for (int i = 0; i < data.T1.Tables.Count; i++)
            {
                if (Tables[i].Table != null)
                {
                    try { Tables[i].Table.Rows.LoadFromFile(Tables[i].FileName, false, true); }
                    catch (Exception) { MessageBox.Show("Ошибка чтения файла (проверьте не открыт ли файл)"); }
                }
            }
        }
    }
}
