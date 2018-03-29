using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.Utilities;

namespace SiGen.UI.Controls
{
    [ToolboxItem(true)]
    public partial class FingerboardMarginEditor : LayoutPropertyEditor
    {
        private enum MarginEditMode
        {
            Edges,
            [Description("Nut & Bridge")]
            NutBridge,
            [Description("Bass & Treble")]
            BassTreble,
            [Description("All Sides")]
            All
        }

        private MarginEditMode EditMode;
        private Dictionary<MarginEditMode, Measure[]> CachedValues;
        

        private bool MarginModified;

        public FingerboardMarginEditor()
        {
            InitializeComponent();
            CachedValues = new Dictionary<MarginEditMode, Measure[]>();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            InitializeCombobox();
            ReadLayoutProperties();
        }

        private void InitializeCombobox()
        {
            var modeList = new List<Tuple<MarginEditMode, string>>();
            foreach (MarginEditMode value in Enum.GetValues(typeof(MarginEditMode)))
                modeList.Add(new Tuple<MarginEditMode, string>(value, EnumHelper.GetEnumDescription(typeof(MarginEditMode), value)));
            cboMarginEditMode.DataSource = modeList;
            cboMarginEditMode.ValueMember = "Item1";
            cboMarginEditMode.DisplayMember = "Item2";
            cboMarginEditMode.SelectedValue = MarginEditMode.Edges;
        }

        protected override void OnLayoutChanged()
        {
            base.OnLayoutChanged();
            CachedValues.Clear();
        }

        protected override void CacheCurrentLayoutValues()
        {
            base.CacheCurrentLayoutValues();
            CachedLayoutData[CurrentLayout]["EditMode"] = (MarginEditMode)cboMarginEditMode.SelectedValue;
            CachedLayoutData[CurrentLayout]["Values"] = CachedValues;
        }

        protected override void RestoreCachedLayoutValues()
        {
            base.RestoreCachedLayoutValues();
            cboMarginEditMode.SelectedValue = CachedLayoutData[CurrentLayout]["EditMode"];
            CachedValues = (Dictionary<MarginEditMode, Measure[]>)CachedLayoutData[CurrentLayout]["Values"];
        }

        protected override void ReadLayoutProperties()
        {
            cboMarginEditMode.Enabled = (CurrentLayout != null);
            mtbLastFret.Enabled = (CurrentLayout != null);
            mtbNutBass.Enabled = (CurrentLayout != null);
            MarginModified = false;

            if (CurrentLayout != null)
            {
                ReadMarginValues();
            }
            else
            {
                mtbLastFret.Value = Measure.Empty;
                mtbNutBass.Value = Measure.Empty;
                mtbNutTreble.Value = Measure.Zero;
                mtbBridgeBass.Value = Measure.Zero;
                mtbBridgeTreble.Value = Measure.Zero;
            }

            EditMode = GetMarginsEditMode();
            cboMarginEditMode.SelectedValue = EditMode;
            UpdateMarginsUI();
        }

        private void ReadMarginValues()
        {
            mtbLastFret.Value = CurrentLayout.Margins.LastFret;
            mtbNutBass.Value = CurrentLayout.Margins.NutMargins[FingerboardSide.Bass];
            mtbNutTreble.Value = CurrentLayout.Margins.NutMargins[FingerboardSide.Treble];
            mtbBridgeBass.Value = CurrentLayout.Margins.BridgeMargins[FingerboardSide.Bass];
            mtbBridgeTreble.Value = CurrentLayout.Margins.BridgeMargins[FingerboardSide.Treble];
        }

        private MarginEditMode GetMarginsEditMode()
        {
            if (CurrentLayout != null)
            {
                if (!CurrentLayout.Margins.Edges.IsEmpty)
                    return MarginEditMode.Edges;
                else if (!CurrentLayout.Margins.MarginAtNut.IsEmpty && !CurrentLayout.Margins.MarginAtBridge.IsEmpty)
                    return MarginEditMode.NutBridge;
                else if (!CurrentLayout.Margins.Bass.IsEmpty && !CurrentLayout.Margins.Treble.IsEmpty)
                    return MarginEditMode.BassTreble;
                else
                    return MarginEditMode.All;
            }

            return MarginEditMode.Edges;
        }

        private void UpdateMarginsUI()
        {
            mtbNutTreble.Visible = (EditMode == MarginEditMode.BassTreble || EditMode == MarginEditMode.All);
            mtbBridgeTreble.Visible = (EditMode == MarginEditMode.All);
            mtbBridgeBass.Visible = (EditMode == MarginEditMode.NutBridge || EditMode == MarginEditMode.All);
            lblBass.Visible = lblTreble.Visible = (EditMode == MarginEditMode.BassTreble || EditMode == MarginEditMode.All);
            lblNut.Visible = lblBridge.Visible = (EditMode == MarginEditMode.NutBridge || EditMode == MarginEditMode.All);
            tableLayoutPanel1.PerformLayout();

            int totalHeight = 0;
            var rowHeights = tableLayoutPanel1.GetRowHeights();
            for (int i = 0; i < rowHeights.Length - 1; i++)
                totalHeight += rowHeights[i];
            tableLayoutPanel1.Height = totalHeight;
        }

