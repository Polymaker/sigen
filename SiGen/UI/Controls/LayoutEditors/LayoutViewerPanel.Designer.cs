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
            this.cmsDocumentTab.SuspendLayout();
            this.toolStripContainer1.BottomToolStripPanel.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.ToolStripDisplayOptions.SuspendLayout();
            this.SuspendLayout();
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
            this.cmsDocumentTab.Size = new System.Drawing.Size(207, 170);
            this.cmsDocumentTab.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDocumentTab_Opening);
            // 
            // tsmiCloseLayout
            // 
            this.tsmiCloseLayout.Name = "tsmiCloseLayout";
            this.tsmiCloseLayout.Size = new System.Drawing.Size(206, 22);
            this.tsmiCloseLayout.Text = "Close layout";
            this.tsmiCloseLayout.Click += new System.EventHandler(this.tsmiCloseLayout_Click);
            // 
            // tsmiCloseOthers
            // 
            this.tsmiCloseOthers.Name = "tsmiCloseOthers";
            this.tsmiCloseOthers.Size = new System.Drawing.Size(206, 22);
            this.tsmiCloseOthers.Text = "Close other layouts";
            this.tsmiCloseOthers.Click += new System.EventHandler(this.tsmiCloseOthers_Click);
            // 
            // tsmiCloseRight
            // 
            this.tsmiCloseRight.Name = "tsmiCloseRight";
            this.tsmiCloseRight.Size = new System.Drawing.Size(206, 22);
            this.tsmiCloseRight.Text = "Close layouts to the right";
            this.tsmiCloseRight.Click += new System.EventHandler(this.tsmiCloseRight_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(203, 6);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.Size = new System.Drawing.Size(206, 22);
            this.tsmiSave.Text = "Save";
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAS
            // 
            this.tsmiSaveAS.Name = "tsmiSaveAS";
            this.tsmiSaveAS.Size = new System.Drawing.Size(206, 22);
            this.tsmiSaveAS.Text = "Save As...";
            this.tsmiSaveAS.Click += new System.EventHandler(this.tsmiSaveAS_Click);
            // 
            // tsmiRename
            // 
            this.tsmiRename.Name = "tsmiRename";
            this.tsmiRename.Size = new System.Drawing.Size(206, 22);
            this.tsmiRename.Text = "Rename";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(203, 6);
            // 
            // tsmiOpenFileDirectory
            // 
            this.tsmiOpenFileDirectory.Name = "tsmiOpenFileDirectory";
            this.tsmiOpenFileDirectory.Size = new System.Drawing.Size(206, 22);
            this.tsmiOpenFileDirectory.Text = "Open file location";
            this.tsmiOpenFileDirectory.Click += new System.EventHandler(this.tsmiOpenFileDirectory_Click);
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
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(577, 311);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(577, 361);
            this.toolStripContainer1.TabIndex = 2;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // ToolStripDisplayOptions
            // 
            this.ToolStripDisplayOptions.Dock = System.Windows.Forms.DockStyle.None;
            this.ToolStripDisplayOptions.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStripDisplayOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.DisplayOptionsDropDown,
            this.ResetCameraButton,
            this.ZoomToolstripLabel,
            this.tsbDisplayOptions,
            this.toolStripSeparator3,
            this.tsbMeasureTool});
            this.ToolStripDisplayOptions.Location = new System.Drawing.Point(0, 0);
            this.ToolStripDisplayOptions.Name = "ToolStripDisplayOptions";
            this.ToolStripDisplayOptions.Size = new System.Drawing.Size(577, 25);
            this.ToolStripDisplayOptions.Stretch = true;
            this.ToolStripDisplayOptions.TabIndex = 0;
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
            this.DisplayOptionsDropDown.Image = ((System.Drawing.Image)(resources.GetObject("DisplayOptionsDropDown.Image")));
            this.DisplayOptionsDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.DisplayOptionsDropDown.Name = "DisplayOptionsDropDown";
            this.DisplayOptionsDropDown.Size = new System.Drawing.Size(58, 22);
            this.DisplayOptionsDropDown.Text = "Display";
            // 
            // DisplayStringsMenuItem
            // 
            this.DisplayStringsMenuItem.Checked = true;
            this.DisplayStringsMenuItem.CheckOnClick = true;
            this.DisplayStringsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayStringsMenuItem.Name = "DisplayStringsMenuItem";
            this.DisplayStringsMenuItem.Size = new System.Drawing.Size(184, 22);
            this.DisplayStringsMenuItem.Text = "Strings";
            this.DisplayStringsMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayStringCentersMenuItem
            // 
            this.DisplayStringCentersMenuItem.Checked = true;
            this.DisplayStringCentersMenuItem.CheckOnClick = true;
            this.DisplayStringCentersMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayStringCentersMenuItem.Name = "DisplayStringCentersMenuItem";
            this.DisplayStringCentersMenuItem.Size = new System.Drawing.Size(184, 22);
            this.DisplayStringCentersMenuItem.Text = "String centers";
            this.DisplayStringCentersMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayFretsMenuItem
            // 
            this.DisplayFretsMenuItem.Checked = true;
            this.DisplayFretsMenuItem.CheckOnClick = true;
            this.DisplayFretsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayFretsMenuItem.Name = "DisplayFretsMenuItem";
            this.DisplayFretsMenuItem.Size = new System.Drawing.Size(184, 22);
            this.DisplayFretsMenuItem.Text = "Frets";
            this.DisplayFretsMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayMarginsMenuItem
            // 
            this.DisplayMarginsMenuItem.Checked = true;
            this.DisplayMarginsMenuItem.CheckOnClick = true;
            this.DisplayMarginsMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayMarginsMenuItem.Name = "DisplayMarginsMenuItem";
            this.DisplayMarginsMenuItem.Size = new System.Drawing.Size(184, 22);
            this.DisplayMarginsMenuItem.Text = "Fingerboard margins";
            this.DisplayMarginsMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayFingerboardMenuItem
            // 
            this.DisplayFingerboardMenuItem.Checked = true;
            this.DisplayFingerboardMenuItem.CheckOnClick = true;
            this.DisplayFingerboardMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.DisplayFingerboardMenuItem.Name = "DisplayFingerboardMenuItem";
            this.DisplayFingerboardMenuItem.Size = new System.Drawing.Size(184, 22);
            this.DisplayFingerboardMenuItem.Text = "Fingerboard";
            this.DisplayFingerboardMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // DisplayCenterLineMenuItem
            // 
            this.DisplayCenterLineMenuItem.CheckOnClick = true;
            this.DisplayCenterLineMenuItem.Name = "DisplayCenterLineMenuItem";
            this.DisplayCenterLineMenuItem.Size = new System.Drawing.Size(184, 22);
            this.DisplayCenterLineMenuItem.Text = "Center line";
            this.DisplayCenterLineMenuItem.CheckedChanged += new System.EventHandler(this.DisplayOptionsMenuItem_CheckedChanged);
            // 
            // ResetCameraButton
            // 
            this.ResetCameraButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ResetCameraButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.ResetCameraButton.Image = ((System.Drawing.Image)(resources.GetObject("ResetCameraButton.Image")));
            this.ResetCameraButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ResetCameraButton.Name = "ResetCameraButton";
            this.ResetCameraButton.Size = new System.Drawing.Size(83, 22);
            this.ResetCameraButton.Text = "Reset Camera";
            this.ResetCameraButton.Click += new System.EventHandler(this.ResetCameraButton_Click);
            // 
            // ZoomToolstripLabel
            // 
            this.ZoomToolstripLabel.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.ZoomToolstripLabel.Name = "ZoomToolstripLabel";
            this.ZoomToolstripLabel.Size = new System.Drawing.Size(73, 22);
            this.ZoomToolstripLabel.Text = "Zoom: 100%";
            // 
            // tsbDisplayOptions
            // 
            this.tsbDisplayOptions.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbDisplayOptions.Image = global::SiGen.Properties.Resources.Settings_x32;
            this.tsbDisplayOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDisplayOptions.Name = "tsbDisplayOptions";
            this.tsbDisplayOptions.Size = new System.Drawing.Size(23, 22);
            this.tsbDisplayOptions.Text = "toolStripButton1";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Margin = new System.Windows.Forms.Padding(0, 0, 10, 0);
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbMeasureTool
            // 
            this.tsbMeasureTool.Checked = true;
            this.tsbMeasureTool.CheckOnClick = true;
            this.tsbMeasureTool.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tsbMeasureTool.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbMeasureTool.Image = global::SiGen.Properties.Resources.Measure_x32;
            this.tsbMeasureTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMeasureTool.Name = "tsbMeasureTool";
            this.tsbMeasureTool.Size = new System.Drawing.Size(23, 22);
            this.tsbMeasureTool.Text = "Measure";
            this.tsbMeasureTool.CheckedChanged += new System.EventHandler(this.tsbMeasureTool_CheckedChanged);
            // 
            // layoutViewer1
            // 
            this.layoutViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.layoutViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutViewer1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewer1.Name = "layoutViewer1";
            this.layoutViewer1.Size = new System.Drawing.Size(577, 311);
            this.layoutViewer1.TabIndex = 1;
            this.layoutViewer1.Text = "layoutViewer1";
            this.layoutViewer1.ZoomChanged += new System.EventHandler(this.layoutViewer1_ZoomChanged);
            // 
            // LayoutViewerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 361);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "LayoutViewerPanel";
            this.TabPageContextMenuStrip = this.cmsDocumentTab;
            this.Text = "LayoutViewerPanel";
            this.cmsDocumentTab.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.BottomToolStripPanel.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ToolStripDisplayOptions.ResumeLayout(false);
            this.ToolStripDisplayOptions.PerformLayout();
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