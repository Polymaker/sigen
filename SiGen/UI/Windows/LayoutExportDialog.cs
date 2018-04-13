using SiGen.Export;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
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
    public partial class LayoutExportDialog : Form
    {
        private LayoutSvgExportOptions SvgExportOptions;
        private bool isLoading;

        public LayoutExportDialog()
        {
            InitializeComponent();
            SvgExportOptions = new LayoutSvgExportOptions();
        }

        public LayoutExportDialog(SILayout layout)
        {
            InitializeComponent();
            SvgExportOptions = new LayoutSvgExportOptions();
            layoutPreview.CurrentLayout = layout;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadOptions();
        }

        private void LoadOptions()
        {
            isLoading = true;

            chkExtendFretSlots.Checked = SvgExportOptions.ExtendFretSlots;
            chkExportFrets.Checked = SvgExportOptions.ExportFrets;
            chkExportStrings.Checked = SvgExportOptions.ExportStrings;
            chkExportStringCenters.Checked = SvgExportOptions.ExportStringCenters;
            UpdatePreview();

            if (SvgExportOptions.ExtendFretSlots)
            {
                if (SvgExportOptions.FretSlotsExtensionAmount > Measure.Zero)
                    rbExtendOutward.Checked = true;
                else
                    rbExtendInward.Checked = true;
                
                mtbFretExtendAmount.Value = Measure.Abs(SvgExportOptions.FretSlotsExtensionAmount);
            }
            else
            {
                rbExtendInward.Checked = true;
                mtbFretExtendAmount.Value = Measure.Mm(2);
            }

            chkFretThickness.Checked = !SvgExportOptions.FretLineThickness.IsEmpty;
            if (!SvgExportOptions.FretLineThickness.IsEmpty)
            {
                mtbFretThickness.Value = SvgExportOptions.FretLineThickness;
            }
            else
            {
                mtbFretThickness.Value = Measure.Mm(0.5);
            }

            pbxFretColor.BackColor = SvgExportOptions.FretColor;

            isLoading = false;
        }

        private void UpdatePreview()
        {
            layoutPreview.DisplayConfig.ShowStrings = SvgExportOptions.ExportStrings;
            layoutPreview.DisplayConfig.ShowFrets = SvgExportOptions.ExportFrets;
            layoutPreview.DisplayConfig.ShowMidlines = SvgExportOptions.ExportStringCenters;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(layoutPreview.CurrentLayout.LayoutName))
                    sfd.FileName = layoutPreview.CurrentLayout.LayoutName + ".svg";
                else
                    sfd.FileName = "layout.svg";

                sfd.Filter = "Scalable Vector Graphics File (*.svg)|*.svg";
                sfd.DefaultExt = ".svg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SvgLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, SvgExportOptions);
                }
            }
        }

        private void chkExtendFretSlots_CheckedChanged(object sender, EventArgs e)
        {
            flpExtendDirection.Enabled = chkExtendFretSlots.Checked;
            mtbFretExtendAmount.Enabled = chkExtendFretSlots.Checked;

            if (!isLoading)
            {
                if (chkExtendFretSlots.Checked)
                    mtbFretExtendAmount_ValueChanged(mtbFretExtendAmount, EventArgs.Empty);
                else
                    SvgExportOptions.FretSlotsExtensionAmount = Measure.Empty;
            }
        }

        private void mtbFretExtendAmount_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                if (!mtbFretExtendAmount.Value.IsEmpty && mtbFretExtendAmount.Value != Measure.Zero)
                {
                    var amount = mtbFretExtendAmount.Value;
                    if (rbExtendInward.Checked)
                        amount = amount * -1;
                    SvgExportOptions.FretSlotsExtensionAmount = amount;
                }
            }
        }

        private void chkFretThickness_CheckedChanged(object sender, EventArgs e)
        {
            mtbFretThickness.Enabled = chkFretThickness.Checked;

            if (!isLoading)
            {
                if (chkExtendFretSlots.Checked)
                    mtbFretThickness_ValueChanged(mtbFretThickness, EventArgs.Empty);
                else
                    SvgExportOptions.FretLineThickness = Measure.Empty;
            }
        }

        private void mtbFretThickness_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                if (!mtbFretThickness.Value.IsEmpty && mtbFretThickness.Value != Measure.Zero)
                    SvgExportOptions.FretLineThickness = mtbFretThickness.Value;
            }
        }

        private void chkExportFrets_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                SvgExportOptions.ExportFrets = chkExportFrets.Checked;
            UpdatePreview();
        }

        private void chkExportStrings_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                SvgExportOptions.ExportStrings = chkExportStrings.Checked;
            UpdatePreview(); 
        }

        private void chkExportStringCenters_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                SvgExportOptions.ExportStringCenters = chkExportStringCenters.Checked;
            UpdatePreview();
        }

        private void btnPickFretColor_Click(object sender, EventArgs e)
        {
            using(var dlg = new ColorDialog())
            {
                dlg.Color = SvgExportOptions.FretColor;
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    SvgExportOptions.FretColor = dlg.Color;
                    pbxFretColor.BackColor = dlg.Color;
                }
            }
        }
    }
}
