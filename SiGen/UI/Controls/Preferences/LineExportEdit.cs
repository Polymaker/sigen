﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Utilities;
using SiGen.Export;

namespace SiGen.UI.Controls.Preferences
{
    public partial class LineExportEdit : UserControl
    {
        private FlagList FlagManager;
        private LineExportConfig _LineConfig;

        public LineExportConfig LineConfig
        {
            get => _LineConfig;
            set => BindExportConfig(value);
        }

        public LineExportEdit()
        {
            InitializeComponent();
            FlagManager = new FlagList();
            StringGaugeCheckbox.Visible = false;
            ExtraOption1Checkbox.Visible = false;
            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private void BindExportConfig(LineExportConfig exportConfig)
        {
            //if (_LineConfig != null)
            //    _LineConfig.PropertyChanged -= LineConfig_PropertyChanged;

            _LineConfig = exportConfig;

            LoadConfigValues();

            //if (exportConfig != null)
            //    _LineConfig.PropertyChanged += LineConfig_PropertyChanged;
        }

        private void LoadConfigValues()
        {
            using (FlagManager.UseFlag("LoadConfig"))
            {
                ColorSelector.Value = LineConfig?.Color ?? Color.Black;

                if (LineConfig != null)
                    ThicknessEditor.SetValues(LineConfig.LineThickness, LineConfig.LineUnit);
                else
                    ThicknessEditor.SetValues(1f, LineUnit.Points);

                DashedCheckbox.Checked = LineConfig?.IsDashed ?? false;

                ThicknessEditor.Enabled = true;

                StringGaugeCheckbox.Visible = false;
                ExtraOption1Checkbox.Visible = false;

                switch (LineConfig)
                {
                    case StringsExportConfig stringsCfg:
                        {
                            StringGaugeCheckbox.Visible = true;
                            ThicknessEditor.Enabled = !stringsCfg.UseStringGauge;
                            StringGaugeCheckbox.Checked = stringsCfg.UseStringGauge;
                            break;
                        }

                    case FingerboardExportConfig fingerboardCfg:
                        {
                            ExtraOption1Checkbox.Text = ContinueEdgeLines;
                            ExtraOption1Checkbox.Visible = true;
                            ExtraOption1Checkbox.Checked = fingerboardCfg.ContinueLines;
                            break;
                        }

                    case FretsExportConfig fretCfg:
                        {
                            ExtraOption1Checkbox.Text = IncludeBridgeLine;
                            ExtraOption1Checkbox.Visible = true;
                            ExtraOption1Checkbox.Checked = fretCfg.ExportBridgeLine;
                            break;
                        }
                }

                AdjustControlHeight();
            }
        }

        //private void LineConfig_PropertyChanged(object sender, PropertyChangedEventArgs e)
        //{
            
        //}

        public void AdjustControlHeight()
        {
            var prefSize = GetPreferredSize(new Size(9999, 20));
            Height = prefSize.Height;
        }

        private void ColorSelector_ValueChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
                LineConfig.Color = ColorSelector.Value;
        }

        private void ThicknessEditor_ThicknessChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && LineConfig != null)
            {
                LineConfig.LineThickness = ThicknessEditor.SelectedThickness;
                LineConfig.LineUnit = ThicknessEditor.SelectedUnit;
            }
        }

        private void StringGaugeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig") && 
                LineConfig is StringsExportConfig stringsCfg)
            {
                stringsCfg.UseStringGauge = StringGaugeCheckbox.Checked;
                ThicknessEditor.Enabled = !StringGaugeCheckbox.Checked;
            }
        }

        private void ExtraOption1Checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!FlagManager.IsSet("LoadConfig"))
            {
                if (LineConfig is FingerboardExportConfig fingerboardCfg)
                {
                    fingerboardCfg.ContinueLines = ExtraOption1Checkbox.Checked;
                }
                else if (LineConfig is FretsExportConfig fretCfg)
                {
                    fretCfg.ExportBridgeLine = ExtraOption1Checkbox.Checked;
                }
            }
        }
    }
}
