using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Forms.VisualStyles;

namespace SiGen.UI.Controls
{
    [Designer(typeof(CollapsiblePanelDesigner))]
    public class CollapsiblePanel : Panel
    {
        private bool _Collapsed;
        private bool _Checked;
        private int _PanelHeight;
        private int _HeaderHeight;
        private bool _AutoSizeHeight;
        private bool _ShowCheckBox;
        private bool InternalSetHeight;
        private bool HasInitialized;
        private int ComputedHeaderHeight;
        private LeftRightAlignment _ArrowAlignment;
        private HorizontalAlignment _CheckBoxAlignment;

        [Browsable(true), RefreshProperties(RefreshProperties.Repaint)]
        [Localizable(true)]
        public override string Text { get => base.Text; set => base.Text = value; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public CollapsiblePanelContainer ContentPanel { get; private set; }

        public override Rectangle DisplayRectangle => GetContainerRectangle();

        public int PanelHeight
        {
            get => _PanelHeight;
            set
            {
                if (_PanelHeight != value && value > 6)
                {
                    _PanelHeight = value;
                    if (!AutoSizeHeight)
                        AdjustPanelSize();
                }
            }
        }

        [DefaultValue(-1)]
        public int HeaderHeight
        {
            get => _HeaderHeight;
            set
            {
                if (_HeaderHeight != value && (value == -1 || value >= 6))
                {
                    _HeaderHeight = value;
                    AdjustPanelSize();
                    RecalculateBounds();
                }
            }
        }

        [DefaultValue(false)]
        public bool Collapsed
        {
            get => _Collapsed;
            set => SetCollapsed(value);
        }

        [DefaultValue(false)]
        public bool AutoSizeHeight
        {
            get => _AutoSizeHeight;
            set
            {
                if (_AutoSizeHeight != value)
                {
                    _AutoSizeHeight = value;
                    AdjustContainerPanelDock();
                }
            }
        }

        [DefaultValue(LeftRightAlignment.Right)]
        public LeftRightAlignment ArrowAlignment
        {
            get => _ArrowAlignment;
            set
            {
                if (_ArrowAlignment != value)
                {
                    _ArrowAlignment = value;
                    RecalculateBounds();
                    if (IsHandleCreated)
                        Invalidate();
                }
            }
        }

        [DefaultValue(false)]
        public bool ShowCheckBox
        {
            get => _ShowCheckBox;
            set => SetShowCheckBox(value);
        }

        [DefaultValue(false)]
        public bool Checked
        {
            get => _Checked;
            set => SetChecked(value);
        }

        [DefaultValue(HorizontalAlignment.Left)]
        public HorizontalAlignment CheckBoxAlignment
        {
            get => _CheckBoxAlignment;
            set
            {
                if (_CheckBoxAlignment != value)
                {
                    _CheckBoxAlignment = value;
                    RecalculateBounds();
                    if (IsHandleCreated && ShowCheckBox)
                        Invalidate();
                }
            }
        }

        #region Events

        public event EventHandler CollapsedChanged;

        public event EventHandler AfterCollapse;

        public event EventHandler AfterExpand;

        public event EventHandler CheckedChanged;

        #endregion

        #region Variables

        protected bool IsOverHeader { get; private set; }
        protected bool IsOverCheckBox { get; private set; }

        protected Rectangle ArrowBounds { get; private set; }
        protected Rectangle CheckBoxBounds { get; private set; }
        protected Rectangle CaptionBounds { get; private set; }

        private Size CaptionSize;

        #endregion

        #region Ctor

        public CollapsiblePanel()
        {
            ContentPanel = new CollapsiblePanelContainer();
            Controls.Add(ContentPanel);

            ContentPanel.Dock = DockStyle.Fill;

            _HeaderHeight = -1;
            _PanelHeight = ContentPanel.Height;
            _ArrowAlignment = LeftRightAlignment.Right;

            SetStyle(ControlStyles.ResizeRedraw | 
                ControlStyles.OptimizedDoubleBuffer | 
                ControlStyles.AllPaintingInWmPaint, true);
        }

        #endregion

        #region Base Events

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            CalculateCaptionSize();
            CalculateHeaderHeight();
           
            HasInitialized = true;

            AdjustPanelSize();

            if(!Collapsed)
                ContentPanel.PerformLayout();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);

            CalculateCaptionSize();
            CalculateHeaderHeight();

            AdjustPanelSize();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            RecalculateBounds();
        }

