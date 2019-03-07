using System;

namespace SiGen.Maths
{
    public struct Angle
    {

        #region Static Consts

        public static readonly Angle Zero = new Angle();
        public static readonly Angle Empty = new Angle() { _Degrees = PreciseDouble.NaN };

        #endregion

        #region Fields

        private PreciseDouble _Degrees;

        #endregion

        #region Properties

        public PreciseDouble Degrees
        {
            get { return _Degrees; }
            set
            {
                _Degrees = value;
            }
        }

        public PreciseDouble Radians
        {
            get { return ToRadians(Degrees); }
            set
            {
                _Degrees = ToDegrees(value);
            }
        }

        public PreciseDouble this[AngleUnit unit]
        {
            get { return unit == AngleUnit.Degrees ? Degrees : Radians; }
        }

        [System.ComponentModel.Browsable(false)]
        public bool IsEmpty
        {
            get
            {
                return PreciseDouble.IsNaN(_Degrees);
            }
        }

        #endregion

        #region Static Ctors

        public static Angle FromDegrees(PreciseDouble degrees)
        {
            return new Angle { Degrees = degrees };
        }

        public static Angle FromRadians(PreciseDouble radians)
        {
            return new Angle { Radians = radians };
        }

        public static Angle FromPoints(Vector v1, Vector v2)
        {
            var dirVec = (v2 - v1).Normalized;
            return FromRadians(MathP.Atan2(dirVec.Y, dirVec.X));
        }

        public static Angle FromPoints(Vector center, Vector v1, Vector v2)
        {
            var ab = v1 - center;
            var bc = v2 - center;
            return FromRadians(MathP.Acos(Vector.Dot(ab, bc) / (ab.Length * bc.Length)));
        }

        public static Angle FromDirectionVector(Vector vec)
        {
            var dirVec = vec.Length == 1 ? vec : vec.Normalized;
            return FromRadians(MathP.Atan2(dirVec.Y, dirVec.X));
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
            return MathP.Abs(a1.Degrees - a2.Degrees) <= double.Epsilon;
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

        public static PreciseDouble ToDegrees(PreciseDouble radians)
        {
            if (PreciseDouble.IsNaN(radians))
                return PreciseDouble.NaN;
            return (radians * 180.0d) / Math.PI;
        }

        public static PreciseDouble ToRadians(PreciseDouble degrees)
        {
            if (PreciseDouble.IsNaN(degrees))
                return PreciseDouble.NaN;
            return Math.PI * degrees / 180.0d;
        }

        #endregion

        #region Functions

        /// <summary>
        /// Clamps the angle between 0-360
        /// </summary>
        public void Normalize()
        {
            _Degrees = NormalizeDegrees(_Degrees);
        }

        public Angle Normalized()
        {
            return FromDegrees(NormalizeDegrees(_Degrees));
        }

        public static PreciseDouble NormalizeDegrees(PreciseDouble degrees)
        {
            degrees = degrees % 360d;
            if (degrees < 0d)
                degrees += 360d;
            return degrees;
        }

        public static double NormalizeRadians(double radians)
        {
            radians = radians % (Math.PI * 2d);
            if (radians < 0d)
                radians += Math.PI * 2d;
            return radians;
        }

        public Angle Diff(Angle other)//clockwise
        {
            var angle1 = NormalizeDegrees(Degrees);
            var angle2 = NormalizeDegrees(other.Degrees);
            if (angle2 > angle1)
            {
                return FromDegrees(angle2 - angle1);
            }
            return FromDegrees((360f - angle1) + angle2);
        }

        public override string ToString()
        {
            return string.Format("{0}°", Degrees);
        }

        public string ToString(AngleUnit unit)
        {
            if(unit == AngleUnit.Degrees)
                return string.Format("{0:0.##}°", Degrees);
            else
                return string.Format("{0:0.##}°", Radians);
        }

        #endregion
    }
}
