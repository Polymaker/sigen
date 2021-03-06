﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public interface ILayoutComponent
    {
        SILayout Layout { get; }
        void BeforeChangingStrings();
        void OnStringsChanged();
    }
}
