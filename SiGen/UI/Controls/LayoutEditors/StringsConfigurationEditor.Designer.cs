﻿namespace SiGen.UI.Controls.LayoutEditors
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
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrings)).BeginInit();
            this.cmsMesureCellMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // nbxNumberOfStrings
            // 
            this.nbxNumberOfStrings.AllowDecimals = false;
            this.nbxNumberOfStrings.Location = new System.Drawing.Point(102, 3);
            this.nbxNumberOfStrings.MaximumValue = 40D;
            this.nbxNumberOfStrings.MinimumValue = 1D;
            this.nbxNumberOfStrings.Name = "nbxNumberOfStrings";
            this.nbxNumberOfStrings.Size = new System.Drawing.Size(66, 20);
            this.nbxNumberOfStrings.TabIndex = 0;
            this.nbxNumberOfStrings.Value = 1D;
            this.nbxNumberOfStrings.ValueChanged += new System.EventHandler(this.nbxNumberOfStrings_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number Of Strings";
            // 
            // chkLeftHanded
            // 
            this.chkLeftHanded.AutoSize = true;
            this.chkLeftHanded.Location = new System.Drawing.Point(174, 6);
            this.chkLeftHanded.Name = "chkLeftHanded";
            this.chkLeftHanded.Size = new System.Drawing.Size(85, 17);
            this.chkLeftHanded.TabIndex = 2;
            this.chkLeftHanded.Text = "Left Handed";
            this.chkLeftHanded.UseVisualStyleBackColor = true;
            this.chkLeftHanded.CheckedChanged += new System.EventHandler(this.chkLeftHanded_CheckedChanged);
            // 
            // nbxNumberOfFrets
            // 
            this.nbxNumberOfFrets.AllowDecimals = false;
            this.nbxNumberOfFrets.Location = new System.Drawing.Point(102, 29);
            this.nbxNumberOfFrets.MaximumValue = 50D;
            this.nbxNumberOfFrets.MinimumValue = 1D;
            this.nbxNumberOfFrets.Name = "nbxNumberOfFrets";
            this.nbxNumberOfFrets.Size = new System.Drawing.Size(66, 20);
            this.nbxNumberOfFrets.TabIndex = 3;
            this.nbxNumberOfFrets.Value = 24D;
            this.nbxNumberOfFrets.ValueChanged += new System.EventHandler(this.nbxNumberOfFrets_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Number Of Frets";
            // 
            // dgvStrings
            // 
            this.dgvStrings.AllowUserToAddRows = false;
            this.dgvStrings.AllowUserToDeleteRows = false;
            this.dgvStrings.AllowUserToResizeRows = false;
            this.dgvStrings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStrings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStrings.Location = new System.Drawing.Point(3, 55);
            this.dgvStrings.MultiSelect = false;
            this.dgvStrings.Name = "dgvStrings";
            this.dgvStrings.RowHeadersWidth = 120;
            this.dgvStrings.Size = new System.Drawing.Size(359, 181);
            this.dgvStrings.TabIndex = 5;
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
            this.chkShowAdvanced.AutoSize = true;
            this.chkShowAdvanced.Location = new System.Drawing.Point(174, 29);
            this.chkShowAdvanced.Name = "chkShowAdvanced";
            this.chkShowAdvanced.Size = new System.Drawing.Size(141, 17);
            this.chkShowAdvanced.TabIndex = 6;
            this.chkShowAdvanced.Text = "Show Advanced Config.";
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
            this.cmsMesureCellMenu.Size = new System.Drawing.Size(181, 164);
            this.cmsMesureCellMenu.Opening += new System.ComponentModel.CancelEventHandler(this.cmsMesureCellMenu_Opening);
            // 
            // tsmiClearValue
            // 
            this.tsmiClearValue.Name = "tsmiClearValue";
            this.tsmiClearValue.Size = new System.Drawing.Size(180, 22);
            this.tsmiClearValue.Text = "Clear Value";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // convertToToolStripMenuItem
            // 
            this.convertToToolStripMenuItem.Name = "convertToToolStripMenuItem";
            this.convertToToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.convertToToolStripMenuItem.Text = "Convert to:";
            // 
            // tsmiConvertToMM
            // 
            this.tsmiConvertToMM.Name = "tsmiConvertToMM";
            this.tsmiConvertToMM.Size = new System.Drawing.Size(180, 22);
            this.tsmiConvertToMM.Text = "Millimeters";
            this.tsmiConvertToMM.Click += new System.EventHandler(this.tsmiConvertToMM_Click);
            // 
            // tsmiConvertToCM
            // 
            this.tsmiConvertToCM.Name = "tsmiConvertToCM";
            this.tsmiConvertToCM.Size = new System.Drawing.Size(180, 22);
            this.tsmiConvertToCM.Text = "Centimeters";
            this.tsmiConvertToCM.Click += new System.EventHandler(this.tsmiConvertToCM_Click);
            // 
            // tsmiConvertToIN
            // 
            this.tsmiConvertToIN.Name = "tsmiConvertToIN";
            this.tsmiConvertToIN.Size = new System.Drawing.Size(180, 22);
            this.tsmiConvertToIN.Text = "Inches";
            this.tsmiConvertToIN.Click += new System.EventHandler(this.tsmiConvertToIN_Click);
            // 
            // tsmiConvertToFT
            // 
            this.tsmiConvertToFT.Name = "tsmiConvertToFT";
            this.tsmiConvertToFT.Size = new System.Drawing.Size(180, 22);
            this.tsmiConvertToFT.Text = "Feets";
            this.tsmiConvertToFT.Click += new System.EventHandler(this.tsmiConvertToFT_Click);
            // 
            // StringsConfigurationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.chkShowAdvanced);
            this.Controls.Add(this.dgvStrings);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nbxNumberOfFrets);
            this.Controls.Add(this.chkLeftHanded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nbxNumberOfStrings);
            this.Name = "StringsConfigurationEditor";
            this.Size = new System.Drawing.Size(365, 239);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStrings)).EndInit();
            this.cmsMesureCellMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}