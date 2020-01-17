using Newtonsoft.Json;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration
{
    public class MeasureJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Measure) || objectType == typeof(UnitOfMeasure);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (objectType == typeof(UnitOfMeasure))
            {
                string unitAbv = (string)reader.Value;
                return UnitOfMeasure.GetUnitByName(unitAbv);
            }
            else if (objectType == typeof(Measure))
            {
                return Measure.ParseInvariant((string)reader.Value);
            }

            throw new NotImplementedException();
        }

        public override void WriteJson(JsonWriter writer,  object value, JsonSerializer serializer)
        {
            if (value is UnitOfMeasure unit)
            {
                writer.WriteValue(unit.Abbreviation);
            }
            else if (value is Measure measure)
            {
                string measureStr = string.Format(CultureInfo.InvariantCulture, "{0}{1}", 
                    measure.Value.DoubleValue, measure.Unit.Abbreviation);

                writer.WriteValue(measureStr);
            }
        }
    }
}
