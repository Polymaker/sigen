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
            this.nbxNumberOfStrings = new SiGen.UI.Controls.NumericBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkLeftHanded = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // nbxNumberOfStrings
            // 
            this.nbxNumberOfStrings.AllowDecimals = false;
            this.nbxNumberOfStrings.Location = new System.Drawing.Point(102, 3);
            this.nbxNumberOfStrings.MaximumValue = 40D;
            this.nbxNumberOfStrings.MinimumValue = 1D;
            this.nbxNumberOfStrings.Name = "nbxNumberOfStrings";
            this.nbxNumberOfStrings.Size = new System.Drawing.Size(66, 20);
            this.nbxNumberOfStrings.TabIndex = 0;
            this.nbxNumberOfStrings.Value = 1D;
            this.nbxNumberOfStrings.ValueChanged += new System.EventHandler(this.nbxNumberOfStrings_ValueChanged);
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
            // chkLeftHanded
            // 
            this.chkLeftHanded.AutoSize = true;
            this.chkLeftHanded.Location = new System.Drawing.Point(174, 6);
            this.chkLeftHanded.Name = "chkLeftHanded";
            this.chkLeftHanded.Size = new System.Drawing.Size(85, 17);
            this.chkLeftHanded.TabIndex = 2;
            this.chkLeftHanded.Text = "Left Handed";
            this.chkLeftHanded.UseVisualStyleBackColor = true;
            this.chkLeftHanded.CheckedChanged += new System.EventHandler(this.chkLeftHanded_CheckedChanged);
            // 
            // StringsConfigurationEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chkLeftHanded);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nbxNumberOfStrings);
            this.Name = "StringsConfigurationEditor";
            this.Size = new System.Drawing.Size(329, 126);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NumericBox nbxNumberOfStrings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkLeftHanded;
    }
}
