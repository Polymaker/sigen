using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public interface IFingerboardEdge
    {
        PointM GetIntersection(LayoutLine line);
        bool IsSideEdge { get; }
    }

    public class FingerboardEdge : LayoutPolyLine, IFingerboardEdge
    {
        private bool _IsSideEdge;

        public bool IsSideEdge
        {
            get { return _IsSideEdge; }
            set { _IsSideEdge = value; }
        }

        public override VisualElementType ElementType => VisualElementType.FingerboardEdge;

        public FingerboardEdge() { }

        public FingerboardEdge(PointM p1, PointM p2)
        {
            Points.Add(p1);
            Points.Add(p2);
        }

        public FingerboardEdge(IEnumerable<PointM> points)
        {
            foreach (var pt in points)
                Points.Add(pt);
        }
    }

    public class FingerboardSideEdge : LayoutLine, IFingerboardEdge, IStringBoundary
    {
        private FingerboardSide _Side;

        public FingerboardSide Side { get { return _Side; } }

        public PointM RealEnd { get; set; }

        public bool IsSideEdge
        {
            get { return true; }
        }

        public override VisualElementType ElementType => VisualElementType.FingerboardEdge;

        public FingerboardSideEdge(PointM p1, PointM p2, FingerboardSide side) : base(p1, p2)
        {
            _Side = side;
            RealEnd = PointM.Empty;
        }

        public PointM GetRelativePoint(StringLine str, PointM pos)
        {
            PointM startPt = PointM.Empty;
            if (str.Index == 0)
                startPt = Side == FingerboardSide.Treble ? str.P1 : str.FretZero;
            else if (str.Index == Layout.NumberOfStrings - 1)
                startPt = Side == FingerboardSide.Bass ? str.P1 : str.FretZero;

            var strPosRatio = PointM.Distance(pos, str.P2) / PointM.Distance(startPt, str.P2);
            var endPt = RealEnd.IsEmpty ? P2 : RealEnd;
            var dist = PointM.Distance(P1, endPt);
            return endPt + (Direction * -1 * (dist * strPosRatio));
        }
    }
}
