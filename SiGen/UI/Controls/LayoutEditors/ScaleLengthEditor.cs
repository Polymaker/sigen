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
using SiGen.Utilities;
using SiGen.Resources;

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
                    Name = Localizations.FingerboardEnd_Nut,
                    PositionRatio = 0
                });
                
                for (int i = 1; i <= fretNum; i++)
                {
                    var ratio = 1d - SILayout.GetEqualTemperedFretPosition(i);
                    FretPositions.Add(new FretPosition()
                    {
                        FretNumber = i,
                        Name = $"{i}{i.GetSuffix()} {Localizations.Words_Fret}",
                        PositionRatio = ratio
                    });
                }

                FretPositions.Add(new FretPosition()
                {
                    FretNumber = FretPositions.Count,
                    Name = Localizations.FingerboardEnd_Bridge,
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

                CalculateMaxCboItemWidth();
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
                        lblTreble.Text = Localizations.Words_Length;
                        break;
                    case ScaleLengthType.Multiple:
                        {
                            mtbTrebleLength.Value = CurrentLayout.MultiScaleConfig.Treble;
                            mtbTrebleLength.AllowEmptyValue = false;
                            mtbBassLength.Value = CurrentLayout.MultiScaleConfig.Bass;
                            nubMultiScaleRatio.Value = CurrentLayout.MultiScaleConfig.PerpendicularFretRatio;
                            SelectClosestFretPosition(CurrentLayout.MultiScaleConfig.PerpendicularFretRatio);
                            lblTreble.Text = Localizations.FingerboardSide_Treble;
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


        private FretPosition SelectClosestFretPosition(double fretRatio, double tolerance = 0.0005)
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

		public string GetPerpendicularFretName()
		{
			if(EditMode == ScaleLengthType.Multiple)
			{
				var current = cboParallelFret.SelectedItem as FretPosition;
				return current?.Name ?? Localizations.Words_CustomRatio;
			}
			return string.Empty;
		}

		public string GetPerpendicularFretName(double fretRatio, double tolerance = 0.0005)
		{
			var closestFretPos = FretPositions.FirstOrDefault(p => p.PositionRatio.EqualOrClose(fretRatio, tolerance));
			return closestFretPos?.Name ?? Localizations.Words_CustomRatio;
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
            if (e.ColumnIndex == colMultiScaleRatio.Index)
            {
                if (!double.TryParse((string)e.FormattedValue, out double ratio)
                    || ratio < 0 || ratio > 1)
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

        private void dgvScaleLengths_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == colStringNumber.Index)
            {
                var rowStr = (SIString)dgvScaleLengths.Rows[e.RowIndex].DataBoundItem;
                e.Value = rowStr.Index + 1;
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

        private int MaxCboItemWidth = 0;

        private void CalculateMaxCboItemWidth()
        {
            MaxCboItemWidth = 0;

            using (var g = cboParallelFret.CreateGraphics())
            {
                MaxCboItemWidth = (int)g.MeasureString(Localizations.Words_CustomRatio, cboParallelFret.Font).Width;

                foreach (FretPosition item in cboParallelFret.Items)
                {
                    var txtSize = g.MeasureString(item.Name, cboParallelFret.Font);
                    if (txtSize.Width > MaxCboItemWidth)
                        MaxCboItemWidth = (int)txtSize.Width;
                }
            }
        }

        private void cboParallelFret_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();

            using (var textBrush = new SolidBrush(e.ForeColor))
            using (var sf = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
            {
                var textBound = new Rectangle(e.Bounds.X, e.Bounds.Y, MaxCboItemWidth + 3, e.Bounds.Height);
                var valueBound = new Rectangle(e.Bounds.Right - 30, e.Bounds.Y, 30, e.Bounds.Height);
                bool isHovered = e.State.HasFlag(DrawItemState.Selected) || e.State.HasFlag(DrawItemState.HotLight);
                if (e.Index >= 0)
                {
                    var fretPos = FretPositions[e.Index];
                    var text = fretPos.Name;

                    sf.Alignment = fretPos.PositionRatio % 1 == 0 ? StringAlignment.Near : StringAlignment.Far;
                    e.Graphics.DrawString(text, e.Font, new SolidBrush(e.ForeColor), textBound, sf);
                    
                    if (!e.State.HasFlag(DrawItemState.ComboBoxEdit))
                    {
                        sf.Alignment = StringAlignment.Center;
                        if ((fretPos.PositionRatio * 2) % 1 == 0)
                        {
                            e.Graphics.DrawString(fretPos.PositionRatio.ToString(), e.Font, new SolidBrush(isHovered? e.ForeColor : Color.DarkSlateGray), valueBound, sf);
                        }
                        else
                        {
                            e.Graphics.DrawLine(Pens.Gray, valueBound.Left + (valueBound.Width / 2) - 1, e.Bounds.Top, valueBound.Left + (valueBound.Width / 2) - 1, e.Bounds.Bottom);
                        }
                        
                        sf.Alignment = StringAlignment.Near;
                    }
                    
                }
                else
                {
                    e.Graphics.DrawString(Localizations.Words_CustomRatio, e.Font, new SolidBrush(e.ForeColor), e.Bounds, sf);
                }
            }
        }

        
    }
}
