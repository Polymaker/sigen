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
        private LineExportConfig _LineConfig;
        private string _ConfigName;
        private FlagList FlagManager;

        [Browsable(false)]
        public LineExportConfig LineConfig
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
                lineExportEdit1.LineConfig = LineConfig;
            }
        }
        
        private void EnableExportChecbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
                LineConfig.Enabled = EnableExportChecbox.Checked;
        }
    }

}
