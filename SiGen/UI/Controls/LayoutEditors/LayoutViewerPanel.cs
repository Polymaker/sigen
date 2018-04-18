using SiGen.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class LayoutViewerPanel : DockContent
    {
        private double ScreenDPI;
        private LayoutFile _CurrentFile;

        public LayoutViewer Viewer { get { return layoutViewer1; } }

        public LayoutFile CurrentFile
        {
            get { return _CurrentFile; }
            set
            {
                if(value != _CurrentFile)
                {
                    _CurrentFile = value;
                    Viewer.CurrentLayout = value != null ? value.Layout : null;
                }
            }
        }

        public LayoutViewerPanel()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ScreenDPI = 109;
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
            Viewer.ResetCamera();
        }

        private void layoutViewer1_ZoomChanged(object sender, EventArgs e)
        {
            var currentZoom = Viewer.Zoom;
            var dpi = ScreenDPI == 0 ? 96 : ScreenDPI;
            
            tsLblZoom.Text = string.Format("Zoom: {0:0.##}%", (currentZoom / (dpi / 2.54)) * 100);
        }
    }
}
