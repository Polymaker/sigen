using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using SiGen.Measuring;

namespace SiGen.UI
{
    [DefaultEvent("ValueChanged")]
    public partial class MeasureEdit : UserControl
    {
        private bool internalChange;
        private Measure _Value;
        private UnitOfMeasure _SelectedUnit;

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public UnitOfMeasure SelectedUnit
        {
            get { return _SelectedUnit; }
            set
            {
                if (value != _SelectedUnit)
                    SetSelectedUnit(value);
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Measure Value
        {
            get { return _Value; }
            set
            {
                if (value != _Value)
                    SetValue(value);
            }
        }

        public event EventHandler ValueChanged;

        public MeasureEdit()
        {
            InitializeComponent();
            _Value = Measure.Mm(0);
            _SelectedUnit = _Value.Unit;
            internalChange = true;

            var unitList = new List<UnitOfMeasure>();
            _SelectedUnit = UnitOfMeasure.Mm;
            unitList.Add(UnitOfMeasure.Mm);
            unitList.Add(UnitOfMeasure.Cm);
            unitList.Add(UnitOfMeasure.Inches);
            unitList.Add(UnitOfMeasure.Feets);
            cboUnitType.DataSource = unitList;
            cboUnitType.DisplayMember = "Symbol";
            nudValue.MinimumSize = new Size(0, cboUnitType.Height);
            internalChange = false;
        }

        private void nudValue_ValueChanged(object sender, EventArgs e)
        {
            var valueStr = nudValue.Value.ToString();
            var decimalSep = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;
            if (valueStr.Contains(decimalSep))
                nudValue.DecimalPlaces = Math.Min(valueStr.Length - (valueStr.IndexOf(decimalSep) + 1), 5);
            else
                nudValue.DecimalPlaces = 0;

            if (!internalChange)
                SetValue(new Measure((double)nudValue.Value, SelectedUnit));
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
                height = cboUnitType.Height;
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void cboUnitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!internalChange && cboUnitType.SelectedItem != null)
                SetSelectedUnit((UnitOfMeasure)cboUnitType.SelectedItem);
        }

        private void SetSelectedUnit(UnitOfMeasure value, bool convertValue = true)
        {
            internalChange = true;

            _SelectedUnit = value;
            cboUnitType.SelectedItem = value;

            if (convertValue)
                SetValue(new Measure(Value[_SelectedUnit], _SelectedUnit));
            else
                SetValue(new Measure(Value.Value, _SelectedUnit));
            internalChange = false;
        }

        private void SetValue(Measure value)
        {
            internalChange = true;

            _Value = value;
            if (_SelectedUnit != value.Unit)
            {
                _SelectedUnit = value.Unit;
                cboUnitType.SelectedItem = value.Unit;
            }
            nudValue.Value = (decimal)value.Value;
            internalChange = false;
            OnValueChanged();
        }

        private void OnValueChanged()
        {
            ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