        protected override void OnPaddingChanged(EventArgs e)
        {
            base.OnPaddingChanged(e);
            PerformLayout();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);

            CalculateCaptionSize();
            if (ShowCheckBox)
                RecalculateBounds();

            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if (!DesignMode)
            {
                var headerRect = GetHeaderRectangle();

                if (headerRect.Contains(e.Location) && e.Button == MouseButtons.Left)
                {
                    if (ShowCheckBox && IsOverCheckBox)
                        SetChecked(!Checked);
                    else
                        ToggleCollapsed();
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (DesignMode)
            {
                var headerRect = GetHeaderRectangle();
                if (headerRect.Contains(e.Location) && e.Button == MouseButtons.Left)
                    ToggleCollapsed();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var headerRect = GetHeaderRectangle();

            if (headerRect.Contains(e.Location))
            {
                if (!IsOverHeader)
                {
                    IsOverHeader = true;
                    Invalidate();
                }

                if (ShowCheckBox && IsOverHeader)
                {
                    bool isOverCheckbox = CheckBoxBounds.Contains(e.Location);

                    isOverCheckbox |= CaptionBounds.Contains(e.Location) && (e.X - CaptionBounds.Left) <= CaptionSize.Width;

                    if (isOverCheckbox != IsOverCheckBox)
                    {
                        IsOverCheckBox = isOverCheckbox;
                        Invalidate();
                    }
                }
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (IsOverHeader || IsOverCheckBox)
            {
                IsOverHeader = false;
                IsOverCheckBox = false;
                Invalidate();
            }
        }

        #endregion

        #region Property Change

        private void SetShowCheckBox(bool value)
        {
            if (_ShowCheckBox != value)
            {
                _ShowCheckBox = value;
                RecalculateBounds();
                if (IsHandleCreated)
                    Invalidate();
            }
        }

        #endregion

        #region Size & layout functions

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Rectangle GetHeaderRectangle()
        {
            return new Rectangle(0, 0, Width, GetHeaderHeight());
        }

        private int GetHeaderHeight()
        {
            if (HeaderHeight == -1)
            {
                if (ComputedHeaderHeight == 0)
                    CalculateHeaderHeight();
                return ComputedHeaderHeight;
            }
            return HeaderHeight;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public Rectangle GetContainerRectangle()
        {
            var rect = new Rectangle(0, 0, Width, Height);

            if (!Collapsed)
            {
                int headerHeight = GetHeaderHeight();
                if (!HasInitialized)
                    headerHeight = Math.Max(headerHeight, Font.Height);
                rect.X += Padding.Left;
                rect.Y += Padding.Top + headerHeight;
                rect.Width -= Padding.Horizontal;
                rect.Height -= Padding.Vertical + headerHeight;
            }

            return rect;
        }

        private void RecalculateBounds()
        {
            var headerRect = GetHeaderRectangle();

            if (ArrowAlignment == LeftRightAlignment.Left)
            {
                ArrowBounds = new Rectangle(headerRect.X, headerRect.Y, 20, 20);
                CaptionBounds = Rectangle.FromLTRB(ArrowBounds.Right, headerRect.Top, headerRect.Right, headerRect.Bottom);
            }
            else
            {
                ArrowBounds = new Rectangle(headerRect.Right - 20, headerRect.Y, 20, 20);
                CaptionBounds = Rectangle.FromLTRB(headerRect.Left + 3, headerRect.Top, ArrowBounds.Left, headerRect.Bottom);
            }

            if (ShowCheckBox)
                CalculateCheckboxBounds();
            else
                CheckBoxBounds = Rectangle.Empty;
        }

        private void CalculateCheckboxBounds()
        {
            int checkboxSize = 20;

            switch (CheckBoxAlignment)
            {
                case HorizontalAlignment.Left:
                    {
                        CheckBoxBounds = Rectangle.FromLTRB(
                        CaptionBounds.Left, CaptionBounds.Top,
                        CaptionBounds.Left + checkboxSize, CaptionBounds.Bottom
                        );
                        var textBounds = CaptionBounds;
                        textBounds.X = CheckBoxBounds.Right;
                        textBounds.Width -= CheckBoxBounds.Width;
                        CaptionBounds = textBounds;
                        break;
                    }
                case HorizontalAlignment.Right:
                    {
                        CheckBoxBounds = Rectangle.FromLTRB(
                        CaptionBounds.Right - checkboxSize, CaptionBounds.Top,
                        CaptionBounds.Right, CaptionBounds.Bottom
                        );
                        var textBounds = CaptionBounds;
                        //textBounds.X = CheckBoxBounds.Right;
                        textBounds.Width -= CheckBoxBounds.Width;
                        CaptionBounds = textBounds;
                        break;
                    }
                case HorizontalAlignment.Center:
                    CheckBoxBounds = Rectangle.FromLTRB(
                        CaptionBounds.Left + 3 + CaptionSize.Width, 
                        CaptionBounds.Top,
                        CaptionBounds.Left + 3 + CaptionSize.Width + checkboxSize, 
                        CaptionBounds.Bottom
                    );
                    break;
            }
        }

        private void CalculateCaptionSize()
        {
            if (!string.IsNullOrEmpty(Text))
                CaptionSize = TextRenderer.MeasureText(Text, Font);
            else
                CaptionSize = Size.Empty;
        }

        internal void AdjustPanelSize()
        {
            if (!HasInitialized)
                return;

            InternalSetHeight = true;

            if (!Collapsed)
            {
                int headerHeight = GetHeaderHeight();
                int newHeight = PanelHeight + headerHeight + Padding.Vertical;
                Height = newHeight;
            }
            else
            {
                Height = GetHeaderHeight();
            }
            
            InternalSetHeight = false;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (specified.HasFlag(BoundsSpecified.Height))
            {
                if (height == 0)
                    height = Height;

                if (!InternalSetHeight)
                {
                    if (!Collapsed)
                    {
                        if (AutoSizeHeight)
                            height = CalculateAutoHeight();
                        else if (HasInitialized)
                            _PanelHeight = Math.Max(height - GetHeaderHeight() - Padding.Vertical, 6);
                        //else
                        //    height = PanelHeight + GetHeaderHeight() + Padding.Vertical;
                    }
                    else if (Collapsed)
                        height = GetHeaderHeight();
                }
            }

            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnLayout(LayoutEventArgs levent)
        {
            base.OnLayout(levent);

            if (AutoSizeHeight && !Collapsed)
            {
                int newHeight = CalculateAutoHeight();
                if (newHeight != Height)
                    UpdateBounds(Left, Top, Width, newHeight);
            }
        }

        public int CalculateAutoHeight()
        {
            var prefSize = ContentPanel.GetPreferredSize(DisplayRectangle.Size);
            int totalHeight = GetHeaderHeight() + Padding.Vertical + prefSize.Height;
            _PanelHeight = prefSize.Height;
            return totalHeight;
        }

        private void CalculateHeaderHeight()
        {
            if (VisualStyleRenderer.IsSupported)
            {
                string headerText = "Wasd123_!";
                if (!string.IsNullOrEmpty(Text))
                    headerText += Text;

                ComputedHeaderHeight = TextRenderer.MeasureText(headerText, Font).Height + 6;

                ComputedHeaderHeight += 4; //button style padding
            }
            else if (IsHandleCreated)
            {
                string headerText = "Wasd123_!";
                if (!string.IsNullOrEmpty(Text))
                    headerText += Text;

                using (var g = CreateGraphics())
                    ComputedHeaderHeight = (int)g.MeasureString(headerText, Font).Height + 6;

                ComputedHeaderHeight += 4; //button style padding
            }
            else
                ComputedHeaderHeight = Font.Height + 6;

            if (HeaderHeight == -1)
                RecalculateBounds();
        }

        #endregion

        #region CheckBox Handling

        private void SetChecked(bool value)
        {
            if (value != _Checked)
            {
                _Checked = value;

                if (ShowCheckBox)
                {
                    OnCheckedChanged(EventArgs.Empty);

                    if (IsHandleCreated)
                        Invalidate();
                }
            }
        }

        protected virtual void OnCheckedChanged(EventArgs args)
        {
            CheckedChanged?.Invoke(this, args);
        }

        #endregion

        #region Collapse/Expand functions

        public void Collapse()
        {
            if (!Collapsed)
                SetCollapsed(true);
        }

        public void ToggleCollapsed()
        {
            SetCollapsed(!Collapsed);
        }

        public void Expand()
        {
            if (Collapsed)
                SetCollapsed(false);
        }

        public void SetCollapsed(bool collapsed)
        {
            if (_Collapsed != collapsed)
            {

                _Collapsed = collapsed;

                SuspendLayout();

                if (collapsed)
                {
                    var curSize = ContentPanel.Size;
                    var curPos = ContentPanel.Location;
                    ContentPanel.Visible = false;
                    ContentPanel.AutoSize = false;
                    ContentPanel.AutoSizeMode = AutoSizeMode.GrowOnly;
                    ContentPanel.Dock = DockStyle.None;
                    ContentPanel.Size = curSize;
                    ContentPanel.Location = curPos;
                }
                else
                {
                    ContentPanel.Dock = AutoSizeHeight ? DockStyle.Top : DockStyle.Fill;
                    ContentPanel.AutoSize = AutoSizeHeight;
                    ContentPanel.AutoSizeMode = AutoSizeHeight ? AutoSizeMode.GrowAndShrink : AutoSizeMode.GrowOnly;
                    ContentPanel.Visible = true;
                }

                ResumeLayout();

                AdjustPanelSize();

                //if (!collapsed)
                //    ContentPanel.PerformLayout();

                CollapsedChanged?.Invoke(this, EventArgs.Empty);

                if (_Collapsed)
                    AfterCollapse?.Invoke(this, EventArgs.Empty);
                else
                    AfterExpand?.Invoke(this, EventArgs.Empty);

                if (!Collapsed && Parent is ScrollableControl scrollableControl)
                {
                    scrollableControl.ScrollControlIntoView(this);
                }
            }
        }

        private void AdjustContainerPanelDock()
        {
            if (!Collapsed && ContentPanel != null)
            {
                ContentPanel.Dock = AutoSizeHeight ? DockStyle.Top : DockStyle.Fill;
                ContentPanel.AutoSize = AutoSizeHeight;
                ContentPanel.AutoSizeMode = AutoSizeHeight ? AutoSizeMode.GrowAndShrink : AutoSizeMode.GrowOnly;

                if (AutoSizeHeight)
                {
                    ContentPanel.PerformLayout();
                    InternalSetHeight = true;
                    Height = CalculateAutoHeight();
                    InternalSetHeight = false;
                }
            }
        }

        #endregion

        #region Drawing

        private Bitmap ArrowGlyph;

        protected Bitmap GetArrowGlyph()
        {
            if (ArrowGlyph == null && VisualStyleRenderer.IsSupported)
            {
                ArrowGlyph = new Bitmap(20, 20);
                using (var g = Graphics.FromImage(ArrowGlyph))
                {
                    var vsr = new VisualStyleRenderer(VisualStyleElement.ToolBar.SplitButtonDropDown.Normal);
                    vsr.DrawBackground(g, new Rectangle(0, 0, 20, 20));
                }
            }
            return ArrowGlyph;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            var headerRect = GetHeaderRectangle();
            var headerArgs = new PaintEventArgs(e.Graphics, headerRect);
            OnDrawHeader(headerArgs);
        }

        protected virtual void OnDrawHeader(PaintEventArgs e)
        {
            var headerRect = e.ClipRectangle;

            if (VisualStyleRenderer.IsSupported)
            {
                var elemStyle = VisualStyleElement.Button.PushButton.Normal;
                if (IsOverHeader)
                    elemStyle = VisualStyleElement.Button.PushButton.Hot;
                var vsr = new VisualStyleRenderer(elemStyle);
                vsr.DrawBackground(e.Graphics, headerRect);
            }
            else
            {

            }

            var glyph = GetArrowGlyph();

            if (glyph != null)
            {
                e.Graphics.TranslateTransform(
                    ArrowBounds.X + (ArrowBounds.Width / 2f),
                    (headerRect.Height / 2f));

                if (Collapsed)
                {
                    if (ArrowAlignment == LeftRightAlignment.Left)
                        e.Graphics.RotateTransform(-90);
                    else
                        e.Graphics.RotateTransform(90);
                }

                e.Graphics.DrawImage(ArrowGlyph, new Rectangle(
                    (ArrowBounds.Width / 2) * -1, (ArrowBounds.Height / 2) * -1, 
                    ArrowBounds.Width, ArrowBounds.Height));

                e.Graphics.ResetTransform();
            }

            using (var brush = new SolidBrush(ForeColor))
            {
                e.Graphics.DrawString(Text, Font, brush,
                    CaptionBounds,
                    new StringFormat() { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near });
            }

            if (ShowCheckBox)
            {
                if (VisualStyleRenderer.IsSupported)
                {
                    VisualStyleElement checkBoxStyle = Checked ? VisualStyleElement.Button.CheckBox.CheckedNormal : 
                        VisualStyleElement.Button.CheckBox.UncheckedNormal;

                    if (IsOverCheckBox)
                        checkBoxStyle = Checked ? 
                            VisualStyleElement.Button.CheckBox.CheckedHot :
                            VisualStyleElement.Button.CheckBox.UncheckedHot;

                    var vsr = new VisualStyleRenderer(checkBoxStyle);
                    vsr.DrawBackground(e.Graphics, CheckBoxBounds);
                }
                else
                {

                }
            }
        }

        #endregion

        [Designer(typeof(CollapsiblePanelContainerDesigner))]
        public class CollapsiblePanelContainer : Panel
        {
            public CollapsiblePanel ParentPanel => Parent as CollapsiblePanel;

            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public new Size Size
            {
                get => base.Size;
                set => base.Size = value;
            }

            public CollapsiblePanelContainer()
            {
                SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected override void OnLayout(LayoutEventArgs levent)
            {
                base.OnLayout(levent);
                if (Visible && ParentPanel.AutoSizeHeight)
                {
                    int newHeight = ParentPanel.CalculateAutoHeight();
                    ParentPanel.Height = newHeight;
                }
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                if (DesignMode)
                {
                    ControlPaint.DrawFocusRectangle(e.Graphics, ClientRectangle);
                }
            }
        }
    }

    #region Designer Classes

    internal class CollapsiblePanelDesigner : ParentControlDesigner
    {
        private DesignerActionListCollection _ActionList;
        private bool PassThrough;

        protected override bool AllowControlLasso => false;

        private CollapsiblePanel Panel => Control as CollapsiblePanel;

        public override SelectionRules SelectionRules
        {
            get
            {
                var rules = base.SelectionRules;
                if (Panel.Collapsed)
                {
                    rules &= ~(SelectionRules.TopSizeable | SelectionRules.BottomSizeable);
                }
                if (Panel.AutoSizeHeight)
                {
                    rules &= ~(SelectionRules.BottomSizeable);
                }
                return rules;
            }
        }

        public override DesignerActionListCollection ActionLists
        {
            get
            {
                if (_ActionList == null)
                {
                    _ActionList = new DesignerActionListCollection();
                    _ActionList.Add(new CollapsiblePanelActionList(this));
                }

                return _ActionList;
            }
        }

        public override void Initialize(IComponent component)
        {
            base.Initialize(component);
            EnableDesignMode((Control as CollapsiblePanel).ContentPanel, "ContentPanel");
        }


        public override bool CanParent(Control control)
        {
            if (control is CollapsiblePanel.CollapsiblePanelContainer)
                return true;
            return false;
        }

        protected override bool GetHitTest(Point point)
        {
            if (PassThrough)
                return false;

            var headerRect = Panel.GetHeaderRectangle();
            Point pt = Control.PointToClient(point);
            return headerRect.Contains(pt) || headerRect.Contains(point);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0201 /*|| m.Msg == 0x0202*/ || m.Msg == 0x0204 || m.Msg == 0x0205)
            {
                PassThrough = true;
                var tmpMsg = new Message()
                {
                    Msg = 0x084,
                    HWnd = m.HWnd,
                    LParam = m.LParam,
                    WParam = m.WParam
                };
                base.WndProc(ref tmpMsg);
                PassThrough = false;
            }

            base.WndProc(ref m);
        }


        private class CollapsiblePanelActionList : DesignerActionList
        {
            public bool Collapsed
            {
                get => (Component as CollapsiblePanel).Collapsed;
                set => (Component as CollapsiblePanel).Collapsed = value;
            }

            public CollapsiblePanelActionList(CollapsiblePanelDesigner designer) : base(designer.Component)
            {
            }
        }

    }

    internal class CollapsiblePanelContainerDesigner : ScrollableControlDesigner
    {
        public override SelectionRules SelectionRules => SelectionRules.Locked;

        protected override void PreFilterProperties(IDictionary properties)
        {
            properties.Remove("AutoSize");
            properties.Remove("AutoSizeMode");
            properties.Remove("Dock");
            properties.Remove("Margin");
            properties.Remove("Visible");
            //properties.Remove("Size");
            properties.Remove("Anchor");
            properties.Remove("MaximumSize");
            properties.Remove("BorderStyle");
            base.PreFilterProperties(properties);
        }
    }

    #endregion
}
