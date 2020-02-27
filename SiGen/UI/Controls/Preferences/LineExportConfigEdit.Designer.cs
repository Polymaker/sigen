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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LineExportConfigEdit));
            this.EnableExportChecbox = new System.Windows.Forms.CheckBox();
            this.lineExportEdit1 = new SiGen.UI.Controls.Preferences.LineExportEdit();
            this.SuspendLayout();
            // 
            // EnableExportChecbox
            // 
            resources.ApplyResources(this.EnableExportChecbox, "EnableExportChecbox");
            this.EnableExportChecbox.Name = "EnableExportChecbox";
            this.EnableExportChecbox.UseVisualStyleBackColor = true;
            this.EnableExportChecbox.CheckedChanged += new System.EventHandler(this.EnableExportChecbox_CheckedChanged);
            // 
            // lineExportEdit1
            // 
            resources.ApplyResources(this.lineExportEdit1, "lineExportEdit1");
            this.lineExportEdit1.LineConfig = null;
            this.lineExportEdit1.Name = "lineExportEdit1";
            // 
            // LineExportConfigEdit
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.EnableExportChecbox);
            this.Controls.Add(this.lineExportEdit1);
            this.Name = "LineExportConfigEdit";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox EnableExportChecbox;
        private LineExportEdit lineExportEdit1;
    }
}
