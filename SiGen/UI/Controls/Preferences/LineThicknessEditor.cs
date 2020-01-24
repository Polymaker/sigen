using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Export;
using SiGen.Measuring;
using System.Windows.Forms.Design;
using System.Collections;
using SiGen.Utilities;
using System.Windows.Forms.Design.Behavior;

namespace SiGen.UI.Controls.ValueEditors
{
    [Designer(typeof(LineThicknessEditorDesigner))]
    [DefaultEvent("ThicknessChanged")]
    public partial class LineThicknessEditor : UserControl
    {
        private LineUnit _SelectedUnit;
        private double _SelectedThickness;
        private bool IsLoading;
        private bool SettingValue;

        [DefaultValue(LineUnit.Points)]
        public LineUnit SelectedUnit
        {
            get => _SelectedUnit;
            set => SetSelectedUnit(value);
        }

        [DefaultValue(1)]
        public double SelectedThickness
        {
            get => _SelectedThickness;
            set => SetSelectedThickness(value);
        }

        public event EventHandler ThicknessChanged;

        public LineThicknessEditor()
        {
            InitializeComponent();
            _SelectedUnit = LineUnit.Points;
            _SelectedThickness = 1;
            IsLoading = true;
        }

        

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PositionControls();

            IsLoading = true;

            cboUnitType.Items.Add(ExportUnitPoints.Text);
            cboUnitType.Items.Add(ExportUnitPixels.Text);
            cboUnitType.Items.Add(ExportUnitMeasure.Text);

            txtNumber.Value = _SelectedThickness;
            txtMeasure.Value = Measure.Mm(1);

            SetSelectedUnit();
            UpdateEditorsVisibility();

            IsLoading = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            PositionControls();
        }

        private void PositionControls()
        {
            txtMeasure.Left = cboUnitType.Right + 2;
            txtMeasure.Width = Width - txtMeasure.Left;
            txtNumber.Left = cboUnitType.Right + 2;
            txtNumber.Width = Width - txtNumber.Left;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (cboUnitType.IsHandleCreated)
                height = Math.Max(cboUnitType.Height, txtMeasure.Height);
            
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            PositionControls();
        }

        public void SetValues(double thickness, LineUnit unit)
        {
            SetSelectedUnit(unit, false);
            SetSelectedThickness(thickness, null, false);
            OnThicknessChanged(EventArgs.Empty);
        }

        private void SetSelectedUnit(LineUnit unit, bool raiseEvent = true)
        {
            if (unit != _SelectedUnit)
            {
                _SelectedUnit = unit;

                if (cboUnitType.IsHandleCreated && txtMeasure.IsHandleCreated)
                {
                    SettingValue = true;
                    if (unit == LineUnit.Points)
                        cboUnitType.SelectedIndex = 0;
                    else if (unit == LineUnit.Pixels)
                        cboUnitType.SelectedIndex = 1;
                    else
                    {
                        cboUnitType.SelectedIndex = 2;

                        double currentValue = txtMeasure.Value.Value.DoubleValue;
                        if (unit == LineUnit.Millimeters)
                            txtMeasure.Value = Measure.Mm(currentValue);
                        else
                            txtMeasure.Value = Measure.Inches(currentValue);
                    }
                    SettingValue = false;

                    UpdateEditorsVisibility();
                }
                

                if (raiseEvent)
                    OnThicknessChanged(EventArgs.Empty);
            }
        }

        private void SetSelectedThickness(double value, object sender = null, bool raiseEvent = true)
        {
            if (value != _SelectedThickness)
            {
                _SelectedThickness = value;

                if (txtNumber.IsHandleCreated && txtMeasure.IsHandleCreated)
                {
                    SettingValue = true;

                    if (sender != txtNumber)
                        txtNumber.Value = value;

                    if (sender != txtMeasure)
                        txtMeasure.Value = new Measure(value, txtMeasure.Value.Unit);

                    SettingValue = false;
                }
                
                if (raiseEvent)
                    OnThicknessChanged(EventArgs.Empty);
            }
        }

        private void UpdateEditorsVisibility()
        {
            bool isMesureUnit = SelectedUnit == LineUnit.Millimeters || SelectedUnit == LineUnit.Inches;
            txtMeasure.Visible = isMesureUnit;
            txtNumber.Visible = !isMesureUnit;

            txtNumber.AllowDecimals = SelectedUnit == LineUnit.Points;
        }

        private void SetSelectedUnit()
        {
            switch (SelectedUnit)
            {
                case LineUnit.Points:
                    cboUnitType.SelectedIndex = 0;
                    break;
                case LineUnit.Pixels:
                    cboUnitType.SelectedIndex = 1;
                    break;
                default:
                    cboUnitType.SelectedIndex = 2;
                    break;
            }
        }

        private LineUnit GetSelectedUnit()
        {
            if (cboUnitType.SelectedIndex == 0)
                return LineUnit.Points;
            else if (cboUnitType.SelectedIndex == 1)
                return LineUnit.Pixels;
            else
            {
                if (txtMeasure.Value.Unit.IsMetric)
                    return LineUnit.Millimeters;
                else
                    return LineUnit.Inches;
            }
        }

        private void CboUnitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cboUnitType.SelectedItem != null )
            {
                if (!IsLoading && !SettingValue)
                    _SelectedUnit = GetSelectedUnit();

                UpdateEditorsVisibility();

                if (!IsLoading && !SettingValue)
                    OnThicknessChanged(EventArgs.Empty);
            }
        }

        private void TxtMeasure_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !SettingValue)
            {
                double baseThickness = 1d;
                if (txtMeasure.Value.Unit.IsMetric)
                    baseThickness = txtMeasure.Value[UnitOfMeasure.Mm].DoubleValue;
                else
                    baseThickness = txtMeasure.Value[UnitOfMeasure.In].DoubleValue;
                SetSelectedThickness(baseThickness, txtMeasure);
            }
        }

        private void TxtNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !SettingValue)
                SetSelectedThickness(txtNumber.Value, txtNumber);
        }

        protected virtual void OnThicknessChanged(EventArgs e)
        {
            ThicknessChanged?.Invoke(this, e);
        }

        internal TextBox GetTextBox()
        {
            return txtNumber;
        }
    }

    class LineThicknessEditorDesigner : ControlDesigner
    {

        public override IList SnapLines
        {
            get
            {
                ArrayList arrayList = base.SnapLines as ArrayList;

                var editor = Control as LineThicknessEditor;
                var editorTxt = editor.GetTextBox();

                int textBaseline = ControlHelper.GetTextBaseline(editorTxt ?? Control, ContentAlignment.TopLeft);
                BorderStyle borderStyle = editorTxt?.BorderStyle ?? BorderStyle.Fixed3D;

                switch (borderStyle)
                {
                    case BorderStyle.None:
                        break;
                    case BorderStyle.FixedSingle:
                        textBaseline += 2;
                        break;
                    case BorderStyle.Fixed3D:
                        textBaseline += 3;
                        break;
                }
                arrayList.Add(new SnapLine(SnapLineType.Baseline, textBaseline, SnapLinePriority.Medium));
                return arrayList;
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                var baseRules = base.SelectionRules;
                baseRules = baseRules & (~SelectionRules.BottomSizeable);
                baseRules = baseRules & (~SelectionRules.TopSizeable);
                return baseRules;
            }
        }
    }
}
