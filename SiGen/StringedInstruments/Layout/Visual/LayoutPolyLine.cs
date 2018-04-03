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
                return IntersectSegmentWithLine(line, 0, 1, out intersection, SegmentHitBounds.Any);
            }
            else
            {
                for (int i = 0; i < Points.Count - 1; i++)
                {
                    var hitFlags = (i == 0 ? SegmentHitBounds.AllowLeft : (i == Points.Count - 2 ? SegmentHitBounds.AllowRight : SegmentHitBounds.InBounds));
                    if (IntersectSegmentWithLine(line, i, i + 1, out intersection, hitFlags))
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
            Any = 4
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
