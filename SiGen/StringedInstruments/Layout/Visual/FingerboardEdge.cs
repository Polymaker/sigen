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
        private SIString _BesideString;

        public FingerboardSide Side { get { return _Side; } }
        public SIString BesideString
        {
            get
            {
                if (_BesideString == null)
                    return Side == FingerboardSide.Bass ? Layout.LastString : Layout.FirstString;
                return _BesideString;
            }
        }

        public FingerboardSideEdge(PointM p1, PointM p2, FingerboardSide side) : base(p1, p2)
        {
            _Side = side;
        }

        public FingerboardSideEdge(PointM p1, PointM p2, SIString str, FingerboardSide side) : base(p1, p2)
        {
            _Side = side;
            _BesideString = str;
        }

        public bool IsAtSideOf(FingerboardSide side, SIString str)
        {
            return Side == side && ((str == null && _BesideString == null) || BesideString == str);
        }
    }
}
