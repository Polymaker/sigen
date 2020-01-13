using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout.Visual
{
    public abstract class VisualElement
    {
        internal SILayout Layout;

        public virtual VisualElementType ElementType => VisualElementType.Unknown;

        public abstract RectangleM Bounds { get; }

        public object Tag { get; set; }

        internal virtual void FlipHandedness()
        {

        }
    }
}
