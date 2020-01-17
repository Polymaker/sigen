using Newtonsoft.Json;
using SiGen.Measuring;
using System;

namespace SiGen.Export
{
    public class FretsExportConfig : LineExportConfig
    {
        private Measure _ExtensionAmount = Measure.Empty;

        [JsonProperty("ExtendAmount")]
        public Measure ExtensionAmount { get => _ExtensionAmount; set { if (value != _ExtensionAmount) { _ExtensionAmount = value; OnPropertyChanged(nameof(ExtensionAmount)); } } }

        [JsonIgnore]
        public bool ExtendFretSlots { get { return !ExtensionAmount.IsEmpty && Math.Abs(ExtensionAmount.NormalizedValue.DoubleValue) > 0; } }
    }
}
