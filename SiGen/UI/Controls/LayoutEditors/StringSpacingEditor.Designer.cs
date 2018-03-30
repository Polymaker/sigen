namespace SiGen.UI.Controls.LayoutEditors
{
    partial class StringSpacingEditor
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
            this.mtbNutSpacing = new SiGen.UI.MeasureTextbox();
            this.mtbNutSpread = new SiGen.UI.MeasureTextbox();
            this.mtbBridgeSpacing = new SiGen.UI.MeasureTextbox();
            this.mtbBridgeSpread = new SiGen.UI.MeasureTextbox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboNutSpacingMethod = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.chkAlignCenter = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // mtbNutSpacing
            // 
            this.mtbNutSpacing.Location = new System.Drawing.Point(93, 19);
            this.mtbNutSpacing.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbNutSpacing.Name = "mtbNutSpacing";
            this.mtbNutSpacing.Size = new System.Drawing.Size(100, 20);
            this.mtbNutSpacing.TabIndex = 0;
            this.mtbNutSpacing.ValueChanged += new System.EventHandler(this.mtbNutSpacing_ValueChanged);
            // 
            // mtbNutSpread
            // 
            this.mtbNutSpread.Location = new System.Drawing.Point(93, 42);
            this.mtbNutSpread.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbNutSpread.Name = "mtbNutSpread";
            this.mtbNutSpread.Size = new System.Drawing.Size(100, 20);
            this.mtbNutSpread.TabIndex = 1;
            this.mtbNutSpread.ValueChanged += new System.EventHandler(this.mtbNutSpread_ValueChanged);
            // 
            // mtbBridgeSpacing
            // 
            this.mtbBridgeSpacing.Location = new System.Drawing.Point(93, 19);
            this.mtbBridgeSpacing.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBridgeSpacing.Name = "mtbBridgeSpacing";
            this.mtbBridgeSpacing.Size = new System.Drawing.Size(100, 20);
            this.mtbBridgeSpacing.TabIndex = 2;
            this.mtbBridgeSpacing.ValueChanged += new System.EventHandler(this.mtbBridgeSpacing_ValueChanged);
            // 
            // mtbBridgeSpread
            // 
            this.mtbBridgeSpread.Location = new System.Drawing.Point(93, 42);
            this.mtbBridgeSpread.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBridgeSpread.Name = "mtbBridgeSpread";
            this.mtbBridgeSpread.Size = new System.Drawing.Size(100, 20);
            this.mtbBridgeSpread.TabIndex = 3;
            this.mtbBridgeSpread.ValueChanged += new System.EventHandler(this.mtbBridgeSpread_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Between Strings";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.cboNutSpacingMethod);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.chkAlignCenter);
            this.groupBox1.Controls.Add(this.mtbNutSpread);
            this.groupBox1.Controls.Add(this.mtbNutSpacing);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 92);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Nut Spacing";
            // 
            // cboNutSpacingMethod
            // 
            this.cboNutSpacingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNutSpacingMethod.FormattingEnabled = true;
            this.cboNutSpacingMethod.Items.AddRange(new object[] {
            "Center to center",
            "Between strings"});
            this.cboNutSpacingMethod.Location = new System.Drawing.Point(93, 65);
            this.cboNutSpacingMethod.Name = "cboNutSpacingMethod";
            this.cboNutSpacingMethod.Size = new System.Drawing.Size(100, 21);
            this.cboNutSpacingMethod.TabIndex = 10;
            this.cboNutSpacingMethod.SelectedIndexChanged += new System.EventHandler(this.cboNutSpacingMethod_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(78, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Layout Method";
            // 
            // chkAlignCenter
            // 
            this.chkAlignCenter.AutoSize = true;
            this.chkAlignCenter.Location = new System.Drawing.Point(199, 67);
            this.chkAlignCenter.Name = "chkAlignCenter";
            this.chkAlignCenter.Size = new System.Drawing.Size(105, 17);
            this.chkAlignCenter.TabIndex = 8;
            this.chkAlignCenter.Text = "Adjust Centerline";
            this.chkAlignCenter.UseVisualStyleBackColor = true;
            this.chkAlignCenter.CheckedChanged += new System.EventHandler(this.chkAlignCenter_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Total Spread";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.mtbBridgeSpacing);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.mtbBridgeSpread);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(3, 101);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 67);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bridge Spacing";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Total Spread";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(84, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Between Strings";
            // 
            // StringSpacingEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "StringSpacingEditor";
            this.Size = new System.Drawing.Size(310, 211);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private MeasureTextbox mtbNutSpacing;
        private MeasureTextbox mtbNutSpread;
        private MeasureTextbox mtbBridgeSpacing;
        private MeasureTextbox mtbBridgeSpread;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkAlignCenter;
        private System.Windows.Forms.ComboBox cboNutSpacingMethod;
        private System.Windows.Forms.Label label5;
    }
}
