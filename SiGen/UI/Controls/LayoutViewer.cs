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
        private Vector cameraPosition;//independant of orientation
        private Vector dragStart;
        private SILayout _CurrentLayout;
        private const int PADDING_BORDER = 6;
        private List<Vector> LayoutIntersections;

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

            InitCamera();
            InitMeasureTool();
            _CachedCenter = Vector.Empty;

            LayoutIntersections = new List<Vector>();
            _DisplayConfig = new LayoutViewerDisplayConfig();
            _DisplayConfig.PropertyChanged += DisplayConfigChanged;
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
                RenderFingerboard(pe.Graphics);
                RenderGuideLines(pe.Graphics);
                RenderFrets(pe.Graphics);
                RenderStrings(pe.Graphics);
            }

            pe.Graphics.ResetTransform();

            if (!measurePos1.IsEmpty)
                RenderMeasureTool(pe.Graphics);
        }

        #endregion

        #region Change events

        private void DisplayConfigChanged(object sender, PropertyChangedEventArgs e)
        {
            if (IsHandleCreated)
                Invalidate();
        }

        private void CurrentLayoutUpdated(object sender, EventArgs e)
        {
            if (IsHandleCreated)
            {
                if (!manualZoom)
                    ResetCamera();
                else
                    Invalidate();
                ClearMeasuring();
                CalculateIntersections();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _CachedCenter = new Vector((Width - 1) / 2d, (Height - 1) / 2d);

            base.OnSizeChanged(e);

            if (!manualZoom)
            {
                ZoomToFit();
            }
        }

        #endregion

        #region Camera

        private void InitCamera()
        {
            _Zoom = 1d;
            dragStart = Vector.Empty;
            cameraPosition = Vector.Zero;
        }

        public void ResetCamera()
        {
            cameraPosition = Vector.Zero;
            ZoomToFit();
            if (IsHandleCreated)
                Invalidate();
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

        #endregion

        #region Mouse Handling

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            Select();
            if (e.Button == MouseButtons.Middle)
                dragStart = DisplayToLocal(e.Location);
            //else if (e.Button == MouseButtons.Left)
            //{
            //    var localPos = DisplayToLocal(e.Location);
            //    var worldPos = LocalToWorld(localPos, _Zoom, true);

            //    Console.WriteLine(string.Format("Display pos: {0}", e.Location));
            //    Console.WriteLine(string.Format("Local pos: {0}", localPos));
            //    Console.WriteLine(string.Format("Display calc pos: {0}", LocalToDisplay(localPos)));
            //    Console.WriteLine(string.Format("World calc pos: {0}", worldPos));
            //    Console.WriteLine(string.Format("Local calc pos: {0}", WorldToLocal(worldPos, _Zoom, true)));
            //}
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
            else
            {
                if (!measurePos1.IsEmpty)
                {
                    var pt1 = WorldToDisplay(measurePos1, _Zoom, true);
                    var minX = (int)Math.Min(pt1.X, e.X);
                    var maxX = (int)Math.Max(pt1.X, e.X);
                    var minY = (int)Math.Min(pt1.Y, e.Y);
                    var maxY = (int)Math.Max(pt1.Y, e.Y);
                    //measure2 = DisplayToWorld(e.Location, _Zoom, true);
                    var updateBounds = Rectangle.FromLTRB(minX, minY, maxX, maxY);
                    updateBounds.Inflate(10, 10);
                    Invalidate(updateBounds);
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

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            if(e.Button == MouseButtons.Left)
            {
                if(LayoutIntersections.Count > 0)
                {
                    if (measurePos1.IsEmpty || !measurePos2.IsEmpty)
                    {
                        Vector pointsNear = Vector.Empty;
                        if (GetLayoutPointNear(e.Location, 2, out pointsNear))
                        {
                            measurePos1 = pointsNear;
                            measurePos2 = Vector.Empty;
                            Invalidate();
                        }
                    }
                    else if (measurePos2.IsEmpty)
                    {
                        Vector pointsNear = Vector.Empty;
                        if (GetLayoutPointNear(e.Location, 2, out pointsNear))
                        {
                            measurePos2 = pointsNear;
                            Invalidate();
                        }
                    }
                }

            }
            else if(e.Button == MouseButtons.Right && !measurePos1.IsEmpty)
            {
                ClearMeasuring();
            }
        }

        #endregion

        #region Measuring tool

        private Vector measurePos1;
        private Vector measurePos2;
        private UnitOfMeasure DisplayUnit;

        private void InitMeasureTool()
        {
            measurePos1 = Vector.Empty;
            measurePos2 = Vector.Empty;
            DisplayUnit = UnitOfMeasure.Mm;
        }

        private void ClearMeasuring()
        {
            if (!measurePos1.IsEmpty)
            {
                measurePos1 = Vector.Empty;
                measurePos2 = Vector.Empty;
                if(IsHandleCreated)
                    Invalidate();
            }
        }

        private bool GetLayoutPointNear(Point pt, double range, out Vector vec)
        {
            vec = Vector.Empty;
            var worldPos = DisplayToWorld(pt, _Zoom, true);
            var pointsNear = LayoutIntersections.Where(i => (i - worldPos).Length <= range);
            if (pointsNear.Any())
                vec = pointsNear.OrderBy(p => (p - worldPos).Length).First();

            return !vec.IsEmpty;
        }

        private void CalculateIntersections()
        {
            LayoutIntersections.Clear();
            if (CurrentLayout != null && CurrentLayout.VisualElements.Count > 0)
            {
                var fretPos = CurrentLayout.VisualElements.OfType<FretLine>().SelectMany(fl => fl.Segments.Where(s => !s.IsVirtual)).Select(s => s.PointOnString).Distinct();

                LayoutIntersections.AddRange(fretPos.Select(p => p.ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<FretLine>().Select(fl => fl.Points.First().ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<FretLine>().Select(fl => fl.Points.Last().ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<LayoutLine>().Select(fl => fl.P1.ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<LayoutLine>().Select(fl => fl.P2.ToVector()));
                LayoutIntersections = LayoutIntersections.Distinct().ToList();
            }
        }

        #endregion

        #region UI <-> 2D coordinates

        private bool IsHorizontal { get { return DisplayConfig.FretboardOrientation == Orientation.Horizontal; } }

        private Vector _CachedCenter;
        private Vector ControlCenter
        {
            get
            {
                return _CachedCenter;
            }
        }

        #region Local <-> Display (UI)

        private Vector DisplayToLocal(Point pt)
        {
            var pos = new Vector(pt.X, (Height - 1) - pt.Y);
            return pos - ControlCenter;
        }

        private Vector DisplayToLocal(PointF pt)
        {
            var pos = new Vector(pt.X, (Height - 1) - pt.Y);
            return pos - ControlCenter;
        }

        private PointF LocalToDisplay(Vector vec)
        {
            var final = ControlCenter + vec;
            return new PointF((float)final.X, (Height - 1) -(float)final.Y);
        }

        #endregion

        #region World <-> Local

        private Vector LocalToWorld(Vector vec, double zoom, bool fixOrientation)
        {
            var worldPos = (vec / zoom) + cameraPosition;
            if (fixOrientation && IsHorizontal)
                worldPos = new Vector(worldPos.Y * -1, worldPos.X);
            return worldPos;
        }

        private Vector WorldToLocal(Vector vec, double zoom, bool fixOrientation)
        {
            if (fixOrientation && IsHorizontal)
                vec = new Vector(vec.Y, vec.X * -1d);
            vec -= cameraPosition;
            return vec * zoom;
        }

        #endregion

        #region World <-> Display

        private Vector DisplayToWorld(Point pt, double zoom, bool fixOrientation)
        {
            return LocalToWorld(DisplayToLocal(pt), zoom, fixOrientation);
        }

        private Vector DisplayToWorld(PointF pt, double zoom, bool fixOrientation)
        {
            return LocalToWorld(DisplayToLocal(pt), zoom, fixOrientation);
        }

        private PointF WorldToDisplay(Vector vec, double zoom, bool fixOrientation)
        {
            return LocalToDisplay(WorldToLocal(vec, zoom, fixOrientation));
        }

        #endregion

        private Vector DisplayToWorld(Point pt)
        {
            return DisplayToWorld(pt, _Zoom, false);
            //return DisplayToWorld(pt, _Zoom);
        }

        private Vector DisplayToWorld(Point pt, double zoom)
        {
            return DisplayToWorld(pt, zoom, false);
            //return (DisplayToLocal(pt) / zoom) + cameraPosition;
        }

        #endregion
    }
}
