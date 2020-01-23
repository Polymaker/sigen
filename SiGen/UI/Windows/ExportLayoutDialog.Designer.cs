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
            this.collapsiblePanel3 = new SiGen.UI.Controls.CollapsiblePanel();
            this.lineExportEdit5 = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportMidlinesPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.lineExportEdit4 = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportStringsPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.StringLinesCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportFretsPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.FretLinesCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportFingerboardPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.FretboardCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.collapsiblePanel1 = new SiGen.UI.Controls.CollapsiblePanel();
            this.lblExportFormat = new System.Windows.Forms.Label();
            this.flpExportFormat = new System.Windows.Forms.FlowLayoutPanel();
            this.rbSvgExport = new System.Windows.Forms.RadioButton();
            this.rbDxfExport = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkExportMargins = new System.Windows.Forms.CheckBox();
            this.chkExportCenterLine = new System.Windows.Forms.CheckBox();
            this.gbxFretsOptions = new System.Windows.Forms.GroupBox();
            this.pbxFretColor = new System.Windows.Forms.PictureBox();
            this.btnPickFretColor = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.mtbFretThickness = new SiGen.UI.Controls.MeasureTextbox();
            this.chkFretThickness = new System.Windows.Forms.CheckBox();
            this.lblExtendAmount = new System.Windows.Forms.Label();
            this.mtbFretExtendAmount = new SiGen.UI.Controls.MeasureTextbox();
            this.flpExtendDirection = new System.Windows.Forms.FlowLayoutPanel();
            this.rbExtendInward = new System.Windows.Forms.RadioButton();
            this.rbExtendOutward = new System.Windows.Forms.RadioButton();
            this.chkExtendFretSlots = new System.Windows.Forms.CheckBox();
            this.layoutPreview = new SiGen.UI.LayoutViewer();
            this.btnExport = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.collapsiblePanel3.ContentPanel.SuspendLayout();
            this.collapsiblePanel3.SuspendLayout();
            this.ExportMidlinesPanel.ContentPanel.SuspendLayout();
            this.ExportMidlinesPanel.SuspendLayout();
            this.ExportStringsPanel.ContentPanel.SuspendLayout();
            this.ExportStringsPanel.SuspendLayout();
            this.ExportFretsPanel.ContentPanel.SuspendLayout();
            this.ExportFretsPanel.SuspendLayout();
            this.ExportFingerboardPanel.ContentPanel.SuspendLayout();
            this.ExportFingerboardPanel.SuspendLayout();
            this.collapsiblePanel1.ContentPanel.SuspendLayout();
            this.collapsiblePanel1.SuspendLayout();
            this.flpExportFormat.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbxFretsOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxFretColor)).BeginInit();
            this.flpExtendDirection.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(427, 455);
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
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.collapsiblePanel3);
            this.splitContainer1.Panel1.Controls.Add(this.ExportMidlinesPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportStringsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportFretsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportFingerboardPanel);
            this.splitContainer1.Panel1.Controls.Add(this.collapsiblePanel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel2.Controls.Add(this.layoutPreview);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnExport);
            this.splitContainer1.Size = new System.Drawing.Size(814, 481);
            this.splitContainer1.SplitterDistance = 305;
            this.splitContainer1.TabIndex = 0;
            // 
            // collapsiblePanel3
            // 
            // 
            // collapsiblePanel3.ContentPanel
            // 
            this.collapsiblePanel3.ContentPanel.Controls.Add(this.lineExportEdit5);
            this.collapsiblePanel3.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.collapsiblePanel3.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel3.ContentPanel.TabIndex = 0;
            this.collapsiblePanel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel3.Location = new System.Drawing.Point(0, 368);
            this.collapsiblePanel3.Name = "collapsiblePanel3";
            this.collapsiblePanel3.PanelHeight = 73;
            this.collapsiblePanel3.ShowCheckBox = true;
            this.collapsiblePanel3.Size = new System.Drawing.Size(305, 96);
            this.collapsiblePanel3.TabIndex = 18;
            this.collapsiblePanel3.Text = "Export Frets";
            // 
            // lineExportEdit5
            // 
            this.lineExportEdit5.AutoSize = true;
            this.lineExportEdit5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lineExportEdit5.LineConfig = null;
            this.lineExportEdit5.Location = new System.Drawing.Point(0, 0);
            this.lineExportEdit5.Name = "lineExportEdit5";
            this.lineExportEdit5.Size = new System.Drawing.Size(186, 53);
            this.lineExportEdit5.TabIndex = 2;
            // 
            // ExportMidlinesPanel
            // 
            this.ExportMidlinesPanel.AutoSizeHeight = true;
            // 
            // ExportMidlinesPanel.ContentPanel
            // 
            this.ExportMidlinesPanel.ContentPanel.Controls.Add(this.lineExportEdit4);
            this.ExportMidlinesPanel.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.ExportMidlinesPanel.ContentPanel.Name = "ContentPanel";
            this.ExportMidlinesPanel.ContentPanel.TabIndex = 0;
            this.ExportMidlinesPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ExportMidlinesPanel.Location = new System.Drawing.Point(0, 289);
            this.ExportMidlinesPanel.Name = "ExportMidlinesPanel";
            this.ExportMidlinesPanel.PanelHeight = 56;
            this.ExportMidlinesPanel.ShowCheckBox = true;
            this.ExportMidlinesPanel.Size = new System.Drawing.Size(305, 79);
            this.ExportMidlinesPanel.TabIndex = 17;
            this.ExportMidlinesPanel.Text = "Export String centers";
            this.ExportMidlinesPanel.CheckedChanged += new System.EventHandler(this.ExportMidlinesPanel_CheckedChanged);
            // 
            // lineExportEdit4
            // 
            this.lineExportEdit4.AutoSize = true;
            this.lineExportEdit4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.lineExportEdit4.LineConfig = null;
            this.lineExportEdit4.Location = new System.Drawing.Point(0, 0);
            this.lineExportEdit4.Name = "lineExportEdit4";
            this.lineExportEdit4.Size = new System.Drawing.Size(186, 53);
            this.lineExportEdit4.TabIndex = 2;
            // 
            // ExportStringsPanel
            // 
            this.ExportStringsPanel.AutoSizeHeight = true;
            // 
            // ExportStringsPanel.ContentPanel
            // 
            this.ExportStringsPanel.ContentPanel.Controls.Add(this.StringLinesCfgEdit);
            this.ExportStringsPanel.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.ExportStringsPanel.ContentPanel.Name = "ContentPanel";
            this.ExportStringsPanel.ContentPanel.TabIndex = 0;
            this.ExportStringsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ExportStringsPanel.Location = new System.Drawing.Point(0, 210);
            this.ExportStringsPanel.Name = "ExportStringsPanel";
            this.ExportStringsPanel.PanelHeight = 56;
            this.ExportStringsPanel.ShowCheckBox = true;
            this.ExportStringsPanel.Size = new System.Drawing.Size(305, 79);
            this.ExportStringsPanel.TabIndex = 15;
            this.ExportStringsPanel.Text = "Export Strings";
            this.ExportStringsPanel.CheckedChanged += new System.EventHandler(this.ExportStringsPanel_CheckedChanged);
            // 
            // StringLinesCfgEdit
            // 
            this.StringLinesCfgEdit.AutoSize = true;
            this.StringLinesCfgEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.StringLinesCfgEdit.LineConfig = null;
            this.StringLinesCfgEdit.Location = new System.Drawing.Point(0, 0);
            this.StringLinesCfgEdit.Name = "StringLinesCfgEdit";
            this.StringLinesCfgEdit.Size = new System.Drawing.Size(186, 53);
            this.StringLinesCfgEdit.TabIndex = 1;
            // 
            // ExportFretsPanel
            // 
            this.ExportFretsPanel.AutoSizeHeight = true;
            // 
            // ExportFretsPanel.ContentPanel
            // 
            this.ExportFretsPanel.ContentPanel.Controls.Add(this.FretLinesCfgEdit);
            this.ExportFretsPanel.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.ExportFretsPanel.ContentPanel.Name = "ContentPanel";
            this.ExportFretsPanel.ContentPanel.TabIndex = 0;
            this.ExportFretsPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ExportFretsPanel.Location = new System.Drawing.Point(0, 131);
            this.ExportFretsPanel.Name = "ExportFretsPanel";
            this.ExportFretsPanel.PanelHeight = 56;
            this.ExportFretsPanel.ShowCheckBox = true;
            this.ExportFretsPanel.Size = new System.Drawing.Size(305, 79);
            this.ExportFretsPanel.TabIndex = 16;
            this.ExportFretsPanel.Text = "Export Frets";
            this.ExportFretsPanel.CheckedChanged += new System.EventHandler(this.ExportFretsPanel_CheckedChanged);
            // 
            // FretLinesCfgEdit
            // 
            this.FretLinesCfgEdit.AutoSize = true;
            this.FretLinesCfgEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FretLinesCfgEdit.LineConfig = null;
            this.FretLinesCfgEdit.Location = new System.Drawing.Point(0, 0);
            this.FretLinesCfgEdit.Name = "FretLinesCfgEdit";
            this.FretLinesCfgEdit.Size = new System.Drawing.Size(186, 53);
            this.FretLinesCfgEdit.TabIndex = 2;
            // 
            // ExportFingerboardPanel
            // 
            this.ExportFingerboardPanel.AutoSizeHeight = true;
            // 
            // ExportFingerboardPanel.ContentPanel
            // 
            this.ExportFingerboardPanel.ContentPanel.Controls.Add(this.FretboardCfgEdit);
            this.ExportFingerboardPanel.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.ExportFingerboardPanel.ContentPanel.Name = "ContentPanel";
            this.ExportFingerboardPanel.ContentPanel.TabIndex = 0;
            this.ExportFingerboardPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ExportFingerboardPanel.Location = new System.Drawing.Point(0, 52);
            this.ExportFingerboardPanel.Name = "ExportFingerboardPanel";
            this.ExportFingerboardPanel.PanelHeight = 56;
            this.ExportFingerboardPanel.ShowCheckBox = true;
            this.ExportFingerboardPanel.Size = new System.Drawing.Size(305, 79);
            this.ExportFingerboardPanel.TabIndex = 14;
            this.ExportFingerboardPanel.Text = "Export Fingerboard";
            this.ExportFingerboardPanel.CheckedChanged += new System.EventHandler(this.ExportFingerboardPanel_CheckedChanged);
            // 
            // FretboardCfgEdit
            // 
            this.FretboardCfgEdit.AutoSize = true;
            this.FretboardCfgEdit.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FretboardCfgEdit.LineConfig = null;
            this.FretboardCfgEdit.Location = new System.Drawing.Point(0, 0);
            this.FretboardCfgEdit.Name = "FretboardCfgEdit";
            this.FretboardCfgEdit.Size = new System.Drawing.Size(186, 53);
            this.FretboardCfgEdit.TabIndex = 0;
            // 
            // collapsiblePanel1
            // 
            this.collapsiblePanel1.AutoSizeHeight = true;
            // 
            // collapsiblePanel1.ContentPanel
            // 
            this.collapsiblePanel1.ContentPanel.Controls.Add(this.lblExportFormat);
            this.collapsiblePanel1.ContentPanel.Controls.Add(this.flpExportFormat);
            this.collapsiblePanel1.ContentPanel.Location = new System.Drawing.Point(0, 23);
            this.collapsiblePanel1.ContentPanel.Name = "ContentPanel";
            this.collapsiblePanel1.ContentPanel.TabIndex = 0;
            this.collapsiblePanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.collapsiblePanel1.Location = new System.Drawing.Point(0, 0);
            this.collapsiblePanel1.Name = "collapsiblePanel1";
            this.collapsiblePanel1.PanelHeight = 29;
            this.collapsiblePanel1.Size = new System.Drawing.Size(305, 52);
            this.collapsiblePanel1.TabIndex = 13;
            this.collapsiblePanel1.Text = "Export Parameters";
            // 
            // lblExportFormat
            // 
            this.lblExportFormat.AutoSize = true;
            this.lblExportFormat.Location = new System.Drawing.Point(6, 8);
            this.lblExportFormat.Name = "lblExportFormat";
            this.lblExportFormat.Size = new System.Drawing.Size(72, 13);
            this.lblExportFormat.TabIndex = 2;
            this.lblExportFormat.Text = "Export Format";
            // 
            // flpExportFormat
            // 
            this.flpExportFormat.AutoSize = true;
            this.flpExportFormat.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpExportFormat.Controls.Add(this.rbSvgExport);
            this.flpExportFormat.Controls.Add(this.rbDxfExport);
            this.flpExportFormat.Location = new System.Drawing.Point(84, 3);
            this.flpExportFormat.Name = "flpExportFormat";
            this.flpExportFormat.Size = new System.Drawing.Size(105, 23);
            this.flpExportFormat.TabIndex = 8;
            // 
            // rbSvgExport
            // 
            this.rbSvgExport.AutoSize = true;
            this.rbSvgExport.Checked = true;
            this.rbSvgExport.Location = new System.Drawing.Point(3, 3);
            this.rbSvgExport.Name = "rbSvgExport";
            this.rbSvgExport.Size = new System.Drawing.Size(47, 17);
            this.rbSvgExport.TabIndex = 0;
            this.rbSvgExport.TabStop = true;
            this.rbSvgExport.Text = "SVG";
            this.rbSvgExport.UseVisualStyleBackColor = true;
            // 
            // rbDxfExport
            // 
            this.rbDxfExport.AutoSize = true;
            this.rbDxfExport.Location = new System.Drawing.Point(56, 3);
            this.rbDxfExport.Name = "rbDxfExport";
            this.rbDxfExport.Size = new System.Drawing.Size(46, 17);
            this.rbDxfExport.TabIndex = 1;
            this.rbDxfExport.Text = "DXF";
            this.rbDxfExport.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkExportMargins);
            this.groupBox1.Controls.Add(this.chkExportCenterLine);
            this.groupBox1.Controls.Add(this.gbxFretsOptions);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(472, 172);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export Options";
            this.groupBox1.Visible = false;
            // 
            // chkExportMargins
            // 
            this.chkExportMargins.AutoSize = true;
            this.chkExportMargins.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportMargins.Location = new System.Drawing.Point(11, 135);
            this.chkExportMargins.Name = "chkExportMargins";
            this.chkExportMargins.Size = new System.Drawing.Size(155, 17);
            this.chkExportMargins.TabIndex = 12;
            this.chkExportMargins.Text = "Export Fingerboard Margins";
            this.chkExportMargins.UseVisualStyleBackColor = true;
            this.chkExportMargins.CheckedChanged += new System.EventHandler(this.chkExportMargins_CheckedChanged);
            // 
            // chkExportCenterLine
            // 
            this.chkExportCenterLine.AutoSize = true;
            this.chkExportCenterLine.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportCenterLine.Location = new System.Drawing.Point(11, 112);
            this.chkExportCenterLine.Name = "chkExportCenterLine";
            this.chkExportCenterLine.Size = new System.Drawing.Size(113, 17);
            this.chkExportCenterLine.TabIndex = 11;
            this.chkExportCenterLine.Text = "Export Center Line";
            this.chkExportCenterLine.UseVisualStyleBackColor = true;
            this.chkExportCenterLine.CheckedChanged += new System.EventHandler(this.chkExportCenterLine_CheckedChanged);
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
            this.gbxFretsOptions.Controls.Add(this.flpExtendDirection);
            this.gbxFretsOptions.Controls.Add(this.chkExtendFretSlots);
            this.gbxFretsOptions.Location = new System.Drawing.Point(195, 13);
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
            this.rbExtendInward.CheckedChanged += new System.EventHandler(this.rbExtendInward_CheckedChanged);
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
            // layoutPreview
            // 
            this.layoutPreview.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.layoutPreview.BackColor = System.Drawing.SystemColors.Window;
            this.layoutPreview.DisplayConfig.ShowCenterLine = false;
            this.layoutPreview.DisplayConfig.ShowFingerboard = true;
            this.layoutPreview.DisplayConfig.ShowFrets = true;
            this.layoutPreview.DisplayConfig.ShowMargins = true;
            this.layoutPreview.DisplayConfig.ShowMidlines = true;
            this.layoutPreview.DisplayConfig.ShowStrings = true;
            this.layoutPreview.Location = new System.Drawing.Point(0, 0);
            this.layoutPreview.Name = "layoutPreview";
            this.layoutPreview.Size = new System.Drawing.Size(505, 449);
            this.layoutPreview.TabIndex = 0;
            this.layoutPreview.Text = "layoutViewer1";
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.Location = new System.Drawing.Point(346, 455);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 4;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ExportLayoutDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(814, 481);
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportLayoutDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Export Instrument Layout";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.collapsiblePanel3.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel3.ContentPanel.PerformLayout();
            this.collapsiblePanel3.ResumeLayout(false);
            this.ExportMidlinesPanel.ContentPanel.ResumeLayout(false);
            this.ExportMidlinesPanel.ContentPanel.PerformLayout();
            this.ExportMidlinesPanel.ResumeLayout(false);
            this.ExportMidlinesPanel.PerformLayout();
            this.ExportStringsPanel.ContentPanel.ResumeLayout(false);
            this.ExportStringsPanel.ContentPanel.PerformLayout();
            this.ExportStringsPanel.ResumeLayout(false);
            this.ExportStringsPanel.PerformLayout();
            this.ExportFretsPanel.ContentPanel.ResumeLayout(false);
            this.ExportFretsPanel.ContentPanel.PerformLayout();
            this.ExportFretsPanel.ResumeLayout(false);
            this.ExportFretsPanel.PerformLayout();
            this.ExportFingerboardPanel.ContentPanel.ResumeLayout(false);
            this.ExportFingerboardPanel.ContentPanel.PerformLayout();
            this.ExportFingerboardPanel.ResumeLayout(false);
            this.ExportFingerboardPanel.PerformLayout();
            this.collapsiblePanel1.ContentPanel.ResumeLayout(false);
            this.collapsiblePanel1.ContentPanel.PerformLayout();
            this.collapsiblePanel1.ResumeLayout(false);
            this.collapsiblePanel1.PerformLayout();
            this.flpExportFormat.ResumeLayout(false);
            this.flpExportFormat.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbxFretsOptions.ResumeLayout(false);
            this.gbxFretsOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxFretColor)).EndInit();
            this.flpExtendDirection.ResumeLayout(false);
            this.flpExtendDirection.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private LayoutViewer layoutPreview;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbSvgExport;
        private System.Windows.Forms.RadioButton rbDxfExport;
        private System.Windows.Forms.Label lblExportFormat;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.FlowLayoutPanel flpExtendDirection;
        private System.Windows.Forms.RadioButton rbExtendInward;
        private System.Windows.Forms.RadioButton rbExtendOutward;
        private System.Windows.Forms.FlowLayoutPanel flpExportFormat;
        private System.Windows.Forms.CheckBox chkExtendFretSlots;
        private SiGen.UI.Controls.MeasureTextbox mtbFretExtendAmount;
        private System.Windows.Forms.GroupBox gbxFretsOptions;
        private System.Windows.Forms.Label lblExtendAmount;
        private SiGen.UI.Controls.MeasureTextbox mtbFretThickness;
        private System.Windows.Forms.CheckBox chkFretThickness;
        private System.Windows.Forms.Button btnPickFretColor;
        private System.Windows.Forms.PictureBox pbxFretColor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chkExportCenterLine;
        private System.Windows.Forms.CheckBox chkExportMargins;
        private Controls.CollapsiblePanel collapsiblePanel1;
        private Controls.CollapsiblePanel ExportFingerboardPanel;
        private Controls.CollapsiblePanel ExportFretsPanel;
        private Controls.CollapsiblePanel ExportStringsPanel;
        private Controls.Preferences.LineExportEdit FretboardCfgEdit;
        private Controls.Preferences.LineExportEdit StringLinesCfgEdit;
        private Controls.Preferences.LineExportEdit FretLinesCfgEdit;
        private Controls.CollapsiblePanel collapsiblePanel3;
        private Controls.Preferences.LineExportEdit lineExportEdit5;
        private Controls.CollapsiblePanel ExportMidlinesPanel;
        private Controls.Preferences.LineExportEdit lineExportEdit4;
    }
}