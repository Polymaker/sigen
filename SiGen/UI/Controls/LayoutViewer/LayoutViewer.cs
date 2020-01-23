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
        #region Fields

        private double _Zoom;
        private ViewerDisplayConfig _DisplayConfig;
        private bool manualZoom;
        private Vector cameraPosition;//independant of orientation
        private LayoutMeasure _CurrentMeasure;
        private bool _EnableMeasureTool;
        private SILayout _CurrentLayout;
        private const int PADDING_BORDER = 6;
        private List<LayoutIntersection> LayoutIntersections;

        #endregion

        #region Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
        public ViewerDisplayConfig DisplayConfig
        {
            get { return _DisplayConfig; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LayoutMeasure CurrentMeasure
        {
            get { return _CurrentMeasure; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), EditorBrowsable(EditorBrowsableState.Advanced)]
        public double Zoom { get { return _Zoom; } }

        [DefaultValue(false)]
        public bool EnableMeasureTool
        {
            get { return _EnableMeasureTool; }
            set
            {
                if(value != _EnableMeasureTool)
                {
                    _EnableMeasureTool = value;
                    if (!DesignMode && !value)
                        ClearMeasuring();
                }
            }
        }

        [Browsable(false)]
        public bool IsMeasuring { get; private set; }

        #endregion

        #region Classes

        private interface IUIElement
        {
            Rectangle DisplayBounds { get; }
        }

        #endregion

        #region Events

        public event EventHandler ZoomChanged;

        #endregion

        public LayoutViewer()
        {
            InitializeComponent();
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.Selectable, true);

            InitializeCamera();
            InitializeMeasureTool();

            _CachedCenter = Vector.Empty;

            MouseDownPos = new Dictionary<MouseButtons, Vector>();
            MouseDownPos[MouseButtons.Left] = Vector.Empty;
            MouseDownPos[MouseButtons.Right] = Vector.Empty;
            MouseDownPos[MouseButtons.Middle] = Vector.Empty;
            
            LayoutIntersections = new List<LayoutIntersection>();

            _DisplayConfig = ViewerDisplayConfig.CreateDefault();
            _DisplayConfig.InitDefaultDesignerValues();
            _DisplayConfig.AttachPropertyChangedEvent();
            _DisplayConfig.PropertyChanged += DisplayConfigChanged;
            
        }

        public void SetDisplayConfig(ViewerDisplayConfig config)
        {
            if (_DisplayConfig != null)
                _DisplayConfig.PropertyChanged -= DisplayConfigChanged;

            _DisplayConfig = config ?? new ViewerDisplayConfig();
            _DisplayConfig.InitDefaultDesignerValues();
            _DisplayConfig.AttachPropertyChangedEvent();
            _DisplayConfig.PropertyChanged += DisplayConfigChanged;
            if (IsHandleCreated)
                Invalidate();
        }

        public void SetDisplayColors(ViewerDisplayConfig config)
        {
            _DisplayConfig.PropertyChanged -= DisplayConfigChanged;

            for (int i = 0; i < config.LineConfigs.Length; i++)
            {
                _DisplayConfig.LineConfigs[i].Color = config.LineConfigs[i].Color;
            }

            _DisplayConfig.PropertyChanged += DisplayConfigChanged;
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

        #region Drawing

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);

            pe.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pe.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            pe.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;

            var center = new PointF(Width / 2f, Height / 2f);
            pe.Graphics.TranslateTransform(center.X, center.Y);
            pe.Graphics.ScaleTransform((float)_Zoom, (float)_Zoom);
            pe.Graphics.TranslateTransform((float)cameraPosition.X * -1, (float)cameraPosition.Y);

            if(CurrentLayout != null)
            {
                RenderFingerboard(pe.Graphics);
                RenderGuideLines(pe.Graphics);

                RenderFrets(pe.Graphics);
                if(DisplayConfig.ShowStrings)
                    RenderStrings(pe.Graphics);
            }

            pe.Graphics.ResetTransform();

            if (EnableMeasureTool)
                RenderMeasureTool(pe.Graphics);
        }

        private void InvalidateWorldRegion(int bleedSize, params Vector[] points)
        {
            Vector minPos = Vector.Empty;
            Vector maxPos = Vector.Empty;

            for (int i = 0; i < points.Length; i++)
            {
                if (!points[i].IsEmpty)
                {
                    if (minPos.IsEmpty)
                    {
                        minPos = points[i];
                        maxPos = points[i];
                    }
                    else
                    {
                        minPos = Vector.Min(minPos, points[i]);
                        maxPos = Vector.Max(maxPos, points[i]);
                    }
                }
            }

            if(!minPos.IsEmpty)
            {
                var minPt = WorldToDisplay(minPos, _Zoom, true);
                var maxPt = WorldToDisplay(maxPos, _Zoom, true);
                var updateBounds = Rectangle.FromLTRB((int)minPt.X - bleedSize, (int)minPt.Y - bleedSize, (int)maxPt.X + bleedSize, (int)maxPt.Y + bleedSize);
                Invalidate(updateBounds);
            }
        }

        #endregion

        #region Change events

        private void DisplayConfigChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(DisplayConfig.FretExtensionAmount):
                    RecalculateFretExtensions();
                    break;
            }
            if (IsHandleCreated)
                Invalidate();
        }

        private void CurrentLayoutUpdated(object sender, EventArgs e)
        {
            if (IsHandleCreated)
            {
                if (!DisplayConfig.FretExtensionAmount.IsEmpty)
                    RecalculateFretExtensions();

                if (!manualZoom)
                    ResetCamera();
                else
                    Invalidate();
                
                CalculateIntersections();

                AdjustMeasureAfterLayoutChanged();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            _CachedCenter = new Vector((Width - 1) / 2d, (Height - 1) / 2d);
            InvalidateMeasureBoxes();

            base.OnSizeChanged(e);

            if (!manualZoom)
                ZoomToFit();
        }

        #endregion

        #region Misc

        private void RecalculateFretExtensions()
        {
            if (CurrentLayout != null)
            {
                foreach (var fretLine in CurrentLayout.VisualElements.OfType<FretLine>())
                {
                    if (DisplayConfig.FretExtensionAmount.IsEmpty)
                        fretLine.Tag = null;
                    else
                        fretLine.Tag = fretLine.GetExtendedFretLine(DisplayConfig.FretExtensionAmount);
                }
            }
            
        }

        #endregion

        #region Camera

        private bool IsMovingCamera;

        private void InitializeCamera()
        {
            _Zoom = 1d;
            lastMousePos = Vector.Empty;
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
                var layoutBounds = CurrentLayout.GetLayoutBounds();
                if (!layoutBounds.IsEmpty)
                {
                    if (DisplayConfig.FingerboardOrientation == Orientation.Horizontal)
                    {
                        _Zoom = (Width - (PADDING_BORDER * 2f)) / (float)layoutBounds.Height.NormalizedValue;
                    }
                    else
                    {
                        _Zoom = (Height - (PADDING_BORDER * 2f)) / (float)layoutBounds.Height.NormalizedValue;
                    }
                    var centerY = layoutBounds.Center.Y.NormalizedValue;
                    cameraPosition = Vector.Zero;
                    if (DisplayConfig.FingerboardOrientation == Orientation.Vertical)
                        cameraPosition.Y += centerY;
                    else
                        cameraPosition.X += centerY;
                }
            }
            manualZoom = false;
            OnCameraChanged(false);
            OnZoomChanged();
        }

        private void PerformMouseWheelZoom(MouseEventArgs e)
        {
            var oldZoom = _Zoom;
            var mousePos = e.Location;
            manualZoom = true;
            if (e.Delta > 0)
                _Zoom *= 1.1;
            else
                _Zoom *= 0.90909090909090909090909090909091;

            _Zoom = Math.Max(8, Math.Min(200, _Zoom));

            if (oldZoom != _Zoom)
            {
                var curWorldPos = DisplayToWorldFast(mousePos, oldZoom);
                var finalWorldPos = DisplayToWorldFast(mousePos);
                cameraPosition -= (finalWorldPos - curWorldPos);
                OnCameraChanged(true);
                OnZoomChanged();
            }
        }

        private void OnCameraChanged(bool invalidate)
        {
            InvalidateMeasureBoxes();

            if (invalidate && IsHandleCreated)
                Invalidate();
        }

        private void OnZoomChanged()
        {
            var handler = ZoomChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
        }

        #endregion

        #region Mouse Handling

        private enum DragEntity
        {
            None,
            Camera,
            Measure,
            Measure2,
            Measure3
        }

        private enum DragInputMode
        {
            None,
            LeftButton,
            MiddleButton,
            Spacebar
        }
        
        private HitTestInfo DragTarget;
        private Dictionary<MouseButtons, Vector> MouseDownPos;

        private bool CanDrag { get { return DragTarget != null; } }
        private bool IsDragging;
        private DragInputMode DragInput;
        private Vector lastMousePos;
        //private Vector dragStart;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Select();
            Focus();

            MouseDownPos[e.Button] = DisplayToLocal(e.Location);

            if (!IsDragging)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    InitDragTarget(new HitTestInfo(), 
                        DragInputMode.MiddleButton, 
                        DisplayToLocal(e.Location), 
                        true);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    var hitInfo = HitTest(e.Location);
                    if (hitInfo.Type == LayoutViewerHitTestType.MeasureBox)
                    {
                        InitDragTarget(hitInfo,
                            DragInputMode.LeftButton,
                            DisplayToLocal(e.Location),
                            false);
                    }
                }
            }
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (CanDrag || IsDragging)
            {
                if ((e.Button == MouseButtons.Left && DragInput == DragInputMode.LeftButton) ||
                    (e.Button == MouseButtons.Middle && DragInput == DragInputMode.MiddleButton))
                {
                    ClearDrag();
                }
            }

            MouseDownPos[e.Button] = Vector.Empty;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (EnableMeasureTool && MeasureLastSelection.IsEmpty && !IsDragging)
                UpdateMeasureSnapPosition(e.Location);

            if (CanDrag || IsDragging)
            {
                var curPos = DisplayToLocal(e.Location);
                var moveVector = curPos - lastMousePos;

                if (moveVector.Length > 1)//1 in local unit = 1 pixel
                {
                    if (!IsDragging)
                        OnDragStart();

                    PerformDragMove(moveVector);

                    lastMousePos = curPos;
                }
            }
            else
            {
                if (!MeasureFirstSelection.IsEmpty && MeasureLastSelection.IsEmpty)
                {
                    var pt1 = WorldToDisplay(MeasureFirstSelection, _Zoom, true);
                    var minX = (int)Math.Min(pt1.X, e.X);
                    var maxX = (int)Math.Max(pt1.X, e.X);
                    var minY = (int)Math.Min(pt1.Y, e.Y);
                    var maxY = (int)Math.Max(pt1.Y, e.Y);

                    var updateBounds = Rectangle.FromLTRB(minX, minY, maxX, maxY);
                    updateBounds.Inflate(20, 20);
                    Invalidate(updateBounds);
                }
            }
        }

        private void InitDragTarget(HitTestInfo target, DragInputMode inputMode, Vector mousePos, bool setCursor)
        {
            DragTarget = target;
            DragInput = inputMode;
            lastMousePos = mousePos;

            if (setCursor)
                Cursor = new Cursor(Properties.Resources.open_hand_icon.Handle);
        }

        private void OnDragStart()
        {
            Cursor = new Cursor(Properties.Resources.closed_hand_icon.Handle);
            //dragStart = lastMousePos;
            IsDragging = true;
        }

        private void PerformDragMove(Vector dragVector)
        {
            if (DragTarget.Type == LayoutViewerHitTestType.None)
            {
                IsMovingCamera = true;
                manualZoom = true;
                dragVector *= -1;
                cameraPosition += dragVector / _Zoom;
                OnCameraChanged(true);
            }
            else if (DragTarget.Type == LayoutViewerHitTestType.MeasureBox)
            {
                var selectedBox = MeasureBoxes[DragTarget.MeasureBoxIndex];
                selectedBox.LocalOffset += dragVector;
                selectedBox.NotifyDirty();
                Invalidate();
            }
        }

        private void ClearDrag()
        {
            if (CanDrag || IsDragging)
            {
                DragInput = DragInputMode.None;
                lastMousePos = Vector.Empty;
                DragTarget = null;
                IsDragging = false;
                IsMovingCamera = false;
                Cursor = Cursors.Default;
                Invalidate();
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            PerformMouseWheelZoom(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            var mouseUpPos = DisplayToLocal(e.Location);

            var cancelClick = !MouseDownPos[e.Button].IsEmpty && (mouseUpPos - MouseDownPos[e.Button]).Length > 8;

            if (!cancelClick)
                HandleMouseClick(e);

            base.OnMouseClick(e);
        }

        private void HandleMouseClick(MouseEventArgs e)
        {
            var hitInfo = HitTest(e.Location);

            if (hitInfo.Type == LayoutViewerHitTestType.MeasureBox)
            {
                var clickedBox = MeasureBoxes[hitInfo.MeasureBoxIndex];

                if (e.Button == MouseButtons.Left)
                {

                }
                else if (e.Button == MouseButtons.Right)
                {
                    menuMeasureBox.Tag = clickedBox;
                    menuMeasureBox.Show(PointToScreen(e.Location));
                }
            }
            else if (hitInfo.Type == LayoutViewerHitTestType.None)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (EnableMeasureTool)
                        CaptureMeasure(e.Location);
                }
                else if (e.Button == MouseButtons.Right && !MeasureFirstSelection.IsEmpty)
                {
                    if (MeasureLastSelection.IsEmpty)
                        ClearMeasuring();
                    else
                    {
                        menuMeasureBox.Tag = null;
                        menuMeasureBox.Show(PointToScreen(e.Location));
                    }
                }
            }
        }

        protected override void OnMouseDoubleClick(MouseEventArgs e)
        {
            base.OnMouseDoubleClick(e);

            if (e.Button == MouseButtons.Middle)
                ResetCamera();
            else
            {
                var hitInfo = HitTest(e.Location);
                if (hitInfo.Type == LayoutViewerHitTestType.MeasureBox)
                {
                    var clickedBox = MeasureBoxes[hitInfo.MeasureBoxIndex];
                    ShowMeasureTextbox(clickedBox);
                }
            }
        }

        public enum LayoutViewerHitTestType
        {
            None,
            MeasureBox,
            Button
        }

        public class HitTestInfo
        {
            internal LayoutViewerHitTestType _Type;
            internal int _MeasureBoxIndex;
            internal int _ButtonID;
            public LayoutViewerHitTestType Type { get { return _Type; } }
            public int MeasureBoxIndex { get { return _MeasureBoxIndex; } }
            public int ButtonID { get { return _ButtonID; } }

            internal HitTestInfo()
            {
                _Type = LayoutViewerHitTestType.None;
                _MeasureBoxIndex = -1;
                _ButtonID = -1;
            }

            internal HitTestInfo(LayoutViewerHitTestType type)
            {
                _Type = type;
                _MeasureBoxIndex = -1;
                _ButtonID = -1;
            }
        }

        public HitTestInfo HitTest(Point pt)
        {
            var hitInfo = new HitTestInfo();

            for (int i = 0; i < MeasureBoxes.Count; i++)
            {
                if (MeasureBoxes[i].IsMeasureVisible() && MeasureBoxes[i].DisplayBounds.Contains(pt))
                {
                    hitInfo._Type = LayoutViewerHitTestType.MeasureBox;
                    hitInfo._MeasureBoxIndex = i;
                    return hitInfo;
                }
            }
            return hitInfo;
        }

        #endregion

        #region Keyboard Handling

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (EnableMeasureTool && MeasureLastSelection.IsEmpty && IsAltPressed(e.KeyCode))
                UpdateMeasureSnapPosition();

            if (!IsDragging && e.KeyCode == Keys.Space)
            {
                InitDragTarget(new HitTestInfo(),
                            DragInputMode.Spacebar,
                            DisplayToLocal(PointToClient(MousePosition)),
                            true);
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            if (EnableMeasureTool && MeasureLastSelection.IsEmpty && IsAltPressed(e.KeyCode))
                UpdateMeasureSnapPosition();

            if (e.KeyCode == Keys.Space &&
                (CanDrag || IsDragging) &&
                DragInput == DragInputMode.Spacebar)
            {
                ClearDrag();
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData.HasFlag(Keys.Alt) || keyData.HasFlag(Keys.Menu))
                return true;
            return base.ProcessDialogKey(keyData);
        }

        protected bool IsAltPressed()
        {
            return IsAltPressed(ModifierKeys);
        }

        protected bool IsAltPressed(Keys keyCode)
        {
            return keyCode.HasFlag(Keys.Alt) || keyCode.HasFlag(Keys.Menu);
        }

        protected bool IsControlPressed()
        {
            return ModifierKeys.HasFlag(Keys.Control) || ModifierKeys.HasFlag(Keys.ControlKey);
        }

        #endregion

        #region UI <-> 2D coordinates

        public enum CoordSpace
        {
            World,
            Local,
            Display
        }

        private bool IsHorizontal { get { return DisplayConfig.FingerboardOrientation == Orientation.Horizontal; } }

        private bool IsFlipHorizontal
        {
            get
            {
                return CurrentLayout != null && DisplayConfig.FingerboardOrientation == Orientation.Horizontal && CurrentLayout.LeftHanded;
            }
        }

        private Vector _CachedCenter;
        private Vector ControlCenter
        {
            get { return _CachedCenter; }
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
            if (fixOrientation)
            {
                if(IsFlipHorizontal)
                    worldPos = new Vector(worldPos.Y, worldPos.X * -1d);
                else if(IsHorizontal)
                    worldPos = new Vector(worldPos.Y * -1d, worldPos.X);
            }
            return worldPos;
        }

        private Vector WorldToLocal(Vector vec, double zoom, bool fixOrientation)
        {
            if (fixOrientation)
            {
                if (IsFlipHorizontal)
                    vec = new Vector(vec.Y * -1d, vec.X);
                else if (IsHorizontal)
                    vec = new Vector(vec.Y, vec.X * -1d);
            }
            
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

        public Vector DisplayToWorld(Point pt)
        {
            return DisplayToWorld(pt, _Zoom, true);
        }

        #endregion

        private Vector DisplayToWorldFast(Point pt)
        {
            return DisplayToWorld(pt, _Zoom, false);
            //return DisplayToWorld(pt, _Zoom);
        }

        private Vector DisplayToWorldFast(Point pt, double zoom)
        {
            return DisplayToWorld(pt, zoom, false);
            //return (DisplayToLocal(pt) / zoom) + cameraPosition;
        }

        #endregion
    }
}
