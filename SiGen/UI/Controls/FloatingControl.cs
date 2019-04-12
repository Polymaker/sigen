using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Controls
{
    public class FloatingControl : ToolStripDropDown
    {
        private ToolStripControlHost ControlHost;
        public Control Control { get; }

        public FloatingControl(Control control)
        {
            Control = control;
            ControlHost = new ToolStripControlHost(control);
            Items.Add(ControlHost);
        }
    }
}
