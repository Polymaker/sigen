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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.EnableExportChecbox = new System.Windows.Forms.CheckBox();
            this.LineColorLabel = new System.Windows.Forms.Label();
            this.LineThicknessLabel = new System.Windows.Forms.Label();
            this.UseStringGaugeCheckbox = new System.Windows.Forms.CheckBox();
            this.DashedCheckbox = new System.Windows.Forms.CheckBox();
            this.LineColorSelector = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.LineThicknessEditor = new SiGen.UI.Controls.ValueEditors.LineThicknessEditor();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 99F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.EnableExportChecbox, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.LineColorLabel, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.LineThicknessLabel, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.LineColorSelector, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.LineThicknessEditor, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.UseStringGaugeCheckbox, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.DashedCheckbox, 2, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(367, 79);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.SizeChanged += new System.EventHandler(this.tableLayoutPanel1_SizeChanged);
            // 
            // EnableExportChecbox
            // 
            this.EnableExportChecbox.AutoSize = true;
            this.EnableExportChecbox.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.tableLayoutPanel1.SetColumnSpan(this.EnableExportChecbox, 2);
            this.EnableExportChecbox.Location = new System.Drawing.Point(3, 3);
            this.EnableExportChecbox.MinimumSize = new System.Drawing.Size(116, 0);
            this.EnableExportChecbox.Name = "EnableExportChecbox";
            this.EnableExportChecbox.Size = new System.Drawing.Size(116, 17);
            this.EnableExportChecbox.TabIndex = 0;
            this.EnableExportChecbox.Text = "[Config Name]";
            this.EnableExportChecbox.UseVisualStyleBackColor = true;
            this.EnableExportChecbox.CheckedChanged += new System.EventHandler(this.EnableExportChecbox_CheckedChanged);
            // 
            // LineColorLabel
            // 
            this.LineColorLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LineColorLabel.AutoSize = true;
            this.LineColorLabel.Location = new System.Drawing.Point(43, 31);
            this.LineColorLabel.Name = "LineColorLabel";
            this.LineColorLabel.Size = new System.Drawing.Size(53, 13);
            this.LineColorLabel.TabIndex = 1;
            this.LineColorLabel.Text = "Line color";
            // 
            // LineThicknessLabel
            // 
            this.LineThicknessLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.LineThicknessLabel.AutoSize = true;
            this.LineThicknessLabel.Location = new System.Drawing.Point(21, 59);
            this.LineThicknessLabel.Name = "LineThicknessLabel";
            this.LineThicknessLabel.Size = new System.Drawing.Size(75, 13);
            this.LineThicknessLabel.TabIndex = 2;
            this.LineThicknessLabel.Text = "Line thickness";
            // 
            // UseStringGaugeCheckbox
            // 
            this.UseStringGaugeCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.UseStringGaugeCheckbox.AutoSize = true;
            this.UseStringGaugeCheckbox.Location = new System.Drawing.Point(248, 57);
            this.UseStringGaugeCheckbox.Name = "UseStringGaugeCheckbox";
            this.UseStringGaugeCheckbox.Size = new System.Drawing.Size(110, 17);
            this.UseStringGaugeCheckbox.TabIndex = 5;
            this.UseStringGaugeCheckbox.Text = "Use String Gauge";
            this.UseStringGaugeCheckbox.UseVisualStyleBackColor = true;
            // 
            // DashedCheckbox
            // 
            this.DashedCheckbox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DashedCheckbox.AutoSize = true;
            this.DashedCheckbox.Location = new System.Drawing.Point(248, 29);
            this.DashedCheckbox.Name = "DashedCheckbox";
            this.DashedCheckbox.Size = new System.Drawing.Size(63, 17);
            this.DashedCheckbox.TabIndex = 6;
            this.DashedCheckbox.Text = "Dashed";
            this.DashedCheckbox.UseVisualStyleBackColor = true;
            this.DashedCheckbox.CheckedChanged += new System.EventHandler(this.DashedCheckbox_CheckedChanged);
            // 
            // LineColorSelector
            // 
            this.LineColorSelector.Location = new System.Drawing.Point(102, 26);
            this.LineColorSelector.Name = "LineColorSelector";
            this.LineColorSelector.Size = new System.Drawing.Size(98, 23);
            this.LineColorSelector.TabIndex = 3;
            this.LineColorSelector.Value = System.Drawing.Color.Red;
            this.LineColorSelector.ValueChanged += new System.EventHandler(this.LineColorSelector_ValueChanged);
            // 
            // LineThicknessEditor
            // 
            this.LineThicknessEditor.Location = new System.Drawing.Point(102, 55);
            this.LineThicknessEditor.Name = "LineThicknessEditor";
            this.LineThicknessEditor.SelectedThickness = 1D;
            this.LineThicknessEditor.SelectedUnit = SiGen.Export.LineUnit.Pixels;
            this.LineThicknessEditor.Size = new System.Drawing.Size(140, 21);
            this.LineThicknessEditor.TabIndex = 4;
            this.LineThicknessEditor.ConfigurationChanged += new System.EventHandler(this.LineThicknessEditor_ConfigurationChanged);
            // 
            // LineExportConfigEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LineExportConfigEdit";
            this.Size = new System.Drawing.Size(367, 106);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.CheckBox EnableExportChecbox;
        private System.Windows.Forms.Label LineColorLabel;
        private System.Windows.Forms.Label LineThicknessLabel;
        private ValueEditors.ColorSelectButton LineColorSelector;
        private ValueEditors.LineThicknessEditor LineThicknessEditor;
        private System.Windows.Forms.CheckBox UseStringGaugeCheckbox;
        private System.Windows.Forms.CheckBox DashedCheckbox;
    }
}
