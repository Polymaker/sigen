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

namespace SiGen.UI.Controls.LayoutEditors
{
    public partial class StringsConfigurationEditor : LayoutPropertyEditor
    {
        public StringsConfigurationEditor()
        {
            InitializeComponent();
        }

        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            nbxNumberOfStrings.Enabled = (CurrentLayout != null);
            nbxNumberOfFrets.Enabled = (CurrentLayout != null);
            chkLeftHanded.Enabled = (CurrentLayout != null);
            chkShowAdvanced.Enabled = (CurrentLayout != null);
            chkShowAdvanced.Checked = false;

            if (CurrentLayout != null)
            {
                nbxNumberOfStrings.Value = CurrentLayout.NumberOfStrings;
                nbxNumberOfFrets.Value = CurrentLayout.MaximumFret;
                chkLeftHanded.Checked = CurrentLayout.LeftHanded;
            }
            else
            {
                nbxNumberOfStrings.Value = 6;
                nbxNumberOfFrets.Value = 24;
                chkLeftHanded.Checked = false;
            }

            FillPivotGrid();
        }

        protected override void OnLayoutUpdated()
        {
            base.OnLayoutUpdated();
            UpdateFieldsVisibility();
            ApplyFieldsVisibility();
            UpdateGridValues();
        }

        protected override void OnNumberOfStringsChanged()
        {
            base.OnNumberOfStringsChanged();
            FillPivotGrid();
        }

        protected override void CacheCurrentLayoutValues()
        {
            base.CacheCurrentLayoutValues();
            CachedLayoutData[CurrentLayout]["ShowAdvanced"] = chkShowAdvanced.Checked;
        }

        protected override void RestoreCachedLayoutValues()
        {
            base.RestoreCachedLayoutValues();
            chkShowAdvanced.Checked = (bool)CachedLayoutData[CurrentLayout]["ShowAdvanced"];
        }

