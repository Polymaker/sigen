using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;
using System.Windows.Forms.Design;
using System.Collections;
using SiGen.Utilities;
using System.Windows.Forms.Design.Behavior;
namespace SiGen.UI.Controls.ValueEditors
{
    [Designer(typeof(ColorSelectButtonDesigner))]
    [DefaultEvent("ValueChanged")]
    public partial class ColorSelectButton : UserControl
    {
        private Color _Value;

        public Color Value
        {
            get => _Value;
            set => SetColor(value);
        }

        [Browsable(false)]
        public override bool AutoSize { get => base.AutoSize; set => base.AutoSize = value; }

        public event EventHandler ValueChanged;

        public ColorSelectButton()
        {
            InitializeComponent();
            _Value = Color.Red;
            //HexTexbox.Text = "FF0000";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetColor(_Value, false);
        }

        internal TextBox GetTextBox()
        {
            return HexTexbox;
        }

        #region Size & Layout

        private int ColorPreviewWidth;
        private int MinimumBoxWidth;
        private int ButtonWidth;

        private void CalculateSizes()
        {
            ColorPreviewWidth = (int)Math.Round(HexTexbox.Height * 1.61803398875f);
            MinimumBoxWidth = MeasureStringWidth("FF33AA");
            ButtonWidth = btnPickColor.Width - 1;
        }

        private int MeasureStringWidth(string text)
        {
            return TextRenderer.MeasureText(text, Font,
                    new Size(9999, 9999), TextFormatFlags.TextBoxControl).Width;
        }

        private int GetColorPreviewSize()
        {
            return (int)(HexTexbox.Height * 1.61803398875f);
        }

        private void PositionControls()
        {
            HexTexbox.Left = ColorPreviewWidth - 1;
            HexTexbox.Width = (Width - ButtonWidth - HexTexbox.Left) + 2;
            btnPickColor.Left = Width - ButtonWidth;
            btnPickColor.Top = -1;
            btnPickColor.Height = HexTexbox.Height + 2;
            
            if (Height != HexTexbox.Height)
                Height = HexTexbox.Height;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (HexTexbox.IsHandleCreated)
            {
                //int ctrlSize = btnPickColor.Right - HexTexbox.Left;
                width = Math.Max(width, ColorPreviewWidth + MinimumBoxWidth + ButtonWidth);
                specified |= BoundsSpecified.Height;
                height = HexTexbox.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        private void ColorSelectButton_SizeChanged(object sender, EventArgs e)
        {
            PositionControls();
        }

        #endregion

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            CalculateSizes();
            PositionControls();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            CalculateSizes();
            PositionControls();
        }

        private void HexTexbox_SizeChanged(object sender, EventArgs e)
        {
            PositionControls();
        }

        

        private void SetColor(Color value, bool fireEvent = true)
        {
            if (_Value != value || !fireEvent)
            {
                _Value = value;

                if (IsHandleCreated)
                {
                    HexTexbox.Text = GetHexColorCode();
                    Invalidate();
                }

                if (fireEvent)
                    ValueChanged?.Invoke(this, EventArgs.Empty);
            }
            
        }

        private string GetHexColorCode()
        {
            return $"{Value.R:X2}{Value.G:X2}{Value.B:X2}";
        }


        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int colorPreviewWidth = GetColorPreviewSize();
            using (var brush = new SolidBrush(_Value))
                e.Graphics.FillRectangle(brush, 0, 0, colorPreviewWidth, Height);
            using(var pen = new Pen(Color.FromArgb(122,122,122)))
                e.Graphics.DrawRectangle(pen, 0, 0, colorPreviewWidth - 1, Height - 1);
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        private void BtnPickColor_Click(object sender, EventArgs e)
        {
            using (var colorDlg = new ColorDialog())
            {
                colorDlg.Color = Value;
                if (colorDlg.ShowDialog(this) == DialogResult.OK)
                    SetColor(colorDlg.Color);
            }
        }

        private void HexTexbox_Validating(object sender, CancelEventArgs e)
        {
            if (HexTexbox.TextLength != 6)
                e.Cancel = true;
            else
            {
                if (!int.TryParse(HexTexbox.Text, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out _))
                    e.Cancel = true;
            }
        }

        private void HexTexbox_Validated(object sender, EventArgs e)
        {
            if (int.TryParse(HexTexbox.Text, 
                NumberStyles.HexNumber, 
                CultureInfo.CurrentCulture, out int rgb))
            {
                int red = (rgb & 0xFF0000) >> 16;
                int green = (rgb & 0x00FF00) >> 8;
                int blue = rgb & 0x0000FF;
                var newColor = Color.FromArgb(red, green, blue);
                SetColor(newColor);
            }
        }

        private void HexTexbox_CommandKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                string expectedCode = GetHexColorCode();
                if (HexTexbox.Text != expectedCode)
                {
                    HexTexbox.Text = expectedCode;
                    e.Handled = true;
                }
                
            }
        }

        
    }

    class ColorSelectButtonDesigner : ControlDesigner
    {
        public override IList SnapLines
        {
            get
            {
                ArrayList arrayList = base.SnapLines as ArrayList;

                var editor = Control as ColorSelectButton;
                var editorTxt = editor.GetTextBox();

                int textBaseline = ControlHelper.GetTextBaseline(editorTxt ?? Control, ContentAlignment.TopLeft);
                BorderStyle borderStyle = editorTxt?.BorderStyle ?? BorderStyle.Fixed3D;

                switch (borderStyle)
                {
                    case BorderStyle.None:
                        break;
                    case BorderStyle.FixedSingle:
                        textBaseline += 2;
                        break;
                    case BorderStyle.Fixed3D:
                        textBaseline += 3;
                        break;
                }
                arrayList.Add(new SnapLine(SnapLineType.Baseline, textBaseline, SnapLinePriority.Medium));
                return arrayList;
            }
        }

        public override SelectionRules SelectionRules
        {
            get
            {
                var baseRules = base.SelectionRules;
                baseRules = baseRules & (~SelectionRules.BottomSizeable);
                baseRules = baseRules & (~SelectionRules.TopSizeable);
                return baseRules;
            }
        }
    }
}
