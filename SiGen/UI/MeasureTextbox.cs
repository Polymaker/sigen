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

namespace SiGen.UI
{
    public partial class MeasureTextbox : UserControl
    {
        private Measure _Value;
        private bool internalChange;
        private Rectangle measureBounds;
        private bool isValidating;
        public bool ReadOnly { get { return innerTextbox.ReadOnly; } set { innerTextbox.ReadOnly = value; } }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Measure Value
        {
            get { return _Value; }
            set
            {
                if (value != _Value)
                {
                    _Value = value;
                    SynchronizeValueToTextbox();
                }
            }
        }

        

        public MeasureTextbox()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            _Value = Measure.Mm(0);
            measureBounds = Rectangle.Empty;
            innerTextbox.Visible = false;
            
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
            var valueStr = _Value.ToString();
            if (valueStr.Contains("~"))
                valueStr = _Value.ToString(_Value.Unit, true);
            innerTextbox.Text = valueStr;
            internalChange = false;
        }

        protected override void OnPaint(PaintEventArgs pe)
        {

            base.OnPaint(pe);

            var curState = System.Windows.Forms.VisualStyles.TextBoxState.Normal;
            if (!Enabled)
                curState = System.Windows.Forms.VisualStyles.TextBoxState.Disabled;
            else if (ReadOnly)
                curState = System.Windows.Forms.VisualStyles.TextBoxState.Readonly;

            TextBoxRenderer.DrawTextBox(pe.Graphics, ClientRectangle, curState);

            using (var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center })
            {
                using(var b1 = new SolidBrush(ForeColor))
                    pe.Graphics.DrawString(_Value.ToString(), Font, b1, ClientRectangle, sf);

            }
        }

        private void UpdateMeasureBounds()
        {
            if (IsHandleCreated)
            {
                using (var g = this.CreateGraphics())
                {
                    var textSize = g.MeasureString(_Value.ToString(), Font);
                    measureBounds = new Rectangle((int)((Width - textSize.Width) / 2), (int)((Height - textSize.Height) / 2), (int)textSize.Width, (int)textSize.Height);
                }
            }
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
            UpdateMeasureBounds();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            UpdateMeasureBounds();
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if(e.Button == MouseButtons.Left && !innerTextbox.Visible)
            {
                innerTextbox.Visible = true;
                innerTextbox.Select();
            }
        }

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            height = FontHeight + 6;
            base.SetBoundsCore(x, y, width, height, specified);
            RepositionTextbox();
        }

        private void RepositionTextbox()
        {
            innerTextbox.Width = Width - 2;
            innerTextbox.Top = (Height - innerTextbox.Height) / 2;
        }

        private void innerTextbox_Leave(object sender, EventArgs e)
        {
            if (isValidating)
                SynchronizeValueToTextbox();
            innerTextbox.Visible = false;
        }

        private void innerTextbox_Validating(object sender, CancelEventArgs e)
        {
            if (!internalChange)
            {
                isValidating = true;
                Measure value = Measure.Empty;
                if (!MeasureParser.TryParse(innerTextbox.Text, out value, _Value.Unit))
                {
                    double noUnitValue = 0;
                    if (!double.TryParse(innerTextbox.Text, out noUnitValue))
                        e.Cancel = true;
                }
            }
            
        }

        private void innerTextbox_Validated(object sender, EventArgs e)
        {
            if (!internalChange)
            {
                double noUnitValue = 0;
                Measure value = Measure.Empty;
                var oldValue = _Value;
                if (MeasureParser.TryParse(innerTextbox.Text, out value, _Value.Unit))
                {
                    _Value = value;
                }
                else if (double.TryParse(innerTextbox.Text, out noUnitValue))
                {
                    _Value = new Measure(noUnitValue, _Value.Unit);
                }
                if (_Value.Unit == null)
                    _Value = new Measure(_Value.Value, oldValue.Unit);
                SynchronizeValueToTextbox();
                isValidating = false;
            }
        }

        private void cmsConvert_Opening(object sender, CancelEventArgs e)
        {
            tsmiConvertToMM.Visible = Value.Unit != UnitOfMeasure.Mm;
            tsmiConvertToCM.Visible = Value.Unit != UnitOfMeasure.Cm;
            tsmiConvertToIN.Visible = Value.Unit != UnitOfMeasure.In;
            tsmiConvertToFT.Visible = Value.Unit != UnitOfMeasure.Ft;
        }

        private void tsmiConvertToMM_Click(object sender, EventArgs e)
        {
            _Value.Unit = UnitOfMeasure.Mm;
            SynchronizeValueToTextbox();
            Invalidate();
        }

        private void tsmiConvertToCM_Click(object sender, EventArgs e)
        {
            _Value.Unit = UnitOfMeasure.Cm;
            SynchronizeValueToTextbox();
            Invalidate();
        }

        private void tsmiConvertToIN_Click(object sender, EventArgs e)
        {
            _Value.Unit = UnitOfMeasure.In;
            SynchronizeValueToTextbox();
            Invalidate();
        }

        private void tsmiConvertToFT_Click(object sender, EventArgs e)
        {
            _Value.Unit = UnitOfMeasure.Ft;
            SynchronizeValueToTextbox();
            Invalidate();
        }
    }
}
