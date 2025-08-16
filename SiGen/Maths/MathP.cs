using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen
{
	public static class MathP
	{
        public static PreciseDouble Max(PreciseDouble a, PreciseDouble b)
        {
            return a > b ? a : b;
        }

        public static PreciseDouble Min(PreciseDouble a, PreciseDouble b)
        {
            return a < b ? a : b;
        }

        public static PreciseDouble Max(PreciseDouble a, params PreciseDouble[] b)
        {
            PreciseDouble maxVal = a;
            for (int i = 0; i < b.Length; i++)
                maxVal = Max(a, b[i]);
            return maxVal;
        }

        public static PreciseDouble Min(PreciseDouble a, params PreciseDouble[] b)
        {
            PreciseDouble minVal = a;
            for (int i = 0; i < b.Length; i++)
                minVal = Min(a, b[i]);
            return minVal;
        }

        public static PreciseDouble Abs(PreciseDouble value) => Math.Abs(value.DoubleValue);

		public static PreciseDouble Floor(PreciseDouble value) => Math.Floor(value.DoubleValue);

		public static PreciseDouble Ceiling(PreciseDouble value) => Math.Ceiling(value.DoubleValue);

		public static PreciseDouble Round(PreciseDouble value) => Math.Round(value.DoubleValue);

		public static PreciseDouble Asin(PreciseDouble value) => Math.Asin(value.DoubleValue);

		public static PreciseDouble Acos(PreciseDouble value) => Math.Acos(value.DoubleValue);

		public static PreciseDouble Atan(PreciseDouble value) => Math.Atan(value.DoubleValue);

		public static PreciseDouble Atan2(PreciseDouble x, PreciseDouble y) => Math.Atan2(x.DoubleValue, y.DoubleValue);

		public static PreciseDouble Sin(PreciseDouble value) => Math.Sin(value.DoubleValue);

		public static PreciseDouble Cos(PreciseDouble value) => Math.Cos(value.DoubleValue);

		public static PreciseDouble Tan(PreciseDouble value) => Math.Tan(value.DoubleValue);

		public static PreciseDouble Pow(PreciseDouble x, PreciseDouble y) => Math.Pow(x.DoubleValue, y.DoubleValue);

		public static PreciseDouble Sqrt(PreciseDouble value) => Math.Sqrt(value.DoubleValue);

        public static PreciseDouble Lerp(PreciseDouble start, PreciseDouble end, PreciseDouble t)
        {
            // Ensure t is clamped between 0 and 1
            t = Max(0.0, Min(1.0, t));

            // Perform linear interpolation
            return start + (end - start) * t;
        }

        public static PreciseDouble InvLerp(PreciseDouble a, PreciseDouble b, PreciseDouble v)
        {
            return (v - a) / (b - a);
        }

        public static PreciseDouble Map(PreciseDouble iMin, PreciseDouble iMax, PreciseDouble oMin, PreciseDouble oMax, PreciseDouble v)
        {
            PreciseDouble t = InvLerp(iMin, iMax, v);
            return Lerp(oMin, oMax, t);
        }
    }
}
