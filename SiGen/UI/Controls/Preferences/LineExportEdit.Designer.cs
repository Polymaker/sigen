namespace SiGen.UI.Controls.Preferences
{
    partial class LineExportEdit
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineExportEdit));
            this.ColorLabel = new System.Windows.Forms.Label();
            this.ThicknessLabel = new System.Windows.Forms.Label();
            this.DashedCheckbox = new System.Windows.Forms.CheckBox();
            this.StringGaugeCheckbox = new System.Windows.Forms.CheckBox();
            this.ThicknessEditor = new SiGen.UI.Controls.ValueEditors.LineThicknessEditor();
            this.ColorSelector = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.ExtraOption1Checkbox = new System.Windows.Forms.CheckBox();
            this.localizableStringList1 = new SiGen.Localization.LocalizableStringList(this.components);
            this.ContinueEdgeLines = new SiGen.Localization.LocalizableString(this.components);
            this.IncludeBridgeLine = new SiGen.Localization.LocalizableString(this.components);
            this.SuspendLayout();
            // 
            // ColorLabel
            // 
            resources.ApplyResources(this.ColorLabel, "ColorLabel");
            this.ColorLabel.Name = "ColorLabel";
            // 
            // ThicknessLabel
            // 
            resources.ApplyResources(this.ThicknessLabel, "ThicknessLabel");
            this.ThicknessLabel.Name = "ThicknessLabel";
            // 
            // DashedCheckbox
            // 
            resources.ApplyResources(this.DashedCheckbox, "DashedCheckbox");
            this.DashedCheckbox.Name = "DashedCheckbox";
            this.DashedCheckbox.UseVisualStyleBackColor = true;
            this.DashedCheckbox.CheckedChanged += new System.EventHandler(this.DashedCheckbox_CheckedChanged);
            // 
            // StringGaugeCheckbox
            // 
            resources.ApplyResources(this.StringGaugeCheckbox, "StringGaugeCheckbox");
            this.StringGaugeCheckbox.Name = "StringGaugeCheckbox";
            this.StringGaugeCheckbox.UseVisualStyleBackColor = true;
            this.StringGaugeCheckbox.CheckedChanged += new System.EventHandler(this.StringGaugeCheckbox_CheckedChanged);
            // 
            // ThicknessEditor
            // 
            resources.ApplyResources(this.ThicknessEditor, "ThicknessEditor");
            this.ThicknessEditor.Name = "ThicknessEditor";
            this.ThicknessEditor.SelectedThickness = 1D;
            this.ThicknessEditor.ThicknessChanged += new System.EventHandler(this.ThicknessEditor_ThicknessChanged);
            // 
            // ColorSelector
            // 
            resources.ApplyResources(this.ColorSelector, "ColorSelector");
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Value = System.Drawing.Color.Red;
            this.ColorSelector.ValueChanged += new System.EventHandler(this.ColorSelector_ValueChanged);
            // 
            // ExtraOption1Checkbox
            // 
            resources.ApplyResources(this.ExtraOption1Checkbox, "ExtraOption1Checkbox");
            this.ExtraOption1Checkbox.Name = "ExtraOption1Checkbox";
            this.ExtraOption1Checkbox.UseVisualStyleBackColor = true;
            this.ExtraOption1Checkbox.CheckedChanged += new System.EventHandler(this.ExtraOption1Checkbox_CheckedChanged);
            // 
            // localizableStringList1
            // 
            this.localizableStringList1.Items.Add(this.ContinueEdgeLines);
            this.localizableStringList1.Items.Add(this.IncludeBridgeLine);
            // 
            // ContinueEdgeLines
            // 
            resources.ApplyResources(this.ContinueEdgeLines, "ContinueEdgeLines");
            // 
            // IncludeBridgeLine
            // 
            resources.ApplyResources(this.IncludeBridgeLine, "IncludeBridgeLine");
            // 
            // LineExportEdit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ExtraOption1Checkbox);
            this.Controls.Add(this.StringGaugeCheckbox);
            this.Controls.Add(this.DashedCheckbox);
            this.Controls.Add(this.ThicknessLabel);
            this.Controls.Add(this.ColorLabel);
            this.Controls.Add(this.ThicknessEditor);
            this.Controls.Add(this.ColorSelector);
            this.Name = "LineExportEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ValueEditors.ColorSelectButton ColorSelector;
        private ValueEditors.LineThicknessEditor ThicknessEditor;
        private System.Windows.Forms.Label ColorLabel;
        private System.Windows.Forms.Label ThicknessLabel;
        private System.Windows.Forms.CheckBox DashedCheckbox;
        private System.Windows.Forms.CheckBox StringGaugeCheckbox;
        private System.Windows.Forms.CheckBox ExtraOption1Checkbox;
        private Localization.LocalizableStringList localizableStringList1;
        private Localization.LocalizableString ContinueEdgeLines;
        private Localization.LocalizableString IncludeBridgeLine;
    }
}
