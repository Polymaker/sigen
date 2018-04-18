namespace SiGen.UI.Windows
{
    partial class ExportLayoutDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btnCancel = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.layoutPreview = new SiGen.UI.LayoutViewer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkExportStringCenters = new System.Windows.Forms.CheckBox();
            this.gbxFretsOptions = new System.Windows.Forms.GroupBox();
            this.pbxFretColor = new System.Windows.Forms.PictureBox();
            this.btnPickFretColor = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mtbFretThickness = new SiGen.UI.MeasureTextbox();
            this.chkFretThickness = new System.Windows.Forms.CheckBox();
            this.lblExtendAmount = new System.Windows.Forms.Label();
            this.mtbFretExtendAmount = new SiGen.UI.MeasureTextbox();
            this.chkExportFrets = new System.Windows.Forms.CheckBox();
            this.flpExtendDirection = new System.Windows.Forms.FlowLayoutPanel();
            this.rbExtendInward = new System.Windows.Forms.RadioButton();
            this.rbExtendOutward = new System.Windows.Forms.RadioButton();
            this.chkExtendFretSlots = new System.Windows.Forms.CheckBox();
            this.chkExportStrings = new System.Windows.Forms.CheckBox();
            this.flpExportFormat = new System.Windows.Forms.FlowLayoutPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.chkExportFingerboard = new System.Windows.Forms.CheckBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.lblExportFormat = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cboExportConfig = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbxFretsOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxFretColor)).BeginInit();
            this.flpExtendDirection.SuspendLayout();
            this.flpExportFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(536, 143);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.layoutPreview);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(617, 415);
            this.splitContainer1.SplitterDistance = 239;
            this.splitContainer1.TabIndex = 0;
            // 
            // layoutPreview
            // 
            this.layoutPreview.BackColor = System.Drawing.SystemColors.Window;
            this.layoutPreview.DisplayConfig.RenderRealFrets = false;
            this.layoutPreview.DisplayConfig.ShowMidlines = false;
            this.layoutPreview.DisplayConfig.ShowStrings = false;
            this.layoutPreview.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutPreview.Location = new System.Drawing.Point(0, 0);
            this.layoutPreview.Name = "layoutPreview";
            this.layoutPreview.Size = new System.Drawing.Size(617, 239);
            this.layoutPreview.TabIndex = 0;
            this.layoutPreview.Text = "layoutViewer1";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboExportConfig);
            this.groupBox1.Controls.Add(this.chkExportStringCenters);
            this.groupBox1.Controls.Add(this.gbxFretsOptions);
            this.groupBox1.Controls.Add(this.chkExportStrings);
            this.groupBox1.Controls.Add(this.flpExportFormat);
            this.groupBox1.Controls.Add(this.chkExportFingerboard);
            this.groupBox1.Controls.Add(this.btnCancel);
            this.groupBox1.Controls.Add(this.btnExport);
            this.groupBox1.Controls.Add(this.lblExportFormat);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(617, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export Options";
            // 
            // chkExportStringCenters
            // 
            this.chkExportStringCenters.AutoSize = true;
            this.chkExportStringCenters.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportStringCenters.Location = new System.Drawing.Point(11, 89);
            this.chkExportStringCenters.Name = "chkExportStringCenters";
            this.chkExportStringCenters.Size = new System.Drawing.Size(125, 17);
            this.chkExportStringCenters.TabIndex = 9;
            this.chkExportStringCenters.Text = "Export String Centers";
            this.chkExportStringCenters.UseVisualStyleBackColor = true;
            this.chkExportStringCenters.CheckedChanged += new System.EventHandler(this.chkExportStringCenters_CheckedChanged);
            // 
            // gbxFretsOptions
            // 
            this.gbxFretsOptions.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxFretsOptions.Controls.Add(this.pbxFretColor);
            this.gbxFretsOptions.Controls.Add(this.btnPickFretColor);
            this.gbxFretsOptions.Controls.Add(this.label1);
            this.gbxFretsOptions.Controls.Add(this.mtbFretThickness);
            this.gbxFretsOptions.Controls.Add(this.chkFretThickness);
            this.gbxFretsOptions.Controls.Add(this.lblExtendAmount);
            this.gbxFretsOptions.Controls.Add(this.mtbFretExtendAmount);
            this.gbxFretsOptions.Controls.Add(this.chkExportFrets);
            this.gbxFretsOptions.Controls.Add(this.flpExtendDirection);
            this.gbxFretsOptions.Controls.Add(this.chkExtendFretSlots);
            this.gbxFretsOptions.Location = new System.Drawing.Point(340, 13);
            this.gbxFretsOptions.Name = "gbxFretsOptions";
            this.gbxFretsOptions.Size = new System.Drawing.Size(270, 119);
            this.gbxFretsOptions.TabIndex = 1;
            this.gbxFretsOptions.TabStop = false;
            // 
            // pbxFretColor
            // 
            this.pbxFretColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbxFretColor.Location = new System.Drawing.Point(106, 90);
            this.pbxFretColor.Name = "pbxFretColor";
            this.pbxFretColor.Size = new System.Drawing.Size(21, 21);
            this.pbxFretColor.TabIndex = 15;
            this.pbxFretColor.TabStop = false;
            // 
            // btnPickFretColor
            // 
            this.btnPickFretColor.Location = new System.Drawing.Point(126, 89);
            this.btnPickFretColor.Name = "btnPickFretColor";
            this.btnPickFretColor.Size = new System.Drawing.Size(81, 23);
            this.btnPickFretColor.TabIndex = 16;
            this.btnPickFretColor.Text = "Pick Color";
            this.btnPickFretColor.UseVisualStyleBackColor = true;
            this.btnPickFretColor.Click += new System.EventHandler(this.btnPickFretColor_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Display Color";
            // 
            // mtbFretThickness
            // 
            this.mtbFretThickness.Enabled = false;
            this.mtbFretThickness.Location = new System.Drawing.Point(127, 66);
            this.mtbFretThickness.Name = "mtbFretThickness";
            this.mtbFretThickness.Size = new System.Drawing.Size(79, 20);
            this.mtbFretThickness.TabIndex = 13;
            this.mtbFretThickness.ValueChanged += new System.EventHandler(this.mtbFretThickness_ValueChanged);
            // 
            // chkFretThickness
            // 
            this.chkFretThickness.AutoSize = true;
            this.chkFretThickness.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkFretThickness.Location = new System.Drawing.Point(8, 69);
            this.chkFretThickness.Name = "chkFretThickness";
            this.chkFretThickness.Size = new System.Drawing.Size(113, 17);
            this.chkFretThickness.TabIndex = 12;
            this.chkFretThickness.Text = "Specify Thickness";
            this.chkFretThickness.UseVisualStyleBackColor = true;
            this.chkFretThickness.CheckedChanged += new System.EventHandler(this.chkFretThickness_CheckedChanged);
            // 
            // lblExtendAmount
            // 
            this.lblExtendAmount.AutoSize = true;
            this.lblExtendAmount.Location = new System.Drawing.Point(80, 46);
            this.lblExtendAmount.Name = "lblExtendAmount";
            this.lblExtendAmount.Size = new System.Drawing.Size(43, 13);
            this.lblExtendAmount.TabIndex = 11;
            this.lblExtendAmount.Text = "Amount";
            // 
            // mtbFretExtendAmount
            // 
            this.mtbFretExtendAmount.Enabled = false;
            this.mtbFretExtendAmount.Location = new System.Drawing.Point(127, 43);
            this.mtbFretExtendAmount.Name = "mtbFretExtendAmount";
            this.mtbFretExtendAmount.Size = new System.Drawing.Size(79, 20);
            this.mtbFretExtendAmount.TabIndex = 10;
            this.mtbFretExtendAmount.ValueChanged += new System.EventHandler(this.mtbFretExtendAmount_ValueChanged);
            // 
            // chkExportFrets
            // 
            this.chkExportFrets.AutoSize = true;
            this.chkExportFrets.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportFrets.Location = new System.Drawing.Point(6, 0);
            this.chkExportFrets.Name = "chkExportFrets";
            this.chkExportFrets.Size = new System.Drawing.Size(82, 17);
            this.chkExportFrets.TabIndex = 3;
            this.chkExportFrets.Text = "Export Frets";
            this.chkExportFrets.UseVisualStyleBackColor = true;
            this.chkExportFrets.CheckedChanged += new System.EventHandler(this.chkExportFrets_CheckedChanged);
            // 
            // flpExtendDirection
            // 
            this.flpExtendDirection.AutoSize = true;
            this.flpExtendDirection.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpExtendDirection.Controls.Add(this.rbExtendInward);
            this.flpExtendDirection.Controls.Add(this.rbExtendOutward);
            this.flpExtendDirection.Enabled = false;
            this.flpExtendDirection.Location = new System.Drawing.Point(127, 19);
            this.flpExtendDirection.Name = "flpExtendDirection";
            this.flpExtendDirection.Size = new System.Drawing.Size(134, 23);
            this.flpExtendDirection.TabIndex = 9;
            // 
            // rbExtendInward
            // 
            this.rbExtendInward.AutoSize = true;
            this.rbExtendInward.Checked = true;
            this.rbExtendInward.Location = new System.Drawing.Point(3, 3);
            this.rbExtendInward.Name = "rbExtendInward";
            this.rbExtendInward.Size = new System.Drawing.Size(57, 17);
            this.rbExtendInward.TabIndex = 0;
            this.rbExtendInward.TabStop = true;
            this.rbExtendInward.Text = "Inward";
            this.rbExtendInward.UseVisualStyleBackColor = true;
            // 
            // rbExtendOutward
            // 
            this.rbExtendOutward.AutoSize = true;
            this.rbExtendOutward.Location = new System.Drawing.Point(66, 3);
            this.rbExtendOutward.Name = "rbExtendOutward";
            this.rbExtendOutward.Size = new System.Drawing.Size(65, 17);
            this.rbExtendOutward.TabIndex = 1;
            this.rbExtendOutward.Text = "Outward";
            this.rbExtendOutward.UseVisualStyleBackColor = true;
            // 
            // chkExtendFretSlots
            // 
            this.chkExtendFretSlots.AutoSize = true;
            this.chkExtendFretSlots.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExtendFretSlots.Location = new System.Drawing.Point(15, 23);
            this.chkExtendFretSlots.Name = "chkExtendFretSlots";
            this.chkExtendFretSlots.Size = new System.Drawing.Size(106, 17);
            this.chkExtendFretSlots.TabIndex = 7;
            this.chkExtendFretSlots.Text = "Extend Fret Slots";
            this.chkExtendFretSlots.UseVisualStyleBackColor = true;
            this.chkExtendFretSlots.CheckedChanged += new System.EventHandler(this.chkExtendFretSlots_CheckedChanged);
            // 
            // chkExportStrings
            // 
            this.chkExportStrings.AutoSize = true;
            this.chkExportStrings.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportStrings.Location = new System.Drawing.Point(11, 66);
            this.chkExportStrings.Name = "chkExportStrings";
            this.chkExportStrings.Size = new System.Drawing.Size(91, 17);
            this.chkExportStrings.TabIndex = 0;
            this.chkExportStrings.Text = "Export Strings";
            this.chkExportStrings.UseVisualStyleBackColor = true;
            this.chkExportStrings.CheckedChanged += new System.EventHandler(this.chkExportStrings_CheckedChanged);
            // 
            // flpExportFormat
            // 
            this.flpExportFormat.AutoSize = true;
            this.flpExportFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpExportFormat.Controls.Add(this.radioButton1);
            this.flpExportFormat.Controls.Add(this.radioButton2);
            this.flpExportFormat.Location = new System.Drawing.Point(90, 18);
            this.flpExportFormat.Name = "flpExportFormat";
            this.flpExportFormat.Size = new System.Drawing.Size(105, 23);
            this.flpExportFormat.TabIndex = 8;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(3, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "SVG";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Enabled = false;
            this.radioButton2.Location = new System.Drawing.Point(56, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(46, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.Text = "DXF";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // chkExportFingerboard
            // 
            this.chkExportFingerboard.AutoSize = true;
            this.chkExportFingerboard.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportFingerboard.Location = new System.Drawing.Point(11, 44);
            this.chkExportFingerboard.Name = "chkExportFingerboard";
            this.chkExportFingerboard.Size = new System.Drawing.Size(115, 17);
            this.chkExportFingerboard.TabIndex = 6;
            this.chkExportFingerboard.Text = "Export Fingerboard";
            this.chkExportFingerboard.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(455, 143);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblExportFormat
            // 
            this.lblExportFormat.AutoSize = true;
            this.lblExportFormat.Location = new System.Drawing.Point(12, 23);
            this.lblExportFormat.Name = "lblExportFormat";
            this.lblExportFormat.Size = new System.Drawing.Size(72, 13);
            this.lblExportFormat.TabIndex = 2;
            this.lblExportFormat.Text = "Export Format";
            // 
            // cboExportConfig
            // 
            this.cboExportConfig.FormattingEnabled = true;
            this.cboExportConfig.Location = new System.Drawing.Point(198, 18);
            this.cboExportConfig.Name = "cboExportConfig";
            this.cboExportConfig.Size = new System.Drawing.Size(121, 21);
            this.cboExportConfig.TabIndex = 10;
            // 
            // ExportLayoutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(617, 415);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportLayoutDialog";
            this.Text = "Export Instrument Layout";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxFretsOptions.ResumeLayout(false);
            this.gbxFretsOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxFretColor)).EndInit();
            this.flpExtendDirection.ResumeLayout(false);
            this.flpExtendDirection.PerformLayout();
            this.flpExportFormat.ResumeLayout(false);
            this.flpExportFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private LayoutViewer layoutPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkExportStrings;
        private System.Windows.Forms.CheckBox chkExportFrets;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Label lblExportFormat;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.CheckBox chkExportFingerboard;
        private System.Windows.Forms.FlowLayoutPanel flpExtendDirection;
        private System.Windows.Forms.RadioButton rbExtendInward;
        private System.Windows.Forms.RadioButton rbExtendOutward;
        private System.Windows.Forms.FlowLayoutPanel flpExportFormat;
        private System.Windows.Forms.CheckBox chkExtendFretSlots;
        private MeasureTextbox mtbFretExtendAmount;
        private System.Windows.Forms.GroupBox gbxFretsOptions;
        private System.Windows.Forms.Label lblExtendAmount;
        private MeasureTextbox mtbFretThickness;
        private System.Windows.Forms.CheckBox chkFretThickness;
        private System.Windows.Forms.Button btnPickFretColor;
        private System.Windows.Forms.PictureBox pbxFretColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkExportStringCenters;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ComboBox cboExportConfig;
    }
}