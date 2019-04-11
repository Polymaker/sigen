using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen
{
	public static class MathP
	{
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

	}
}
