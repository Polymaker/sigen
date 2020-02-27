namespace SiGen.UI
{
    partial class LayoutEditorWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutEditorWindow));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.AboutAppButton = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tssbOpen = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparatorOpen = new System.Windows.Forms.ToolStripSeparator();
            this.tssbSave = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbUndo = new System.Windows.Forms.ToolStripSplitButton();
            this.tssbRedo = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AboutAppButton,
            this.toolStripSeparator2,
            this.tsbNew,
            this.tssbOpen,
            this.tssbSave,
            this.tsbClose,
            this.toolStripSeparator3,
            this.tssbUndo,
            this.tssbRedo,
            this.toolStripSeparator1,
            this.tsbExport,
            this.tsbOptions});
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Stretch = true;
            // 
            // AboutAppButton
            // 
            this.AboutAppButton.Image = global::SiGen.Properties.Resources.SiGenIcon_x32;
            resources.ApplyResources(this.AboutAppButton, "AboutAppButton");
            this.AboutAppButton.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.AboutAppButton.Name = "AboutAppButton";
            this.AboutAppButton.Click += new System.EventHandler(this.AboutAppButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // tsbNew
            // 
            this.tsbNew.Image = global::SiGen.Properties.Resources.NewLayout_x32;
            resources.ApplyResources(this.tsbNew, "tsbNew");
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tssbOpen
            // 
            this.tssbOpen.DropDownButtonWidth = 24;
            this.tssbOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenFile,
            this.tsmiOpenTemplate,
            this.tsSeparatorOpen});
            this.tssbOpen.Image = global::SiGen.Properties.Resources.OpenLayout_x32;
            resources.ApplyResources(this.tssbOpen, "tssbOpen");
            this.tssbOpen.Name = "tssbOpen";
            this.tssbOpen.ButtonClick += new System.EventHandler(this.tssbOpen_ButtonClick);
            this.tssbOpen.MouseHover += new System.EventHandler(this.tssbOpen_MouseHover);
            // 
            // tsmiOpenFile
            // 
            this.tsmiOpenFile.Name = "tsmiOpenFile";
            resources.ApplyResources(this.tsmiOpenFile, "tsmiOpenFile");
            this.tsmiOpenFile.Click += new System.EventHandler(this.tsmiOpenFile_Click);
            // 
            // tsmiOpenTemplate
            // 
            this.tsmiOpenTemplate.Name = "tsmiOpenTemplate";
            resources.ApplyResources(this.tsmiOpenTemplate, "tsmiOpenTemplate");
            // 
            // tsSeparatorOpen
            // 
            this.tsSeparatorOpen.Name = "tsSeparatorOpen";
            resources.ApplyResources(this.tsSeparatorOpen, "tsSeparatorOpen");
            // 
            // tssbSave
            // 
            this.tssbSave.DropDownButtonWidth = 24;
            this.tssbSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiSaveAs,
            this.tsmiSaveTemplate});
            this.tssbSave.Image = global::SiGen.Properties.Resources.SaveLayout_x32;
            resources.ApplyResources(this.tssbSave, "tssbSave");
            this.tssbSave.Name = "tssbSave";
            this.tssbSave.ButtonClick += new System.EventHandler(this.tssbSave_ButtonClick);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            resources.ApplyResources(this.tsmiSave, "tsmiSave");
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            resources.ApplyResources(this.tsmiSaveAs, "tsmiSaveAs");
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // tsmiSaveTemplate
            // 
            this.tsmiSaveTemplate.Name = "tsmiSaveTemplate";
            resources.ApplyResources(this.tsmiSaveTemplate, "tsmiSaveTemplate");
            // 
            // tsbClose
            // 
            this.tsbClose.Image = global::SiGen.Properties.Resources.CloseLayout_x32;
            resources.ApplyResources(this.tsbClose, "tsbClose");
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // tssbUndo
            // 
            this.tssbUndo.Image = global::SiGen.Properties.Resources.Undo_x32;
            resources.ApplyResources(this.tssbUndo, "tssbUndo");
            this.tssbUndo.Name = "tssbUndo";
            this.tssbUndo.ButtonClick += new System.EventHandler(this.tssbUndo_ButtonClick);
            this.tssbUndo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tssbUndo_DropDownItemClicked);
            // 
            // tssbRedo
            // 
            this.tssbRedo.Image = global::SiGen.Properties.Resources.Redo_x32;
            resources.ApplyResources(this.tssbRedo, "tssbRedo");
            this.tssbRedo.Name = "tssbRedo";
            this.tssbRedo.ButtonClick += new System.EventHandler(this.tssbRedo_ButtonClick);
            this.tssbRedo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tssbRedo_DropDownItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // tsbExport
            // 
            this.tsbExport.Image = global::SiGen.Properties.Resources.ExportLayout_x32;
            resources.ApplyResources(this.tsbExport, "tsbExport");
            this.tsbExport.Name = "tsbExport";
            this.tsbExport.Click += new System.EventHandler(this.tsbExport_Click);
            // 
            // tsbOptions
            // 
            this.tsbOptions.Image = global::SiGen.Properties.Resources.Settings_x32;
            resources.ApplyResources(this.tsbOptions, "tsbOptions");
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Click += new System.EventHandler(this.tsbOptions_Click);
            // 
            // dockPanel1
            // 
            this.dockPanel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            resources.ApplyResources(this.dockPanel1, "dockPanel1");
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.ActiveDocumentChanged += new System.EventHandler(this.dockPanel1_ActiveDocumentChanged);
            // 
            // BottomToolStripPanel
            // 
            resources.ApplyResources(this.BottomToolStripPanel, "BottomToolStripPanel");
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // TopToolStripPanel
            // 
            resources.ApplyResources(this.TopToolStripPanel, "TopToolStripPanel");
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // RightToolStripPanel
            // 
            resources.ApplyResources(this.RightToolStripPanel, "RightToolStripPanel");
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // LeftToolStripPanel
            // 
            resources.ApplyResources(this.LeftToolStripPanel, "LeftToolStripPanel");
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            // 
            // ContentPanel
            // 
            resources.ApplyResources(this.ContentPanel, "ContentPanel");
            // 
            // statusStrip1
            // 
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // LayoutEditorWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.IsMdiContainer = true;
            this.Name = "LayoutEditorWindow";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton tssbSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAs;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveTemplate;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripSplitButton tssbOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiOpenTemplate;
        private System.Windows.Forms.ToolStripSeparator tsSeparatorOpen;
        private WeifenLuo.WinFormsUI.Docking.DockPanel dockPanel1;
        private System.Windows.Forms.ToolStripButton tsbOptions;
        private System.Windows.Forms.ToolStripButton tsbExport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.StatusStrip statusStrip1;
		private System.Windows.Forms.ToolStripSplitButton tssbUndo;
		private System.Windows.Forms.ToolStripSplitButton tssbRedo;
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel AboutAppButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}