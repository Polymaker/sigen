using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen
{
    public static class NumberHelper
    {
		public static bool EqualOrClose(this PreciseDouble n1, PreciseDouble n2)
		{
			return EqualOrClose(n1, n2, double.Epsilon);
		}

		public static bool EqualOrClose(this PreciseDouble n1, PreciseDouble n2, PreciseDouble tolerence)
		{
			return MathP.Abs(n1 - n2) <= tolerence;
		}

		public static bool EqualOrClose(this double n1, double n2)
        {
            return EqualOrClose(n1, n2, double.Epsilon);
        }

        public static bool EqualOrClose(this double n1, double n2, double tolerence)
        {
            return Math.Abs(n1 - n2) <= tolerence;
        }

        public static double Round(this double value, double step)
        {
            return Math.Round(value / step) * step;
        }

        public static string GetSuffix(this int value)
        {
            if (value <= 0)
                return string.Empty;

            string number = value.ToString();
            if(number.Length >= 2 && number[number.Length - 2] == '1' && value % 10 <= 3 && value % 10 > 0)
                return "th";

            switch (number.Last())
            {
                case '1':
                    return "st";
                case '2':
                    return "nd";
                case '3':
                    return "rd";
                default:
                    return "th";
            }
        }

		public static bool SmartTryParse(string s, out double result)
		{
			return SmartTryParse(s, NumberFormatInfo.CurrentInfo, out result);
		}

		public static bool SmartTryParse(string s, IFormatProvider provider, out double result)
		{
			result = 0;
			var numberFormat = NumberFormatInfo.GetInstance(provider);

			if ((s.Contains(".") || s.Contains(",")) && !s.Contains(numberFormat.NumberDecimalSeparator))
			{
				var other = numberFormat.NumberDecimalSeparator == "," ? "." : ",";
				if (double.TryParse(s.Replace(other, numberFormat.NumberDecimalSeparator), NumberStyles.Number, provider, out result))
					return true;
			}

			return double.TryParse(s, NumberStyles.Number, provider, out result);
		}
	}
}
