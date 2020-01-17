using Newtonsoft.Json;

namespace SiGen.Export
{
    public class StringsExportConfig : LineExportConfig
    {
        private bool _UseStringGauge;

        [JsonProperty]
        public bool UseStringGauge { get => _UseStringGauge; set { if (value != _UseStringGauge) { _UseStringGauge = value; OnPropertyChanged(nameof(UseStringGauge)); } } }
    }
}
