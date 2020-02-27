using Newtonsoft.Json;
using SiGen.Measuring;
using System;
using System.ComponentModel;

namespace SiGen.Export
{
    public class FretsExportConfig : LineExportConfig
    {
        private Measure _ExtensionAmount = Measure.Empty;

        [JsonProperty("ExtendAmount", DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Measure ExtensionAmount { get => _ExtensionAmount; set { if (value != _ExtensionAmount) { _ExtensionAmount = value; OnPropertyChanged(nameof(ExtensionAmount)); } } }

        [JsonIgnore]
        public bool ExtendFretSlots { get { return !ExtensionAmount.IsEmpty && Math.Abs(ExtensionAmount.NormalizedValue.DoubleValue) > 0; } }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeExtensionAmount()
        {
            return ExtendFretSlots;
        }
    }
}
