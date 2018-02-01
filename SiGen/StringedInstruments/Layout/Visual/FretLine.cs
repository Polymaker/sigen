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
            get
            {
                return RectangleM.Empty;
            }
        }

        public bool IsStraight
        {
            get { return _IsStraight; }
        }

        public FretLine(IEnumerable<FretSegment> segments)
        {
            _Segments = new List<FretSegment>(segments.OrderBy(s=>s.String.Index));
            _Points = new List<PointM>();
        }

        public void VerifyIsStraight()
        {
            _IsStraight = true;
            if (Segments.Count > 2)
            {
                Vector dir = Vector.Empty;
                for(int i = 0; i < Segments.Count - 1; i++)
                {
                    if (dir.IsEmpty)
                        dir = (Segments[i + 1].PointOnString - Segments[i].PointOnString).ToVector();
                    else
                    {
                        var curDir = (Segments[i + 1].PointOnString - Segments[i].PointOnString).ToVector();
                        if (!Vector.EqualOrClose(dir,curDir, 0.00001))
                        {
                            _IsStraight = false;
                            break;
                        }
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
                _Points.Add(Segments.First().P2);//first segment is toward treble side so edge is at right (P2)
                _Points.AddRange(Segments.Select(s => s.PointOnString));
                _Points.Add(Segments.Last().P1);//last segment is toward bass side so edge is at left (P1)
            }
        }
    }
}
