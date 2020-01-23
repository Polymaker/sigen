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
            btnPickColor.Top = 0;
            btnPickColor.Left = Width - btnPickColor.Width;
            _Value = Color.Red;
            //HexTexbox.Text = "FF0000";
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            PositionControls();

            SetColor(_Value, false);
        }

        internal TextBox GetTextBox()
        {
            return HexTexbox;
        }

        #region Size & Layout

        private int GetColorPreviewSize()
        {
            return (int)(HexTexbox.Height * 1.61803398875f);
        }

        private void PositionControls()
        {
            HexTexbox.Left = GetColorPreviewSize() - 1;
            btnPickColor.Left = HexTexbox.Right - 2;
            btnPickColor.Top = -1;
            Height = HexTexbox.Height;
            btnPickColor.Height = HexTexbox.Height + 2;
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (HexTexbox.IsHandleCreated)
            {
                int ctrlSize = btnPickColor.Right - HexTexbox.Left;
                width = ctrlSize + GetColorPreviewSize() - 1;
                specified |= BoundsSpecified.Height;
                height = HexTexbox.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnParentFontChanged(EventArgs e)
        {
            base.OnParentFontChanged(e);
            PositionControls();
        }

        protected override void OnFontChanged(EventArgs e)
        {
            base.OnFontChanged(e);
            PositionControls();
        }

        private void HexTexbox_SizeChanged(object sender, EventArgs e)
        {
            PositionControls();
        }


        #endregion

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
    }
}
