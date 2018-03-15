using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class FingerboardEdge : LayoutLine
    {
        public FingerboardEdge(PointM p1, PointM p2) : base(p1,p2, VisualElementType.FingerboardEdge)
        {

        }
    }

    public class FingerboardSideEdge : FingerboardEdge
    {
        private FingerboardSide _Side;

        public FingerboardSide Side { get { return _Side; } }

        public FingerboardSideEdge(PointM p1, PointM p2, FingerboardSide side) : base(p1, p2)
        {
            _Side = side;
        }

    }
}
