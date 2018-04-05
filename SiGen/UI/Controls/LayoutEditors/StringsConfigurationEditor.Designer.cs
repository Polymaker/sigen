namespace SiGen.UI.Controls.LayoutEditors
{
    partial class StringsConfigurationEditor
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
            this.numericBox1 = new SiGen.UI.Controls.NumericBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // numericBox1
            // 
            this.numericBox1.AllowDecimals = false;
            this.numericBox1.Location = new System.Drawing.Point(102, 3);
            this.numericBox1.MaximumValue = 40D;
            this.numericBox1.MinimumValue = 1D;
            this.numericBox1.Name = "numericBox1";
            this.numericBox1.Size = new System.Drawing.Size(66, 20);
            this.numericBox1.TabIndex = 0;
            this.numericBox1.Value = 1D;
            this.numericBox1.ValueChanged += new System.EventHandler(this.numericBox1_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 7);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number Of Strings";
            // 
            // StringsConfigurationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numericBox1);
            this.Name = "StringsConfigurationEditor";
            this.Size = new System.Drawing.Size(329, 126);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericBox numericBox1;
        private System.Windows.Forms.Label label1;
    }
}
