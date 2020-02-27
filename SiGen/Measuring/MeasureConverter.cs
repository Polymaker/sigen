using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
    public class MeasureConverter : TypeConverter
    {
        private static Regex ParseSimpleReg;
        private static Regex ParseFractionReg;

        static MeasureConverter()
        {
            ParseSimpleReg = new Regex("^\\s*(-?[\\d .,]+)\\s*([a-zA-Z\"']+)", RegexOptions.Compiled);
            ParseFractionReg = new Regex("^\\s*(\\d+\\s+)?(\\d{1,2})\\s*\\/\\s*(\\d{1,2})\\s*([a-zA-Z\"']+)", RegexOptions.Compiled);
        }

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                var measureStr = (string)value;
                if (!string.IsNullOrWhiteSpace(measureStr))
                {
                    if (ParseSimpleReg.IsMatch(measureStr))
                    {
                        var match = ParseSimpleReg.Match(measureStr);
                        var numberStr = match.Groups[1].Value;
                        var unitName = match.Groups[2].Value;
                        var unit = UnitOfMeasure.GetUnitByName(unitName);
                        if (unit == null)
                            throw new ArgumentException("#1 Invalid Unit of measure" + unitName);

                        if (numberStr.Contains(".") || numberStr.Contains(","))
                        {
                            var cleanStr = (Func<string, string>)((str) =>
                            {
                                return str.Replace(",", string.Empty).Replace(".", string.Empty).Replace(" ", string.Empty);
                            });

                            int idx = numberStr.LastIndexOfAny(new char[] { '.', ',' });
                            var left = cleanStr(numberStr.Substring(0, idx));
                            var right = cleanStr(numberStr.Substring(idx));
                            numberStr = left + NumberFormatInfo.CurrentInfo.NumberDecimalSeparator + right;
                        }
                        double measureValue = 0;
                        if (double.TryParse(numberStr, out measureValue))
                            return new Measure(measureValue, unit);
                        else
                            throw new ArgumentException("Invalid measure");
                    }
                    else if (ParseFractionReg.IsMatch(measureStr))
                    {
                        var match = ParseFractionReg.Match(measureStr);
                        var whole = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0;
                        var n1 = double.Parse(match.Groups[2].Value);
                        var n2 = double.Parse(match.Groups[3].Value);
                        var unit = UnitOfMeasure.GetUnitByName(match.Groups[4].Value.Trim());
                        if (unit == null)
                            throw new ArgumentException("#2 Invalid Unit of measure" + match.Groups[4].Value);
                        return new Measure(whole + (n1 / n2), unit);
                    }
                    else if (measureStr.ToUpper().Trim() == "N/A")
                        return Measure.Empty;
                    else
                    {
                        double cm = 0;
                        if (double.TryParse(measureStr, out cm))
                            return Measure.FromNormalizedValue(cm, null);
                    }
                }
            }
            return base.ConvertFrom(context, culture, value);
        }

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
                    if(measure.IsEmpty)
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
                    return measure.ToString(/*measure.Unit, true*/);
                }
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
