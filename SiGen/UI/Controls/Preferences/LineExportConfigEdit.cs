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

namespace SiGen.UI.Controls.Preferences
{
    public partial class LineExportConfigEdit : UserControl
    {
        private LayoutLineExportConfig _LineConfig;
        private string _ConfigName;
        private FlagList FlagManager;

        [Browsable(false)]
        public LayoutLineExportConfig LineConfig
        {
            get => _LineConfig;
            set
            {
                if (_LineConfig != value)
                {
                    _LineConfig = value;
                    UpdateControls();
                }
            }
        }

        [Localizable(true)]
        public string ConfigName
        {
            get => _ConfigName;
            set
            {
                if (_ConfigName != value)
                {
                    _ConfigName = value;
                    EnableExportChecbox.Text = _ConfigName;
                }
            }
        }

        public LineExportConfigEdit()
        {
            InitializeComponent();
            FlagManager = new FlagList();
            EnableExportChecbox.Text = _ConfigName ?? "[Config Name]";
            //UseStringGaugeCheckbox.Visible = false;

            SetControlsFont();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            SetControlsFont();
        }

        private void SetControlsFont()
        {
            EnableExportChecbox.Font = new Font(Font.FontFamily, Font.Size * 1.2f);
        }

        private void UpdateControls()
        {
            
            using (FlagManager.UseFlag("LoadConfig"))
            {
                EnableExportChecbox.Checked = LineConfig?.Enabled ?? false;

                UseStringGaugeCheckbox.Visible = (LineConfig is StringsExportConfig);
                UseStringGaugeCheckbox.Checked = (LineConfig is StringsExportConfig sc && sc.UseStringGauge);
                
                DashedCheckbox.Checked = LineConfig?.IsDashed ?? false;

                LineColorSelector.Value = LineConfig?.Color ?? Color.Black;
                LineThicknessEditor.SelectedUnit = LineConfig?.LineUnit ?? LineUnit.Points;
                LineThicknessEditor.SelectedThickness = LineConfig?.LineThickness ?? 1d;
            }
        }

        private void EnableExportChecbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
                LineConfig.Enabled = EnableExportChecbox.Checked;
        }

        private void LineColorSelector_ValueChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
                LineConfig.Color = LineColorSelector.Value;
        }

        private void LineThicknessEditor_ConfigurationChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
            {
                LineConfig.LineThickness = LineThicknessEditor.SelectedThickness;
                LineConfig.LineUnit = LineThicknessEditor.SelectedUnit;
            }
        }

        private void DashedCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
                LineConfig.IsDashed = DashedCheckbox.Checked;
        }
    }

}
