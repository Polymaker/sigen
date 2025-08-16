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

            Task.Factory.StartNew(() =>
            {
                var currentSettings = AppConfig.Current.Clone();
                ExportConfig = currentSettings.ExportConfig;
                DisplayConfig = currentSettings.DisplayConfig;
                BeginInvoke(new MethodInvoker(LoadConfigValues));
            });
        }

        private void LoadConfigValues()
        {
            LoadDisplayConfigs();
            LoadExportConfigs();
        }

        private void LoadDisplayConfigs()
        {
            ChkRealisticStrings.Checked = DisplayConfig.Strings.RenderMode == Configuration.Display.LineRenderMode.RealisticLook;
            StringsColorSelect.Value = DisplayConfig.Strings.Color;
            StringsColorSelect.Enabled = !ChkRealisticStrings.Checked;

            ChkRealisticFrets.Checked = DisplayConfig.Frets.RenderMode != Configuration.Display.LineRenderMode.PlainLine;
            FretsColorSelect.Value = DisplayConfig.Frets.Color;
            FretsColorSelect.Enabled = !ChkRealisticFrets.Checked;

            FretWidthBox.Enabled = ChkRealisticFrets.Checked;
            FretWidthBox.Value = DisplayConfig.Frets.RenderWidth;

            ChkRealFretPos.Checked = DisplayConfig.Frets.DisplayAccuratePositions;
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


        private void Export_SaveButton_Click(object sender, EventArgs e)
        {
            AppConfig.Current.ExportConfig = ExportConfig;
            AppConfigManager.Save();
            DialogResult = DialogResult.OK;
            
        }

        private void Display_SaveButton_Click(object sender, EventArgs e)
        {
            DisplayConfig.Strings.RenderMode = ChkRealisticStrings.Checked ? 
                Configuration.Display.LineRenderMode.RealisticLook : 
                Configuration.Display.LineRenderMode.PlainLine;

            DisplayConfig.Strings.Color = StringsColorSelect.Value;

            DisplayConfig.Frets.RenderMode = ChkRealisticFrets.Checked ?
                Configuration.Display.LineRenderMode.RealisticLook :
                Configuration.Display.LineRenderMode.PlainLine;

            DisplayConfig.Frets.Color = FretsColorSelect.Value;
            DisplayConfig.Frets.RenderWidth = FretWidthBox.Value;
            DisplayConfig.Frets.DisplayAccuratePositions = ChkRealFretPos.Checked;
            AppConfig.Current.DisplayConfig = DisplayConfig;
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

        private void ChkRealisticFrets_CheckedChanged(object sender, EventArgs e)
        {
            FretWidthBox.Enabled = ChkRealisticFrets.Checked;
            FretsColorSelect.Enabled = !ChkRealisticFrets.Checked;
        }

        private void ResetDisplayButton_Click(object sender, EventArgs e)
        {
            DisplayConfig = ViewerDisplayConfig.CreateDefault();
            LoadDisplayConfigs();
        }

        private void ChkRealisticStrings_CheckedChanged(object sender, EventArgs e)
        {
            StringsColorSelect.Enabled = !ChkRealisticStrings.Checked;
        }

        private void ChkRealFretPos_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
