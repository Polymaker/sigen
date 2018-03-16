using System;
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

namespace SiGen.UI.Controls
{
    [ToolboxItem(true)]
    public partial class FingerboardMarginEditor : LayoutPropertyEditor
    {
        private enum MarginEditMode
        {
            Edges,
            NutBridge,
            BassTreble,
            All
        }

        private MarginEditMode EditMode;

        public FingerboardMarginEditor()
        {
            InitializeComponent();
        }

        protected override void ReadLayoutProperties()
        {
            mtbLastFret.Enabled = (CurrentLayout != null);
            mtbNutBass.Enabled = (CurrentLayout != null);

            if (CurrentLayout != null)
            {
                mtbLastFret.Value = CurrentLayout.Margins.LastFret;
                mtbNutBass.Value = CurrentLayout.Margins.NutMargins[FingerboardSide.Bass];
                mtbNutTreble.Value = CurrentLayout.Margins.NutMargins[FingerboardSide.Treble];
                mtbBridgeBass.Value = CurrentLayout.Margins.BridgeMargins[FingerboardSide.Bass];
                mtbBridgeTreble.Value = CurrentLayout.Margins.BridgeMargins[FingerboardSide.Treble];
            }
            else
            {
                mtbLastFret.Value = Measure.Empty;
                mtbNutBass.Value = Measure.Zero;
                mtbNutTreble.Value = Measure.Zero;
                mtbBridgeBass.Value = Measure.Zero;
                mtbBridgeTreble.Value = Measure.Zero;
            }

            EditMode = GetMarginsEditMode();
            UpdateMarginsUI();
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
        }

        private void mtbLastFret_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
                CurrentLayout.Margins.LastFret = mtbLastFret.Value;
        }

        private void mtbFingerboardMargins_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
                SetMarginValues(sender);
        }

        private void SetMarginValues(object sender)
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
        }
    }
}
