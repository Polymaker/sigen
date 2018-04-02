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
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.BackColor = System.Drawing.SystemColors.Window;
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbLayoutWidth, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblLayoutWidth, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.measureTextbox1, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.mtbShortFret, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.mtbLayoutHeight, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.mtbLongFret, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(280, 116);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 51);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Total Est. Fretwire";
            // 
            // mtbLayoutWidth
            // 
            this.mtbLayoutWidth.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbLayoutWidth.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.mtbLayoutWidth.BackColor = System.Drawing.SystemColors.Window;
            this.mtbLayoutWidth.HideBorders = true;
            this.mtbLayoutWidth.Location = new System.Drawing.Point(143, 4);
            this.mtbLayoutWidth.Name = "mtbLayoutWidth";
            this.mtbLayoutWidth.ReadOnly = true;
            this.mtbLayoutWidth.Size = new System.Drawing.Size(133, 16);
            this.mtbLayoutWidth.TabIndex = 0;
            // 
            // lblLayoutWidth
            // 
            this.lblLayoutWidth.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblLayoutWidth.AutoSize = true;
            this.lblLayoutWidth.Location = new System.Drawing.Point(4, 5);
            this.lblLayoutWidth.Name = "lblLayoutWidth";
            this.lblLayoutWidth.Size = new System.Drawing.Size(70, 13);
            this.lblLayoutWidth.TabIndex = 0;
            this.lblLayoutWidth.Text = "Layout Width";
            // 
            // measureTextbox1
            // 
            this.measureTextbox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.measureTextbox1.BackColor = System.Drawing.SystemColors.Window;
            this.measureTextbox1.HideBorders = true;
            this.measureTextbox1.Location = new System.Drawing.Point(143, 50);
            this.measureTextbox1.Name = "measureTextbox1";
            this.measureTextbox1.ReadOnly = true;
            this.measureTextbox1.Size = new System.Drawing.Size(133, 16);
            this.measureTextbox1.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Shortest Fret";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // mtbShortFret
            // 
            this.mtbShortFret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbShortFret.BackColor = System.Drawing.SystemColors.Window;
            this.mtbShortFret.HideBorders = true;
            this.mtbShortFret.Location = new System.Drawing.Point(143, 73);
            this.mtbShortFret.Name = "mtbShortFret";
            this.mtbShortFret.ReadOnly = true;
            this.mtbShortFret.Size = new System.Drawing.Size(133, 16);
            this.mtbShortFret.TabIndex = 3;
            // 
            // mtbLayoutHeight
            // 
            this.mtbLayoutHeight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbLayoutHeight.BackColor = System.Drawing.SystemColors.Window;
            this.mtbLayoutHeight.HideBorders = true;
            this.mtbLayoutHeight.Location = new System.Drawing.Point(143, 27);
            this.mtbLayoutHeight.Name = "mtbLayoutHeight";
            this.mtbLayoutHeight.ReadOnly = true;
            this.mtbLayoutHeight.Size = new System.Drawing.Size(133, 16);
            this.mtbLayoutHeight.TabIndex = 1;
            // 
            // mtbLongFret
            // 
            this.mtbLongFret.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbLongFret.BackColor = System.Drawing.SystemColors.Window;
            this.mtbLongFret.HideBorders = true;
            this.mtbLongFret.Location = new System.Drawing.Point(143, 96);
            this.mtbLongFret.Name = "mtbLongFret";
            this.mtbLongFret.ReadOnly = true;
            this.mtbLongFret.Size = new System.Drawing.Size(133, 16);
            this.mtbLongFret.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Layout Height";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 97);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Longest Fret";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // LayoutProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(150, 0);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "LayoutProperties";
            this.Size = new System.Drawing.Size(280, 193);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
    }
}
