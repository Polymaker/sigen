namespace SiGen.UI.Controls.ValueEditors
{
    partial class LineThicknessEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineThicknessEditor));
            this.cboUnitType = new System.Windows.Forms.ComboBox();
            this.txtMeasure = new SiGen.UI.Controls.MeasureTextbox();
            this.txtNumber = new SiGen.UI.Controls.NumericBox();
            this.localizableStrings = new SiGen.Localization.LocalizableStringList(this.components);
            this.ExportUnitPixels = new SiGen.Localization.LocalizableString(this.components);
            this.ExportUnitPoints = new SiGen.Localization.LocalizableString(this.components);
            this.ExportUnitMeasure = new SiGen.Localization.LocalizableString(this.components);
            this.SuspendLayout();
            // 
            // cboUnitType
            // 
            this.cboUnitType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboUnitType.FormattingEnabled = true;
            resources.ApplyResources(this.cboUnitType, "cboUnitType");
            this.cboUnitType.Name = "cboUnitType";
            this.cboUnitType.SelectedIndexChanged += new System.EventHandler(this.CboUnitType_SelectedIndexChanged);
            // 
            // txtMeasure
            // 
            resources.ApplyResources(this.txtMeasure, "txtMeasure");
            this.txtMeasure.Name = "txtMeasure";
            this.txtMeasure.ValueChanged += new System.EventHandler(this.TxtMeasure_ValueChanged);
            // 
            // txtNumber
            // 
            resources.ApplyResources(this.txtNumber, "txtNumber");
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.ValueChanged += new System.EventHandler(this.TxtNumber_ValueChanged);
            // 
            // localizableStrings
            // 
            this.localizableStrings.Items.Add(this.ExportUnitPixels);
            this.localizableStrings.Items.Add(this.ExportUnitPoints);
            this.localizableStrings.Items.Add(this.ExportUnitMeasure);
            // 
            // ExportUnitPixels
            // 
            resources.ApplyResources(this.ExportUnitPixels, "ExportUnitPixels");
            // 
            // ExportUnitPoints
            // 
            resources.ApplyResources(this.ExportUnitPoints, "ExportUnitPoints");
            // 
            // ExportUnitMeasure
            // 
            resources.ApplyResources(this.ExportUnitMeasure, "ExportUnitMeasure");
            // 
            // LineThicknessEditor
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtMeasure);
            this.Controls.Add(this.txtNumber);
            this.Controls.Add(this.cboUnitType);
            this.Name = "LineThicknessEditor";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cboUnitType;
        private NumericBox txtNumber;
        private MeasureTextbox txtMeasure;
        private Localization.LocalizableStringList localizableStrings;
        private Localization.LocalizableString ExportUnitPixels;
        private Localization.LocalizableString ExportUnitPoints;
        private Localization.LocalizableString ExportUnitMeasure;
    }
}
