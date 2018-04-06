using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;
using SiGen.Maths;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class FretLine : LayoutPolyLine
    {
        private List<FretSegment> _Segments;
        private bool _IsStraight;
        private bool _IsNut;

        public List<FretSegment> Segments
        {
            get { return _Segments; }
        }

        public int FretIndex { get { return _Segments[0].FretIndex; } }

        public IEnumerable<SIString> Strings { get { return _Segments.Where(s => !s.IsVirtual).Select(s => s.String); } }

        public int StringCount { get { return _Segments.Count(s => !s.IsVirtual); } }

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

            double tolerance = Layout.ShouldHaveStraightFrets() ? 2.5 : 1;
            //double maxDiff = 0;

            for (int i = 1; i < points.Count() - 1; i++)
            {
                var curAngle = Angle.FromPoints(points.First().ToVector(), points.ElementAt(i).ToVector());
                curAngle.Normalize();
                var angleDiff = Math.Abs(curAngle.Degrees - avgAngle.Degrees);
                if (angleDiff > tolerance)
                    return false;
                //if (angleDiff > maxDiff)
                //    maxDiff = angleDiff;
            }

            return true;

            //if (maxDiff >= 1)
            //    Console.WriteLine(string.Format("max diff for fret {0}: {1}", FretIndex, Angle.FromDegrees(maxDiff)));
            //return maxDiff <= tolerance;
        }

        public void ComputeFretShape()
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
                Points.Add(PointM.FromVector(line.GetIntersection(leftBound.Equation), UnitOfMeasure.Centimeters));
                Points.Add(PointM.FromVector(line.GetIntersection(rightBound.Equation), UnitOfMeasure.Centimeters));
            }
            else
            {
                if (Layout.FretInterpolation == FretInterpolationMethod.Spline && StringCount >= 2)
                {
                    foreach (var seg in Segments.Where(s => !s.IsVirtual || s.String.HasFret(s.FretIndex)))
                        Points.Add(seg.PointOnString);

                    InterpolateCurve();

                    ExtendToBorders();
                }
                else if(Layout.FretInterpolation == FretInterpolationMethod.NotchedSpline && StringCount >= 2)
                {
                    
                    foreach (var seg in Segments.Where(s => !s.IsVirtual))
                    {
                        Points.Add(PointM.Average(seg.P2, seg.PointOnString));
                        Points.Add(seg.PointOnString);
                        Points.Add(PointM.Average(seg.P1, seg.PointOnString));
                    }

                    InterpolateCurve();

                    ExtendToBorders();
                }
                else
                {
                    foreach (var seg in Segments.Where(s => !s.IsVirtual))
                        Points.Add(seg.PointOnString);
                    ExtendToBorders();
                }

                (Points as ObservableCollectionEx<PointM>).Reverse();

                if (Points.Count == 2)
                    _IsStraight = true;
                
            }
        }

        private void ExtendToBorders()
        {
            var firstSeg = Segments.First(fs => !fs.IsVirtual);
            var firstBound = (IStringBoundary)Layout.GetStringBoundaryLine(firstSeg.String, FingerboardSide.Treble);//first segment is toward treble side so edge is at right (Treble)

            var lastSeg = Segments.Last(fs => !fs.IsVirtual);
            var lastBound = (IStringBoundary)Layout.GetStringBoundaryLine(lastSeg.String, FingerboardSide.Bass);//last segment is toward bass side so edge is at left (Bass)

            var p1 = GetIntersection((LayoutLine)firstBound);
            var p2 = GetIntersection((LayoutLine)lastBound);

            for(int i = 0; i < Points.Count - 1; i++)
            {
                if (p1.X > Points[i].X)
                    break;
                Points.RemoveAt(0);
            }

            for (int i = Points.Count - 1; i > 0; i--)
            {
                if (p2.X < Points[i].X)
                    break;
                Points.RemoveAt(i);
            }

            Points.Insert(0, p1);
            Points.Add(p2);
            //var p11 = firstBound.GetRelativePoint(firstSeg.String.LayoutLine, firstSeg.PointOnString);
            //var p21 = lastBound.GetRelativePoint(lastSeg.String.LayoutLine, lastSeg.PointOnString);

            //if (Points.Count >= 2)
            //{
            //    var p12 = new LayoutLine(Points[0], Points[1]).GetIntersection((LayoutLine)firstBound);
            //    var p22 = new LayoutLine(Points[Points.Count - 1], Points[Points.Count - 2]).GetIntersection((LayoutLine)lastBound);

            //    Points.Insert(0, p12);
            //    Points.Add(p22);

            //    //Points.Insert(0, PointM.Average(p11, p12));
            //    //Points.Add(PointM.Average(p21, p22));
            //}
            //else
            //{
            //    Points.Insert(0, p11);
            //    Points.Add(p21);
            //}
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
                    newLine.ComputeFretShape();
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
                    newLine.ComputeFretShape();
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
            left.ComputeFretShape();
            right.ComputeFretShape();
        }

        internal override void FlipHandedness()
        {
            base.FlipHandedness();
            _Segments.ForEach(s => s.FlipHandedness());
        }
    }
}
