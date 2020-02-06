using SiGen.Configuration;
using SiGen.Export;
using SiGen.UI.Controls.Preferences;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Windows
{
    public partial class OptionsDialog : Form
    {

        private LayoutExportConfig ExportConfig { get; set; }

        private ViewerDisplayConfig DisplayConfig { get; set; }

        public OptionsDialog()
        {
            InitializeComponent();
            Icon = Properties.Resources.SiGenIcon;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            var currentSettings = AppConfig.Current.Clone();
            ExportConfig = currentSettings.ExportConfig;
            DisplayConfig = currentSettings.DisplayConfig;

            LoadDisplayConfigs();
            LoadExportConfigs();
        }

        private void LoadDisplayConfigs()
        {
            checkBox1.Checked = DisplayConfig.Frets.Visible;
            colorSelectButton1.Value = DisplayConfig.Frets.Color;
        }

        private void LoadExportConfigs()
        {
            FretsExportCfg.LineConfig = ExportConfig.Frets;
            FingerboardExportCfg.LineConfig = ExportConfig.FingerboardEdges;
            MarginsExportCfg.LineConfig = ExportConfig.FingerboardMargins;
            CenterLineExportCfg.LineConfig = ExportConfig.CenterLine;
            StringsExportCfg.LineConfig = ExportConfig.Strings;
            StringCentersExportCfg.LineConfig = ExportConfig.Midlines;
            GuideExportCfg.LineConfig = ExportConfig.GuideLines;
        }


        private void SaveExportConfigButton_Click(object sender, EventArgs e)
        {
            AppConfig.Current.ExportConfig = ExportConfig;
            AppConfigManager.Save();
            DialogResult = DialogResult.OK;
        }

        private void Display_CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void Export_CancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
