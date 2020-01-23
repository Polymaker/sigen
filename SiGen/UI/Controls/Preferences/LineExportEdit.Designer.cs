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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ThicknessEditor = new SiGen.UI.Controls.ValueEditors.LineThicknessEditor();
            this.ColorSelector = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Line Color";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Thickness";
            // 
            // ThicknessEditor
            // 
            this.ThicknessEditor.Location = new System.Drawing.Point(68, 29);
            this.ThicknessEditor.Name = "ThicknessEditor";
            this.ThicknessEditor.SelectedThickness = 1D;
            this.ThicknessEditor.Size = new System.Drawing.Size(115, 21);
            this.ThicknessEditor.TabIndex = 1;
            this.ThicknessEditor.ThicknessChanged += new System.EventHandler(this.ThicknessEditor_ThicknessChanged);
            // 
            // ColorSelector
            // 
            this.ColorSelector.Location = new System.Drawing.Point(68, 3);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(115, 20);
            this.ColorSelector.TabIndex = 0;
            this.ColorSelector.Value = System.Drawing.Color.Red;
            this.ColorSelector.ValueChanged += new System.EventHandler(this.ColorSelector_ValueChanged);
            // 
            // LineExportEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ThicknessEditor);
            this.Controls.Add(this.ColorSelector);
            this.Name = "LineExportEdit";
            this.Size = new System.Drawing.Size(186, 53);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ValueEditors.ColorSelectButton ColorSelector;
        private ValueEditors.LineThicknessEditor ThicknessEditor;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
