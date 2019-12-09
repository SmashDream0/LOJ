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
    public partial class MarksCompanyEdit_Form : Form
    {
        public MarksCompanyEdit_Form()
        {
            InitializeComponent();

            this.Marks = new MarkVObject_class(this, Show_check);
        }

        /// <summary>визуальное представление лимитов по показателям и нормам(Mark Visual Object)</summary>
        class MarkVObject_class : IDisposable
        {
            public MarkVObject_class(Form form, CheckBox Show_check)
            {
                this.form = form;
                this.Show_check = Show_check;
                this.Show_check.CheckedChanged += Show_check_CheckedChanged;
                this.Norm = T.Norm.CreateSubTable(false);

                form.Resize += Form_Resize;

                MarkGroup = new GroupBox();
                MarkGroup.Parent = form;
                MarkGroup.Location = new Point(data.Divide, data.LocationY);
                MarkGroup.Resize += group_Resize;
                MarkGroup.Text = "Показатели";

                MarkUp = new Button();
                MarkUp.Parent = MarkGroup;
                MarkUp.Click += MarkUp_button_Click;
                MarkUp.Location = new Point(data.Divide, data.ButtonsLocationY);
                MarkUp.Text = "/\\";
                MarkUp.Size = new Size(data.ButtonsHeight * 2, data.ButtonsHeight);

                MarkDown = new Button();
                MarkDown.Parent = MarkGroup;
                MarkDown.Click += MarkDown_button_Click;
                MarkDown.Location = new Point(MarkUp.Location.X + MarkUp.Size.Width + data.Divide, data.ButtonsLocationY);
                MarkDown.Text = "\\/";
                MarkDown.Size = MarkUp.Size;

                MarkAdd = new Button();
                MarkAdd.Parent = MarkGroup;
                MarkAdd.Click += MarkAdd_button_Click;
                MarkAdd.Location = new Point(MarkDown.Location.X + MarkDown.Size.Width + data.Divide, data.ButtonsLocationY);
                MarkAdd.Text = "Добавить";

                Grid = new DataGridView();
                Grid.Parent = MarkGroup;
                Grid.AllowUserToAddRows = false;
                Grid.AllowUserToDeleteRows = false;
                Grid.AllowUserToOrderColumns = false;
                Grid.AllowUserToResizeRows = false;
                Grid.Location = new Point(data.Divide, data.GridLocationY);
                Grid.RowHeadersVisible = false;
                Grid.ScrollBars = ScrollBars.Vertical;

                Grid.Columns.Clear();
                Grid.Columns.Add("ID", "ID");
                Grid.Columns.Add("Code", "Код");
                Grid.Columns.Add("Name", "Наименование");
                Grid.Columns.Add("EdType", "Ед.изм.");
                Grid.Columns.Add("OPType", "Ед.изм.вывода");
                Grid.Columns.Add("Round", "Точность");
                {
                    var column = new DataGridViewColumn();
                    column.CellTemplate = new DataGridViewCheckBoxCell();
                    column.Name = "ShowZr";
                    column.HeaderText = "Показывать пустое";

                    Grid.Columns.Add(column);
                }
                {
                    var column = new DataGridViewColumn();
                    column.CellTemplate = new DataGridViewCheckBoxCell();
                    column.Name = "Exp";
                    column.HeaderText = "a*(b^c)";

                    Grid.Columns.Add(column);
                }
                {
                    var column = new DataGridViewColumn();
                    column.CellTemplate = new DataGridViewComboBoxCell();
                    column.ValueType = typeof(string);

                    for (int i = 0; i < G.VarType.Rows.Count; i++)
                    { ((DataGridViewComboBoxCell)column.CellTemplate).Items.Add(G.VarType.Rows.Get<string>(i, C.VarType.Name)); }

                    column.Name = "VarType";
                    column.HeaderText = "Тип";

                    Grid.Columns.Add(column);
                }

                Grid.Columns[(int)Marks_enum.ID].FillWeight = 1f;
                Grid.Columns[(int)Marks_enum.Code].FillWeight = 1f;
                Grid.Columns[(int)Marks_enum.Name].FillWeight = 6f;
                Grid.Columns[(int)Marks_enum.EdType].FillWeight = 2f;
                Grid.Columns[(int)Marks_enum.OPType].FillWeight = 2f;
                Grid.Columns[(int)Marks_enum.Round].FillWeight = 1f;
                Grid.Columns[(int)Marks_enum.Exp].FillWeight = 2.5f;
                Grid.Columns[(int)Marks_enum.VarType].FillWeight = 2.5f;
                Grid.Columns[(int)Marks_enum.ShowZr].FillWeight = 2.5f;
                Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                CreateGroups();

                Grid.VirtualMode = true;
                Grid.RowCount = RCache.Marks.Count;
                Grid.SelectionChanged += Grid_SelectionChanged;
                Grid.CellValuePushed += Grid_CellValuePushed;
                Grid.CellValueNeeded += Grid_CellValueNeeded;
                Grid.Scroll += Grid_Scroll;
                Grid.ColumnWidthChanged += Grid_ColumnWidthChanged;
                Grid.CellBeginEdit += Grid_CellBeginEdit;
                Grid.CellDoubleClick += Grid_CellDoubleClick;
                Grid.KeyDown += Grid_KeyDown;

                MarkUp.Enabled = MarkDown.Enabled = data.User<uint>(C.User.UType) != (uint)data.UType.Employe;
                Grid.ReadOnly = !MarkUp.Enabled;
            }

            interface INormObject : IDisposable
            {
                void CreateGridColumns();
                bool Show { get; set; }
                DataGridView Grid { get; }
                GroupBox Limit_group { get; }
                Button MerrorEdit { get; }
                Button SH { get; }
                int Count { get; }
                bool Disposed { get; }
            }
            /// <summary>визуальное представление лимитов по нормам</summary>
            class NormObject_class : IDisposable
            {
                public NormObject_class(MarkVObject_class Parent, uint NormID)
                {
                    this.Parent = Parent;
                    this.NormID = NormID;
                    this.ntype = (data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType);
                    this.Index = RCache.Marks.Norms.GetNormIndex(NormID);

                    Limit_group = new GroupBox();
                    Limit_group.Text = T.Norm.Rows.Get<string>(NormID, C.Norm.Name);
                    Limit_group.Parent = Parent.form;
                    Limit_group.Location = new Point(data.Divide, data.LocationY);
                    Limit_group.Resize += group_Resize;

                    Grid = new DataGridView();
                    Grid.Parent = Limit_group;
                    Grid.AllowUserToAddRows = false;
                    Grid.AllowUserToDeleteRows = false;
                    Grid.AllowUserToOrderColumns = false;
                    Grid.AllowUserToResizeRows = false;
                    Grid.Location = new Point(data.Divide, data.GridLocationY);
                    Grid.RowHeadersVisible = false;
                    Grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                    Grid.ScrollBars = ScrollBars.None;

                    CreateGridColumns();

                    SH = new Button();
                    SH.Parent = Limit_group;
                    SH.Click += ShowHide_button_Click;
                    SH.Size = new Size(data.ButtonsHeight, data.ButtonsHeight);
                    SH.Location = new Point(data.Divide, data.ButtonsLocationY);
                    SH.Text = "-";

                    MerrorEdit = new Button();
                    MerrorEdit.Parent = Limit_group;
                    MerrorEdit.Click += MErrorEdit_button_Click;
                    MerrorEdit.Size = new Size(63, data.ButtonsHeight);
                    MerrorEdit.Location = new Point(SH.Location.X + SH.Size.Width + data.Divide, data.ButtonsLocationY);
                    MerrorEdit.Text = "Точность";

                    Grid.Resize += group_Resize;
                    Grid.Scroll += Parent.Grid_Scroll;
                    Grid.SelectionChanged += Parent.Grid_SelectionChanged;
                    Grid.ColumnWidthChanged += Parent.Grid_ColumnWidthChanged;

                    if (Show)
                    {
                        SH.Text = "-";
                        MerrorEdit.Visible = Grid.Visible = true;
                    }
                    else
                    {
                        SH.Text = "+";
                        MerrorEdit.Visible = Grid.Visible = false;
                    }

                    Grid.ReadOnly = data.User<uint>(C.User.UType) == (uint)data.UType.Employe;
                }

                public bool Show
                {
                    get { return T.Norm.Rows.Get<bool>(NormID, C.Norm.Show); }
                    set
                    {
                        if (value)
                        {
                            SH.Text = "-";
                            MerrorEdit.Visible = Grid.Visible = true;
                        }
                        else
                        {
                            SH.Text = "+";
                            MerrorEdit.Visible = Grid.Visible = false;
                        }

                        T.Norm.Rows.Set(NormID, C.Norm.Show, value);

                        Parent.CheckShows();
                    }
                }

                data.NType ntype;
                public bool Disposed { get; internal set; }

                bool NeedToChange { get { return !Disposed && (data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType) != ntype; } }
                protected int Index;
                protected uint NormID;

                public DataGridView Grid { get; internal set; }
                public GroupBox Limit_group { get; internal set; }
                public Button MerrorEdit { get; internal set; }
                public Button SH { get; internal set; }
                MarkVObject_class Parent;

                public void CreateGridColumns()
                {
                    Grid.Columns.Clear();
                    this.Count = RCache.Marks.Norms.GetSubElementsCount(NormID);
                    Grid.Columns.Add("Method", "Метод");
                    Grid.Columns[0].FillWeight = 3f;


                    if (this.Count == 0)
                    {
                        this.Count = 1;
                        Grid.ReadOnly = true;
                    }
                    else
                    {
                        for (int i = 0; i < Count; i++)
                        {
                            var PID = G.Podr.Rows.GetID(i);
                            Grid.Columns.Add(T.Podr.Rows.Get<string>(PID, C.Podr.ShrName), T.Podr.Rows.Get<string>(PID, C.Podr.ShrName));
                            Grid.Columns[Grid.Columns.Count - 1].FillWeight = 1f;
                        }

                        Grid.ReadOnly = false;
                    }
                }

                public int Count { get; internal set; }

                void group_Resize(object sender, EventArgs e)
                {
                    Grid.Size = new Size(Limit_group.Size.Width - Grid.Location.X - data.Divide, Limit_group.Size.Height - Grid.Location.Y - data.Divide);
                }

                void ShowHide_button_Click(object sender, EventArgs e)
                {
                    Show = !Show;
                    Parent.Form_Resize(null, null);
                }

                void MErrorEdit_button_Click(object sender, EventArgs e)
                {
                    new MErrorList_Form(RCache.Marks[Grid.CurrentCell.RowIndex].ID, NormID).ShowDialog();
                }

                public void Dispose()
                {
                    if (NeedToChange)
                    {
                        this.Limit_group.Parent = null;
                        this.MerrorEdit.Parent = null;
                        this.Grid.Parent = null;

                        if (this.Grid.IsCurrentCellInEditMode)
                        { this.Grid.EndEdit(); }

                        this.Limit_group.Dispose();
                        this.MerrorEdit.Dispose();
                        this.Grid.Dispose();
                        this.Disposed = true;
                    }
                }

                public class PodrNormVObject_class : NormObject_class, INormObject
                {
                    public PodrNormVObject_class(MarkVObject_class Parent, uint NormID)
                        : base(Parent, NormID)
                    {
                        CreateGridColumns();

                        Grid.CellValueNeeded += Grid_CellValueNeeded;
                        Grid.CellValuePushed += Grid_CellValuePushed;
                        Grid.CellPainting += Grid_CellPainting;
                        Grid.CellToolTipTextNeeded += Grid_CellToolTipTextNeeded;

                        if (Show)
                        {
                            SH.Text = "-";
                            MerrorEdit.Visible = Grid.Visible = true;
                        }
                        else
                        {
                            SH.Text = "+";
                            MerrorEdit.Visible = Grid.Visible = false;
                        }
                    }

                    public new void CreateGridColumns()
                    {
                        Grid.Columns.Clear();
                        this.Count = RCache.Marks.Norms.GetSubElementsCount(NormID);
                        Grid.Columns.Add("Method", "Метод");
                        Grid.Columns[0].FillWeight = 3f;

                        if (this.Count == 0)
                        {
                            this.Count = 1;
                            Grid.ReadOnly = true;
                        }
                        else
                        {
                            for (int i = 0; i < Count; i++)
                            {
                                var PID = RCache.Marks.Norms.GetPodrID(this.NormID, i);
                                Grid.Columns.Add(T.Podr.Rows.Get<string>(PID, C.Podr.ShrName), T.Podr.Rows.Get<string>(PID, C.Podr.ShrName));
                                Grid.Columns[Grid.Columns.Count - 1].FillWeight = 1f;
                            }

                            Grid.ReadOnly = false;
                        }
                        Grid.VirtualMode = true;
                        Grid.RowCount = RCache.Marks.Count;
                    }

                    private void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
                    {
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                e.Value = RCache.Marks[e.RowIndex].GetNorm(Index).MethodName;
                                break;
                            default:
                                e.Value = RCache.Marks[e.RowIndex].GetNorm(Index).Station(e.ColumnIndex - 1).Range;
                                break;
                        }
                    }

                    private void Grid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
                    {
                        if (e.RowIndex > -1 && e.ColumnIndex > 0)
                        {
                            if (RCache.Marks[e.RowIndex].GetNorm(Index).Station(e.ColumnIndex - 1).Range.Length > 0)
                            { e.ToolTipText = "диапазон " + RCache.Marks[e.RowIndex].GetNorm(Index).Station(e.ColumnIndex - 1).FullRange; }
                            else
                            { e.ToolTipText = "нет диапазона"; }
                        }
                    }

                    private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
                    {
                        if (e.RowIndex > -1 && e.ColumnIndex > 0)
                        { e.CellStyle.BackColor = Color.White; }
                    }

                    private void Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
                    {
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                RCache.Marks[e.RowIndex].GetNorm(Index).MethodName = (string)e.Value;
                                break;
                            default:
                                if (RCache.Marks[e.RowIndex].GetNorm(Index).MethodName.Length == 0)
                                {
                                    MessageBox.Show("Сначало надо заполнить название метода проведения анализов", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else
                                { RCache.Marks[e.RowIndex].GetNorm(Index).Station(e.ColumnIndex - 1).Range = (string)e.Value; }

                                break;
                        }
                    }
                }
                public class VolumeNormVObject_class : NormObject_class, INormObject
                {
                    public VolumeNormVObject_class(MarkVObject_class Parent, uint NormID)
                        : base(Parent, NormID)
                    {
                        CreateGridColumns();

                        Grid.CellValueNeeded += Grid_CellValueNeeded;
                        Grid.CellValuePushed += Grid_CellValuePushed;
                        Grid.CellPainting += Grid_CellPainting;
                        Grid.CellToolTipTextNeeded += Grid_CellToolTipTextNeeded;
                        //Grid.CellDoubleClick += Grid_CellDoubleClick;

                        if (Show)
                        {
                            SH.Text = "-";
                            MerrorEdit.Visible = Grid.Visible = true;
                        }
                        else
                        {
                            SH.Text = "+";
                            MerrorEdit.Visible = Grid.Visible = false;
                        }
                    }

                    public new void CreateGridColumns()
                    {
                        Grid.Columns.Clear();
                        this.Count = RCache.Marks.Norms.GetSubElementsCount(NormID);
                        Grid.Columns.Add("Method", "Метод");
                        Grid.Columns[0].FillWeight = 3f;

                        if (this.Count == 0)
                        {
                            this.Count = 1;
                            Grid.ReadOnly = true;
                        }
                        else
                        {
                            for (int i = 0; i < Count; i++)
                            {
                                var PID = RCache.Marks.Norms.GetVolumeID(this.NormID, i);
                                Grid.Columns.Add(T.OLocation.Rows.Get<string>(PID, C.OLocation.ShrName), T.OLocation.Rows.Get<string>(PID, C.OLocation.ShrName));
                                Grid.Columns[Grid.Columns.Count - 1].FillWeight = 1f;
                            }

                            Grid.ReadOnly = false;
                        }
                        Grid.VirtualMode = true;
                        Grid.RowCount = RCache.Marks.Count;
                    }

                    private void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
                    {
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                e.Value = RCache.Marks[e.RowIndex].GetNorm(Index).MethodName;
                                break;
                            default:
                                e.Value = RCache.Marks[e.RowIndex].GetNorm(Index).Volume(e.ColumnIndex - 1).Range;
                                break;
                        }
                    }

                    private void Grid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
                    {
                        if (e.RowIndex > -1 && e.ColumnIndex > 0)
                        {
                            if (RCache.Marks[e.RowIndex].GetNorm(Index).Volume(e.ColumnIndex - 1).Range.Length > 0)
                            { e.ToolTipText = "диапазон " + RCache.Marks[e.RowIndex].GetNorm(Index).Volume(e.ColumnIndex - 1).FullRange; }
                            else
                            { e.ToolTipText = "нет диапазона"; }
                        }
                    }

                    private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
                    {
                        if (e.RowIndex > -1 && e.ColumnIndex > 0)
                        { e.CellStyle.BackColor = Color.White; }
                    }

                    /*private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
                    {
                        if ((uint)data.UType.Employe != data.User<uint>(C.User.UType) && e.ColumnIndex > 0)
                        {
                            RCache.Marks[e.RowIndex].GetNorm(base.Index)[e.ColumnIndex - 1].Ground = !RCache.Marks[e.RowIndex].GetNorm(Index)[e.ColumnIndex - 1].Ground;
                        }
                    }*/

                    private void Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
                    {
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                RCache.Marks[e.RowIndex].GetNorm(Index).MethodName = (string)e.Value;
                                break;
                            default:
                                if (RCache.Marks[e.RowIndex].GetNorm(Index).MethodName.Length == 0)
                                {
                                    MessageBox.Show("Сначало надо заполнить название метода проведения анализов", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else
                                { RCache.Marks[e.RowIndex].GetNorm(Index).Volume(e.ColumnIndex - 1).Range = (string)e.Value; }
                                break;
                        }
                    }
                }
                public class MarkNormVObject_class : NormObject_class, INormObject
                {
                    public MarkNormVObject_class(MarkVObject_class Parent, uint NormID)
                        : base(Parent, NormID)
                    {
                        Grid.Columns.Clear();
                        Grid.Columns.Add("Method", "Метод");
                        Grid.Columns.Add("Range", "Норма");
                        Grid.Columns[0].FillWeight = 3f;
                        Grid.Columns[1].FillWeight = 1f;
                        Grid.RowCount = RCache.Marks.Count;

                        Grid.CellValueNeeded += Grid_CellValueNeeded;
                        Grid.CellValuePushed += Grid_CellValuePushed;
                        Grid.CellPainting += Grid_CellPainting;
                        //Grid.CellDoubleClick += Grid_CellDoubleClick;
                        Grid.CellToolTipTextNeeded += Grid_CellToolTipTextNeeded;
                        Grid.Scroll += Parent.Grid_Scroll;
                        Grid.SelectionChanged += Parent.Grid_SelectionChanged;
                        Grid.ColumnWidthChanged += Parent.Grid_ColumnWidthChanged;

                        Grid.VirtualMode = true;
                        Grid.RowCount = RCache.Marks.Count;
                    }

                    private void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
                    {
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                e.Value = RCache.Marks[e.RowIndex].GetNorm(Index).MethodName;
                                break;
                            case 1:
                                e.Value = RCache.Marks[e.RowIndex].GetNorm(Index).Range.Range;
                                break;
                        }
                    }

                    private void Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
                    {
                        switch (e.ColumnIndex)
                        {
                            case 0:
                                RCache.Marks[e.RowIndex].GetNorm(Index).MethodName = (string)e.Value;
                                break;
                            case 1:
                                RCache.Marks[e.RowIndex].GetNorm(Index).Range.Range = (string)e.Value;
                                break;
                        }
                    }

                    private void Grid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
                    {
                        if (e.RowIndex > -1 && e.ColumnIndex > 0)
                        { e.CellStyle.BackColor = Color.White; }
                    }

                    private void Grid_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
                    {
                        if (e.RowIndex > -1 && e.ColumnIndex > 0)
                        {
                            if (RCache.Marks[e.RowIndex].GetNorm(Index).Range.Range.Length > 0)
                            { e.ToolTipText = "диапазон " + RCache.Marks[e.RowIndex].GetNorm(Index).Range.FullRange; }
                            else
                            { e.ToolTipText = "нет диапазона"; }
                        }
                    }
                }
            }

            /// <summary>Колонки таблицы показателей</summary>
            enum Marks_enum : byte
            {
                /// <summary>Уникальный идентификатор показателя</summary>
                ID,
                /// <summary>Код показателя, согласно законному акту</summary>
                Code,
                /// <summary>Наименование показателя</summary>
                Name,
                /// <summary>Единица измерения показателя</summary>
                EdType,
                /// <summary>Единица измерения вывода</summary>
                OPType,
                /// <summary>Количество знаков после запятой</summary>
                Round,
                /// <summary>Показывать ноль(не используется)</summary>
                ShowZr,
                /// <summary>Экспотенциальный вывод</summary>
                Exp,
                /// <summary>Тип значения</summary>
                VarType,
            };

            DataBase.ISTable Norm;
            /// <summary>Родитель визуальных элементов</summary>
            Control form;
            /// <summary></summary>
            GroupBox MarkGroup;
            /// <summary>Табличное представление показателей</summary>
            DataGridView Grid;
            /// <summary>Кнопка - поднять показатель</summary>
            Button MarkUp;
            /// <summary>Кнопка - опустить показатель</summary>
            Button MarkDown;
            /// <summary>Кнопка - добавить показатель</summary>
            Button MarkAdd;
            /// <summary>Показать/скрыть все нормы</summary>
            CheckBox Show_check;

            /// <summary>Массив всех норм</summary>
            INormObject[] Norms;

            /// <summary></summary>
            int SummCount;
            /// <summary>Можно-ли действовать в пределах события</summary>
            bool CanDo = true;

            /// <summary>Создать визуальное представление норм</summary>
            public void CreateGroups()
            {
                //Перезагружаю нормы
                RCache.Marks.ReloadNorm();

                this.Norm.QUERRY().SHOW.WHERE.AC(C.Norm.NType).More.BV<uint>(0).AND.C(C.Norm.Enabled, true).DO();

                //Уничтожаю визуальные представление норм, если есть, 
                if (Norms != null)
                {
                    for (int i = 0; i < Norms.Length; i++)
                    { Norms[i].Dispose(); }

                    if (Norms.Length != this.Norm.Rows.Count)
                    { Array.Resize(ref Norms, this.Norm.Rows.Count); }
                }
                else
                {//Создаю новое визуальное представление норм
                    Norms = new INormObject[this.Norm.Rows.Count];
                }

                int Count = 0;
                int Correction = 0;
                int ShowCheck = 0;

                CanDo = false;

                for (int i = 0; i < this.Norm.Rows.Count; i++)
                {
                    var NormID = this.Norm.Rows.GetID(i);
                    {
                        switch ((data.NType)T.Norm.Rows.Get_UnShow<uint>(NormID, C.Norm.NType))
                        {
                            case data.NType.Mark:
                                if (Norms[Count] == null || Norms[Count].Disposed)
                                { Norms[Count] = new NormObject_class.MarkNormVObject_class(this, NormID); }
                                Count++;
                                break;
                            case data.NType.PodrAll:
                            case data.NType.PodrK:
                            case data.NType.PodrV:
                                if (Norms[Count] == null || Norms[Count].Disposed)
                                { Norms[Count] = new NormObject_class.PodrNormVObject_class(this, NormID); }
                                Count++;
                                break;
                            case data.NType.Volume:
                                if (Norms[Count] == null || Norms[Count].Disposed)
                                { Norms[Count] = new NormObject_class.VolumeNormVObject_class(this, NormID); }
                                Count++;
                                break;
                        }

                        if (T.Norm.Rows.Get_UnShow<bool>(NormID, C.Norm.Show))
                        { ShowCheck++; }
                    }
                }

                if (Correction > 0)
                { Array.Resize(ref Norms, Norms.Length - Correction); }

                if (ShowCheck > 0)
                { Show_check.CheckState = (Norms.Length == ShowCheck ? CheckState.Checked : CheckState.Indeterminate); }
                else
                { Show_check.Checked = false; }

                CanDo = true;
                Form_Resize(null, null);
            }

            void CountSummCount()
            {
                SummCount = data.ForMark;
                for (int i = 0; i < Norms.Length; i++)
                {
                    if (Norms[i].Show)
                    { SummCount += Norms[i].Count * data.PerPodr + data.ForMethod; }
                }
            }

            void CheckAll(bool Check)
            {
                CanDo = false;

                string text;

                text = (Check ? "-" : "+");

                for (int i = 0; i < Norms.Length; i++)
                {
                    Norms[i].Show = Check;
                    Norms[i].MerrorEdit.Visible = Norms[i].Grid.Visible = Check;
                    Norms[i].SH.Text = text;
                }

                Form_Resize(null, null);

                CanDo = true;
            }

            void CheckShows()
            {
                CanDo = false;

                bool All = false;

                if (Norms.Length > 0)
                {
                    All = Norms[0].Show;

                    for (int i = 1; i < Norms.Length; i++)
                    {
                        if (All != Norms[i].Show)
                        {
                            Show_check.CheckState = CheckState.Indeterminate;
                            goto TheEnd;
                        }
                    }
                }

                Show_check.CheckState = (All ? CheckState.Checked : CheckState.Unchecked);

            TheEnd: ;
                CanDo = true;
            }

            private void Grid_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
            {
                if ((uint)data.UType.Employe == data.User<uint>(C.User.UType))
                { e.Cancel = true; }
                else
                {
                    switch ((Marks_enum)e.ColumnIndex)
                    {
                        case Marks_enum.ID:
                        case Marks_enum.EdType:
                        case Marks_enum.OPType:
                            e.Cancel = true;
                            break;
                    }
                }
            }

            private void Grid_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
            {
                if (CanDo)
                {
                    CanDo = false;

                    var Grid = (DataGridView)sender;

                    if (e.Column.Index == 0)
                    {
                        for (int i = 0; i < Norms.Length; i++)
                        {
                            if (Norms[i].Grid != Grid)
                            { Norms[i].Grid.Columns[0].FillWeight = e.Column.FillWeight; }
                        }
                    }

                    CanDo = true;
                }
            }

            void Show_check_CheckedChanged(object sender, EventArgs e)
            {
                if (CanDo)
                {
                    CanDo = false;

                    CheckAll(Show_check.Checked);

                    CanDo = true;
                }
            }

            private void Grid_SelectionChanged(object sender, EventArgs e)
            {
                if (!CanDo || ((DataGridView)sender).CurrentCell == null) return;

                var Grid = (DataGridView)sender;

                int SelectedRowIndex = Grid.CurrentCell.RowIndex;
                CanDo = false;

                if (Grid != this.Grid)
                {
                    this.Grid.ClearSelection();
                    this.Grid.Rows[SelectedRowIndex].Selected = true;
                    this.Grid.CurrentCell = this.Grid[0, SelectedRowIndex];
                }

                for (int i = 0; i < Norms.Length; i++)
                {
                    if (Grid != Norms[i].Grid)
                    {
                        Norms[i].Grid.ClearSelection();
                        Norms[i].Grid.Rows[SelectedRowIndex].Selected = true;
                        Norms[i].Grid.CurrentCell = Norms[i].Grid[0, SelectedRowIndex];
                    }
                }

                CanDo = true;
            }

            private void Grid_Scroll(object sender, ScrollEventArgs e)
            {
                if (CanDo && e.ScrollOrientation == ScrollOrientation.VerticalScroll)
                {
                    CanDo = false;
                    Grid.FirstDisplayedScrollingRowIndex = e.NewValue;

                    for (int i = 0; i < Norms.Length; i++)
                    { Norms[i].Grid.FirstDisplayedScrollingRowIndex = e.NewValue; }

                    CanDo = true;
                }
            }

            private void MarkDown_button_Click(object sender, EventArgs e)
            {
                if (Grid.CurrentCell == null || Grid.CurrentCell.RowIndex == Grid.RowCount - 1)
                { return; }

                SetNumber(1);
            }

            private void MarkAdd_button_Click(object sender, EventArgs e)
            {
                var NewName = new SetText_Form("Введите наименование показателя");

                if (NewName.ShowDialog() == DialogResult.OK)
                {
                    var Values = new object[T.Mark.Columns.Count];
                    Values[C.Mark.Name] = NewName.ResultText;
                    Values[C.Mark.EdType] = (uint)1;
                    Values[C.Mark.VarType] = (uint)data.VarType.dbl;
                    Values[C.Mark.Number] = (byte)(RCache.Marks[RCache.Marks.Count - 1].Number + 1);

                    G.Mark.Rows.Add(Values);

                    RCache.Marks.Update();

                    this.Grid.RowCount = RCache.Marks.Count;

                    for (int i = 0; i < Norms.Length; i++)
                    { Norms[i].Grid.RowCount = this.Grid.RowCount; }

                    this.Grid.CurrentCell = this.Grid[0, this.Grid.RowCount - 1];
                }
            }

            private void MarkUp_button_Click(object sender, EventArgs e)
            {
                if (Grid.CurrentCell == null || Grid.CurrentCell.RowIndex == 0)
                { return; }

                SetNumber(-1);
            }

            void SetNumber(int Direction)
            {
                int RowIndex = Grid.CurrentCell.RowIndex;

                if (RowIndex + Direction < 0 || RowIndex + Direction == Grid.RowCount)
                { return; }

                RCache.Marks[RowIndex].Number = RowIndex + Direction + 1;
                Grid.InvalidateRow(RowIndex);
                Grid.InvalidateRow(RowIndex + Direction);
                Grid.CurrentCell = Grid[Grid.CurrentCell.ColumnIndex, RowIndex + Direction];
                Grid_SelectionChanged(Grid, null);
            }

            private void Grid_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
            {
                switch ((Marks_enum)e.ColumnIndex)
                {
                    case Marks_enum.Code:
                        int Code;
                        if (int.TryParse((string)e.Value, out Code))
                        { RCache.Marks[e.RowIndex].Code = Code; }
                        break;
                    case Marks_enum.Name:
                        RCache.Marks[e.RowIndex].Name = (string)e.Value;
                        break;
                    case Marks_enum.Round:
                        RCache.Marks[e.RowIndex].Round = Convert.ToByte(DataBase.NoABC_Byte_Static((string)e.Value));
                        break;
                    case Marks_enum.Exp:
                        RCache.Marks[e.RowIndex].Exp = (bool)e.Value;
                        break;
                    case Marks_enum.ShowZr:
                        RCache.Marks[e.RowIndex].ShowZero = (bool)e.Value;
                        break;
                    case Marks_enum.VarType:

                        if (e.Value != null)
                        {
                            for (int i = 0; i < G.VarType.Rows.Count; i++)
                            {
                                if (G.VarType.Rows.Get<string>(i, C.VarType.Name) == (string)e.Value)
                                {
                                    RCache.Marks[e.RowIndex].VarType = (data.VarType)G.VarType.Rows.GetID(i);
                                    break;
                                }
                            }
                        }
                        break;
                }
            }

            private void Grid_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
            {
                switch ((Marks_enum)e.ColumnIndex)
                {
                    case Marks_enum.ID:
                        e.Value = RCache.Marks[e.RowIndex].ID;
                        break;
                    case Marks_enum.Code:
                        e.Value = RCache.Marks[e.RowIndex].Code;
                        break;
                    case Marks_enum.Exp:
                        e.Value = RCache.Marks[e.RowIndex].Exp;
                        break;
                    case Marks_enum.Round:
                        e.Value = RCache.Marks[e.RowIndex].Round;
                        break;
                    case Marks_enum.Name:
                        e.Value = RCache.Marks[e.RowIndex].Name;
                        break;
                    case Marks_enum.EdType:
                        e.Value = RCache.Marks[e.RowIndex].EdType;
                        break;
                    case Marks_enum.OPType:
                        e.Value = RCache.Marks[e.RowIndex].OPType;
                        break;
                    case Marks_enum.ShowZr:
                        e.Value = RCache.Marks[e.RowIndex].ShowZero;
                        break;
                    case Marks_enum.VarType:
                        e.Value = T.VarType.Rows.Get<string>((uint)RCache.Marks[e.RowIndex].VarType, C.VarType.Name);
                        break;
                }
            }

            private void Grid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
            {
                if ((uint)data.UType.Employe == data.User<uint>(C.User.UType))
                { return; }

                switch ((Marks_enum)e.ColumnIndex)
                {
                    case Marks_enum.OPType:
                        if (RCache.Marks[e.RowIndex].EdTypeID == 0)
                        {
                            MessageBox.Show("Прежде чем выбирать единицу измерения вывода, нужно выбрать единицу измерения ввода.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            G.OPType.QUERRY().SHOW.WHERE.C(C.OPType.EdTypeF, RCache.Marks[e.RowIndex].EdTypeID).DO();
                            G.OPType.GetAutoForm((RowIndex) =>
                            {
                                RCache.Marks[e.RowIndex].OPTypeID = G.OPType.Rows.GetID(RowIndex);
                                return true;
                            }, AutoForm.ShowType.User).ShowDialog();
                            this.Grid.InvalidateCell(e.ColumnIndex, e.RowIndex);
                        }
                        break;
                    case Marks_enum.EdType:
                        G.EdType.QUERRY().SHOW.DO();
                        G.EdType.GetAutoForm((RowIndex) =>
                            {
                                if (RCache.Marks[e.RowIndex].OPTypeID > 0 && T.OPType.Rows.Get_UnShow<uint>(RCache.Marks[e.RowIndex].OPTypeID, C.OPType.EdTypeF) != G.EdType.Rows.GetID(RowIndex))
                                {
                                    G.Mark.QUERRY().SET.C<uint>(C.Mark.EdType, G.EdType.Rows.GetID(RowIndex)).C<uint>(C.Mark.OPType, 0).WHERE.ID(RCache.Marks[e.RowIndex].ID).DO();
                                }
                                else
                                { RCache.Marks[e.RowIndex].EdTypeID = G.EdType.Rows.GetID(RowIndex); }

                                return true;
                            }, AutoForm.ShowType.User).ShowDialog();
                        this.Grid.InvalidateRow(e.RowIndex);
                        break;
                }
            }

            private void Grid_KeyDown(object sender, KeyEventArgs e)
            {
                if (Grid.CurrentCell == null || (data.UType)data.User<uint>(C.User.UType) == data.UType.Employe || e.KeyData != Keys.Delete) return;

                switch ((Marks_enum)Grid.CurrentCell.ColumnIndex)
                {
                    case Marks_enum.Code:
                        RCache.Marks[Grid.CurrentCell.RowIndex].Code = 0;
                        Grid.InvalidateCell(Grid.CurrentCell);
                        break;
                    case Marks_enum.EdType:
                        RCache.Marks[Grid.CurrentCell.RowIndex].EdTypeID = 0;
                        Grid.InvalidateCell(Grid.CurrentCell);
                        break;
                    case Marks_enum.Exp:
                        RCache.Marks[Grid.CurrentCell.RowIndex].Exp = false;
                        Grid.InvalidateCell(Grid.CurrentCell);
                        break;
                    case Marks_enum.OPType:
                        RCache.Marks[Grid.CurrentCell.RowIndex].OPTypeID = 0;
                        Grid.InvalidateCell(Grid.CurrentCell);
                        break;
                    case Marks_enum.Round:
                        RCache.Marks[Grid.CurrentCell.RowIndex].Round = 0;
                        Grid.InvalidateCell(Grid.CurrentCell);
                        break;
                }
            }

            private void group_Resize(object sender, EventArgs e)
            {
                Grid.Size = new Size(MarkGroup.Size.Width - Grid.Location.X - data.Divide, MarkGroup.Size.Height - Grid.Location.Y - data.Divide);
            }

            private void Form_Resize(object sender, EventArgs e)
            {
                CountSummCount();

                double Width = (double)(form.Size.Width - 20) / SummCount;

                {
                    var HidedWidth = ((form.Size.Width - 20) * data.ForHidedP);
                    double minusWidth = 0;

                    for (int i = 0; i < Norms.Length; i++)
                    {
                        if (!Norms[i].Show)
                        {
                            Norms[i].Limit_group.Size = new Size((int)HidedWidth, form.Size.Height - Norms[i].Limit_group.Location.Y - data.MinusLocationY);
                            minusWidth += HidedWidth;
                        }
                    }

                    Width = (double)(form.Size.Width - 20 - minusWidth) / SummCount;
                }

                MarkGroup.Size = new Size((int)(Width * data.ForMark), form.Size.Height - MarkGroup.Location.Y - data.MinusLocationY);

                int SLocation = MarkGroup.Location.X + MarkGroup.Size.Width;

                for (int i = 0; i < Norms.Length; i++)
                {
                    if (Norms[i].Show)
                    {
                        Norms[i].Limit_group.Size = new Size((int)(Width * (Norms[i].Count * data.PerPodr + data.ForMethod)), form.Size.Height - Norms[i].Limit_group.Location.Y - data.MinusLocationY);
                    }

                    Norms[i].Limit_group.Location = new Point(SLocation, Norms[i].Limit_group.Location.Y);

                    SLocation = Norms[i].Limit_group.Location.X + Norms[i].Limit_group.Size.Width;
                }
            }

            public void Dispose()
            {
                if (this.Grid.IsCurrentCellInEditMode)
                { this.Grid.EndEdit(); }

                for (int i = 0; i < Norms.Length; i++)
                { this.Norms[i].Dispose(); }

                this.MarkGroup.Dispose();
                this.MarkAdd.Dispose();
                this.MarkDown.Dispose();
                this.MarkUp.Dispose();
                this.Grid.Dispose();
            }
        }

        MarkVObject_class Marks;

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Norm_button_Click(object sender, EventArgs e)
        {
            AutoForm.ShowType AST;

            G.Norm.QUERRY().SHOW.DO();

            AST = (data.User<uint>(C.User.UType) == (uint)data.UType.Admin ? AutoForm.ShowType.Admin : AutoForm.ShowType.User);

            G.Norm.GetAutoForm(this, data.User<uint>(C.User.UType) != (uint)data.UType.Employe, AST).ShowDialog();

            this.Marks.CreateGroups();
        }

        private void MarksCompanyEdit_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            Marks.Dispose();
        }

        private void Show_check_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}