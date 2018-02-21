using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class FretSegment : LayoutLine
    {
        private int _FretIndex;
        private SIString _String;
        private PointM _PointOnString;
        public int FretIndex { get { return _FretIndex; } }
        public SIString String { get { return _String; } }
        public PointM PointOnString { get { return _PointOnString; } }
        /// <summary>
        /// Means that the strings does not have this fret.
        /// We still create the segment to help define the fret shape of neighbouring strings that has the fret
        /// </summary>
        public bool IsVirtual { get; set; }
        public bool IsNut { get { return String.StartingFret == FretIndex; } }

        public FretSegment(int index, SIString str, PointM p1, PointM p2) : base(p1, p2, VisualElementType.Fret)
        {
            _FretIndex = index;
            _String = str;
        }

        public FretSegment(int index, SIString str, PointM center, PointM left, PointM right) : base(left, right, VisualElementType.Fret)
        {
            _FretIndex = index;
            _String = str;
            _PointOnString = center;
        }

        public FretSegment Clone()
        {
            return new FretSegment(FretIndex, String, P1, P2)
            {
                _PointOnString = PointOnString,
                IsVirtual = true
            };
        }

        internal override void FlipHandedness()
        {
            base.FlipHandedness();
            _PointOnString = new PointM(_PointOnString.X * -1, _PointOnString.Y);
        }
    }
}
