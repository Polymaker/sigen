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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineExportEdit));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.DashedCheckbox = new System.Windows.Forms.CheckBox();
            this.StringGaugeCheckbox = new System.Windows.Forms.CheckBox();
            this.ThicknessEditor = new SiGen.UI.Controls.ValueEditors.LineThicknessEditor();
            this.ColorSelector = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // DashedCheckbox
            // 
            resources.ApplyResources(this.DashedCheckbox, "DashedCheckbox");
            this.DashedCheckbox.Name = "DashedCheckbox";
            this.DashedCheckbox.UseVisualStyleBackColor = true;
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
            // LineExportEdit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StringGaugeCheckbox);
            this.Controls.Add(this.DashedCheckbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ThicknessEditor);
            this.Controls.Add(this.ColorSelector);
            this.Name = "LineExportEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ValueEditors.ColorSelectButton ColorSelector;
        private ValueEditors.LineThicknessEditor ThicknessEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox DashedCheckbox;
        private System.Windows.Forms.CheckBox StringGaugeCheckbox;
    }
}
