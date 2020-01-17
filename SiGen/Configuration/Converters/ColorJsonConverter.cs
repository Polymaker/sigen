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
            string rgbaHEX = (string)reader.Value;
            
            if (rgbaHEX.StartsWith("#"))
            {
                int red = int.Parse(rgbaHEX.Substring(1, 2), 
                    System.Globalization.NumberStyles.HexNumber);
                int green = int.Parse(rgbaHEX.Substring(3, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int blue = int.Parse(rgbaHEX.Substring(5, 2),
                    System.Globalization.NumberStyles.HexNumber);
                int alpha = int.Parse(rgbaHEX.Substring(7, 2),
                    System.Globalization.NumberStyles.HexNumber);
                return Color.FromArgb(alpha, red, green, blue);
            }
            else
            {
                try
                {
                    return Color.FromName(rgbaHEX);
                }
                catch { }
            }
            return Color.Empty;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is Color color)
                writer.WriteValue($"#{color.R:X2}{color.G:X2}{color.B:X2}{color.A:X2}");
        }
    }
}
