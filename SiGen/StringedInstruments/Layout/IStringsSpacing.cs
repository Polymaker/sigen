using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public interface IStringsSpacing
    {
        Measure StringSpreadAtNut { get; }
        Measure StringSpreadAtBridge { get; }
        StringSpacingAlignment NutAlignment { get; set; }
        StringSpacingAlignment BridgeAlignment { get; set; }
        Measure GetSpacingBetweenStrings(int index1, int index2, bool atNut);
        Measure GetSpacing(int index, bool atNut);
        void SetSpacing(int index, Measure value, bool atNut);
        Measure[] GetStringPositions(bool atNut, out Measure center);
    }
}
