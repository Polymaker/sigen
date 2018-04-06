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

        public void InterpolateCurve()
        {
            if (Points.Count < 3)
                return;

            var basePoints = Points.ToArray();
            var finalPoints = new List<PointM>();

            for(int i = 0; i < basePoints.Length - 2; i++)
            {
                var p1 = basePoints[i];
                var p2 = basePoints[i + 1];
                var p3 = basePoints[i + 2];

                var l1 = new LayoutLine(p1, p2);
                var l2 = new LayoutLine(p3, p2);

                var m1 = PointM.Average(p1, p2);
                var m2 = PointM.Average(p2, p3);

                l1 = new LayoutLine(m1, l1.GetPerpendicularPoint(m1, Measure.Cm(1)));
                l2 = new LayoutLine(m2, l2.GetPerpendicularPoint(m2, Measure.Cm(1)));

                var dir = (m2.ToVector() - m1.ToVector()).Normalized;
                var tan = new LayoutLine(p2, p2 + (dir * Measure.Cm(1)));

                var c1 = l1.GetIntersection(tan);
                var c2 = l2.GetIntersection(tan);

                finalPoints.Add(p1);
                finalPoints.Add(PointM.Average(m1, c1));
                if (i == basePoints.Length - 3)
                {
                    finalPoints.Add(p2);
                    finalPoints.Add(PointM.Average(m2, c2));
                    finalPoints.Add(p3);
                }
            }
            _Points.Clear();
            _Points.AddRange(finalPoints);
        }

        #region Intersection

        public PointM GetIntersection(LayoutLine other)
        {
            var intersection = PointM.Empty;
            Intersects(other, out intersection);
            return intersection;
        }

        public bool Intersects(LayoutLine line, out PointM intersection)
        {
            return Intersects(line.Equation, out intersection);
        }

        public bool Intersects(Line line, out PointM intersection)
        {
            intersection = PointM.Empty;
            if (Points.Count == 2)
            {
                bool flipOrientation = (Points[1].X < Points[0].X);
                return IntersectSegmentWithLine(line, flipOrientation ? 1 : 0, flipOrientation ? 0 : 1, out intersection, SegmentHitBounds.Any);
            }
            else
            {
                for (int i = 0; i < Points.Count - 1; i++)
                {

                    SegmentHitBounds hitFlags = SegmentHitBounds.InBounds; // (i == 0 ? SegmentHitBounds.AllowLeft : (i == Points.Count - 2 ? SegmentHitBounds.AllowRight : SegmentHitBounds.InBounds));
                    bool flipOrientation = (Points[i + 1].X < Points[i].X);

                    if (i == 0)
                        hitFlags = !flipOrientation ? SegmentHitBounds.AllowLeft : SegmentHitBounds.AllowRight;
                    else if(i == Points.Count - 2)
                        hitFlags = !flipOrientation ? SegmentHitBounds.AllowRight : SegmentHitBounds.AllowLeft;

                    if (IntersectSegmentWithLine(line, i + (flipOrientation ? 1 : 0), i + (flipOrientation ? 0 : 1), out intersection, hitFlags))
                        return true;
                }
            }
            return false;
        }

        public bool Intersects(LayoutPolyLine line, out PointM intersection)
        {
            intersection = PointM.Empty;

            for (int i = 0; i < line.Points.Count - 1; i++)
            {
                var segLine = Line.FromPoints(line.Points[i].ToVector(), line.Points[i + 1].ToVector());
                if (Intersects(segLine, out intersection))
                    return true;
            }

            return false;
        }

        private bool IntersectSegmentWithLine(Line line, int idx1, int idx2, out PointM inter, SegmentHitBounds flags)
        {
            var segLine = Line.FromPoints(Points[idx1].ToVector(), Points[idx2].ToVector());
            Vector virtualInter;
            inter = PointM.Empty;
            if (segLine.Intersect(line, out virtualInter))
            {
                if ((virtualInter.X >= Points[idx1].X.NormalizedValue || flags.HasFlag(SegmentHitBounds.AllowLeft)) &&
                    (virtualInter.X <= Points[idx2].X.NormalizedValue || flags.HasFlag(SegmentHitBounds.AllowRight)))
                {
                    inter = PointM.FromVector(virtualInter, UnitOfMeasure.Centimeters);
                    return true;
                }
            }

            return false;
        }

        [Flags]
        private enum SegmentHitBounds
        {
            InBounds = 0,
            AllowLeft = 1,
            AllowRight = 2,
            Any = 3
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
