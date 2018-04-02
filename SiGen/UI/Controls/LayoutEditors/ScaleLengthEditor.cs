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
using SiGen.Measuring;

namespace SiGen.UI.Controls
{
    public partial class ScaleLengthEditor : LayoutPropertyEditor
    {
        private ScaleLengthType EditMode;
        private Dictionary<int, double> _FretPositions;

        public ScaleLengthEditor()
        {
            InitializeComponent();
            _FretPositions = new Dictionary<int, double>();
            dgvScaleLengths.AutoGenerateColumns = false;
        }

        #region RadioButton Glitch Handling

        //For some reasons, when the control is activated, the RadioButton that was last clicked by the user get checked
        //

        private bool enteringControl;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            (ParentForm as WeifenLuo.WinFormsUI.Docking.DockContent).DockPanel.ActiveContentChanged += DockPanel_ActiveContentChanged;
        }

        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);
            if (ParentForm is WeifenLuo.WinFormsUI.Docking.DockContent)
            {
                enteringControl = true;
                rbSingle.AutoCheck = false;
                rbDual.AutoCheck = false;
                rbMultiple.AutoCheck = false;
            }
        }

        private void DockPanel_ActiveContentChanged(object sender, EventArgs e)
        {
            if (enteringControl)
            {
                enteringControl = false;
                rbSingle.AutoCheck = true;
                rbDual.AutoCheck = true;
                rbMultiple.AutoCheck = true;
            }
        }

        #endregion

        private void FetchFretPositions()
        {
            var fretPos = new List<double>();
            int fretNum = CurrentLayout != null ? CurrentLayout.Strings.Max(s => s.NumberOfFrets) : 24;
            _FretPositions.Clear();
            _FretPositions.Add(0, 0);
            for (int i = 1; i <= fretNum; i++)
                _FretPositions.Add(i, 1d - SILayout.GetEqualTemperedFretPosition(i));
            _FretPositions.Add(-1, 1d);
        }

        protected override void OnCurrentLayoutChanged()
        {
            base.OnCurrentLayoutChanged();
            FetchFretPositions();
        }

        protected override void OnNumberOfStringsChanged()
        {
            base.OnNumberOfStringsChanged();
            if (CurrentLayout != null)
                dgvScaleLengths.DataSource = CurrentLayout.Strings;
        }

        protected override void ReadLayoutProperties()
        {
            if (dgvScaleLengths.IsCurrentCellInEditMode)
                dgvScaleLengths.CancelEdit();

            dgvScaleLengths.DataSource = null;
            
            if (CurrentLayout != null)
            {
                EditMode = CurrentLayout.ScaleLengthMode;
                dgvScaleLengths.DataSource = CurrentLayout.Strings;
            }
            else
                EditMode = ScaleLengthType.Single;

            ApplyLayout();
        }

        private void ApplyLayout()
        {
            flowLayoutPanel1.Enabled = (CurrentLayout != null);
            mtbTrebleLength.Enabled = (CurrentLayout != null);
            mtbBassLength.Enabled = (CurrentLayout != null);

            lblTreble.Visible = (EditMode == ScaleLengthType.Multiple);
            lblBass.Visible = (EditMode == ScaleLengthType.Multiple);
            lblMultiScaleRatio.Visible = (EditMode == ScaleLengthType.Multiple);
            mtbTrebleLength.Visible = (EditMode != ScaleLengthType.Individual);
            mtbBassLength.Visible = (EditMode == ScaleLengthType.Multiple);
            nubMultiScaleRatio.Visible = (EditMode == ScaleLengthType.Multiple);
            dgvScaleLengths.Visible = (EditMode == ScaleLengthType.Individual);

            SetSelectedEditMode(EditMode);

            if (CurrentLayout != null)
            {
                switch (EditMode)
                {
                    case ScaleLengthType.Single:
                        mtbTrebleLength.Value = CurrentLayout.SingleScaleConfig.Length;
                        mtbTrebleLength.AllowEmptyValue = false;
                        break;
                    case ScaleLengthType.Multiple:
                        {
                            mtbTrebleLength.Value = CurrentLayout.MultiScaleConfig.Treble;
                            mtbTrebleLength.AllowEmptyValue = false;
                            mtbBassLength.Value = CurrentLayout.MultiScaleConfig.Bass;
                            nubMultiScaleRatio.Value = CurrentLayout.MultiScaleConfig.PerpendicularFretRatio;
                            //if (_FretPositions.Values.Contains(CurrentLayout.MultiScaleConfig.PerpendicularFretRatio))
                            //{
                            //    var fretPos = _FretPositions.First(kv => kv.Value == CurrentLayout.MultiScaleConfig.PerpendicularFretRatio);

                            //    Console.WriteLine("Aligned to fret " + fretPos.Key);
                            //}
                        }
                        break;
                    case ScaleLengthType.Individual:
                        dgvScaleLengths.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        break;
                }

                

                int totalHeight = 0;
                var rowHeights = tableLayoutPanel1.GetRowHeights();
                for (int i = 0; i < rowHeights.Length - 1; i++)
                    totalHeight += rowHeights[i];
                if (dgvScaleLengths.Visible)
                    totalHeight += dgvScaleLengths.MinimumSize.Height;
                tableLayoutPanel1.MinimumSize = new Size(0, totalHeight);
                AutoScrollMinSize = new Size(0, totalHeight);
            }
            else
            {
                mtbTrebleLength.AllowEmptyValue = true;
                mtbTrebleLength.Value = Measure.Empty;
                tableLayoutPanel1.MinimumSize = Size.Empty;
                AutoScrollMinSize = Size.Empty;
            }
        }

        private void rbScaleLengthMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !FlagManager["SetMode"] && (sender as RadioButton).Checked)
            {
                if (enteringControl)
                {
                    SetSelectedEditMode(EditMode);
                    return;
                }

                EditMode = GetSelectedEditMode();
                if (CurrentLayout != null)
                {
                    //if(EditMode == ScaleLengthType.Multiple && !CurrentLayout.Strings.AllEqual(s=>s.MultiScaleRatio))

                    CurrentLayout.ScaleLengthMode = EditMode;
                    CurrentLayout.RebuildLayout();
                }
                ApplyLayout();
            }
        }

        private ScaleLengthType GetSelectedEditMode()
        {
            if (rbSingle.Checked)
                return ScaleLengthType.Single;
            else if (rbDual.Checked)
                return ScaleLengthType.Multiple;
            else if (rbMultiple.Checked)
                return ScaleLengthType.Individual;
            return ScaleLengthType.Single;
        }

        private void SetSelectedEditMode(ScaleLengthType mode)
        {
            using (FlagManager.UseFlag("SetMode"))
            {
                switch (mode)
                {
                    default:
                    case ScaleLengthType.Single:
                        rbSingle.Checked = true;
                        break;
                    case ScaleLengthType.Multiple:
                        rbDual.Checked = true;
                        break;
                    case ScaleLengthType.Individual:
                        rbMultiple.Checked = true;
                        break;
                }
            }
        }

        #region Value Changed Events

        private void mtbTrebleLength_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                if (EditMode == ScaleLengthType.Single)
                    CurrentLayout.SingleScaleConfig.Length = mtbTrebleLength.Value;
                else if (EditMode == ScaleLengthType.Multiple)
                    CurrentLayout.MultiScaleConfig.Treble = mtbTrebleLength.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void mtbBassLength_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && EditMode == ScaleLengthType.Multiple)
            {
                CurrentLayout.MultiScaleConfig.Bass = mtbBassLength.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void nubMultiScaleRatio_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && EditMode == ScaleLengthType.Multiple)
            {
                CurrentLayout.MultiScaleConfig.PerpendicularFretRatio = nubMultiScaleRatio.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        #endregion

        #region Manual Mode

        private void dgvScaleLengths_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && e.RowIndex >= 0)
            {
                CurrentLayout.RebuildLayout();
            }
        }

        private void dgvScaleLengths_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control != null && e.Control is TextBox)
            {
                if (dgvScaleLengths.CurrentCell.ColumnIndex == colScaleLength.Index)
                {
                    var length = (Measure)dgvScaleLengths.CurrentCell.Value;
                    (e.Control as TextBox).Text = length.ToString(new Measure.MeasureFormat()
                    {
                        ShowFractions = false,
                        AllowApproximation = false
                    });
                }
            }
        }

        private void dgvScaleLengths_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == colScaleLength.Index)
            {

            }
            else if (e.ColumnIndex == colMultiScaleRatio.Index)
            {
                double ratio = 0;
                if (!double.TryParse((string)e.FormattedValue, out ratio) || ratio < 0 || ratio > 1)
                {

                    //dgvScaleLengths[e.ColumnIndex, e.RowIndex]
                    e.Cancel = true;

                }
            }
        }


        #endregion
    }
}
