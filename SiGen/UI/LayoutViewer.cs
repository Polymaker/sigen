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
        private const int PADDING_BORDER = 6;
        private List<Vector> intersections;
        private Vector measure1;
        private Vector measure2;

        public SILayout CurrentLayout
        {
            get { return _CurrentLayout; }
            set
            {
                if (value != _CurrentLayout)
                    SetLayout(value);
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
            measure1 = Vector.Empty;
            measure2 = Vector.Empty;
            intersections = new List<Vector>();
            _DisplayConfig = new LayoutViewerDisplayConfig();
            _DisplayConfig.PropertyChanged += DisplayConfigChanged;
        }

        private void DisplayConfigChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsHandleCreated)
                Invalidate();
        }

        private void SetLayout(SILayout layout)
        {
            if(_CurrentLayout != null)
                _CurrentLayout.LayoutUpdated -= CurrentLayoutUpdated;
            _CurrentLayout = layout;
            if (_CurrentLayout != null)
                _CurrentLayout.LayoutUpdated += CurrentLayoutUpdated;

            ResetCamera();
            CalculateIntersections();
        }

        private void CurrentLayoutUpdated(object sender, EventArgs e)
        {
            if (IsHandleCreated)
            {
                if (!manualZoom)
                    ResetCamera();
                else
                    Invalidate();
                CalculateIntersections();
            }
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

            if(CurrentLayout != null)
            {
                DrawFingerboard(pe.Graphics);
                DrawFrets(pe.Graphics);
                DrawStrings(pe.Graphics);
            }

            if (!measure1.IsEmpty)
            {
                DrawLine(pe.Graphics, measure1, measure2, Color.Black);
            }

            pe.Graphics.ResetTransform();

        }

        private void DrawVisualElement(Graphics g, VisualElement elem)
        {
            if (elem is StringLine)
                DrawString(g, (StringLine)elem);
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

        private void DrawFingerboard(Graphics g)
        {
            foreach (var edge in CurrentLayout.VisualElements.OfType<StringCenter>())
            {
                DrawLine(g, edge.P1, edge.P2, Color.Gainsboro);
            }

            foreach (var edge in CurrentLayout.VisualElements.OfType<FingerboardEdge>())
            {
                DrawLine(g, edge.P1, edge.P2, Color.Blue);
            }
        }

        private void DrawFrets(Graphics g)
        {
            Pen fretPen = null;
            Pen nutPen = new Pen(Color.Red, 1f / (float)_Zoom);
            if (!DisplayConfig.RenderRealFrets)
                fretPen = new Pen(Color.Red, 1f / (float)_Zoom);
            else
                fretPen = new Pen(Color.DarkGray, (float)DisplayConfig.FretWidth.NormalizedValue);

            foreach (var fretLine in CurrentLayout.VisualElements.OfType<FretLine>())
            {
                var penToUse = fretLine.IsNut ? nutPen : fretPen;
                if (fretLine.IsStraight)
                    g.DrawLines(penToUse, fretLine.Points.Select(p => PointToUI(p)).ToArray());
                else
                    g.DrawCurve(penToUse, fretLine.Points.Select(p => PointToUI(p)).ToArray(), 0.3f);

                //g.DrawLine(new Pen(Color.Blue, 1f / (float)_Zoom), PointToUI(fretLine.Segments.First().PointOnString), PointToUI(fretLine.Segments.Last().PointOnString));
                //g.DrawLines(new Pen(Color.Red, 1f / (float)_Zoom), fretLine.Points.Select(p => PointToUI(p)).ToArray());
            }
            nutPen.Dispose();
            fretPen.Dispose();
        }

        private void DrawStrings(Graphics g)
        {
            foreach (var stringLine in CurrentLayout.VisualElements.OfType<StringLine>())
            {
                if (DisplayConfig.RenderRealStrings)
                {
                    if (stringLine.String.Gauge != Measure.Empty && stringLine.String.Gauge > Measure.Zero)
                    {
                        DrawLine(g, stringLine.P1, stringLine.P2, Color.Black, stringLine.String.Gauge);
                    }
                }
                else
                    DrawLine(g, stringLine.P1, stringLine.P2, Color.Black);
            }
        }

        private Vector PointToVector(PointM pos)
        {
            if (DisplayConfig.FretboardOrientation == Orientation.Horizontal)
                return new Vector(pos.Y.NormalizedValue, pos.X.NormalizedValue * -1);
            return new Vector(pos.X.NormalizedValue, pos.Y.NormalizedValue);
        }

        private PointF PointToUI(PointM point)
        {
            var vec = PointToVector(point);
            return new PointF((float)vec.X, (float)vec.Y * -1);
        }

        private void DrawLine(Graphics g, PointM p1, PointM p2, Color color)
        {
            DrawLine(g, PointToVector(p1), PointToVector(p2), color);
        }

        private void DrawLine(Graphics g, Vector p1, Vector p2, Color color)
        {
            using (var pen = new Pen(color, 1f / (float)_Zoom))
                g.DrawLine(pen, (float)p1.X, (float)p1.Y * -1, (float)p2.X, (float)p2.Y * -1);
        }

        private void DrawLine(Graphics g, PointM p1, PointM p2, Color color, Measure size)
        {
            DrawLine(g, PointToVector(p1), PointToVector(p2), color, size);
        }

        private void DrawLine(Graphics g, Vector p1, Vector p2, Color color, Measure size)
        {
            using (var pen = new Pen(color, (float)size.NormalizedValue))
                g.DrawLine(pen, (float)p1.X, (float)p1.Y * -1, (float)p2.X, (float)p2.Y * -1);
        }

        private void DrawLine(Graphics g, PointM[] points, Color color, Measure size)
        {
            DrawLine(g, points.Select(p => PointToVector(p)).ToArray(), color, size);
        }

        private void DrawLine(Graphics g, Vector[] points, Color color, Measure size)
        {
            using (var pen = new Pen(color, (float)size.NormalizedValue))
            {
                var gdiPoints = points.Select(p => new PointF((float)p.X, (float)p.Y * -1)).ToArray();
                g.DrawLines(pen, gdiPoints);
            }
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

        private void CalculateIntersections()
        {
            intersections.Clear();
            if(CurrentLayout != null && CurrentLayout.VisualElements.Count > 0)
            {
                var fretPos = CurrentLayout.VisualElements.OfType<FretLine>().SelectMany(fl => fl.Segments.Where(s => !s.IsVirtual)).Select(s=>s.PointOnString).Distinct();
                intersections.AddRange(fretPos.Select(p => PointToVector(p)));

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
                        _Zoom = (Width - (PADDING_BORDER * 2f)) / (float)layoutBounds.Height.NormalizedValue;
                    }
                    else
                    {
                        _Zoom = (Height - (PADDING_BORDER * 2f)) / (float)layoutBounds.Height.NormalizedValue;
                    }
                    var centerY = layoutBounds.Center.Y.NormalizedValue;
                    cameraPosition = Vector.Zero;
                    if (DisplayConfig.FretboardOrientation == Orientation.Vertical)
                        cameraPosition.Y += centerY;
                    else
                        cameraPosition.X += centerY;
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

            if (e.Button == MouseButtons.Left)
            {
                if (measure1.IsEmpty)
                {
                    var curPos = DisplayToWorld(e.Location);
                    var closeInters = intersections.Where(i => (i - curPos).Length <= 0.1);
                    if (closeInters.Any())
                    {
                        measure1 = closeInters.First();
                    }
                }
                else
                {
                    var measuredLength = Measure.FromNormalizedValue((measure2 - measure1).Length, UnitOfMeasure.Mm);
                    Console.WriteLine(string.Format("Measured length: {0}", measuredLength));
                    measure1 = Vector.Empty;
                    Invalidate();
                }
                
            }
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
            else
            {
                if (!measure1.IsEmpty)
                {
                    measure2 = DisplayToWorld(e.Location);
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
