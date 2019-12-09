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
    public partial class MErrorList_Form : Form
    {
        public MErrorList_Form(uint MarkID, uint NormID)
        {
            InitializeComponent();
            Diap = new Diaps_class(MarkID, NormID, MError_Grid);

            Add_button.Enabled = Delete_button.Enabled = data.User<uint>(C.User.UType) != (uint)data.UType.Employe;
        }
        
        public class Diaps_class
        {
            public Diaps_class(uint MarkID, uint NormID, DataGridView Grid)
            {
                this.MarkID = MarkID;
                this.NormID = NormID;

                NM = RCache.Marks[MarkID].GetNorm(NormID);

                for (int i = 0; i < NM.Ranges.Count; i++)
                    Diap.Add(new Diap_class(this, NM.Ranges[i].ID));

                this.Grid = Grid;

                Grid.Columns.Add("Number", "Номер");
                Grid.Columns.Add("Diapazon", "Диапазон");
                Grid.Columns.Add("Percent", "Тип");
                Grid.Columns.Add("Volume", "Значение");
                Grid.Columns[0].Width = 35;
                Grid.Columns[1].Width = 120;
                Grid.Columns[2].Width = 100;
                Grid.Columns[3].Width = 40;

                Grid.CellValueNeeded += Grid_CellValueNeeded;
                if (data.User<uint>(C.User.UType) != (uint)data.UType.Employe)
                    Grid.CellDoubleClick += Grid_CellDoubleClick;

                Grid.VirtualMode = true;
                Grid.ReadOnly = true;

                Grid.RowCount = Diap.Count;
            }
            uint MarkID;
            uint NormID;

            public enum MError : byte { Number, Diapazon, Percent, Volume };

            public class Diap_class
            {
                public Diap_class(Diaps_class Parent, uint ID)
                {
                    this.ID = ID;
                    this.Parent = Parent;
                }

                public readonly uint ID;
                readonly Diaps_class Parent;
                public uint MarkID { get { return T.MError.Rows.Get_UnShow<uint>(ID, C.MError.Mark); } }
                public double From {
                    get { return T.MError.Rows.Get_UnShow<double>(ID, C.MError.From); }
                    set { T.MError.Rows.Set(ID, C.MError.From, value); } 
                }
                public double To 
                {
                    get { return T.MError.Rows.Get_UnShow<double>(ID, C.MError.To); }
                    set { T.MError.Rows.Set(ID, C.MError.To, value); } 
                }
                public double Volume { get { return T.MError.Rows.Get_UnShow<double>(ID, C.MError.Volume); } }
                public bool Percent { get { return T.MError.Rows.Get_UnShow<bool>(ID, C.MError.Percent); } }
                public string Range { get { return RCache.Marks_class.Mark_class.GetRange(From, To); } }

                public void SetVolumes(double From, double To, double Volume, bool Percent)
                {
                    G.MError.QUERRY().SET.C(C.MError.From, From).C(C.MError.To, To).C(C.MError.Volume, Volume).C(C.MError.Percent, Percent).WHERE.ID(ID).DO();
                }
            }

            public readonly RCache.Marks_class.Mark_class.INorm NM;
            DataGridView Grid;
            List<Diap_class> Diap = new List<Diap_class>();

            public Diap_class this[int Index] { get { return Diap[Index]; } }

            public int Count { get { return Diap.Count; } }

            private void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
            {
                switch ((Diaps_class.MError)e.ColumnIndex)
                {
                    case Diaps_class.MError.Diapazon:
                        e.Value = Diap[e.RowIndex].Range;
                        break;
                    case Diaps_class.MError.Number:
                        e.Value = (e.RowIndex + 1);
                        break;
                    case Diaps_class.MError.Percent:
                        if (Diap[e.RowIndex].Percent)
                            e.Value = "Процент";
                        else
                            e.Value = "Абсолютное";
                        break;
                    case MError.Volume:
                        e.Value = Diap[e.RowIndex].Volume;
                        break;
                }
            }

            private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            {
                if (Grid.CurrentCell == null) return;

                Change(Grid.CurrentCell.RowIndex);
            }
            /// <summary>Удалить диапазон</summary>
            public void RemoveAt(int Index) 
            {
                if (Index > 0 && Index < Diap.Count - 1)
                { Diap[Index - 1].To = Diap[Index + 1].From; }

                Grid.RowCount--;
                G.MError.Rows.Delete(Diap[Index].ID);
                Diap.RemoveAt(Index);
                RCache.Marks[MarkID].GetNorm(NormID).Ranges.RemoveAt(Index);

                Grid.Invalidate();
            }

            /// <summary>Добавить диапазон визуальной формой</summary>
            public void Add()
            {
                new MErrorEdit_Form(this).ShowDialog();
            }
            /// <summary>Добавить диапазон</summary>
            public bool Add(double From, double To, double Volume, bool Percent)
            {
                if ((bool)G.MError.QUERRY()
                    .ADD
                        .C(C.MError.From, From)
                        .C(C.MError.To, To)
                        .C(C.MError.Volume, Volume)
                        .C(C.MError.Percent, Percent)
                        .C(C.MError.Mark, this.NM.Mark.ID)
                        .C(C.MError.Norm, this.NM.NormID)
                    .DO()[0].Value)
                {
                    var newDiap = new Diap_class(this, G.MError.Rows.GetID(G.MError.Rows.Count - 1));
                    Diap.Add(newDiap);
                    Grid.RowCount = Diap.Count;

                    NM.Ranges.Add(new RCache.Marks_class.Mark_class.Ranges_struct(NM.Mark, newDiap.ID));
                    return true;
                }
                else
                {
                    return false;
                }
            }

            /// <summary>Изменить диапазон визуальной формой</summary>
            public void Change(int Index)
            {
                new MErrorEdit_Form(this, Index).ShowDialog();
            }

            /// <summary>Изменить диапазон</summary>
            public bool Change(int Index, double From, double To, double Volume, bool Percent)
            {
                var MEC = new DataBase.FastSet_class(G.MError, Diap[Index].ID);
                MEC.C(C.MError.From, From);
                MEC.C(C.MError.To, To);
                MEC.C(C.MError.Volume, Volume);
                MEC.C(C.MError.Percent, Percent);
                var Ret = MEC.DO();
                Grid.InvalidateRow(Index);
                return Ret;
            }
        }

        Diaps_class Diap;

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Add_button_Click(object sender, EventArgs e)
        {
            Diap.Add();
        }

        private void Delete_button_Click(object sender, EventArgs e)
        {
            if (MError_Grid.CurrentCell == null) return;

            Diap.RemoveAt(MError_Grid.CurrentCell.RowIndex);
        }
    }
}
