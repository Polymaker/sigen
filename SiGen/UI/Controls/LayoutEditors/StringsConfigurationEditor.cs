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
        private ListSortDirection GridColumnsSortOrder;

        public StringsConfigurationEditor()
        {
            InitializeComponent();
            GridColumnsSortOrder = ListSortDirection.Ascending;
            InitializePivotFields();
        }

        protected override void ReadLayoutProperties()
        {
            base.ReadLayoutProperties();
            nbxNumberOfStrings.Enabled = (CurrentLayout != null);
            nbxNumberOfFrets.Enabled = (CurrentLayout != null);
            chkLeftHanded.Enabled = (CurrentLayout != null);
            chkShowAdvanced.Enabled = (CurrentLayout != null);

            if(IsLayoutFirstLoad)
                chkShowAdvanced.Checked = false;
            GridColumnsSortOrder = ListSortDirection.Ascending;

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

            UpdateFieldsVisibility();
            BuildPivotGridLayout(false);
        }

        protected override void OnLayoutUpdated()
        {
            base.OnLayoutUpdated();
            UpdateFieldsVisibility();
            ApplyFieldsVisibility();
            UpdateGridValues();
        }

        protected override void OnCurrentLayoutChanged()
        {
            base.OnCurrentLayoutChanged();
            //if (!CachedLayoutData.ContainsKey(CurrentLayout))
            //    chkShowAdvanced.Checked = false;
        }

        protected override void OnNumberOfStringsChanged()
        {
            base.OnNumberOfStringsChanged();
            BuildPivotGridLayout();
        }

        protected override void CacheCurrentLayoutValues()
        {
            base.CacheCurrentLayoutValues();
            CachedLayoutData[CurrentLayout]["ShowAdvanced"] = chkShowAdvanced.Checked;
            CachedLayoutData[CurrentLayout]["StringOrder"] = GridColumnsSortOrder;
        }

        protected override void RestoreCachedLayoutValues()
        {
            base.RestoreCachedLayoutValues();
            chkShowAdvanced.Checked = (bool)CachedLayoutData[CurrentLayout]["ShowAdvanced"];
            GridColumnsSortOrder = (ListSortDirection)CachedLayoutData[CurrentLayout]["StringOrder"];
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
                CurrentLayout.StartBatchChanges();
                CurrentLayout.Strings.SetAll(s => s.NumberOfFrets, (int)nbxNumberOfFrets.Value);
                CurrentLayout.FinishBatchChanges();
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

        private void chkShowAdvanced_CheckedChanged(object sender, EventArgs e)
        {
            if (!IsLoading && CurrentLayout != null)
            {
                UpdateFieldsVisibility();
                ApplyFieldsVisibility();
            }
        }

        #region Pivot DataGridView

        private class PivotField
        {
            public string HeaderText { get; set; }
            public string PropertyName { get; set; }
            public bool Visible { get; set; }
            public bool ReadOnly { get; set; }
            public Type ValueType { get; set; }
            public PivotField(string propName, string display)
            {
                PropertyName = propName;
                HeaderText = display;
                Visible = true;
            }
        }

        private List<PivotField> PivotFields;
        private PropertyDescriptorCollection StringProperties;
        private PropertyDescriptorCollection PhysicalProperties;
        private PropertyDescriptorCollection TuningProperties;

        private void InitializePivotFields()
        {
            StringProperties = TypeDescriptor.GetProperties(typeof(SIString));
            PhysicalProperties = TypeDescriptor.GetProperties(typeof(StringedInstruments.Data.StringProperties));
            TuningProperties = TypeDescriptor.GetProperties(typeof(StringedInstruments.Data.StringTuning));

            PivotFields = new List<PivotField>();
            PivotFields.Add(new PivotField("StartingFret", "Starting Fret"));
            PivotFields.Add(new PivotField("NumberOfFrets", "Number Of Frets"));
            PivotFields.Add(new PivotField("ScaleLength", "Scale Length") { Visible = false });
            PivotFields.Add(new PivotField("MultiScaleRatio", "Align. Ratio") { Visible = false });
            PivotFields.Add(new PivotField("Gauge", "Gauge"));
            PivotFields.Add(new PivotField("PhysicalProperties.CoreWireDiameter", "Core Wire diam.") { Visible = false });
            PivotFields.Add(new PivotField("PhysicalProperties.UnitWeight", "Unit Weight (lbs/in)") { Visible = false });
            PivotFields.Add(new PivotField("PhysicalProperties.ModulusOfElasticity", "Elast. Modulus (GPa)") { Visible = false });
            PivotFields.Add(new PivotField("Tuning.Note", "Note") { Visible = false });

            foreach (var field in PivotFields)
                field.ValueType = GetValueType(field.PropertyName);
        }

        private void BuildPivotGridLayout(bool keepSelection = true)
        {
            using (FlagManager.UseFlag("CreatePivotGrid"))
            {
                if (CurrentLayout == null)
                {
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
                            Tag = i,
                            MinimumWidth = 50
                        };
                        dgvStrings.Columns.Add(stringCol);
                    }

                    ApplyColumnOrdering();

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

                    if (keepSelection)
                    {
                        if (currentCell.X == -1 && currentCell.Y == -1)
                            dgvStrings.CurrentCell = null;
                        else if (currentCell.X < dgvStrings.ColumnCount && currentCell.Y < dgvStrings.RowCount)
                            dgvStrings.CurrentCell = dgvStrings[currentCell.X, currentCell.Y];
                    }
                    else
                        dgvStrings.CurrentCell = null;
                }
            }
        }
        
        private void ApplyColumnOrdering()
        {
            for (int i = 0; i < dgvStrings.ColumnCount; i++)
            {
                dgvStrings.Columns[i].DisplayIndex = GridColumnsSortOrder == ListSortDirection.Ascending ? i : (dgvStrings.ColumnCount - (i + 1));
            }
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
                    else if (field.PropertyName.Contains("Tuning"))
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
            if(CurrentLayout != null)
            {
                using (FlagManager.UseFlag("FillPivotGrid"))
                {
                    foreach (DataGridViewRow row in dgvStrings.Rows)
                    {
                        var field = (PivotField)row.Tag;
                        for (int i = 0; i < dgvStrings.ColumnCount; i++)
                        {
                            row.Cells[i].Value = GetCellValue(i, row.Index);
                            row.Cells[i].ValueType = field.ValueType;
                        }
                    }
                }
            }
        }

        #region Grid Events

        private void dgvStrings_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0/* && e.RowIndex >= 0*/)
            {
                e.PaintBackground(e.ClipBounds, true);
                e.Handled = true;

                string cellText = string.Empty;

                if (e.RowIndex >= 0)
                {
                    cellText = GetRowField(e.RowIndex).HeaderText;
                }
                else
                {
                    cellText = GridColumnsSortOrder == ListSortDirection.Ascending ? "Treble -> Bass" : "Bass -> Treble";
                }

                using (var sf = new StringFormat() { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center, FormatFlags = StringFormatFlags.NoWrap, Trimming = StringTrimming.EllipsisCharacter })
                using (var textBrush = new SolidBrush(e.CellStyle.ForeColor))
                {
                    var textBound = Rectangle.FromLTRB(e.CellBounds.Left + 6, e.CellBounds.Top, e.CellBounds.Right, e.CellBounds.Bottom);
                    e.Graphics.DrawString(cellText, e.CellStyle.Font, textBrush, textBound, sf);
                }
            }
        }

        private void dgvStrings_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!IsLoading && !FlagManager["FillPivotGrid"] && CurrentLayout != null)
            {
                SetCellValue(e.ColumnIndex, e.RowIndex, dgvStrings[e.ColumnIndex, e.RowIndex].Value);
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
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && !IsLoading && !FlagManager["CreatePivotGrid"])
            {
                var cellInfo = GetCellFieldAndObject(e.ColumnIndex, e.RowIndex);
                if (cellInfo.Item1.PropertyName == "MultiScaleRatio")
                {
                    double ratio = 0;
                    if (!double.TryParse((string)e.FormattedValue, out ratio) || ratio < 0 || ratio > 1)
                        e.Cancel = true;
                }
            }
        }

        private void dgvStrings_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (dgvStrings.CurrentCell.ValueType == typeof(Measure))
            {
                var length = (Measure)dgvStrings.CurrentCell.Value;
                (e.Control as TextBox).Text = length.ToString(new Measure.MeasureFormat()
                {
                    ShowFractions = false,
                    AllowApproximation = false
                });
            }
        }

        private void dgvStrings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && !IsLoading && !FlagManager["CreatePivotGrid"])
            {
                var cellInfo = GetCellFieldAndObject(e.ColumnIndex, e.RowIndex);
                if (cellInfo.Item1.PropertyName == "Gauge")
                {
                    if (e.Value != null && e.Value is Measure)
                    {
                        var curValue = (Measure)e.Value;
                        e.Value = curValue.ToString(new Measure.MeasureFormat()
                        {
                            ShowFractions = false,
                            AllowApproximation = false
                        });
                    }
                }
            }
        }

        private void dgvStrings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(CurrentLayout != null && e.ColumnIndex == -1 && e.RowIndex == -1)
            {
                if (GridColumnsSortOrder == ListSortDirection.Ascending)
                    GridColumnsSortOrder = ListSortDirection.Descending;
                else
                    GridColumnsSortOrder = ListSortDirection.Ascending;
                ApplyColumnOrdering();
            }
        }

        #endregion

        #region Row/Column (Field/Object) Accessors

        private int GetColumnStringIndex(int columnIndex)
        {
            return (int)dgvStrings.Columns[columnIndex].Tag;
        }

        private SIString GetColumnObject(int columnIndex)
        {
            return CurrentLayout.Strings[(int)dgvStrings.Columns[columnIndex].Tag];
        }

        private PivotField GetRowField(int rowIndex)
        {
            return (PivotField)dgvStrings.Rows[rowIndex].Tag;
        }

        private Tuple<PivotField, SIString> GetCellFieldAndObject(int columnIndex, int rowIndex)
        {
            return new Tuple<PivotField, SIString>(GetRowField(rowIndex), GetColumnObject(columnIndex));
        }

        private Type GetValueType(string propertyName)
        {
            if (!propertyName.Contains("."))
            {
                return StringProperties[propertyName].PropertyType;
            }
            else
            {
                string[] propNames = propertyName.Split('.');
                if (propNames[0] == "PhysicalProperties")
                    return PhysicalProperties[propNames[1]].PropertyType;
                else if (propNames[0] == "Tuning")
                    return TuningProperties[propNames[1]].PropertyType;
            }
            return null;
        }

        private void SetCellValue(int columnIndex, int rowIndex, object value)
        {
            var cellInfo = GetCellFieldAndObject(columnIndex, rowIndex);
            if (!cellInfo.Item1.PropertyName.Contains("."))
            {
                StringProperties[cellInfo.Item1.PropertyName].SetValue(cellInfo.Item2, value);
            }
            else
            {
                string[] propNames = cellInfo.Item1.PropertyName.Split('.');
                var baseObj = StringProperties[propNames[0]].GetValue(cellInfo.Item2);
                if (baseObj != null)
                {
                    if (propNames[0] == "PhysicalProperties")
                        PhysicalProperties[propNames[1]].SetValue(baseObj, value);
                    else if (propNames[0] == "Tuning")
                        TuningProperties[propNames[1]].SetValue(baseObj, value);
                }
            }
        }

        private object GetCellValue(int columnIndex, int rowIndex)
        {
            var cellInfo = GetCellFieldAndObject(columnIndex, rowIndex);
            if (!cellInfo.Item1.PropertyName.Contains("."))
            {
                return StringProperties[cellInfo.Item1.PropertyName].GetValue(cellInfo.Item2);
            }
            else
            {
                string[] propNames = cellInfo.Item1.PropertyName.Split('.');
                var baseObj = StringProperties[propNames[0]].GetValue(cellInfo.Item2);
                if (baseObj != null)
                {
                    if (propNames[0] == "PhysicalProperties")
                        return PhysicalProperties[propNames[1]].GetValue(baseObj);
                    else if (propNames[0] == "Tuning")
                        return TuningProperties[propNames[1]].GetValue(baseObj);
                }
            }
            return null;
        }



        #endregion

        #endregion

        private void dgvStrings_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                if(dgvStrings[e.ColumnIndex, e.RowIndex].ValueType == typeof(Measure))
                {
                    //cmsMesureCellMenu.Tag = dgvStrings[e.ColumnIndex, e.RowIndex];

                    if (dgvStrings.CurrentCell != dgvStrings[e.ColumnIndex, e.RowIndex])
                        dgvStrings.CurrentCell = dgvStrings[e.ColumnIndex, e.RowIndex];

                    var localMousePos = dgvStrings.PointToClient(Cursor.Position);
                    cmsMesureCellMenu.Show(dgvStrings, localMousePos);
                }
            }
        }

        private void tsmiConvertToMM_Click(object sender, EventArgs e)
        {
            var currentMeasure = (Measure)dgvStrings.CurrentCell.Value;
            currentMeasure.Unit = UnitOfMeasure.Mm;
            dgvStrings.CurrentCell.Value = currentMeasure;
            dgvStrings.UpdateCellValue(dgvStrings.CurrentCell.ColumnIndex, dgvStrings.CurrentCell.RowIndex);
        }

        private void tsmiConvertToCM_Click(object sender, EventArgs e)
        {
            var currentMeasure = (Measure)dgvStrings.CurrentCell.Value;
            currentMeasure.Unit = UnitOfMeasure.Cm;
            dgvStrings.CurrentCell.Value = currentMeasure;
            dgvStrings.UpdateCellValue(dgvStrings.CurrentCell.ColumnIndex, dgvStrings.CurrentCell.RowIndex);
        }

        private void tsmiConvertToIN_Click(object sender, EventArgs e)
        {
            var currentMeasure = (Measure)dgvStrings.CurrentCell.Value;
            currentMeasure.Unit = UnitOfMeasure.In;
            dgvStrings.CurrentCell.Value = currentMeasure;
            dgvStrings.UpdateCellValue(dgvStrings.CurrentCell.ColumnIndex, dgvStrings.CurrentCell.RowIndex);
        }

        private void tsmiConvertToFT_Click(object sender, EventArgs e)
        {
            var currentMeasure = (Measure)dgvStrings.CurrentCell.Value;
            currentMeasure.Unit = UnitOfMeasure.Ft;
            dgvStrings.CurrentCell.Value = currentMeasure;
            dgvStrings.UpdateCellValue(dgvStrings.CurrentCell.ColumnIndex, dgvStrings.CurrentCell.RowIndex);
        }

        private void cmsMesureCellMenu_Opening(object sender, CancelEventArgs e)
        {
            var currentMeasure = (Measure)dgvStrings.CurrentCell.Value;
            tsmiConvertToMM.Visible = currentMeasure.Unit != UnitOfMeasure.Mm;
            tsmiConvertToCM.Visible = currentMeasure.Unit != UnitOfMeasure.Cm;
            tsmiConvertToIN.Visible = currentMeasure.Unit != UnitOfMeasure.In;
            tsmiConvertToFT.Visible = currentMeasure.Unit != UnitOfMeasure.Ft;
        }
    }
}
