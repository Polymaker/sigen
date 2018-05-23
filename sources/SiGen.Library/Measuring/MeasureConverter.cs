using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Measuring
{
    public class MeasureConverter : TypeConverter
    {
        #region Convert From

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var measureStr = (string)value;
                if (string.IsNullOrWhiteSpace(measureStr))
                    return Measure.Empty;
                return MeasureParser.Parse((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        #region Convert To

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(InstanceDescriptor))
                return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is Measure)
            {
                if (destinationType == typeof(InstanceDescriptor))
                {
                    if (value == null)
                        return null;
                    var measure = (Measure)value;
                    if (measure.IsEmpty)
                    {
                        var member = typeof(Measure).GetField("Empty", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                        return new InstanceDescriptor(member, new object[0]);
                    }
                    var ctor = typeof(Measure).GetConstructor(new Type[] { typeof(double), typeof(UnitOfMeasure) });
                    return new InstanceDescriptor(ctor, new object[] { measure.Value, measure.Unit });
                }
                if (destinationType == typeof(string))
                {
                    if (value == null)
                        return null;
                    var measure = (Measure)value;
                    return measure.ToString();
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

    }
}
