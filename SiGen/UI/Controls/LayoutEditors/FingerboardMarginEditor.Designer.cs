namespace SiGen.UI.Controls
{
    partial class FingerboardMarginEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FingerboardMarginEditor));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblBridge = new System.Windows.Forms.Label();
            this.chkCompensateGauge = new System.Windows.Forms.CheckBox();
            this.lblNut = new System.Windows.Forms.Label();
            this.lblBass = new System.Windows.Forms.Label();
            this.mtbBridgeTreble = new SiGen.UI.Controls.MeasureTextbox();
            this.mtbBridgeBass = new SiGen.UI.Controls.MeasureTextbox();
            this.mtbNutTreble = new SiGen.UI.Controls.MeasureTextbox();
            this.mtbNutBass = new SiGen.UI.Controls.MeasureTextbox();
            this.mtbLastFret = new SiGen.UI.Controls.MeasureTextbox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblEditMode = new System.Windows.Forms.Label();
            this.lblTreble = new System.Windows.Forms.Label();
            this.cboMarginEditMode = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.lblBridge, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chkCompensateGauge, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblNut, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.lblBass, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.mtbBridgeTreble, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.mtbBridgeBass, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.mtbNutTreble, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbNutBass, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.mtbLastFret, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.lblEditMode, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblTreble, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.cboMarginEditMode, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // lblBridge
            // 
            resources.ApplyResources(this.lblBridge, "lblBridge");
            this.lblBridge.Name = "lblBridge";
            // 
            // chkCompensateGauge
            // 
            resources.ApplyResources(this.chkCompensateGauge, "chkCompensateGauge");
            this.tableLayoutPanel1.SetColumnSpan(this.chkCompensateGauge, 2);
            this.chkCompensateGauge.Name = "chkCompensateGauge";
            this.chkCompensateGauge.UseVisualStyleBackColor = true;
            this.chkCompensateGauge.CheckedChanged += new System.EventHandler(this.chkCompensateGauge_CheckedChanged);
            // 
            // lblNut
            // 
            resources.ApplyResources(this.lblNut, "lblNut");
            this.lblNut.Name = "lblNut";
            // 
            // lblBass
            // 
            resources.ApplyResources(this.lblBass, "lblBass");
            this.lblBass.Name = "lblBass";
            // 
            // mtbBridgeTreble
            // 
            resources.ApplyResources(this.mtbBridgeTreble, "mtbBridgeTreble");
            this.mtbBridgeTreble.Name = "mtbBridgeTreble";
            this.mtbBridgeTreble.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbBridgeBass
            // 
            resources.ApplyResources(this.mtbBridgeBass, "mtbBridgeBass");
            this.mtbBridgeBass.Name = "mtbBridgeBass";
            this.mtbBridgeBass.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbNutTreble
            // 
            resources.ApplyResources(this.mtbNutTreble, "mtbNutTreble");
            this.mtbNutTreble.Name = "mtbNutTreble";
            this.mtbNutTreble.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbNutBass
            // 
            resources.ApplyResources(this.mtbNutBass, "mtbNutBass");
            this.mtbNutBass.Name = "mtbNutBass";
            this.mtbNutBass.ValueChanged += new System.EventHandler(this.mtbFingerboardMargins_ValueChanged);
            // 
            // mtbLastFret
            // 
            this.mtbLastFret.AllowEmptyValue = true;
            resources.ApplyResources(this.mtbLastFret, "mtbLastFret");
            this.mtbLastFret.Name = "mtbLastFret";
            this.mtbLastFret.ValueChanged += new System.EventHandler(this.mtbLastFret_ValueChanged);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // lblEditMode
            // 
            resources.ApplyResources(this.lblEditMode, "lblEditMode");
            this.lblEditMode.Name = "lblEditMode";
            // 
            // lblTreble
            // 
            resources.ApplyResources(this.lblTreble, "lblTreble");
            this.lblTreble.Name = "lblTreble";
            // 
            // cboMarginEditMode
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.cboMarginEditMode, 2);
            this.cboMarginEditMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMarginEditMode.FormattingEnabled = true;
            resources.ApplyResources(this.cboMarginEditMode, "cboMarginEditMode");
            this.cboMarginEditMode.Name = "cboMarginEditMode";
            this.cboMarginEditMode.SelectedIndexChanged += new System.EventHandler(this.cboMarginEditMode_SelectedIndexChanged);
            // 
            // FingerboardMarginEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "FingerboardMarginEditor";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblEditMode;
        private MeasureTextbox mtbLastFret;
        private System.Windows.Forms.Label label2;
        private MeasureTextbox mtbBridgeTreble;
        private MeasureTextbox mtbBridgeBass;
        private MeasureTextbox mtbNutTreble;
        private MeasureTextbox mtbNutBass;
        private System.Windows.Forms.Label lblBridge;
        private System.Windows.Forms.Label lblNut;
        private System.Windows.Forms.Label lblBass;
        private System.Windows.Forms.Label lblTreble;
        private System.Windows.Forms.ComboBox cboMarginEditMode;
        private System.Windows.Forms.CheckBox chkCompensateGauge;
    }
}
