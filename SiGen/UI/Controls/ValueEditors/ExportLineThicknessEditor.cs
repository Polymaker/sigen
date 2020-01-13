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
    public partial class ExportLineThicknessEditor : UserControl
    {
        private ExportUnit _SelectedUnit;
        private double _SelectedThickness;
        private bool IsLoading;
        private bool SettingValue;

        [DefaultValue(ExportUnit.Points)]
        public ExportUnit SelectedUnit
        {
            get
            {
                return _SelectedUnit;
            }
            set
            {
                if (cboUnitType.IsHandleCreated && cboUnitType.SelectedItem != null && !string.IsNullOrEmpty(cboUnitType.ValueMember))
                    cboUnitType.SelectedValue = value;
                else
                    _SelectedUnit = value;
            }
        }

        [DefaultValue(1)]
        public double SelectedThickness
        {
            get => _SelectedThickness;
            set => SetSelectedThickness(value);
        }

        public event EventHandler ConfigurationChanged;

        public ExportLineThicknessEditor()
        {
            InitializeComponent();
            _SelectedUnit = ExportUnit.Points;
            _SelectedThickness = 1;
            IsLoading = true;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            IsLoading = true;

            var unitList = new List<EnumHelper.EnumItem>
            {
                new EnumHelper.EnumItem(ExportUnit.Pixels, ExportUnitPixels),
                new EnumHelper.EnumItem(ExportUnit.Points, ExportUnitPoints),
                new EnumHelper.EnumItem(ExportUnit.Measure, ExportUnitMeasure)
            };

            cboUnitType.ValueMember = EnumHelper.EnumItem.ValueMember;
            cboUnitType.DisplayMember = EnumHelper.EnumItem.DisplayMember;
            cboUnitType.DataSource = unitList.ToList();

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

        private void CboUnitType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //var oldType = _SelectedUnit;

            if (!IsLoading && cboUnitType.SelectedItem != null && !string.IsNullOrEmpty(cboUnitType.ValueMember))
            {
                _SelectedUnit = (ExportUnit)cboUnitType.SelectedValue;
            }

            if (cboUnitType.SelectedItem != null)
            {
                txtMeasure.Visible = SelectedUnit == ExportUnit.Measure;
                txtNumber.Visible = SelectedUnit != ExportUnit.Measure;

                txtNumber.AllowDecimals = SelectedUnit == ExportUnit.Points;

                if (!IsLoading)
                    OnConfigurationChanged(this, EventArgs.Empty);
            }
        }

        private void TxtMeasure_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !SettingValue)
                SetSelectedThickness((double)txtMeasure.Value.NormalizedValue);
        }

        private void TxtNumber_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && !SettingValue)
                SetSelectedThickness(txtNumber.Value);
        }

        private void SetSelectedThickness(double value)
        {
            if (value != _SelectedThickness)
            {
                SettingValue = true;
                _SelectedThickness = value;
                txtNumber.Value = value;
                txtMeasure.Value = Measure.FromNormalizedValue(value, txtMeasure.Value.Unit);

                SettingValue = false;
                OnConfigurationChanged(this, EventArgs.Empty);
            }
        }

        protected virtual void OnConfigurationChanged(object sender, EventArgs e)
        {
            ConfigurationChanged?.Invoke(sender, e);
        }
    }
}
