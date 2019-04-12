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
    public class LayoutPolyLine : VisualElement, ILayoutLine
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

        /// <summary>
        /// Total Length
        /// </summary>
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
            return Intersects(line, out intersection, out _, infiniteLine);
        }

        public bool Intersects(LayoutLine line, out PointM intersection, out int segmentIndex, bool infiniteLine = true)
        {
            intersection = PointM.Empty;

            if (Intersects(line.Equation, out Vector virtualInter, out segmentIndex, infiniteLine))
            {
                intersection = PointM.FromVector(virtualInter, Points.First().Unit);
                return true;
            }

            return false;
        }

        public bool Intersects(Line line, out Vector intersection, bool infiniteLine = true)
        {
            return Intersects(line, out intersection, out _, infiniteLine);
        }

        protected bool Intersects(Line line, out Vector intersection, out int segmentIndex, bool infiniteLine = true)
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

        protected PointRelation GetLocationRelativeToSegment(Vector s1, Vector s2, Vector pt)
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

        public bool Intersects(LayoutPolyLine line, out PointM intersection)
        {
            intersection = PointM.Empty;
            for (int i = 0; i < line.Points.Count - 1; i++)
            {
                var segLine = Line.FromPoints(line.Points[i].ToVector(), line.Points[i + 1].ToVector());
                if (Intersects(segLine, out Vector virtualInter, false))
                {

                    return true;
                }
            }

            return false;
        }

        #endregion

        public void MergePoints(double tolerence = 0.01)
        {
            for (int i = 0; i < Points.Count - 1; i++)
            {
                if (Points.Count - 1 > 3 && PointM.Distance(Points[i + 1], Points[i]).NormalizedValue < tolerence)
                    Points.RemoveAt(i--);
            }
        }

        #region Trim & Extend

        public void TrimStart(LayoutLine trimLine, bool extendIfNeeded = false)
        {
            int interIdx;
            PointM interPt = PointM.Empty;

            if (!Intersects(trimLine, out interPt, out interIdx, extendIfNeeded))
                return;

            var pointsToKeep = new List<PointM>();
            pointsToKeep.Add(interPt);

            for (int i = interIdx; i < Points.Count; i++)
            {
                if (interIdx == i)
                {
                    var ptRel = GetLocationRelativeToSegment(Points[i].ToVector(), Points[i + 1].ToVector(), interPt.ToVector());
                    if (ptRel != PointRelation.Before)
                        continue;
                }

                pointsToKeep.Add(Points[i]);
            }

            Points.Clear();
            _Points.AddRange(pointsToKeep);

            //remove points that are very close
            MergePoints(0.05);
        }

        public void TrimEnd(LayoutLine trimLine, bool extendIfNeeded = false)
        {
            int interIdx;
            PointM interPt = PointM.Empty;

            if (!Intersects(trimLine, out interPt, out interIdx, extendIfNeeded))
                return;

            var pointsToKeep = new List<PointM>();
            for (int i = 0; i <= interIdx; i++)
            {
                pointsToKeep.Add(Points[i]);

                if (interIdx == i)
                {
                    var ptRel = GetLocationRelativeToSegment(Points[i].ToVector(), Points[i + 1].ToVector(), interPt.ToVector());
                    if (ptRel == PointRelation.After)
                        pointsToKeep.Add(Points[i + 1]);
                }
            }

            pointsToKeep.Add(interPt);

            Points.Clear();
            _Points.AddRange(pointsToKeep);

            //remove points that are very close
            MergePoints(0.05);
        }

        public void TrimBetween(LayoutLine l1, LayoutLine l2, bool extendIfNeeded = false)
        {
            int inter1Idx, inter2Idx;
            PointM p1 = PointM.Empty;
            PointM p2 = PointM.Empty;

            bool canTrimStart = Intersects(l1, out p1, out inter1Idx, extendIfNeeded);
            bool canTrimEnd = Intersects(l2, out p2, out inter2Idx, extendIfNeeded);

            if (!canTrimStart || !canTrimEnd)
            {
                if (canTrimStart)
                    TrimStart(l1, extendIfNeeded);
                else if (canTrimEnd)
                    TrimEnd(l2, extendIfNeeded);
                return;
            }

            var pointsToKeep = new List<PointM>();
            pointsToKeep.Add(p1);

            for (int i = inter1Idx; i <= inter2Idx; i++)
            {
                if (inter1Idx == i)
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

            pointsToKeep.Add(p2);

            Points.Clear();
            _Points.AddRange(pointsToKeep);

            //remove points that are very close
            MergePoints(0.05);
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
