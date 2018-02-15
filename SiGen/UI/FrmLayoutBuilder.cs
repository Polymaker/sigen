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

namespace SiGen.UI
{
    public partial class FrmLayoutBuilder : Form
    {

        private bool isLoading;
        private bool internalChange;

        public FrmLayoutBuilder()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            layoutViewer1.CurrentLayout = CreateDefaultLayout();
            
            UpdateParameters();
        }

        private static SILayout CreateDefaultLayout()
        {
            var layout = new SILayout();
            layout.NumberOfStrings = 6;
            layout.SingleScaleConfig.Length = Measure.Inches(25.5);
            layout.MultiScaleConfig.Treble = Measure.Inches(25.5);
            layout.MultiScaleConfig.Bass = Measure.Inches(27);

            var spacing = (StringSpacingSimple)layout.StringSpacing;
            spacing.StringSpacingAtNut = Measure.Mm(7.3);
            spacing.StringSpacingAtBridge = Measure.Mm(10.5);
            layout.Margins.Edges = Measure.Mm(3.25);
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
                if(layout.StringSpacing is StringSpacingSimple)
                {
                    var spacing = (StringSpacingSimple)layout.StringSpacing;
                    if(sender == meSpacingNut1)
                    {
                        spacing.StringSpacingAtNut = meSpacingNut1.Value;
                    }
                    else if (sender == meSpacingNut2)
                    {
                        spacing.StringSpreadAtNut = meSpacingNut2.Value;
                    }
                    else if (sender == meSpacingBridge1)
                    {
                        spacing.StringSpacingAtBridge = meSpacingBridge1.Value;
                    }
                    else if (sender == meSpacingBridge2)
                    {
                        spacing.StringSpreadAtBridge = meSpacingBridge2.Value;
                    }
                    FillSpacingValues();
                    RebuildLayoutIfNeeded();
                }
            }
        }

        private void FillSpacingValues()
        {
            var spacing = (StringSpacingSimple)layoutViewer1.CurrentLayout.StringSpacing;
            SetControlValue(meSpacingNut1, spacing.StringSpacingAtNut);
            SetControlValue(meSpacingNut2, spacing.StringSpreadAtNut);
            SetControlValue(meSpacingBridge1, spacing.StringSpacingAtBridge);
            SetControlValue(meSpacingBridge2, spacing.StringSpreadAtBridge);
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

    }
}
