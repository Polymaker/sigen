using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;
using SiGen.Maths;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class FretLine : VisualElement
    {
        private List<FretSegment> _Segments;
        private List<PointM> _Points;
        private RectangleM _Bounds;
        private bool _IsStraight;
        private bool _IsNut;

        public List<FretSegment> Segments
        {
            get { return _Segments; }
        }

        public List<PointM> Points
        {
            get { return _Points; }
        }

        public int FretIndex { get { return _Segments[0].FretIndex; } }

        public IEnumerable<SIString> Strings { get { return _Segments.Where(s => !s.IsVirtual).Select(s => s.String); } }

        public override RectangleM Bounds
        {
            get { return _Bounds; }
        }

        public bool IsStraight
        {
            get { return _IsStraight; }
        }

        public bool IsNut
        {
            get { return _IsNut; }
        }

        public FretLine(IEnumerable<FretSegment> segments)
        {
            _Segments = new List<FretSegment>(segments.OrderBy(s=>s.String.Index));
            _Points = new List<PointM>();
            _Bounds = RectangleM.Empty;
            _IsNut = Segments.All(s => s.IsNut || s.IsVirtual);
        }

        public void VerifyIsStraight()
        {
            _IsStraight = true;
            if (Segments.Count > 2)
            {
                _IsStraight = CheckPointsAreStraight(Segments.Select(s => s.PointOnString));
            }
            else if(Points.Count > 2)
            {
                //unused at the moment, was coded in case I needed to create fret lines directly from points
                _IsStraight = CheckPointsAreStraight(Points);
            }
        }

        private bool CheckForHardBreak(/*IEnumerable<PointM> points*/)
        {
            var points = new List<PointM>();
            for (int i = 0; i < Segments.Count; i++)
            {
                if (i == 0)
                    points.Add(Segments[i].P2);

                points.Add(Segments[i].PointOnString);

                if (i == Segments.Count - 1)
                    points.Add(Segments[i].P1);
            }

            int straightCtr = 0;
            Vector prevDir = Vector.Empty;
            Angle prevAngle = Angle.Empty;
            for (int i = 0; i < points.Count - 1; i++)
            {
                var curDir = (points[i + 1] - points[i]).Direction;
                var curAngle = Angle.FromPoints(points[i].ToVector(), points[i + 1].ToVector()).Normalized();
                if (!prevAngle.IsEmpty)
                {
                    if(Math.Abs(curAngle.Degrees - prevAngle.Degrees) > 25)
                    {
                        //hard break
                        SplitFret(i - 1);
                        return true;
                    }
                }
                if (!prevDir.IsEmpty)
                {
                    if ((prevDir - curDir).Length <= 0.01)
                        straightCtr++;
                }
                prevAngle = curAngle;
                prevDir = curDir;
            }
            return false;
        }

        private bool CheckPointsAreStraight(IEnumerable<PointM> points)
        {
            Angle avgAngle = Angle.FromPoints(points.First().ToVector(), points.Last().ToVector());
            avgAngle.Normalize();

            for (int i = 0; i < points.Count() - 1; i++)
            {
                var curAngle = Angle.FromPoints(points.ElementAt(i).ToVector(), points.ElementAt(i + 1).ToVector());
                curAngle.Normalize();
                if (Math.Abs(curAngle.Degrees - avgAngle.Degrees) > 0.9)
                    return false;
            }
            return true;
        }

        public void BuildLayout()
        {
            VerifyIsStraight();
            //if (!IsStraight && !Layout.CompensateFretPositions && Layout.FretsTemperament == Physics.Temperament.Equal && CheckForHardBreak())
            //    return;

            var layout = Segments[0].String.Layout;

            if (Segments.Any(s => s.IsNut && !s.IsVirtual) && !Segments.All(s => s.IsNut || s.IsVirtual))
            {
                SeparateNutFromFrets();
                return;
            }

            var leftBound = layout.GetStringBoundaryLine(Segments.Last(fs => !fs.IsVirtual).String, FingerboardSide.Bass);
            var rightBound = layout.GetStringBoundaryLine(Segments.First(fs => !fs.IsVirtual).String, FingerboardSide.Treble);

            if (IsStraight && Segments.Count > 1)
            {
                var line = Line.FromPoints(Segments.First().PointOnString.ToVector(), Segments.Last().PointOnString.ToVector());
                _Points.Add(PointM.FromVector(line.GetIntersection(leftBound.Equation), UnitOfMeasure.Centimeters));
                _Points.Add(PointM.FromVector(line.GetIntersection(rightBound.Equation), UnitOfMeasure.Centimeters));
            }
            else
            {
                _Points.Add(Segments.First(fs => !fs.IsVirtual).P2);//first segment is toward treble side so edge is at right (P2)

                foreach (var seg in Segments.Where(s => !s.IsVirtual))
                {
                    _Points.Add(PointM.Average(seg.P2, seg.PointOnString));
                    _Points.Add(seg.PointOnString);
                    _Points.Add(PointM.Average(seg.P1, seg.PointOnString));
                }

                _Points.Add(Segments.Last(fs => !fs.IsVirtual).P1);//last segment is toward bass side so edge is at left (P1)

                _Points.Reverse();
            }

            _Bounds = RectangleM.BoundingRectangle(Points);
        }

        private void BuildCurve()
        {
            var segLL = new LinkedList<FretSegment>(Segments);
            var curSeg = segLL.First;

            while (curSeg != null)
            {
                if (!curSeg.Value.IsVirtual)
                {
                    if (Points.Count == 0 && curSeg.Next != null)
                    {
                        var dir1 = (curSeg.Value.PointOnString - curSeg.Next.Value.PointOnString).Direction;
                        var avgLine = Line.FromPoints(curSeg.Value.PointOnString.ToVector(), curSeg.Value.PointOnString.ToVector() + dir1);
                        var lineBound = Layout.GetStringBoundaryLine(curSeg.Value.String, FingerboardSide.Treble);
                        var testPs = lineBound.GetIntersection(avgLine);
                        _Points.Add(PointM.Average(testPs, curSeg.Value.P2));
                    }
                    else if (Points.Count == 0)
                        _Points.Add(Segments.First(fs => !fs.IsVirtual).P2);//first segment is toward treble side so edge is at right (P2)
                    _Points.Add(curSeg.Value.PointOnString);
                    if (curSeg.Next == null && curSeg.Previous != null)
                    {
                        var dir1 = (curSeg.Previous.Value.PointOnString - curSeg.Value.PointOnString).Direction;
                        var avgLine = Line.FromPoints(curSeg.Value.PointOnString.ToVector(), curSeg.Value.PointOnString.ToVector() + dir1);
                        var lineBound = Layout.GetStringBoundaryLine(curSeg.Value.String, FingerboardSide.Bass);
                        var testPs = lineBound.GetIntersection(avgLine);
                        _Points.Add(testPs);
                    }
                }

                curSeg = curSeg.Next;
            }
        }

        private void SeparateNutFromFrets()
        {
            var nutLines = new List<FretLine>();
            for (int i = 0; i < Segments.Count; i++)
            {
                if (Segments[i].IsNut)
                {
                    var nutSegments = Segments.Skip(i).TakeWhile(s => s.IsNut).ToList();

                    if (i + nutSegments.Count < Segments.Count)
                        nutSegments.Add(Segments[i + nutSegments.Count].Clone());

                    if (i > 0)
                        nutSegments.Add(Segments[i - 1].Clone());

                    var newLine = new FretLine(nutSegments);
                    newLine.Layout = Layout;
                    Layout.VisualElements.Add(newLine);
                    nutLines.Add(newLine);
                    newLine.BuildLayout();
                    i += nutSegments.Count(s => !s.IsVirtual) - 1;
                }
            }

            Layout.VisualElements.Remove(this);

            for (int i = 0; i < Segments.Count; i++)
            {
                if (!Segments[i].IsVirtual && !Segments[i].IsNut)
                {
                    var fretSegments = Segments.Skip(i).TakeWhile(s => !s.IsNut && !s.IsVirtual).ToList();
                    int origSegCount = fretSegments.Count;
                    if (i + origSegCount < Segments.Count)
                        fretSegments.Add(Segments[i + origSegCount].Clone());

                    if (i > 0)
                        fretSegments.Add(Segments[i - 1].Clone());

                    var newLine = new FretLine(fretSegments);
                    newLine.Layout = Layout;
                    Layout.VisualElements.Add(newLine);
                    newLine.BuildLayout();
                    i += origSegCount - 1;
                }
            }
        }

        private void SplitFret(int index)
        {
            var left = new FretLine(Segments.Take(index + 1)) { Layout = Layout };
            Layout.VisualElements.Add(left);

            var right = new FretLine(Segments.Skip(index + 1)) { Layout = Layout };
            Layout.VisualElements.Add(right);

            Layout.VisualElements.Remove(this);
            left.BuildLayout();
            right.BuildLayout();
        }

        internal PointM GetPointForX(Measure x)
        {
            for(int i = 0; i < Points.Count - 1; i++)
            {
                if(x>= Points[i].X && x <= Points[i + 1].X)
                {
                    var pt = Line.FromPoints(Points[i].ToVector(), Points[i + 1].ToVector()).GetPointForX(x.NormalizedValue);
                    return PointM.FromVector(pt, UnitOfMeasure.Centimeters);
                }
            }
            return PointM.Empty;
        }

        internal PointM GetIntersection(LayoutLine other)
        {
            var intersection = PointM.Empty;
            if(Points.Count == 2)
            {
                IntersectSegmentWithLine(other.Equation, 0, 1, out intersection);
            }
            else
            {
                for (int i = 0; i < Points.Count - 1; i++)
                {
                    if (IntersectSegmentWithLine(other.Equation, i, i + 1, out intersection))
                        return intersection;
                }
            }
            return intersection;
        }

        private bool IntersectSegmentWithLine(Line line, int idx1, int idx2, out PointM inter)
        {
            var segLine = Line.FromPoints(Points[idx1].ToVector(), Points[idx2].ToVector());
            Vector virtualInter;
            inter = PointM.Empty;
            if (segLine.Intersect(line, out virtualInter))
            {
                if(virtualInter.X >= Points[idx1].X.NormalizedValue && virtualInter.X <= Points[idx2].X.NormalizedValue)
                {
                    inter = PointM.FromVector(virtualInter, UnitOfMeasure.Centimeters);
                    return true;
                }
            }
            return false;
        }

        internal override void FlipHandedness()
        {
            base.FlipHandedness();
            _Points = _Points.Select(p => new PointM(p.X * -1, p.Y)).ToList();
            _Segments.ForEach(s => s.FlipHandedness());
        }
    }
}
