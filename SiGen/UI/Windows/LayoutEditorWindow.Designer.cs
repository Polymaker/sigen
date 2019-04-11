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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutEditorWindow));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tssbOpen = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiOpenFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiOpenTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsSeparatorOpen = new System.Windows.Forms.ToolStripSeparator();
            this.tssbSave = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveTemplate = new System.Windows.Forms.ToolStripMenuItem();
            this.tssbExport = new System.Windows.Forms.ToolStripButton();
            this.tsbOptions = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbUndo = new System.Windows.Forms.ToolStripSplitButton();
            this.tssbRedo = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbMeasureTool = new System.Windows.Forms.ToolStripButton();
            this.tsbLayoutProperties = new System.Windows.Forms.ToolStripButton();
            this.dockPanel1 = new WeifenLuo.WinFormsUI.Docking.DockPanel();
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.localizableStringList1 = new SiGen.Localization.LocalizableStringList(this.components);
            this.MSG_FileAlreadyOpen = new SiGen.Localization.LocalizableString(this.components);
            this.MSG_SaveBeforeClose = new SiGen.Localization.LocalizableString(this.components);
            this.LBL_Warning = new SiGen.Localization.LocalizableString(this.components);
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(100, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button2.Location = new System.Drawing.Point(85, 50);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tssbOpen,
            this.tssbSave,
            this.tssbExport,
            this.tsbOptions,
            this.toolStripSeparator2,
            this.tssbUndo,
            this.tssbRedo,
            this.toolStripSeparator1,
            this.tsbMeasureTool,
            this.tsbLayoutProperties});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1344, 69);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.Image = global::SiGen.Properties.Resources.NewLayoutIcon_32;
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(74, 66);
            this.tsbNew.Text = "New Layout";
            this.tsbNew.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbNew.Click += new System.EventHandler(this.tsbNew_Click);
            // 
            // tssbOpen
            // 
            this.tssbOpen.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiOpenFile,
            this.tsmiOpenTemplate,
            this.tsSeparatorOpen});
            this.tssbOpen.Image = global::SiGen.Properties.Resources.OpenLayoutIcon_32;
            this.tssbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbOpen.Name = "tssbOpen";
            this.tssbOpen.Size = new System.Drawing.Size(52, 66);
            this.tssbOpen.Text = "Open";
            this.tssbOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tssbOpen.ButtonClick += new System.EventHandler(this.tssbOpen_ButtonClick);
            this.tssbOpen.MouseHover += new System.EventHandler(this.tssbOpen_MouseHover);
            // 
            // tsmiOpenFile
            // 
            this.tsmiOpenFile.Name = "tsmiOpenFile";
            this.tsmiOpenFile.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.tsmiOpenFile.Size = new System.Drawing.Size(176, 22);
            this.tsmiOpenFile.Text = "Open File...";
            this.tsmiOpenFile.Click += new System.EventHandler(this.tsmiOpenFile_Click);
            // 
            // tsmiOpenTemplate
            // 
            this.tsmiOpenTemplate.Name = "tsmiOpenTemplate";
            this.tsmiOpenTemplate.Size = new System.Drawing.Size(176, 22);
            this.tsmiOpenTemplate.Text = "Open Template...";
            // 
            // tsSeparatorOpen
            // 
            this.tsSeparatorOpen.Name = "tsSeparatorOpen";
            this.tsSeparatorOpen.Size = new System.Drawing.Size(173, 6);
            // 
            // tssbSave
            // 
            this.tssbSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiSaveAs,
            this.tsmiSaveTemplate});
            this.tssbSave.Image = ((System.Drawing.Image)(resources.GetObject("tssbSave.Image")));
            this.tssbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbSave.Name = "tssbSave";
            this.tssbSave.Size = new System.Drawing.Size(48, 66);
            this.tssbSave.Text = "Save";
            this.tssbSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tssbSave.ButtonClick += new System.EventHandler(this.tssbSave_ButtonClick);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSave.Size = new System.Drawing.Size(186, 22);
            this.tsmiSave.Text = "Save";
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.tsmiSaveAs.Size = new System.Drawing.Size(186, 22);
            this.tsmiSaveAs.Text = "Save As...";
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // tsmiSaveTemplate
            // 
            this.tsmiSaveTemplate.Name = "tsmiSaveTemplate";
            this.tsmiSaveTemplate.Size = new System.Drawing.Size(186, 22);
            this.tsmiSaveTemplate.Text = "Save As Template";
            // 
            // tssbExport
            // 
            this.tssbExport.Image = ((System.Drawing.Image)(resources.GetObject("tssbExport.Image")));
            this.tssbExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbExport.Name = "tssbExport";
            this.tssbExport.Size = new System.Drawing.Size(44, 66);
            this.tssbExport.Text = "Export";
            this.tssbExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tssbExport.Click += new System.EventHandler(this.tssbExport_Click);
            // 
            // tsbOptions
            // 
            this.tsbOptions.Image = global::SiGen.Properties.Resources.OptionsIcon_32;
            this.tsbOptions.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOptions.Name = "tsbOptions";
            this.tsbOptions.Size = new System.Drawing.Size(53, 66);
            this.tsbOptions.Text = "Options";
            this.tsbOptions.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbOptions.Click += new System.EventHandler(this.tsbOptions_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 69);
            // 
            // tssbUndo
            // 
            this.tssbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbUndo.Image = ((System.Drawing.Image)(resources.GetObject("tssbUndo.Image")));
            this.tssbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbUndo.Name = "tssbUndo";
            this.tssbUndo.Size = new System.Drawing.Size(52, 66);
            this.tssbUndo.Text = "Undo";
            this.tssbUndo.ButtonClick += new System.EventHandler(this.tssbUndo_ButtonClick);
            this.tssbUndo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tssbUndo_DropDownItemClicked);
            // 
            // tssbRedo
            // 
            this.tssbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbRedo.Image = ((System.Drawing.Image)(resources.GetObject("tssbRedo.Image")));
            this.tssbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbRedo.Name = "tssbRedo";
            this.tssbRedo.Size = new System.Drawing.Size(50, 66);
            this.tssbRedo.Text = "Redo";
            this.tssbRedo.ButtonClick += new System.EventHandler(this.tssbRedo_ButtonClick);
            this.tssbRedo.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tssbRedo_DropDownItemClicked);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 69);
            // 
            // tsbMeasureTool
            // 
            this.tsbMeasureTool.CheckOnClick = true;
            this.tsbMeasureTool.Image = global::SiGen.Properties.Resources.MeasureIcon_32;
            this.tsbMeasureTool.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbMeasureTool.Name = "tsbMeasureTool";
            this.tsbMeasureTool.Size = new System.Drawing.Size(56, 66);
            this.tsbMeasureTool.Text = "Measure";
            this.tsbMeasureTool.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.tsbMeasureTool.Click += new System.EventHandler(this.tsbMeasureTool_Click);
            // 
            // tsbLayoutProperties
            // 
            this.tsbLayoutProperties.Image = ((System.Drawing.Image)(resources.GetObject("tsbLayoutProperties.Image")));
            this.tsbLayoutProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLayoutProperties.Name = "tsbLayoutProperties";
            this.tsbLayoutProperties.Size = new System.Drawing.Size(64, 66);
            this.tsbLayoutProperties.Text = "Layout\r\nProperties";
            this.tsbLayoutProperties.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // dockPanel1
            // 
            this.dockPanel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.dockPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dockPanel1.Location = new System.Drawing.Point(0, 100);
            this.dockPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.dockPanel1.Name = "dockPanel1";
            this.dockPanel1.Size = new System.Drawing.Size(1344, 716);
            this.dockPanel1.TabIndex = 2;
            this.dockPanel1.ActiveDocumentChanged += new System.EventHandler(this.dockPanel1_ActiveDocumentChanged);
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(518, 258);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 816);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.statusStrip1.Size = new System.Drawing.Size(1344, 22);
            this.statusStrip1.TabIndex = 5;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripContainer1
            // 
            this.toolStripContainer1.BottomToolStripPanelVisible = false;
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1344, 6);
            this.toolStripContainer1.ContentPanel.Visible = false;
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolStripContainer1.LeftToolStripPanelVisible = false;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.RightToolStripPanelVisible = false;
            this.toolStripContainer1.Size = new System.Drawing.Size(1344, 100);
            this.toolStripContainer1.TabIndex = 7;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip2);
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip2.Location = new System.Drawing.Point(3, 69);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(35, 25);
            this.toolStrip2.TabIndex = 3;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // localizableStringList1
            // 
            this.localizableStringList1.Items.Add(this.MSG_FileAlreadyOpen);
            this.localizableStringList1.Items.Add(this.MSG_SaveBeforeClose);
            this.localizableStringList1.Items.Add(this.LBL_Warning);
            // 
            // MSG_FileAlreadyOpen
            // 
            this.MSG_FileAlreadyOpen.Text = "The file is already open. do you want to reaload it?";
            // 
            // MSG_SaveBeforeClose
            // 
            this.MSG_SaveBeforeClose.Text = "Do you want to save the changes made to the layout before closing?";
            // 
            // LBL_Warning
            // 
            this.LBL_Warning.Text = "Warning";
            // 
            // LayoutEditorWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1344, 838);
            this.Controls.Add(this.dockPanel1);
            this.Controls.Add(this.toolStripContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IsMdiContainer = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LayoutEditorWindow";
            this.Text = "Stringed Instrument Layout Generator";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
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
        private System.Windows.Forms.ToolStripPanel BottomToolStripPanel;
        private System.Windows.Forms.ToolStripPanel TopToolStripPanel;
        private System.Windows.Forms.ToolStripPanel RightToolStripPanel;
        private System.Windows.Forms.ToolStripPanel LeftToolStripPanel;
        private System.Windows.Forms.ToolStripContentPanel ContentPanel;
        private System.Windows.Forms.ToolStripButton tsbOptions;
        private System.Windows.Forms.ToolStripButton tssbExport;
        private Localization.LocalizableStringList localizableStringList1;
        private Localization.LocalizableString MSG_FileAlreadyOpen;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbLayoutProperties;
        private System.Windows.Forms.ToolStripButton tsbMeasureTool;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private Localization.LocalizableString MSG_SaveBeforeClose;
        private Localization.LocalizableString LBL_Warning;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSplitButton tssbUndo;
		private System.Windows.Forms.ToolStripSplitButton tssbRedo;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
    }
}