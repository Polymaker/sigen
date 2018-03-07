namespace SiGen.UI
{
    partial class FrmLayoutBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLayoutBuilder));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.panel1 = new System.Windows.Forms.Panel();
            this.gbxStringSpacing = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.meSpacingBridge2 = new SiGen.UI.MeasureEdit();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.meSpacingBridge1 = new SiGen.UI.MeasureEdit();
            this.label6 = new System.Windows.Forms.Label();
            this.meSpacingNut2 = new SiGen.UI.MeasureEdit();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.meSpacingNut1 = new SiGen.UI.MeasureEdit();
            this.gbxScaleLength = new System.Windows.Forms.GroupBox();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.tlpScaleLenghts = new System.Windows.Forms.TableLayoutPanel();
            this.lblMultiScaleRatio = new System.Windows.Forms.Label();
            this.lblBassLength = new System.Windows.Forms.Label();
            this.lblTrebleLength = new System.Windows.Forms.Label();
            this.lblScaleLength = new System.Windows.Forms.Label();
            this.nudMultiScaleOffset = new System.Windows.Forms.NumericUpDown();
            this.meSingleScale = new SiGen.UI.MeasureEdit();
            this.meBassScale = new SiGen.UI.MeasureEdit();
            this.meTrebleScale = new SiGen.UI.MeasureEdit();
            this.rbMultiScale = new System.Windows.Forms.RadioButton();
            this.rbSingleScale = new System.Windows.Forms.RadioButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.nudNumberOfFrets = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudNumberOfStrings = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.layoutViewer1 = new SiGen.UI.LayoutViewer();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbNew = new System.Windows.Forms.ToolStripButton();
            this.tsbOpen = new System.Windows.Forms.ToolStripButton();
            this.tssbSave = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiSave = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiSaveAs = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSplitButton2 = new System.Windows.Forms.ToolStripSplitButton();
            this.exportAsSVGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAsDXFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gbxStringSpacing.SuspendLayout();
            this.gbxScaleLength.SuspendLayout();
            this.tlpScaleLenghts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMultiScaleOffset)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfFrets)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfStrings)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.layoutViewer1);
            this.splitContainer1.Size = new System.Drawing.Size(820, 428);
            this.splitContainer1.SplitterDistance = 331;
            this.splitContainer1.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.gbxStringSpacing);
            this.panel1.Controls.Add(this.gbxScaleLength);
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(329, 426);
            this.panel1.TabIndex = 6;
            // 
            // gbxStringSpacing
            // 
            this.gbxStringSpacing.Controls.Add(this.button3);
            this.gbxStringSpacing.Controls.Add(this.label7);
            this.gbxStringSpacing.Controls.Add(this.meSpacingBridge2);
            this.gbxStringSpacing.Controls.Add(this.label8);
            this.gbxStringSpacing.Controls.Add(this.label9);
            this.gbxStringSpacing.Controls.Add(this.meSpacingBridge1);
            this.gbxStringSpacing.Controls.Add(this.label6);
            this.gbxStringSpacing.Controls.Add(this.meSpacingNut2);
            this.gbxStringSpacing.Controls.Add(this.label5);
            this.gbxStringSpacing.Controls.Add(this.label4);
            this.gbxStringSpacing.Controls.Add(this.meSpacingNut1);
            this.gbxStringSpacing.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxStringSpacing.Location = new System.Drawing.Point(3, 243);
            this.gbxStringSpacing.Name = "gbxStringSpacing";
            this.gbxStringSpacing.Size = new System.Drawing.Size(323, 157);
            this.gbxStringSpacing.TabIndex = 5;
            this.gbxStringSpacing.TabStop = false;
            this.gbxStringSpacing.Text = "String Spacing";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(242, 62);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 15;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(31, 129);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(72, 13);
            this.label7.TabIndex = 14;
            this.label7.Text = "String spread:";
            // 
            // meSpacingBridge2
            // 
            this.meSpacingBridge2.Location = new System.Drawing.Point(106, 126);
            this.meSpacingBridge2.Name = "meSpacingBridge2";
            this.meSpacingBridge2.Size = new System.Drawing.Size(94, 21);
            this.meSpacingBridge2.TabIndex = 13;
            this.meSpacingBridge2.ValueChanged += new System.EventHandler(this.StringSpacingChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(18, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(85, 13);
            this.label8.TabIndex = 12;
            this.label8.Text = "Between strings:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(9, 86);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(111, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Spacing at bridge:";
            // 
            // meSpacingBridge1
            // 
            this.meSpacingBridge1.Location = new System.Drawing.Point(106, 102);
            this.meSpacingBridge1.Name = "meSpacingBridge1";
            this.meSpacingBridge1.Size = new System.Drawing.Size(94, 21);
            this.meSpacingBridge1.TabIndex = 11;
            this.meSpacingBridge1.ValueChanged += new System.EventHandler(this.StringSpacingChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 9;
            this.label6.Text = "String spread:";
            // 
            // meSpacingNut2
            // 
            this.meSpacingNut2.Location = new System.Drawing.Point(106, 59);
            this.meSpacingNut2.Name = "meSpacingNut2";
            this.meSpacingNut2.Size = new System.Drawing.Size(94, 21);
            this.meSpacingNut2.TabIndex = 8;
            this.meSpacingNut2.ValueChanged += new System.EventHandler(this.StringSpacingChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(18, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Between strings:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(9, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(94, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Spacing at nut:";
            // 
            // meSpacingNut1
            // 
            this.meSpacingNut1.Location = new System.Drawing.Point(106, 35);
            this.meSpacingNut1.Name = "meSpacingNut1";
            this.meSpacingNut1.Size = new System.Drawing.Size(94, 21);
            this.meSpacingNut1.TabIndex = 6;
            this.meSpacingNut1.ValueChanged += new System.EventHandler(this.StringSpacingChanged);
            // 
            // gbxScaleLength
            // 
            this.gbxScaleLength.Controls.Add(this.radioButton1);
            this.gbxScaleLength.Controls.Add(this.tlpScaleLenghts);
            this.gbxScaleLength.Controls.Add(this.rbMultiScale);
            this.gbxScaleLength.Controls.Add(this.rbSingleScale);
            this.gbxScaleLength.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbxScaleLength.Location = new System.Drawing.Point(3, 81);
            this.gbxScaleLength.Name = "gbxScaleLength";
            this.gbxScaleLength.Size = new System.Drawing.Size(323, 162);
            this.gbxScaleLength.TabIndex = 4;
            this.gbxScaleLength.TabStop = false;
            this.gbxScaleLength.Text = "Scale Length Configuration";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(173, 20);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(70, 17);
            this.radioButton1.TabIndex = 6;
            this.radioButton1.Text = "Individual";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // tlpScaleLenghts
            // 
            this.tlpScaleLenghts.AutoSize = true;
            this.tlpScaleLenghts.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpScaleLenghts.ColumnCount = 2;
            this.tlpScaleLenghts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tlpScaleLenghts.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 59.5F));
            this.tlpScaleLenghts.Controls.Add(this.lblMultiScaleRatio, 0, 3);
            this.tlpScaleLenghts.Controls.Add(this.lblBassLength, 0, 2);
            this.tlpScaleLenghts.Controls.Add(this.lblTrebleLength, 0, 1);
            this.tlpScaleLenghts.Controls.Add(this.lblScaleLength, 0, 0);
            this.tlpScaleLenghts.Controls.Add(this.nudMultiScaleOffset, 1, 3);
            this.tlpScaleLenghts.Controls.Add(this.meSingleScale, 1, 0);
            this.tlpScaleLenghts.Controls.Add(this.meBassScale, 1, 2);
            this.tlpScaleLenghts.Controls.Add(this.meTrebleScale, 1, 1);
            this.tlpScaleLenghts.Location = new System.Drawing.Point(6, 43);
            this.tlpScaleLenghts.Name = "tlpScaleLenghts";
            this.tlpScaleLenghts.RowCount = 4;
            this.tlpScaleLenghts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpScaleLenghts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpScaleLenghts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpScaleLenghts.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpScaleLenghts.Size = new System.Drawing.Size(205, 99);
            this.tlpScaleLenghts.TabIndex = 5;
            // 
            // lblMultiScaleRatio
            // 
            this.lblMultiScaleRatio.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblMultiScaleRatio.AutoSize = true;
            this.lblMultiScaleRatio.Location = new System.Drawing.Point(3, 80);
            this.lblMultiScaleRatio.Name = "lblMultiScaleRatio";
            this.lblMultiScaleRatio.Size = new System.Drawing.Size(81, 13);
            this.lblMultiScaleRatio.TabIndex = 9;
            this.lblMultiScaleRatio.Text = "Alignment Ratio";
            // 
            // lblBassLength
            // 
            this.lblBassLength.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBassLength.AutoSize = true;
            this.lblBassLength.Location = new System.Drawing.Point(18, 56);
            this.lblBassLength.Name = "lblBassLength";
            this.lblBassLength.Size = new System.Drawing.Size(66, 13);
            this.lblBassLength.TabIndex = 8;
            this.lblBassLength.Text = "Bass Length";
            // 
            // lblTrebleLength
            // 
            this.lblTrebleLength.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblTrebleLength.AutoSize = true;
            this.lblTrebleLength.Location = new System.Drawing.Point(11, 31);
            this.lblTrebleLength.Name = "lblTrebleLength";
            this.lblTrebleLength.Size = new System.Drawing.Size(73, 13);
            this.lblTrebleLength.TabIndex = 7;
            this.lblTrebleLength.Text = "Treble Length";
            // 
            // lblScaleLength
            // 
            this.lblScaleLength.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblScaleLength.AutoSize = true;
            this.lblScaleLength.Location = new System.Drawing.Point(14, 6);
            this.lblScaleLength.Name = "lblScaleLength";
            this.lblScaleLength.Size = new System.Drawing.Size(70, 13);
            this.lblScaleLength.TabIndex = 6;
            this.lblScaleLength.Text = "Scale Length";
            // 
            // nudMultiScaleOffset
            // 
            this.nudMultiScaleOffset.DecimalPlaces = 1;
            this.nudMultiScaleOffset.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMultiScaleOffset.Location = new System.Drawing.Point(89, 77);
            this.nudMultiScaleOffset.Margin = new System.Windows.Forms.Padding(2);
            this.nudMultiScaleOffset.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudMultiScaleOffset.Name = "nudMultiScaleOffset";
            this.nudMultiScaleOffset.Size = new System.Drawing.Size(72, 20);
            this.nudMultiScaleOffset.TabIndex = 5;
            this.nudMultiScaleOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudMultiScaleOffset.ValueChanged += new System.EventHandler(this.nudMultiScaleOffset_ValueChanged);
            // 
            // meSingleScale
            // 
            this.meSingleScale.Location = new System.Drawing.Point(89, 2);
            this.meSingleScale.Margin = new System.Windows.Forms.Padding(2);
            this.meSingleScale.Name = "meSingleScale";
            this.meSingleScale.Size = new System.Drawing.Size(114, 21);
            this.meSingleScale.TabIndex = 2;
            this.meSingleScale.ValueChanged += new System.EventHandler(this.meSingleScale_ValueChanged);
            // 
            // meBassScale
            // 
            this.meBassScale.Location = new System.Drawing.Point(89, 52);
            this.meBassScale.Margin = new System.Windows.Forms.Padding(2);
            this.meBassScale.Name = "meBassScale";
            this.meBassScale.Size = new System.Drawing.Size(114, 21);
            this.meBassScale.TabIndex = 6;
            this.meBassScale.ValueChanged += new System.EventHandler(this.meBassScale_ValueChanged);
            // 
            // meTrebleScale
            // 
            this.meTrebleScale.Location = new System.Drawing.Point(89, 27);
            this.meTrebleScale.Margin = new System.Windows.Forms.Padding(2);
            this.meTrebleScale.Name = "meTrebleScale";
            this.meTrebleScale.Size = new System.Drawing.Size(114, 21);
            this.meTrebleScale.TabIndex = 5;
            this.meTrebleScale.ValueChanged += new System.EventHandler(this.meTrebleScale_ValueChanged);
            // 
            // rbMultiScale
            // 
            this.rbMultiScale.AutoSize = true;
            this.rbMultiScale.Location = new System.Drawing.Point(92, 20);
            this.rbMultiScale.Name = "rbMultiScale";
            this.rbMultiScale.Size = new System.Drawing.Size(75, 17);
            this.rbMultiScale.TabIndex = 4;
            this.rbMultiScale.Text = "Dual-scale";
            this.rbMultiScale.UseVisualStyleBackColor = true;
            this.rbMultiScale.CheckedChanged += new System.EventHandler(this.ScaleLengthMode_CheckedChanged);
            // 
            // rbSingleScale
            // 
            this.rbSingleScale.AutoSize = true;
            this.rbSingleScale.Location = new System.Drawing.Point(6, 20);
            this.rbSingleScale.Name = "rbSingleScale";
            this.rbSingleScale.Size = new System.Drawing.Size(82, 17);
            this.rbSingleScale.TabIndex = 3;
            this.rbSingleScale.Text = "Single-scale";
            this.rbSingleScale.UseVisualStyleBackColor = true;
            this.rbSingleScale.CheckedChanged += new System.EventHandler(this.ScaleLengthMode_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.nudNumberOfFrets);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.nudNumberOfStrings);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(323, 78);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Strings Configuration";
            // 
            // nudNumberOfFrets
            // 
            this.nudNumberOfFrets.Location = new System.Drawing.Point(113, 47);
            this.nudNumberOfFrets.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudNumberOfFrets.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumberOfFrets.Name = "nudNumberOfFrets";
            this.nudNumberOfFrets.Size = new System.Drawing.Size(57, 20);
            this.nudNumberOfFrets.TabIndex = 3;
            this.nudNumberOfFrets.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumberOfFrets.ValueChanged += new System.EventHandler(this.nudNumberOfFrets_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(79, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Number of frets";
            // 
            // nudNumberOfStrings
            // 
            this.nudNumberOfStrings.Location = new System.Drawing.Point(113, 23);
            this.nudNumberOfStrings.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudNumberOfStrings.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumberOfStrings.Name = "nudNumberOfStrings";
            this.nudNumberOfStrings.Size = new System.Drawing.Size(57, 20);
            this.nudNumberOfStrings.TabIndex = 1;
            this.nudNumberOfStrings.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudNumberOfStrings.ValueChanged += new System.EventHandler(this.nudNumberOfStrings_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of strings";
            // 
            // layoutViewer1
            // 
            this.layoutViewer1.BackColor = System.Drawing.Color.White;
            this.layoutViewer1.CurrentLayout = null;
            this.layoutViewer1.DisplayConfig.FretWidth = new SiGen.Measuring.Measure(2.5D, SiGen.Measuring.UnitOfMeasure.Millimeters);
            this.layoutViewer1.DisplayConfig.RenderRealStrings = true;
            this.layoutViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layoutViewer1.Location = new System.Drawing.Point(0, 0);
            this.layoutViewer1.Name = "layoutViewer1";
            this.layoutViewer1.Size = new System.Drawing.Size(483, 426);
            this.layoutViewer1.TabIndex = 0;
            this.layoutViewer1.Text = "layoutViewer1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 22);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(85, 50);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 7;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbNew,
            this.tsbOpen,
            this.tssbSave,
            this.toolStripSplitButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(820, 25);
            this.toolStrip1.Stretch = true;
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbNew
            // 
            this.tsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tsbNew.Image")));
            this.tsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNew.Name = "tsbNew";
            this.tsbNew.Size = new System.Drawing.Size(90, 22);
            this.tsbNew.Text = "New Layout";
            // 
            // tsbOpen
            // 
            this.tsbOpen.Image = ((System.Drawing.Image)(resources.GetObject("tsbOpen.Image")));
            this.tsbOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbOpen.Name = "tsbOpen";
            this.tsbOpen.Size = new System.Drawing.Size(56, 22);
            this.tsbOpen.Text = "Open";
            // 
            // tssbSave
            // 
            this.tssbSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSave,
            this.tsmiSaveAs});
            this.tssbSave.Image = ((System.Drawing.Image)(resources.GetObject("tssbSave.Image")));
            this.tssbSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbSave.Name = "tssbSave";
            this.tssbSave.Size = new System.Drawing.Size(63, 22);
            this.tssbSave.Text = "Save";
            this.tssbSave.ButtonClick += new System.EventHandler(this.tssbSave_ButtonClick);
            // 
            // tsmiSave
            // 
            this.tsmiSave.Name = "tsmiSave";
            this.tsmiSave.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.tsmiSave.Size = new System.Drawing.Size(186, 22);
            this.tsmiSave.Text = "Save";
            this.tsmiSave.Click += new System.EventHandler(this.tsmiSave_Click);
            // 
            // tsmiSaveAs
            // 
            this.tsmiSaveAs.Name = "tsmiSaveAs";
            this.tsmiSaveAs.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.tsmiSaveAs.Size = new System.Drawing.Size(186, 22);
            this.tsmiSaveAs.Text = "Save As...";
            this.tsmiSaveAs.Click += new System.EventHandler(this.tsmiSaveAs_Click);
            // 
            // toolStripSplitButton2
            // 
            this.toolStripSplitButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportAsSVGToolStripMenuItem,
            this.exportAsDXFToolStripMenuItem});
            this.toolStripSplitButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripSplitButton2.Image")));
            this.toolStripSplitButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton2.Name = "toolStripSplitButton2";
            this.toolStripSplitButton2.Size = new System.Drawing.Size(72, 22);
            this.toolStripSplitButton2.Text = "Export";
            // 
            // exportAsSVGToolStripMenuItem
            // 
            this.exportAsSVGToolStripMenuItem.Name = "exportAsSVGToolStripMenuItem";
            this.exportAsSVGToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exportAsSVGToolStripMenuItem.Text = "Export as SVG...";
            this.exportAsSVGToolStripMenuItem.Click += new System.EventHandler(this.exportAsSVGToolStripMenuItem_Click);
            // 
            // exportAsDXFToolStripMenuItem
            // 
            this.exportAsDXFToolStripMenuItem.Name = "exportAsDXFToolStripMenuItem";
            this.exportAsDXFToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exportAsDXFToolStripMenuItem.Text = "Export as DXF...";
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(820, 428);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(820, 453);
            this.toolStripContainer1.TabIndex = 3;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.toolStrip1);
            // 
            // FrmLayoutBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 453);
            this.Controls.Add(this.toolStripContainer1);
            this.Name = "FrmLayoutBuilder";
            this.Text = "Stringed Instrument Layout Generator";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.gbxStringSpacing.ResumeLayout(false);
            this.gbxStringSpacing.PerformLayout();
            this.gbxScaleLength.ResumeLayout(false);
            this.gbxScaleLength.PerformLayout();
            this.tlpScaleLenghts.ResumeLayout(false);
            this.tlpScaleLenghts.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMultiScaleOffset)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfFrets)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNumberOfStrings)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private LayoutViewer layoutViewer1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.NumericUpDown nudNumberOfStrings;
        private System.Windows.Forms.Label label1;
        private MeasureEdit meSingleScale;
        private System.Windows.Forms.GroupBox gbxScaleLength;
        private System.Windows.Forms.RadioButton rbMultiScale;
        private System.Windows.Forms.RadioButton rbSingleScale;
        private MeasureEdit meBassScale;
        private MeasureEdit meTrebleScale;
        private System.Windows.Forms.NumericUpDown nudMultiScaleOffset;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TableLayoutPanel tlpScaleLenghts;
        private System.Windows.Forms.Label lblBassLength;
        private System.Windows.Forms.Label lblTrebleLength;
        private System.Windows.Forms.Label lblScaleLength;
        private System.Windows.Forms.GroupBox gbxStringSpacing;
        private System.Windows.Forms.Label label7;
        private MeasureEdit meSpacingBridge2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private MeasureEdit meSpacingBridge1;
        private System.Windows.Forms.Label label6;
        private MeasureEdit meSpacingNut2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private MeasureEdit meSpacingNut1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown nudNumberOfFrets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblMultiScaleRatio;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsbNew;
        private System.Windows.Forms.ToolStripSplitButton tssbSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSave;
        private System.Windows.Forms.ToolStripMenuItem tsmiSaveAs;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton2;
        private System.Windows.Forms.ToolStripMenuItem exportAsSVGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportAsDXFToolStripMenuItem;
        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.ToolStripButton tsbOpen;
    }
}