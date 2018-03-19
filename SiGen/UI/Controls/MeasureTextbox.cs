using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SiGen.Measuring;
using SiGen.Utilities;
using System.Windows.Forms.VisualStyles;
using System.Windows.Forms.Design;

namespace SiGen.UI
{
    [Designer(typeof(MeasureTextboxDesigner)), DefaultEvent("ValueChanged")]
    public partial class MeasureTextbox : UserControl
    {
        private Measure _Value;
        private bool internalChange;
        private bool _IsEditing;
        private bool _AllowEmptyValue;
        private bool isMouseOver;
        private Rectangle measureBounds;

        [DefaultValue(false), RefreshProperties(RefreshProperties.All | RefreshProperties.Repaint)]
        public bool ReadOnly
        {
            get { return innerTextbox.ReadOnly; }
            set
            {
                if(value != innerTextbox.ReadOnly)
                {
                    innerTextbox.ReadOnly = value;
                    Invalidate();
                }
            }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Measure Value
        {
            get { return _Value; }
            set
            {
                if (value != _Value || value.Unit != _Value.Unit)
                {
                    if (_IsEditing)
                        CancelEdit();
                    _Value = value;
                    SynchronizeValueToTextbox();
                    OnValueChanged(EventArgs.Empty);
                }
            }
        }

        [DefaultValue(false)]
        public bool AllowEmptyValue
        {
            get { return _AllowEmptyValue; }
            set
            {
                if(_AllowEmptyValue != value)
                {
                    _AllowEmptyValue = value;
                }
            }
        }

        [Browsable(false)]
        public bool IsEditing
        {
            get { return _IsEditing; }
        }

        #region Events

        public event EventHandler BeginEdit;
        public event EventHandler EndEdit;
        public event EventHandler ValueChanged;

        #endregion

        public MeasureTextbox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer | ControlStyles.Selectable, true);
            _Value = Measure.Mm(0);
            measureBounds = Rectangle.Empty;
            innerTextbox.Visible = false;
            base.BackColor = Color.Empty;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            UpdateMeasureBounds();
            SynchronizeValueToTextbox();
        }

        private void SynchronizeValueToTextbox()
        {
            internalChange = true;
            if (_Value.IsEmpty)
                innerTextbox.Text = string.Empty;
            else
            {
                var valueStr = _Value.ToString(Measure.MeasureFormatFlag.DisableApproximation);
                //if (valueStr.Contains("~"))
                //    valueStr = _Value.ToString(_Value.Unit, true);
                innerTextbox.Text = valueStr;
            }
            internalChange = false;

            if (IsHandleCreated)
                Invalidate();
            UpdateMeasureBounds();
        }

        #region Drawing

