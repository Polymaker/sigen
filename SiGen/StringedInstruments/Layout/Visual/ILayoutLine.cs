using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public interface ILayoutLine
    {
        bool Intersects(LayoutLine line, out PointM intersection, bool infiniteLine = true);
        bool Intersects(Line line, out Vector intersection, bool infiniteLine = true);
    }
}
