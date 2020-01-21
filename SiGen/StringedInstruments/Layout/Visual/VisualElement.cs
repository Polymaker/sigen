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

        private VisualElementType _ElementType = VisualElementType.Unknown;

        public virtual VisualElementType ElementType => _ElementType;

        public abstract RectangleM Bounds { get; }

        public object Tag { get; set; }

        public VisualElement()
        {

        }

        protected VisualElement(VisualElementType elementType)
        {
            _ElementType = elementType;
        }

        internal virtual void FlipHandedness()
        {

        }
    }
}