        const int EPSN_NORMAL = 1;
        const int EPSN_HOT = 2;
        const int EPSN_FOCUSED = 3;
        const int EPSN_DISABLED = 4;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            if (VisualStyleRenderer.IsSupported)
                RenderVisualStyleTextBox(pe.Graphics);

            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                using (var b1 = new SolidBrush(!Enabled ? SystemColors.GrayText : ForeColor))
                    pe.Graphics.DrawString(_Value.ToString(), Font, b1, ClientRectangle, sf);
            }
        }

        private void RenderVisualStyleTextBox(Graphics g)
        {
            VisualStyleElement borderElement = null;

            if (!Enabled)
                borderElement = VisualStyleElement.CreateElement("EDIT", 6, EPSN_DISABLED);
            else if (ContainsFocus)
                borderElement = VisualStyleElement.CreateElement("EDIT", 6, EPSN_FOCUSED);
            else if (isMouseOver)
                borderElement = VisualStyleElement.CreateElement("EDIT", 6, EPSN_HOT);
            else
                borderElement = VisualStyleElement.CreateElement("EDIT", 6, EPSN_NORMAL);

            if (borderElement != null && VisualStyleRenderer.IsElementDefined(borderElement))
            {
                VisualStyleRenderer borderRenderer = new VisualStyleRenderer(borderElement);
                borderRenderer.DrawBackground(g, ClientRectangle);

                if (ReadOnly || !Enabled || BackColor != SystemColors.Window)
                {
                    var textBoxRenderer = new VisualStyleRenderer(VisualStyleElement.TextBox.TextEdit.ReadOnly);
                    var textBounds = textBoxRenderer.GetBackgroundContentRectangle(g, ClientRectangle);
                    textBounds.Inflate(-1, -1);

                    if (!Enabled)
                    {
                        g.FillRectangle(SystemBrushes.Control, textBounds);
                    }
                    else if(BackColor != SystemColors.Window)
                    {
                        using (var brush = new SolidBrush(BackColor))
                            g.FillRectangle(brush, textBounds);
                    }
                }
            }
        }

        #endregion

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            isMouseOver = true;
            Invalidate();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            isMouseOver = false;
            Invalidate();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!innerTextbox.Visible && measureBounds != Rectangle.Empty)
            {
                if (measureBounds.Contains(e.Location))
                    Cursor = Cursors.IBeam;
                else
                    Cursor = Cursors.Default;
            }
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            Height = FontHeight + (SystemInformation.BorderSize.Height * 4) + 3;
            UpdateMeasureBounds();
        }

        protected override void OnForeColorChanged(EventArgs e)
        {
            base.OnForeColorChanged(e);
            innerTextbox.ForeColor = ForeColor;
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateMeasureBounds();
        }

        private bool preventFocus;

        protected override void OnGotFocus(EventArgs e)
        {
            if(!preventFocus)
                ShowTextBox();
            preventFocus = false;

            Invalidate();//repaint border

            base.OnGotFocus(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!ContainsFocus && e.Button == MouseButtons.Right)
                preventFocus = true;

            if (e.Button == MouseButtons.Left && measureBounds.Contains(e.Location))
                ShowTextBox(true);

            base.OnMouseDown(e);
        }

        protected void ShowTextBox(bool select = false)
        {
            if (!innerTextbox.Visible)
            {
                innerTextbox.BackColor = BackColor;
                innerTextbox.Visible = true;
                innerTextbox.Focus();
                if(select)
                    innerTextbox.SelectAll();
            }
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();//repaint border
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            height = FontHeight + (SystemInformation.BorderSize.Height * 4) + 3;
            base.SetBoundsCore(x, y, width, height, specified);
            RepositionTextbox();
        }

        private void UpdateMeasureBounds()
        {
            if (IsHandleCreated)
            {
                using (var g = CreateGraphics())
                {
                    
                    var textSize = g.MeasureString(_Value.ToString(), Font);
                    measureBounds = new Rectangle((int)((Width - textSize.Width) / 2), (int)((Height - textSize.Height) / 2), (int)textSize.Width, (int)textSize.Height);
                }
            }
        }

        private void RepositionTextbox()
        {
            innerTextbox.Width = Width - 4;
            innerTextbox.Top = (Height - innerTextbox.Height) / 2;
        }

        public override Color BackColor
        {
            get
            {
                if (ShouldSerializeBackColorBase())
                    return base.BackColor;
                if (ReadOnly)
                    return SystemColors.Control;
                return SystemColors.Window;
            }
            set
            {
                base.BackColor = value;
            }
        }

        public bool ShouldSerializeBackColorBase()
        {
            return (bool)typeof(Control).GetMethod("ShouldSerializeBackColor", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(this, null);
        }

        #region Context Menu

        private void cmsConvert_Opening(object sender, CancelEventArgs e)
        {
            tsmiConvertToMM.Visible = Value.Unit != UnitOfMeasure.Mm;
            tsmiConvertToCM.Visible = Value.Unit != UnitOfMeasure.Cm;
            tsmiConvertToIN.Visible = Value.Unit != UnitOfMeasure.In;
            tsmiConvertToFT.Visible = Value.Unit != UnitOfMeasure.Ft;
            tsmiClearValue.Visible = AllowEmptyValue;
            toolStripSeparator1.Visible = AllowEmptyValue;
        }

        private void tsmiConvertToMM_Click(object sender, EventArgs e)
        {
            PerformEndEdit(true);
            _Value.Unit = UnitOfMeasure.Mm;
            SynchronizeValueToTextbox();
        }

        private void tsmiConvertToCM_Click(object sender, EventArgs e)
        {
            PerformEndEdit(true);
            _Value.Unit = UnitOfMeasure.Cm;
            SynchronizeValueToTextbox();
        }

        private void tsmiConvertToIN_Click(object sender, EventArgs e)
        {
            PerformEndEdit(true);
            _Value.Unit = UnitOfMeasure.In;
            SynchronizeValueToTextbox();
        }

        private void tsmiConvertToFT_Click(object sender, EventArgs e)
        {
            PerformEndEdit(true);
            _Value.Unit = UnitOfMeasure.Ft;
            SynchronizeValueToTextbox();
        }

        private void tsmiClearValue_Click(object sender, EventArgs e)
        {
            Value = Measure.Empty;
        }

        #endregion

        #region Editing

        private void innerTextbox_TextChanged(object sender, EventArgs e)
        {
            if (!internalChange)
            {
                if (!_IsEditing)
                {
                    _IsEditing = true;
                    OnBeginEdit(EventArgs.Empty);
                }
            }
        }

        public void PerformEndEdit(bool keepTextBoxVisible = false)
        {
            if (_IsEditing)
            {
                Measure newValue = Measure.Empty;

                if(!string.IsNullOrEmpty(innerTextbox.Text) || !AllowEmptyValue)
                {
                    if (!MeasureParser.TryParse(innerTextbox.Text, out newValue, _Value.Unit))
                        newValue = _Value;
                }

                _IsEditing = false;
                if(!keepTextBoxVisible)
                    innerTextbox.Visible = false;

                OnEndEdit(EventArgs.Empty);
                if (newValue != Value)
                    Value = newValue;
                else
                    SynchronizeValueToTextbox();
            }
            else if (innerTextbox.Visible && !keepTextBoxVisible)
                innerTextbox.Visible = false;
        }

        public void CancelEdit()
        {
            if (_IsEditing)
            {
                _IsEditing = false;
                innerTextbox.Visible = false;
                SynchronizeValueToTextbox();
                OnEndEdit(EventArgs.Empty);
            }
            else if (innerTextbox.Visible)
                innerTextbox.Visible = false;
        }

        protected void OnBeginEdit(EventArgs e)
        {
            var handler = BeginEdit;
            if (handler != null)
                handler(this, e);
        }

        protected void OnEndEdit(EventArgs e)
        {
            var handler = EndEdit;
            if (handler != null)
                handler(this, e);
        }

        protected virtual void OnValueChanged(EventArgs e)
        {
            var handler = ValueChanged;
            if (handler != null)
                handler(this, e);
        }

        private void innerTextbox_Validating(object sender, CancelEventArgs e)
        {
            if (!internalChange && _IsEditing)
            {
                Measure value = Measure.Empty;

                if(string.IsNullOrEmpty(innerTextbox.Text))
                {
                    if(!AllowEmptyValue)
                    {
                        System.Media.SystemSounds.Exclamation.Play();
                        e.Cancel = true;
                    }
                }
                else if (!MeasureParser.TryParse(innerTextbox.Text, out value, _Value.Unit))
                {
                    System.Media.SystemSounds.Exclamation.Play();
                    e.Cancel = true;
                }
            }
        }

        private void innerTextbox_Validated(object sender, EventArgs e)
        {
            PerformEndEdit();
            Invalidate();
        }

        private void innerTextbox_CommandKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Escape)
            {
                if (IsEditing)
                {
                    CancelEdit();
                    e.Handled = true;
                }
                else if (innerTextbox.Visible)
                {
                    innerTextbox.Visible = false;
                    e.Handled = true;
                }      
            }
            else if(e.KeyData == Keys.Tab)
                Parent.SelectNextControl(this, Control.ModifierKeys != Keys.Shift, true, true, true);
        }

        #endregion
    }

    internal class MeasureTextboxDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                selectionRules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                return selectionRules;
            }
        }
    }
}
