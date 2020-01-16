namespace SiGen.UI.Controls.Preferences
{
    partial class LineExportConfigEdit
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
            this.EnableExportChecbox = new System.Windows.Forms.CheckBox();
            this.LineColorLabel = new System.Windows.Forms.Label();
            this.LineThicknessLabel = new System.Windows.Forms.Label();
            this.LineColorSelector = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.LineThicknessEditor = new SiGen.UI.Controls.ValueEditors.LineThicknessEditor();
            this.UseStringGaugeCheckbox = new System.Windows.Forms.CheckBox();
            this.DashedCheckbox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // EnableExportChecbox
            // 
            this.EnableExportChecbox.AutoSize = true;
            this.EnableExportChecbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.EnableExportChecbox.Location = new System.Drawing.Point(5, 5);
            this.EnableExportChecbox.MinimumSize = new System.Drawing.Size(116, 0);
            this.EnableExportChecbox.Name = "EnableExportChecbox";
            this.EnableExportChecbox.Size = new System.Drawing.Size(116, 17);
            this.EnableExportChecbox.TabIndex = 7;
            this.EnableExportChecbox.Text = "[Config Name]";
            this.EnableExportChecbox.UseVisualStyleBackColor = true;
            // 
            // LineColorLabel
            // 
            this.LineColorLabel.AutoSize = true;
            this.LineColorLabel.Location = new System.Drawing.Point(45, 31);
            this.LineColorLabel.Name = "LineColorLabel";
            this.LineColorLabel.Size = new System.Drawing.Size(53, 13);
            this.LineColorLabel.TabIndex = 8;
            this.LineColorLabel.Text = "Line color";
            // 
            // LineThicknessLabel
            // 
            this.LineThicknessLabel.AutoSize = true;
            this.LineThicknessLabel.Location = new System.Drawing.Point(23, 58);
            this.LineThicknessLabel.Name = "LineThicknessLabel";
            this.LineThicknessLabel.Size = new System.Drawing.Size(75, 13);
            this.LineThicknessLabel.TabIndex = 9;
            this.LineThicknessLabel.Text = "Line thickness";
            // 
            // LineColorSelector
            // 
            this.LineColorSelector.Location = new System.Drawing.Point(104, 28);
            this.LineColorSelector.Name = "LineColorSelector";
            this.LineColorSelector.Size = new System.Drawing.Size(103, 20);
            this.LineColorSelector.TabIndex = 10;
            this.LineColorSelector.Value = System.Drawing.Color.Red;
            // 
            // LineThicknessEditor
            // 
            this.LineThicknessEditor.Location = new System.Drawing.Point(104, 54);
            this.LineThicknessEditor.Name = "LineThicknessEditor";
            this.LineThicknessEditor.SelectedThickness = 1D;
            this.LineThicknessEditor.SelectedUnit = SiGen.Export.LineUnit.Pixels;
            this.LineThicknessEditor.Size = new System.Drawing.Size(140, 21);
            this.LineThicknessEditor.TabIndex = 11;
            // 
            // UseStringGaugeCheckbox
            // 
            this.UseStringGaugeCheckbox.AutoSize = true;
            this.UseStringGaugeCheckbox.Location = new System.Drawing.Point(250, 56);
            this.UseStringGaugeCheckbox.Name = "UseStringGaugeCheckbox";
            this.UseStringGaugeCheckbox.Size = new System.Drawing.Size(110, 17);
            this.UseStringGaugeCheckbox.TabIndex = 12;
            this.UseStringGaugeCheckbox.Text = "Use String Gauge";
            this.UseStringGaugeCheckbox.UseVisualStyleBackColor = true;
            // 
            // DashedCheckbox
            // 
            this.DashedCheckbox.AutoSize = true;
            this.DashedCheckbox.Location = new System.Drawing.Point(250, 29);
            this.DashedCheckbox.Name = "DashedCheckbox";
            this.DashedCheckbox.Size = new System.Drawing.Size(63, 17);
            this.DashedCheckbox.TabIndex = 13;
            this.DashedCheckbox.Text = "Dashed";
            this.DashedCheckbox.UseVisualStyleBackColor = true;
            // 
            // LineExportConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.EnableExportChecbox);
            this.Controls.Add(this.LineColorLabel);
            this.Controls.Add(this.LineThicknessLabel);
            this.Controls.Add(this.LineColorSelector);
            this.Controls.Add(this.LineThicknessEditor);
            this.Controls.Add(this.UseStringGaugeCheckbox);
            this.Controls.Add(this.DashedCheckbox);
            this.Name = "LineExportConfigEdit";
            this.Size = new System.Drawing.Size(363, 78);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox EnableExportChecbox;
        private System.Windows.Forms.Label LineColorLabel;
        private System.Windows.Forms.Label LineThicknessLabel;
        private ValueEditors.ColorSelectButton LineColorSelector;
        private ValueEditors.LineThicknessEditor LineThicknessEditor;
        private System.Windows.Forms.CheckBox UseStringGaugeCheckbox;
        private System.Windows.Forms.CheckBox DashedCheckbox;
    }
}
