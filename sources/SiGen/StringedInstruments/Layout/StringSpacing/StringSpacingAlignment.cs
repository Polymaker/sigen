using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public enum StringSpacingAlignment
    {
        /// <summary>
        /// Centers the fretboard in the middle of the strings spread, so that first and last string are equally distant from the center. 
        /// </summary>
        SpacingMiddle,
        /// <summary>
        /// Centers the fretboard along the middle string. 
        /// Useful for having a trully straight middle string when the nut slots are adjusted relative to the strings gauges.
        /// </summary>
        StringCenter
            //,Symmetrical
    }
}
