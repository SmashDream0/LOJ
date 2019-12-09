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
    public partial class Increment_Form : Form
    {
        public Increment_Form()
        {
            InitializeComponent();

            Description_label.Text = "Инкрементировано: " + Tables.Increm.ToString();

            Increment_Grid.RowCount = Tables.Count;
        }

        public class Table_class
        {
            public Table_class()
            {
                var Tables = new List<Table_struct>();

                for (int i = 0; i < data.T1.Tables.Count; i++)
                {
                    if (data.T1.Tables[i].RemoteType != DataBase.RemoteType.Local)
                    { Tables.Add(new Table_struct(data.T1.Tables[i])); }
                }

                this.Tables = Tables.ToArray();
            }

            public void AddIncrement(uint increment)
            {
                for (int i = 0; i < Tables.Length; i++)
                { Tables[i].Increment(increment); }
            }

            public uint Increm
            {
                get
                {
                    var Multi = Tables[0].Multiply;

                    for (int i = 1; i < Tables.Length; i++)
                    {
                        if (Multi != Tables[i].Multiply)
                        { return 0; }
                    }

                    return Multi;
                }
                set
                {
                    for (int i = 0; i < Tables.Length; i++)
                    { Tables[i].Increment(value); }
                }
            }

            public struct Table_struct
            {
                public Table_struct(DataBase.ITable Table)
                { this.Table = Table; }

                readonly DataBase.ITable Table;
                public const uint IncrementCount = 100000000;   //новый инкремент действует примерно с конца месяца 08.2016

                public string Name { get { return Table.AlterName; } }
                public uint Used { get { return (Multiply < 1 ? IncrementCount - Table.DataSource.Increment : Table.DataSource.Increment - Multiply * IncrementCount); } }
                /*public uint Limit 
                {
                    get
                    {
                        return Table.DataSource.Increment + IncrementCount + 1; 
                    }
                }*/
                public uint CurrentIncrement
                { 
                    get { return Table.DataSource.Increment; }
                }

                public void SetIncrement(uint Increment)
                { Table.DataSource.Increment = Increment; }

                public uint Multiply
                {
                    get
                    { return (Table.DataSource.Increment - (Table.DataSource.Increment % IncrementCount)) / IncrementCount; }
                }

                public void Increment(uint Multiply)
                {
                    var NewInc = Multiply * IncrementCount;

                    Table.DataSource.Increment = (NewInc == 0 ? 1 : NewInc);
                }

                public override string ToString()
                {
                    return Name + ": " + CurrentIncrement.ToString() + "-" + Used;
                }
            }

            public Table_struct[] Tables;

            public int Count { get { return Tables.Length; } }

            public Table_struct this[int Index] { get { return Tables[Index]; } }

            public override string ToString()
            {
                return Count.ToString();
            }
        }

        public static Table_class Tables = new Table_class();

        enum Columns_enum : byte { Name, IVolume, Multiply, Limit };

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Increment_button_Click(object sender, EventArgs e)
        {
            Tables.Increm++;

            Description_label.Text = "Инкрементировано: " + Tables.Increm.ToString();

            Increment_Grid.Invalidate();
        }

        private void Decrement_button_Click(object sender, EventArgs e)
        {
            Tables.Increm--;

            Description_label.Text = "Инкрементировано: " + Tables.Increm.ToString();

            Increment_Grid.Invalidate();
        }

        private void Increment_Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                switch ((Columns_enum)e.ColumnIndex)
                {
                    case Columns_enum.IVolume:
                        e.Value = Tables[e.RowIndex].CurrentIncrement.ToString();
                        break;
                    case Columns_enum.Limit:
                        e.Value = Tables[e.RowIndex].Used + " из " + Table_class.Table_struct.IncrementCount.ToString();
                        break;
                    case Columns_enum.Name:
                        e.Value = Tables[e.RowIndex].Name;
                        break;
                    case Columns_enum.Multiply:
                        e.Value = Tables[e.RowIndex].Multiply;
                        break;
                }
            }
        }

        private void Clear_button_Click(object sender, EventArgs e)
        {
            Tables.Increm = 0;

            Description_label.Text = "Инкрементировано: " + Tables.Increm.ToString();
        }

        private void Increment_Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex > -1)
            {
                switch ((Columns_enum)e.ColumnIndex)
                {
                    case Columns_enum.IVolume:
                        uint Value;
                        if (uint.TryParse((string)e.Value, out Value))
                        {
                            Tables[e.RowIndex].SetIncrement(Value); 
                        }
                        break;
                }
            }
        }
    }
}
