using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Configuration;

namespace SiGen.UI.Controls.PreferencesPanels
{
    public partial class MonitorPreferences : PreferencesPanel
    {

        private ScreenConfiguration _CurrentScreen;

        public MonitorPreferences()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        private void pbxPreviewDPI_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(pbxPreviewDPI.BackColor);
            if(_CurrentScreen != null && _CurrentScreen.DPI != 0)
            {
                var g = e.Graphics;
                float sizeCM = _CurrentScreen.DPI / 2.54f;
                float sizeIN = _CurrentScreen.DPI;
                var center = new PointF(pbxPreviewDPI.Width / 2f, pbxPreviewDPI.Height / 2f);

                g.DrawRectangle(Pens.Blue, center.X - sizeCM / 2f, center.Y - sizeCM / 2f, sizeCM, sizeCM);
                g.DrawRectangle(Pens.Blue, center.X - sizeIN / 2f, center.Y - sizeIN / 2f, sizeIN, sizeIN);
            }
        }

        private void AdjustPreviewsize()
        {
            if (_CurrentScreen != null && _CurrentScreen.DPI != 0)
            {
                gbxPreviewDPI.Size = new Size(_CurrentScreen.DPI + gbxPreviewDPI.Padding.Horizontal + 10,
                    _CurrentScreen.DPI + gbxPreviewDPI.Padding.Vertical + pbxPreviewDPI.Top + 10);
            }
        }
    }
}
