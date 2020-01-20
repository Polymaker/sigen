using Newtonsoft.Json;
using SiGen.Common;
using SiGen.Configuration;
using SiGen.Configuration.Display;
using SiGen.Export;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Windows
{
    public partial class ExportLayoutDialog : Form
    {
        private LayoutExportConfig ExportOptions;
        private bool isLoading;

        public ExportLayoutDialog()
        {
            InitializeComponent();
            ExportOptions = LayoutExportConfig.CreateDefault();
        }

        public ExportLayoutDialog(SILayout layout)
        {
            InitializeComponent();
            ExportOptions = LayoutExportConfig.CreateDefault();
            layoutPreview.CurrentLayout = layout;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            ExportOptions = AppConfig.Current.ExportConfig;
            LoadOptions();
        }

        private void LoadOptions()
        {
            isLoading = true;

            chkExtendFretSlots.Checked = ExportOptions.ExtendFretSlots;
            chkExportFrets.Checked = ExportOptions.ExportFrets;
            chkExportStrings.Checked = ExportOptions.ExportStrings;
            chkExportStringCenters.Checked = ExportOptions.ExportMidlines;
            chkExportCenterLine.Checked = ExportOptions.ExportCenterLine;
            chkExportFingerboard.Checked = ExportOptions.ExportFingerboardEdges;
            UpdatePreview();

            if (ExportOptions.ExtendFretSlots)
            {
                if (ExportOptions.Frets.ExtensionAmount > Measure.Zero)
                    rbExtendOutward.Checked = true;
                else
                    rbExtendInward.Checked = true;
                
                mtbFretExtendAmount.Value = Measure.Abs(ExportOptions.Frets.ExtensionAmount);
            }
            else
            {
                rbExtendInward.Checked = true;
                mtbFretExtendAmount.Value = Measure.Mm(2);
            }

            chkFretThickness.Checked = (ExportOptions.Frets.LineUnit >= LineUnit.Millimeters);
            if (chkFretThickness.Checked)
            {
                if (ExportOptions.Frets.LineUnit == LineUnit.Millimeters)
                    mtbFretThickness.Value = new Measure(ExportOptions.Frets.LineThickness, UnitOfMeasure.Mm);
                else
                    mtbFretThickness.Value = new Measure(ExportOptions.Frets.LineThickness, UnitOfMeasure.In);
            }
            else
            {
                mtbFretThickness.Value = Measure.Mm(0.5);
            }

            pbxFretColor.BackColor = ExportOptions.Frets.Color;

            isLoading = false;
        }

        private void UpdatePreview()
        {
            layoutPreview.DisplayConfig.ShowMidlines = ExportOptions.ExportMidlines;
            layoutPreview.DisplayConfig.ShowMargins = ExportOptions.ExportFingerboardMargins;
            layoutPreview.DisplayConfig.ShowCenterLine = ExportOptions.ExportCenterLine;

            layoutPreview.DisplayConfig.Fingerboard.Color = ExportOptions.FingerboardEdges.Color;
            layoutPreview.DisplayConfig.Fingerboard.Visible = ExportOptions.ExportFingerboardEdges;

            layoutPreview.DisplayConfig.Frets.Visible = ExportOptions.Frets.Enabled;
            layoutPreview.DisplayConfig.Frets.Color = ExportOptions.Frets.Color;

            layoutPreview.DisplayConfig.Strings.Visible = ExportOptions.Strings.Enabled;
            layoutPreview.DisplayConfig.Strings.Color = ExportOptions.Strings.Color;
            layoutPreview.DisplayConfig.Strings.RenderMode = 
                ExportOptions.Strings.UseStringGauge ? 
                LineRenderMode.RealWidth : 
                LineRenderMode.PlainLine;

            layoutPreview.DisplayConfig.FretExtensionAmount = ExportOptions.Frets.ExtensionAmount;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            //string json = JsonConvert.SerializeObject(ExportOptions, Formatting.Indented);
            //if (rbSvgExport.Checked)
            //    ExportSvgLayout();
            //else
            //    ExportDxfLayout();

            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(layoutPreview.CurrentLayout.LayoutName))
                    sfd.FileName = layoutPreview.CurrentLayout.LayoutName;
                else
                    sfd.FileName = LayoutDocument.GenerateLayoutName(layoutPreview.CurrentLayout);


                sfd.Filter = "Scalable Vector Graphics File (*.svg)|*.svg|Drawing Interchange Format File (*.dxf)|*.dxf|All files|*.*";
                if (rbDxfExport.Checked)
                    sfd.FilterIndex = 2;
                sfd.AddExtension = true;

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (sfd.FilterIndex == 1)
                        SvgLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, ExportOptions);
                    else if (sfd.FilterIndex == 2)
                        DxfLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, ExportOptions);
                    else
                    {
                        var fileExt = Path.GetExtension(sfd.FileName);
                        switch (fileExt.ToLower())
                        {
                            case ".svg":
                                SvgLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, ExportOptions);
                                break;
                            case ".dxf":
                                DxfLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, ExportOptions);
                                break;
                            default:
                                MessageBox.Show("todo");
                                break;
                        }
                    }
                }
            }
        }

        private void ExportSvgLayout()
        {
            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(layoutPreview.CurrentLayout.LayoutName))
                    sfd.FileName = layoutPreview.CurrentLayout.LayoutName + ".svg";
                else
                    sfd.FileName = "layout.svg";


                sfd.Filter = "Scalable Vector Graphics File|*.svg";
                sfd.DefaultExt = ".svg";

                if (sfd.ShowDialog() == DialogResult.OK)
                    SvgLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, ExportOptions);
            }
        }

        private void ExportDxfLayout()
        {
            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(layoutPreview.CurrentLayout.LayoutName))
                    sfd.FileName = layoutPreview.CurrentLayout.LayoutName + ".dxf";
                else
                    sfd.FileName = "layout.dxf";


                sfd.Filter = "Drawing Interchange Format File (*.dxf)|*.dxf";
                sfd.DefaultExt = ".dxf";

                if (sfd.ShowDialog() == DialogResult.OK)
                    DxfLayoutExporter.ExportLayout(sfd.FileName, layoutPreview.CurrentLayout, ExportOptions);
            }
        }

        private void rbExtendInward_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                mtbFretExtendAmount_ValueChanged(mtbFretExtendAmount, EventArgs.Empty);
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
                    ExportOptions.Frets.ExtensionAmount = Measure.Empty;
                UpdatePreview();
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
                    ExportOptions.Frets.ExtensionAmount = amount;
                    UpdatePreview();
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
                {
                    ExportOptions.Frets.LineThickness = 1d;
                    ExportOptions.Frets.LineUnit = LineUnit.Points;
                }
            }
        }

        private void mtbFretThickness_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                if (!mtbFretThickness.Value.IsEmpty && mtbFretThickness.Value != Measure.Zero)
                {
                    if (mtbFretThickness.Value.Unit.IsMetric)
                    {
                        ExportOptions.Frets.LineThickness = (double)mtbFretThickness.Value[UnitOfMeasure.Mm];
                        ExportOptions.Frets.LineUnit = LineUnit.Millimeters;
                    }
                    else
                    {
                        ExportOptions.Frets.LineThickness = (double)mtbFretThickness.Value[UnitOfMeasure.In];
                        ExportOptions.Frets.LineUnit = LineUnit.Inches;
                    }
                }
            }
        }

        private void chkExportFrets_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                ExportOptions.ExportFrets = chkExportFrets.Checked;
            UpdatePreview();
        }

        private void chkExportStrings_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                ExportOptions.ExportStrings = chkExportStrings.Checked;
            UpdatePreview(); 
        }

        private void chkExportStringCenters_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                ExportOptions.ExportMidlines = chkExportStringCenters.Checked;
            UpdatePreview();
        }

        private void chkExportCenterLine_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                ExportOptions.ExportCenterLine = chkExportCenterLine.Checked;
            UpdatePreview();
        }

        private void chkExportMargins_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                ExportOptions.ExportFingerboardMargins = chkExportMargins.Checked;
            UpdatePreview();
        }

        private void chkExportFingerboard_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
                ExportOptions.ExportFingerboardEdges = chkExportFingerboard.Checked;
            UpdatePreview();
        }

        private void btnPickFretColor_Click(object sender, EventArgs e)
        {
            using(var dlg = new ColorDialog())
            {
                dlg.Color = ExportOptions.Frets.Color;
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    ExportOptions.Frets.Color = dlg.Color;
                    pbxFretColor.BackColor = dlg.Color;
                    UpdatePreview();
                }
            }
        }

        
    }
}
