using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System
{
    public static class NumberExtensions
    {
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

        public static double Floor(this double value, double step)
        {
            return Math.Floor(value / step) * step;
        }

        public static double Ceiling(this double value, double step)
        {
            return Math.Ceiling(value / step) * step;
        }
    }
}
