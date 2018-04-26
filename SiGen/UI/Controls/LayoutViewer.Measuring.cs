using SiGen.Maths;
using SiGen.Measuring;
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
        private class MeasureManager
        {
            private LayoutViewer Viewer;

            public MeasureManager(LayoutViewer owner)
            {
                Viewer = owner;
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

        private Vector MeasureFirstSelection;
        private Vector MeasureLastSelection;
        private List<MeasureValueBox> MeasureBoxes;

        private void InitializeMeasureTool()
        {
            MeasureBoxes = new List<MeasureValueBox>();
            MeasureFirstSelection = Vector.Empty;
            MeasureLastSelection = Vector.Empty;
            _CurrentMeasure = null;
            _IsMeasuring = false;
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
                _IsMeasuring = false;
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
                }
                var pos = GrabMeasureLocation(position);
                if (!pos.IsEmpty)
                {
                    MeasureFirstSelection = pos;
                    _IsMeasuring = true;
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
                var fingerboardEdges = CurrentLayout.VisualElements.OfType<IFingerboardEdge>();

                foreach(var fretLine in fretLines)
                {
                    LayoutIntersections.AddRange(stringLines.Select(s => fretLine.GetIntersection(s).ToVector()));
                }
                //var fretPos = fretLines.SelectMany(fl => fl.Segments.Where(s => !s.IsVirtual)).Select(s => s.PointOnString).Distinct();
                //LayoutIntersections.AddRange(fretPos.Select(p => p.ToVector()));

                LayoutIntersections.AddRange(stringCenters.SelectMany(s => fretLines.Select(f => f.GetIntersection(s).ToVector())));
                LayoutIntersections.RemoveAll(p => p.IsEmpty);

                LayoutIntersections.AddRange(stringLines.SelectMany(s => fingerboardEdges.Where(e => !e.IsSideEdge).Select(f => f.GetIntersection(s).ToVector())));
                LayoutIntersections.RemoveAll(p => p.IsEmpty);

                LayoutIntersections.AddRange(stringCenters.SelectMany(s => fingerboardEdges.Where(e => !e.IsSideEdge).Select(f => f.GetIntersection(s).ToVector())));
                LayoutIntersections.RemoveAll(p => p.IsEmpty);

                LayoutIntersections.AddRange(fretLines.Select(fl => fl.Points.First().ToVector()));
                LayoutIntersections.AddRange(fretLines.Select(fl => fl.Points.Last().ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<LayoutLine>().Select(fl => fl.P1.ToVector()));
                LayoutIntersections.AddRange(CurrentLayout.VisualElements.OfType<LayoutLine>().Select(fl => fl.P2.ToVector()));

                LayoutIntersections.RemoveAll(p => p.IsEmpty);
                LayoutIntersections = LayoutIntersections.Distinct().ToList();
            }
        }

        private FloatingTextBox measureEditor;

        private void ShowMeasureTextbox(MeasureValueBox box)
        {
            if (measureEditor == null)
                measureEditor = new FloatingTextBox(this);

            measureEditor.Font = Font;

            var boxBounds = box.DisplayBounds;

            var finalSize = new Size(boxBounds.Width - 2, measureEditor.textbox.PreferredHeight);
            var screenPos = PointToScreen(boxBounds.Location);

            screenPos.Y += (int)Math.Round((boxBounds.Height - (double)finalSize.Height) / 2d);
            screenPos.X += (int)Math.Round((boxBounds.Width - (double)finalSize.Width) / 2d);

            measureEditor.ShowAt(new Rectangle(screenPos, finalSize), box.GetEditValue());
        }

        private class FloatingTextBox : Form
        {
            internal Controls.TextBoxEx textbox;
            internal DateTime LastClosedTime;
            private LayoutViewer owner;

            public FloatingTextBox(LayoutViewer viewer)
            {
                owner = viewer;
                LastClosedTime = DateTime.Today;
                FormBorderStyle = FormBorderStyle.None;
                StartPosition = FormStartPosition.Manual;
                Padding = Padding.Empty;
                ShowInTaskbar = false;
                textbox = new Controls.TextBoxEx()
                {
                    ReadOnly = true,
                    BackColor = SystemColors.Window,
                    BorderStyle = BorderStyle.None,
                    TextAlign = HorizontalAlignment.Center
                };
                
                Controls.Add(textbox);
                textbox.CommandKeyPressed += Textbox_CommandKeyPressed;
                Deactivate += FloatingTextBox_Deactivate;
                textbox.LostFocus += FloatingTextBox_Deactivate;
            }

            private void FloatingTextBox_Deactivate(object sender, EventArgs e)
            {
                Hide();
                LastClosedTime = DateTime.Now;
            }

            private void Textbox_CommandKeyPressed(object sender, KeyEventArgs e)
            {
                if (e.KeyCode == Keys.Escape || e.KeyCode == Keys.Tab)
                    Hide();
            }

            public void ShowAt(Rectangle bounds, string value)
            {
                textbox.Text = value;
                Location = bounds.Location;
                Show();
                Size = bounds.Size;
                textbox.Width = bounds.Width;
                textbox.Focus();
            }
        }
    }
}
