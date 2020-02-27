using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    static class FretHelper
    {
        public static int GetFretNumberFromRatio(double ratio)
        {
            if ((ratio % 1) == 0)
                return (int)ratio;

            for (int i = 1; i <= 30; i++)
            {
                var fretRatio = (PreciseDouble)Math.Pow(2, i / 12d);
                if (fretRatio.EqualOrClose(ratio, 0.0005))
                    return i;
            }

            return -1;
        }
    }
}
