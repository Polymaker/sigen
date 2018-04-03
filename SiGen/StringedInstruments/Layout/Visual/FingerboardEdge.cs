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

        public FingerboardEdge()
        {
            _ElementType = VisualElementType.FingerboardEdge;
        }

        public FingerboardEdge(PointM p1, PointM p2)
        {
            _ElementType = VisualElementType.FingerboardEdge;
            Points.Add(p1);
            Points.Add(p2);
        }

        public FingerboardEdge(IEnumerable<PointM> points)
        {
            _ElementType = VisualElementType.FingerboardEdge;
            foreach (var pt in points)
                Points.Add(pt);
        }
    }

    public class FingerboardSideEdge : LayoutLine, IFingerboardEdge
    {
        private FingerboardSide _Side;

        public FingerboardSide Side { get { return _Side; } }

        public bool IsSideEdge
        {
            get { return true; }
        }

        public FingerboardSideEdge(PointM p1, PointM p2, FingerboardSide side) : base(p1, p2, VisualElementType.FingerboardEdge)
        {
            _Side = side;
        }

    }
}
