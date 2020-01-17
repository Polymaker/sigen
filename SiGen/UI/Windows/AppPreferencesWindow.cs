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
    public partial class AppPreferencesWindow : Form
    {

        private LayoutExportConfig ExportConfig { get; set; }

        private LayoutViewerDisplayConfig DisplayConfig { get; set; }

        public AppPreferencesWindow()
        {
            InitializeComponent();
            //CreateExportConfigControls();
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
            checkBox1.Checked = DisplayConfig.ShowFrets;
            colorSelectButton1.Value = DisplayConfig.FretLineColor;
        }

        private void LoadExportConfigs()
        {
            FretsExportCfg.LineConfig = ExportConfig.Frets;
            FingerboardExportCfg.LineConfig = ExportConfig.FingerboardEdges;
            MarginsExportCfg.LineConfig = ExportConfig.FingerboardMargins;
            CenterLineExportCfg.LineConfig = ExportConfig.CenterLine;
            StringsExportCfg.LineConfig = ExportConfig.Strings;
            StringCentersExportCfg.LineConfig = ExportConfig.StringCenters;
            GuideExportCfg.LineConfig = ExportConfig.GuideLines;
        }

        private void SaveExportConfigButton_Click(object sender, EventArgs e)
        {
            AppConfig.Current.ExportConfig = ExportConfig;
            AppConfigManager.Save();
            DialogResult = DialogResult.OK;
        }



        //private void CreateExportConfigControls()
        //{
        //    var titleFont = new Font(Font.FontFamily, Font.Size * 1.5f);

        //    void AddTitle(string caption)
        //    {
        //        if (flowLayoutPanel1.Controls.Count > 0)
        //        {
        //            var lastCtrl = flowLayoutPanel1.Controls[flowLayoutPanel1.Controls.Count - 1];
        //            flowLayoutPanel1.SetFlowBreak(lastCtrl, true);
        //        }
        //        var titleLabel = new Label()
        //        {
        //            Text = caption,
        //            Font = titleFont
        //        };
        //        flowLayoutPanel1.Controls.Add(titleLabel);
        //        var dummyLabel = new Label()
        //        {
        //            Size = new Size(0,0),
        //            Padding = new Padding(0)
        //        };
        //        flowLayoutPanel1.Controls.Add(dummyLabel);
        //        flowLayoutPanel1.SetFlowBreak(dummyLabel, true);
        //    }
        //    void AddLineEdit(LayoutLineExportConfig lineConfig, string title)
        //    {
        //        var lineEdit = new LineExportConfigEdit();
        //        lineEdit.ConfigName = title;
        //        //lineEdit.LineConfig = lineConfig;
        //        flowLayoutPanel1.Controls.Add(lineEdit);
        //    }

        //    AddTitle("Frets");
        //    AddLineEdit(null, "Export frets");

        //    AddTitle("Strings");
        //    AddLineEdit(null, "Export strings");

        //    AddTitle("Fingerboard");
        //    AddLineEdit(null, "Fingerboard edges");
        //    AddLineEdit(null, "Fingerboard margins");
        //}
    }
}
