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
        private List<FretPosition> FretPositions;

        class FretPosition
        {
            public int FretNumber { get; set; }
            public double PositionRatio { get; set; }
            public string Name { get; set; }
        }

        public ScaleLengthEditor()
        {
            InitializeComponent();
            FretPositions = new List<FretPosition>();
            dgvScaleLengths.AutoGenerateColumns = false;
        }

        private void FetchFretPositions()
        {
            int fretNum = CurrentLayout != null ? CurrentLayout.Strings.Max(s => s.NumberOfFrets) : 24;

            if (fretNum + 2 != FretPositions.Count)
            {
                cboParallelFret.DataSource = null;
                FretPositions.Clear();
                FretPositions.Add(new FretPosition()
                {
                    FretNumber = 0,
                    Name = "Nut",
                    PositionRatio = 0
                });
                
                for (int i = 1; i <= fretNum; i++)
                {
                    var ratio = 1d - SILayout.GetEqualTemperedFretPosition(i);
                    FretPositions.Add(new FretPosition()
                    {
                        FretNumber = i,
                        Name = $"{i}{i.GetSuffix()} Fret",
                        PositionRatio = ratio
                    });
                }

                FretPositions.Add(new FretPosition()
                {
                    FretNumber = FretPositions.Count,
                    Name = "Bridge",
                    PositionRatio = 1
                });

                //FretPositions.Add(new FretPosition()
                //{
                //    FretNumber = -1,
                //    Name = "Custom",
                //    PositionRatio = -1
                //});

                cboParallelFret.DisplayMember = "Name";
                cboParallelFret.ValueMember = "FretNumber";
                cboParallelFret.DataSource = FretPositions;
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

            SetControlsVisibility(EditMode == ScaleLengthType.Multiple, lblBass, lblMultiScaleRatio, lblParallelFret, mtbBassLength, nubMultiScaleRatio, cboParallelFret);

            lblTreble.Visible = (EditMode != ScaleLengthType.Individual);
            mtbTrebleLength.Visible = (EditMode != ScaleLengthType.Individual);
            dgvScaleLengths.Visible = (EditMode == ScaleLengthType.Individual);
            
            SetSelectedEditMode(EditMode);

            if (CurrentLayout != null)
            {
                switch (EditMode)
                {
                    case ScaleLengthType.Single:
                        mtbTrebleLength.Value = CurrentLayout.SingleScaleConfig.Length;
                        mtbTrebleLength.AllowEmptyValue = false;
                        lblTreble.Text = "Length";
                        break;
                    case ScaleLengthType.Multiple:
                        {
                            mtbTrebleLength.Value = CurrentLayout.MultiScaleConfig.Treble;
                            mtbTrebleLength.AllowEmptyValue = false;
                            mtbBassLength.Value = CurrentLayout.MultiScaleConfig.Bass;
                            nubMultiScaleRatio.Value = CurrentLayout.MultiScaleConfig.PerpendicularFretRatio;
                            SelectClosestFretPosition(CurrentLayout.MultiScaleConfig.PerpendicularFretRatio);
                            lblTreble.Text = "Treble";
                        }
                        break;
                    case ScaleLengthType.Individual:
                        dgvScaleLengths.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                        break;
                }

                int totalHeight = tlpLayout.Height;
                if (dgvScaleLengths.Visible)
                    totalHeight += dgvScaleLengths.MinimumSize.Height;
                AutoScrollMinSize = new Size(AutoScrollMinSize.Width, totalHeight);
            }
            else
            {
                mtbTrebleLength.AllowEmptyValue = true;
                mtbTrebleLength.Value = Measure.Empty;
                tlpLayout.MinimumSize = Size.Empty;
                AutoScrollMinSize = Size.Empty;
            }
        }

        #region Scale Length Mode Management

        private void rbScaleLengthMode_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !FlagManager["SetMode"] && (sender as RadioButton).Checked)
            {
                if (enteringControl)//fallback to prevent Windows to force check the last clicked radiobutton
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
            if (!IsLoading && CurrentLayout != null && EditMode == ScaleLengthType.Multiple && !FlagManager["AdjustPositionRatio"])
            {
                var closestFret = SelectClosestFretPosition(nubMultiScaleRatio.Value);

                if(closestFret != null)
                {
                    using (FlagManager.UseFlag("DetermineFretAlignment"))
                        nubMultiScaleRatio.Value = closestFret.PositionRatio;// Math.Round(closestFret.PositionRatio, 4);
                }

                CurrentLayout.MultiScaleConfig.PerpendicularFretRatio = nubMultiScaleRatio.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void cboParallelFret_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null && EditMode == ScaleLengthType.Multiple && !FlagManager["DetermineFretAlignment"])
            {
                if(cboParallelFret.SelectedItem != null)
                {
                    var selectedPosition = (FretPosition)cboParallelFret.SelectedItem;
                    nubMultiScaleRatio.Value = selectedPosition.PositionRatio;// Math.Round(selectedPosition.PositionRatio, 4);
                }
            }
        }

        #endregion


        private FretPosition SelectClosestFretPosition(double fretRation, double tolerance = 0.0005)
        {
            var closestFretPos = FretPositions.FirstOrDefault(p => p.PositionRatio.EqualOrClose(nubMultiScaleRatio.Value, tolerance));
            using (FlagManager.UseFlag("DetermineFretAlignment"))
            {
                if (closestFretPos != null)
                    cboParallelFret.SelectedValue = closestFretPos.FretNumber;
                else
                    cboParallelFret.SelectedItem = null;
            }
            return closestFretPos;
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

        private void cboParallelFret_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            using (var textBrush = new SolidBrush(e.ForeColor))
            using(var sf = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                if (e.Index >= 0)
                {
                    var fretPos = FretPositions[e.Index];
                    var text = fretPos.Name;
                    e.Graphics.DrawString(text, e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                }
                else
                {
                    e.Graphics.DrawString("Custom", e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                }
            }
        }
    }
}
