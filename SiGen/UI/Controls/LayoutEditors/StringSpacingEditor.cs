﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.StringedInstruments.Layout;
using SiGen.Utilities;
using SiGen.Measuring;
using SiGen.Resources;

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class StringSpacingEditor : LayoutPropertyEditor
    {
        public StringSpacingEditor()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateComboboxes();
        }

        private void UpdateComboboxes(bool preserveValues = false)
        {
            using (FlagManager.UseFlag("UpdateComboboxes"))
            {
                var alignmentList = new List<EnumHelper.EnumItem>();

                alignmentList.Add(new EnumHelper.EnumItem(StringSpacingAlignment.OuterStrings, 
                    Localizations.StringSpacingAlignment_OuterStrings));

                if (CurrentLayout == null || CurrentLayout.NumberOfStrings % 2 == 0)
                    alignmentList.Add(new EnumHelper.EnumItem(StringSpacingAlignment.MiddleString,
                        Localizations.StringSpacingAlignment_MiddleStringEven));
                else
                    alignmentList.Add(new EnumHelper.EnumItem(StringSpacingAlignment.MiddleString,
                        Localizations.StringSpacingAlignment_MiddleStringOdd));

                alignmentList.Add(new EnumHelper.EnumItem(StringSpacingAlignment.FingerboardEdges,
                    Localizations.StringSpacingAlignment_FingerboardEdges));

                cboNutSpacingAlignment.ValueMember = EnumHelper.EnumItem.ValueMember;
                cboNutSpacingAlignment.DisplayMember = EnumHelper.EnumItem.DisplayMember;
                cboBridgeSpacingAlignment.ValueMember = EnumHelper.EnumItem.ValueMember;
                cboBridgeSpacingAlignment.DisplayMember = EnumHelper.EnumItem.DisplayMember;

                cboNutSpacingAlignment.DataSource = alignmentList;
                cboBridgeSpacingAlignment.DataSource = alignmentList.ToList();//Clone

                var spacingModeList = new List<EnumHelper.EnumItem>();
                spacingModeList.Add(new EnumHelper.EnumItem(StringSpacingMethod.EqualDistance,
                    Localizations.StringSpacingMethod_EqualDistance));
                spacingModeList.Add(new EnumHelper.EnumItem(StringSpacingMethod.EqualSpacing,
                    Localizations.StringSpacingMethod_EqualSpacing));

                cboNutSpacingMethod.ValueMember = EnumHelper.EnumItem.ValueMember;
                cboNutSpacingMethod.DisplayMember = EnumHelper.EnumItem.DisplayMember;
                cboBridgeSpacingMethod.ValueMember = EnumHelper.EnumItem.ValueMember;
                cboBridgeSpacingMethod.DisplayMember = EnumHelper.EnumItem.DisplayMember;

                cboNutSpacingMethod.DataSource = spacingModeList;
                cboBridgeSpacingMethod.DataSource = spacingModeList.ToList();//Clone

                if (preserveValues && CurrentLayout != null)
                {
                    cboNutSpacingMethod.SelectedValue = CurrentLayout.SimpleStringSpacing.NutSpacingMode;
                    cboNutSpacingAlignment.SelectedValue = CurrentLayout.SimpleStringSpacing.NutAlignment;

                    cboBridgeSpacingMethod.SelectedValue = CurrentLayout.SimpleStringSpacing.BridgeSpacingMode;
                    cboBridgeSpacingAlignment.SelectedValue = CurrentLayout.SimpleStringSpacing.BridgeAlignment;
                }
                else
                {
                    cboNutSpacingAlignment.SelectedValue = StringSpacingAlignment.OuterStrings;
                    cboBridgeSpacingAlignment.SelectedValue = StringSpacingAlignment.OuterStrings;

                    cboNutSpacingMethod.SelectedValue = StringSpacingMethod.EqualDistance;
                    cboBridgeSpacingMethod.SelectedValue = StringSpacingMethod.EqualDistance;
                }
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
        }

        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            tlpNutSpacingAuto.Enabled = (CurrentLayout != null);
            tlpBridgeSpacingAuto.Enabled = (CurrentLayout != null);
            
            if (CurrentLayout != null)
            {
                UpdateComboboxes();

                mtbNutSpacing.Value = CurrentLayout.SimpleStringSpacing.StringSpacingAtNut;
                mtbNutSpread.Value = CurrentLayout.SimpleStringSpacing.StringSpreadAtNut;

                mtbBridgeSpacing.Value = CurrentLayout.SimpleStringSpacing.StringSpacingAtBridge;
                mtbBridgeSpread.Value = CurrentLayout.SimpleStringSpacing.StringSpreadAtBridge;

                mtbNutSpacing.AllowEmptyValue = mtbNutSpread.AllowEmptyValue = false;
                mtbBridgeSpacing.AllowEmptyValue = mtbBridgeSpread.AllowEmptyValue = false;

                cboNutSpacingMethod.SelectedValue = CurrentLayout.SimpleStringSpacing.NutSpacingMode;
                cboNutSpacingAlignment.SelectedValue = CurrentLayout.SimpleStringSpacing.NutAlignment;

                cboBridgeSpacingMethod.SelectedValue = CurrentLayout.SimpleStringSpacing.BridgeSpacingMode;
                cboBridgeSpacingAlignment.SelectedValue = CurrentLayout.SimpleStringSpacing.BridgeAlignment;
            }
            else
            {
                mtbNutSpacing.AllowEmptyValue = mtbNutSpread.AllowEmptyValue = true;
                mtbNutSpacing.Value = Measure.Empty;
                mtbNutSpread.Value = Measure.Empty;

                mtbBridgeSpacing.AllowEmptyValue = mtbBridgeSpread.AllowEmptyValue = true;
                mtbBridgeSpacing.Value = Measure.Empty;
                mtbBridgeSpread.Value = Measure.Empty;

                cboNutSpacingMethod.SelectedItem = null;
                cboNutSpacingAlignment.SelectedItem = null;
                cboBridgeSpacingMethod.SelectedItem = null;
                cboBridgeSpacingAlignment.SelectedItem = null;
            }
        }

        protected override void OnNumberOfStringsChanged()
        {
            base.OnNumberOfStringsChanged();

            using (FlagManager.UseFlag("UpdateSpacing"))
            {
                UpdateComboboxes(true);
                mtbNutSpread.Value = CurrentLayout.SimpleStringSpacing.StringSpreadAtNut;
                mtbBridgeSpread.Value = CurrentLayout.SimpleStringSpacing.StringSpreadAtBridge;
            }
        }

        #region Simple Nut Spacing Events

        private void mtbNutSpacing_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateSpacing"])
            {
                CurrentLayout.SimpleStringSpacing.StringSpacingAtNut = mtbNutSpacing.Value;
                using (FlagManager.UseFlag("UpdateSpacing"))
                    mtbNutSpread.Value = CurrentLayout.StringSpacing.StringSpreadAtNut;
                CurrentLayout.RebuildLayout();
            }
        }

        private void mtbNutSpread_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateSpacing"])
            {
                CurrentLayout.SimpleStringSpacing.StringSpreadAtNut = mtbNutSpread.Value;
                using (FlagManager.UseFlag("UpdateSpacing"))
                    mtbNutSpacing.Value = CurrentLayout.SimpleStringSpacing.StringSpacingAtNut;
                CurrentLayout.RebuildLayout();
            }
        }

        private void cboNutSpacingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateComboboxes"])
            {
                CurrentLayout.SimpleStringSpacing.NutSpacingMode = (StringSpacingMethod)cboNutSpacingMethod.SelectedValue;
                CurrentLayout.RebuildLayout();
            }
            if (cboNutSpacingMethod.SelectedItem != null && !FlagManager["UpdateComboboxes"])
            {
                if ((StringSpacingMethod)cboNutSpacingMethod.SelectedValue == StringSpacingMethod.EqualSpacing)
                    lblNutStringSpacing.Text = Text_AvgSpacing;
                else
                    lblNutStringSpacing.Text = Text_Spacing;
            }
        }

        private void cboNutSpacingAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateComboboxes"])
            {
                CurrentLayout.SimpleStringSpacing.NutAlignment = (StringSpacingAlignment)cboNutSpacingAlignment.SelectedValue;
                CurrentLayout.RebuildLayout();
            }
        }

        #endregion

        #region Simple Bridge Spacing Events

        private void mtbBridgeSpacing_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateSpacing"])
            {
                CurrentLayout.SimpleStringSpacing.StringSpacingAtBridge = mtbBridgeSpacing.Value;
                using (FlagManager.UseFlag("UpdateSpacing"))
                    mtbBridgeSpread.Value = CurrentLayout.StringSpacing.StringSpreadAtBridge;
                CurrentLayout.RebuildLayout();
            }
        }

        private void mtbBridgeSpread_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateSpacing"])
            {
                CurrentLayout.SimpleStringSpacing.StringSpreadAtBridge = mtbBridgeSpread.Value;
                using (FlagManager.UseFlag("UpdateSpacing"))
                    mtbBridgeSpacing.Value = CurrentLayout.SimpleStringSpacing.StringSpacingAtBridge;
                CurrentLayout.RebuildLayout();
            }
        }

        private void cboBridgeSpacingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateComboboxes"])
            {
                CurrentLayout.SimpleStringSpacing.BridgeSpacingMode = (StringSpacingMethod)cboBridgeSpacingMethod.SelectedValue;
                CurrentLayout.RebuildLayout();
            }
            if (cboBridgeSpacingMethod.SelectedItem != null)
            {
                if ((StringSpacingMethod)cboBridgeSpacingMethod.SelectedValue == StringSpacingMethod.EqualSpacing)
                    lblBridgeStringSpacing.Text = Text_AvgSpacing;
                else
                    lblBridgeStringSpacing.Text = Text_Spacing;
            }
        }

        private void cboBridgeSpacingAlignment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && !FlagManager["UpdateComboboxes"])
            {
                CurrentLayout.SimpleStringSpacing.BridgeAlignment = (StringSpacingAlignment)cboBridgeSpacingAlignment.SelectedValue;
                CurrentLayout.RebuildLayout();
            }
        }


        #endregion

        private void mtbSpacings_ValueChanging(object sender, MeasureTextbox.ValueChangingEventArgs e)
        {
            if((e.NewValue.IsEmpty && e.UserChange) || (!e.NewValue.IsEmpty && e.NewValue.NormalizedValue < 0.1))
            {
                e.Cancel = true;
                System.Media.SystemSounds.Exclamation.Play();
            }
        }
    }
}
