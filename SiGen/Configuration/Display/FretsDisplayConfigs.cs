using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration.Display
{
    public class FretsDisplayConfigs : LineDisplayConfig
    {
        private Measure _RenderWidth;
        private bool _RealWidth;
        private bool _DisplayAccurateLine;

        public Measure RenderWidth
        {
            get => _RenderWidth;
            set => SetPropertyValue(ref _RenderWidth, value);
        }

        public bool DisplayAccurateLine
        {
            get => _DisplayAccurateLine;
            set => SetPropertyValue(ref _DisplayAccurateLine, value);
        }
    }
}
