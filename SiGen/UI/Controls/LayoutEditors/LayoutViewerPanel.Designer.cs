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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsLblSpacer = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsLblZoom = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.layoutViewer1 = new SiGen.UI.LayoutViewer();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLblSpacer,
            this.tsLblZoom,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 299);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.statusStrip1.Size = new System.Drawing.Size(598, 24);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsLblSpacer
            // 
            this.tsLblSpacer.Name = "tsLblSpacer";
            this.tsLblSpacer.Size = new System.Drawing.Size(427, 19);
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
            // layoutViewer1
            // 
            this.layoutViewer1.BackColor = System.Drawing.SystemColors.Window;
            this.layoutViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutViewer1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewer1.Name = "layoutViewer1";
            this.layoutViewer1.Size = new System.Drawing.Size(598, 299);
            this.layoutViewer1.TabIndex = 1;
            this.layoutViewer1.Text = "layoutViewer1";
            this.layoutViewer1.ZoomChanged += new System.EventHandler(this.layoutViewer1_ZoomChanged);
            // 
            // LayoutViewerPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 323);
            this.Controls.Add(this.layoutViewer1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "LayoutViewerPanel";
            this.Text = "LayoutViewerPanel";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private LayoutViewer layoutViewer1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsLblSpacer;
        private System.Windows.Forms.ToolStripStatusLabel tsLblZoom;
    }
}