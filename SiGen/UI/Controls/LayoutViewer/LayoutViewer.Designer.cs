namespace SiGen.UI
{
    partial class LayoutViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LayoutViewer));
            this.menuMeasureBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemDisplayMeasureMM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureCM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureIN = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureFT = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDisplayMeasureShowDecimals = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureClearMeasure = new System.Windows.Forms.ToolStripMenuItem();
            this.menuMeasureBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMeasureBox
            // 
            this.menuMeasureBox.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemDisplayMeasureMM,
            this.menuItemDisplayMeasureCM,
            this.menuItemDisplayMeasureIN,
            this.menuItemDisplayMeasureFT,
            this.toolStripSeparator1,
            this.menuItemDisplayMeasureShowDecimals,
            this.menuItemDisplayMeasureClearMeasure});
            this.menuMeasureBox.Name = "menuMeasureBox";
            resources.ApplyResources(this.menuMeasureBox, "menuMeasureBox");
            // 
            // menuItemDisplayMeasureMM
            // 
            this.menuItemDisplayMeasureMM.Checked = true;
            this.menuItemDisplayMeasureMM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemDisplayMeasureMM.Name = "menuItemDisplayMeasureMM";
            resources.ApplyResources(this.menuItemDisplayMeasureMM, "menuItemDisplayMeasureMM");
            // 
            // menuItemDisplayMeasureCM
            // 
            this.menuItemDisplayMeasureCM.Name = "menuItemDisplayMeasureCM";
            resources.ApplyResources(this.menuItemDisplayMeasureCM, "menuItemDisplayMeasureCM");
            // 
            // menuItemDisplayMeasureIN
            // 
            this.menuItemDisplayMeasureIN.Name = "menuItemDisplayMeasureIN";
            resources.ApplyResources(this.menuItemDisplayMeasureIN, "menuItemDisplayMeasureIN");
            // 
            // menuItemDisplayMeasureFT
            // 
            this.menuItemDisplayMeasureFT.Name = "menuItemDisplayMeasureFT";
            resources.ApplyResources(this.menuItemDisplayMeasureFT, "menuItemDisplayMeasureFT");
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // menuItemDisplayMeasureShowDecimals
            // 
            this.menuItemDisplayMeasureShowDecimals.CheckOnClick = true;
            this.menuItemDisplayMeasureShowDecimals.Name = "menuItemDisplayMeasureShowDecimals";
            resources.ApplyResources(this.menuItemDisplayMeasureShowDecimals, "menuItemDisplayMeasureShowDecimals");
            // 
            // menuItemDisplayMeasureClearMeasure
            // 
            this.menuItemDisplayMeasureClearMeasure.Name = "menuItemDisplayMeasureClearMeasure";
            resources.ApplyResources(this.menuItemDisplayMeasureClearMeasure, "menuItemDisplayMeasureClearMeasure");
            this.menuMeasureBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip menuMeasureBox;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayMeasureMM;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayMeasureCM;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayMeasureIN;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayMeasureFT;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayMeasureShowDecimals;
        private System.Windows.Forms.ToolStripMenuItem menuItemDisplayMeasureClearMeasure;
    }
}
