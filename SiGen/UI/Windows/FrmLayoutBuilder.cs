using SiGen.Export;
using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
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

namespace SiGen.UI
{
    public partial class FrmLayoutBuilder : Form
    {

        private bool isLoading;
        private bool internalChange;
        private LayoutFile _CurrentFile;

        public FrmLayoutBuilder()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            LoadLayout(new LayoutFile(CreateDefaultLayout()));
        }

        private static SILayout CreateDefaultLayout()
        {
            var layout = new SILayout();
            layout.NumberOfStrings = 6;
            layout.SingleScaleConfig.Length = Measure.Inches(25.5);
            layout.MultiScaleConfig.Treble = Measure.Inches(25.5);
            layout.MultiScaleConfig.Bass = Measure.Inches(27);
            
            layout.SetStringsTuning(
                MusicalNote.EqualNote(NoteName.E, 4),
                MusicalNote.EqualNote(NoteName.B, 3),
                MusicalNote.EqualNote(NoteName.G, 3),
                MusicalNote.EqualNote(NoteName.D, 3),
                MusicalNote.EqualNote(NoteName.A, 2),
                MusicalNote.EqualNote(NoteName.E, 2)
                );
            layout.Strings.MassAssign(s => s.PhysicalProperties,
                new StringProperties(Measure.Inches(0.010), Measure.Inches(0.010), 0.00002215, 29442660.75919),
                new StringProperties(Measure.Inches(0.013), Measure.Inches(0.013), 0.00003744, 29442660.75919),
                new StringProperties(Measure.Inches(0.017), Measure.Inches(0.017), 0.00006402, 29442660.75919),
                new StringProperties(Measure.Inches(0.014), Measure.Inches(0.026), 0.00012671, 29442660.75919),
                new StringProperties(Measure.Inches(0.014), Measure.Inches(0.036), 0.00023964, 29442660.75919),
                new StringProperties(Measure.Inches(0.016), Measure.Inches(0.046), 0.00038216, 29442660.75919)
                );
            layout.Strings.MassAssign(s => s.ActionAtTwelfthFret,
                Measure.Inches(0.063),
                Measure.Inches(0.069),
                Measure.Inches(0.075),
                Measure.Inches(0.082),
                Measure.Inches(0.088),
                Measure.Inches(0.094)
                );

            layout.SimpleStringSpacing.StringSpacingAtNut = Measure.Mm(7.3);
            layout.SimpleStringSpacing.StringSpacingAtBridge = Measure.Mm(10.5);
            layout.SimpleStringSpacing.NutSpacingMode = NutSpacingMode.BetweenStrings;
            //layout.ScaleLengthMode = ScaleLengthType.Individual;
            layout.Margins.Edges = Measure.Mm(3.25);
            layout.Margins.LastFret = Measure.Mm(10);
            layout.RebuildLayout();
            return layout;
        }

        private void UpdateParameters()
        {
            isLoading = true;
            var layout = layoutViewer1.CurrentLayout;
            switch (layout.ScaleLengthMode)
            {
                case ScaleLengthType.Single:
                    rbSingleScale.Checked = true;
                    break;
                case ScaleLengthType.Multiple:
                    rbMultiScale.Checked = true;
                    break;
                case ScaleLengthType.Individual:
                    radioButton1.Checked = true;
                    break;
            }
            nudNumberOfStrings.Value = layout.NumberOfStrings;
            SetControlValue(nudNumberOfFrets, layout.Strings[0].NumberOfFrets);
            meSingleScale.Value = layout.SingleScaleConfig.Length;
            meTrebleScale.Value = layout.MultiScaleConfig.Treble;
            meBassScale.Value = layout.MultiScaleConfig.Bass;
            nudMultiScaleOffset.Value = (decimal)layout.MultiScaleConfig.PerpendicularFretRatio;
            FillSpacingValues();
            isLoading = false;
        }

