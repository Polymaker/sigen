using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class LayoutPolyLine : VisualElement
    {
        private bool isDirty;
        private RectangleM _Bounds;
        private Measure _Length;
        private ObservableCollectionEx<PointM> _Points;
        private BezierSpline _Spline;

        public IList<PointM> Points
        {
            get { return _Points; }
        }

        public Measure Length
        {
            get
            {
                if (isDirty)
                    UpdateInfos();
                return _Length;
            }
        }

        public override RectangleM Bounds
        {
            get
            {
                if (isDirty)
                    UpdateInfos();
                return _Bounds;
            }
        }

        public BezierSpline Spline
        {
            get { return _Spline; }
            protected set { _Spline = value; }
        }

        public LayoutPolyLine()
        {
            _Points = new ObservableCollectionEx<PointM>();
            _Points.CollectionChanged += Points_CollectionChanged;
        }

        public LayoutPolyLine(IEnumerable<PointM> points)
        {
            _Points = new ObservableCollectionEx<PointM>();
            _Points.CollectionChanged += Points_CollectionChanged;
            _Points.AddRange(points);
        }

        private void Points_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            isDirty = true;
        }

        private void UpdateInfos()
        {
            _Length = Measure.Zero;
            for (int i = 0; i < Points.Count - 1; i++)
                _Length += PointM.Distance(Points[i], Points[i + 1]);
            _Bounds = RectangleM.BoundingRectangle(_Points);
            isDirty = false;
        }

        public void InterpolateSpline(double resolution = 0.25, double weight = 1)
        {
            if (Points.Count < 3)
                return;

            if(_Spline == null)
                _Spline = new BezierSpline(Points.Select(p => p.ToVector()), weight);

            var finalPoints = new List<PointM>();
            var splinePoints = _Spline.Interpolate(resolution);
            finalPoints.AddRange(splinePoints.Select(v => PointM.FromVector(v, Points[0].Unit)));
            
            if (finalPoints.Count > 1)
            {
                _Points.Clear();
                _Points.AddRange(finalPoints);
            }
        }

        public void InterpolateSplineV2(double resolution = 0.1)
        {
            if (Points.Count < 3)
                return;

            if (_Spline == null)
                _Spline = new BezierSpline(Points.Select(p => p.ToVector()).ToArray());

            var finalPoints = new List<PointM>();
            var splinePoints = _Spline.InterpolateV2(0.1);
            finalPoints.AddRange(splinePoints.Select(v => PointM.FromVector(v, Points[0].Unit)));

            if (finalPoints.Count > 1)
            {
                _Points.Clear();
                _Points.AddRange(finalPoints);
            }
        }

        #region Intersection

        public PointM GetIntersection(LayoutLine other)
        {
            var intersection = PointM.Empty;
            Intersects(other, out intersection, true);
            return intersection;
        }

        public PointM GetIntersection(LayoutLine other, bool infiniteLine)
        {
            var intersection = PointM.Empty;
            Intersects(other, out intersection, infiniteLine);
            return intersection;
        }

        public PointM GetIntersection(LayoutLine other, out int segmentIndex, bool infiniteLine = true)
        {
            var intersection = PointM.Empty;
            Intersects(other, out intersection, out segmentIndex, infiniteLine);
            return intersection;
        }

        public bool Intersects(LayoutLine line, out PointM intersection, bool infiniteLine = true)
        {
            int dummy;
            return Intersects(line, out intersection, out dummy, infiniteLine);
        }

        public bool Intersects(LayoutLine line, out PointM intersection, out int segmentIndex, bool infiniteLine = true)
        {
            intersection = PointM.Empty;
            Vector virtualInter;

            if (Intersects(line.Equation, out virtualInter, out segmentIndex, infiniteLine))
            {
                intersection = PointM.FromVector(virtualInter, Points.First().Unit);
                return true;
            }

            return false;
        }

        public bool Intersects(LayoutPolyLine line, out PointM intersection)
        {
            intersection = PointM.Empty;
            Vector virtualInter;
            for (int i = 0; i < line.Points.Count - 1; i++)
            {
                var segLine = Line.FromPoints(line.Points[i].ToVector(), line.Points[i + 1].ToVector());
                if (Intersects(segLine, out virtualInter, false))
                {

                    return true;
                }
            }

            return false;
        }

        public bool Intersects(Line line, out Vector intersection, bool infiniteLine = true)
        {
            int dummy;
            return Intersects(line, out intersection, out dummy, infiniteLine);
        }

        public bool Intersects(Line line, out Vector intersection, out int segmentIndex, bool infiniteLine = true)
        {
            intersection = Vector.Empty;
            segmentIndex = -1;
            for (int i = 0; i < Points.Count - 1; i++)
            {
                var p1 = Points[i].ToVector();
                var p2 = Points[i + 1].ToVector();
                var segLine = Line.FromPoints(p1, p2);
                Vector virtualInter;
                if (line.Intersect(segLine, out virtualInter))
                {
                    var ptRelation = GetLocationRelativeToSegment(p1, p2, virtualInter);
                    if (ptRelation == PointRelation.Inside ||
                        (infiniteLine &&
                        ((i == 0 && ptRelation == PointRelation.Before) ||
                        (i == Points.Count - 2 && ptRelation == PointRelation.After))
                        ))
                    {
                        segmentIndex = i;
                        intersection = virtualInter;
                        return true;
                    }
                }
            }
            return false;
        }

        public PointRelation GetLocationRelativeToSegment(Vector s1, Vector s2, Vector pt)
        {
            var dist = (s2 - s1).Length;
            var dist1 = (pt - s1).Length;
            var dist2 = (pt - s2).Length;

            if ((dist1 <= dist || dist1.EqualOrClose(dist, 0.0001)) && (dist2 <= dist || dist2.EqualOrClose(dist, 0.0001)))
                return PointRelation.Inside;
            else if ((s2 - s1).Normalized.EqualOrClose((pt - s1).Normalized, 0.001))
                return PointRelation.After;
            else if ((s1 - s2).Normalized.EqualOrClose((pt - s2).Normalized, 0.001))
                return PointRelation.Before;

            return PointRelation.Invalid;
        }

        public enum PointRelation
        {
            Invalid,
            Before,
            Inside,
            After
        }

        #endregion

        internal override void FlipHandedness()
        {
            base.FlipHandedness();
            var points = Points.ToList();
            Points.Clear();
            _Points.AddRange(points.Select(p => new PointM(p.X * -1, p.Y)));
        }
    }
}
