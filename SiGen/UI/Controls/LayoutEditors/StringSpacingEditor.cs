using System;
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
            InitializeComboboxes();
        }

        private void InitializeComboboxes()
        {

            //var alignmentList = new List<Tuple<StringSpacingAlignment, string>>();

            //alignmentList.Add(new Tuple<StringSpacingAlignment, string>(StringSpacingAlignment.SpacingMiddle, "Centered equally"));
            //alignmentList.Add(new Tuple<StringSpacingAlignment, string>(StringSpacingAlignment.StringCenter, "Centered in between the strings"));

            //cboNutAlignment.DataSource = alignmentList;
            //cboNutAlignment.ValueMember = "Item1";
            //cboNutAlignment.DisplayMember = "Item2";
            //cboNutAlignment.SelectedValue = StringSpacingAlignment.SpacingMiddle;
        }

        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            groupBox1.Enabled = (CurrentLayout != null);
            groupBox2.Enabled = (CurrentLayout != null);

            if (CurrentLayout != null)
            {

                //cboNutAlignment.SelectedValue = CurrentLayout.StringSpacing.NutAlignment;
                mtbNutSpacing.Value = CurrentLayout.SimpleStringSpacing.StringSpacingAtNut;
                mtbNutSpread.Value = CurrentLayout.SimpleStringSpacing.StringSpreadAtNut;

                mtbBridgeSpacing.Value = CurrentLayout.SimpleStringSpacing.StringSpacingAtBridge;
                mtbBridgeSpread.Value = CurrentLayout.SimpleStringSpacing.StringSpreadAtBridge;

                mtbNutSpacing.AllowEmptyValue = mtbNutSpread.AllowEmptyValue = false;
                mtbBridgeSpacing.AllowEmptyValue = mtbBridgeSpread.AllowEmptyValue = false;

                cboNutSpacingMethod.SelectedIndex = (int)CurrentLayout.SimpleStringSpacing.NutSpacingMode;
                chkAlignCenter.Checked = (CurrentLayout.StringSpacing.NutAlignment == StringSpacingAlignment.StringCenter);
                chkAlignCenter.Enabled = (CurrentLayout.SimpleStringSpacing.NutSpacingMode == NutSpacingMode.BetweenStrings);
            }
            else
            {
                mtbNutSpacing.AllowEmptyValue = mtbNutSpread.AllowEmptyValue = true;
                mtbNutSpacing.Value = Measure.Empty;
                mtbNutSpread.Value = Measure.Empty;

                mtbBridgeSpacing.AllowEmptyValue = mtbBridgeSpread.AllowEmptyValue = true;
                mtbBridgeSpacing.Value = Measure.Empty;
                mtbBridgeSpread.Value = Measure.Empty;
                chkAlignCenter.Checked = false;
                cboNutSpacingMethod.SelectedItem = null;
            }
        }

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

        private void chkAlignCenter_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                if (chkAlignCenter.Checked)
                    CurrentLayout.StringSpacing.NutAlignment = StringSpacingAlignment.StringCenter;
                else
                    CurrentLayout.StringSpacing.NutAlignment = StringSpacingAlignment.SpacingMiddle;
                CurrentLayout.RebuildLayout();
            }
        }

        private void cboNutSpacingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.SimpleStringSpacing.NutSpacingMode = (NutSpacingMode)cboNutSpacingMethod.SelectedIndex;
                chkAlignCenter.Enabled = (CurrentLayout.SimpleStringSpacing.NutSpacingMode == NutSpacingMode.BetweenStrings);
                CurrentLayout.RebuildLayout();
            }
        }
    }
}
