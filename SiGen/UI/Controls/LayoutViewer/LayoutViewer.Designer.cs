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
            this.menuMeasureBox = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemDisplayMeasureMM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureCM = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureIN = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemDisplayMeasureFT = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemDisplayMeasureShowDecimals = new System.Windows.Forms.ToolStripMenuItem();
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
            this.menuItemDisplayMeasureShowDecimals});
            this.menuMeasureBox.Name = "menuMeasureBox";
            this.menuMeasureBox.Size = new System.Drawing.Size(193, 120);
            // 
            // menuItemDisplayMeasureMM
            // 
            this.menuItemDisplayMeasureMM.Checked = true;
            this.menuItemDisplayMeasureMM.CheckState = System.Windows.Forms.CheckState.Checked;
            this.menuItemDisplayMeasureMM.Name = "menuItemDisplayMeasureMM";
            this.menuItemDisplayMeasureMM.Size = new System.Drawing.Size(192, 22);
            this.menuItemDisplayMeasureMM.Text = "Display in Milimeters";
            // 
            // menuItemDisplayMeasureCM
            // 
            this.menuItemDisplayMeasureCM.Name = "menuItemDisplayMeasureCM";
            this.menuItemDisplayMeasureCM.Size = new System.Drawing.Size(192, 22);
            this.menuItemDisplayMeasureCM.Text = "Display in Centimeters";
            // 
            // menuItemDisplayMeasureIN
            // 
            this.menuItemDisplayMeasureIN.Name = "menuItemDisplayMeasureIN";
            this.menuItemDisplayMeasureIN.Size = new System.Drawing.Size(192, 22);
            this.menuItemDisplayMeasureIN.Text = "Display in Inches";
            // 
            // menuItemDisplayMeasureFT
            // 
            this.menuItemDisplayMeasureFT.Name = "menuItemDisplayMeasureFT";
            this.menuItemDisplayMeasureFT.Size = new System.Drawing.Size(192, 22);
            this.menuItemDisplayMeasureFT.Text = "Display in Feets";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
            // 
            // menuItemDisplayMeasureShowDecimals
            // 
            this.menuItemDisplayMeasureShowDecimals.Name = "menuItemDisplayMeasureShowDecimals";
            this.menuItemDisplayMeasureShowDecimals.Size = new System.Drawing.Size(192, 22);
            this.menuItemDisplayMeasureShowDecimals.Text = "Show Exact Value";
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
    }
}
