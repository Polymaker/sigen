using SiGen.Measuring;
using System.ComponentModel;
using System.Drawing;

namespace SiGen.Export
{
    public class LayoutLineExportConfig/* : INotifyPropertyChanged*/
    {
        public bool Enabled { get; set; }
        public Color Color { get; set; }
        public ExportUnit LineUnit { get; set; }
        public double LineThickness { get; set; }
        public bool IsDashed { get; set; }

        public LayoutLineExportConfig()
        {
            LineThickness = 1d;
            LineUnit = ExportUnit.Points;
            IsDashed = false;
            Color = Color.Black;
        }
        //public event PropertyChangedEventHandler PropertyChanged;



    }

    public class StringsExportConfig : LayoutLineExportConfig
    {
        public bool UseStringGauge { get; set; }
    }

    public class FretsExportConfig : LayoutLineExportConfig
    {
        public Measure ExtensionAmount { get; set; } = Measure.Empty;

        public bool ExtendFretSlots { get { return !ExtensionAmount.IsEmpty && ExtensionAmount > Measure.Zero; } }
    }
}
