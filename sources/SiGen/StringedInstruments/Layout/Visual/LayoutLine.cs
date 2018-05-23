using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class LayoutLine : VisualElement
    {
        private bool isDirty;
        private Vector _Direction;
        private Line _Equation;
        private RectangleM _Bounds;
        private PointM _P1;
        private PointM _P2;
        private Measure _Length;
        
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

        public LayoutLine() { }

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

        public PointM SnapToLine(PointM pos, bool horizontally = false)
        {
            if (horizontally)
                return PointM.FromVector(Equation.GetPointForY(pos.Y.NormalizedValue), P1.Unit ?? P2.Unit);

            var perp = Line.GetPerpendicular(Equation, (Vector)pos);
            Vector inter;
            if (Equation.Intersect(perp, out inter))
                return new PointM(inter.X, inter.Y, P1.Unit ?? P2.Unit);
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

        public PointM GetIntersection(Line line)
        {
            var inter = Equation.GetIntersection(line);
            return PointM.FromVector(inter, P1.Unit);
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
    }
}
