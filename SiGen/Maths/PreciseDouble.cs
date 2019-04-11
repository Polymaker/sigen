using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SiGen
{
	[DebuggerDisplay("{DoubleValue}")]
	public struct PreciseDouble : IComparable, IConvertible, IFormattable, IComparable<PreciseDouble>, IEquatable<PreciseDouble>
	{
		private double doubleValue;

		public static readonly PreciseDouble MaxValue = new PreciseDouble(double.MaxValue);

		public static readonly PreciseDouble MinValue = new PreciseDouble(double.MinValue);

		public static readonly PreciseDouble NaN = new PreciseDouble(double.NaN);

		public decimal DecimalValue => (decimal)doubleValue;

		public double DoubleValue => doubleValue;

		public PreciseDouble(double value)
		{
			doubleValue = value;
		}

		public static implicit operator PreciseDouble(double value)
		{
			return new PreciseDouble() { doubleValue = value };
		}

		public static implicit operator PreciseDouble(decimal value)
		{
			return new PreciseDouble() { doubleValue = (double)value };
		}

		public static implicit operator PreciseDouble(int value)
		{
			return new PreciseDouble() { doubleValue = value };
		}

		public static explicit operator double(PreciseDouble value)
		{
			return value.doubleValue;
		}

		#region Arithmetic operators

		public static PreciseDouble operator +(PreciseDouble v1, PreciseDouble v2)
		{
			var result = v1.doubleValue + v2.doubleValue;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)(v1.DecimalValue + v2.DecimalValue);

			return new PreciseDouble() { doubleValue = result };
		}

		public static double operator +(PreciseDouble v1, double v2)
		{
			var result = v1.doubleValue + v2;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2) && InDecimalRange(result))
				result = (double)(v1.DecimalValue + (decimal)v2);

			return result;
		}

		public static double operator +(double v1, PreciseDouble v2)
		{
			var result = v1 + v2.doubleValue;

			if (InDecimalRange(v1) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)((decimal)v1 + v2.DecimalValue);

			return result;
		}

		public static PreciseDouble operator -(PreciseDouble v1, PreciseDouble v2)
		{
			var result = v1.doubleValue - v2.doubleValue;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)(v1.DecimalValue - v2.DecimalValue);

			return new PreciseDouble() { doubleValue = result };
		}

		public static double operator -(PreciseDouble v1, double v2)
		{
			var result = v1.doubleValue - v2;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2) && InDecimalRange(result))
				result = (double)(v1.DecimalValue - (decimal)v2);

			return result;
		}

		public static double operator -(double v1, PreciseDouble v2)
		{
			var result = v1 - v2.doubleValue;

			if (InDecimalRange(v1) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)((decimal)v1 - v2.DecimalValue);

			return result;
		}

		public static PreciseDouble operator *(PreciseDouble v1, PreciseDouble v2)
		{
			var result = v1.doubleValue * v2.doubleValue;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)(v1.DecimalValue * v2.DecimalValue);

			return new PreciseDouble() { doubleValue = result };
		}

		public static double operator *(PreciseDouble v1, double v2)
		{
			var result = v1.doubleValue * v2;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2) && InDecimalRange(result))
				return (double)(v1.DecimalValue * (decimal)v2);

			return result;
		}

		public static double operator *(double v1, PreciseDouble v2)
		{
			var result = v1 * v2.doubleValue;

			if (InDecimalRange(v1) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				return (double)((decimal)v1 * v2.DecimalValue);

			return result;
		}

		public static PreciseDouble operator /(PreciseDouble v1, PreciseDouble v2)
		{
			var result = v1.doubleValue / v2.doubleValue;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)(v1.DecimalValue / v2.DecimalValue);

			return new PreciseDouble() { doubleValue = result };
		}

		public static double operator /(PreciseDouble v1, double v2)
		{
			var result = v1.doubleValue / v2;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2) && InDecimalRange(result))
				return (double)(v1.DecimalValue / (decimal)v2);

			return result;
		}

		public static double operator /(double v1, PreciseDouble v2)
		{
			var result = v1 / v2.doubleValue;

			if (InDecimalRange(v1) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				return (double)((decimal)v1 / v2.DecimalValue);

			return result;
		}

		public static PreciseDouble operator %(PreciseDouble v1, PreciseDouble v2)
		{
			var result = v1.doubleValue % v2.doubleValue;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)(v1.DecimalValue % v2.DecimalValue);

			return new PreciseDouble() { doubleValue = result };
		}

		public static double operator %(PreciseDouble v1, double v2)
		{
			var result = v1.doubleValue % v2;

			if (InDecimalRange(v1.doubleValue) && InDecimalRange(v2) && InDecimalRange(result))
				result = (double)(v1.DecimalValue % (decimal)v2);

			return result;
		}

		public static double operator %(double v1, PreciseDouble v2)
		{
			var result = v1 % v2.doubleValue;

			if (InDecimalRange(v1) && InDecimalRange(v2.doubleValue) && InDecimalRange(result))
				result = (double)((decimal)v1 % v2.DecimalValue);

			return result;
		}

		#endregion

		#region Comparison operators

		public static bool operator >(PreciseDouble v1, PreciseDouble v2)
		{
			return v1.doubleValue > v2.doubleValue;
		}

		public static bool operator <(PreciseDouble v1, PreciseDouble v2)
		{
			return v1.doubleValue < v2.doubleValue;
		}

		public static bool operator >=(PreciseDouble v1, PreciseDouble v2)
		{
			return v1.doubleValue >= v2.doubleValue;
		}

		public static bool operator <=(PreciseDouble v1, PreciseDouble v2)
		{
			return v1.doubleValue <= v2.doubleValue;
		}

		public int CompareTo(object obj)
		{
			return doubleValue.CompareTo(obj);
		}

		public int CompareTo(PreciseDouble other)
		{
			return doubleValue.CompareTo(other.doubleValue);
		}

		#endregion

		#region IConvertible

		TypeCode IConvertible.GetTypeCode()
		{
			return ((IConvertible)doubleValue).GetTypeCode();
		}

		bool IConvertible.ToBoolean(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToBoolean(provider);
		}

		char IConvertible.ToChar(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToChar(provider);
		}

		sbyte IConvertible.ToSByte(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToSByte(provider);
		}

		byte IConvertible.ToByte(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToByte(provider);
		}

		short IConvertible.ToInt16(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToInt16(provider);
		}

		ushort IConvertible.ToUInt16(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToUInt16(provider);
		}

		int IConvertible.ToInt32(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToInt32(provider);
		}

		uint IConvertible.ToUInt32(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToUInt32(provider);
		}

		long IConvertible.ToInt64(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToInt64(provider);
		}

		ulong IConvertible.ToUInt64(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToUInt64(provider);
		}

		float IConvertible.ToSingle(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToSingle(provider);
		}

		double IConvertible.ToDouble(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToDouble(provider);
		}

		decimal IConvertible.ToDecimal(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToDecimal(provider);
		}

		DateTime IConvertible.ToDateTime(IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToDateTime(provider);
		}

		object IConvertible.ToType(Type conversionType, IFormatProvider provider)
		{
			return ((IConvertible)doubleValue).ToType(conversionType, provider);
		}

		#endregion

		#region Equality operators

		public override bool Equals(object obj)
		{
			if (!(obj is PreciseDouble))
				return false;
			PreciseDouble num = (PreciseDouble)obj;
			if (num == this)
				return true;
			return doubleValue.Equals(num.doubleValue);
		}

		public bool Equals(PreciseDouble obj)
		{
			if (obj == this)
				return true;
			return doubleValue.Equals(obj.doubleValue);
		}

		public static bool operator ==(PreciseDouble v1, PreciseDouble v2)
		{
			return v1.doubleValue == v2.doubleValue;
		}

		public static bool operator !=(PreciseDouble v1, PreciseDouble v2)
		{
			return !(v1 == v2);
		}

		public override int GetHashCode()
		{
			return doubleValue.GetHashCode();
		}

		#endregion

		private static bool InDecimalRange(double value)
		{
			if (double.IsNaN(value) || double.IsInfinity(value))
				return false;
			return value > (double)decimal.MinValue && value < (double)decimal.MaxValue;
		}

		public static bool IsNaN(PreciseDouble d)
		{
			return double.IsNaN(d.doubleValue);
		}

        public static PreciseDouble Min(PreciseDouble v1, PreciseDouble v2)
        {
            return v1 < v2 ? v1 : v2;
        }

        public static PreciseDouble Min(PreciseDouble v1, PreciseDouble[] vals)
        {
            PreciseDouble min = v1;
            for (int i = 0; i < vals.Length; i++)
                min = Min(min, vals[i]);
            return min;
        }


        public static PreciseDouble Max(PreciseDouble v1, PreciseDouble v2)
        {
            return v1 > v2 ? v1 : v2;
        }

        public static PreciseDouble Max(PreciseDouble v1, PreciseDouble[] vals)
        {
            PreciseDouble max = v1;
            for (int i = 0; i < vals.Length; i++)
                max = Max(max, vals[i]);
            return max;
        }

        public static bool IsInfinity(PreciseDouble d)
		{
			return double.IsInfinity(d.doubleValue);
		}

		#region ToString

		public override string ToString()
		{
			return doubleValue.ToString();
		}

		public string ToString(IFormatProvider provider)
		{
			return doubleValue.ToString(provider);
		}

		public string ToString(string format, IFormatProvider provider)
		{
			return doubleValue.ToString(format, provider);
		}

		#endregion

	}
}
