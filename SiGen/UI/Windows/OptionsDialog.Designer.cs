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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OptionsDialog));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.ResetDisplayButton = new System.Windows.Forms.Button();
            this.FretWidthLabel = new System.Windows.Forms.Label();
            this.FretsColorLabel = new System.Windows.Forms.Label();
            this.ChkRealisticFrets = new System.Windows.Forms.CheckBox();
            this.StringsColorLabel = new System.Windows.Forms.Label();
            this.ChkRealisticStrings = new System.Windows.Forms.CheckBox();
            this.Display_CancelButton = new System.Windows.Forms.Button();
            this.Display_SaveButton = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.Export_CancelButton = new System.Windows.Forms.Button();
            this.Export_SaveButton = new System.Windows.Forms.Button();
            this.FretWidthBox = new SiGen.UI.Controls.MeasureTextbox();
            this.FretsColorSelect = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.StringsColorSelect = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.StringCentersExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.GuideExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.StringsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.CenterLineExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.MarginsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FingerboardExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FretsExportCfg = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
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
            this.tabPage1.Controls.Add(this.ResetDisplayButton);
            this.tabPage1.Controls.Add(this.FretWidthLabel);
            this.tabPage1.Controls.Add(this.FretWidthBox);
            this.tabPage1.Controls.Add(this.FretsColorLabel);
            this.tabPage1.Controls.Add(this.FretsColorSelect);
            this.tabPage1.Controls.Add(this.ChkRealisticFrets);
            this.tabPage1.Controls.Add(this.StringsColorLabel);
            this.tabPage1.Controls.Add(this.StringsColorSelect);
            this.tabPage1.Controls.Add(this.ChkRealisticStrings);
            this.tabPage1.Controls.Add(this.Display_CancelButton);
            this.tabPage1.Controls.Add(this.Display_SaveButton);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // ResetDisplayButton
            // 
            resources.ApplyResources(this.ResetDisplayButton, "ResetDisplayButton");
            this.ResetDisplayButton.Name = "ResetDisplayButton";
            this.ResetDisplayButton.UseVisualStyleBackColor = true;
            this.ResetDisplayButton.Click += new System.EventHandler(this.ResetDisplayButton_Click);
            // 
            // FretWidthLabel
            // 
            resources.ApplyResources(this.FretWidthLabel, "FretWidthLabel");
            this.FretWidthLabel.Name = "FretWidthLabel";
            // 
            // FretsColorLabel
            // 
            resources.ApplyResources(this.FretsColorLabel, "FretsColorLabel");
            this.FretsColorLabel.Name = "FretsColorLabel";
            // 
            // ChkRealisticFrets
            // 
            resources.ApplyResources(this.ChkRealisticFrets, "ChkRealisticFrets");
            this.ChkRealisticFrets.Name = "ChkRealisticFrets";
            this.ChkRealisticFrets.UseVisualStyleBackColor = true;
            this.ChkRealisticFrets.CheckedChanged += new System.EventHandler(this.ChkRealisticFrets_CheckedChanged);
            // 
            // StringsColorLabel
            // 
            resources.ApplyResources(this.StringsColorLabel, "StringsColorLabel");
            this.StringsColorLabel.Name = "StringsColorLabel";
            // 
            // ChkRealisticStrings
            // 
            resources.ApplyResources(this.ChkRealisticStrings, "ChkRealisticStrings");
            this.ChkRealisticStrings.Name = "ChkRealisticStrings";
            this.ChkRealisticStrings.UseVisualStyleBackColor = true;
            // 
            // Display_CancelButton
            // 
            resources.ApplyResources(this.Display_CancelButton, "Display_CancelButton");
            this.Display_CancelButton.Name = "Display_CancelButton";
            this.Display_CancelButton.UseVisualStyleBackColor = true;
            this.Display_CancelButton.Click += new System.EventHandler(this.Display_CancelButton_Click);
            // 
            // Display_SaveButton
            // 
            resources.ApplyResources(this.Display_SaveButton, "Display_SaveButton");
            this.Display_SaveButton.Name = "Display_SaveButton";
            this.Display_SaveButton.UseVisualStyleBackColor = true;
            this.Display_SaveButton.Click += new System.EventHandler(this.Display_SaveButton_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.flowLayoutPanel1);
            this.tabPage2.Controls.Add(this.Export_CancelButton);
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
            // Export_SaveButton
            // 
            resources.ApplyResources(this.Export_SaveButton, "Export_SaveButton");
            this.Export_SaveButton.Name = "Export_SaveButton";
            this.Export_SaveButton.UseVisualStyleBackColor = true;
            this.Export_SaveButton.Click += new System.EventHandler(this.Export_SaveButton_Click);
            // 
            // FretWidthBox
            // 
            resources.ApplyResources(this.FretWidthBox, "FretWidthBox");
            this.FretWidthBox.Name = "FretWidthBox";
            // 
            // FretsColorSelect
            // 
            resources.ApplyResources(this.FretsColorSelect, "FretsColorSelect");
            this.FretsColorSelect.Name = "FretsColorSelect";
            this.FretsColorSelect.Value = System.Drawing.Color.Red;
            // 
            // StringsColorSelect
            // 
            resources.ApplyResources(this.StringsColorSelect, "StringsColorSelect");
            this.StringsColorSelect.Name = "StringsColorSelect";
            this.StringsColorSelect.Value = System.Drawing.Color.Red;
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
            // flowLayoutPanel1
            // 
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Controls.Add(this.FretsExportCfg);
            this.flowLayoutPanel1.Controls.Add(this.FingerboardExportCfg);
            this.flowLayoutPanel1.Controls.Add(this.MarginsExportCfg);
            this.flowLayoutPanel1.Controls.Add(this.CenterLineExportCfg);
            this.flowLayoutPanel1.Controls.Add(this.StringsExportCfg);
            this.flowLayoutPanel1.Controls.Add(this.StringCentersExportCfg);
            this.flowLayoutPanel1.Controls.Add(this.GuideExportCfg);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // OptionsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "OptionsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button Export_SaveButton;
        private Controls.Preferences.LineExportConfigEdit CenterLineExportCfg;
        private Controls.Preferences.LineExportConfigEdit MarginsExportCfg;
        private Controls.Preferences.LineExportConfigEdit FingerboardExportCfg;
        private Controls.Preferences.LineExportConfigEdit FretsExportCfg;
        private Controls.Preferences.LineExportConfigEdit StringsExportCfg;
        private Controls.Preferences.LineExportConfigEdit StringCentersExportCfg;
        private Controls.Preferences.LineExportConfigEdit GuideExportCfg;
        private System.Windows.Forms.Button Display_SaveButton;
        private System.Windows.Forms.Button Display_CancelButton;
        private System.Windows.Forms.Button Export_CancelButton;
        private System.Windows.Forms.Label FretsColorLabel;
        private Controls.ValueEditors.ColorSelectButton FretsColorSelect;
        private System.Windows.Forms.CheckBox ChkRealisticFrets;
        private System.Windows.Forms.Label StringsColorLabel;
        private Controls.ValueEditors.ColorSelectButton StringsColorSelect;
        private System.Windows.Forms.CheckBox ChkRealisticStrings;
        private System.Windows.Forms.Label FretWidthLabel;
        private Controls.MeasureTextbox FretWidthBox;
        private System.Windows.Forms.Button ResetDisplayButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    }
}