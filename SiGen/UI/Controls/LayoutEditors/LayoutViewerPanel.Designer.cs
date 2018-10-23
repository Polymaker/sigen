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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLblSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLblZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.cmsDocumentTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiCloseLayout = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseRight = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCloseOthers = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAS = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiRename = new System.Windows.Forms.ToolStripMenuItem();
            this.layoutViewer1 = new SiGen.UI.LayoutViewer();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiOpenFileDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.cmsDocumentTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLblSpacer,
            this.tsLblZoom,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 337);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(577, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLblSpacer
            // 
            this.tsLblSpacer.Name = "tsLblSpacer";
            this.tsLblSpacer.Size = new System.Drawing.Size(406, 19);
            this.tsLblSpacer.Spring = true;
            // 
            // tsLblZoom
            // 
            this.tsLblZoom.Name = "tsLblZoom";
            this.tsLblZoom.Size = new System.Drawing.Size(73, 19);
            this.tsLblZoom.Text = "Zoom: 100%";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Top) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right) 
            | System.Windows.Forms.ToolStripStatusLabelBorderSides.Bottom)));
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(83, 19);
            this.toolStripStatusLabel1.Text = "Reset Camera";
            this.toolStripStatusLabel1.Click += new System.EventHandler(this.toolStripStatusLabel1_Click);
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
            this.cmsDocumentTab.Size = new System.Drawing.Size(207, 192);
            this.cmsDocumentTab.Opening += new System.ComponentModel.CancelEventHandler(this.cmsDocumentTab_Opening);
            // 
            // tsmiCloseLayout
            // 
            this.tsmiCloseLayout.Name = "tsmiCloseLayout";
            this.tsmiCloseLayout.Size = new System.Drawing.Size(206, 22);
            this.tsmiCloseLayout.Text = "Close layout";
            this.tsmiCloseLayout.Click += new System.EventHandler(this.tsmiCloseLayout_Click);
            // 
            // tsmiCloseRight
            // 
            this.tsmiCloseRight.Name = "tsmiCloseRight";
            this.tsmiCloseRight.Size = new System.Drawing.Size(206, 22);
            this.tsmiCloseRight.Text = "Close layouts to the right";
            this.tsmiCloseRight.Click += new System.EventHandler(this.tsmiCloseRight_Click);
            // 
            // tsmiCloseOthers
            // 
            this.tsmiCloseOthers.Name = "tsmiCloseOthers";
            this.tsmiCloseOthers.Size = new System.Drawing.Size(206, 22);
            this.tsmiCloseOthers.Text = "Close other layouts";
            this.tsmiCloseOthers.Click += new System.EventHandler(this.tsmiCloseOthers_Click);
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
            // layoutViewer1
            // 
            this.layoutViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.layoutViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutViewer1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewer1.Name = "layoutViewer1";
            this.layoutViewer1.Size = new System.Drawing.Size(577, 337);
            this.layoutViewer1.TabIndex = 1;
            this.layoutViewer1.Text = "layoutViewer1";
            this.layoutViewer1.ZoomChanged += new System.EventHandler(this.layoutViewer1_ZoomChanged);
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
            // LayoutViewerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(577, 361);
            this.Controls.Add(this.layoutViewer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "LayoutViewerPanel";
            this.TabPageContextMenuStrip = this.cmsDocumentTab;
            this.Text = "LayoutViewerPanel";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.cmsDocumentTab.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private LayoutViewer layoutViewer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsLblSpacer;
        private System.Windows.Forms.ToolStripStatusLabel tsLblZoom;
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
    }
}