        private void nbxNumberOfStrings_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.NumberOfStrings = (int)nbxNumberOfStrings.Value;
                CurrentLayout.RebuildLayout();
            }
        }

        private void nbxNumberOfFrets_ValueChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.Strings.SetAll(s => s.NumberOfFrets, (int)nbxNumberOfFrets.Value);
                CurrentLayout.RebuildLayout();
            }
        }

        private void chkLeftHanded_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                CurrentLayout.LeftHanded = chkLeftHanded.Checked;
                CurrentLayout.RebuildLayout();
            }
        }

        #region Pivot DataGridView

        private class PivotField
        {
            public string HeaderText { get; set; }
            public string PropertyName { get; set; }
            public bool Visible { get; set; }
            public bool ReadOnly { get; set; }
            public PivotField(string propName, string display)
            {
                PropertyName = propName;
                HeaderText = display;
                Visible = true;
            }
        }

        private List<PivotField> PivotFields;
        private bool LoadingGridValues;
        private PropertyDescriptorCollection StringProperties;
        private PropertyDescriptorCollection PhysicalProperties;

        private void FillPivotGrid()
        {
            if (PivotFields == null)
                InitializePivotFields();

            if(CurrentLayout == null)
            {
                //dgvStrings.DataSource = null;
                dgvStrings.Columns.Clear();
                dgvStrings.Rows.Clear();
            }
            else
            {
                var currentCell = dgvStrings.CurrentCellAddress;

                dgvStrings.SuspendLayout();
                dgvStrings.Columns.Clear();
                dgvStrings.Rows.Clear();

                for (int i = 0; i < CurrentLayout.NumberOfStrings; i++)
                {
                    var stringCol = new DataGridViewTextBoxColumn()
                    {
                        HeaderText = (i + 1).ToString(),
                        SortMode = DataGridViewColumnSortMode.NotSortable,
                        Tag = i
                    };
                    dgvStrings.Columns.Add(stringCol);
                }

                UpdateFieldsVisibility();

                foreach (var field in PivotFields)
                {
                    var fieldRow = new DataGridViewRow()
                    {
                        Tag = field,
                    };
                    fieldRow.ReadOnly = field.ReadOnly;
                    fieldRow.Visible = field.Visible;
                    dgvStrings.Rows.Add(fieldRow);
                }
                dgvStrings.ResumeLayout(true);
                UpdateGridValues();
                dgvStrings.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            }
        }

        private void InitializePivotFields()
        {
            StringProperties = TypeDescriptor.GetProperties(typeof(SIString));
            PhysicalProperties = TypeDescriptor.GetProperties(typeof(StringedInstruments.Data.StringProperties));

            PivotFields = new List<PivotField>();
            PivotFields.Add(new PivotField("StartingFret", "Starting Fret"));
            PivotFields.Add(new PivotField("NumberOfFrets", "Number Of Frets"));
            PivotFields.Add(new PivotField("ScaleLength", "Scale Length") { Visible = false });
            PivotFields.Add(new PivotField("MultiScaleRatio", "Align. Ratio") { Visible = false });
            PivotFields.Add(new PivotField("Gauge", "Gauge"));
            PivotFields.Add(new PivotField("PhysicalProperties.CoreWireDiameter", "Core Diam.") { Visible = false });
            PivotFields.Add(new PivotField("PhysicalProperties.UnitWeight", "Unit Weight") { Visible = false });
            PivotFields.Add(new PivotField("PhysicalProperties.ModulusOfElasticity", "Elast. Modulus") { Visible = false });
        }

        public void UpdateFieldsVisibility()
        {
            if(CurrentLayout != null)
            {
                foreach (var field in PivotFields)
                {
                    if (field.PropertyName == "ScaleLength" || field.PropertyName == "MultiScaleRatio")
                        field.Visible = (CurrentLayout.ScaleLengthMode == ScaleLengthType.Individual);
                    else if (field.PropertyName.Contains("PhysicalProperties"))
                        field.Visible = chkShowAdvanced.Checked;
                }
            }
        }

        public void ApplyFieldsVisibility()
        {
            bool anyChange = false;
            foreach (DataGridViewRow row in dgvStrings.Rows)
            {
                var field = (PivotField)row.Tag;
                if (row.Visible != field.Visible)
                {
                    if (!field.Visible && dgvStrings.CurrentCell != null && dgvStrings.CurrentCell.RowIndex == row.Index)
                        dgvStrings.CurrentCell = null;
                    row.Visible = field.Visible;
                    anyChange = true;
                }
                row.ReadOnly = field.ReadOnly;
            }
            if (anyChange)
                dgvStrings.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }

        private void UpdateGridValues()
        {
            LoadingGridValues = true;
            if(CurrentLayout != null)
            {
                foreach(DataGridViewRow row in dgvStrings.Rows)
                {
                    var field = (PivotField)row.Tag;
                    for (int i = 0; i < CurrentLayout.NumberOfStrings; i++)
                    {
                        
                        row.Cells[i].Value = GetStringValue(i, field.PropertyName);
                        row.Cells[i].ValueType = GetValueType(field.PropertyName);
                    }
                }
            }
            LoadingGridValues = false;
        }

        private object GetStringValue(int stringIndex, string propertyName)
        {
            if (!propertyName.Contains("."))
            {
                return StringProperties[propertyName].GetValue(CurrentLayout.Strings[stringIndex]);
            }
            else
            {
                string[] names = propertyName.Split('.');
                var baseObj = StringProperties[names[0]].GetValue(CurrentLayout.Strings[stringIndex]);
                if(baseObj != null)
                {
                    if(names[0] == "PhysicalProperties")
                        return PhysicalProperties[names[1]].GetValue(baseObj);
                }
            }
            return null;
        }

        private Type GetValueType(string propertyName)
        {
            if (!propertyName.Contains("."))
            {
                return StringProperties[propertyName].PropertyType;
            }
            else
            {
                string[] names = propertyName.Split('.');
                if (names[0] == "PhysicalProperties")
                    return PhysicalProperties[names[1]].PropertyType;
            }
            return null;
        }

        private void SetStringValue(int stringIndex, string propertyName, object value)
        {
            if (!propertyName.Contains("."))
            {

                StringProperties[propertyName].SetValue(CurrentLayout.Strings[stringIndex], value);
            }
            else
            {
                string[] names = propertyName.Split('.');
                var baseObj = StringProperties[names[0]].GetValue(CurrentLayout.Strings[stringIndex]);
                if (baseObj != null)
                {
                    if (names[0] == "PhysicalProperties")
                        PhysicalProperties[names[1]].SetValue(baseObj, value);
                }
            }
        }

        private void dgvStrings_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 && e.RowIndex >= 0)
            {
                var field = (PivotField)dgvStrings.Rows[e.RowIndex].Tag;
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;
                using (var sf = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center })
                {
                    using (var textBrush = new SolidBrush(e.CellStyle.ForeColor))
                    {
                        e.Graphics.DrawString(field.HeaderText, e.CellStyle.Font, textBrush, e.CellBounds, sf);
                    }
                }
            }
        }

        private void chkShowAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading)
            {
                UpdateFieldsVisibility();
                ApplyFieldsVisibility();
            }
        }

        private void dgvStrings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!LoadingGridValues && CurrentLayout != null)
            {
                var field = (PivotField)dgvStrings.Rows[e.RowIndex].Tag;
                SetStringValue(e.ColumnIndex, field.PropertyName, dgvStrings[e.ColumnIndex, e.RowIndex].Value);
                CurrentLayout.RebuildLayout();
                dgvStrings.AutoResizeColumn(e.ColumnIndex, DataGridViewAutoSizeColumnMode.AllCells);
            }
        }

        private void dgvStrings_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (dgvStrings[e.ColumnIndex, e.RowIndex].ValueType == typeof(Measure))
            {
                var curValue = (Measure)dgvStrings[e.ColumnIndex, e.RowIndex].Value;
                Measure newValue;
                if (MeasureParser.TryParse((string)e.Value, out newValue, curValue.Unit))
                {
                    e.Value = newValue;
                    e.ParsingApplied = true;
                }
            }
        }

        private void dgvStrings_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {

        }

        private void dgvStrings_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            //var currentField = (PivotField)dgvStrings.Rows[dgvStrings.CurrentCell.RowIndex].Tag;
            if(dgvStrings.CurrentCell.ValueType == typeof(Measure))
            {
                var length = (Measure)dgvStrings.CurrentCell.Value;
                (e.Control as TextBox).Text = length.ToString(new Measure.MeasureFormat()
                {
                    ShowFractions = false,
                    AllowApproximation = false
                });
            }
        }

        #endregion


    }
}
