namespace SiGen.UI.Controls
{
    partial class MeasureTextbox
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasureTextbox));
            this.innerTextbox = new SiGen.UI.Controls.TextBoxEx();
            this.cmsConvert = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearValue = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.convertToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToMM = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToCM = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToIN = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToFT = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsConvert.SuspendLayout();
            this.SuspendLayout();
            // 
            // innerTextbox
            // 
            this.innerTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerTextbox.ContextMenuStrip = this.cmsConvert;
            resources.ApplyResources(this.innerTextbox, "innerTextbox");
            this.innerTextbox.Name = "innerTextbox";
            this.innerTextbox.ValidateOnEnter = true;
            this.innerTextbox.CommandKeyPressed += new System.Windows.Forms.KeyEventHandler(this.innerTextbox_CommandKeyPressed);
            this.innerTextbox.TextChanged += new System.EventHandler(this.innerTextbox_TextChanged);
            this.innerTextbox.Enter += new System.EventHandler(this.innerTextbox_Enter);
            this.innerTextbox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.innerTextbox_KeyDown);
            this.innerTextbox.Leave += new System.EventHandler(this.innerTextbox_Leave);
            this.innerTextbox.Validating += new System.ComponentModel.CancelEventHandler(this.innerTextbox_Validating);
            this.innerTextbox.Validated += new System.EventHandler(this.innerTextbox_Validated);
            // 
            // cmsConvert
            // 
            this.cmsConvert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearValue,
            this.toolStripSeparator1,
            this.convertToToolStripMenuItem,
            this.tsmiConvertToMM,
            this.tsmiConvertToCM,
            this.tsmiConvertToIN,
            this.tsmiConvertToFT});
            this.cmsConvert.Name = "cmsConvert";
            resources.ApplyResources(this.cmsConvert, "cmsConvert");
            this.cmsConvert.Opening += new System.ComponentModel.CancelEventHandler(this.cmsConvert_Opening);
            // 
            // tsmiClearValue
            // 
            this.tsmiClearValue.Name = "tsmiClearValue";
            resources.ApplyResources(this.tsmiClearValue, "tsmiClearValue");
            this.tsmiClearValue.Click += new System.EventHandler(this.tsmiClearValue_Click);
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
            // MeasureTextbox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cmsConvert;
            this.Controls.Add(this.innerTextbox);
            this.Name = "MeasureTextbox";
            this.cmsConvert.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip cmsConvert;
        private System.Windows.Forms.ToolStripMenuItem convertToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToMM;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToCM;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToIN;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToFT;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearValue;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        protected Controls.TextBoxEx innerTextbox;
    }
}
