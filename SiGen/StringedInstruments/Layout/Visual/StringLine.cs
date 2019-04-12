using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class StringLine : LayoutLine
    {
        private int _StringIndex;
        public SIString String { get; }

        public int Index => _StringIndex;

        public PointM FretZero { get; set; }

        public override VisualElementType ElementType => VisualElementType.String;

        public StringLine(SIString str, PointM p1, PointM p2) : base(p1, p2)
        {
            String = str;
            _StringIndex = str.Index;
            FretZero = p1;
        }

        internal override void FlipHandedness()
        {
            base.FlipHandedness();
            if (!FretZero.IsEmpty)
                FretZero = new PointM(FretZero.X * -1, FretZero.Y);
        }
    }
}
