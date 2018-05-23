using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Measuring
{
    public struct MeasureSquared
    {
        #region Static Consts

        //public static readonly MeasureSquared Zero;
        public static readonly MeasureSquared Empty = new MeasureSquared(double.NaN, null);

        #endregion


        #region Fields

        private double normalizedRootValue;
        private UnitOfMeasure _Unit;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default unit of this measure. 
        /// Changing the unit does not affect the measure and the value will be automatically converted to the new unit.
        /// </summary>
        public UnitOfMeasure Unit { get { return _Unit; } set { _Unit = value; } }

        public double Value
        {
            get
            {
                double result = (Unit != null ? normalizedRootValue / Unit.ConversionFactor : normalizedRootValue);
                return result * result;
            }
            set
            {
                if (Unit != null)
                    normalizedRootValue = Math.Sqrt(value) * Unit.ConversionFactor;
                else
                    normalizedRootValue = Math.Sqrt(value);
            }
        }

        /// <summary>
        /// The value in centimeters
        /// </summary>
        public double NormalizedValue { get { return normalizedRootValue * normalizedRootValue; } }

        public double NormalizedRootValue { get { return normalizedRootValue; } }

        public double this[UnitOfMeasure unit]
        {
            get
            {
                double result = (unit != null ? normalizedRootValue / unit.ConversionFactor : normalizedRootValue);
                return result * result;
            }
            set
            {
                normalizedRootValue = Math.Sqrt(value) * (unit != null ? unit.ConversionFactor : 1d);
            }
        }

        public bool IsEmpty
        {
            get { return double.IsNaN(normalizedRootValue); }
        }

        #endregion

        #region Ctors

        public MeasureSquared(double value, UnitOfMeasure unit)
        {
            if (unit != null)
                normalizedRootValue = Math.Sqrt(value) * unit.ConversionFactor;
            else
                normalizedRootValue = Math.Sqrt(value);
            _Unit = unit;
        }

        #endregion

        #region Static Ctors

        public static MeasureSquared FromNormalizedValue(double value, UnitOfMeasure displayUnit)
        {
            return new MeasureSquared() { Unit = displayUnit, normalizedRootValue = Math.Sqrt(value) };
        }

        //public static MeasureSquared FromNormalizedRootValue(double value, UnitOfMeasure displayUnit)
        //{
        //    return new MeasureSquared() { Unit = displayUnit, normalizedRootValue = value };
        //}

        public static MeasureSquared Cm(double value) { return new MeasureSquared(value, UnitOfMeasure.Cm); }

        public static MeasureSquared Mm(double value) { return new MeasureSquared(value, UnitOfMeasure.Mm); }

        public static MeasureSquared Inches(double value) { return new MeasureSquared(value, UnitOfMeasure.Inches); }

        public static MeasureSquared Feets(double value) { return new MeasureSquared(value, UnitOfMeasure.Feets); }

        #endregion

        #region Equality operators

        public override bool Equals(object obj)
        {
            if (!(obj is MeasureSquared))
                return false;
            return this == (MeasureSquared)obj;
        }

        public static bool operator ==(MeasureSquared m1, MeasureSquared m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                return m1.IsEmpty == m2.IsEmpty;
            return m1.NormalizedRootValue.EqualOrClose(m2.NormalizedRootValue);
        }

        public static bool operator !=(MeasureSquared m1, MeasureSquared m2)
        {
            return !(m1 == m2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
            //return normalizedValue.GetHashCode();
        }

        public static bool EqualOrClose(MeasureSquared value1, MeasureSquared value2, Measure tolerence)
        {
            if (value1.IsEmpty || value2.IsEmpty)
                return value1.IsEmpty == value2.IsEmpty;

            return NumberExtensions.EqualOrClose(value1.NormalizedValue, value2.NormalizedValue, tolerence.NormalizedValue);
        }

        #endregion

        #region Comparison operators

        public static bool operator >(MeasureSquared m1, MeasureSquared m2)
        {
            return m1.NormalizedRootValue > m2.NormalizedRootValue;
        }

        public static bool operator <(MeasureSquared m1, MeasureSquared m2)
        {
            return m1.NormalizedRootValue < m2.NormalizedRootValue;
        }

        public static bool operator >=(MeasureSquared m1, MeasureSquared m2)
        {
            return m1.NormalizedRootValue >= m2.NormalizedRootValue;
        }

        public static bool operator <=(MeasureSquared m1, MeasureSquared m2)
        {
            return m1.NormalizedRootValue <= m2.NormalizedRootValue;
        }

        public int CompareTo(MeasureSquared other)
        {
            if (this < other)
                return -1;
            else if (this > other)
                return 1;
            else if (this == other)
                return 0;
            if (!IsEmpty)
                return 1;
            if (!other.IsEmpty)
                return -1;
            return 0;
        }

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;
            if (!(other is MeasureSquared))
                throw new InvalidOperationException("object is not MeasureSquared");
            return CompareTo((MeasureSquared)other);
        }

        #endregion

        #region Arithmetic operators

        public static MeasureSquared operator *(MeasureSquared measure, double value)
        {
            if (measure.IsEmpty)
                ThrowNaNOperationException();
            return FromNormalizedValue(measure.NormalizedValue * value, measure.Unit);
        }

        public static MeasureSquared operator /(MeasureSquared measure, double value)
        {
            if (measure.IsEmpty)
                ThrowNaNOperationException();
            return FromNormalizedValue(measure.NormalizedValue / value, measure.Unit);
        }

        public static MeasureSquared operator +(MeasureSquared m1, MeasureSquared m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                ThrowNaNOperationException();
            return FromNormalizedValue(m1.NormalizedValue + m2.NormalizedValue, m1.Unit ?? m2.Unit);
        }

        public static MeasureSquared operator -(MeasureSquared m1, MeasureSquared m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                ThrowNaNOperationException();
            return FromNormalizedValue(m1.NormalizedValue - m2.NormalizedValue, m1.Unit ?? m2.Unit);
        }

        public static double operator /(MeasureSquared m1, MeasureSquared m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                ThrowNaNOperationException();
            return m1.NormalizedValue / m2.NormalizedValue;
        }

        #endregion

        private void EnsureIsNotNaN()
        {
            if (IsEmpty)
                ThrowNaNOperationException();
        }

        private static void ThrowNaNOperationException()
        {
            throw new InvalidOperationException("You cannot perform any operation on an empty measure\u00B2.");
        }
    }
}
