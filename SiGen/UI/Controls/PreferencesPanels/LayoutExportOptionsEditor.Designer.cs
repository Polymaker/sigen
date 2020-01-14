namespace SiGen.UI.Controls.PreferencesPanels
{
    partial class LayoutExportOptionsEditor
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
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.FingerboardEdgesConfig = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FingerboardMarginsConfig = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.ConfigSelectionLayout = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.SelectConfigCombo = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.StringsConfig = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.FretsConfig = new SiGen.UI.Controls.Preferences.LineExportConfigEdit();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1.SuspendLayout();
            this.ConfigSelectionLayout.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 225);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fingerboard";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.FingerboardEdgesConfig);
            this.flowLayoutPanel1.Controls.Add(this.FingerboardMarginsConfig);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 247);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(612, 178);
            this.flowLayoutPanel1.TabIndex = 3;
            // 
            // FingerboardEdgesConfig
            // 
            this.FingerboardEdgesConfig.ConfigName = "Fingerboard edges";
            this.FingerboardEdgesConfig.LineConfig = null;
            this.FingerboardEdgesConfig.Location = new System.Drawing.Point(3, 3);
            this.FingerboardEdgesConfig.Name = "FingerboardEdgesConfig";
            this.FingerboardEdgesConfig.Size = new System.Drawing.Size(367, 83);
            this.FingerboardEdgesConfig.TabIndex = 2;
            // 
            // FingerboardMarginsConfig
            // 
            this.FingerboardMarginsConfig.ConfigName = "Fingerboard margins";
            this.FingerboardMarginsConfig.LineConfig = null;
            this.FingerboardMarginsConfig.Location = new System.Drawing.Point(3, 92);
            this.FingerboardMarginsConfig.Name = "FingerboardMarginsConfig";
            this.FingerboardMarginsConfig.Size = new System.Drawing.Size(367, 83);
            this.FingerboardMarginsConfig.TabIndex = 4;
            // 
            // ConfigSelectionLayout
            // 
            this.ConfigSelectionLayout.AutoSize = true;
            this.ConfigSelectionLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ConfigSelectionLayout.ColumnCount = 3;
            this.ConfigSelectionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ConfigSelectionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.ConfigSelectionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ConfigSelectionLayout.Controls.Add(this.label6, 0, 0);
            this.ConfigSelectionLayout.Controls.Add(this.SelectConfigCombo, 1, 0);
            this.ConfigSelectionLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.ConfigSelectionLayout.Location = new System.Drawing.Point(0, 0);
            this.ConfigSelectionLayout.Name = "ConfigSelectionLayout";
            this.ConfigSelectionLayout.RowCount = 1;
            this.ConfigSelectionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.ConfigSelectionLayout.Size = new System.Drawing.Size(635, 27);
            this.ConfigSelectionLayout.TabIndex = 4;
            // 
            // label6
            // 
            this.label6.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 7);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Configuration:";
            // 
            // SelectConfigCombo
            // 
            this.SelectConfigCombo.FormattingEnabled = true;
            this.SelectConfigCombo.Location = new System.Drawing.Point(103, 3);
            this.SelectConfigCombo.Name = "SelectConfigCombo";
            this.SelectConfigCombo.Size = new System.Drawing.Size(173, 21);
            this.SelectConfigCombo.TabIndex = 1;
            this.SelectConfigCombo.SelectedIndexChanged += new System.EventHandler(this.SelectConfigCombo_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 27);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(635, 434);
            this.panel1.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.StringsConfig, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.FretsConfig, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 4);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 7;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(618, 448);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // StringsConfig
            // 
            this.StringsConfig.ConfigName = "Export strings";
            this.StringsConfig.LineConfig = null;
            this.StringsConfig.Location = new System.Drawing.Point(3, 136);
            this.StringsConfig.Name = "StringsConfig";
            this.StringsConfig.Size = new System.Drawing.Size(367, 83);
            this.StringsConfig.TabIndex = 6;
            // 
            // FretsConfig
            // 
            this.FretsConfig.ConfigName = "Export frets";
            this.FretsConfig.LineConfig = null;
            this.FretsConfig.Location = new System.Drawing.Point(3, 25);
            this.FretsConfig.Name = "FretsConfig";
            this.FretsConfig.Size = new System.Drawing.Size(370, 83);
            this.FretsConfig.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(3, 114);
            this.label3.Margin = new System.Windows.Forms.Padding(3);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 16);
            this.label3.TabIndex = 5;
            this.label3.Text = "Strings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 16);
            this.label2.TabIndex = 5;
            this.label2.Text = "Frets";
            // 
            // LayoutExportOptionsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ConfigSelectionLayout);
            this.Name = "LayoutExportOptionsEditor";
            this.Size = new System.Drawing.Size(635, 461);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ConfigSelectionLayout.ResumeLayout(false);
            this.ConfigSelectionLayout.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private Preferences.LineExportConfigEdit FingerboardEdgesConfig;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Preferences.LineExportConfigEdit FingerboardMarginsConfig;
        private System.Windows.Forms.TableLayoutPanel ConfigSelectionLayout;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox SelectConfigCombo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private Preferences.LineExportConfigEdit StringsConfig;
        private Preferences.LineExportConfigEdit FretsConfig;
    }
}
