﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout
{
	[Flags]
    public enum LayoutProcessElement
    {
        StringLayout,
        FretPlacement,
        FingerboardShape,
        Visual
    }
}
