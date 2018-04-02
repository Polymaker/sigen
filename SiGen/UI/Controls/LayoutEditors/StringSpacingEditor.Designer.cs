namespace SiGen.UI.Controls.LayoutEditors
{
    partial class StringSpacingEditor
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mtbNutSpacing = new SiGen.UI.MeasureTextbox();
            this.mtbNutSpread = new SiGen.UI.MeasureTextbox();
            this.mtbBridgeSpacing = new SiGen.UI.MeasureTextbox();
            this.mtbBridgeSpread = new SiGen.UI.MeasureTextbox();
            this.lblNutStringSpacing = new System.Windows.Forms.Label();
            this.cboNutSpacingMethod = new System.Windows.Forms.ComboBox();
            this.lblNutSpacingMethod = new System.Windows.Forms.Label();
            this.lblNutTotalSpread = new System.Windows.Forms.Label();
            this.lblBridgeTotalSpread = new System.Windows.Forms.Label();
            this.lblBridgeStringSpacing = new System.Windows.Forms.Label();
            this.tlpNutSpacingAuto = new System.Windows.Forms.TableLayoutPanel();
            this.lblNutSpacing = new System.Windows.Forms.Label();
            this.cboNutSpacingAlignment = new System.Windows.Forms.ComboBox();
            this.lblNutSpacingAlignment = new System.Windows.Forms.Label();
            this.tlpBridgeSpacingAuto = new System.Windows.Forms.TableLayoutPanel();
            this.cboBridgeSpacingAlignment = new System.Windows.Forms.ComboBox();
            this.cboBridgeSpacingMethod = new System.Windows.Forms.ComboBox();
            this.lblBridgeSpacingMethod = new System.Windows.Forms.Label();
            this.lblBridgeSpacing = new System.Windows.Forms.Label();
            this.lblBridgeSpacingAlignment = new System.Windows.Forms.Label();
            this.tlpNutSpacingAuto.SuspendLayout();
            this.tlpBridgeSpacingAuto.SuspendLayout();
            this.SuspendLayout();
            // 
            // mtbNutSpacing
            // 
            this.mtbNutSpacing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbNutSpacing.Location = new System.Drawing.Point(93, 16);
            this.mtbNutSpacing.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbNutSpacing.Name = "mtbNutSpacing";
            this.mtbNutSpacing.Size = new System.Drawing.Size(122, 20);
            this.mtbNutSpacing.TabIndex = 0;
            this.mtbNutSpacing.ValueChanging += new System.EventHandler<SiGen.UI.MeasureTextbox.ValueChangingEventArgs>(this.mtbSpacings_ValueChanging);
            this.mtbNutSpacing.ValueChanged += new System.EventHandler(this.mtbNutSpacing_ValueChanged);
            // 
            // mtbNutSpread
            // 
            this.mtbNutSpread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbNutSpread.Location = new System.Drawing.Point(221, 16);
            this.mtbNutSpread.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbNutSpread.Name = "mtbNutSpread";
            this.mtbNutSpread.Size = new System.Drawing.Size(122, 20);
            this.mtbNutSpread.TabIndex = 1;
            this.mtbNutSpread.ValueChanging += new System.EventHandler<SiGen.UI.MeasureTextbox.ValueChangingEventArgs>(this.mtbSpacings_ValueChanging);
            this.mtbNutSpread.ValueChanged += new System.EventHandler(this.mtbNutSpread_ValueChanged);
            // 
            // mtbBridgeSpacing
            // 
            this.mtbBridgeSpacing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbBridgeSpacing.Location = new System.Drawing.Point(93, 16);
            this.mtbBridgeSpacing.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBridgeSpacing.Name = "mtbBridgeSpacing";
            this.mtbBridgeSpacing.Size = new System.Drawing.Size(122, 20);
            this.mtbBridgeSpacing.TabIndex = 2;
            this.mtbBridgeSpacing.ValueChanging += new System.EventHandler<SiGen.UI.MeasureTextbox.ValueChangingEventArgs>(this.mtbSpacings_ValueChanging);
            this.mtbBridgeSpacing.ValueChanged += new System.EventHandler(this.mtbBridgeSpacing_ValueChanged);
            // 
            // mtbBridgeSpread
            // 
            this.mtbBridgeSpread.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.mtbBridgeSpread.Location = new System.Drawing.Point(221, 16);
            this.mtbBridgeSpread.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.mtbBridgeSpread.Name = "mtbBridgeSpread";
            this.mtbBridgeSpread.Size = new System.Drawing.Size(122, 20);
            this.mtbBridgeSpread.TabIndex = 3;
            this.mtbBridgeSpread.ValueChanging += new System.EventHandler<SiGen.UI.MeasureTextbox.ValueChangingEventArgs>(this.mtbSpacings_ValueChanging);
            this.mtbBridgeSpread.ValueChanged += new System.EventHandler(this.mtbBridgeSpread_ValueChanged);
            // 
            // lblNutStringSpacing
            // 
            this.lblNutStringSpacing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNutStringSpacing.AutoSize = true;
            this.lblNutStringSpacing.Location = new System.Drawing.Point(131, 0);
            this.lblNutStringSpacing.Name = "lblNutStringSpacing";
            this.lblNutStringSpacing.Size = new System.Drawing.Size(46, 13);
            this.lblNutStringSpacing.TabIndex = 6;
            this.lblNutStringSpacing.Text = "Spacing";
            // 
            // cboNutSpacingMethod
            // 
            this.cboNutSpacingMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpNutSpacingAuto.SetColumnSpan(this.cboNutSpacingMethod, 2);
            this.cboNutSpacingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNutSpacingMethod.FormattingEnabled = true;
            this.cboNutSpacingMethod.Location = new System.Drawing.Point(93, 40);
            this.cboNutSpacingMethod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.cboNutSpacingMethod.Name = "cboNutSpacingMethod";
            this.cboNutSpacingMethod.Size = new System.Drawing.Size(250, 21);
            this.cboNutSpacingMethod.TabIndex = 10;
            this.cboNutSpacingMethod.SelectedIndexChanged += new System.EventHandler(this.cboNutSpacingMethod_SelectedIndexChanged);
            // 
            // lblNutSpacingMethod
            // 
            this.lblNutSpacingMethod.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNutSpacingMethod.AutoSize = true;
            this.lblNutSpacingMethod.Location = new System.Drawing.Point(44, 43);
            this.lblNutSpacingMethod.Name = "lblNutSpacingMethod";
            this.lblNutSpacingMethod.Size = new System.Drawing.Size(43, 13);
            this.lblNutSpacingMethod.TabIndex = 9;
            this.lblNutSpacingMethod.Text = "Method";
            // 
            // lblNutTotalSpread
            // 
            this.lblNutTotalSpread.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblNutTotalSpread.AutoSize = true;
            this.lblNutTotalSpread.Location = new System.Drawing.Point(248, 0);
            this.lblNutTotalSpread.Name = "lblNutTotalSpread";
            this.lblNutTotalSpread.Size = new System.Drawing.Size(68, 13);
            this.lblNutTotalSpread.TabIndex = 7;
            this.lblNutTotalSpread.Text = "Total Spread";
            // 
            // lblBridgeTotalSpread
            // 
            this.lblBridgeTotalSpread.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBridgeTotalSpread.AutoSize = true;
            this.lblBridgeTotalSpread.Location = new System.Drawing.Point(248, 0);
            this.lblBridgeTotalSpread.Name = "lblBridgeTotalSpread";
            this.lblBridgeTotalSpread.Size = new System.Drawing.Size(68, 13);
            this.lblBridgeTotalSpread.TabIndex = 7;
            this.lblBridgeTotalSpread.Text = "Total Spread";
            // 
            // lblBridgeStringSpacing
            // 
            this.lblBridgeStringSpacing.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblBridgeStringSpacing.AutoSize = true;
            this.lblBridgeStringSpacing.Location = new System.Drawing.Point(131, 0);
            this.lblBridgeStringSpacing.Name = "lblBridgeStringSpacing";
            this.lblBridgeStringSpacing.Size = new System.Drawing.Size(46, 13);
            this.lblBridgeStringSpacing.TabIndex = 6;
            this.lblBridgeStringSpacing.Text = "Spacing";
            // 
            // tlpNutSpacingAuto
            // 
            this.tlpNutSpacingAuto.AutoSize = true;
            this.tlpNutSpacingAuto.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpNutSpacingAuto.ColumnCount = 3;
            this.tlpNutSpacingAuto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpNutSpacingAuto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpNutSpacingAuto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpNutSpacingAuto.Controls.Add(this.lblNutSpacingMethod, 0, 2);
            this.tlpNutSpacingAuto.Controls.Add(this.lblNutSpacing, 0, 1);
            this.tlpNutSpacingAuto.Controls.Add(this.cboNutSpacingMethod, 1, 2);
            this.tlpNutSpacingAuto.Controls.Add(this.mtbNutSpacing, 1, 1);
            this.tlpNutSpacingAuto.Controls.Add(this.mtbNutSpread, 2, 1);
            this.tlpNutSpacingAuto.Controls.Add(this.lblNutTotalSpread, 2, 0);
            this.tlpNutSpacingAuto.Controls.Add(this.lblNutStringSpacing, 1, 0);
            this.tlpNutSpacingAuto.Controls.Add(this.cboNutSpacingAlignment, 1, 3);
            this.tlpNutSpacingAuto.Controls.Add(this.lblNutSpacingAlignment, 0, 3);
            this.tlpNutSpacingAuto.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpNutSpacingAuto.Location = new System.Drawing.Point(0, 0);
            this.tlpNutSpacingAuto.MinimumSize = new System.Drawing.Size(250, 0);
            this.tlpNutSpacingAuto.Name = "tlpNutSpacingAuto";
            this.tlpNutSpacingAuto.RowCount = 4;
            this.tlpNutSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpNutSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpNutSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpNutSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpNutSpacingAuto.Size = new System.Drawing.Size(346, 87);
            this.tlpNutSpacingAuto.TabIndex = 3;
            // 
            // lblNutSpacing
            // 
            this.lblNutSpacing.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNutSpacing.AutoSize = true;
            this.lblNutSpacing.Location = new System.Drawing.Point(21, 18);
            this.lblNutSpacing.Name = "lblNutSpacing";
            this.lblNutSpacing.Size = new System.Drawing.Size(66, 13);
            this.lblNutSpacing.TabIndex = 4;
            this.lblNutSpacing.Text = "Nut Spacing";
            // 
            // cboNutSpacingAlignment
            // 
            this.cboNutSpacingAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpNutSpacingAuto.SetColumnSpan(this.cboNutSpacingAlignment, 2);
            this.cboNutSpacingAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboNutSpacingAlignment.FormattingEnabled = true;
            this.cboNutSpacingAlignment.Location = new System.Drawing.Point(93, 65);
            this.cboNutSpacingAlignment.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.cboNutSpacingAlignment.Name = "cboNutSpacingAlignment";
            this.cboNutSpacingAlignment.Size = new System.Drawing.Size(250, 21);
            this.cboNutSpacingAlignment.TabIndex = 4;
            this.cboNutSpacingAlignment.SelectedIndexChanged += new System.EventHandler(this.cboNutSpacingAlignment_SelectedIndexChanged);
            // 
            // lblNutSpacingAlignment
            // 
            this.lblNutSpacingAlignment.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblNutSpacingAlignment.AutoSize = true;
            this.lblNutSpacingAlignment.Location = new System.Drawing.Point(34, 68);
            this.lblNutSpacingAlignment.Name = "lblNutSpacingAlignment";
            this.lblNutSpacingAlignment.Size = new System.Drawing.Size(53, 13);
            this.lblNutSpacingAlignment.TabIndex = 11;
            this.lblNutSpacingAlignment.Text = "Alignment";
            // 
            // tlpBridgeSpacingAuto
            // 
            this.tlpBridgeSpacingAuto.AutoSize = true;
            this.tlpBridgeSpacingAuto.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tlpBridgeSpacingAuto.ColumnCount = 3;
            this.tlpBridgeSpacingAuto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 90F));
            this.tlpBridgeSpacingAuto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBridgeSpacingAuto.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpBridgeSpacingAuto.Controls.Add(this.cboBridgeSpacingAlignment, 1, 3);
            this.tlpBridgeSpacingAuto.Controls.Add(this.cboBridgeSpacingMethod, 1, 2);
            this.tlpBridgeSpacingAuto.Controls.Add(this.lblBridgeSpacingMethod, 0, 2);
            this.tlpBridgeSpacingAuto.Controls.Add(this.lblBridgeSpacing, 0, 1);
            this.tlpBridgeSpacingAuto.Controls.Add(this.mtbBridgeSpacing, 1, 1);
            this.tlpBridgeSpacingAuto.Controls.Add(this.lblBridgeStringSpacing, 1, 0);
            this.tlpBridgeSpacingAuto.Controls.Add(this.mtbBridgeSpread, 2, 1);
            this.tlpBridgeSpacingAuto.Controls.Add(this.lblBridgeTotalSpread, 2, 0);
            this.tlpBridgeSpacingAuto.Controls.Add(this.lblBridgeSpacingAlignment, 0, 3);
            this.tlpBridgeSpacingAuto.Dock = System.Windows.Forms.DockStyle.Top;
            this.tlpBridgeSpacingAuto.Location = new System.Drawing.Point(0, 87);
            this.tlpBridgeSpacingAuto.MinimumSize = new System.Drawing.Size(250, 0);
            this.tlpBridgeSpacingAuto.Name = "tlpBridgeSpacingAuto";
            this.tlpBridgeSpacingAuto.RowCount = 4;
            this.tlpBridgeSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBridgeSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBridgeSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBridgeSpacingAuto.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tlpBridgeSpacingAuto.Size = new System.Drawing.Size(346, 87);
            this.tlpBridgeSpacingAuto.TabIndex = 9;
            // 
            // cboBridgeSpacingAlignment
            // 
            this.cboBridgeSpacingAlignment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpBridgeSpacingAuto.SetColumnSpan(this.cboBridgeSpacingAlignment, 2);
            this.cboBridgeSpacingAlignment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBridgeSpacingAlignment.FormattingEnabled = true;
            this.cboBridgeSpacingAlignment.Location = new System.Drawing.Point(93, 65);
            this.cboBridgeSpacingAlignment.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.cboBridgeSpacingAlignment.Name = "cboBridgeSpacingAlignment";
            this.cboBridgeSpacingAlignment.Size = new System.Drawing.Size(250, 21);
            this.cboBridgeSpacingAlignment.TabIndex = 10;
            this.cboBridgeSpacingAlignment.SelectedIndexChanged += new System.EventHandler(this.cboBridgeSpacingAlignment_SelectedIndexChanged);
            // 
            // cboBridgeSpacingMethod
            // 
            this.cboBridgeSpacingMethod.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.tlpBridgeSpacingAuto.SetColumnSpan(this.cboBridgeSpacingMethod, 2);
            this.cboBridgeSpacingMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBridgeSpacingMethod.FormattingEnabled = true;
            this.cboBridgeSpacingMethod.Location = new System.Drawing.Point(93, 40);
            this.cboBridgeSpacingMethod.Margin = new System.Windows.Forms.Padding(3, 3, 3, 1);
            this.cboBridgeSpacingMethod.Name = "cboBridgeSpacingMethod";
            this.cboBridgeSpacingMethod.Size = new System.Drawing.Size(250, 21);
            this.cboBridgeSpacingMethod.TabIndex = 11;
            this.cboBridgeSpacingMethod.SelectedIndexChanged += new System.EventHandler(this.cboBridgeSpacingMethod_SelectedIndexChanged);
            // 
            // lblBridgeSpacingMethod
            // 
            this.lblBridgeSpacingMethod.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBridgeSpacingMethod.AutoSize = true;
            this.lblBridgeSpacingMethod.Location = new System.Drawing.Point(44, 43);
            this.lblBridgeSpacingMethod.Name = "lblBridgeSpacingMethod";
            this.lblBridgeSpacingMethod.Size = new System.Drawing.Size(43, 13);
            this.lblBridgeSpacingMethod.TabIndex = 10;
            this.lblBridgeSpacingMethod.Text = "Method";
            // 
            // lblBridgeSpacing
            // 
            this.lblBridgeSpacing.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBridgeSpacing.AutoSize = true;
            this.lblBridgeSpacing.Location = new System.Drawing.Point(8, 18);
            this.lblBridgeSpacing.Name = "lblBridgeSpacing";
            this.lblBridgeSpacing.Size = new System.Drawing.Size(79, 13);
            this.lblBridgeSpacing.TabIndex = 10;
            this.lblBridgeSpacing.Text = "Bridge Spacing";
            // 
            // lblBridgeSpacingAlignment
            // 
            this.lblBridgeSpacingAlignment.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.lblBridgeSpacingAlignment.AutoSize = true;
            this.lblBridgeSpacingAlignment.Location = new System.Drawing.Point(34, 68);
            this.lblBridgeSpacingAlignment.Name = "lblBridgeSpacingAlignment";
            this.lblBridgeSpacingAlignment.Size = new System.Drawing.Size(53, 13);
            this.lblBridgeSpacingAlignment.TabIndex = 12;
            this.lblBridgeSpacingAlignment.Text = "Alignment";
            // 
            // StringSpacingEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoScrollMinSize = new System.Drawing.Size(250, 0);
            this.Controls.Add(this.tlpBridgeSpacingAuto);
            this.Controls.Add(this.tlpNutSpacingAuto);
            this.Name = "StringSpacingEditor";
            this.Size = new System.Drawing.Size(346, 308);
            this.tlpNutSpacingAuto.ResumeLayout(false);
            this.tlpNutSpacingAuto.PerformLayout();
            this.tlpBridgeSpacingAuto.ResumeLayout(false);
            this.tlpBridgeSpacingAuto.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MeasureTextbox mtbNutSpacing;
        private MeasureTextbox mtbNutSpread;
        private MeasureTextbox mtbBridgeSpacing;
        private MeasureTextbox mtbBridgeSpread;
        private System.Windows.Forms.Label lblNutStringSpacing;
        private System.Windows.Forms.Label lblNutTotalSpread;
        private System.Windows.Forms.Label lblBridgeTotalSpread;
        private System.Windows.Forms.Label lblBridgeStringSpacing;
        private System.Windows.Forms.ComboBox cboNutSpacingMethod;
        private System.Windows.Forms.Label lblNutSpacingMethod;
        private System.Windows.Forms.TableLayoutPanel tlpNutSpacingAuto;
        private System.Windows.Forms.ComboBox cboNutSpacingAlignment;
        private System.Windows.Forms.Label lblNutSpacing;
        private System.Windows.Forms.Label lblNutSpacingAlignment;
        private System.Windows.Forms.TableLayoutPanel tlpBridgeSpacingAuto;
        private System.Windows.Forms.ComboBox cboBridgeSpacingMethod;
        private System.Windows.Forms.Label lblBridgeSpacingMethod;
        private System.Windows.Forms.Label lblBridgeSpacing;
        private System.Windows.Forms.ComboBox cboBridgeSpacingAlignment;
        private System.Windows.Forms.Label lblBridgeSpacingAlignment;
    }
}
