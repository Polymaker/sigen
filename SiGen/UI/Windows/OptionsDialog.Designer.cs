namespace SiGen.UI.Windows
{
    partial class OptionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Display_CancelButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.colorSelectButton2 = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.colorSelectButton1 = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.Display_SaveButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Export_CancelButton = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.StringCentersExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.GuideExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.StringsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.CenterLineExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.MarginsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FingerboardExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FretsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.Export_SaveButton = new System.Windows.Forms.Button();
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
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Display_CancelButton);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.Display_SaveButton);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Display_CancelButton
            // 
            resources.ApplyResources(this.Display_CancelButton, "Display_CancelButton");
            this.Display_CancelButton.Name = "Display_CancelButton";
            this.Display_CancelButton.UseVisualStyleBackColor = true;
            this.Display_CancelButton.Click += new System.EventHandler(this.Display_CancelButton_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.checkBox2);
            this.panel2.Controls.Add(this.colorSelectButton2);
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.colorSelectButton1);
            this.panel2.Name = "panel2";
            // 
            // checkBox2
            // 
            resources.ApplyResources(this.checkBox2, "checkBox2");
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // colorSelectButton2
            // 
            resources.ApplyResources(this.colorSelectButton2, "colorSelectButton2");
            this.colorSelectButton2.Name = "colorSelectButton2";
            this.colorSelectButton2.Value = System.Drawing.Color.Red;
            // 
            // checkBox1
            // 
            resources.ApplyResources(this.checkBox1, "checkBox1");
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // colorSelectButton1
            // 
            resources.ApplyResources(this.colorSelectButton1, "colorSelectButton1");
            this.colorSelectButton1.Name = "colorSelectButton1";
            this.colorSelectButton1.Value = System.Drawing.Color.Red;
            // 
            // Display_SaveButton
            // 
            resources.ApplyResources(this.Display_SaveButton, "Display_SaveButton");
            this.Display_SaveButton.Name = "Display_SaveButton";
            this.Display_SaveButton.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.Export_CancelButton);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Controls.Add(this.Export_SaveButton);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // Export_CancelButton
            // 
            resources.ApplyResources(this.Export_CancelButton, "Export_CancelButton");
            this.Export_CancelButton.Name = "Export_CancelButton";
            this.Export_CancelButton.UseVisualStyleBackColor = true;
            this.Export_CancelButton.Click += new System.EventHandler(this.Export_CancelButton_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.StringCentersExportCfg);
            this.panel1.Controls.Add(this.GuideExportCfg);
            this.panel1.Controls.Add(this.StringsExportCfg);
            this.panel1.Controls.Add(this.CenterLineExportCfg);
            this.panel1.Controls.Add(this.MarginsExportCfg);
            this.panel1.Controls.Add(this.FingerboardExportCfg);
            this.panel1.Controls.Add(this.FretsExportCfg);
            this.panel1.Name = "panel1";
            // 
            // StringCentersExportCfg
            // 
            resources.ApplyResources(this.StringCentersExportCfg, "StringCentersExportCfg");
            this.StringCentersExportCfg.LineConfig = null;
            this.StringCentersExportCfg.Name = "StringCentersExportCfg";
            // 
            // GuideExportCfg
            // 
            resources.ApplyResources(this.GuideExportCfg, "GuideExportCfg");
            this.GuideExportCfg.LineConfig = null;
            this.GuideExportCfg.Name = "GuideExportCfg";
            // 
            // StringsExportCfg
            // 
            resources.ApplyResources(this.StringsExportCfg, "StringsExportCfg");
            this.StringsExportCfg.LineConfig = null;
            this.StringsExportCfg.Name = "StringsExportCfg";
            // 
            // CenterLineExportCfg
            // 
            resources.ApplyResources(this.CenterLineExportCfg, "CenterLineExportCfg");
            this.CenterLineExportCfg.LineConfig = null;
            this.CenterLineExportCfg.Name = "CenterLineExportCfg";
            // 
            // MarginsExportCfg
            // 
            resources.ApplyResources(this.MarginsExportCfg, "MarginsExportCfg");
            this.MarginsExportCfg.LineConfig = null;
            this.MarginsExportCfg.Name = "MarginsExportCfg";
            // 
            // FingerboardExportCfg
            // 
            resources.ApplyResources(this.FingerboardExportCfg, "FingerboardExportCfg");
            this.FingerboardExportCfg.LineConfig = null;
            this.FingerboardExportCfg.Name = "FingerboardExportCfg";
            // 
            // FretsExportCfg
            // 
            resources.ApplyResources(this.FretsExportCfg, "FretsExportCfg");
            this.FretsExportCfg.LineConfig = null;
            this.FretsExportCfg.Name = "FretsExportCfg";
            // 
            // Export_SaveButton
            // 
            resources.ApplyResources(this.Export_SaveButton, "Export_SaveButton");
            this.Export_SaveButton.Name = "Export_SaveButton";
            this.Export_SaveButton.UseVisualStyleBackColor = true;
            this.Export_SaveButton.Click += new System.EventHandler(this.SaveExportConfigButton_Click);
            // 
            // AppPreferencesWindow
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "AppPreferencesWindow";
            this.ShowIcon = false;
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
        private System.Windows.Forms.Button Export_SaveButton;
        private System.Windows.Forms.Panel panel1;
        private Controls.Preferences.LineExportConfigEdit CenterLineExportCfg;
        private Controls.Preferences.LineExportConfigEdit MarginsExportCfg;
        private Controls.Preferences.LineExportConfigEdit FingerboardExportCfg;
        private Controls.Preferences.LineExportConfigEdit FretsExportCfg;
        private Controls.Preferences.LineExportConfigEdit StringsExportCfg;
        private Controls.Preferences.LineExportConfigEdit StringCentersExportCfg;
        private Controls.Preferences.LineExportConfigEdit GuideExportCfg;
        private System.Windows.Forms.Button Display_SaveButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox2;
        private Controls.ValueEditors.ColorSelectButton colorSelectButton2;
        private System.Windows.Forms.CheckBox checkBox1;
        private Controls.ValueEditors.ColorSelectButton colorSelectButton1;
        private System.Windows.Forms.Button Display_CancelButton;
        private System.Windows.Forms.Button Export_CancelButton;
    }
}