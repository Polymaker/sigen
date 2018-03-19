namespace SiGen.UI.Controls
{
    partial class FingerboardMarginEditor
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
            this.lblBridge = new System.Windows.Forms.Label();
            this.lblNut = new System.Windows.Forms.Label();
            this.lblBass = new System.Windows.Forms.Label();
            this.mtbBridgeTreble = new SiGen.UI.MeasureTextbox();
            this.mtbBridgeBass = new SiGen.UI.MeasureTextbox();
            this.mtbNutTreble = new SiGen.UI.MeasureTextbox();
            this.mtbNutBass = new SiGen.UI.MeasureTextbox();
            this.mtbLastFret = new SiGen.UI.MeasureTextbox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEditMode = new System.Windows.Forms.Label();
            this.lblTreble = new System.Windows.Forms.Label();
            this.cboMarginEditMode = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 69F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.lblBridge, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblNut, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBass, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.mtbBridgeTreble, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.mtbBridgeBass, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.mtbNutTreble, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbNutBass, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbLastFret, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblEditMode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTreble, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboMarginEditMode, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(264, 131);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // lblBridge
            // 
            this.lblBridge.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBridge.AutoSize = true;
            this.lblBridge.Location = new System.Drawing.Point(29, 69);
            this.lblBridge.Name = "lblBridge";
            this.lblBridge.Size = new System.Drawing.Size(37, 13);
            this.lblBridge.TabIndex = 2;
            this.lblBridge.Text = "Bridge";
            // 
            // lblNut
            // 
            this.lblNut.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNut.AutoSize = true;
            this.lblNut.Location = new System.Drawing.Point(42, 45);
            this.lblNut.Name = "lblNut";
            this.lblNut.Size = new System.Drawing.Size(24, 13);
            this.lblNut.TabIndex = 2;
            this.lblNut.Text = "Nut";
            // 
            // lblBass
            // 
            this.lblBass.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBass.AutoSize = true;
            this.lblBass.Location = new System.Drawing.Point(102, 27);
            this.lblBass.Name = "lblBass";
            this.lblBass.Size = new System.Drawing.Size(30, 13);
            this.lblBass.TabIndex = 1;
            this.lblBass.Text = "Bass";
            // 
            // mtbBridgeTreble
            // 
            this.mtbBridgeTreble.Location = new System.Drawing.Point(169, 67);
            this.mtbBridgeTreble.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBridgeTreble.Name = "mtbBridgeTreble";
            this.mtbBridgeTreble.Size = new System.Drawing.Size(92, 20);
            this.mtbBridgeTreble.TabIndex = 2;
            this.mtbBridgeTreble.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbBridgeBass
            // 
            this.mtbBridgeBass.Location = new System.Drawing.Point(72, 67);
            this.mtbBridgeBass.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBridgeBass.Name = "mtbBridgeBass";
            this.mtbBridgeBass.Size = new System.Drawing.Size(91, 20);
            this.mtbBridgeBass.TabIndex = 2;
            this.mtbBridgeBass.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbNutTreble
            // 
            this.mtbNutTreble.Location = new System.Drawing.Point(169, 43);
            this.mtbNutTreble.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbNutTreble.Name = "mtbNutTreble";
            this.mtbNutTreble.Size = new System.Drawing.Size(92, 20);
            this.mtbNutTreble.TabIndex = 2;
            this.mtbNutTreble.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbNutBass
            // 
            this.mtbNutBass.Location = new System.Drawing.Point(72, 43);
            this.mtbNutBass.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbNutBass.Name = "mtbNutBass";
            this.mtbNutBass.Size = new System.Drawing.Size(91, 20);
            this.mtbNutBass.TabIndex = 1;
            this.mtbNutBass.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbLastFret
            // 
            this.mtbLastFret.AllowEmptyValue = true;
            this.mtbLastFret.Location = new System.Drawing.Point(72, 91);
            this.mtbLastFret.Name = "mtbLastFret";
            this.mtbLastFret.Size = new System.Drawing.Size(91, 20);
            this.mtbLastFret.TabIndex = 1;
            this.mtbLastFret.ValueChanged += new System.EventHandler(this.mtbLastFret_ValueChanged);
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 88);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 26);
            this.label2.TabIndex = 1;
            this.label2.Text = "Last Fret Overhang";
            // 
            // lblEditMode
            // 
            this.lblEditMode.AutoSize = true;
            this.lblEditMode.Location = new System.Drawing.Point(3, 0);
            this.lblEditMode.Name = "lblEditMode";
            this.lblEditMode.Size = new System.Drawing.Size(59, 26);
            this.lblEditMode.TabIndex = 0;
            this.lblEditMode.Text = "Define Margins By";
            // 
            // lblTreble
            // 
            this.lblTreble.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblTreble.AutoSize = true;
            this.lblTreble.Location = new System.Drawing.Point(196, 27);
            this.lblTreble.Name = "lblTreble";
            this.lblTreble.Size = new System.Drawing.Size(37, 13);
            this.lblTreble.TabIndex = 3;
            this.lblTreble.Text = "Treble";
            // 
            // cboMarginEditMode
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cboMarginEditMode, 2);
            this.cboMarginEditMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarginEditMode.FormattingEnabled = true;
            this.cboMarginEditMode.Location = new System.Drawing.Point(72, 3);
            this.cboMarginEditMode.Name = "cboMarginEditMode";
            this.cboMarginEditMode.Size = new System.Drawing.Size(148, 21);
            this.cboMarginEditMode.TabIndex = 4;
            this.cboMarginEditMode.SelectedIndexChanged += new System.EventHandler(this.cboMarginEditMode_SelectedIndexChanged);
            // 
            // FingerboardMarginEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FingerboardMarginEditor";
            this.Size = new System.Drawing.Size(292, 134);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblEditMode;
        private MeasureTextbox mtbLastFret;
        private System.Windows.Forms.Label label2;
        private MeasureTextbox mtbBridgeTreble;
        private MeasureTextbox mtbBridgeBass;
        private MeasureTextbox mtbNutTreble;
        private MeasureTextbox mtbNutBass;
        private System.Windows.Forms.Label lblBridge;
        private System.Windows.Forms.Label lblNut;
        private System.Windows.Forms.Label lblBass;
        private System.Windows.Forms.Label lblTreble;
        private System.Windows.Forms.ComboBox cboMarginEditMode;
    }
}
