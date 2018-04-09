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
using SiGen.Measuring;
using SiGen.Utilities;

namespace SiGen.UI.Controls
{
    public partial class ScaleLengthEditor : LayoutPropertyEditor
    {
        private ScaleLengthType EditMode;
        private List<double> _FretPositions;

        public ScaleLengthEditor()
        {
            InitializeComponent();
            _FretPositions = new List<double>();
            dgvScaleLengths.AutoGenerateColumns = false;
        }

        private void FetchFretPositions()
        {
            int fretNum = CurrentLayout != null ? CurrentLayout.Strings.Max(s => s.NumberOfFrets) : 24;
            if (fretNum + 2 != _FretPositions.Count)
            {
                _FretPositions.Clear();
                _FretPositions.Add(0);
                for (int i = 1; i <= fretNum; i++)
                    _FretPositions.Add(1d - SILayout.GetEqualTemperedFretPosition(i));
                _FretPositions.Add(1d);
            }
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
            {
                var currentList = dgvScaleLengths.DataSource as SortableBindingList<SIString>;
                if (currentList == null)
                    dgvScaleLengths.DataSource = new SortableBindingList<SIString>(CurrentLayout.Strings);
                else
                {
                    currentList.RemoveAll(s => !CurrentLayout.Strings.Contains(s));
                    if (dgvScaleLengths.SortedColumn == colStringNumber && dgvScaleLengths.SortOrder == SortOrder.Descending)
                    {
                        currentList.InsertRange(0, CurrentLayout.Strings.Where(s => !currentList.Contains(s)).OrderByDescending(s => s.Index));
                    }
                    else
                        currentList.AddRange(CurrentLayout.Strings.Where(s => !currentList.Contains(s)));
                }
            }
        }

        protected override void ReadLayoutProperties()
        {
            if (dgvScaleLengths.IsCurrentCellInEditMode)
                dgvScaleLengths.CancelEdit();

            dgvScaleLengths.DataSource = null;
            
            if (CurrentLayout != null)
            {
                EditMode = CurrentLayout.ScaleLengthMode;
                dgvScaleLengths.DataSource = new SortableBindingList<SIString>(CurrentLayout.Strings);
                dgvScaleLengths.Sort(colStringNumber, ListSortDirection.Descending);
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
            lblPerpFret.Visible = (EditMode == ScaleLengthType.Multiple);
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
                        }
                        break;
                    case ScaleLengthType.Individual:
                        dgvScaleLengths.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        break;
                }

                int totalHeight = tableLayoutPanel1.Height;
                if (dgvScaleLengths.Visible)
                    totalHeight += dgvScaleLengths.MinimumSize.Height;
                AutoScrollMinSize = new Size(AutoScrollMinSize.Width, totalHeight);
            }
            else
            {
                mtbTrebleLength.AllowEmptyValue = true;
                mtbTrebleLength.Value = Measure.Empty;
                tableLayoutPanel1.MinimumSize = Size.Empty;
                AutoScrollMinSize = Size.Empty;
            }
        }

        #region Scale Length Mode Management

        private void rbScaleLengthMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !FlagManager["SetMode"] && (sender as RadioButton).Checked)
            {
                if (enteringControl)//fallback to prevent windows to force check the last clicked radiobutton
                {
                    SetSelectedEditMode(EditMode);
                    return;
                }

                EditMode = GetSelectedEditMode();
                if (CurrentLayout != null)
                {
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

        #endregion

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
            DetermineFretAlignment();
        }

        #endregion

        private void DetermineFretAlignment()
        {
            if (nubMultiScaleRatio.Value == 0)
            {
                lblPerpFret.Text = "Perpendicular at Nut";
            }
            else if (nubMultiScaleRatio.Value == 1)
            {
                lblPerpFret.Text = "Perpendicular at Bridge";
            }
            else if (_FretPositions.Any(p => p.EqualOrClose(nubMultiScaleRatio.Value, 0.0005)))
            {
                var closest = _FretPositions.First(p => p.EqualOrClose(nubMultiScaleRatio.Value, 0.0005));
                int fretIndex = _FretPositions.IndexOf(closest);
                lblPerpFret.Text = string.Format("Perpendicular at {0} fret", fretIndex);
            }
            else
            {
                lblPerpFret.Text = "Custom alignement";
            }
        }

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
                    e.Cancel = true;
            }
        }

        private void dgvScaleLengths_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e.ColumnIndex == colScaleLength.Index)
            {
                var rowStr = (SIString)dgvScaleLengths.Rows[e.RowIndex].DataBoundItem;
                Measure newValue;
                if (MeasureParser.TryParse((string)e.Value, out newValue, rowStr.ScaleLength.Unit))
                {
                    e.Value = newValue;
                    e.ParsingApplied = true;
                }
            }
        }

        #endregion

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

        protected override Point ScrollToControl(Control activeControl)
        {
            if (activeControl == dgvScaleLengths)
                return DisplayRectangle.Location;
            return base.ScrollToControl(activeControl);
        }
    }
}
