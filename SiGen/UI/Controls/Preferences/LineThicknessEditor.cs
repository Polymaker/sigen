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

namespace SiGen.UI.Controls.ValueEditors
{
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

        public event EventHandler ConfigurationChanged;

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

            IsLoading = true;

            cboUnitType.Items.Add(ExportUnitPoints.Text);
            cboUnitType.Items.Add(ExportUnitPixels.Text);
            cboUnitType.Items.Add(ExportUnitMeasure.Text);

            txtNumber.Value = _SelectedThickness;
            txtMeasure.Value = Measure.Mm(1);

            cboUnitType.SelectedValue = _SelectedUnit;

            IsLoading = false;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (cboUnitType.IsHandleCreated)
                height = Math.Max(cboUnitType.Height, txtMeasure.Height);
            base.SetBoundsCore(x, y, width, height, specified);
        }

        public void SetValues(double thickness, LineUnit unit)
        {
            SetSelectedUnit(unit, false);
            SetSelectedThickness(thickness, null, false);
            OnConfigurationChanged(this, EventArgs.Empty);
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
                    OnConfigurationChanged(this, EventArgs.Empty);
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
                    OnConfigurationChanged(this, EventArgs.Empty);
            }
        }

        private void UpdateEditorsVisibility()
        {
            bool isMesureUnit = SelectedUnit == LineUnit.Millimeters || SelectedUnit == LineUnit.Inches;
            txtMeasure.Visible = isMesureUnit;
            txtNumber.Visible = !isMesureUnit;

            txtNumber.AllowDecimals = SelectedUnit == LineUnit.Points;
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
                    OnConfigurationChanged(this, EventArgs.Empty);
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

        protected virtual void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(sender, e);
        }
    }
}
