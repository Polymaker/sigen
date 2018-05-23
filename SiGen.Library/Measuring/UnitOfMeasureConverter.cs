using System;
using System.ComponentModel;
using System.Globalization;

namespace SiGen.Measuring
{
    public class UnitOfMeasureConverter : TypeConverter
    {
        #region Convert From

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if(value is string)
                return UnitOfMeasure.GetUnitByName((string)value);
            return base.ConvertFrom(context, culture, value);
        }

        #endregion

        #region Convert To

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (value is UnitOfMeasure && destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor))
            {
                var unit = value as UnitOfMeasure;
                var member = typeof(UnitOfMeasure).GetField(unit.Name, System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
                return new System.ComponentModel.Design.Serialization.InstanceDescriptor(member, new object[0]);
            }
            else if (value is UnitOfMeasure && destinationType == typeof(string))
            {
                var unit = value as UnitOfMeasure;
                return unit.Name;
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion

    }
}
