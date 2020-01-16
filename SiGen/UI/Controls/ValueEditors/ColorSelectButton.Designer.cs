namespace SiGen.UI.Controls.ValueEditors
{
    partial class ColorSelectButton
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
            this.btnPickColor = new System.Windows.Forms.Button();
            this.HexTexbox = new SiGen.UI.Controls.TextBoxEx();
            this.SuspendLayout();
            // 
            // btnPickColor
            // 
            this.btnPickColor.Location = new System.Drawing.Point(78, -1);
            this.btnPickColor.Name = "btnPickColor";
            this.btnPickColor.Size = new System.Drawing.Size(26, 22);
            this.btnPickColor.TabIndex = 0;
            this.btnPickColor.Text = "...";
            this.btnPickColor.UseVisualStyleBackColor = true;
            this.btnPickColor.Click += new System.EventHandler(this.BtnPickColor_Click);
            // 
            // HexTexbox
            // 
            this.HexTexbox.Location = new System.Drawing.Point(20, 0);
            this.HexTexbox.Name = "HexTexbox";
            this.HexTexbox.Size = new System.Drawing.Size(60, 20);
            this.HexTexbox.TabIndex = 1;
            this.HexTexbox.Text = "FFFFFF";
            this.HexTexbox.ValidateOnEnter = true;
            this.HexTexbox.CommandKeyPressed += new System.Windows.Forms.KeyEventHandler(this.HexTexbox_CommandKeyPressed);
            this.HexTexbox.Validating += new System.ComponentModel.CancelEventHandler(this.HexTexbox_Validating);
            this.HexTexbox.Validated += new System.EventHandler(this.HexTexbox_Validated);
            // 
            // ColorSelectButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.HexTexbox);
            this.Controls.Add(this.btnPickColor);
            this.Name = "ColorSelectButton";
            this.Size = new System.Drawing.Size(109, 20);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnPickColor;
        private TextBoxEx HexTexbox;
    }
}
