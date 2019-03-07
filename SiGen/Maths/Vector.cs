using System;

namespace SiGen.Maths
{
    public struct Vector
    {

        #region Static Consts

        public static readonly Vector Zero;
        public static readonly Vector One = new Vector(1, 1);
        public static readonly Vector Empty = new Vector(PreciseDouble.NaN, PreciseDouble.NaN);

        #endregion

        #region Fields

        private PreciseDouble x;
        private PreciseDouble y;

        #endregion

        #region Properties

        [System.ComponentModel.Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return PreciseDouble.IsNaN(x) && PreciseDouble.IsNaN(y);
            }
        }

        public PreciseDouble X
        {
            get { return x; }
            set { x = value; }
        }

        public PreciseDouble Y
        {
            get { return y; }
            set { y = value; }
        }

        public PreciseDouble Length
        {
            get
            {
                if (IsEmpty)
                    return PreciseDouble.NaN;
                if (this == Zero)
                    return 0;
                return MathP.Abs(MathP.Sqrt((X * X) + (Y * Y)));
            }
        }

        public Vector Normalized
        {
            get
            {
                if (IsEmpty || this == Zero)
                    return Vector.Zero;
                return this / Length;
            }
        }

		#endregion

		#region Ctors

		public Vector(PreciseDouble x, PreciseDouble y)
		{
			this.x = x;
			this.y = y;
		}

		public Vector(double x, double y)
        {
            this.x = x;
            this.y = y;
        }

        public Vector(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        #endregion

        #region Equality operators

        public static bool operator ==(Vector left, Vector right)
        {
            if (left.IsEmpty && right.IsEmpty)
                return true;
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(Vector left, Vector right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Vector))
                return false;
            Vector vec = (Vector)obj;
            if (IsEmpty || vec.IsEmpty)
                return IsEmpty == vec.IsEmpty;
            return vec.X == X && vec.Y == Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public bool EqualOrClose(Vector vec)
        {
            return EqualOrClose(this, vec);
        }

        public bool EqualOrClose(Vector vec, PreciseDouble tolerence)
        {
            return EqualOrClose(this, vec, tolerence);
        }

        public static bool EqualOrClose(Vector v1, Vector v2)
        {
            if (v1.IsEmpty || v2.IsEmpty)
                return v1.IsEmpty == v2.IsEmpty;
            return v1.X.EqualOrClose(v2.X) && v1.Y.EqualOrClose(v2.Y);
        }

        public static bool EqualOrClose(Vector v1, Vector v2, PreciseDouble tolerence)
        {
            if (v1.IsEmpty || v2.IsEmpty)
                return v1.IsEmpty == v2.IsEmpty;
            return v1.X.EqualOrClose(v2.X, tolerence) && v1.Y.EqualOrClose(v2.Y, tolerence);
        }

        #endregion

        #region Arithmetic operators

        public static Vector operator +(Vector pt1, Vector pt2)
        {
            return new Vector(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static Vector operator -(Vector pt1, Vector pt2)
        {
            return new Vector(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static Vector operator /(Vector pt, PreciseDouble value)
        {
            return new Vector(pt.X / value, pt.Y / value);
        }

        public static Vector operator /(Vector pt1, Vector pt2)
        {
            return new Vector(pt1.X / pt2.X, pt1.Y / pt2.Y);
        }

        public static Vector operator *(Vector pt, PreciseDouble value)
        {
            return new Vector(pt.X * value, pt.Y * value);
        }

        public static Vector operator *(PreciseDouble value, Vector pt)
        {
            return new Vector(pt.X * value, pt.Y * value);
        }

        public static Vector operator *(Vector pt1, Vector pt2)
        {
            return new Vector(pt1.X * pt2.X, pt1.Y * pt2.Y);
        }

        public static explicit operator System.Drawing.PointF(Vector vec)
        {
            return new System.Drawing.PointF((float)vec.x, (float)vec.y);
        }

        public static explicit operator Vector(System.Drawing.PointF pt)
        {
            return new Vector(pt.X, pt.Y);
        }

        public static PreciseDouble Dot(Vector left, Vector right)
        {
            return left.X * right.X + left.Y * right.Y;
        }

        #endregion

        public override string ToString()
        {
            return string.Format("[{0};{1}]", X, Y);
        }
    }
}
