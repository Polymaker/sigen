namespace SiGen.UI.Controls.LayoutEditors
{
    partial class StringsConfigurationEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StringsConfigurationEditor));
            this.nbxNumberOfStrings = new SiGen.UI.Controls.NumericBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLeftHanded = new System.Windows.Forms.CheckBox();
            this.nbxNumberOfFrets = new SiGen.UI.Controls.NumericBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvStrings = new System.Windows.Forms.DataGridView();
            this.chkShowAdvanced = new System.Windows.Forms.CheckBox();
            this.cmsMesureCellMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearValue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.convertToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToMM = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToCM = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToIN = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToFT = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrings)).BeginInit();
            this.cmsMesureCellMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // nbxNumberOfStrings
            // 
            this.nbxNumberOfStrings.AllowDecimals = false;
            resources.ApplyResources(this.nbxNumberOfStrings, "nbxNumberOfStrings");
            this.nbxNumberOfStrings.MaximumValue = 40D;
            this.nbxNumberOfStrings.MinimumValue = 1D;
            this.nbxNumberOfStrings.Name = "nbxNumberOfStrings";
            this.nbxNumberOfStrings.Value = 1D;
            this.nbxNumberOfStrings.ValueChanged += new System.EventHandler(this.nbxNumberOfStrings_ValueChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // chkLeftHanded
            // 
            resources.ApplyResources(this.chkLeftHanded, "chkLeftHanded");
            this.chkLeftHanded.Name = "chkLeftHanded";
            this.chkLeftHanded.UseVisualStyleBackColor = true;
            this.chkLeftHanded.CheckedChanged += new System.EventHandler(this.chkLeftHanded_CheckedChanged);
            // 
            // nbxNumberOfFrets
            // 
            this.nbxNumberOfFrets.AllowDecimals = false;
            resources.ApplyResources(this.nbxNumberOfFrets, "nbxNumberOfFrets");
            this.nbxNumberOfFrets.MaximumValue = 50D;
            this.nbxNumberOfFrets.MinimumValue = 1D;
            this.nbxNumberOfFrets.Name = "nbxNumberOfFrets";
            this.nbxNumberOfFrets.Value = 24D;
            this.nbxNumberOfFrets.ValueChanged += new System.EventHandler(this.nbxNumberOfFrets_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // dgvStrings
            // 
            this.dgvStrings.AllowUserToAddRows = false;
            this.dgvStrings.AllowUserToDeleteRows = false;
            this.dgvStrings.AllowUserToResizeRows = false;
            this.dgvStrings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tableLayoutPanel1.SetColumnSpan(this.dgvStrings, 3);
            resources.ApplyResources(this.dgvStrings, "dgvStrings");
            this.dgvStrings.MultiSelect = false;
            this.dgvStrings.Name = "dgvStrings";
            this.dgvStrings.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStrings_CellClick);
            this.dgvStrings.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvStrings_CellFormatting);
            this.dgvStrings.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvStrings_CellMouseClick);
            this.dgvStrings.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvStrings_CellPainting);
            this.dgvStrings.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvStrings_CellParsing);
            this.dgvStrings.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvStrings_CellValidating);
            this.dgvStrings.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvStrings_CellValueChanged);
            this.dgvStrings.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvStrings_EditingControlShowing);
            // 
            // chkShowAdvanced
            // 
            resources.ApplyResources(this.chkShowAdvanced, "chkShowAdvanced");
            this.chkShowAdvanced.Name = "chkShowAdvanced";
            this.chkShowAdvanced.UseVisualStyleBackColor = true;
            this.chkShowAdvanced.CheckedChanged += new System.EventHandler(this.chkShowAdvanced_CheckedChanged);
            // 
            // cmsMesureCellMenu
            // 
            this.cmsMesureCellMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearValue,
            this.toolStripSeparator1,
            this.convertToToolStripMenuItem,
            this.tsmiConvertToMM,
            this.tsmiConvertToCM,
            this.tsmiConvertToIN,
            this.tsmiConvertToFT});
            this.cmsMesureCellMenu.Name = "cmsConvert";
            resources.ApplyResources(this.cmsMesureCellMenu, "cmsMesureCellMenu");
            this.cmsMesureCellMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMesureCellMenu_Opening);
            // 
            // tsmiClearValue
            // 
            this.tsmiClearValue.Name = "tsmiClearValue";
            resources.ApplyResources(this.tsmiClearValue, "tsmiClearValue");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // convertToToolStripMenuItem
            // 
            this.convertToToolStripMenuItem.Name = "convertToToolStripMenuItem";
            resources.ApplyResources(this.convertToToolStripMenuItem, "convertToToolStripMenuItem");
            // 
            // tsmiConvertToMM
            // 
            this.tsmiConvertToMM.Name = "tsmiConvertToMM";
            resources.ApplyResources(this.tsmiConvertToMM, "tsmiConvertToMM");
            this.tsmiConvertToMM.Click += new System.EventHandler(this.tsmiConvertToMM_Click);
            // 
            // tsmiConvertToCM
            // 
            this.tsmiConvertToCM.Name = "tsmiConvertToCM";
            resources.ApplyResources(this.tsmiConvertToCM, "tsmiConvertToCM");
            this.tsmiConvertToCM.Click += new System.EventHandler(this.tsmiConvertToCM_Click);
            // 
            // tsmiConvertToIN
            // 
            this.tsmiConvertToIN.Name = "tsmiConvertToIN";
            resources.ApplyResources(this.tsmiConvertToIN, "tsmiConvertToIN");
            this.tsmiConvertToIN.Click += new System.EventHandler(this.tsmiConvertToIN_Click);
            // 
            // tsmiConvertToFT
            // 
            this.tsmiConvertToFT.Name = "tsmiConvertToFT";
            resources.ApplyResources(this.tsmiConvertToFT, "tsmiConvertToFT");
            this.tsmiConvertToFT.Click += new System.EventHandler(this.tsmiConvertToFT_Click);
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.dgvStrings, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkShowAdvanced, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.nbxNumberOfStrings, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.nbxNumberOfFrets, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkLeftHanded, 2, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // StringsConfigurationEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "StringsConfigurationEditor";
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrings)).EndInit();
            this.cmsMesureCellMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private NumericBox nbxNumberOfStrings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLeftHanded;
        private NumericBox nbxNumberOfFrets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgvStrings;
        private System.Windows.Forms.CheckBox chkShowAdvanced;
        private System.Windows.Forms.ContextMenuStrip cmsMesureCellMenu;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearValue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem convertToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToMM;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToCM;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToIN;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToFT;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    }
}
