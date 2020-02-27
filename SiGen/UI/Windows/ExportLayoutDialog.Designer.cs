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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExportLayoutDialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ExportMidlinesPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.MidlinesCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportStringsPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.StringLinesCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportCenterLinePanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.CenterLineCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExtendFretsPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.lblExtendAmount = new System.Windows.Forms.Label();
            this.flpExtendDirection = new System.Windows.Forms.FlowLayoutPanel();
            this.rbExtendInward = new System.Windows.Forms.RadioButton();
            this.rbExtendOutward = new System.Windows.Forms.RadioButton();
            this.mtbFretExtendAmount = new SiGen.UI.Controls.MeasureTextbox();
            this.ExportFretsPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.FretLinesCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportMarginsPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.MarginsCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.ExportFingerboardPanel = new SiGen.UI.Controls.CollapsiblePanel();
            this.ContinueEdgesCheckbox = new System.Windows.Forms.CheckBox();
            this.FretboardCfgEdit = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.lblExportFormat = new System.Windows.Forms.Label();
            this.flpExportFormat = new System.Windows.Forms.FlowLayoutPanel();
            this.rbSvgExport = new System.Windows.Forms.RadioButton();
            this.rbDxfExport = new System.Windows.Forms.RadioButton();
            this.layoutPreview = new SiGen.UI.LayoutViewer();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.ExportMidlinesPanel.ContentPanel.SuspendLayout();
            this.ExportMidlinesPanel.SuspendLayout();
            this.ExportStringsPanel.ContentPanel.SuspendLayout();
            this.ExportStringsPanel.SuspendLayout();
            this.ExportCenterLinePanel.ContentPanel.SuspendLayout();
            this.ExportCenterLinePanel.SuspendLayout();
            this.ExtendFretsPanel.ContentPanel.SuspendLayout();
            this.ExtendFretsPanel.SuspendLayout();
            this.flpExtendDirection.SuspendLayout();
            this.ExportFretsPanel.ContentPanel.SuspendLayout();
            this.ExportFretsPanel.SuspendLayout();
            this.ExportMarginsPanel.ContentPanel.SuspendLayout();
            this.ExportMarginsPanel.SuspendLayout();
            this.ExportFingerboardPanel.ContentPanel.SuspendLayout();
            this.ExportFingerboardPanel.SuspendLayout();
            this.flpExportFormat.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.ExportMidlinesPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportStringsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportCenterLinePanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExtendFretsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportFretsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportMarginsPanel);
            this.splitContainer1.Panel1.Controls.Add(this.ExportFingerboardPanel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lblExportFormat);
            this.splitContainer1.Panel2.Controls.Add(this.flpExportFormat);
            this.splitContainer1.Panel2.Controls.Add(this.layoutPreview);
            this.splitContainer1.Panel2.Controls.Add(this.btnCancel);
            this.splitContainer1.Panel2.Controls.Add(this.btnExport);
            // 
            // ExportMidlinesPanel
            // 
            // 
            // ExportMidlinesPanel.ContentPanel
            // 
            this.ExportMidlinesPanel.ContentPanel.Controls.Add(this.MidlinesCfgEdit);
            resources.ApplyResources(this.ExportMidlinesPanel.ContentPanel, "ExportMidlinesPanel.ContentPanel");
            this.ExportMidlinesPanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExportMidlinesPanel, "ExportMidlinesPanel");
            this.ExportMidlinesPanel.Name = "ExportMidlinesPanel";
            this.ExportMidlinesPanel.PanelHeight = 56;
            this.ExportMidlinesPanel.ShowCheckBox = true;
            this.ExportMidlinesPanel.CheckedChanged += new System.EventHandler(this.ExportMidlinesPanel_CheckedChanged);
            // 
            // MidlinesCfgEdit
            // 
            resources.ApplyResources(this.MidlinesCfgEdit, "MidlinesCfgEdit");
            this.MidlinesCfgEdit.LineConfig = null;
            this.MidlinesCfgEdit.Name = "MidlinesCfgEdit";
            // 
            // ExportStringsPanel
            // 
            this.ExportStringsPanel.AutoSizeHeight = true;
            // 
            // ExportStringsPanel.ContentPanel
            // 
            this.ExportStringsPanel.ContentPanel.Controls.Add(this.StringLinesCfgEdit);
            resources.ApplyResources(this.ExportStringsPanel.ContentPanel, "ExportStringsPanel.ContentPanel");
            this.ExportStringsPanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExportStringsPanel, "ExportStringsPanel");
            this.ExportStringsPanel.Name = "ExportStringsPanel";
            this.ExportStringsPanel.PanelHeight = 56;
            this.ExportStringsPanel.ShowCheckBox = true;
            this.ExportStringsPanel.CheckedChanged += new System.EventHandler(this.ExportStringsPanel_CheckedChanged);
            // 
            // StringLinesCfgEdit
            // 
            resources.ApplyResources(this.StringLinesCfgEdit, "StringLinesCfgEdit");
            this.StringLinesCfgEdit.LineConfig = null;
            this.StringLinesCfgEdit.Name = "StringLinesCfgEdit";
            // 
            // ExportCenterLinePanel
            // 
            this.ExportCenterLinePanel.Collapsed = true;
            // 
            // ExportCenterLinePanel.ContentPanel
            // 
            this.ExportCenterLinePanel.ContentPanel.Controls.Add(this.CenterLineCfgEdit);
            resources.ApplyResources(this.ExportCenterLinePanel.ContentPanel, "ExportCenterLinePanel.ContentPanel");
            this.ExportCenterLinePanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExportCenterLinePanel, "ExportCenterLinePanel");
            this.ExportCenterLinePanel.Name = "ExportCenterLinePanel";
            this.ExportCenterLinePanel.PanelHeight = 56;
            this.ExportCenterLinePanel.ShowCheckBox = true;
            this.ExportCenterLinePanel.CheckedChanged += new System.EventHandler(this.ExportCenterLinePanel_CheckedChanged);
            // 
            // CenterLineCfgEdit
            // 
            resources.ApplyResources(this.CenterLineCfgEdit, "CenterLineCfgEdit");
            this.CenterLineCfgEdit.LineConfig = null;
            this.CenterLineCfgEdit.Name = "CenterLineCfgEdit";
            // 
            // ExtendFretsPanel
            // 
            // 
            // ExtendFretsPanel.ContentPanel
            // 
            this.ExtendFretsPanel.ContentPanel.Controls.Add(this.lblExtendAmount);
            this.ExtendFretsPanel.ContentPanel.Controls.Add(this.flpExtendDirection);
            this.ExtendFretsPanel.ContentPanel.Controls.Add(this.mtbFretExtendAmount);
            resources.ApplyResources(this.ExtendFretsPanel.ContentPanel, "ExtendFretsPanel.ContentPanel");
            this.ExtendFretsPanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExtendFretsPanel, "ExtendFretsPanel");
            this.ExtendFretsPanel.Name = "ExtendFretsPanel";
            this.ExtendFretsPanel.PanelHeight = 34;
            this.ExtendFretsPanel.ShowCheckBox = true;
            this.ExtendFretsPanel.CheckedChanged += new System.EventHandler(this.ExtendFretsPanel_CheckedChanged);
            // 
            // lblExtendAmount
            // 
            resources.ApplyResources(this.lblExtendAmount, "lblExtendAmount");
            this.lblExtendAmount.Name = "lblExtendAmount";
            // 
            // flpExtendDirection
            // 
            resources.ApplyResources(this.flpExtendDirection, "flpExtendDirection");
            this.flpExtendDirection.Controls.Add(this.rbExtendInward);
            this.flpExtendDirection.Controls.Add(this.rbExtendOutward);
            this.flpExtendDirection.Name = "flpExtendDirection";
            // 
            // rbExtendInward
            // 
            resources.ApplyResources(this.rbExtendInward, "rbExtendInward");
            this.rbExtendInward.Checked = true;
            this.rbExtendInward.Name = "rbExtendInward";
            this.rbExtendInward.TabStop = true;
            this.rbExtendInward.UseVisualStyleBackColor = true;
            this.rbExtendInward.CheckedChanged += new System.EventHandler(this.rbExtendInward_CheckedChanged);
            // 
            // rbExtendOutward
            // 
            resources.ApplyResources(this.rbExtendOutward, "rbExtendOutward");
            this.rbExtendOutward.Name = "rbExtendOutward";
            this.rbExtendOutward.UseVisualStyleBackColor = true;
            // 
            // mtbFretExtendAmount
            // 
            resources.ApplyResources(this.mtbFretExtendAmount, "mtbFretExtendAmount");
            this.mtbFretExtendAmount.Name = "mtbFretExtendAmount";
            this.mtbFretExtendAmount.ValueChanged += new System.EventHandler(this.mtbFretExtendAmount_ValueChanged);
            // 
            // ExportFretsPanel
            // 
            // 
            // ExportFretsPanel.ContentPanel
            // 
            this.ExportFretsPanel.ContentPanel.Controls.Add(this.FretLinesCfgEdit);
            resources.ApplyResources(this.ExportFretsPanel.ContentPanel, "ExportFretsPanel.ContentPanel");
            this.ExportFretsPanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExportFretsPanel, "ExportFretsPanel");
            this.ExportFretsPanel.Name = "ExportFretsPanel";
            this.ExportFretsPanel.PanelHeight = 56;
            this.ExportFretsPanel.ShowCheckBox = true;
            this.ExportFretsPanel.CheckedChanged += new System.EventHandler(this.ExportFretsPanel_CheckedChanged);
            // 
            // FretLinesCfgEdit
            // 
            resources.ApplyResources(this.FretLinesCfgEdit, "FretLinesCfgEdit");
            this.FretLinesCfgEdit.LineConfig = null;
            this.FretLinesCfgEdit.Name = "FretLinesCfgEdit";
            // 
            // ExportMarginsPanel
            // 
            this.ExportMarginsPanel.Collapsed = true;
            // 
            // ExportMarginsPanel.ContentPanel
            // 
            this.ExportMarginsPanel.ContentPanel.Controls.Add(this.MarginsCfgEdit);
            resources.ApplyResources(this.ExportMarginsPanel.ContentPanel, "ExportMarginsPanel.ContentPanel");
            this.ExportMarginsPanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExportMarginsPanel, "ExportMarginsPanel");
            this.ExportMarginsPanel.Name = "ExportMarginsPanel";
            this.ExportMarginsPanel.PanelHeight = 56;
            this.ExportMarginsPanel.ShowCheckBox = true;
            this.ExportMarginsPanel.CheckedChanged += new System.EventHandler(this.ExportMarginsPanel_CheckedChanged);
            // 
            // MarginsCfgEdit
            // 
            resources.ApplyResources(this.MarginsCfgEdit, "MarginsCfgEdit");
            this.MarginsCfgEdit.LineConfig = null;
            this.MarginsCfgEdit.Name = "MarginsCfgEdit";
            // 
            // ExportFingerboardPanel
            // 
            // 
            // ExportFingerboardPanel.ContentPanel
            // 
            this.ExportFingerboardPanel.ContentPanel.Controls.Add(this.ContinueEdgesCheckbox);
            this.ExportFingerboardPanel.ContentPanel.Controls.Add(this.FretboardCfgEdit);
            resources.ApplyResources(this.ExportFingerboardPanel.ContentPanel, "ExportFingerboardPanel.ContentPanel");
            this.ExportFingerboardPanel.ContentPanel.Name = "ContentPanel";
            resources.ApplyResources(this.ExportFingerboardPanel, "ExportFingerboardPanel");
            this.ExportFingerboardPanel.Name = "ExportFingerboardPanel";
            this.ExportFingerboardPanel.PanelHeight = 76;
            this.ExportFingerboardPanel.ShowCheckBox = true;
            this.ExportFingerboardPanel.CheckedChanged += new System.EventHandler(this.ExportFingerboardPanel_CheckedChanged);
            // 
            // ContinueEdgesCheckbox
            // 
            resources.ApplyResources(this.ContinueEdgesCheckbox, "ContinueEdgesCheckbox");
            this.ContinueEdgesCheckbox.Name = "ContinueEdgesCheckbox";
            this.ContinueEdgesCheckbox.UseVisualStyleBackColor = true;
            this.ContinueEdgesCheckbox.CheckedChanged += new System.EventHandler(this.ContinueEdgesCheckbox_CheckedChanged);
            // 
            // FretboardCfgEdit
            // 
            resources.ApplyResources(this.FretboardCfgEdit, "FretboardCfgEdit");
            this.FretboardCfgEdit.LineConfig = null;
            this.FretboardCfgEdit.Name = "FretboardCfgEdit";
            // 
            // lblExportFormat
            // 
            resources.ApplyResources(this.lblExportFormat, "lblExportFormat");
            this.lblExportFormat.Name = "lblExportFormat";
            // 
            // flpExportFormat
            // 
            resources.ApplyResources(this.flpExportFormat, "flpExportFormat");
            this.flpExportFormat.Controls.Add(this.rbSvgExport);
            this.flpExportFormat.Controls.Add(this.rbDxfExport);
            this.flpExportFormat.Name = "flpExportFormat";
            // 
            // rbSvgExport
            // 
            resources.ApplyResources(this.rbSvgExport, "rbSvgExport");
            this.rbSvgExport.Checked = true;
            this.rbSvgExport.Name = "rbSvgExport";
            this.rbSvgExport.TabStop = true;
            this.rbSvgExport.UseVisualStyleBackColor = true;
            // 
            // rbDxfExport
            // 
            resources.ApplyResources(this.rbDxfExport, "rbDxfExport");
            this.rbDxfExport.Name = "rbDxfExport";
            this.rbDxfExport.UseVisualStyleBackColor = true;
            // 
            // layoutPreview
            // 
            resources.ApplyResources(this.layoutPreview, "layoutPreview");
            this.layoutPreview.BackColor = System.Drawing.SystemColors.Window;
            this.layoutPreview.DisplayConfig.FingerboardOrientation = System.Windows.Forms.Orientation.Vertical;
            this.layoutPreview.DisplayConfig.ShowCenterLine = false;
            this.layoutPreview.DisplayConfig.ShowFingerboard = true;
            this.layoutPreview.DisplayConfig.ShowFrets = true;
            this.layoutPreview.DisplayConfig.ShowMargins = true;
            this.layoutPreview.DisplayConfig.ShowMidlines = true;
            this.layoutPreview.DisplayConfig.ShowStrings = true;
            this.layoutPreview.Name = "layoutPreview";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // ExportLayoutDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.splitContainer1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ExportLayoutDialog";
            this.ShowIcon = false;
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ExportMidlinesPanel.ContentPanel.ResumeLayout(false);
            this.ExportMidlinesPanel.ContentPanel.PerformLayout();
            this.ExportMidlinesPanel.ResumeLayout(false);
            this.ExportStringsPanel.ContentPanel.ResumeLayout(false);
            this.ExportStringsPanel.ContentPanel.PerformLayout();
            this.ExportStringsPanel.ResumeLayout(false);
            this.ExportStringsPanel.PerformLayout();
            this.ExportCenterLinePanel.ContentPanel.ResumeLayout(false);
            this.ExportCenterLinePanel.ContentPanel.PerformLayout();
            this.ExportCenterLinePanel.ResumeLayout(false);
            this.ExtendFretsPanel.ContentPanel.ResumeLayout(false);
            this.ExtendFretsPanel.ContentPanel.PerformLayout();
            this.ExtendFretsPanel.ResumeLayout(false);
            this.flpExtendDirection.ResumeLayout(false);
            this.flpExtendDirection.PerformLayout();
            this.ExportFretsPanel.ContentPanel.ResumeLayout(false);
            this.ExportFretsPanel.ContentPanel.PerformLayout();
            this.ExportFretsPanel.ResumeLayout(false);
            this.ExportMarginsPanel.ContentPanel.ResumeLayout(false);
            this.ExportMarginsPanel.ContentPanel.PerformLayout();
            this.ExportMarginsPanel.ResumeLayout(false);
            this.ExportFingerboardPanel.ContentPanel.ResumeLayout(false);
            this.ExportFingerboardPanel.ContentPanel.PerformLayout();
            this.ExportFingerboardPanel.ResumeLayout(false);
            this.flpExportFormat.ResumeLayout(false);
            this.flpExportFormat.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private LayoutViewer layoutPreview;
        private System.Windows.Forms.RadioButton rbSvgExport;
        private System.Windows.Forms.RadioButton rbDxfExport;
        private System.Windows.Forms.Label lblExportFormat;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.FlowLayoutPanel flpExtendDirection;
        private System.Windows.Forms.RadioButton rbExtendInward;
        private System.Windows.Forms.RadioButton rbExtendOutward;
        private System.Windows.Forms.FlowLayoutPanel flpExportFormat;
        private SiGen.UI.Controls.MeasureTextbox mtbFretExtendAmount;
        private System.Windows.Forms.Label lblExtendAmount;
        private Controls.CollapsiblePanel ExportFingerboardPanel;
        private Controls.CollapsiblePanel ExportFretsPanel;
        private Controls.CollapsiblePanel ExportStringsPanel;
        private Controls.Preferences.LineExportEdit FretboardCfgEdit;
        private Controls.Preferences.LineExportEdit StringLinesCfgEdit;
        private Controls.Preferences.LineExportEdit FretLinesCfgEdit;
        private Controls.CollapsiblePanel ExportCenterLinePanel;
        private Controls.Preferences.LineExportEdit CenterLineCfgEdit;
        private Controls.CollapsiblePanel ExportMidlinesPanel;
        private Controls.Preferences.LineExportEdit MidlinesCfgEdit;
        private System.Windows.Forms.CheckBox ContinueEdgesCheckbox;
        private Controls.CollapsiblePanel ExtendFretsPanel;
        private Controls.CollapsiblePanel ExportMarginsPanel;
        private Controls.Preferences.LineExportEdit MarginsCfgEdit;
    }
}