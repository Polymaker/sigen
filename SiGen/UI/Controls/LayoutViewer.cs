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
        private LayoutViewerDisplayConfig _DisplayConfig;
        private bool manualZoom;
        private Vector cameraPosition;//independant of orientation
        private LayoutMeasure _CurrentMeasure;
        
        private SILayout _CurrentLayout;
        private const int PADDING_BORDER = 6;
        private List<Vector> LayoutIntersections;
        //private List<IUIElement> _UIElements;

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
        public LayoutViewerDisplayConfig DisplayConfig
        {
            get { return _DisplayConfig; }
        }

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public LayoutMeasure CurrentMeasure
        {
            get { return _CurrentMeasure; }
        }

        #endregion

        #region Classes

        private interface IUIElement
        {
            Rectangle DisplayBounds { get; }
        }

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

            if (!MeasureFirstSelection.IsEmpty)
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
            InvalidateMeasureBoxes();

            base.OnSizeChanged(e);

            if (!manualZoom)
            {
                ZoomToFit();
            }
        }

        #endregion

        #region Camera

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
            OnCameraChanged(false);
        }

        private void OnCameraChanged(bool invalidate)
        {
            InvalidateMeasureBoxes();

            if (invalidate && IsHandleCreated)
                Invalidate();
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

        private DragEntity DragTarget;
        private object DraggedObject;
        private Dictionary<MouseButtons, Vector> MouseDownPos;

        private bool CanDrag { get { return DragTarget != DragEntity.None; } }
        private bool IsDragging;

        private Vector lastMousePos;
        private Vector dragStart;

        protected override void OnMouseDown(MouseEventArgs e)
        {
            Select();

            MouseDownPos[e.Button] = DisplayToLocal(e.Location);
            if (!IsDragging)
            {
                if (e.Button == MouseButtons.Middle)
                {
                    DragTarget = DragEntity.Camera;
                    lastMousePos = DisplayToLocal(e.Location);
                }
                else if (e.Button == MouseButtons.Left)
                {
                    var hitInfo = HitTest(e.Location);
                    if (hitInfo.Type == LayoutViewerHitTestType.MeasureBox)
                    {
                        DragTarget = DragEntity.Measure;
                        DraggedObject = MeasureBoxes[hitInfo.MeasureBoxIndex];
                        lastMousePos = DisplayToLocal(e.Location);
                    }
                }
            }
            
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            ClearDrag();

            MouseDownPos[e.Button] = Vector.Empty;
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (CanDrag || IsDragging)
            {
                var curPos = DisplayToLocal(e.Location);
                var moveVector = curPos - lastMousePos;
                if (moveVector.Length > 1)//1 in local unit = 1 pixel
                {
                    if (!IsDragging)
                    {
                        dragStart = lastMousePos;
                        IsDragging = true;
                    }

                    PerformDragMove(moveVector);

                    lastMousePos = curPos;
                }
            }
            else
            {
                if (!MeasureFirstSelection.IsEmpty)
                {
                    var pt1 = WorldToDisplay(MeasureFirstSelection, _Zoom, true);
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

        private void PerformDragMove(Vector dragVector)
        {
            if (DragTarget == DragEntity.Camera)
            {
                manualZoom = true;
                dragVector *= -1;
                cameraPosition += dragVector / _Zoom;
                OnCameraChanged(true);
            }
            else if (DragTarget == DragEntity.Measure)
            {
                var selectedBox = (MeasureValueBox)DraggedObject;
                selectedBox.LocalOffset += dragVector;
                selectedBox.NotifyDirty();
                Invalidate();
            }
        }

        private void ClearDrag()
        {
            if (CanDrag || IsDragging)
            {
                lastMousePos = Vector.Empty;
                DragTarget = DragEntity.None;
                DraggedObject = null;
                IsDragging = false;
            }
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
                OnCameraChanged(true);
            }
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
                    if (IsMeasuring)
                        CaptureMeasure(e.Location);
                    
                }
                else if (e.Button == MouseButtons.Right && !MeasureFirstSelection.IsEmpty)
                {
                    ClearMeasuring();
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

        #region Measuring tool

        public enum MeasureType
        {
            Length,
            Angle
        }

        public enum LengthType
        {
            Length,
            Width,
            Height
        }

        public class LayoutMeasure
        {
            private PointM _First;
            private PointM _Last;
            private Measure _Length;
            private Measure _Width;
            private Measure _Height;
            private Angle _Direction;

            public Measure this[LengthType type]
            {
                get
                {
                    switch (type)
                    {
                        default:
                        case LengthType.Length:
                            return _Length;
                        case LengthType.Height:
                            return _Height;
                        case LengthType.Width:
                            return _Width;
                    }
                }
            }

            public PointM FromPoint { get { return _First; } }
            public PointM ToPoint { get { return _Last; } }
            public Measure Length { get { return _Length; } }
            public Measure Width { get { return _Width; } }
            public Measure Height { get { return _Height; } }
            public Angle Direction { get { return _Direction; } }

            public LayoutMeasure(PointM p1, PointM p2)
            {
                _First = p1;
                _Last = p2;
                _Length = PointM.Distance(p1, p2);
                _Width = Measure.Abs(p2.X - p1.X);
                _Height = Measure.Abs(p2.Y - p1.Y);
                _Direction = Angle.FromPoints(p1.ToVector(), p2.ToVector());
            }
        }

        private abstract class MeasureValueBox : IUIElement
        {
            private bool isDirty;
            private LayoutMeasure _Measure;
            private Rectangle _DisplayBounds;
            internal LayoutViewer Viewer;
            protected LayoutMeasure SourceMeasure { get { return _Measure; } }

            public SizeF Size { get; set; }
            public Vector TargetPosition { get; set; }
            public Vector OriginalOffset { get; set; }
            public Vector LocalOffset { get; set; }
            
            public bool Suppressed { get; set; }
            public bool Visible { get; set; }
            public bool ShowExactValue { get; set; }
            public bool IsDirty { get { return isDirty; } }

            public Rectangle DisplayBounds
            {
                get
                {
                    if (isDirty)
                        UpdateDisplayBounds();
                    return _DisplayBounds;
                }
            }

            public MeasureValueBox()
            {
                Visible = true;
                isDirty = true;
            }

            internal MeasureValueBox(LayoutMeasure measure)
            {
                _Measure = measure;
                Visible = true;
                isDirty = true;
                LocalOffset = Vector.Zero;
                OriginalOffset = Vector.Zero;
            }

            public virtual string GetDisplayValue()
            {
                return string.Empty;
            }

            public virtual string GetEditValue()
            {
                return string.Empty;
            }

            protected virtual void CalculateDisplayBounds()
            {
                var boxBounds = Viewer.GetMeasureBoxBounds(this);
                _DisplayBounds = new Rectangle((int)boxBounds.X, (int)boxBounds.Y, (int)boxBounds.Width, (int)boxBounds.Height);
            }

            internal void UpdateDisplayBounds()
            {
                if (isDirty)
                {
                    CalculateDisplayBounds();
                    isDirty = false;
                }
            }

            public void NotifyDirty()
            {
                isDirty = true;
            }

            public virtual bool IsMeasureVisible()
            {
                return Visible;
            }

            public virtual void UpdateSize()
            {
                SizeF textSize = SizeF.Empty;
                using (var g = Viewer.CreateGraphics())
                    textSize = g.MeasureString(GetDisplayValue(), Viewer.Font);
                Size = new SizeF(textSize.Width + 2, textSize.Height + 2);
            }
        }

        private class LengthValueBox : MeasureValueBox
        {
            private LengthType _Type;
            public LengthType Type { get { return _Type; } }
            public Measure Value { get { return SourceMeasure[Type]; } }
            public UnitOfMeasure DisplayUnit { get; set; }

            public Vector P1 { get; set; }
            public Vector P2 { get; set; }

            internal LengthValueBox(LayoutMeasure measure, LengthType type, Vector p1, Vector p2) : base(measure)
            {
                _Type = type;
                P1 = p1;
                P2 = p2;
                TargetPosition = (p1 + p2) / 2;
            }

            public override string GetDisplayValue()
            {
                var format = new Measure.MeasureFormat() { OverrideUnit = DisplayUnit };
                if (ShowExactValue)
                {
                    format.ShowFractions = false;
                    format.MaximumDecimals = 0;
                }
                return Value.ToString(format);
            }

            public override string GetEditValue()
            {
                var format = new Measure.MeasureFormat()
                {
                    OverrideUnit = DisplayUnit,
                    ShowUnitOfMeasure = false,
                    ShowFractions = false
                };
                if (ShowExactValue)
                    format.MaximumDecimals = 0;
                return Value.ToString(format);
            }

            public override bool IsMeasureVisible()
            {
                return base.IsMeasureVisible() && (Value.NormalizedValue * Viewer._Zoom > 3);
            }
        }

        private class AngleValueBox : MeasureValueBox
        {
            public Angle Value { get; set; }
            public AngleUnit DisplayUnit { get; set; }

            internal AngleValueBox(LayoutMeasure measure, Angle value) : base(measure)
            {
                Value = value;
                DisplayUnit = AngleUnit.Degrees;
            }

            public override string GetDisplayValue()
            {
                return Value.ToString(DisplayUnit);
            }

            public override string GetEditValue()
            {
                return Value.ToString(DisplayUnit);
            }
        }

        private bool IsMeasuring;
        private Vector MeasureFirstSelection;
        private Vector MeasureLastSelection;
        private List<MeasureValueBox> MeasureBoxes;

        private void InitializeMeasureTool()
        {
            MeasureBoxes = new List<MeasureValueBox>();
            MeasureFirstSelection = Vector.Empty;
            MeasureLastSelection = Vector.Empty;
            _CurrentMeasure = null;
            IsMeasuring = true;
            InitializeMeasureContextMenu();
        }

        #region MeaureBox Context Menu

        private void InitializeMeasureContextMenu()
        {
            menuMeasureBox.Opening += MenuMeasureBox_Opening;
            menuItemDisplayMeasureMM.Tag = UnitOfMeasure.Mm;
            menuItemDisplayMeasureMM.Click += MenuItemDisplayMeasure_Click;

            menuItemDisplayMeasureCM.Tag = UnitOfMeasure.Cm;
            menuItemDisplayMeasureCM.Click += MenuItemDisplayMeasure_Click;

            menuItemDisplayMeasureIN.Tag = UnitOfMeasure.In;
            menuItemDisplayMeasureIN.Click += MenuItemDisplayMeasure_Click;

            menuItemDisplayMeasureFT.Tag = UnitOfMeasure.Ft;
            menuItemDisplayMeasureFT.Click += MenuItemDisplayMeasure_Click;

            menuItemDisplayMeasureShowDecimals.Click += MenuItemDisplayMeasureShowDecimals_Click;
        }

        private void MenuMeasureBox_Opening(object sender, CancelEventArgs e)
        {
            var targetBox = (MeasureValueBox)menuMeasureBox.Tag;
            if (targetBox is LengthValueBox)
            {
                var lengthBox = (LengthValueBox)targetBox;
                menuItemDisplayMeasureMM.Checked = lengthBox.DisplayUnit == UnitOfMeasure.Mm;
                menuItemDisplayMeasureCM.Checked = lengthBox.DisplayUnit == UnitOfMeasure.Cm;
                menuItemDisplayMeasureIN.Checked = lengthBox.DisplayUnit == UnitOfMeasure.In;
                menuItemDisplayMeasureFT.Checked = lengthBox.DisplayUnit == UnitOfMeasure.Ft;
                menuItemDisplayMeasureShowDecimals.Checked = lengthBox.ShowExactValue;
            }
            else
                e.Cancel = true;
            
        }

        private void MenuItemDisplayMeasureShowDecimals_Click(object sender, EventArgs e)
        {
            var targetBox = (LengthValueBox)menuMeasureBox.Tag;
            targetBox.ShowExactValue = !targetBox.ShowExactValue;
            UpdateMeasureBoxBounds(targetBox);
        }

        private void MenuItemDisplayMeasure_Click(object sender, EventArgs e)
        {
            var targetBox = (LengthValueBox)menuMeasureBox.Tag;
            var newUnit = (UnitOfMeasure)(sender as ToolStripMenuItem).Tag;
            if (newUnit != targetBox.DisplayUnit)
            {
                targetBox.DisplayUnit = newUnit;
                targetBox.ShowExactValue = false;
                UpdateMeasureBoxBounds(targetBox);
            }
        }

        #endregion

        private void ClearMeasuring()
        {
            if (_CurrentMeasure != null || !MeasureFirstSelection.IsEmpty || MeasureBoxes.Count > 0)
            {
                _CurrentMeasure = null;
                MeasureBoxes.Clear();
                MeasureFirstSelection = Vector.Empty;
                MeasureLastSelection = Vector.Empty;
                if (IsHandleCreated)
                    Invalidate();
            }
        }

        private void CompleteMeasure()
        {
            _CurrentMeasure = new LayoutMeasure(
                PointM.FromVector(MeasureFirstSelection, DisplayConfig.DefaultDisplayUnit), 
                PointM.FromVector(MeasureLastSelection, DisplayConfig.DefaultDisplayUnit));

            MeasureBoxes.Add(CreateLengthMeasureBox(_CurrentMeasure, LengthType.Length));
            MeasureBoxes.Add(CreateLengthMeasureBox(_CurrentMeasure, LengthType.Width));
            MeasureBoxes.Add(CreateLengthMeasureBox(_CurrentMeasure, LengthType.Height));
            //MeasureBoxes.Add(CreateAngleMeasureBox(_CurrentMeasure));
            CreateAngleMeasureBox2(_CurrentMeasure);
            MeasureBoxes.RemoveAll(m => m == null);
            
            Invalidate();
        }

        private void CaptureMeasure(Point position)
        {
            if (MeasureFirstSelection.IsEmpty || !MeasureLastSelection.IsEmpty)
            {
                if (!MeasureLastSelection.IsEmpty)
                    ClearMeasuring();
                var pos = GrabMeasureLocation(position);
                if (!pos.IsEmpty)
                {
                    MeasureFirstSelection = pos;
                    Invalidate();
                }
            }
            else if (MeasureLastSelection.IsEmpty)
            {
                var pos = GrabMeasureLocation(position);
                if (!pos.IsEmpty)
                {
                    MeasureLastSelection = pos;
                    if (Vector.EqualOrClose(MeasureFirstSelection, MeasureLastSelection))
                        ClearMeasuring();
                    else
                        CompleteMeasure();
                }
            }
        }

        private Vector GrabMeasureLocation(Point position, double snapRange = 2)
        {
            if ((ModifierKeys & Keys.Control) == Keys.Control)
                return DisplayToWorld(position, _Zoom, true);

            Vector measureLocation = Vector.Empty;
            if (LayoutIntersections.Count > 0)
                GetNearestLayoutPoint(position, snapRange, out measureLocation);
            return measureLocation;
        }

        private LengthValueBox CreateLengthMeasureBox(LayoutMeasure selection, LengthType type)
        {
            LengthValueBox measureBox = null;

            var corner = new PointM(selection.FromPoint.X, selection.ToPoint.Y);
            var center = PointM.Average(selection.FromPoint, selection.ToPoint, corner);
            var centerV = center.ToVector();
            var p1v = selection.FromPoint.ToVector();
            var p2v = selection.ToPoint.ToVector();

            switch (type)
            {
                case LengthType.Length:
                    measureBox = new LengthValueBox(selection, type, p1v, p2v);
                    break;
                case LengthType.Height:
                    measureBox = new LengthValueBox(selection, type, p1v, corner.ToVector());
                    break;
                case LengthType.Width:
                    measureBox = new LengthValueBox(selection, type, corner.ToVector(), p2v);
                    break;
            }

            measureBox.DisplayUnit = DisplayConfig.DefaultDisplayUnit;
            measureBox.Viewer = this;

            if (type != LengthType.Length && Vector.EqualOrClose(centerV, measureBox.TargetPosition, 0.01))
                return null;

            if(!measureBox.Suppressed)
            {
                measureBox.UpdateSize();

                var measureLine = Line.FromPoints(measureBox.P1, measureBox.P2);
                var perp = measureLine.GetPerpendicular(measureBox.TargetPosition);
                Vector perpCenter = perp.GetClosestPointOnLine(centerV);
                if ((measureBox.TargetPosition - perpCenter).Length < 0.001)
                    perpCenter = measureBox.TargetPosition + perp.Vector * 2;

                var boxOffsetDir = WorldToLocal(measureBox.TargetPosition, _Zoom, true) - WorldToLocal(perpCenter, _Zoom, true);

                measureBox.OriginalOffset = measureBox.LocalOffset = GetMeasureBoxOffset(boxOffsetDir, measureBox.Size);
                if (measureBox.OriginalOffset == Vector.Zero)
                    measureBox.Suppressed = true;
                measureBox.UpdateDisplayBounds();
            }
            
            return measureBox;
        }

        private AngleValueBox CreateAngleMeasureBox(LayoutMeasure selection)
        {
            var corner = new PointM(selection.FromPoint.X, selection.ToPoint.Y).ToVector();
            var p1v = selection.FromPoint.ToVector();
            var p2v = selection.ToPoint.ToVector();
            var center = (p2v + corner) / 2;
            var angleBox = new AngleValueBox(selection, Angle.FromPoints(p1v, p2v, corner)) { Viewer = this };
            angleBox.TargetPosition = p1v;
            SizeF textSize = SizeF.Empty;
            using (var g = CreateGraphics())
                textSize = g.MeasureString(angleBox.GetDisplayValue(), Font);
            angleBox.Size = new SizeF(textSize.Width + 2, textSize.Height + 2);
            return angleBox;
        }

        private void CreateAngleMeasureBox2(LayoutMeasure selection)
        {
            var corner = new PointM(selection.FromPoint.X, selection.ToPoint.Y).ToVector();
            var p1v = selection.FromPoint.ToVector();
            var p2v = selection.ToPoint.ToVector();
            var center = (p2v + corner) / 2;

            var angle1Box = new AngleValueBox(selection, Angle.FromPoints(p1v, p2v, corner)) { Viewer = this };
            var angle2Box = new AngleValueBox(selection, Angle.FromPoints(p2v, p1v, corner)) { Viewer = this };
            angle1Box.TargetPosition = p1v;
            angle2Box.TargetPosition = p2v;

            angle1Box.UpdateSize();
            angle2Box.UpdateSize();
            MeasureBoxes.Add(angle1Box);
            MeasureBoxes.Add(angle2Box);
        }

        private void UpdateMeasureBoxBounds(MeasureValueBox box)
        {
            if (!box.Suppressed)
            {
                var oldSize = box.Size;
                box.UpdateSize();

                var wasMoved = box.OriginalOffset != box.LocalOffset;

                var centerUI = WorldToLocal(box.TargetPosition, _Zoom, true);
                var targetUI = centerUI + box.OriginalOffset;
                var boxOffsetDir = targetUI - centerUI;

                box.OriginalOffset = GetMeasureBoxOffset(boxOffsetDir, box.Size);

                if (!wasMoved)
                    box.LocalOffset = box.OriginalOffset;

                box.NotifyDirty();

                var boxBounds = GetMeasureBoxBounds(box);
                if (oldSize.Width > box.Size.Width)
                    boxBounds.Inflate((oldSize.Width - box.Size.Width) / 2f, 0);
                boxBounds.Inflate((float)box.LocalOffset.Length + 10f, (float)box.LocalOffset.Length + 10f);
                if(box.IsMeasureVisible())
                    Invalidate(new Rectangle((int)boxBounds.X, (int)boxBounds.Y, (int)boxBounds.Width, (int)boxBounds.Height));
            }
        }

        private Vector GetMeasureBoxOffset(Vector dir, SizeF boxSize)
        {
            var pointOnRec = GetRectIntersect(dir, boxSize);
            if (!pointOnRec.IsEmpty)
                return (dir.Normalized * (pointOnRec.Length + (FontHeight / 2)));
            return Vector.Zero;
        }

        private Vector GetRectIntersect(Vector dir, SizeF boxSize)
        {
            var inters = new List<Vector>();
            var line = Line.FromPoints(Vector.Zero, dir);
            if (line.IsHorizontal)
            {
                inters.Add(new Vector(boxSize.Width * -0.5, 0));
                inters.Add(new Vector(boxSize.Width * 0.5, 0));
            }
            else if (line.IsVertical)
            {
                inters.Add(new Vector(0, boxSize.Height * -0.5));
                inters.Add(new Vector(0, boxSize.Height * 0.5));
            }
            else
            {
                Vector inter = Vector.Empty;
                var tl = new Vector(boxSize.Width * -.5, boxSize.Height * -.5);
                var tr = new Vector(boxSize.Width * .5, boxSize.Height * -.5);
                var bl = new Vector(boxSize.Width * -.5, boxSize.Height * .5);
                var br = new Vector(boxSize.Width * .5, boxSize.Height * .5);
                if (line.Intersect(Line.FromPoints(tl, tr), out inter))
                    inters.Add(inter);
                if (line.Intersect(Line.FromPoints(tl, bl), out inter))
                    inters.Add(inter);
                if (line.Intersect(Line.FromPoints(bl, br), out inter))
                    inters.Add(inter);
                if (line.Intersect(Line.FromPoints(tr, br), out inter))
                    inters.Add(inter);
            }

            inters = inters.Where(x => Vector.EqualOrClose((Vector.Zero - x).Normalized, dir.Normalized, 0.001)).ToList();
            if (inters.Count > 0)
                return inters.OrderBy(i => i.Length).First();

            return Vector.Empty;
        }

        private void InvalidateMeasureBoxes()
        {
            MeasureBoxes.ForEach(b =>
            {
                if (b.IsMeasureVisible())
                    b.NotifyDirty();
            });
        }

        private bool GetNearestLayoutPoint(Point pt, double range, out Vector vec)
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
                var fretLines = CurrentLayout.VisualElements.OfType<FretLine>();
                var stringLines = CurrentLayout.VisualElements.OfType<StringLine>();
                var stringCenters = CurrentLayout.VisualElements.OfType<StringCenter>();
                var fingerboardEdges = CurrentLayout.VisualElements.OfType<FingerboardEdge>();

                var fretPos = fretLines.SelectMany(fl => fl.Segments.Where(s => !s.IsVirtual)).Select(s => s.PointOnString).Distinct();
                LayoutIntersections.AddRange(fretPos.Select(p => p.ToVector()));

                LayoutIntersections.AddRange(stringCenters.SelectMany(s => fretLines.Select(f => f.GetIntersection(s).ToVector())));
                LayoutIntersections.RemoveAll(p => p.IsEmpty);

                LayoutIntersections.AddRange(stringLines.SelectMany(s => fingerboardEdges.Where(f=> !(f is FingerboardSideEdge)).Select(f => f.GetIntersection(s).ToVector())));
                LayoutIntersections.RemoveAll(p => p.IsEmpty);

                LayoutIntersections.AddRange(stringCenters.SelectMany(s => fingerboardEdges.Where(f => !(f is FingerboardSideEdge)).Select(f => f.GetIntersection(s).ToVector())));
                LayoutIntersections.RemoveAll(p => p.IsEmpty);

                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<FretLine>().Select(fl => fl.Points.First().ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<FretLine>().Select(fl => fl.Points.Last().ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<LayoutLine>().Select(fl => fl.P1.ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<LayoutLine>().Select(fl => fl.P2.ToVector()));

                LayoutIntersections.RemoveAll(p => p.IsEmpty);
                LayoutIntersections = LayoutIntersections.Distinct().ToList();
            }
        }

        private void ShowMeasureTextbox(MeasureValueBox box)
        {
            var boxBounds = GetMeasureBoxBounds(box);
            var popupForm = new Form()
            {
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.Manual,
                Padding = Padding.Empty,
                ShowInTaskbar = false
            };
            
            var valueTextbox = new Controls.TextBoxEx()
            {
                ReadOnly = true,
                BackColor = SystemColors.Window,
                Text = box.GetEditValue(),
                BorderStyle = BorderStyle.None,
                Font = Font,
                TextAlign = HorizontalAlignment.Center
            };
            
            popupForm.Controls.Add(valueTextbox);
            valueTextbox.Location = new Point(0, 0);
            valueTextbox.Width = (int)(boxBounds.Width - 2);

            var finalSize = new Size(valueTextbox.Width, valueTextbox.PreferredHeight);
            var screenPos = PointToScreen(new Point((int)boxBounds.X, (int)boxBounds.Y));
            screenPos.Y += (int)Math.Round((boxBounds.Height - (double)finalSize.Height) / 2d);
            screenPos.X += (int)Math.Round((boxBounds.Width - (double)finalSize.Width) / 2d);

            popupForm.Location = screenPos;
            popupForm.Show(this);
            popupForm.Size = finalSize;
            valueTextbox.Focus();

            popupForm.Deactivate += PopupForm_Deactivate;
            valueTextbox.Validated += PopupForm_Deactivate;
            valueTextbox.CommandKeyPressed += ValueTextbox_CommandKeyPressed;
        }

        private void ValueTextbox_CommandKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                PopupForm_Deactivate(sender, EventArgs.Empty);
        }

        private void PopupForm_Deactivate(object sender, EventArgs e)
        {
            if(sender is Form)
                (sender as Form).Close();
            else if (sender is Control)
                (sender as Control).FindForm().Close();
        }

        #endregion

        #region UI <-> 2D coordinates

        private bool IsHorizontal { get { return DisplayConfig.FretboardOrientation == Orientation.Horizontal; } }

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
