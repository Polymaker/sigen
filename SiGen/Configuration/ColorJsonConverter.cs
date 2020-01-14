using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration
{
    public class ColorJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Color);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            string argbHex = (string)reader.Value;
            if (argbHex.StartsWith("#"))
            {
                int alpha = int.Parse(argbHex.Substring(1, 2), 
                    System.Globalization.NumberStyles.HexNumber);
                int red = int.Parse(argbHex.Substring(3, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(argbHex.Substring(5, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(argbHex.Substring(7, 2),
                    System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(alpha, red, green, blue);
            }
            return Color.Empty;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Color color)
                writer.WriteValue($"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}");
        }
    }
}
