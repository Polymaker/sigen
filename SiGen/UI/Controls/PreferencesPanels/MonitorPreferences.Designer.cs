namespace SiGen.UI.Controls.PreferencesPanels
{
    partial class MonitorPreferences
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
            this.tlpLayout = new System.Windows.Forms.TableLayoutPanel();
            this.gbxPreviewDPI = new System.Windows.Forms.GroupBox();
            this.pbxPreviewDPI = new System.Windows.Forms.PictureBox();
            this.tlpLayout.SuspendLayout();
            this.gbxPreviewDPI.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbxPreviewDPI)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpLayout
            // 
            this.tlpLayout.ColumnCount = 2;
            this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpLayout.Controls.Add(this.gbxPreviewDPI, 1, 0);
            this.tlpLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpLayout.Location = new System.Drawing.Point(0, 0);
            this.tlpLayout.Name = "tlpLayout";
            this.tlpLayout.RowCount = 2;
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpLayout.Size = new System.Drawing.Size(625, 235);
            this.tlpLayout.TabIndex = 0;
            // 
            // gbxPreviewDPI
            // 
            this.gbxPreviewDPI.Controls.Add(this.pbxPreviewDPI);
            this.gbxPreviewDPI.Location = new System.Drawing.Point(429, 3);
            this.gbxPreviewDPI.Name = "gbxPreviewDPI";
            this.tlpLayout.SetRowSpan(this.gbxPreviewDPI, 2);
            this.gbxPreviewDPI.Size = new System.Drawing.Size(193, 181);
            this.gbxPreviewDPI.TabIndex = 14;
            this.gbxPreviewDPI.TabStop = false;
            this.gbxPreviewDPI.Text = "Preview";
            // 
            // pbxPreviewDPI
            // 
            this.pbxPreviewDPI.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbxPreviewDPI.Location = new System.Drawing.Point(3, 16);
            this.pbxPreviewDPI.Name = "pbxPreviewDPI";
            this.pbxPreviewDPI.Size = new System.Drawing.Size(187, 162);
            this.pbxPreviewDPI.TabIndex = 0;
            this.pbxPreviewDPI.TabStop = false;
            this.pbxPreviewDPI.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxPreviewDPI_Paint);
            // 
            // MonitorPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tlpLayout);
            this.Name = "MonitorPreferences";
            this.Size = new System.Drawing.Size(625, 235);
            this.tlpLayout.ResumeLayout(false);
            this.gbxPreviewDPI.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbxPreviewDPI)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpLayout;
        private System.Windows.Forms.GroupBox gbxPreviewDPI;
        private System.Windows.Forms.PictureBox pbxPreviewDPI;
    }
}
