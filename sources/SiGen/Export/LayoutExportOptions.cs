using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{
    [Serializable]
    public class LayoutExportOptions
    {
        public bool ExportStrings { get; set; }
        public bool ExportStringCenters { get; set; }
        public bool ExportCenterLine { get; set; }
        public bool ExportFrets { get; set; }
        public bool ExportFingerboard { get; set; }
        public bool ExportFingerboardMargin { get; set; }

        public Measure FretSlotsExtensionAmount { get; set; }

        public bool ExtendFretSlots { get { return !FretSlotsExtensionAmount.IsEmpty && FretSlotsExtensionAmount > Measure.Zero; } }

        public LayoutExportOptions()
        {
            FretSlotsExtensionAmount = Measure.Empty;
            ExportFrets = true;
            ExportFingerboard = true;
        }
    }

    [Serializable]
    public class LayoutSvgExportOptions : LayoutExportOptions
    {
        public bool InkscapeCompatible { get; set; }
        public int TargetDPI { get; set; }
        public Measure FretLineThickness { get; set; }
        public Color FretColor { get; set; }
        public Color StringColor { get; set; }
        public Color FingerboardColor { get; set; }
        public bool UseStringGauge { get; set; }

        public LayoutSvgExportOptions() : base()
        {
            InkscapeCompatible = true;
            TargetDPI = 90;
            FretColor = Color.Red;
            StringColor = Color.Black;
            FingerboardColor = Color.Blue;
            FretLineThickness = Measure.Empty;
        }
    }
}
