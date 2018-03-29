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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblBass = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.rbSingle = new System.Windows.Forms.RadioButton();
            this.rbDual = new System.Windows.Forms.RadioButton();
            this.rbMultiple = new System.Windows.Forms.RadioButton();
            this.mtbTrebleLength = new SiGen.UI.MeasureTextbox();
            this.mtbBassLength = new SiGen.UI.MeasureTextbox();
            this.lblTreble = new System.Windows.Forms.Label();
            this.lblMultiScaleRatio = new System.Windows.Forms.Label();
            this.nubMultiScaleRatio = new SiGen.UI.Controls.NumericBox();
            this.dgvScaleLengths = new System.Windows.Forms.DataGridView();
            this.colStringNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScaleLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMultiScaleRatio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScaleLengths)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblBass, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.mtbTrebleLength, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.mtbBassLength, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblTreble, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblMultiScaleRatio, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.nubMultiScaleRatio, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgvScaleLengths, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(309, 202);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblBass
            // 
            this.lblBass.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBass.AutoSize = true;
            this.lblBass.Location = new System.Drawing.Point(57, 52);
            this.lblBass.Name = "lblBass";
            this.lblBass.Size = new System.Drawing.Size(30, 13);
            this.lblBass.TabIndex = 5;
            this.lblBass.Text = "Bass";
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Scale Length";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.CausesValidation = false;
            this.flowLayoutPanel1.Controls.Add(this.rbSingle);
            this.flowLayoutPanel1.Controls.Add(this.rbDual);
            this.flowLayoutPanel1.Controls.Add(this.rbMultiple);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(90, 0);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(219, 23);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // rbSingle
            // 
            this.rbSingle.AutoSize = true;
            this.rbSingle.Location = new System.Drawing.Point(3, 3);
            this.rbSingle.Name = "rbSingle";
            this.rbSingle.Size = new System.Drawing.Size(54, 17);
            this.rbSingle.TabIndex = 0;
            this.rbSingle.Text = "Single";
            this.rbSingle.UseVisualStyleBackColor = true;
            this.rbSingle.CheckedChanged += new System.EventHandler(this.rbScaleLengthMode_CheckedChanged);
            // 
            // rbDual
            // 
            this.rbDual.AutoSize = true;
            this.rbDual.Location = new System.Drawing.Point(63, 3);
            this.rbDual.Name = "rbDual";
            this.rbDual.Size = new System.Drawing.Size(47, 17);
            this.rbDual.TabIndex = 1;
            this.rbDual.Text = "Dual";
            this.rbDual.UseVisualStyleBackColor = true;
            this.rbDual.CheckedChanged += new System.EventHandler(this.rbScaleLengthMode_CheckedChanged);
            // 
            // rbMultiple
            // 
            this.rbMultiple.AutoSize = true;
            this.rbMultiple.Location = new System.Drawing.Point(116, 3);
            this.rbMultiple.Name = "rbMultiple";
            this.rbMultiple.Size = new System.Drawing.Size(61, 17);
            this.rbMultiple.TabIndex = 2;
            this.rbMultiple.Text = "Multiple";
            this.rbMultiple.UseVisualStyleBackColor = true;
            this.rbMultiple.CheckedChanged += new System.EventHandler(this.rbScaleLengthMode_CheckedChanged);
            // 
            // mtbTrebleLength
            // 
            this.mtbTrebleLength.Location = new System.Drawing.Point(93, 26);
            this.mtbTrebleLength.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbTrebleLength.Name = "mtbTrebleLength";
            this.mtbTrebleLength.Size = new System.Drawing.Size(100, 20);
            this.mtbTrebleLength.TabIndex = 2;
            this.mtbTrebleLength.ValueChanged += new System.EventHandler(this.mtbTrebleLength_ValueChanged);
            // 
            // mtbBassLength
            // 
            this.mtbBassLength.Location = new System.Drawing.Point(93, 50);
            this.mtbBassLength.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBassLength.Name = "mtbBassLength";
            this.mtbBassLength.Size = new System.Drawing.Size(100, 20);
            this.mtbBassLength.TabIndex = 3;
            this.mtbBassLength.ValueChanged += new System.EventHandler(this.mtbBassLength_ValueChanged);
            // 
            // lblTreble
            // 
            this.lblTreble.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTreble.AutoSize = true;
            this.lblTreble.Location = new System.Drawing.Point(50, 28);
            this.lblTreble.Name = "lblTreble";
            this.lblTreble.Size = new System.Drawing.Size(37, 13);
            this.lblTreble.TabIndex = 4;
            this.lblTreble.Text = "Treble";
            // 
            // lblMultiScaleRatio
            // 
            this.lblMultiScaleRatio.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMultiScaleRatio.AutoSize = true;
            this.lblMultiScaleRatio.Location = new System.Drawing.Point(6, 76);
            this.lblMultiScaleRatio.Name = "lblMultiScaleRatio";
            this.lblMultiScaleRatio.Size = new System.Drawing.Size(81, 13);
            this.lblMultiScaleRatio.TabIndex = 6;
            this.lblMultiScaleRatio.Text = "Alignment Ratio";
            // 
            // nubMultiScaleRatio
            // 
            this.nubMultiScaleRatio.Location = new System.Drawing.Point(93, 74);
            this.nubMultiScaleRatio.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.nubMultiScaleRatio.MaximumValue = 1D;
            this.nubMultiScaleRatio.Name = "nubMultiScaleRatio";
            this.nubMultiScaleRatio.Size = new System.Drawing.Size(100, 20);
            this.nubMultiScaleRatio.TabIndex = 7;
            this.nubMultiScaleRatio.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nubMultiScaleRatio.ValueChanged += new System.EventHandler(this.nubMultiScaleRatio_ValueChanged);
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
            this.tableLayoutPanel1.SetColumnSpan(this.dgvScaleLengths, 2);
            this.dgvScaleLengths.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvScaleLengths.Location = new System.Drawing.Point(3, 98);
            this.dgvScaleLengths.MinimumSize = new System.Drawing.Size(0, 100);
            this.dgvScaleLengths.Name = "dgvScaleLengths";
            this.dgvScaleLengths.RowHeadersVisible = false;
            this.dgvScaleLengths.Size = new System.Drawing.Size(303, 101);
            this.dgvScaleLengths.TabIndex = 8;
            this.dgvScaleLengths.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvScaleLengths_CellValidating);
            this.dgvScaleLengths.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvScaleLengths_CellValueChanged);
            this.dgvScaleLengths.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvScaleLengths_EditingControlShowing);
            // 
            // colStringNumber
            // 
            this.colStringNumber.DataPropertyName = "Index";
            this.colStringNumber.HeaderText = "String #";
            this.colStringNumber.Name = "colStringNumber";
            this.colStringNumber.ReadOnly = true;
            // 
            // colScaleLength
            // 
            this.colScaleLength.DataPropertyName = "ScaleLength";
            this.colScaleLength.HeaderText = "Scale Length";
            this.colScaleLength.Name = "colScaleLength";
            // 
            // colMultiScaleRatio
            // 
            this.colMultiScaleRatio.DataPropertyName = "MultiScaleRatio";
            this.colMultiScaleRatio.HeaderText = "Alignment Ratio";
            this.colMultiScaleRatio.Name = "colMultiScaleRatio";
            // 
            // ScaleLengthEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ScaleLengthEditor";
            this.Size = new System.Drawing.Size(309, 202);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvScaleLengths)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
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
    }
}
