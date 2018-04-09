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

            if (IsStraight && Segments.Count > 1)
            {
                var leftBound = layout.GetStringBoundaryLine(Segments.Last(fs => !fs.IsVirtual).String, FingerboardSide.Bass);
                var rightBound = layout.GetStringBoundaryLine(Segments.First(fs => !fs.IsVirtual).String, FingerboardSide.Treble);

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

                    InterpolateSplineV2( 1d / (Segments.Count * 2.2));

                    ExtendToBorders();
                }
                else if(Layout.FretInterpolation == FretInterpolationMethod.NotchedSpline && StringCount >= 2)
                {
                    
                    foreach (var seg in Segments.Where(s => !s.IsVirtual || s.String.HasFret(s.FretIndex)))
                    {
                        //Points.Add(seg.PointOnString + (seg.Direction * Measure.Mm(1.5)));
                        Points.Add(PointM.Average(seg.P2, seg.PointOnString));
                        //Points.Add(seg.PointOnString);
                        Points.Add(PointM.Average(seg.P1, seg.PointOnString));
                        //Points.Add(seg.PointOnString + (seg.Direction * -1 * Measure.Mm(1.5)));
                    }

                    InterpolateSpline(0.33, 0.4);

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
            var firstSegment = Segments.First(fs => !fs.IsVirtual);
            var firstBound = (IStringBoundary)Layout.GetStringBoundaryLine(firstSegment.String, FingerboardSide.Treble);//first segment is toward treble side so edge is at right (Treble)

            var lastSegment = Segments.Last(fs => !fs.IsVirtual);
            var lastBound = (IStringBoundary)Layout.GetStringBoundaryLine(lastSegment.String, FingerboardSide.Bass);//last segment is toward bass side so edge is at left (Bass)

            if(Points.Count == 1)
            {
                Points.Insert(0, firstBound.GetRelativePoint(firstSegment.String.LayoutLine, firstSegment.PointOnString));
                Points.Add(lastBound.GetRelativePoint(firstSegment.String.LayoutLine, firstSegment.PointOnString));
                Spline = null;
                return;
            }
            if (Points.Count == 2)
            {
                Points.Insert(0, GetIntersection((LayoutLine)firstBound));
                Points.Add(GetIntersection((LayoutLine)lastBound));
                Spline = null;
                return;
            }

            int inter1Idx, inter2Idx;
            var p1 = GetIntersection((LayoutLine)firstBound, out inter1Idx);
            var p2 = GetIntersection((LayoutLine)lastBound, out inter2Idx);

            var pointsToKeep = new List<PointM>();

            for (int i = inter1Idx; i <= inter2Idx; i++)
            {
                if(inter1Idx == i)
                {
                    var ptRel = GetLocationRelativeToSegment(Points[i].ToVector(), Points[i + 1].ToVector(), p1.ToVector());
                    if (ptRel != PointRelation.Before)
                        continue;
                }
                
                pointsToKeep.Add(Points[i]);

                if (inter2Idx == i)
                {
                    var ptRel = GetLocationRelativeToSegment(Points[i].ToVector(), Points[i + 1].ToVector(), p2.ToVector());
                    if (ptRel == PointRelation.After)
                        pointsToKeep.Add(Points[i + 1]);
                }
            }

            Points.Clear();
            Points.Add(p1);
            (Points as ObservableCollectionEx<PointM>).AddRange(pointsToKeep);
            Points.Add(p2);

            for (int i = 0; i < Points.Count - 1; i++)
            {
                if (Points.Count - 1 > 3 && PointM.Distance(Points[i + 1], Points[i]).NormalizedValue < 0.05)
                    Points.RemoveAt(i--);
            }
            
        }

        public void RebuildSpline()
        {
            if (Spline != null)
            {
                Spline = new BezierSpline(Points.Select(p => p.ToVector()).ToArray());
                //var splinePoints = new List<PointM>();

                //bool isReversed = PointM.Distance(Points[0], Segments.First(s => !s.IsVirtual).PointOnString) >
                //    PointM.Distance(Points.Last(), Segments.First(s => !s.IsVirtual).PointOnString);

                //if(Spline.SplinePoints.Length == Segments.Count)
                //{
                //    double step = 1d / (double)Segments.Count;
                //    for (int i = 0; i < Segments.Count; i++)
                //    {
                //        if (!Segments[i].IsVirtual)
                //        {
                //            var t = (step * i) + step / 2;
                //            var avgPt = Spline.GetPoint(t);
                //            splinePoints.Add(PointM.FromVector(avgPt, Points[0].Unit));
                //        }
                //    }
                //}
                //else
                //{
                //    for (int i = 0; i < StringCount; i++)
                //    {
                //        var pointOnFret = GetIntersection(Strings.ElementAt(i).LayoutLine);
                //        if (pointOnFret.IsEmpty)
                //            pointOnFret = Segments.Where(s => !s.IsVirtual).ElementAt(i).PointOnString;
                //        splinePoints.Add(pointOnFret);
                //    }
                //}

                //if (isReversed)
                //{
                //    splinePoints.Insert(0, Points[Points.Count - 1]);
                //    splinePoints.Add(Points[0]);
                //}
                //else
                //{
                //    splinePoints.Insert(0, Points[0]);
                //    splinePoints.Add(Points[Points.Count - 1]);
                //}

                //Spline = new BezierSpline(splinePoints.Select(p => p.ToVector()).ToArray());
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
