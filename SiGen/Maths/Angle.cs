using System;

namespace SiGen.Maths
{
    public struct Angle
    {

        #region Static Consts

        public static readonly Angle Zero = new Angle();
        public static readonly Angle Empty = new Angle() { _Degrees = double.NaN };

        #endregion

        #region Fields

        private double _Degrees;

        #endregion

        #region Properties

        public double Degrees
        {
            get { return _Degrees; }
            set
            {
                _Degrees = value;
            }
        }

        public double Radians
        {
            get { return ToRadians(Degrees); }
            set
            {
                _Degrees = ToDegrees(value);
            }
        }

        public double this[AngleUnit unit]
        {
            get { return unit == AngleUnit.Degrees ? Degrees : Radians; }
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return double.IsNaN(_Degrees);
            }
        }

        #endregion

        #region Static Ctors

        public static Angle FromDegrees(double degrees)
        {
            return new Angle { Degrees = degrees };
        }

        public static Angle FromRadians(double radians)
        {
            return new Angle { Radians = radians };
        }

        public static Angle FromPoints(Vector v1, Vector v2)
        {
            var dirVec = (v2 - v1).Normalized;
            return FromRadians(Math.Atan2(dirVec.Y, dirVec.X));
        }

        #endregion

        #region Arithmetic operators

        public static Angle operator +(Angle a1, Angle a2)
        {
            return FromDegrees(a1.Degrees + a2.Degrees);
        }

        public static Angle operator -(Angle a1, Angle a2)
        {
            return FromDegrees(a1.Degrees - a2.Degrees);
        }

        public static Angle operator *(Angle a1, double value)
        {
            return FromDegrees(a1.Degrees * value);
        }

        public static Angle operator /(Angle a1, double value)
        {
            return FromDegrees(a1.Degrees / value);
        }

        public static Angle operator %(Angle a1, double value)
        {
            return FromDegrees(a1.Degrees % value);
        }

        #endregion

        #region Equality operators

        public static bool operator ==(Angle a1, Angle a2)
        {
            return Math.Abs(a1.Degrees - a2.Degrees) <= double.Epsilon;
        }

        public static bool operator !=(Angle a1, Angle a2)
        {
            return !(a1 == a2);
        }

        public override bool Equals(object obj)
        {
            if (obj is Angle)
                return Degrees.EqualOrClose(((Angle)obj).Degrees);
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
            //return _Degrees.GetHashCode();
        }

        #endregion

        #region Comparison operators

        public static bool operator >(Angle a1, Angle a2)
        {
            return a1.Degrees > a2.Degrees;
        }

        public static bool operator <(Angle a1, Angle a2)
        {
            return a1.Degrees < a2.Degrees;
        }

        public static bool operator >=(Angle a1, Angle a2)
        {
            return a1.Degrees >= a2.Degrees;
        }

        public static bool operator <=(Angle a1, Angle a2)
        {
            return a1.Degrees <= a2.Degrees;
        }

        #endregion

        #region Convertion

        public static double ToDegrees(double radians)
        {
            if (double.IsNaN(radians))
                return double.NaN;
            return (radians * 180.0d) / Math.PI;
        }

        public static double ToRadians(double degrees)
        {
            if (double.IsNaN(degrees))
                return double.NaN;
            return Math.PI * degrees / 180.0d;
        }

        #endregion

        #region Functions


        #endregion
    }
}
