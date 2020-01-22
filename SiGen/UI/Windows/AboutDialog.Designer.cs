namespace SiGen.UI.Windows
{
    partial class AboutDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutDialog));
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.AppTitleLabel = new System.Windows.Forms.Label();
            this.AppDescLabel = new System.Windows.Forms.Label();
            this.AppInfoTextbox = new System.Windows.Forms.TextBox();
            this.AppVersionLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::SiGen.Properties.Resources.SiGenIcon_x32;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // AppTitleLabel
            // 
            resources.ApplyResources(this.AppTitleLabel, "AppTitleLabel");
            this.AppTitleLabel.Name = "AppTitleLabel";
            // 
            // AppDescLabel
            // 
            resources.ApplyResources(this.AppDescLabel, "AppDescLabel");
            this.AppDescLabel.Name = "AppDescLabel";
            // 
            // AppInfoTextbox
            // 
            resources.ApplyResources(this.AppInfoTextbox, "AppInfoTextbox");
            this.AppInfoTextbox.Name = "AppInfoTextbox";
            this.AppInfoTextbox.ReadOnly = true;
            // 
            // AppVersionLabel
            // 
            resources.ApplyResources(this.AppVersionLabel, "AppVersionLabel");
            this.AppVersionLabel.Name = "AppVersionLabel";
            // 
            // AboutDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.Controls.Add(this.AppVersionLabel);
            this.Controls.Add(this.AppInfoTextbox);
            this.Controls.Add(this.AppDescLabel);
            this.Controls.Add(this.AppTitleLabel);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutDialog";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label AppTitleLabel;
        private System.Windows.Forms.Label AppDescLabel;
        private System.Windows.Forms.TextBox AppInfoTextbox;
        private System.Windows.Forms.Label AppVersionLabel;
    }
}