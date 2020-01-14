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
using SiGen.Configuration;

namespace SiGen.UI.Controls.PreferencesPanels
{
    public partial class LayoutExportOptionsEditor : UserControl
    {
        private LayoutExportOptions CurrentConfig;
        private bool ConfigChanged;
        private FlagList FlagManager;
        private List<LayoutExportOptions> ExportConfigs;


        public LayoutExportOptionsEditor()
        {
            InitializeComponent();
            FlagManager = new FlagList();
            CurrentConfig = new LayoutExportOptions();
            SelectConfigCombo.DisplayMember = "ConfigurationName";
            SelectConfigCombo.ValueMember = "this";
            ExportConfigs = new List<LayoutExportOptions>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FillExistingConfigs();
            LoadConfiguration(ExportConfigs.FirstOrDefault());
        }

        private void FillExistingConfigs()
        {
            SelectConfigCombo.DataSource = null;

            ExportConfigs.Clear();
            ExportConfigs.AddRange(AppConfigManager.Current.ExportConfigs);

            if (!ExportConfigs.Any())
            {
                var defaultCfg = LayoutExportOptions.CreateDefault();
                defaultCfg.ConfigurationName = "*" + defaultCfg.ConfigurationName;
                ExportConfigs.Add(defaultCfg);
            }
            SelectConfigCombo.DataSource = ExportConfigs;
        }

        private void LoadConfiguration(LayoutExportOptions exportOptions)
        {
            if (CurrentConfig != null)
            {
                CurrentConfig.DettachPropertyChangedEvent();
                CurrentConfig.PropertyChanged -= CurrentConfig_PropertyChanged;
            }

            CurrentConfig = exportOptions;

            if (CurrentConfig != null)
            {
                using (FlagManager.UseFlag("LoadConfig"))
                {
                    FretsConfig.LineConfig = CurrentConfig.Frets;
                    StringsConfig.LineConfig = CurrentConfig.Strings;
                    FingerboardEdgesConfig.LineConfig = CurrentConfig.FingerboardEdges;
                    FingerboardMarginsConfig.LineConfig = CurrentConfig.FingerboardMargins;
                }

                CurrentConfig.AttachPropertyChangedEvent();
                CurrentConfig.PropertyChanged += CurrentConfig_PropertyChanged;
                ConfigChanged = false;
            }
            
        }

        private void CurrentConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ConfigChanged = true;
        }

        private void SelectConfigCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SelectConfigCombo.SelectedItem != null)
            {
                LoadConfiguration(SelectConfigCombo.SelectedItem as LayoutExportOptions);
            }
        }
    }
}
