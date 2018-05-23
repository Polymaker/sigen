using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class StringFretInfo
    {
        public int StringIndex { get; set; }
        public double PositionRatio { get; set; }
        public int FretIndex { get; set; }
        public PointM Position { get; set; }
        public bool IsVirtual { get; set; }
    }
}
