﻿using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public class StringLine : LayoutLine
    {
        private readonly SIString _String;

        public SIString String
        {
            get { return _String; }
        }

        public int Index { get { return _String.Index; } }

        public PointM FretZero { get; set; }
        
        public StringLine(SIString str, PointM p1, PointM p2) : base(p1, p2, VisualElementType.String)
        {
            _String = str;
        }
    }
}