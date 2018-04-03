using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen
{
    public static class NumberHelper
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

    }
}
