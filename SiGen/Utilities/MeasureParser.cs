using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
    public static class MeasureParser
    {
        private static Regex SimpleMeasurePattern = new Regex("^\\s*(-?[\\d .,]+)\\s*([a-zA-Z\"']+\\D*)?$", RegexOptions.Compiled);
        private static Regex InchFractionPattern = new Regex("^\\s*(-?[\\d ]+\\s+)?(\\d{1,2})\\s*\\/\\s*(\\d{1,2})\\s*([a-zA-Z\"']+\\D*)?$", RegexOptions.Compiled);
        private static Regex FeetFractionPattern = new Regex("^\\s*(\\d+\\s*)('|ft|feet|foot)?\\s+(\\d+\\s+)?(\\d{1,2})\\s*\\/\\s*(\\d{1,2})", RegexOptions.Compiled);
        //private static string NumberPattern;
        //private static string UnitPattern;

        static MeasureParser()
        {
            //var currentCulture = System.Threading.Thread.CurrentThread.CurrentCulture;
            ////currentCulture.NumberFormat.NumberGroupSeparator
            //NumberPattern = string.Format("(?:-\\s*)?\\d+(?:[ {0}]\\d{{3}})*(?:{1}\\d+)?",
            //    Regex.Escape(currentCulture.NumberFormat.NumberGroupSeparator), 
            //    Regex.Escape(currentCulture.NumberFormat.NumberDecimalSeparator)
            //    );
            //UnitPattern = "(?:\\w+)";
            //SimpleMeasurePattern = new Regex("(" + NumberPattern + ")\\s*([a-zA-Z\"']+)?");
            //InchFractionPattern = new Regex("^\\s*(\\d+\\s+)?(\\d{1,2})\\s*\\/\\s*(\\d{1,2})\\s*([a-zA-Z\"']+)?$");

        }

        public static Measure Parse(string s, UnitOfMeasure defaultUnit = null)
        {
            double numericValue = 0;
            if(TryParseNumber(s, out numericValue))
                return new Measure(numericValue, defaultUnit);

            if (SimpleMeasurePattern.IsMatch(s))
            {
                var match = SimpleMeasurePattern.Match(s);
                var numberStr = match.Groups[1].Value;
                var unit = defaultUnit;

                if (match.Groups[2].Success)
                {
                    var unitName = match.Groups[2].Value.Trim();
                    unit = UnitOfMeasure.GetUnitByName(unitName);
                    if (unit == null)
                        throw new ArgumentException("#1 Invalid Unit of measure" + unitName);
                }

                double measureValue = 0;
                if (TryParseNumber(numberStr, out measureValue))
                    return new Measure(measureValue, unit);
                else
                    throw new ArgumentException("Invalid measure");
            }
            else if (InchFractionPattern.IsMatch(s))
            {
                var match = InchFractionPattern.Match(s);
                var whole = match.Groups[1].Success ? int.Parse(match.Groups[1].Value) : 0;
                var n1 = double.Parse(match.Groups[2].Value);
                var n2 = double.Parse(match.Groups[3].Value);
                UnitOfMeasure unit = defaultUnit ?? UnitOfMeasure.In;
                if (match.Groups[4].Success)
                {
                    unit = UnitOfMeasure.GetUnitByName(match.Groups[4].Value.Trim());
                    if (unit == null)
                        throw new ArgumentException("#2 Invalid Unit of measure" + match.Groups[4].Value);
                }

                return new Measure(whole + (n1 / n2), unit);
            }
            else if (FeetFractionPattern.IsMatch(s))
            {
                var match = FeetFractionPattern.Match(s);
                var wholeFeet = int.Parse(match.Groups[1].Value);
                var wholeInch = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;
                var n1 = double.Parse(match.Groups[4].Value);
                var n2 = double.Parse(match.Groups[5].Value);

                return new Measure(wholeFeet, UnitOfMeasure.Ft) + Measure.Inches(wholeInch + (n1 / n2));
            }
            else if (s.ToUpper().Trim() == "N/A")
                return Measure.Empty;

            throw new InvalidOperationException();
            //return Measure.Empty;
        }

        public static bool TryParse(string s, out Measure value, UnitOfMeasure defaultUnit = null)
        {
            value = Measure.Empty;
            double decimalValue = 0;

            if(double.TryParse(s, out decimalValue))
            {
                value = new Measure(decimalValue, null);
            }
            else if (SimpleMeasurePattern.IsMatch(s))
            {
                var matchResult = SimpleMeasurePattern.Match(s);
                var numberStr = matchResult.Groups[1].Value;
                var unit = defaultUnit;

                if (matchResult.Groups[2].Success)
                {
                    unit = UnitOfMeasure.GetUnitByName(matchResult.Groups[2].Value.Trim());
                    if (unit == null)
                        return false;
                }

                double measureValue = 0;
                if (TryParseNumber(numberStr, out measureValue))
                    value = new Measure(measureValue, unit);
            }
            else if (InchFractionPattern.IsMatch(s))
            {
                var matchResult = InchFractionPattern.Match(s);
                var whole = matchResult.Groups[1].Success ? int.Parse(matchResult.Groups[1].Value) : 0;
                var n1 = double.Parse(matchResult.Groups[2].Value);
                var n2 = double.Parse(matchResult.Groups[3].Value);
                UnitOfMeasure unit = defaultUnit ?? UnitOfMeasure.In;
                if (matchResult.Groups[4].Success)
                {
                    unit = UnitOfMeasure.GetUnitByName(matchResult.Groups[4].Value.Trim());
                    if (unit == null)
                        return false;
                }
                value = new Measure(whole + (n1 / n2), unit);
            }

            if (!value.IsEmpty && value.Unit == null && defaultUnit != null)
                value = new Measure(value.Value, defaultUnit);

            return !value.IsEmpty;
        }

        private static bool TryParseNumber(string s, out double result)
        {
            return double.TryParse(s, NumberStyles.Number, System.Threading.Thread.CurrentThread.CurrentCulture, out result);
        }
    }
}
