using System.ComponentModel;
using System.Drawing;

namespace SiGen.Export
{
    public class LayoutItemExportConfig/* : INotifyPropertyChanged*/
    {
        public bool Enabled { get; set; }
        public Color Color { get; set; }
        public ExportUnit LineUnit { get; set; }
        public double LineThickness { get; set; }

        //public event PropertyChangedEventHandler PropertyChanged;
    }
}
