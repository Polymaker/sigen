using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class StringCenter : LayoutLine
    {
        private StringLine _Left;
        private StringLine _Right;
        /// <summary>
        /// Towwards Bass
        /// </summary>
        public StringLine Left { get { return _Left; } }
        /// <summary>
        /// Towards Treble
        /// </summary>
        public StringLine Right { get { return _Right; } }

        public override VisualElementType ElementType
        {
            get
            {
                return  VisualElementType.StringCenter;
            }
        }

        public StringCenter(StringLine left, StringLine right)
        {
            _Left = left;
            _Right = right;
            P1 = PointM.Average(left.P1, right.P1);
            P2 = PointM.Average(left.P2, right.P2);

            //P1 = PointM.FromVector(Equation.GetPointForY(Math.Min(left.P1.Y.NormalizedValue, right.P1.Y.NormalizedValue)), P1.Unit);

            //P1 = PointM.Average(left.FretZero, right.FretZero);
            //P2 = PointM.Average(left.P2, right.P2);
            //var nutLine = Line.FromPoints((Vector)left.P1, (Vector)right.P1);

            //P1 = PointM.FromVector(Equation.GetIntersection(nutLine), P1.Unit ?? P2.Unit);
        }
    }
}
