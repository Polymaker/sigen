using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public interface IStringBoundary
    {
        PointM GetRelativePoint(StringLine str, PointM pos);
    }
}
