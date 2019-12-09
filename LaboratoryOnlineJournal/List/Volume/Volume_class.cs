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
    class Volume_class:IDisposable
    {
        public Volume_class(Control Parent, uint VGroupID, ref int LocationY)
        {
            Name_label = new Label();
            Name_label.Text = "";
            Name_label.Parent = Parent;
            Name_label.Size = new System.Drawing.Size(T.OLocation.GetColumn(C.OLocation.Name).Length + 4 + T.SPoint.GetColumn(C.SPoint.Name).Length, data.ButtonsHeight);
            Name_label.TextAlign = ContentAlignment.MiddleLeft;
            Name_label.BorderStyle = BorderStyle.FixedSingle;

            Volume_Box = new TextBox();
            Volume_Box.Text = RCache.Volumes.GetVolume(RCache.Volumes.GetIndex(VGroupID)).ToString();
            Volume_Box.Parent = Parent;
            Volume_Box.Size = new System.Drawing.Size(75, data.ButtonsHeight);
            Volume_Box.TextChanged += Volume_Box_TextChanged;
            Volume_Box.Enabled = false;

            this.VGroupID = VGroupID;

            Location = new Point(Location.X, LocationY);
            LocationY += data.Divide + this.Size.Height;

            this.UpdateName();
        }

        Label Name_label;
        TextBox Volume_Box;
        bool CanDo = true;

        public uint VGroupID { get; internal set; }
        public uint SPointID;

        public double Volume
        {
            get
            {
                if (Volume_Box.TextLength == 0)
                { return 0; }
                else
                { return Convert.ToDouble(Volume_Box.Text); }
            }
            set { Volume_Box.Text = value.ToString(); }
        }
        public string Name
        {
            get { return Name_label.Text; }
        }

        public Point Location
        {
            set
            {
                Name_label.Location = new Point(value.X + data.Divide * 2, value.Y);
                Volume_Box.Location = new Point(Name_label.Location.X + Name_label.Size.Width + data.Divide, Name_label.Location.Y);
            }

            get { return new Point(Name_label.Location.X - data.Divide * 2, Name_label.Location.Y); }
        }
        public Size Size
        {
            get { return new Size(Name_label.Size.Width + data.Divide * 3 + Volume_Box.Size.Width, Name_label.Size.Height); }
            set
            {
                Name_label.Size = new Size(value.Width - Volume_Box.Size.Width - data.Divide * 2, data.ButtonsHeight);
                Volume_Box.Size = new Size(value.Width - Name_label.Size.Width - data.Divide * 3, data.ButtonsHeight);
            }
        }

        public void Dispose()
        {
            Name_label.Dispose();
            Volume_Box.Dispose();
        }

        void Name_Box_TextChanged(object sender, EventArgs e)
        {
        }
        public void AddVolume()
        {
            var Volume = Convert.ToDouble(Volume_Box.Text);
            if (Volume != RCache.Volumes.GetVolume(RCache.Volumes.GetIndex(VGroupID)))
            { RCache.Volumes.SetVolume(RCache.Volumes.GetIndex(VGroupID), Volume); }
        }

        void Volume_Box_TextChanged(object sender, EventArgs e)
        {
            if (CanDo)
            {
                CanDo = false;
                DataBase.NoABC_Double_Dinamic(sender as TextBox);
                CanDo = true;
            }
        }
        public void UpdateName()
        {
            if (SPointID > 0)
            {
                Name_label.Text = RCache.Volumes.GetName(RCache.Volumes.GetIndex(VGroupID)) + " - " + T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Podr, C.Podr.ShrName) + " - " + T.SPoint.Rows.Get<string>(SPointID, C.SPoint.Name);
                Volume_Box.Enabled = Employe_Form.SPoints.YM == Employe_Form.WorkYM && ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye && Volume > 0 || (data.UType)data.User<uint>(C.User.UType) != data.UType.MainEmploye);
            }
            else
            {
                Name_label.Text = RCache.Volumes.GetName(RCache.Volumes.GetIndex(VGroupID));
                Volume_Box.Enabled = false;
            }
        }

        public override string ToString()
        {
            return Name_label.Text + " = " + Volume_Box.Text;
        }
    }
}