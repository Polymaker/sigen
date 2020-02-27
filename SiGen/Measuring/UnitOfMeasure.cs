using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Measuring
{
    [System.ComponentModel.TypeConverter(typeof(UnitOfMeasureConverter))]
    [DebuggerDisplay("{Name}")]
    public class UnitOfMeasure
    {
        public static readonly PreciseDouble CmToInch = 1d / 2.54d;

        //0.3937007874015748d
        //0.3937007874015748031496062992126d

        #region Fields

        private PreciseDouble _ConversionFactor;

        #endregion

        #region Properties

        public string Name { get; }

        public string Abbreviation { get; }

        public string Symbol { get; }

        public bool IsMetric { get; }

        public bool IsImperial => !IsMetric;

        public PreciseDouble ConversionFactor => _ConversionFactor;

        #endregion

        private UnitOfMeasure(string name, string symbol, string abv, PreciseDouble factor)
        {
            Name = name;
            Abbreviation = abv;
            Symbol = symbol;
            _ConversionFactor = factor;
            IsMetric = ((factor.DoubleValue * 100d) % 1d) == 0;
        }

        public static readonly UnitOfMeasure Millimeters = new UnitOfMeasure("Millimeters", "mm", "mm", 0.1d);
        public static readonly UnitOfMeasure Centimeters = new UnitOfMeasure("Centimeters", "cm", "cm", 1d);

        public static readonly UnitOfMeasure Inches = new UnitOfMeasure("Inches", "\"", "in", 2.54d);
        public static readonly UnitOfMeasure Feets = new UnitOfMeasure("Feets", "'", "ft", (2.54d) * 12d);

        public static UnitOfMeasure Mm { get { return Millimeters; } }
        public static UnitOfMeasure Cm { get { return Centimeters; } }
        public static UnitOfMeasure In { get { return Inches; } }
        public static UnitOfMeasure Ft { get { return Feets; } }

        public static PreciseDouble ConvertTo(PreciseDouble value, UnitOfMeasure from, UnitOfMeasure to)
        {
            if (from == to)
                return value;
            return (value * from.ConversionFactor) / to.ConversionFactor;
        }

        public static UnitOfMeasure GetUnitByName(string unitName)
        {
            switch (unitName.ToLower())
            {
                case "mm":
                case "millimeter":
                case "millimeters":
                    return Millimeters;
                //default:
                case "cm":
                case "centimeter":
                case "centimeters":
                    return Centimeters;
                case "\"":
                case "in":
                case "inch":
                case "inches":
                    return Inches;
                case "'":
                case "ft":
                case "foot":
                case "feet":
                case "feets":
                    return Feets;
            }
            return null;
        }

        #region Designer Code

        public class UnitOfMeasureConverter : System.ComponentModel.TypeConverter
        {
            public override bool CanConvertTo(System.ComponentModel.ITypeDescriptorContext context, Type destinationType)
            {
                return destinationType == typeof(System.ComponentModel.Design.Serialization.InstanceDescriptor) || destinationType == typeof(string) || base.CanConvertTo(context, destinationType) ;
            }

            public override object ConvertTo(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
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

            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                    return true;
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            {
                if (value is string unitStr)
                    return UnitOfMeasure.GetUnitByName(unitStr);
                return base.ConvertFrom(context, culture, value);
            }
        }

        #endregion
    }
}
