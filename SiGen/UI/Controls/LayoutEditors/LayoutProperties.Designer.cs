namespace SiGen.UI.Controls.LayoutEditors
{
    partial class LayoutProperties
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.mtbLayoutWidth = new SiGen.UI.MeasureTextbox();
            this.lblLayoutWidth = new System.Windows.Forms.Label();
            this.measureTextbox1 = new SiGen.UI.MeasureTextbox();
            this.label3 = new System.Windows.Forms.Label();
            this.mtbShortFret = new SiGen.UI.MeasureTextbox();
            this.mtbLayoutHeight = new SiGen.UI.MeasureTextbox();
            this.mtbLongFret = new SiGen.UI.MeasureTextbox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.mtbLayoutWidth, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLayoutWidth, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.measureTextbox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbShortFret, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbLayoutHeight, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.mtbLongFret, 2, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 147);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Fretwire";
            // 
            // mtbLayoutWidth
            // 
            this.mtbLayoutWidth.Location = new System.Drawing.Point(103, 3);
            this.mtbLayoutWidth.Name = "mtbLayoutWidth";
            this.mtbLayoutWidth.ReadOnly = true;
            this.mtbLayoutWidth.Size = new System.Drawing.Size(82, 20);
            this.mtbLayoutWidth.TabIndex = 2;
            // 
            // lblLayoutWidth
            // 
            this.lblLayoutWidth.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblLayoutWidth.AutoSize = true;
            this.lblLayoutWidth.Location = new System.Drawing.Point(35, 6);
            this.lblLayoutWidth.Name = "lblLayoutWidth";
            this.lblLayoutWidth.Size = new System.Drawing.Size(62, 13);
            this.lblLayoutWidth.TabIndex = 0;
            this.lblLayoutWidth.Text = "Layout Size";
            // 
            // measureTextbox1
            // 
            this.measureTextbox1.Location = new System.Drawing.Point(103, 29);
            this.measureTextbox1.Name = "measureTextbox1";
            this.measureTextbox1.ReadOnly = true;
            this.measureTextbox1.Size = new System.Drawing.Size(82, 20);
            this.measureTextbox1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 26);
            this.label3.TabIndex = 5;
            this.label3.Text = "Shortest/Longest Fret";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mtbShortFret
            // 
            this.mtbShortFret.Location = new System.Drawing.Point(103, 55);
            this.mtbShortFret.Name = "mtbShortFret";
            this.mtbShortFret.ReadOnly = true;
            this.mtbShortFret.Size = new System.Drawing.Size(82, 20);
            this.mtbShortFret.TabIndex = 6;
            // 
            // mtbLayoutHeight
            // 
            this.mtbLayoutHeight.Location = new System.Drawing.Point(193, 3);
            this.mtbLayoutHeight.Name = "mtbLayoutHeight";
            this.mtbLayoutHeight.ReadOnly = true;
            this.mtbLayoutHeight.Size = new System.Drawing.Size(82, 20);
            this.mtbLayoutHeight.TabIndex = 1;
            // 
            // mtbLongFret
            // 
            this.mtbLongFret.Location = new System.Drawing.Point(193, 55);
            this.mtbLongFret.Name = "mtbLongFret";
            this.mtbLongFret.ReadOnly = true;
            this.mtbLongFret.Size = new System.Drawing.Size(82, 20);
            this.mtbLongFret.TabIndex = 7;
            // 
            // LayoutProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LayoutProperties";
            this.Size = new System.Drawing.Size(280, 193);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblLayoutWidth;
        private MeasureTextbox mtbLayoutHeight;
        private MeasureTextbox mtbLayoutWidth;
        private System.Windows.Forms.Label label1;
        private MeasureTextbox measureTextbox1;
        private System.Windows.Forms.Label label3;
        private MeasureTextbox mtbShortFret;
        private MeasureTextbox mtbLongFret;
    }
}
