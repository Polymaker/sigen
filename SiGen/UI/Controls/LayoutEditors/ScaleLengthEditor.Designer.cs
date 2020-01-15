namespace SiGen.UI.Controls
{
    partial class ScaleLengthEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScaleLengthEditor));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tlpLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblBass = new System.Windows.Forms.Label();
            this.lblScaleLength = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbDual = new System.Windows.Forms.RadioButton();
            this.rbMultiple = new System.Windows.Forms.RadioButton();
            this.mtbTrebleLength = new SiGen.UI.Controls.MeasureTextbox();
            this.mtbBassLength = new SiGen.UI.Controls.MeasureTextbox();
            this.lblTreble = new System.Windows.Forms.Label();
            this.lblMultiScaleRatio = new System.Windows.Forms.Label();
            this.lblParallelFret = new System.Windows.Forms.Label();
            this.nubMultiScaleRatio = new SiGen.UI.Controls.NumericBox();
            this.cboParallelFret = new System.Windows.Forms.ComboBox();
            this.dgvScaleLengths = new System.Windows.Forms.DataGridView();
            this.colStringNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScaleLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMultiScaleRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tlpLayout.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScaleLengths)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpLayout
            // 
            resources.ApplyResources(this.tlpLayout, "tlpLayout");
            this.tlpLayout.Controls.Add(this.lblBass, 0, 2);
            this.tlpLayout.Controls.Add(this.lblScaleLength, 0, 0);
            this.tlpLayout.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tlpLayout.Controls.Add(this.mtbTrebleLength, 1, 1);
            this.tlpLayout.Controls.Add(this.mtbBassLength, 1, 2);
            this.tlpLayout.Controls.Add(this.lblTreble, 0, 1);
            this.tlpLayout.Controls.Add(this.lblMultiScaleRatio, 0, 4);
            this.tlpLayout.Controls.Add(this.lblParallelFret, 0, 3);
            this.tlpLayout.Controls.Add(this.nubMultiScaleRatio, 1, 4);
            this.tlpLayout.Controls.Add(this.cboParallelFret, 1, 3);
            this.tlpLayout.Name = "tlpLayout";
            // 
            // lblBass
            // 
            resources.ApplyResources(this.lblBass, "lblBass");
            this.lblBass.Name = "lblBass";
            // 
            // lblScaleLength
            // 
            resources.ApplyResources(this.lblScaleLength, "lblScaleLength");
            this.lblScaleLength.Name = "lblScaleLength";
            // 
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.tlpLayout.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Controls.Add(this.rbSingle);
            this.flowLayoutPanel1.Controls.Add(this.rbDual);
            this.flowLayoutPanel1.Controls.Add(this.rbMultiple);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // rbSingle
            // 
            resources.ApplyResources(this.rbSingle, "rbSingle");
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.UseVisualStyleBackColor = true;
            this.rbSingle.CheckedChanged += new System.EventHandler(this.rbScaleLengthMode_CheckedChanged);
            // 
            // rbDual
            // 
            resources.ApplyResources(this.rbDual, "rbDual");
            this.rbDual.Name = "rbDual";
            this.rbDual.UseVisualStyleBackColor = true;
            this.rbDual.CheckedChanged += new System.EventHandler(this.rbScaleLengthMode_CheckedChanged);
            // 
            // rbMultiple
            // 
            resources.ApplyResources(this.rbMultiple, "rbMultiple");
            this.rbMultiple.Name = "rbMultiple";
            this.rbMultiple.UseVisualStyleBackColor = true;
            this.rbMultiple.CheckedChanged += new System.EventHandler(this.rbScaleLengthMode_CheckedChanged);
            // 
            // mtbTrebleLength
            // 
            resources.ApplyResources(this.mtbTrebleLength, "mtbTrebleLength");
            this.mtbTrebleLength.Name = "mtbTrebleLength";
            this.mtbTrebleLength.ValueChanged += new System.EventHandler(this.mtbTrebleLength_ValueChanged);
            // 
            // mtbBassLength
            // 
            resources.ApplyResources(this.mtbBassLength, "mtbBassLength");
            this.mtbBassLength.Name = "mtbBassLength";
            this.mtbBassLength.ValueChanged += new System.EventHandler(this.mtbBassLength_ValueChanged);
            // 
            // lblTreble
            // 
            resources.ApplyResources(this.lblTreble, "lblTreble");
            this.lblTreble.Name = "lblTreble";
            // 
            // lblMultiScaleRatio
            // 
            resources.ApplyResources(this.lblMultiScaleRatio, "lblMultiScaleRatio");
            this.lblMultiScaleRatio.Name = "lblMultiScaleRatio";
            // 
            // lblParallelFret
            // 
            resources.ApplyResources(this.lblParallelFret, "lblParallelFret");
            this.lblParallelFret.Name = "lblParallelFret";
            // 
            // nubMultiScaleRatio
            // 
            resources.ApplyResources(this.nubMultiScaleRatio, "nubMultiScaleRatio");
            this.nubMultiScaleRatio.MaximumValue = 1D;
            this.nubMultiScaleRatio.Name = "nubMultiScaleRatio";
            this.nubMultiScaleRatio.ValueChanged += new System.EventHandler(this.nubMultiScaleRatio_ValueChanged);
            // 
            // cboParallelFret
            // 
            this.cboParallelFret.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cboParallelFret.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboParallelFret.FormattingEnabled = true;
            resources.ApplyResources(this.cboParallelFret, "cboParallelFret");
            this.cboParallelFret.Name = "cboParallelFret";
            this.cboParallelFret.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cboParallelFret_DrawItem);
            this.cboParallelFret.SelectedIndexChanged += new System.EventHandler(this.cboParallelFret_SelectedIndexChanged);
            // 
            // dgvScaleLengths
            // 
            this.dgvScaleLengths.AllowUserToAddRows = false;
            this.dgvScaleLengths.AllowUserToDeleteRows = false;
            this.dgvScaleLengths.AllowUserToResizeRows = false;
            this.dgvScaleLengths.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvScaleLengths.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colStringNumber,
            this.colScaleLength,
            this.colMultiScaleRatio});
            resources.ApplyResources(this.dgvScaleLengths, "dgvScaleLengths");
            this.dgvScaleLengths.Name = "dgvScaleLengths";
            this.dgvScaleLengths.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvScaleLengths_CellFormatting);
            this.dgvScaleLengths.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvScaleLengths_CellParsing);
            this.dgvScaleLengths.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvScaleLengths_CellValidating);
            this.dgvScaleLengths.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScaleLengths_CellValueChanged);
            this.dgvScaleLengths.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvScaleLengths_EditingControlShowing);
            // 
            // colStringNumber
            // 
            this.colStringNumber.DataPropertyName = "Index";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colStringNumber.DefaultCellStyle = dataGridViewCellStyle4;
            resources.ApplyResources(this.colStringNumber, "colStringNumber");
            this.colStringNumber.Name = "colStringNumber";
            this.colStringNumber.ReadOnly = true;
            // 
            // colScaleLength
            // 
            this.colScaleLength.DataPropertyName = "ScaleLength";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colScaleLength.DefaultCellStyle = dataGridViewCellStyle5;
            resources.ApplyResources(this.colScaleLength, "colScaleLength");
            this.colScaleLength.Name = "colScaleLength";
            this.colScaleLength.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // colMultiScaleRatio
            // 
            this.colMultiScaleRatio.DataPropertyName = "MultiScaleRatio";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.colMultiScaleRatio.DefaultCellStyle = dataGridViewCellStyle6;
            resources.ApplyResources(this.colMultiScaleRatio, "colMultiScaleRatio");
            this.colMultiScaleRatio.Name = "colMultiScaleRatio";
            this.colMultiScaleRatio.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // ScaleLengthEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvScaleLengths);
            this.Controls.Add(this.tlpLayout);
            this.Name = "ScaleLengthEditor";
            this.tlpLayout.ResumeLayout(false);
            this.tlpLayout.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScaleLengths)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLayout;
        private System.Windows.Forms.Label lblScaleLength;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.RadioButton rbSingle;
        private System.Windows.Forms.RadioButton rbDual;
        private System.Windows.Forms.RadioButton rbMultiple;
        private MeasureTextbox mtbTrebleLength;
        private MeasureTextbox mtbBassLength;
        private System.Windows.Forms.Label lblBass;
        private System.Windows.Forms.Label lblTreble;
        private System.Windows.Forms.Label lblMultiScaleRatio;
        private NumericBox nubMultiScaleRatio;
        private System.Windows.Forms.DataGridView dgvScaleLengths;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStringNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScaleLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMultiScaleRatio;
        private System.Windows.Forms.Label lblParallelFret;
        private System.Windows.Forms.ComboBox cboParallelFret;
    }
}
