﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public enum LengthFunction
    {
        /// <summary>
        /// The scale length is applied/calculated with the string's real length. 
        /// Commonly used for multiscale. Is also used for calculating fret compensation.
        /// </summary>
        AlongString,
        /// <summary>
        /// The scale length is applied/calculated with the string's vertical length.
        /// Standard for non-multiscale instruments.
        /// </summary>
        AlongFingerboard
    }
}
