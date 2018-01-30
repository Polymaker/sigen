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
        private VisualElementType _ElementType;
        public virtual VisualElementType ElementType
        {
            get { return _ElementType; }
        }
        public abstract RectangleM Bounds { get; }
    }
}
