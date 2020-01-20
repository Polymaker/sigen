namespace SiGen.UI.Windows
{
    partial class AppPreferencesWindow
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.colorSelectButton2 = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.colorSelectButton1 = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.SaveDisplayConfigButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.StringCentersExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.GuideExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.StringsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.CenterLineExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.MarginsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FingerboardExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FretsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.SaveExportConfigButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(621, 403);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.SaveDisplayConfigButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(613, 377);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Viewer Settings";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.AutoScroll = true;
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.colorSelectButton2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.colorSelectButton1);
            this.panel2.Location = new System.Drawing.Point(0, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(610, 339);
            this.panel2.TabIndex = 1;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox2.Location = new System.Drawing.Point(5, 32);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(114, 17);
            this.checkBox2.TabIndex = 4;
            this.checkBox2.Text = "Fingerboard edges";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // colorSelectButton2
            // 
            this.colorSelectButton2.Location = new System.Drawing.Point(128, 30);
            this.colorSelectButton2.Name = "colorSelectButton2";
            this.colorSelectButton2.Size = new System.Drawing.Size(103, 20);
            this.colorSelectButton2.TabIndex = 3;
            this.colorSelectButton2.Value = System.Drawing.Color.Red;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Location = new System.Drawing.Point(5, 6);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(76, 17);
            this.checkBox1.TabIndex = 2;
            this.checkBox1.Text = "Show frets";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // colorSelectButton1
            // 
            this.colorSelectButton1.Location = new System.Drawing.Point(128, 4);
            this.colorSelectButton1.Name = "colorSelectButton1";
            this.colorSelectButton1.Size = new System.Drawing.Size(103, 20);
            this.colorSelectButton1.TabIndex = 1;
            this.colorSelectButton1.Value = System.Drawing.Color.Red;
            // 
            // SaveDisplayConfigButton
            // 
            this.SaveDisplayConfigButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveDisplayConfigButton.Location = new System.Drawing.Point(532, 348);
            this.SaveDisplayConfigButton.Name = "SaveDisplayConfigButton";
            this.SaveDisplayConfigButton.Size = new System.Drawing.Size(75, 23);
            this.SaveDisplayConfigButton.TabIndex = 0;
            this.SaveDisplayConfigButton.Text = "Save";
            this.SaveDisplayConfigButton.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.SaveExportConfigButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(613, 377);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Export Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.StringCentersExportCfg);
            this.panel1.Controls.Add(this.GuideExportCfg);
            this.panel1.Controls.Add(this.StringsExportCfg);
            this.panel1.Controls.Add(this.CenterLineExportCfg);
            this.panel1.Controls.Add(this.MarginsExportCfg);
            this.panel1.Controls.Add(this.FingerboardExportCfg);
            this.panel1.Controls.Add(this.FretsExportCfg);
            this.panel1.Location = new System.Drawing.Point(0, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(610, 339);
            this.panel1.TabIndex = 3;
            // 
            // StringCentersExportCfg
            // 
            this.StringCentersExportCfg.AutoSize = true;
            this.StringCentersExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.StringCentersExportCfg.ConfigName = "Export string centers";
            this.StringCentersExportCfg.LineConfig = null;
            this.StringCentersExportCfg.Location = new System.Drawing.Point(3, 423);
            this.StringCentersExportCfg.Name = "StringCentersExportCfg";
            this.StringCentersExportCfg.Size = new System.Drawing.Size(346, 78);
            this.StringCentersExportCfg.TabIndex = 6;
            // 
            // GuideExportCfg
            // 
            this.GuideExportCfg.AutoSize = true;
            this.GuideExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GuideExportCfg.ConfigName = "Misc. layout line";
            this.GuideExportCfg.LineConfig = null;
            this.GuideExportCfg.Location = new System.Drawing.Point(3, 507);
            this.GuideExportCfg.Name = "GuideExportCfg";
            this.GuideExportCfg.Size = new System.Drawing.Size(346, 78);
            this.GuideExportCfg.TabIndex = 5;
            // 
            // StringsExportCfg
            // 
            this.StringsExportCfg.AutoSize = true;
            this.StringsExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.StringsExportCfg.ConfigName = "Export strings";
            this.StringsExportCfg.LineConfig = null;
            this.StringsExportCfg.Location = new System.Drawing.Point(3, 339);
            this.StringsExportCfg.Name = "StringsExportCfg";
            this.StringsExportCfg.Size = new System.Drawing.Size(346, 78);
            this.StringsExportCfg.TabIndex = 4;
            // 
            // CenterLineExportCfg
            // 
            this.CenterLineExportCfg.AutoSize = true;
            this.CenterLineExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.CenterLineExportCfg.ConfigName = "Export center line";
            this.CenterLineExportCfg.LineConfig = null;
            this.CenterLineExportCfg.Location = new System.Drawing.Point(3, 255);
            this.CenterLineExportCfg.Name = "CenterLineExportCfg";
            this.CenterLineExportCfg.Size = new System.Drawing.Size(346, 78);
            this.CenterLineExportCfg.TabIndex = 3;
            // 
            // MarginsExportCfg
            // 
            this.MarginsExportCfg.AutoSize = true;
            this.MarginsExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.MarginsExportCfg.ConfigName = "Export margins";
            this.MarginsExportCfg.LineConfig = null;
            this.MarginsExportCfg.Location = new System.Drawing.Point(3, 171);
            this.MarginsExportCfg.Name = "MarginsExportCfg";
            this.MarginsExportCfg.Size = new System.Drawing.Size(346, 78);
            this.MarginsExportCfg.TabIndex = 2;
            // 
            // FingerboardExportCfg
            // 
            this.FingerboardExportCfg.AutoSize = true;
            this.FingerboardExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FingerboardExportCfg.ConfigName = "Export fingerboard";
            this.FingerboardExportCfg.LineConfig = null;
            this.FingerboardExportCfg.Location = new System.Drawing.Point(3, 87);
            this.FingerboardExportCfg.Name = "FingerboardExportCfg";
            this.FingerboardExportCfg.Size = new System.Drawing.Size(346, 78);
            this.FingerboardExportCfg.TabIndex = 1;
            // 
            // FretsExportCfg
            // 
            this.FretsExportCfg.AutoSize = true;
            this.FretsExportCfg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.FretsExportCfg.ConfigName = "Export frets";
            this.FretsExportCfg.LineConfig = null;
            this.FretsExportCfg.Location = new System.Drawing.Point(3, 3);
            this.FretsExportCfg.Name = "FretsExportCfg";
            this.FretsExportCfg.Size = new System.Drawing.Size(346, 78);
            this.FretsExportCfg.TabIndex = 0;
            // 
            // SaveExportConfigButton
            // 
            this.SaveExportConfigButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveExportConfigButton.Location = new System.Drawing.Point(532, 348);
            this.SaveExportConfigButton.Name = "SaveExportConfigButton";
            this.SaveExportConfigButton.Size = new System.Drawing.Size(75, 23);
            this.SaveExportConfigButton.TabIndex = 2;
            this.SaveExportConfigButton.Text = "Save";
            this.SaveExportConfigButton.UseVisualStyleBackColor = true;
            this.SaveExportConfigButton.Click += new System.EventHandler(this.SaveExportConfigButton_Click);
            // 
            // AppPreferencesWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(621, 403);
            this.Controls.Add(this.tabControl1);
            this.Name = "AppPreferencesWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AppPreferencesWindow";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button SaveExportConfigButton;
        private System.Windows.Forms.Panel panel1;
        private Controls.Preferences.LineExportConfigEdit CenterLineExportCfg;
        private Controls.Preferences.LineExportConfigEdit MarginsExportCfg;
        private Controls.Preferences.LineExportConfigEdit FingerboardExportCfg;
        private Controls.Preferences.LineExportConfigEdit FretsExportCfg;
        private Controls.Preferences.LineExportConfigEdit StringsExportCfg;
        private Controls.Preferences.LineExportConfigEdit StringCentersExportCfg;
        private Controls.Preferences.LineExportConfigEdit GuideExportCfg;
        private System.Windows.Forms.Button SaveDisplayConfigButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox2;
        private Controls.ValueEditors.ColorSelectButton colorSelectButton2;
        private System.Windows.Forms.CheckBox checkBox1;
        private Controls.ValueEditors.ColorSelectButton colorSelectButton1;
    }
}