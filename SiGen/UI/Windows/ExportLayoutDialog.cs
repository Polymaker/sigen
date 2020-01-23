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

        public LayoutDocument LayoutToExport { get; set; }

        private bool HasInitialized;
        private bool isLoading;

        public ExportLayoutDialog()
        {
            InitializeComponent();
            //ExportOptions = LayoutExportConfig.CreateDefault();
        }

        public ExportLayoutDialog(LayoutDocument layoutDocument)
        {
            InitializeComponent();
            //ExportOptions = LayoutExportConfig.CreateDefault();
            LayoutToExport = layoutDocument;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            ExportOptions = AppConfig.Current.ExportConfig;
            HasInitialized = true;

            LoadOptions();

            UpdatePreview();

            if (LayoutToExport != null)
                layoutPreview.CurrentLayout = LayoutToExport.Layout;

            ExportOptions.AttachPropertyChangedEvent();
            ExportOptions.PropertyChanged += ExportOptions_PropertyChanged;
        }

        private void ExportOptions_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdatePreview();
        }

        private void LoadOptions()
        {
            isLoading = true;

            chkExtendFretSlots.Checked = ExportOptions.ExtendFretSlots;
            ExportFretsPanel.Checked = ExportOptions.ExportFrets;
            ExportStringsPanel.Checked = ExportOptions.ExportStrings;
            ExportMidlinesPanel.Checked = ExportOptions.ExportMidlines;
            chkExportCenterLine.Checked = ExportOptions.ExportCenterLine;
            ExportFingerboardPanel.Checked = ExportOptions.ExportFingerboardEdges;
            
            FretboardCfgEdit.LineConfig = ExportOptions.FingerboardEdges;
            FretLinesCfgEdit.LineConfig = ExportOptions.Frets;
            StringLinesCfgEdit.LineConfig = ExportOptions.Strings;

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
            if (!HasInitialized)
                return;

            var layoutCfg = layoutPreview.DisplayConfig;
            var exportCfg = ExportOptions;

            layoutCfg.Midlines.Visible = exportCfg.ExportMidlines;
            layoutCfg.Midlines.Color = exportCfg.Midlines.Color;

            layoutCfg.Margins.Visible = exportCfg.FingerboardMargins.Enabled;
            layoutCfg.Margins.Color = exportCfg.FingerboardMargins.Color;

            layoutCfg.CenterLine.Visible = exportCfg.ExportCenterLine;
            layoutCfg.CenterLine.Color = exportCfg.CenterLine.Color;

            layoutCfg.Fingerboard.Color = exportCfg.FingerboardEdges.Color;
            layoutCfg.Fingerboard.Visible = exportCfg.ExportFingerboardEdges;

            layoutCfg.Frets.Visible = exportCfg.Frets.Enabled;
            layoutCfg.Frets.Color = exportCfg.Frets.Color;

            layoutCfg.Strings.Visible = exportCfg.Strings.Enabled;
            layoutCfg.Strings.Color = exportCfg.Strings.Color;
            layoutCfg.Strings.RenderMode =
                exportCfg.Strings.UseStringGauge ? 
                LineRenderMode.RealWidth : 
                LineRenderMode.PlainLine;

            layoutPreview.DisplayConfig.FretExtensionAmount = ExportOptions.Frets.ExtensionAmount;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (!HasInitialized)
                return;

            using (var sfd = new SaveFileDialog())
            {
                if (!string.IsNullOrEmpty(LayoutToExport.DocumentName))
                    sfd.FileName = LayoutToExport.DocumentName;
                else
                    sfd.FileName = "layout";


                sfd.Filter = "Scalable Vector Graphics File|*.svg|Drawing Interchange Format File|*.dxf|All files|*.*";
                
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

        private void rbExtendInward_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                mtbFretExtendAmount_ValueChanged(mtbFretExtendAmount, EventArgs.Empty);
        }

        private void chkExtendFretSlots_CheckedChanged(object sender, EventArgs e)
        {
            flpExtendDirection.Enabled = chkExtendFretSlots.Checked;
            mtbFretExtendAmount.Enabled = chkExtendFretSlots.Checked;

            if (!isLoading && HasInitialized)
            {
                if (chkExtendFretSlots.Checked)
                    mtbFretExtendAmount_ValueChanged(mtbFretExtendAmount, EventArgs.Empty);
                else
                    ExportOptions.Frets.ExtensionAmount = Measure.Empty;
                //UpdatePreview();
            }
        }

        private void mtbFretExtendAmount_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
            {
                if (!mtbFretExtendAmount.Value.IsEmpty && mtbFretExtendAmount.Value != Measure.Zero)
                {
                    var amount = mtbFretExtendAmount.Value;
                    if (rbExtendInward.Checked)
                        amount = amount * -1;
                    ExportOptions.Frets.ExtensionAmount = amount;
                    //UpdatePreview();
                }
            }
        }

        private void chkFretThickness_CheckedChanged(object sender, EventArgs e)
        {
            mtbFretThickness.Enabled = chkFretThickness.Checked;

            if (!isLoading && HasInitialized)
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
            if (!isLoading && HasInitialized)
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

        private void ExportFretsPanel_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                ExportOptions.ExportFrets = ExportFretsPanel.Checked;
            //UpdatePreview();
        }

        private void ExportStringsPanel_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                ExportOptions.ExportStrings = ExportStringsPanel.Checked;
        }

        private void ExportMidlinesPanel_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                ExportOptions.ExportMidlines = ExportMidlinesPanel.Checked;
        }

        private void chkExportCenterLine_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                ExportOptions.ExportCenterLine = chkExportCenterLine.Checked;
        }

        private void chkExportMargins_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                ExportOptions.ExportFingerboardMargins = chkExportMargins.Checked;
        }

        private void ExportFingerboardPanel_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading && HasInitialized)
                ExportOptions.ExportFingerboardEdges = ExportFingerboardPanel.Checked;
        }

        private void btnPickFretColor_Click(object sender, EventArgs e)
        {
            if (!HasInitialized)
                return;

            using (var dlg = new ColorDialog())
            {
                dlg.Color = ExportOptions.Frets.Color;
                if(dlg.ShowDialog() == DialogResult.OK)
                {
                    ExportOptions.Frets.Color = dlg.Color;
                    pbxFretColor.BackColor = dlg.Color;
                }
            }
        }
    }
}
