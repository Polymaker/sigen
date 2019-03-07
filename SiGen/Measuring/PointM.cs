using SiGen.Maths;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Measuring
{
    public struct PointM
    {

        #region Static Consts

        public static readonly PointM Empty = new PointM(Measure.Empty, Measure.Empty);
        public static readonly PointM Zero = new PointM();

        #endregion

        #region Fields

        private Measure x;
        private Measure y;

        #endregion

        #region Properties

        public Measure X
        {
            get { return x; }
            set { x = value; }
        }

        public Measure Y
        {
            get { return y; }
            set { y = value; }
        }

        public UnitOfMeasure Unit
        {
            get { return X.Unit ?? Y.Unit; }
            set
            {
                x.Unit = value;
                y.Unit = value;
            }
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return x.IsEmpty && y.IsEmpty;
            }
        }

        public Vector Direction
        {
            get { return new Vector(X.NormalizedValue, Y.NormalizedValue).Normalized; }
        }

        #endregion

        #region Ctors

        public PointM(Measure x, Measure y)
        {
            this.x = x;
            this.y = y;
        }

        public PointM(PreciseDouble x, PreciseDouble y, UnitOfMeasure unit)
        {
            this.x = new Measure(x, unit);
            this.y = new Measure(y, unit);
        }

        #endregion

        #region Static Ctors

        public static PointM FromVector(Vector vec, UnitOfMeasure unit)
        {
            return new PointM(Measure.FromNormalizedValue(vec.X, unit), Measure.FromNormalizedValue(vec.Y, unit));
        }

        #endregion

        #region Equality operators

        public override bool Equals(object obj)
        {
            if (!(obj is PointM))
                return false;
            return this == (PointM)obj;
        }

        public static bool operator ==(PointM left, PointM right)
        {
            return left.X == right.X && left.Y == right.Y;
        }

        public static bool operator !=(PointM left, PointM right)
        {
            return !(left == right);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion

        #region Arithmetic operators

        public static PointM operator +(PointM pt1, PointM pt2)
        {
            return new PointM(pt1.X + pt2.X, pt1.Y + pt2.Y);
        }

        public static PointM operator -(PointM pt1, PointM pt2)
        {
            return new PointM(pt1.X - pt2.X, pt1.Y - pt2.Y);
        }

        public static PointM operator /(PointM pt, PreciseDouble value)
        {
            return new PointM(pt.X / value, pt.Y / value);
        }

        public static PointM operator *(PreciseDouble value, PointM pt)
        {
            return new PointM(value * pt.X, value * pt.Y);
        }

        public static PointM operator *(PointM pt, PreciseDouble value)
        {
            return new PointM(pt.X * value, pt.Y * value);
        }

        #endregion

        #region Conversion operators

        /// <summary>
        /// Returns a vector in normalized space (centimeters).
        /// </summary>
        /// <param name="point"></param>
        public static explicit operator Vector(PointM point)
        {
            return point.ToVector();
        }

        public Vector ToVector()
        {
            if (IsEmpty)
                return Vector.Empty;
            return new Vector(X.NormalizedValue, Y.NormalizedValue);
        }

        #endregion

        #region Functions


        public static PointM Average(PointM p1, params PointM[] points)
        {
            PointM total = p1;
            for (int i = 0; i < points.Length; i++)
                total += points[i];
            return total / (points.Length + 1);
        }

        public static Measure Distance(PointM p1, PointM p2)
        {
            var vdiff = (Vector)p2 - (Vector)p1;
            return Measure.FromNormalizedValue(vdiff.Length, p1.Unit ?? p2.Unit);
        }

        #endregion
    }
}