        private void mtbLastFret_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.Margins.LastFret = mtbLastFret.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void mtbFingerboardMargins_ValueChanged(object sender, EventArgs e)
        {
            if (!(IsLoading || FlagManager["ForceRead"]) && CurrentLayout != null)
            {
                SetMarginValues(sender);
                MarginModified = true;
            }
        }

        private void SetMarginValues(object sender)
        {
            if (CurrentLayout != null)
            {
                switch (EditMode)
                {
                    case MarginEditMode.Edges:
                        CurrentLayout.Margins.Edges = mtbNutBass.Value;
                        break;
                    case MarginEditMode.NutBridge:
                        if (sender == mtbNutBass)
                            CurrentLayout.Margins.MarginAtNut = mtbNutBass.Value;
                        else if (sender == mtbBridgeBass)
                            CurrentLayout.Margins.MarginAtBridge = mtbBridgeBass.Value;
                        else
                        {
                            CurrentLayout.Margins.MarginAtNut = mtbNutBass.Value;
                            CurrentLayout.Margins.MarginAtBridge = mtbBridgeBass.Value;
                        }
                        break;
                    case MarginEditMode.BassTreble:
                        if (sender == mtbNutBass)
                            CurrentLayout.Margins.Bass = mtbNutBass.Value;
                        else if (sender == mtbNutTreble)
                            CurrentLayout.Margins.Treble = mtbNutTreble.Value;
                        else
                        {
                            CurrentLayout.Margins.Bass = mtbNutBass.Value;
                            CurrentLayout.Margins.Treble = mtbNutTreble.Value;
                        }
                        break;
                    case MarginEditMode.All:

                        if (sender == mtbNutBass)
                            CurrentLayout.Margins.NutMargins[FingerboardSide.Bass] = mtbNutBass.Value;
                        else if (sender == mtbNutTreble)
                            CurrentLayout.Margins.NutMargins[FingerboardSide.Treble] = mtbNutTreble.Value;
                        else if (sender == mtbBridgeBass)
                            CurrentLayout.Margins.BridgeMargins[FingerboardSide.Bass] = mtbBridgeBass.Value;
                        else if (sender == mtbBridgeTreble)
                            CurrentLayout.Margins.BridgeMargins[FingerboardSide.Treble] = mtbBridgeTreble.Value;
                        else
                        {
                            CurrentLayout.Margins.NutMargins[FingerboardSide.Bass] = mtbNutBass.Value;
                            CurrentLayout.Margins.NutMargins[FingerboardSide.Treble] = mtbNutTreble.Value;
                            CurrentLayout.Margins.BridgeMargins[FingerboardSide.Bass] = mtbBridgeBass.Value;
                            CurrentLayout.Margins.BridgeMargins[FingerboardSide.Treble] = mtbBridgeTreble.Value;
                        }
                        break;
                }
                CurrentLayout.RebuildLayout();
            } 
        }

        private void cboMarginEditMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!IsLoading && CurrentLayout != null)
            {
                if(MarginModified)
                {
                    CacheCurrentValues();
                    if (CachedValues.ContainsKey(MarginEditMode.All) && EditMode != MarginEditMode.All)
                        CachedValues.Remove(MarginEditMode.All);
                }

                EditMode = (MarginEditMode)cboMarginEditMode.SelectedValue;

                using (FlagManager.UseFlag("ForceRead"))
                {
                    if (CachedValues.ContainsKey(EditMode))
                        ReadCachedValues();
                    else
                        ReadMarginValues();
                }

                MarginModified = false;
                SetMarginValues(null);
                UpdateMarginsUI();
            }
        }

        private void CacheCurrentValues()
        {
            var values = new Measure[]
            {
                CurrentLayout.Margins.NutMargins[FingerboardSide.Bass],
                CurrentLayout.Margins.NutMargins[FingerboardSide.Treble],
                CurrentLayout.Margins.BridgeMargins[FingerboardSide.Bass],
                CurrentLayout.Margins.BridgeMargins[FingerboardSide.Treble]
            };
            if (!CachedValues.ContainsKey(EditMode))
                CachedValues.Add(EditMode, values);
            else
                CachedValues[EditMode] = values;
        }

        private void ReadCachedValues()
        {
            if (CachedValues.ContainsKey(EditMode))
            {
                mtbNutBass.Value = CachedValues[EditMode][0];
                mtbNutTreble.Value = CachedValues[EditMode][1];
                mtbBridgeBass.Value = CachedValues[EditMode][2];
                mtbBridgeTreble.Value = CachedValues[EditMode][3];
            }
        }
    }
}
