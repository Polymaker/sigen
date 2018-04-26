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
    [ToolboxItem(false)]
    public partial class PreferencesPanel : UserControl
    {
        public PreferencesPanel()
        {
            InitializeComponent();
        }

        public virtual void Save()
        {

        }

        public virtual void ResetDefault()
        {

        }
    }
}
