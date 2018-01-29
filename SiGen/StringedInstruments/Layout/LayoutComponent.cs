using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class LayoutComponent
    {
        private readonly SILayout _Layout;
        public SILayout Layout { get { return _Layout; } }
        public int NumberOfStrings { get { return Layout.NumberOfStrings; } }

        public LayoutComponent(SILayout layout)
        {
            _Layout = layout;
        }
    }
}
