using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SiGen.Configuration;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SiGen.UI
{
    public class LayoutViewerDisplayConfig : INotifyPropertyChanged
    {
        private bool _ShowCenterLine;
        private bool _ShowStrings;
        private bool _ShowFrets;
        private bool _ShowMidlines;
        private bool _ShowMargins;
        private bool _ShowFingerboard;
        private bool _ShowTheoreticalFrets;
        private bool _RenderRealStrings;
        private bool _RenderRealFrets;
        private Color _FretLineColor;
        private UnitOfMeasure _DefaultDisplayUnit;
        private Measure _FretWidth;
        private Orientation _FingerboardOrientation;
        private Measure _FretExtensionAmount;

        [DefaultValue(false)]
        [JsonProperty]
        public bool ShowCenterLine
        {
            get => _ShowCenterLine;
            set => SetPropertyValue(ref _ShowCenterLine, value);
        }

        [DefaultValue(true)]
        [JsonProperty]
        public bool ShowStrings
        {
            get => _ShowStrings;
            set => SetPropertyValue(ref _ShowStrings, value);
        }

        [DefaultValue(true)]
        [JsonProperty]
        public bool ShowFrets
        {
            get => _ShowFrets;
            set => SetPropertyValue(ref _ShowFrets, value);
        }

        [DefaultValue(false)]
        [JsonProperty]
        public bool ShowTheoreticalFrets
        {
            get => _ShowTheoreticalFrets;
            set => SetPropertyValue(ref _ShowTheoreticalFrets, value);
        }

        [DefaultValue(true)]
        [JsonProperty]
        public bool ShowMidlines
        {
            get => _ShowMidlines;
            set => SetPropertyValue(ref _ShowMidlines, value);
        }

        [DefaultValue(true)]
        [JsonProperty]
        public bool ShowMargins
        {
            get => _ShowMargins;
            set => SetPropertyValue(ref _ShowMargins, value);
        }

        [DefaultValue(true)]
        [JsonProperty]
        public bool ShowFingerboard
        {
            get => _ShowFingerboard;
            set => SetPropertyValue(ref _ShowFingerboard, value);
        }

        [DefaultValue(Orientation.Horizontal)]
        [JsonProperty]
        [JsonConverter(typeof(StringEnumConverter))]
        public Orientation FingerboardOrientation
        {
            get => _FingerboardOrientation;
            set => SetPropertyValue(ref _FingerboardOrientation, value);
        }

        [DefaultValue(true)]
        [JsonProperty]
        public bool RenderRealFrets
        {
            get => _RenderRealFrets;
            set => SetPropertyValue(ref _RenderRealFrets, value);
        }

        [JsonIgnore]
        public bool ExtendFrets => !FretExtensionAmount.IsEmpty;

        [Browsable(false)]
        [JsonIgnore]
        public Measure FretExtensionAmount
        {
            get => _FretExtensionAmount;
            set => SetPropertyValue(ref _FretExtensionAmount, value);
        }

        [DefaultValue(typeof(Color), "0xFF0000")]
        [JsonProperty, JsonConverter(typeof(ColorJsonConverter))]
        public Color FretLineColor
        {
            get => _FretLineColor;
            set => SetPropertyValue(ref _FretLineColor, value);
        }

        [DefaultValue(false)]
        [JsonProperty]
        public bool RenderRealStrings
        {
            get => _RenderRealStrings;
            set => SetPropertyValue(ref _RenderRealStrings, value);
        }

        [Editor(typeof(Controls.Designers.MeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [JsonProperty, JsonConverter(typeof(MeasureJsonConverter))]
        public Measure FretWidth
        {
            get { return _FretWidth; }
            set
            {
                if (!value.IsEmpty && (value != _FretWidth || _FretWidth.Unit != value.Unit))
                {
                    _FretWidth = value;
                    OnPropertyChanged(nameof(FretWidth));
                }
            }
        }

        [Editor(typeof(Designers.UnitOfMeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [JsonProperty, JsonConverter(typeof(MeasureJsonConverter))]
        public UnitOfMeasure DefaultDisplayUnit
        {
            get => _DefaultDisplayUnit;
            set => SetPropertyValue(ref _DefaultDisplayUnit, value);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public LayoutViewerDisplayConfig()
        {
            _ShowStrings = true;
            _ShowFrets = true;
            _ShowTheoreticalFrets = false;
            _ShowMidlines = true;
            _ShowMargins = true;
            _RenderRealFrets = true;
            _ShowFingerboard = true;
            _FingerboardOrientation = Orientation.Horizontal;
            _DefaultDisplayUnit = UnitOfMeasure.Mm;
            _FretWidth = Measure.Mm(2.5);
            _FretExtensionAmount = Measure.Empty;
            _FretLineColor = Color.Red;
        }

        public static LayoutViewerDisplayConfig CreateDefault()
        {
            return new LayoutViewerDisplayConfig()
            {

            };
        }

        public LayoutViewerDisplayConfig Clone()
        {
            string json = JsonConvert.SerializeObject(this);
            JsonSerializer serializer = new JsonSerializer();
            using (var sr = new StringReader(json))
                return (LayoutViewerDisplayConfig)serializer.Deserialize(sr, typeof(LayoutViewerDisplayConfig));
        }

        protected void SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                property = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetFretWidth()
        {
            FretWidth = Measure.Mm(2.5);
        }

        public bool ShouldSerializeFretWidth()
        {
            return FretWidth.NormalizedValue != 0.25;
        }

        public void ResetFretExtensionAmount()
        {
            FretExtensionAmount = Measure.Empty;
        }

        public bool ShouldSerializeFretExtensionAmount()
        {
            return !FretExtensionAmount.IsEmpty;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetDefaultDisplayUnit()
        {
            DefaultDisplayUnit = UnitOfMeasure.Mm;
        }

        public bool ShouldSerializeDefaultDisplayUnit()
        {
            return DefaultDisplayUnit != UnitOfMeasure.Mm;
        }
    }
}
