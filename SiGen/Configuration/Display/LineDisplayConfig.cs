using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration.Display
{
    public class LineDisplayConfig : ConfigObjectBase
    {
        private bool _Visible;
        private Color _Color;

        public bool Visible
        {
            get => _Visible;
            set => SetPropertyValue(ref _Visible, value);
        }

        public Color Color
        {
            get => _Color;
            set => SetPropertyValue(ref _Color, value);
        }
    }
}
