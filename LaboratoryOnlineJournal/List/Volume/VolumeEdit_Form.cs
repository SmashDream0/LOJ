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
    public partial class VolumeEdit_Form : Form
    {
        public VolumeEdit_Form()
        {
            InitializeComponent();
            
            Volumes_panel.Location = new Point(data.Divide, data.Divide);

            int LocationY = data.Divide;

            for (int i = 0; i < RCache.Volumes.Count; i++)
            { Volumes.Add(new Volume_class(Volumes_panel, RCache.Volumes.GetVGroupID(i), ref LocationY)); }

            var SPoint = T.SPoint.CreateSubTable();

            if ((data.UType)data.User<uint>(C.User.UType) == data.UType.MainEmploye)
            {
                SPoint.QUERRY()
                       .SHOW
                           .WHERE
                               .ARC(C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                               .AND.AC(C.SPoint.YMDS).Less.BV(Employe_Form.SPoints.EndDay)
                               .AND.OB()
                                   .AC(C.SPoint.YMDE).More.BV<int>(Employe_Form.SPoints.StartDay)
                                   .OR.AC(C.SPoint.YMDE).EQUI.BV<int>(0)
                               .CB()
                           .DO();
            }
            else
            {
                SPoint.QUERRY()
                      .SHOW
                          .WHERE
                              .ARC(C.SPoint.Object, C.Object.OLocationFrom, C.OLocation.Volumed).EQUI.BV<bool>(true)
                              .AND.AC(C.SPoint.YMDS).Less.BV(Employe_Form.SPoints.EndDay)
                              .AND.C(C.SPoint.Podr, data.User<uint>(C.User.Podr))
                              .AND.OB()
                                  .AC(C.SPoint.YMDE).More.BV<int>(Employe_Form.SPoints.StartDay)
                                  .OR.AC(C.SPoint.YMDE).EQUI.BV<int>(0)
                              .CB()
                          .DO();
            }

            for (int i = 0; i < SPoint.Rows.Count; i++)
            {
                var VGIndex = RCache.Volumes.GetIndex(SPoint.Rows.Get_UnShow<uint>(i, C.SPoint.Object, C.Object.OLocationFrom));
                if (VGIndex > -1)
                {
                    Volumes[VGIndex].SPointID = SPoint.Rows.GetID(i);
                    Volumes[VGIndex].UpdateName();
                }
            }

            VolumeEdit_Form_Resize(null, null);
        }

        List<Volume_class> Volumes = new List<Volume_class>();

        private void Close_button_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void VolumeEdit_Form_Resize(object sender, EventArgs e)
        {
            Volumes_panel.Size = new Size(this.Size.Width - Volumes_panel.Location.X - data.Divide * 2 - 14, this.Size.Height - Volumes_panel.Location.Y - data.Divide * 3 - Close_button.Size.Height - 45);            
        }

        private void VolumeEdit_Form_FormClosed(object sender, FormClosedEventArgs e)
        {
            for (int i = 0; i < Volumes.Count; i++)
            { Volumes[i].AddVolume(); }
        }
    }
}
