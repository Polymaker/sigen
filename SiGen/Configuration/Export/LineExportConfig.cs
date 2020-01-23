using Newtonsoft.Json;
using SiGen.Configuration;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;

namespace SiGen.Export
{
    public class LineExportConfig : ConfigObjectBase
    {
        private bool _Enabled;
        private Color _Color;
        private LineUnit _LineUnit;
        private double _LineThickness;
        private bool _IsDashed;

        [JsonProperty]
        public bool Enabled { get => _Enabled; set { if (value != _Enabled) { _Enabled = value; OnPropertyChanged(nameof(Enabled)); } } }
        
        [JsonProperty, JsonConverter(typeof(ColorJsonConverter))]
        public Color Color
        {
            get => _Color;
            set => SetPropertyValue(ref _Color, value);
        }
        
        [JsonProperty, JsonConverter(typeof(LineUnitConverter))]
        public LineUnit LineUnit { get => _LineUnit; set { if (value != _LineUnit) { _LineUnit = value; OnPropertyChanged(nameof(LineUnit)); } } }
        
        [JsonProperty]
        public double LineThickness { get => _LineThickness; set { if (value != _LineThickness) { _LineThickness = value; OnPropertyChanged(nameof(LineThickness)); } } }
        
        [JsonProperty]
        public bool IsDashed { get => _IsDashed; set { if (value != _IsDashed) { _IsDashed = value; OnPropertyChanged(nameof(IsDashed)); } } }

        public LineExportConfig()
        {
            _LineThickness = 1d;
            _LineUnit = LineUnit.Points;
            _IsDashed = false;
            _Color = Color.Black;
        }


        


        class LineUnitConverter : JsonConverter
        {
            public override bool CanConvert(Type objectType)
            {
                return objectType == typeof(LineUnit);
            }

            public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            {
                string unitStr = (string)reader.Value;
                switch (unitStr.Trim().ToLower())
                {
                    case "px":
                    case "pixel":
                    case "pixels":
                        return LineUnit.Pixels;
                    case "pt":
                    case "point":
                    case "points":
                        return LineUnit.Points;
                    case "mm":
                    case "millimeter":
                    case "millimeters":
                        return LineUnit.Millimeters;
                    case "in":
                    case "inch":
                    case "inches":
                        return LineUnit.Inches;
                }
                return LineUnit.Points;
            }

            public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
            {
                if (value is LineUnit unit)
                {
                    switch (unit)
                    {
                        case LineUnit.Pixels:
                            writer.WriteValue("px");
                            break;
                        case LineUnit.Points:
                            writer.WriteValue("pt");
                            break;
                        case LineUnit.Millimeters:
                            writer.WriteValue("mm");
                            break;
                        case LineUnit.Inches:
                            writer.WriteValue("in");
                            break;
                    }
                }
            }
        }
    }
}
