using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{
    public class LayoutExportOptions
    {
        public bool ExportStrings { get; set; }
        public bool ExportStringCenters { get; set; }
        public bool ExportCenterLine { get; set; }

        public bool UseStringGauge { get; set; }
        public Measure FretLineThickness { get; set; }
        public Measure ExtendFretSlots { get; set; }

        public LayoutExportOptions()
        {
            ExtendFretSlots = Measure.Empty;
            FretLineThickness = Measure.Empty;
        }
    }

    public class LayoutSvgExportOptions : LayoutExportOptions
    {
        public bool InkscapeCompatible { get; set; }
        public int TargetDPI { get; set; }

        public LayoutSvgExportOptions() : base()
        {
            InkscapeCompatible = true;
            TargetDPI = 90;
        }
    }
}
