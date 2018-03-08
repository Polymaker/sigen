namespace SiGen.Maths
{
    public struct Line
    {
        #region Fields

        private bool _IsVertical;
        private double _A;
        private double _B;
        private double _X;

        #endregion

        #region Properties

        public double A { get { return _A; } }

        public double B { get { return _B; } }

        public double X { get { return _X; } }

        public bool IsVertical { get { return _IsVertical; } }

        public bool IsHorizontal { get { return !IsVertical && A.EqualOrClose(0); } }

        public Vector Vector
        {
            get
            {
                if (IsVertical)
                    return new Vector(0, 1);
                return new Vector(1, A).Normalized;
            }
        }

        #endregion

        #region Ctors

        public Line(double x)
        {
            _IsVertical = true;
            _X = x;
            _A = 0;
            _B = 0;
        }

        public Line(double a, double b)
        {
            _IsVertical = false;
            _X = 0;
            _A = a;
            _B = b;
        }

        #endregion

        #region Static Ctors

        public static Line FromPoints(Vector p1, Vector p2)
        {
            var left = p1.X < p2.X ? p1 : p2;
            var right = p1.X < p2.X ? p2 : p1;
            var dx = right.X - left.X;
            var dy = right.Y - left.Y;

            if (dx.EqualOrClose(0, 0.00001))
                return new Line(p1.X);//vertical line

            var slope = dy / dx;

            if (double.IsInfinity(slope))
                return new Line(p1.X);//vertical line

            var b = left.Y + ((left.X * -1) * slope);
            
            if (slope.EqualOrClose(0, 0.00001))
                slope = 0;//horizontal line

            return new Line(slope, b);
        }

        #endregion

        #region Equality operators

        public static bool operator ==(Line left, Line right)
        {
            if (left.IsVertical != right.IsVertical)
                return false;
            if (left.IsVertical)
                return NumberHelper.EqualOrClose(left.X, right.X, 0.001);

            return NumberHelper.EqualOrClose(left.A, right.A, 0.0001) && NumberHelper.EqualOrClose(left.B, right.B, 0.0001);
        }

        public static bool operator !=(Line left, Line right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Line))
                return false;

            return this == (Line)obj;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Functions

        public Vector GetPointForX(double x)
        {
            if (IsVertical)
                return Vector.Empty;
            return new Vector(x, B + (x * A));
        }

        public Vector GetPointForY(double y)
        {
            if (IsVertical)
                return new Vector(X, y);
            else if (IsHorizontal)
                return Vector.Empty;
            return new Vector((y - B) / A, y);
        }

        public static bool AreParallel(Line l1, Line l2)
        {
            if (l1.IsVertical || l2.IsVertical)
                return l1.IsVertical && l2.IsVertical;

            return NumberHelper.EqualOrClose(l1.A, l2.A);
        }

        public static bool ArePerpendicular(Line l1, Line l2)
        {
            if ((l1.IsHorizontal && l2.IsVertical) || (l2.IsHorizontal && l1.IsVertical))
                return true;
            return NumberHelper.EqualOrClose((1d / l1.A) * -1, l2.A) || NumberHelper.EqualOrClose((1d / l2.A) * -1, l1.A);
        }

        public static Line GetPerpendicular(Line line, Vector pt)
        {
            if (line.IsVertical)
                return new Line(0, pt.Y);
            if (line.IsHorizontal)
                return new Line(pt.X);
            var newSlope = (1 / line.A) * -1;
            var b = pt.Y - (pt.X * newSlope);
            return new Line(newSlope, b);
        }

        public Line GetPerpendicular(Vector pt)
        {
            return GetPerpendicular(this, pt);
        }

        public static bool LinesIntersect(Line l1, Line l2, out Vector intersection)
        {
            intersection = Vector.Empty;
            if (AreParallel(l1, l2))
                return false;
            if (l1.IsVertical)
            {
                intersection = l2.GetPointForX(l1.X);
                return true;
            }
            else if (l2.IsVertical)
            {
                intersection = l1.GetPointForX(l2.X);
                return true;
            }
            intersection = new Vector((l2.B - l1.B) / (l1.A - l2.A), l1.A * (l2.B - l1.B) / (l1.A - l2.A) + l1.B);
            return true;
        }

        public bool Intersect(Line line, out Vector intersection)
        {
            return LinesIntersect(this, line, out intersection);
        }

        public bool Intersect(Line line)
        {
            Vector dummy;
            return LinesIntersect(this, line, out dummy);
        }

        public Vector GetIntersection(Line line)
        {
            Vector intersection;
            Intersect(line, out intersection);
            return intersection;
        }

        public Vector GetClosestPointOnLine(Vector point)
        {
            var perp = GetPerpendicular(this, point);
            Vector inter;
            if (Intersect(perp, out inter))
                return inter;
            return Vector.Empty;
        }

        #endregion
    }
}
