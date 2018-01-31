using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public partial class LayoutViewer : Control
    {
        private double _Zoom;
        private LayoutViewerDisplayConfig _DisplayConfig;
        private bool manualZoom;
        private Vector cameraPosition;
        private Vector dragStart;
        private SILayout _CurrentLayout;

        public SILayout CurrentLayout
        {
            get { return _CurrentLayout; }
            set
            {
                if(value != _CurrentLayout)
                {
                    _CurrentLayout = value;
                    ResetCamera();
                }
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content), TypeConverter(typeof(ExpandableObjectConverter))]
        public LayoutViewerDisplayConfig DisplayConfig
        {
            get { return _DisplayConfig; }
        }

        public LayoutViewer()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable, true);
            _Zoom = 1d;
            dragStart = Vector.Empty;
            cameraPosition = Vector.Zero;
            _DisplayConfig = new LayoutViewerDisplayConfig();
            _DisplayConfig.PropertyChanged += DisplayConfigChanged;
        }

        private void DisplayConfigChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsHandleCreated)
                Invalidate();
        }

        #region Drawing

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            var zoomf = (float)_Zoom;
            var center = new PointF(Width / 2f, Height / 2f);
            pe.Graphics.TranslateTransform(center.X, center.Y);
            pe.Graphics.ScaleTransform((float)_Zoom, (float)_Zoom);
            pe.Graphics.TranslateTransform((float)cameraPosition.X * -1, (float)cameraPosition.Y);

            //pe.Graphics.DrawRectangle(Pens.Black, -10, -10, 20, 20);
            //pe.Graphics.DrawRectangle(Pens.Black, -20, -20, 40, 40);
            //DrawLine(pe.Graphics, new Vector(-10, -10), new Vector(10, 10), Color.Red);

            if(CurrentLayout != null)
            {
                foreach(var vElem in CurrentLayout.VisualElements/*.OfType<LayoutLine>()*/)
                {
                    DrawVisualElement(pe.Graphics, vElem);
                    //DrawLine(pe.Graphics, PointToVector(vElem.P1), PointToVector(vElem.P2), vElem.ElementType == VisualElementType.FingerboardEdge ? Color.Blue : Color.Black);
                }
            }
        }

        private void DrawVisualElement(Graphics g, VisualElement elem)
        {
            if (elem is StringLine)
                DrawString(g, (StringLine)elem);

            switch (elem.ElementType)
            {
                case VisualElementType.FingerboardEdge:
                    var edgeLine = (LayoutLine)elem;
                    DrawLine(g, PointToVector(edgeLine.P1), PointToVector(edgeLine.P2), Color.Blue);
                    break;
                //case VisualElementType.String:
                //    var stringLine = (StringLine)elem;
                //    DrawLine(g, PointToVector(stringLine.P1), PointToVector(stringLine.P2), Color.Black);
                //    break;
                case VisualElementType.StringCenter:
                case VisualElementType.GuideLine:
                    var guideLine = (LayoutLine)elem;
                    DrawLine(g, PointToVector(guideLine.P1), PointToVector(guideLine.P2), Color.Silver);
                    break;
            }
        }

        private void DrawString(Graphics g, StringLine stringLine)
        {
            if (DisplayConfig.RenderRealStrings)
            {
                if(stringLine.String.Gauge != Measure.Empty && stringLine.String.Gauge > Measure.Zero)
                {
                    var stringColor = Color.Black;// stringLine.String.Gauge[UnitOfMeasure.In] >= 0.02 ? Color.LightGray : Color.DimGray;
                    //if(stringLine.String.Gauge[UnitOfMeasure.In] >= 0.02)
                    //    DrawLine(g, PointToVector(stringLine.P1), PointToVector(stringLine.P2), Color.DimGray, stringLine.String.Gauge + Measure.FromNormalizedValue(1.5 / _Zoom,null));
                    DrawLine(g, PointToVector(stringLine.P1), PointToVector(stringLine.P2), stringColor, stringLine.String.Gauge);
                }
            }
            else
            {
                DrawLine(g, PointToVector(stringLine.P1), PointToVector(stringLine.P2), Color.Black);
            }
        }

        private Vector PointToVector(PointM pos)
        {
            if (DisplayConfig.FretboardOrientation == Orientation.Horizontal)
                return new Vector(pos.Y.NormalizedValue, pos.X.NormalizedValue * -1);
            return new Vector(pos.X.NormalizedValue, pos.Y.NormalizedValue);
        }

        private void DrawLine(Graphics g, Vector p1, Vector p2, Color color)
        {
            using (var pen = new Pen(color, 1f / (float)_Zoom))
                g.DrawLine(pen, (float)p1.X, (float)p1.Y * -1, (float)p2.X, (float)p2.Y * -1);
        }

        private void DrawLine(Graphics g, Vector p1, Vector p2, Color color, Measure size)
        {
            using (var pen = new Pen(color, (float)size.NormalizedValue))
                g.DrawLine(pen, (float)p1.X, (float)p1.Y * -1, (float)p2.X, (float)p2.Y * -1);
        }

        #endregion

        public void ResetCamera()
        {
            cameraPosition = Vector.Zero;
            ZoomToFit();
            if (IsHandleCreated)
                Invalidate();
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (!manualZoom)
            {
                ZoomToFit();
            } 
        }

        private void ZoomToFit()
        {
            _Zoom = 1d;

            if (CurrentLayout != null)
            {
                var layoutBounds = CurrentLayout.GetBounds();
                if (!layoutBounds.IsEmpty)
                {
                    if (DisplayConfig.FretboardOrientation == Orientation.Horizontal)
                    {
                        _Zoom = (Width - 6f) / (float)layoutBounds.Height.NormalizedValue;
                    }
                    else
                    {
                        _Zoom = (Height - 6f) / (float)layoutBounds.Height.NormalizedValue;
                    }
                }
            }
            manualZoom = false;
        }

        #region Mouse Handling

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Select();
            if (e.Button == MouseButtons.Middle)
                dragStart = DisplayToLocal(e.Location);
            else if (e.Button == MouseButtons.Left)
            {
                var worldPos = DisplayToWorld(e.Location);
                Console.WriteLine(worldPos);
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            if (e.Button == MouseButtons.Middle)
                dragStart = Vector.Empty;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (!dragStart.IsEmpty)
            {
                var curPos = DisplayToLocal(e.Location);
                var diffVec = curPos - dragStart;
                if (diffVec.Length > 1)
                {
                    manualZoom = true;
                    diffVec *= -1;
                    cameraPosition += diffVec / _Zoom;
                    dragStart = curPos;
                    Invalidate();
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);
            if (e.Button == MouseButtons.Middle)
                ResetCamera();
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            if (/*(ModifierKeys & Keys.Control) == Keys.Control*/true)
            {
                var oldZoom = _Zoom;
                var mousePos = e.Location;
                manualZoom = true;
                if (e.Delta > 0)
                    _Zoom *= 1.1;
                else
                    _Zoom *= 0.9;

                var curWorldPos = DisplayToWorld(mousePos, oldZoom);
                var finalWorldPos = DisplayToWorld(mousePos);
                cameraPosition -= (finalWorldPos - curWorldPos);
                Invalidate();
            }
        }

        #endregion

        #region UI <-> 2D coordinates

        private Vector DisplayToLocal(Point pt)
        {
            var center = new Vector(Width / 2d, Height / 2d);
            var pos = new Vector(pt.X, (Height - 1) - pt.Y);
            return pos - center;
        }

        private Vector DisplayToWorld(Point pt)
        {
            return DisplayToWorld(pt, _Zoom);
        }

        private Vector DisplayToWorld(Point pt, double zoom)
        {
            return (DisplayToLocal(pt) / zoom) + cameraPosition;
        }

        #endregion
    }
}
