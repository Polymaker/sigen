namespace SiGen.UI.Controls.PreferencesPanels
{
    partial class StringsManagementPanel
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Brand = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CoreDiameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OuterDiameter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MOE = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Brand,
            this.Type,
            this.CoreDiameter,
            this.OuterDiameter,
            this.UnitWeight,
            this.MOE});
            this.dataGridView1.Location = new System.Drawing.Point(3, 3);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(842, 239);
            this.dataGridView1.TabIndex = 0;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            // 
            // Brand
            // 
            this.Brand.HeaderText = "Brand";
            this.Brand.Name = "Brand";
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            // 
            // CoreDiameter
            // 
            this.CoreDiameter.HeaderText = "Core Diameter";
            this.CoreDiameter.Name = "CoreDiameter";
            // 
            // OuterDiameter
            // 
            this.OuterDiameter.HeaderText = "Outer Diameter";
            this.OuterDiameter.Name = "OuterDiameter";
            // 
            // UnitWeight
            // 
            this.UnitWeight.HeaderText = "Unit Weight";
            this.UnitWeight.Name = "UnitWeight";
            // 
            // MOE
            // 
            this.MOE.HeaderText = "MoE";
            this.MOE.Name = "MOE";
            // 
            // StringsManagementPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Name = "StringsManagementPanel";
            this.Size = new System.Drawing.Size(853, 427);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Brand;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn CoreDiameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn OuterDiameter;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn MOE;
    }
}
