using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public partial class LayoutViewer
    {
        private class MeasureSelection
        {
            public Vector Position { get; set; }
            public LayoutIntersection Intersection { get; set; }
            public bool IsUserDefined => Intersection == null;

            public MeasureSelection(Vector position)
            {
                Position = position;
            }

            public MeasureSelection(LayoutIntersection intersection)
            {
                Intersection = intersection;
                Position = intersection.WorldCoord;
            }
        }

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
            public Measure this[LengthType type]
            {
                get
                {
                    switch (type)
                    {
                        default:
                        case LengthType.Length:
                            return Length;
                        case LengthType.Height:
                            return Height;
                        case LengthType.Width:
                            return Width;
                    }
                }
            }

            public PointM FromPoint { get; }
            public PointM ToPoint { get; }
            public Measure Length { get; }
            public Measure Width { get; }
            public Measure Height { get; }
            public Angle Direction { get; }

            public LayoutMeasure(PointM p1, PointM p2)
            {
                FromPoint = p1;
                ToPoint = p2;
                Length = PointM.Distance(p1, p2);
                Width = Measure.Abs(p2.X - p1.X);
                Height = Measure.Abs(p2.Y - p1.Y);
                Direction = Angle.FromPoints(p1.ToVector(), p2.ToVector());
            }
        }

        private abstract class MeasureValueBox : IUIElement
        {
            private Rectangle _DisplayBounds;
            internal LayoutViewer Viewer;
            protected LayoutMeasure SourceMeasure { get; }

            public SizeF Size { get; set; }
            public Vector TargetPosition { get; set; }
            public Vector OriginalOffset { get; set; }
            public Vector LocalOffset { get; set; }

            public bool Suppressed { get; set; }
            public bool Visible { get; set; }
            public bool ShowExactValue { get; set; }
            public bool IsDirty { get; private set; }

            public Rectangle DisplayBounds
            {
                get
                {
                    if (IsDirty)
                        UpdateDisplayBounds();
                    return _DisplayBounds;
                }
            }

            public MeasureValueBox()
            {
                Visible = true;
                IsDirty = true;
            }

            internal MeasureValueBox(LayoutMeasure measure)
            {
                SourceMeasure = measure;
                Visible = true;
                IsDirty = true;
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
                if (IsDirty)
                {
                    CalculateDisplayBounds();
                    IsDirty = false;
                }
            }

            public void NotifyDirty()
            {
                IsDirty = true;
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
            public LengthType Type { get; }
            public Measure Value { get { return SourceMeasure[Type]; } }
            public UnitOfMeasure DisplayUnit { get; set; }

            public Vector P1 { get; set; }
            public Vector P2 { get; set; }

            internal LengthValueBox(LayoutMeasure measure, LengthType type, Vector p1, Vector p2) : base(measure)
            {
                Type = type;
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

        private Vector MeasureFirstSelection;
        private LayoutIntersection FirstIntersection;
        private LayoutIntersection LastIntersection;
        private Vector MeasureLastSelection;
        private Vector MeasureSnapPosition;

        private List<MeasureValueBox> MeasureBoxes;

        private void InitializeMeasureTool()
        {
            MeasureBoxes = new List<MeasureValueBox>();
            MeasureFirstSelection = Vector.Empty;
            MeasureLastSelection = Vector.Empty;
            MeasureSnapPosition = Vector.Empty;
            FirstIntersection = null;
            LastIntersection = null;
            _CurrentMeasure = null;
            IsMeasuring = false;
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
            menuItemDisplayMeasureClearMeasure.Click += MenuItemDisplayMeasureClearMeasure_Click;
        }

        private void MenuItemDisplayMeasureClearMeasure_Click(object sender, EventArgs e)
        {
            ClearMeasuring();
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
                menuItemDisplayMeasureClearMeasure.Visible = false;
            }
            else if (MeasureBoxes.Any())
            {
                var selectedUnits = MeasureBoxes.OfType<LengthValueBox>().Select(x => x.DisplayUnit).Distinct().ToList();
                menuItemDisplayMeasureMM.Checked = selectedUnits.Count == 1 && selectedUnits[0] == UnitOfMeasure.Mm;
                menuItemDisplayMeasureCM.Checked = selectedUnits.Count == 1 && selectedUnits[0] == UnitOfMeasure.Cm;
                menuItemDisplayMeasureIN.Checked = selectedUnits.Count == 1 && selectedUnits[0] == UnitOfMeasure.In;
                menuItemDisplayMeasureFT.Checked = selectedUnits.Count == 1 && selectedUnits[0] == UnitOfMeasure.Ft;
                menuItemDisplayMeasureShowDecimals.Checked = MeasureBoxes.OfType<LengthValueBox>().All(x => x.ShowExactValue);

                menuItemDisplayMeasureClearMeasure.Visible = true;
            }

        }

        private void MenuItemDisplayMeasureShowDecimals_Click(object sender, EventArgs e)
        {
            if (menuMeasureBox.Tag is LengthValueBox targetBox)
            {
                targetBox.ShowExactValue = menuItemDisplayMeasureShowDecimals.Checked;
                UpdateMeasureBoxBounds(targetBox);
            }
            else
            {
                foreach (var lbox in MeasureBoxes.OfType<LengthValueBox>())
                {
                    lbox.ShowExactValue = menuItemDisplayMeasureShowDecimals.Checked;
                    UpdateMeasureBoxBounds(lbox);
                }
            }
        }

        private void MenuItemDisplayMeasure_Click(object sender, EventArgs e)
        {
            var selectedUnit = (UnitOfMeasure)(sender as ToolStripMenuItem).Tag;

            if (menuMeasureBox.Tag is LengthValueBox targetBox && selectedUnit != targetBox.DisplayUnit)
            {
                targetBox.DisplayUnit = selectedUnit;
                targetBox.ShowExactValue = false;
                UpdateMeasureBoxBounds(targetBox);
            }
            else
            {
                foreach(var lbox in MeasureBoxes.OfType<LengthValueBox>())
                {
                    if (lbox.DisplayUnit != selectedUnit)
                    {
                        lbox.DisplayUnit = selectedUnit;
                        lbox.ShowExactValue = false;
                        UpdateMeasureBoxBounds(lbox);
                    }
                }
            }
        }

        #endregion

        private void ClearMeasuring(bool redraw = true)
        {
            if (_CurrentMeasure != null || !MeasureFirstSelection.IsEmpty || MeasureBoxes.Count > 0)
            {
                _CurrentMeasure = null;
                IsMeasuring = false;
                MeasureBoxes.Clear();
                MeasureFirstSelection = Vector.Empty;
                MeasureLastSelection = Vector.Empty;
                MeasureSnapPosition = Vector.Empty;
                FirstIntersection = null;
                LastIntersection = null;
                //if (menuMeasureBox.)
                //    menuMeasureBox.Close();
                if (IsHandleCreated && redraw)
                    Invalidate();
            }
        }

        private void AdjustMeasureAfterLayoutChanged()
        {
            _CurrentMeasure = null;
            IsMeasuring = false;
            MeasureBoxes.Clear();

            if (FirstIntersection != null && LastIntersection != null)
            {
                var similar1 = LayoutIntersections.FirstOrDefault(x => x.IsSimilar(FirstIntersection));
                var similar2 = LayoutIntersections.FirstOrDefault(x => x.IsSimilar(LastIntersection));

                if (similar1 != null && similar2 != null)
                {
                    FirstIntersection = similar1;
                    MeasureFirstSelection = similar1.WorldCoord;
                    LastIntersection = similar2;
                    MeasureLastSelection = similar2.WorldCoord;
                    CompleteMeasure();
                }
                else
                    ClearMeasuring(false);
            }
            else
                ClearMeasuring(false);
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
            //CreateAngleMeasureBox2(_CurrentMeasure);
            MeasureBoxes.RemoveAll(m => m == null);

            Invalidate();
        }

        private void CaptureMeasure(Point position)
        {
            if (MeasureFirstSelection.IsEmpty || !MeasureLastSelection.IsEmpty)
            {
                if (!MeasureLastSelection.IsEmpty)
                {
                    if (measureEditor != null && (DateTime.Now - measureEditor.LastClosedTime).TotalMilliseconds < 200)
                        return;
                    ClearMeasuring();
                    return;
                }
                var pos = MeasureSnapPosition;// GrabMeasureLocation(position);
                if (!pos.IsEmpty)
                {
                    MeasureFirstSelection = pos;
                    FirstIntersection = GetVisibleIntersections().FirstOrDefault(x => x.WorldCoord.EqualOrClose(pos, 0.001));

                    IsMeasuring = true;
                    Invalidate();
                }
            }
            else if (MeasureLastSelection.IsEmpty)
            {
                var pos = MeasureSnapPosition;
                if (!pos.IsEmpty)
                {
                    MeasureLastSelection = pos;
                    LastIntersection = GetVisibleIntersections().FirstOrDefault(x => x.WorldCoord.EqualOrClose(pos, 0.001));
                    if (Vector.EqualOrClose(MeasureFirstSelection, MeasureLastSelection))
                        ClearMeasuring();
                    else
                        CompleteMeasure();
                }
            }
        }

        private void UpdateMeasureSnapPosition()
        {
            UpdateMeasureSnapPosition(PointToClient(MousePosition));
        }

        private void UpdateMeasureSnapPosition(Point mousePos)
        {
            var previousSnapPos = MeasureSnapPosition;
            var mouseWorldPos = DisplayToWorld(mousePos);
            bool angleSnap = !MeasureFirstSelection.IsEmpty && IsControlPressed();
            var interSnapDist = 20 / _Zoom; // 20 pixels
            var lineSnapDist = 32 / _Zoom; // 32 pixels

            if (angleSnap)
            {
                var measureAngle = Angle.FromDirectionVector(mouseWorldPos - MeasureFirstSelection);
                var measureDist = (mouseWorldPos - MeasureFirstSelection).Length;
                double snapAngle = 11.25;
                measureAngle = Angle.FromDegrees(Math.Round(measureAngle.Degrees / snapAngle) * snapAngle);
                mouseWorldPos = MeasureFirstSelection + (Vector.FromAngle(measureAngle) * measureDist);
            }

            if(IsAltPressed())
            {
                MeasureSnapPosition = mouseWorldPos;
            }
            else if (angleSnap)
            {
                bool isSnapped = SnapToClosestLine(MeasureFirstSelection, mouseWorldPos, 0.5, out MeasureSnapPosition, out _);

                if (!isSnapped)
                    isSnapped = SnapToClosestLine(mouseWorldPos, 0.5, out MeasureSnapPosition, out _);

                if (!isSnapped)
                    MeasureSnapPosition = previousSnapPos;
            }
            else
            {
                MeasureSnapPosition = SnapToNearest(mouseWorldPos, interSnapDist, lineSnapDist);
            }

            InvalidateWorldRegion(20, mouseWorldPos, previousSnapPos, MeasureSnapPosition, MeasureFirstSelection);
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

            if (!measureBox.Suppressed)
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
            if (angle1Box.Value.IsEmpty || angle2Box.Value.IsEmpty || angle1Box.Value == Angle.Zero || angle2Box.Value == Angle.Zero)
                return;
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
                var oldBounds = box.DisplayBounds;

                box.UpdateSize();

                var wasMoved = box.OriginalOffset != box.LocalOffset;

                var centerUI = WorldToLocal(box.TargetPosition, _Zoom, true);
                var targetUI = centerUI + box.OriginalOffset;
                var boxOffsetDir = targetUI - centerUI;

                box.OriginalOffset = GetMeasureBoxOffset(boxOffsetDir, box.Size);

                if (!wasMoved)
                    box.LocalOffset = box.OriginalOffset;

                box.NotifyDirty();
                
                if (box.IsMeasureVisible())
                {
                    var newBounds = box.DisplayBounds;
                    oldBounds.Inflate(10, 10);
                    newBounds.Inflate(10, 10);
                    //oldBounds.Inflate((int)box.LocalOffset.Length + 10, (int)box.LocalOffset.Length + 10);
                    //newBounds.Inflate((int)box.LocalOffset.Length + 10, (int)box.LocalOffset.Length + 10);
                    Invalidate(oldBounds);
                    Invalidate(newBounds);
                }
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

        private bool SnapToClosestIntersection(Vector mouseWorldPos, double snapRange, out Vector snapPos, out double snapDist)
        {
            snapPos = Vector.Empty;
            snapDist = -1;
            
            var pointsNear = GetVisibleIntersections().Where(i => i.GetDistance(mouseWorldPos) <= snapRange);

            if (pointsNear.Any())
            {
                snapPos = pointsNear.OrderBy(i => i.GetDistance(mouseWorldPos)).First().WorldCoord;
                snapDist = (double)(mouseWorldPos - snapPos).Length;
            }

            return !snapPos.IsEmpty;
        }

        private bool SnapToClosestLine(Vector mouseWorldPos, double snapRange, out Vector snapPos, out double snapDist)
        {
            snapPos = Vector.Empty;
            snapDist = -1;

            foreach (var line in CurrentLayout.VisualElements.OfType<LayoutLine>())
            {
                if (!IsElementVisible(line))
                    continue;

                var linePt = line.SnapToLine(mouseWorldPos, LineSnapDirection.Perpendicular, false);
                if (linePt.IsEmpty)
                    continue;

                var ptDist = (linePt - mouseWorldPos).Length;

                if (ptDist <= snapRange && (snapPos.IsEmpty || ptDist < snapDist))
                {
                    snapDist = (double)ptDist;
                    snapPos = linePt;
                }
            }

            return !snapPos.IsEmpty;
        }

        private bool SnapToClosestLine(Vector measureStart, Vector measureEnd, double snapRange, out Vector snapPos, out double snapDist)
        {
            snapPos = Vector.Empty;
            snapDist = -1;
            var measureLine = Line.FromPoints(measureStart, measureEnd);

            foreach (var line in CurrentLayout.VisualElements.OfType<LayoutLine>())
            {
                if (!IsElementVisible(line))
                    continue;
                
                if (!line.Intersects(measureLine, out Vector linePt, false))
                    continue;
                
                var ptDist = (linePt - measureEnd).Length;

                if (ptDist <= snapRange && (snapPos.IsEmpty || ptDist < snapDist))
                {
                    snapDist = (double)ptDist;
                    snapPos = linePt;
                }
            }

            return !snapPos.IsEmpty;
        }

        public Vector SnapToNearest(Vector worldPos, double intersectionRange, double lineRange)
        {
            //var pointsNear = LayoutIntersections2.Where(i => i.GetDistance(worldPos) <= intersectionRange);
            
            SnapToClosestIntersection(worldPos, intersectionRange, out Vector closestInter, out double interDist);

            SnapToClosestLine(worldPos, lineRange, out Vector closestLine, out double lineDist);

            if (!closestInter.IsEmpty && !closestLine.IsEmpty)
            {
                var interRatio = interDist / intersectionRange;
                return interRatio < 0.75 ? closestInter : closestLine;
                //var lineRatio = lineDist / lineRange;
                //return interRatio < lineRatio ? closestInter : closestLine;
            }
            else if (!closestInter.IsEmpty)
                return closestInter;
            else if (!closestLine.IsEmpty)
                return closestLine;

            return Vector.Empty;
        }

        private void CalculateIntersections()
        {
            LayoutIntersections.Clear();

            if (CurrentLayout != null && CurrentLayout.VisualElements.Count > 0)
            {
                var fretLines = CurrentLayout.VisualElements.OfType<FretLine>();
                var stringLines = CurrentLayout.VisualElements.OfType<StringLine>();
                var stringCenters = CurrentLayout.VisualElements.OfType<StringCenter>();
                var fingerboardEdges = CurrentLayout.VisualElements.OfType<IFingerboardEdge>();
                var centerLine = CurrentLayout.VisualElements.OfType<LayoutLine>().FirstOrDefault(x => x.ElementType == VisualElementType.CenterLine);

                foreach (var fretLine in fretLines)
                {
                    foreach (var line in stringLines)
                    {
                        if (fretLine.Intersects(line, out PointM inter, false))
                            LayoutIntersections.Add(new LayoutIntersection(line, fretLine, inter));
                    }

                    foreach (var line in stringCenters)
                    {
                        if (fretLine.Intersects(line, out PointM inter, false))
                            LayoutIntersections.Add(new LayoutIntersection(line, fretLine, inter));
                    }

                    foreach (var line in fingerboardEdges.OfType<FingerboardSideEdge>())
                    {
                        if (fretLine.Intersects(line, out PointM inter, false))
                            LayoutIntersections.Add(new LayoutIntersection(line, fretLine, inter));
                    }


                    if(fretLine.Intersects(centerLine, out PointM inter2, false))
                        LayoutIntersections.Add(new LayoutIntersection(centerLine, fretLine, inter2));
                }

                foreach (var fingerboardEnd in fingerboardEdges.Where(e => !e.IsSideEdge).OfType<ILayoutLine>())
                {
                    foreach (var line in stringLines)
                    {
                        if (fingerboardEnd.Intersects(line, out PointM inter, false))
                            LayoutIntersections.Add(new LayoutIntersection(line, fingerboardEnd, inter));
                    }

                    foreach (var line in stringCenters)
                    {
                        if (fingerboardEnd.Intersects(line, out PointM inter, false))
                            LayoutIntersections.Add(new LayoutIntersection(line, fingerboardEnd, inter));
                    }

                    if (fingerboardEnd.Intersects(centerLine, out PointM inter2, false))
                        LayoutIntersections.Add(new LayoutIntersection(centerLine, fingerboardEnd, inter2));
                }

                foreach (var line in CurrentLayout.VisualElements.OfType<LayoutLine>())
                {
                    var v1 = line.P1.ToVector();
                    var v2 = line.P2.ToVector();

                    //if (!LayoutIntersections.Any(x => x.WorldCoord.EqualOrClose(v1, 0.001)))
                        LayoutIntersections.Add(new LayoutIntersection(line, line.P1));

                    //if (!LayoutIntersections.Any(x => x.WorldCoord.EqualOrClose(v2, 0.001)))
                        LayoutIntersections.Add(new LayoutIntersection(line, line.P2));
                }
            }
        }

        public IEnumerable<LayoutIntersection> GetVisibleIntersections()
        {
            return LayoutIntersections.Where(x => IsElementVisible(x.Element1) && (x.Element2 == null || IsElementVisible(x.Element2)));
        }

        private FloatingTextBox measureEditor;

        private void ShowMeasureTextbox(MeasureValueBox box)
        {
            if (measureEditor == null)
                measureEditor = new FloatingTextBox(this);

            var boxBounds = box.DisplayBounds;

            var finalSize = new Size(boxBounds.Width - 2, measureEditor.ValueTextbox.PreferredHeight);
            //var screenPos = PointToScreen(boxBounds.Location);

            //screenPos.Y += (int)Math.Round((boxBounds.Height - (double)finalSize.Height) / 2d);
            //screenPos.X += (int)Math.Round((boxBounds.Width - (double)finalSize.Width) / 2d);

            measureEditor.ShowAt(new Rectangle(boxBounds.Location, finalSize), box.GetEditValue());
        }

        private class FloatingTextBox
        {
            private ToolStripDropDown DropDown;
            private ToolStripControlHost ControlHost;
            internal Controls.TextBoxEx ValueTextbox;
            internal DateTime LastClosedTime;
            private readonly LayoutViewer Owner;
            //private MeasureTextbox MeasureTextbox;

            public FloatingTextBox(LayoutViewer viewer)
            {
                DropDown = new ToolStripDropDown();
                DropDown.Padding = new Padding(1, 1, 2, 1);
                Owner = viewer;
                LastClosedTime = DateTime.Today;

                ValueTextbox = new Controls.TextBoxEx()
                {
                    ReadOnly = true,
                    BackColor = SystemColors.Window,
                    BorderStyle = BorderStyle.None,
                    TextAlign = HorizontalAlignment.Center,
                    Font = viewer.Font,
                    Margin = new Padding(0),
                    Dock = DockStyle.Fill
                };

                ControlHost = new ToolStripControlHost(ValueTextbox)
                {
                    AutoSize = false,
                    Padding = new Padding(0),
                };

                DropDown.Items.Add(ControlHost);
                ValueTextbox.CommandKeyPressed += Textbox_CommandKeyPressed;
                DropDown.Closed += DropDown_Closed;
            }

            private void DropDown_Closed(object sender, ToolStripDropDownClosedEventArgs e)
            {
                LastClosedTime = DateTime.Now;
            }

            private void Textbox_CommandKeyPressed(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Tab)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    DropDown.Close();
                }
            }

            public void ShowAt(Rectangle bounds, string value)
            {
                ControlHost.Size = bounds.Size;
                ValueTextbox.Text = value;
                DropDown.Show(Owner, bounds.Location);
                ValueTextbox.Focus();
            }
        }

        
    }
}
