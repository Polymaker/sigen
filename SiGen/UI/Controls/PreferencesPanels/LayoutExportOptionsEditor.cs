using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Controls.PreferencesPanels
{
    public partial class LayoutExportOptionsEditor : UserControl
    {
        public LayoutExportOptionsEditor()
        {
            InitializeComponent();
        }

        private void ChkExportFingerboardEdges_CheckedChanged(object sender, EventArgs e)
        {
            colorBtnFingerboardEdges.Enabled = chkExportFingerboardEdges.Checked;
            lineOptFingerboardEdges.Enabled = chkExportFingerboardEdges.Checked;
        }
    }
}
