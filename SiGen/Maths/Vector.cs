using System;

namespace SiGen.Maths
{
    public struct Vector
    {

        #region Static Consts

        public static readonly Vector Zero;
        public static readonly Vector One = new Vector(1, 1);
        public static readonly Vector Empty = new Vector(double.NaN, double.NaN);

        #endregion

        #region Fields

        private double x;
        private double y;

        #endregion

        #region Properties

        [System.ComponentModel.Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return double.IsNaN(x) && double.IsNaN(y);
            }
        }

        public double X
        {
            get { return x; }
            set { x = value; }
        }

        public double Y
        {
            get { return y; }
            set { y = value; }
        }

        public double Length
        {
            get
            {
                if (IsEmpty)
                    return double.NaN;
                if (this == Zero)
                    return 0;
                return Math.Abs(Math.Sqrt((X * X) + (Y * Y)));
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

        public bool EqualOrClose(Vector vec, double tolerence)
        {
            return EqualOrClose(this, vec, tolerence);
        }

        public static bool EqualOrClose(Vector v1, Vector v2)
        {
            if (v1.IsEmpty || v2.IsEmpty)
                return v1.IsEmpty == v2.IsEmpty;
            return v1.X.EqualOrClose(v2.X) && v1.Y.EqualOrClose(v2.Y);
        }

        public static bool EqualOrClose(Vector v1, Vector v2, double tolerence)
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

        public static Vector operator /(Vector pt, double value)
        {
            return new Vector(pt.X / value, pt.Y / value);
        }

        public static Vector operator *(Vector pt, double value)
        {
            return new Vector(pt.X * value, pt.Y * value);
        }

        public static Vector operator *(double value, Vector pt)
        {
            return new Vector(pt.X * value, pt.Y * value);
        }

        #endregion
    }
}
