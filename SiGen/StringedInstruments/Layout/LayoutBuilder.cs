using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class LayoutBuilder
    {
        public void ConstructString(Measure nutPos, Measure bridgePos, Measure length, ScaleLengthMethod method)
        {
            var opp = Measure.Abs(nutPos - bridgePos);
            Measure adj = length;

            if (method == ScaleLengthMethod.AlongString)
            {
                var theta = Math.Asin(opp.NormalizedValue / length.NormalizedValue);
                adj = Math.Cos(theta) * length;
            }

            var p1 = new PointM(nutPos, (adj / 2d));
            var p2 = new PointM(bridgePos, (adj / 2d) * -1d);
        }
    }
}
