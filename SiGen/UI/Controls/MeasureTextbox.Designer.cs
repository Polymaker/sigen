namespace SiGen.UI
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
            this.innerTextbox = new System.Windows.Forms.TextBox();
            this.cmsConvert = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.convertToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToCM = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToMM = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToIN = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiConvertToFT = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsConvert.SuspendLayout();
            this.SuspendLayout();
            // 
            // innerTextbox
            // 
            this.innerTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.innerTextbox.Location = new System.Drawing.Point(1, 0);
            this.innerTextbox.Name = "innerTextbox";
            this.innerTextbox.Size = new System.Drawing.Size(100, 13);
            this.innerTextbox.TabIndex = 0;
            this.innerTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.innerTextbox.Leave += new System.EventHandler(this.innerTextbox_Leave);
            this.innerTextbox.Validating += new System.ComponentModel.CancelEventHandler(this.innerTextbox_Validating);
            this.innerTextbox.Validated += new System.EventHandler(this.innerTextbox_Validated);
            // 
            // cmsConvert
            // 
            this.cmsConvert.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.convertToToolStripMenuItem,
            this.tsmiConvertToMM,
            this.tsmiConvertToCM,
            this.tsmiConvertToIN,
            this.tsmiConvertToFT});
            this.cmsConvert.Name = "cmsConvert";
            this.cmsConvert.Size = new System.Drawing.Size(139, 114);
            this.cmsConvert.Opening += new System.ComponentModel.CancelEventHandler(this.cmsConvert_Opening);
            // 
            // convertToToolStripMenuItem
            // 
            this.convertToToolStripMenuItem.Name = "convertToToolStripMenuItem";
            this.convertToToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.convertToToolStripMenuItem.Text = "Convert to:";
            // 
            // tsmiConvertToCM
            // 
            this.tsmiConvertToCM.Name = "tsmiConvertToCM";
            this.tsmiConvertToCM.Size = new System.Drawing.Size(152, 22);
            this.tsmiConvertToCM.Text = "Centimeters";
            this.tsmiConvertToCM.Click += new System.EventHandler(this.tsmiConvertToCM_Click);
            // 
            // tsmiConvertToMM
            // 
            this.tsmiConvertToMM.Name = "tsmiConvertToMM";
            this.tsmiConvertToMM.Size = new System.Drawing.Size(152, 22);
            this.tsmiConvertToMM.Text = "Millimeters";
            this.tsmiConvertToMM.Click += new System.EventHandler(this.tsmiConvertToMM_Click);
            // 
            // tsmiConvertToIN
            // 
            this.tsmiConvertToIN.Name = "tsmiConvertToIN";
            this.tsmiConvertToIN.Size = new System.Drawing.Size(152, 22);
            this.tsmiConvertToIN.Text = "Inches";
            this.tsmiConvertToIN.Click += new System.EventHandler(this.tsmiConvertToIN_Click);
            // 
            // tsmiConvertToFT
            // 
            this.tsmiConvertToFT.Name = "tsmiConvertToFT";
            this.tsmiConvertToFT.Size = new System.Drawing.Size(152, 22);
            this.tsmiConvertToFT.Text = "Feets";
            this.tsmiConvertToFT.Click += new System.EventHandler(this.tsmiConvertToFT_Click);
            // 
            // MeasureTextbox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ContextMenuStrip = this.cmsConvert;
            this.Controls.Add(this.innerTextbox);
            this.Name = "MeasureTextbox";
            this.Size = new System.Drawing.Size(111, 49);
            this.cmsConvert.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox innerTextbox;
        private System.Windows.Forms.ContextMenuStrip cmsConvert;
        private System.Windows.Forms.ToolStripMenuItem convertToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToMM;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToCM;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToIN;
        private System.Windows.Forms.ToolStripMenuItem tsmiConvertToFT;
    }
}