        private void meSingleScale_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                layoutViewer1.CurrentLayout.SingleScaleConfig.Length = meSingleScale.Value;
                RebuildLayoutIfNeeded();
            }
        }

        private void nudNumberOfStrings_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                layoutViewer1.CurrentLayout.NumberOfStrings = (int)nudNumberOfStrings.Value;
                FillSpacingValues();
                RebuildLayoutIfNeeded();
            }
        }

        private void meTrebleScale_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                layoutViewer1.CurrentLayout.MultiScaleConfig.Treble = meTrebleScale.Value;
                RebuildLayoutIfNeeded();
            }
        }

        private void meBassScale_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                layoutViewer1.CurrentLayout.MultiScaleConfig.Bass = meBassScale.Value;
                RebuildLayoutIfNeeded();
            }
        }

        private void nudMultiScaleOffset_ValueChanged(object sender, EventArgs e)
        {
            if (!isLoading && !internalChange)
            {
                layoutViewer1.CurrentLayout.MultiScaleConfig.PerpendicularFretRatio = (double)nudMultiScaleOffset.Value;
                var valueStr = layoutViewer1.CurrentLayout.MultiScaleConfig.PerpendicularFretRatio.ToString();
                if (valueStr.Contains("."))
                    nudMultiScaleOffset.DecimalPlaces = valueStr.Length - (valueStr.IndexOf('.') + 1);
                RebuildLayoutIfNeeded();
            }
        }

        private void ScaleLengthMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!isLoading)
            {
                var button = (RadioButton)sender;
                if (button.Checked)
                {
                    var layout = layoutViewer1.CurrentLayout;
                    if (button == rbSingleScale)
                        layout.ScaleLengthMode = ScaleLengthType.Single;
                    else if (button == rbMultiScale)
                        layout.ScaleLengthMode = ScaleLengthType.Multiple;

                    RebuildLayoutIfNeeded();

                }
            }

            lblScaleLength.Visible = rbSingleScale.Checked;
            meSingleScale.Visible = rbSingleScale.Checked;

            lblTrebleLength.Visible = rbMultiScale.Checked;
            meTrebleScale.Visible = rbMultiScale.Checked;
            lblBassLength.Visible = rbMultiScale.Checked;
            meBassScale.Visible = rbMultiScale.Checked;
            nudMultiScaleOffset.Visible = rbMultiScale.Checked;
            lblMultiScaleRatio.Visible = rbMultiScale.Checked;
            gbxScaleLength.Height = tlpScaleLenghts.Bottom + 6;
        }

        private void RebuildLayoutIfNeeded()
        {
            if (layoutViewer1.CurrentLayout.IsLayoutDirty)
                layoutViewer1.CurrentLayout.RebuildLayout();
        }

        #region String spacing

        private void StringSpacingChanged(object sender, EventArgs e)
        {
            if (!isLoading && !internalChange && layoutViewer1.CurrentLayout != null)
            {
                var layout = layoutViewer1.CurrentLayout;
                if(layout.StringSpacingMode == StringSpacingType.Simple)
                {
                    if(sender == meSpacingNut1)
                    {
                        layout.SimpleStringSpacing.StringSpacingAtNut = meSpacingNut1.Value;
                    }
                    else if (sender == meSpacingNut2)
                    {
                        layout.SimpleStringSpacing.StringSpreadAtNut = meSpacingNut2.Value;
                    }
                    else if (sender == meSpacingBridge1)
                    {
                        layout.SimpleStringSpacing.StringSpacingAtBridge = meSpacingBridge1.Value;
                    }
                    else if (sender == meSpacingBridge2)
                    {
                        layout.SimpleStringSpacing.StringSpreadAtBridge = meSpacingBridge2.Value;
                    }
                    FillSpacingValues();
                    RebuildLayoutIfNeeded();
                }
            }
        }

        private void FillSpacingValues()
        {
            var layout = layoutViewer1.CurrentLayout;
            SetControlValue(meSpacingNut1, layout.SimpleStringSpacing.StringSpacingAtNut);
            SetControlValue(meSpacingNut2, layout.SimpleStringSpacing.StringSpreadAtNut);
            SetControlValue(meSpacingBridge1, layout.SimpleStringSpacing.StringSpacingAtBridge);
            SetControlValue(meSpacingBridge2, layout.SimpleStringSpacing.StringSpreadAtBridge);
        }

        #endregion

        private void SetControlValue(Control ctrl, object value, string propName = "Value")
        {
            internalChange = true;
            var valueProp = TypeDescriptor.GetProperties(ctrl.GetType())[propName];
            if (value != null && value.GetType() != valueProp.PropertyType)
            {
                if (value is IConvertible)
                    TypeDescriptor.GetProperties(ctrl.GetType())[propName].SetValue(ctrl, Convert.ChangeType(value, valueProp.PropertyType));
                else
                    TypeDescriptor.GetProperties(ctrl.GetType())[propName].SetValue(ctrl, TypeDescriptor.GetConverter(valueProp.PropertyType).ConvertFrom(value));
            }
            else
                TypeDescriptor.GetProperties(ctrl.GetType())[propName].SetValue(ctrl, value);

            internalChange = false;
        }

        private void nudNumberOfFrets_ValueChanged(object sender, EventArgs e)
        {
            if(!isLoading && !internalChange)
            {
                var layout = layoutViewer1.CurrentLayout;
                layout.Strings.SetAll(s => s.NumberOfFrets, (int)nudNumberOfFrets.Value);
                RebuildLayoutIfNeeded();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            layoutViewer1.DisplayConfig.ShowTheoreticalFrets = !layoutViewer1.DisplayConfig.ShowTheoreticalFrets;
            
        }

        #region Save

        private void SaveLayout(bool selectPath)
        {
            if (selectPath)
            {
                using (var sfd = new SaveFileDialog())
                {
                    sfd.FileName = "test.sil";
                    sfd.Filter = "SI Layout file (*.sil)|*.sil";
                    sfd.DefaultExt = ".sil";

                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        _CurrentFile.FileName = sfd.FileName;
                    }
                    else
                        return;
                }
            }

            _CurrentFile.Layout.Save(_CurrentFile.FileName);
        }

        private void tssbSave_ButtonClick(object sender, EventArgs e)
        {
            if (_CurrentFile != null)
                SaveLayout(string.IsNullOrEmpty(_CurrentFile.FileName));
        }

        private void tsmiSave_Click(object sender, EventArgs e)
        {
            if (_CurrentFile != null)
                SaveLayout(string.IsNullOrEmpty(_CurrentFile.FileName));
        }

        private void tsmiSaveAs_Click(object sender, EventArgs e)
        {
            if (_CurrentFile != null)
                SaveLayout(true);
        }

        #endregion

        private void tssbOpen_ButtonClick(object sender, EventArgs e)
        {
            using (var ofd = new OpenFileDialog())
            {
                ofd.Filter = "SI Layout file (*.sil)|*.sil";
                if(ofd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        var layoutToOpen = new LayoutFile(ofd.FileName);
                        LoadLayout(layoutToOpen);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show("An error occured: " + ex.ToString());
                    }
                }
            }
        }

        private class LayoutFile
        {
            public SILayout Layout { get; set; }
            public string FileName { get; set; }

            public LayoutFile(SILayout layout)
            {
                Layout = layout;
                FileName = string.Empty;
            }

            public LayoutFile(string filename)
            {
                FileName = filename;
                Layout = SILayout.Load(filename);
            }
        }

        private void LoadLayout(LayoutFile layout)
        {
            _CurrentFile = layout;

            if(layout == null)
            {
                layoutViewer1.CurrentLayout = null;
                fingerboardMarginEditor1.CurrentLayout = null;

            }
            else
            {
                if (_CurrentFile.Layout.VisualElements.Count == 0 || _CurrentFile.Layout.IsLayoutDirty)
                    _CurrentFile.Layout.RebuildLayout();

                layoutViewer1.CurrentLayout = _CurrentFile.Layout;
                layoutViewer1.Select();
                fingerboardMarginEditor1.CurrentLayout = _CurrentFile.Layout;
                UpdateParameters();
            }
        }

        private void exportAsSVGToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var sfd = new SaveFileDialog())
            {
                sfd.FileName = "layout.svg";
                sfd.Filter = "Scalable Vector Graphics File (*.svg)|*.svg";
                sfd.DefaultExt = ".svg";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    SvgLayoutExporter.ExportLayout(sfd.FileName, layoutViewer1.CurrentLayout,
                        new LayoutSvgExportOptions()
                        {
                            ExportStrings = false,
                            ExportStringCenters = false
                        });
                }
            }
        }

    }
}
