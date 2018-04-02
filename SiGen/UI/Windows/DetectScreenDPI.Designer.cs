namespace SiGen.UI.Windows
{
    partial class DetectScreenDPI
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.pbxMeasureDPI = new System.Windows.Forms.PictureBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboScreens = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.rbCM = new System.Windows.Forms.RadioButton();
            this.rbIN = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numDPI = new SiGen.UI.Controls.NumericBox();
            this.measureTextbox1 = new SiGen.UI.MeasureTextbox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)(this.pbxMeasureDPI)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current DPI";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(96, 115);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 22);
            this.button1.TabIndex = 3;
            this.button1.Text = "Calculate DPI";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pbxMeasureDPI
            // 
            this.pbxMeasureDPI.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.pbxMeasureDPI.Location = new System.Drawing.Point(20, 7);
            this.pbxMeasureDPI.Name = "pbxMeasureDPI";
            this.pbxMeasureDPI.Size = new System.Drawing.Size(128, 122);
            this.pbxMeasureDPI.TabIndex = 5;
            this.pbxMeasureDPI.TabStop = false;
            this.pbxMeasureDPI.Paint += new System.Windows.Forms.PaintEventHandler(this.pbxMeasureDPI_Paint);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Current Screen";
            // 
            // cboScreens
            // 
            this.cboScreens.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScreens.FormattingEnabled = true;
            this.cboScreens.Location = new System.Drawing.Point(96, 15);
            this.cboScreens.Name = "cboScreens";
            this.cboScreens.Size = new System.Drawing.Size(181, 21);
            this.cboScreens.TabIndex = 7;
            this.cboScreens.SelectedIndexChanged += new System.EventHandler(this.cboScreens_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(115, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Screen Size (Diagonal)";
            // 
            // rbCM
            // 
            this.rbCM.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.rbCM.AutoSize = true;
            this.rbCM.Checked = true;
            this.rbCM.Location = new System.Drawing.Point(3, 3);
            this.rbCM.Name = "rbCM";
            this.rbCM.Size = new System.Drawing.Size(75, 17);
            this.rbCM.TabIndex = 10;
            this.rbCM.TabStop = true;
            this.rbCM.Text = "Centimeter";
            this.rbCM.UseVisualStyleBackColor = true;
            this.rbCM.CheckedChanged += new System.EventHandler(this.rbPreviewSize_CheckChanged);
            // 
            // rbIN
            // 
            this.rbIN.AutoSize = true;
            this.rbIN.Location = new System.Drawing.Point(84, 3);
            this.rbIN.Name = "rbIN";
            this.rbIN.Size = new System.Drawing.Size(46, 17);
            this.rbIN.TabIndex = 11;
            this.rbIN.TabStop = true;
            this.rbIN.Text = "Inch";
            this.rbIN.UseVisualStyleBackColor = true;
            this.rbIN.CheckedChanged += new System.EventHandler(this.rbPreviewSize_CheckChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.pbxMeasureDPI, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(168, 160);
            this.tableLayoutPanel1.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.tableLayoutPanel1);
            this.groupBox1.Location = new System.Drawing.Point(283, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(174, 179);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Preview";
            // 
            // numDPI
            // 
            this.numDPI.Location = new System.Drawing.Point(96, 43);
            this.numDPI.MaximumValue = 500D;
            this.numDPI.Name = "numDPI";
            this.numDPI.Size = new System.Drawing.Size(75, 20);
            this.numDPI.TabIndex = 8;
            this.numDPI.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numDPI.ValueChanged += new System.EventHandler(this.numDPI_ValueChanged);
            // 
            // measureTextbox1
            // 
            this.measureTextbox1.Location = new System.Drawing.Point(96, 91);
            this.measureTextbox1.Name = "measureTextbox1";
            this.measureTextbox1.Size = new System.Drawing.Size(100, 20);
            this.measureTextbox1.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.rbCM);
            this.flowLayoutPanel1.Controls.Add(this.rbIN);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(17, 137);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(133, 23);
            this.flowLayoutPanel1.TabIndex = 6;
            // 
            // DetectScreenDPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(469, 195);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.numDPI);
            this.Controls.Add(this.cboScreens);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.measureTextbox1);
            this.Controls.Add(this.groupBox1);
            this.Name = "DetectScreenDPI";
            this.Text = "Adjust Screen DPI";
            ((System.ComponentModel.ISupportInitialize)(this.pbxMeasureDPI)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MeasureTextbox measureTextbox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pbxMeasureDPI;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboScreens;
        private Controls.NumericBox numDPI;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbCM;
        private System.Windows.Forms.RadioButton rbIN;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}