namespace SiGen.UI.Controls.LayoutEditors
{
    partial class LayoutViewerPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutViewerPanel));
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.ToolStripDisplayOptions = new System.Windows.Forms.ToolStrip();
            this.DisplayOptionsDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.DisplayStringsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayStringCentersMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayFretsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayMarginsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayFingerboardMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DisplayCenterLineMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ResetCameraButton = new System.Windows.Forms.ToolStripButton();
            this.ZoomToolstripLabel = new System.Windows.Forms.ToolStripLabel();
            this.tsbDisplayOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMeasureTool = new System.Windows.Forms.ToolStripButton();
            this.layoutViewer1 = new SiGen.UI.LayoutViewer();
            this.cmsDocumentTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCloseLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseRight = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAS = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOpenFileDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.ToolStripDisplayOptions.SuspendLayout();
            this.cmsDocumentTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.BottomToolStripPanel
            // 
            this.toolStripContainer1.BottomToolStripPanel.Controls.Add(this.ToolStripDisplayOptions);
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.layoutViewer1);
            resources.ApplyResources(this.toolStripContainer1.ContentPanel, "toolStripContainer1.ContentPanel");
            resources.ApplyResources(this.toolStripContainer1, "toolStripContainer1");
            this.toolStripContainer1.Name = "toolStripContainer1";
            // 
            // ToolStripDisplayOptions
            // 
            resources.ApplyResources(this.ToolStripDisplayOptions, "ToolStripDisplayOptions");
            this.ToolStripDisplayOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripDisplayOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplayOptionsDropDown,
            this.ResetCameraButton,
            this.ZoomToolstripLabel,
            this.tsbDisplayOptions,
            this.toolStripSeparator3,
            this.tsbMeasureTool});
            this.ToolStripDisplayOptions.Name = "ToolStripDisplayOptions";
            this.ToolStripDisplayOptions.Stretch = true;
            // 
            // DisplayOptionsDropDown
            // 
            this.DisplayOptionsDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.DisplayOptionsDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplayStringsMenuItem,
            this.DisplayStringCentersMenuItem,
            this.DisplayFretsMenuItem,
            this.DisplayMarginsMenuItem,
            this.DisplayFingerboardMenuItem,
            this.DisplayCenterLineMenuItem});
            resources.ApplyResources(this.DisplayOptionsDropDown, "DisplayOptionsDropDown");
            this.DisplayOptionsDropDown.Name = "DisplayOptionsDropDown";
            this.DisplayOptionsDropDown.DropDownOpening += new System.EventHandler(this.DisplayOptionsDropDown_DropDownOpening);
            // 
            // DisplayStringsMenuItem
            // 
            this.DisplayStringsMenuItem.Checked = true;
            this.DisplayStringsMenuItem.CheckOnClick = true;
            this.DisplayStringsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayStringsMenuItem.Name = "DisplayStringsMenuItem";
            resources.ApplyResources(this.DisplayStringsMenuItem, "DisplayStringsMenuItem");
            this.DisplayStringsMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayStringCentersMenuItem
            // 
            this.DisplayStringCentersMenuItem.Checked = true;
            this.DisplayStringCentersMenuItem.CheckOnClick = true;
            this.DisplayStringCentersMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayStringCentersMenuItem.Name = "DisplayStringCentersMenuItem";
            resources.ApplyResources(this.DisplayStringCentersMenuItem, "DisplayStringCentersMenuItem");
            this.DisplayStringCentersMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayFretsMenuItem
            // 
            this.DisplayFretsMenuItem.Checked = true;
            this.DisplayFretsMenuItem.CheckOnClick = true;
            this.DisplayFretsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayFretsMenuItem.Name = "DisplayFretsMenuItem";
            resources.ApplyResources(this.DisplayFretsMenuItem, "DisplayFretsMenuItem");
            this.DisplayFretsMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayMarginsMenuItem
            // 
            this.DisplayMarginsMenuItem.Checked = true;
            this.DisplayMarginsMenuItem.CheckOnClick = true;
            this.DisplayMarginsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayMarginsMenuItem.Name = "DisplayMarginsMenuItem";
            resources.ApplyResources(this.DisplayMarginsMenuItem, "DisplayMarginsMenuItem");
            this.DisplayMarginsMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayFingerboardMenuItem
            // 
            this.DisplayFingerboardMenuItem.Checked = true;
            this.DisplayFingerboardMenuItem.CheckOnClick = true;
            this.DisplayFingerboardMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayFingerboardMenuItem.Name = "DisplayFingerboardMenuItem";
            resources.ApplyResources(this.DisplayFingerboardMenuItem, "DisplayFingerboardMenuItem");
            this.DisplayFingerboardMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayCenterLineMenuItem
            // 
            this.DisplayCenterLineMenuItem.CheckOnClick = true;
            this.DisplayCenterLineMenuItem.Name = "DisplayCenterLineMenuItem";
            resources.ApplyResources(this.DisplayCenterLineMenuItem, "DisplayCenterLineMenuItem");
            this.DisplayCenterLineMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // ResetCameraButton
            // 
            this.ResetCameraButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ResetCameraButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            resources.ApplyResources(this.ResetCameraButton, "ResetCameraButton");
            this.ResetCameraButton.Name = "ResetCameraButton";
            this.ResetCameraButton.Click += new System.EventHandler(this.ResetCameraButton_Click);
            // 
            // ZoomToolstripLabel
            // 
            this.ZoomToolstripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ZoomToolstripLabel.Name = "ZoomToolstripLabel";
            resources.ApplyResources(this.ZoomToolstripLabel, "ZoomToolstripLabel");
            // 
            // tsbDisplayOptions
            // 
            this.tsbDisplayOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDisplayOptions.Image = global::SiGen.Properties.Resources.Settings_x32;
            resources.ApplyResources(this.tsbDisplayOptions, "tsbDisplayOptions");
            this.tsbDisplayOptions.Name = "tsbDisplayOptions";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tsbMeasureTool
            // 
            this.tsbMeasureTool.Checked = true;
            this.tsbMeasureTool.CheckOnClick = true;
            this.tsbMeasureTool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbMeasureTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMeasureTool.Image = global::SiGen.Properties.Resources.Measure_x32;
            resources.ApplyResources(this.tsbMeasureTool, "tsbMeasureTool");
            this.tsbMeasureTool.Name = "tsbMeasureTool";
            this.tsbMeasureTool.CheckedChanged += new System.EventHandler(this.tsbMeasureTool_CheckedChanged);
            // 
            // layoutViewer1
            // 
            this.layoutViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.layoutViewer1.DisplayConfig.ShowCenterLine = false;
            this.layoutViewer1.DisplayConfig.ShowFingerboard = true;
            this.layoutViewer1.DisplayConfig.ShowFrets = true;
            this.layoutViewer1.DisplayConfig.ShowMargins = true;
            this.layoutViewer1.DisplayConfig.ShowMidlines = true;
            this.layoutViewer1.DisplayConfig.ShowStrings = true;
            resources.ApplyResources(this.layoutViewer1, "layoutViewer1");
            this.layoutViewer1.Name = "layoutViewer1";
            this.layoutViewer1.ZoomChanged += new System.EventHandler(this.layoutViewer1_ZoomChanged);
            // 
            // cmsDocumentTab
            // 
            this.cmsDocumentTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiCloseLayout,
            this.tsmiCloseOthers,
            this.tsmiCloseRight,
            this.toolStripSeparator1,
            this.tsmiSave,
            this.tsmiSaveAS,
            this.tsmiRename,
            this.toolStripSeparator2,
            this.tsmiOpenFileDirectory});
            this.cmsDocumentTab.Name = "cmsDocumentTab";
            resources.ApplyResources(this.cmsDocumentTab, "cmsDocumentTab");
            this.cmsDocumentTab.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDocumentTab_Opening);
            // 
            // tsmiCloseLayout
            // 
            this.tsmiCloseLayout.Name = "tsmiCloseLayout";
            resources.ApplyResources(this.tsmiCloseLayout, "tsmiCloseLayout");
            this.tsmiCloseLayout.Click += new System.EventHandler(this.tsmiCloseLayout_Click);
            // 
            // tsmiCloseOthers
            // 
            this.tsmiCloseOthers.Name = "tsmiCloseOthers";
            resources.ApplyResources(this.tsmiCloseOthers, "tsmiCloseOthers");
            this.tsmiCloseOthers.Click += new System.EventHandler(this.tsmiCloseOthers_Click);
            // 
            // tsmiCloseRight
            // 
            this.tsmiCloseRight.Name = "tsmiCloseRight";
            resources.ApplyResources(this.tsmiCloseRight, "tsmiCloseRight");
            this.tsmiCloseRight.Click += new System.EventHandler(this.tsmiCloseRight_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            resources.ApplyResources(this.tsmiSave, "tsmiSave");
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAS
            // 
            this.tsmiSaveAS.Name = "tsmiSaveAS";
            resources.ApplyResources(this.tsmiSaveAS, "tsmiSaveAS");
            this.tsmiSaveAS.Click += new System.EventHandler(this.tsmiSaveAS_Click);
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            resources.ApplyResources(this.tsmiRename, "tsmiRename");
            this.tsmiRename.Click += new System.EventHandler(this.tsmiRename_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsmiOpenFileDirectory
            // 
            this.tsmiOpenFileDirectory.Name = "tsmiOpenFileDirectory";
            resources.ApplyResources(this.tsmiOpenFileDirectory, "tsmiOpenFileDirectory");
            this.tsmiOpenFileDirectory.Click += new System.EventHandler(this.tsmiOpenFileDirectory_Click);
            // 
            // LayoutViewerPanel
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "LayoutViewerPanel";
            this.TabPageContextMenuStrip = this.cmsDocumentTab;
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ToolStripDisplayOptions.ResumeLayout(false);
            this.ToolStripDisplayOptions.PerformLayout();
            this.cmsDocumentTab.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private LayoutViewer layoutViewer1;
        private System.Windows.Forms.ContextMenuStrip cmsDocumentTab;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseLayout;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseRight;
        private System.Windows.Forms.ToolStripMenuItem tsmiCloseOthers;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAS;
        private System.Windows.Forms.ToolStripMenuItem tsmiRename;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFileDirectory;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip ToolStripDisplayOptions;
        private System.Windows.Forms.ToolStripDropDownButton DisplayOptionsDropDown;
        private System.Windows.Forms.ToolStripMenuItem DisplayStringsMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel ZoomToolstripLabel;
        private System.Windows.Forms.ToolStripButton ResetCameraButton;
        private System.Windows.Forms.ToolStripMenuItem DisplayStringCentersMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisplayFretsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisplayMarginsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisplayFingerboardMenuItem;
        private System.Windows.Forms.ToolStripMenuItem DisplayCenterLineMenuItem;
        private System.Windows.Forms.ToolStripButton tsbDisplayOptions;
        private System.Windows.Forms.ToolStripButton tsbMeasureTool;
    }
}