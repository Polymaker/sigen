using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public enum StringSpacingAlignment
    {
        /// <summary>
        /// Centers the fretboard between the outer strings.<br/>
        /// The first and last string are equally distant from the center of the layout. 
        /// </summary>
        [OldValue("SpacingMiddle"), LocDescription("StringSpacingAlignment_OuterStrings")]
        OuterStrings,
        /// <summary>
        /// Centers the fretboard along the middle string (or between the middle strings).<br/> 
        /// Useful for having a trully straight middle string when the nut slots are adjusted relative to the strings gauges.
        /// </summary>
        [OldValue("StringCenter")]
        MiddleString,
        /// <summary>
        /// 
        /// </summary>
        FingerboardEdges,
    }
}
