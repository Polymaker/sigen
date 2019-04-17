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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.chkExportFingerboardEdges = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.colorBtnFingerboardEdges = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.lineOptFingerboardEdges = new SiGen.UI.Controls.ValueEditors.ExportLineThicknessEditor();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.colorSelectButton1 = new SiGen.UI.Controls.ValueEditors.ColorSelectButton();
            this.label5 = new System.Windows.Forms.Label();
            this.exportLineThicknessEditor1 = new SiGen.UI.Controls.ValueEditors.ExportLineThicknessEditor();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.73797F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 73.26203F));
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.colorBtnFingerboardEdges, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.lineOptFingerboardEdges, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label4, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.chkExportFingerboardEdges, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.colorSelectButton1, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.label5, 0, 6);
            this.tableLayoutPanel1.Controls.Add(this.exportLineThicknessEditor1, 1, 6);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 8;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(374, 232);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label1, 2);
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Margin = new System.Windows.Forms.Padding(3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Fingerboard";
            // 
            // chkExportFingerboardEdges
            // 
            this.chkExportFingerboardEdges.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.chkExportFingerboardEdges.AutoSize = true;
            this.chkExportFingerboardEdges.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkExportFingerboardEdges.Checked = true;
            this.chkExportFingerboardEdges.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.chkExportFingerboardEdges, 2);
            this.chkExportFingerboardEdges.Location = new System.Drawing.Point(3, 25);
            this.chkExportFingerboardEdges.Name = "chkExportFingerboardEdges";
            this.chkExportFingerboardEdges.Size = new System.Drawing.Size(144, 17);
            this.chkExportFingerboardEdges.TabIndex = 3;
            this.chkExportFingerboardEdges.Text = "Export fingerboard edges";
            this.chkExportFingerboardEdges.UseVisualStyleBackColor = true;
            this.chkExportFingerboardEdges.CheckedChanged += new System.EventHandler(this.ChkExportFingerboardEdges_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Line thickness";
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(44, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Line color";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(226, 238);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // colorBtnFingerboardEdges
            // 
            this.colorBtnFingerboardEdges.Location = new System.Drawing.Point(103, 46);
            this.colorBtnFingerboardEdges.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.colorBtnFingerboardEdges.Name = "colorBtnFingerboardEdges";
            this.colorBtnFingerboardEdges.Size = new System.Drawing.Size(98, 23);
            this.colorBtnFingerboardEdges.TabIndex = 1;
            this.colorBtnFingerboardEdges.Value = System.Drawing.Color.Red;
            // 
            // lineOptFingerboardEdges
            // 
            this.lineOptFingerboardEdges.Location = new System.Drawing.Point(103, 73);
            this.lineOptFingerboardEdges.Name = "lineOptFingerboardEdges";
            this.lineOptFingerboardEdges.SelectedUnit = SiGen.Export.ExportUnit.Pixels;
            this.lineOptFingerboardEdges.Size = new System.Drawing.Size(150, 21);
            this.lineOptFingerboardEdges.TabIndex = 1;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.checkBox1.AutoSize = true;
            this.checkBox1.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBox1, 2);
            this.checkBox1.Location = new System.Drawing.Point(3, 100);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(151, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Export fingerboard margins";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 126);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Line color";
            // 
            // colorSelectButton1
            // 
            this.colorSelectButton1.Location = new System.Drawing.Point(103, 121);
            this.colorSelectButton1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.colorSelectButton1.Name = "colorSelectButton1";
            this.colorSelectButton1.Size = new System.Drawing.Size(98, 23);
            this.colorSelectButton1.TabIndex = 7;
            this.colorSelectButton1.Value = System.Drawing.Color.Red;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 152);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Line thickness";
            // 
            // exportLineThicknessEditor1
            // 
            this.exportLineThicknessEditor1.Location = new System.Drawing.Point(103, 148);
            this.exportLineThicknessEditor1.Name = "exportLineThicknessEditor1";
            this.exportLineThicknessEditor1.SelectedUnit = SiGen.Export.ExportUnit.Pixels;
            this.exportLineThicknessEditor1.Size = new System.Drawing.Size(150, 21);
            this.exportLineThicknessEditor1.TabIndex = 9;
            // 
            // LayoutExportOptionsEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "LayoutExportOptionsEditor";
            this.Size = new System.Drawing.Size(374, 277);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkExportFingerboardEdges;
        private System.Windows.Forms.Label label3;
        private ValueEditors.ExportLineThicknessEditor lineOptFingerboardEdges;
        private ValueEditors.ColorSelectButton colorBtnFingerboardEdges;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private ValueEditors.ColorSelectButton colorSelectButton1;
        private System.Windows.Forms.Label label5;
        private ValueEditors.ExportLineThicknessEditor exportLineThicknessEditor1;
    }
}
