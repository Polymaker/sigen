using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class LayoutIntersection
    {
        public ILayoutLine Item1 { get; }
        public ILayoutLine Item2 { get; }

        public VisualElement Element1 => Item1 as VisualElement;
        public VisualElement Element2 => Item2 as VisualElement;

        public VisualElementType ElementType1 => Element1.ElementType;
        public VisualElementType ElementType2 => Element2?.ElementType ?? VisualElementType.Unknown;

        public PointM Intersection { get; }
        public Vector WorldCoord { get; }

        public bool IsSinglePoint => Item2 == null;

        public LayoutIntersection(ILayoutLine item1, ILayoutLine item2, PointM intersection)
        {
            Item1 = item1;
            Item2 = item2;
            Intersection = intersection;
            WorldCoord = Intersection.ToVector();
        }

        public LayoutIntersection(ILayoutLine item1, PointM intersection)
        {
            Item1 = item1;
            Intersection = intersection;
            WorldCoord = Intersection.ToVector();
        }

        public PreciseDouble GetDistance(Vector worldPos)
        {
            return (WorldCoord - worldPos).Length;
        }

        public Measure GetDistance(PointM point)
        {
            return Measure.FromNormalizedValue(GetDistance(point.ToVector()), Intersection.Unit ?? point.Unit);
        }

        public bool IsSimilar(LayoutIntersection other)
        {
            if (IsSinglePoint != other.IsSinglePoint)
                return false;

            ILayoutLine line1 = other.Item1, line2 = other.Item2;

            if (ElementType1 == other.ElementType2 && ElementType2 == other.ElementType1)
            {
                line1 = other.Item2;
                line2 = other.Item1;
            }
            else if (!(ElementType1 == other.ElementType1 && ElementType2 == other.ElementType2))
                return false;

            if (!AreSimilar(Item1, line1, ElementType1))
                return false;

            if (!IsSinglePoint && !AreSimilar(Item2, line2, ElementType2))
                return false;

            return true;
        }

        private bool AreSimilar(ILayoutLine line1, ILayoutLine line2, VisualElementType lineType)
        {
            switch (lineType)
            {
                case VisualElementType.Fret:
                    return (line1 as FretLine).FretIndex == (line2 as FretLine).FretIndex;
                case VisualElementType.String:
                    return (line1 as StringLine).Index == (line2 as StringLine).Index;
                case VisualElementType.StringCenter:
                    return (line1 as StringCenter).Right.Index == (line2 as StringCenter).Right.Index &&
                        (line1 as StringCenter).Left.Index == (line2 as StringCenter).Left.Index;
                case VisualElementType.FingerboardEdge:
                    {
                        if (line1 is FingerboardSideEdge side1 && line2 is FingerboardSideEdge side2)
                            return side1.Side == side2.Side;
                        break;
                    }

            }
            return false;
        }
    }
}
