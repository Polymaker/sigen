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

        public List<FretSegment> Segments
        {
            get { return _Segments; }
        }

        public List<PointM> Points
        {
            get { return _Points; }
        }

        public int FretIndex { get { return _Segments[0].FretIndex; } }

        public IEnumerable<SIString> Strings { get { return _Segments.Select(s => s.String); } }

        public override RectangleM Bounds
        {
            get { return _Bounds; }
        }

        public bool IsStraight
        {
            get { return _IsStraight; }
        }

        public FretLine(IEnumerable<FretSegment> segments)
        {
            _Segments = new List<FretSegment>(segments.OrderBy(s=>s.String.Index));
            _Points = new List<PointM>();
            _Bounds = RectangleM.Empty;
        }

        public void VerifyIsStraight()
        {
            _IsStraight = true;
            if (Segments.Count > 2)
            {
                Angle avgAngle = Angle.FromPoints(Segments[0].PointOnString.ToVector(), Segments[Segments.Count - 1].PointOnString.ToVector());
                avgAngle.Normalize();

                for (int i = 0; i < Segments.Count - 1; i++)
                {
                    var curAngle = Angle.FromPoints(Segments[i].PointOnString.ToVector(), Segments[i + 1].PointOnString.ToVector());
                    curAngle.Normalize();
                    if (Math.Abs(curAngle.Degrees - avgAngle.Degrees) > 0.9)
                    {
                        _IsStraight = false;
                        break;
                    }
                }

            }
            
        }

        public void BuildLayout()
        {
            VerifyIsStraight();
            var layout = Segments[0].String.Layout;
            if (IsStraight && Segments.Count > 1)
            {
                var leftBound = layout.GetStringBoundaryLine(Segments.Last(fs=>!fs.IsVirtual).String, FingerboardSide.Bass);
                var rightBound = layout.GetStringBoundaryLine(Segments.First(fs => !fs.IsVirtual).String, FingerboardSide.Treble);

                var line = Line.FromPoints(Segments.First().PointOnString.ToVector(), Segments.Last().PointOnString.ToVector());
                _Points.Add(PointM.FromVector(line.GetIntersection(leftBound.Equation), UnitOfMeasure.Centimeters));
                _Points.Add(PointM.FromVector(line.GetIntersection(rightBound.Equation), UnitOfMeasure.Centimeters));
            }
            else
            {
                _Points.Add(Segments.First(fs => !fs.IsVirtual).P2);//first segment is toward treble side so edge is at right (P2)
                foreach(var seg in Segments.Where(s => !s.IsVirtual))
                {
                    _Points.Add(PointM.Average(seg.P2, seg.PointOnString));
                    _Points.Add(PointM.Average(seg.P1, seg.PointOnString));
                }
                //_Points.AddRange(Segments.Where(fs => !fs.IsVirtual).Select(fs => fs.PointOnString));
                _Points.Add(Segments.Last(fs => !fs.IsVirtual).P1);//last segment is toward bass side so edge is at left (P1)
            }
            _Bounds = RectangleM.BoundingRectangle(Points);
        }
    }
}
