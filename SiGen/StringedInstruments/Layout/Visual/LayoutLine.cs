using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class LayoutLine : VisualElement, ILayoutLine
    {
        private bool isDirty;
        private Vector _Direction;
        private Line _Equation;
        private RectangleM _Bounds;
        private PointM _P1;
        private PointM _P2;
        private Measure _Length;
        private VisualElementType _ElementType;

        public PointM P1
        {
            get { return _P1; }
            set
            {
                _P1 = value;
                isDirty = true;
            }
        }

        public PointM P2
        {
            get { return _P2; }
            set
            {
                _P2 = value;
                isDirty = true;
            }
        }

        public Vector Direction
        {
            get
            {
                if (isDirty)
                    UpdateInfos();
                return _Direction;
            }
        }

        public Measure Length
        {
            get
            {
                if (isDirty)
                    UpdateInfos();
                return _Length;
            }
        }

        public override RectangleM Bounds
        {
            get
            {
                if (isDirty)
                    UpdateInfos();
                return _Bounds;
            }
        }

        public Line Equation
        {
            get
            {
                if (isDirty)
                    UpdateInfos();
                return _Equation;
            }
        }

        public override VisualElementType ElementType => _ElementType;

        public LayoutLine()
        {
            _ElementType = VisualElementType.GuideLine;
        }

        public LayoutLine(PointM p1, PointM p2)
        {
            P1 = p1;
            P2 = p2;
            _ElementType = VisualElementType.GuideLine;
        }

        public LayoutLine(PointM p1, PointM p2, VisualElementType type)
        {
            P1 = p1;
            P2 = p2;
            _ElementType = type;
        }

        private void UpdateInfos()
        {
            _Direction = ((Vector)P2 - (Vector)P1).Normalized;
            _Equation = Line.FromPoints((Vector)P1, (Vector)P2);
            _Bounds = RectangleM.FromLTRB(Measure.Min(P1.X, P2.X), Measure.Max(P1.Y, P2.Y), Measure.Max(P1.X, P2.X), Measure.Min(P1.Y, P2.Y));
            _Length = Measure.FromNormalizedValue(((Vector)P2 - (Vector)P1).Length, P1.Unit ?? P2.Unit);
            isDirty = false;
        }

        public Vector SnapToLine(Vector pos, LineSnapDirection snapMode = LineSnapDirection.Perpendicular, bool infiniteLine = true)
        {
            Vector result = Vector.Empty;

            if (snapMode == LineSnapDirection.Horizontal)
            {
                result = Equation.GetPointForY(pos.Y);
            }
            else if (snapMode == LineSnapDirection.Vertical)
            {
                result = Equation.GetPointForX(pos.X);
            }
            else
            {
                var perp = Line.GetPerpendicular(Equation, pos);
                Equation.Intersect(perp, out result);
            }
            
            if (!result.IsEmpty && !infiniteLine)
            {
                var v1 = result - P1.ToVector();
                var v2 = result - P2.ToVector();

                if (!v1.Normalized.EqualOrClose(Direction, 0.0001))
                    return snapMode == LineSnapDirection.Perpendicular ? P1.ToVector() : Vector.Empty;
                else if (v1.Length > Length.NormalizedValue)
                    return snapMode == LineSnapDirection.Perpendicular ? P2.ToVector() : Vector.Empty;

                if (!v2.Normalized.EqualOrClose(Direction * -1, 0.0001))
                    return snapMode == LineSnapDirection.Perpendicular ? P2.ToVector() : Vector.Empty;
                else if (v2.Length > Length.NormalizedValue)
                    return snapMode == LineSnapDirection.Perpendicular ? P1.ToVector() : Vector.Empty;

            }

            return result;
        }

        public PointM SnapToLine(PointM pos, LineSnapDirection snapMode = LineSnapDirection.Perpendicular, bool infiniteLine = true)
        {
            var result = SnapToLine(pos.ToVector(), snapMode, infiniteLine);
            if (!result.IsEmpty)
                return PointM.FromVector(result, pos.Unit ?? P1.Unit ?? P2.Unit);
            return PointM.Empty;
        }

        public PointM GetPerpendicularPoint(PointM pos, Measure dist)
        {
            var perp = Maths.Line.GetPerpendicular(Equation, (Vector)pos);
            return pos + perp.Vector * dist;
        }

        public PointM GetIntersection(LayoutLine line)
        {
            var inter = Equation.GetIntersection(line.Equation);
            return PointM.FromVector(inter, P1.Unit);
        }

        public Vector GetIntersection(Line line)
        {
            return Equation.GetIntersection(line);
        }

        public static LayoutLine Offset(LayoutLine line, Measure amount)
        {
            var p1 = line.GetPerpendicularPoint(line.P1, amount);
            var p2 = line.GetPerpendicularPoint(line.P2, amount);
            return new LayoutLine(p1, p2);
        }

        internal override void FlipHandedness()
        {
            base.FlipHandedness();
            P1 = new PointM(P1.X * -1, P1.Y);
            P2 = new PointM(P2.X * -1, P2.Y);
        }

        public bool Intersects(LayoutLine line, out PointM intersection, bool infiniteLine = true)
        {
            intersection = PointM.Empty;

            if (Intersects(line.Equation, out Vector inter, infiniteLine))
            {
                intersection = PointM.FromVector(inter, P1.Unit ?? P2.Unit);
                return true;
            }

            return false;
        }

        public bool Intersects(Line line, out Vector intersection, bool infiniteLine = true)
        {
            intersection = Equation.GetIntersection(line);

            if (intersection.IsEmpty)
                return false;
            else if (infiniteLine)
                return true;


            var v1 = intersection - P1.ToVector();
            var v2 = intersection - P2.ToVector();

            if (!v1.Normalized.EqualOrClose(Direction, 0.0001) || v1.Length > Length.NormalizedValue)
                return false;

            if (!v2.Normalized.EqualOrClose(Direction * -1, 0.0001) || v2.Length > Length.NormalizedValue)
                return false;

            return true;
        }

        public PointM[] GetLinePoints()
        {
            return new PointM[] { P1, P2 };
        }
    }
}
