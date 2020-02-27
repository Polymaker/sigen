using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class StringCenter : LayoutLine, IStringBoundary
    {

        /// <summary>
        /// Towwards Bass
        /// </summary>
        public StringLine Left { get; }
        /// <summary>
        /// Towards Treble
        /// </summary>
        public StringLine Right { get; }

        public override VisualElementType ElementType => VisualElementType.StringMidline;

        public PointM P0 { get; private set; }

        public StringCenter(StringLine left, StringLine right)
        {
            Left = left;
            Right = right;
            P1 = PointM.Average(left.P1, right.P1);
            P2 = PointM.Average(left.P2, right.P2);
            P0 = PointM.Average(left.FretZero, right.FretZero);
            //P1 = PointM.FromVector(Equation.GetPointForY(Math.Min(left.P1.Y.NormalizedValue, right.P1.Y.NormalizedValue)), P1.Unit);

            //P1 = PointM.Average(left.FretZero, right.FretZero);
            //P2 = PointM.Average(left.P2, right.P2);
            //var nutLine = Line.FromPoints((Vector)left.P1, (Vector)right.P1);

            //P1 = PointM.FromVector(Equation.GetIntersection(nutLine), P1.Unit ?? P2.Unit);
        }

        public PointM GetRelativePoint(StringLine str, PointM pos)
        {
            //if (!(str == Left || str == Right))
            //    return PointM.Empty;
            var strPosRatio = PointM.Distance(pos, str.P2) / PointM.Distance(str.FretZero, str.P2);
            var dist = PointM.Distance(P0, P2);
            return P2 + (Direction * -1 * (dist * strPosRatio));
        }
    }
}
