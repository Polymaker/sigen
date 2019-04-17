using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI.Controls.ValueEditors
{
    public partial class ColorSelectButton : UserControl
    {
        private Color _Value;

        public Color Value
        {
            get => _Value;
            set
            {
                if (_Value != value)
                {
                    _Value = value;
                    if (IsHandleCreated)
                        Invalidate();
                    OnValueChanged(this, EventArgs.Empty);
                }
            }
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
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (btnPickColor.IsHandleCreated)
            {
                width = btnPickColor.Width + btnPickColor.Height;
                height = btnPickColor.Height;
            }
            base.SetBoundsCore(x, y, width, height, specified);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var brush = new SolidBrush(_Value))
                e.Graphics.FillRectangle(brush, 1, 1, Height - 2, Height - 2);

            e.Graphics.DrawRectangle(SystemPens.ActiveBorder, 1, 1, Height - 3, Height - 3);
        }

        protected virtual void OnValueChanged(object sender, EventArgs e)
        {
            ValueChanged?.Invoke(sender, e);
        }

        private void BtnPickColor_Click(object sender, EventArgs e)
        {
            using(var colorDlg = new ColorDialog())
            {
                colorDlg.Color = Value;
                if (colorDlg.ShowDialog(this) == DialogResult.OK)
                {
                    Value = colorDlg.Color;
                }
            }
        }
    }
}
