using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Export;
using SiGen.Utilities;

namespace SiGen.UI.Controls.PreferencesPanels
{
    public partial class LayoutExportOptionsEditor : UserControl
    {
        private LayoutExportOptions CurrentConfig;
        private FlagList FlagManager;

        public LayoutExportOptionsEditor()
        {
            InitializeComponent();
            FlagManager = new FlagList();
            CurrentConfig = new LayoutExportOptions();
        }
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadConfiguration(LayoutExportOptions.CreateDefault());
        }

        private void LoadConfiguration(LayoutExportOptions exportOptions)
        {
            CurrentConfig = exportOptions;

            using (FlagManager.UseFlag("LoadConfig"))
            {
                FretsConfig.LineConfig = CurrentConfig.Frets;
                StringsConfig.LineConfig = CurrentConfig.Strings;
                FingerboardEdgesConfig.LineConfig = CurrentConfig.FingerboardEdges;
                FingerboardMarginsConfig.LineConfig = CurrentConfig.FingerboardMargins;
            }
        }
    }
}
