using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public enum StringSpacingMethod
    {
        [OldValue("StringsCenter"), LocDescription("StringSpacingMethod_EqualDistance")]
        EqualDistance,
        [OldValue("BetweenStrings"), LocDescription("StringSpacingMethod_EqualSpacing")]
        EqualSpacing,
        //StringsCenter,
        //BetweenStrings
    }
}
