using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SiGen.Measuring
{
    [Serializable, TypeConverter(typeof(Utilities.MeasureConverter))]
    public struct Measure : ISerializable, IXmlSerializable, IComparable, IComparable<Measure>
    {

        #region Static Consts

        public static readonly Measure Zero;
        public static readonly Measure Empty = new Measure(PreciseDouble.NaN, null);

        #endregion

        #region Fields

        private PreciseDouble normalizedValue;
        private UnitOfMeasure _Unit;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the default unit of this measure. 
        /// Changing the unit does not affect the measure and the value will be automatically converted to the new unit.
        /// </summary>
        [XmlIgnore]
        public UnitOfMeasure Unit { get { return _Unit; } set { _Unit = value; } }

        [XmlIgnore]
        public PreciseDouble Value
        {
            get
            {
                if (Unit != null)
                    return normalizedValue / Unit.ConversionFactor;
                return normalizedValue;
            }
            set
            {
                if (Unit != null)
                    normalizedValue = value * Unit.ConversionFactor;
                else
                    normalizedValue = value;
            }
        }

        /// <summary>
        /// The value in centimeters
        /// </summary>
        [XmlIgnore]
        public PreciseDouble NormalizedValue { get { return normalizedValue; } }

        [XmlIgnore]
        public PreciseDouble this[UnitOfMeasure unit]
        {
            get
            {
                return normalizedValue / unit.ConversionFactor;
            }
            set
            {
                normalizedValue = value * unit.ConversionFactor;
            }
        }

        //[XmlIgnore, Obsolete("Use IsEmpty")]
        //public bool IsNaN
        //{
        //    get { return PreciseDouble.IsNaN(normalizedValue); }
        //}

        [XmlIgnore]
        public bool IsEmpty
        {
            get { return PreciseDouble.IsNaN(normalizedValue); }
        }

        #endregion

        #region Ctors

        public Measure(PreciseDouble value, UnitOfMeasure unit)
        {
            if (unit != null)
                normalizedValue = value * unit.ConversionFactor;
            else
                normalizedValue = value;
            _Unit = unit;
        }

        #endregion

        #region Static Ctors

        public static Measure FromNormalizedValue(PreciseDouble value, UnitOfMeasure displayUnit)
        {
            return new Measure() { Unit = displayUnit, normalizedValue = value };
        }

        public static Measure Cm(PreciseDouble value) { return new Measure(value, UnitOfMeasure.Cm); }

        public static Measure Mm(PreciseDouble value) { return new Measure(value, UnitOfMeasure.Mm); }

        public static Measure Inches(PreciseDouble value) { return new Measure(value, UnitOfMeasure.Inches); }

        public static Measure Feets(PreciseDouble value) { return new Measure(value, UnitOfMeasure.Feets); }

        #endregion

        #region Equality operators

        public override bool Equals(object obj)
        {
            if (!(obj is Measure))
                return false;
            return this == (Measure)obj;
        }

        public static bool operator ==(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                return m1.IsEmpty == m2.IsEmpty;
            return m1.NormalizedValue.EqualOrClose(m2.NormalizedValue);
        }

        public static bool operator !=(Measure m1, Measure m2)
        {
            return !(m1 == m2);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
            //return normalizedValue.GetHashCode();
        }

        public static bool EqualOrClose(Measure value1, Measure value2, Measure tolerence)
        {
            if (value1.IsEmpty || value2.IsEmpty)
                return value1.IsEmpty == value2.IsEmpty;

            return NumberHelper.EqualOrClose(value1.NormalizedValue, value2.NormalizedValue, tolerence.NormalizedValue);
        }

        #endregion

        #region Comparison operators

        public static bool operator >(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                return false;
            return m1.NormalizedValue > m2.NormalizedValue;
        }

        public static bool operator <(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                return false;
            return m1.NormalizedValue < m2.NormalizedValue;
        }

        public static bool operator >=(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                return false;
            return m1.NormalizedValue >= m2.NormalizedValue;
        }

        public static bool operator <=(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                return false;
            return m1.NormalizedValue <= m2.NormalizedValue;
        }

        public int CompareTo(Measure other)
        {
            if (this < other)
                return -1;
            else if (this > other)
                return 1;
            else if (this == other)
                return 0;
            if (!IsEmpty)
                return 1;
            if (!other.IsEmpty)
                return -1;
            return 0;
        }

        public int CompareTo(object other)
        {
            if (other == null)
                return 1;
            if (!(other is Measure))
                throw new ArgumentException("");
            return CompareTo((Measure)other);
        }

        #endregion

        #region Arithmetic operators

        public static Measure operator *(Measure m1, PreciseDouble value)
        {
            m1.EnsureIsNotNaN();
            return new Measure(m1.Value * value, m1.Unit);
        }

        public static Measure operator *(PreciseDouble value, Measure m1)
        {
            m1.EnsureIsNotNaN();
            return new Measure(m1.Value * value, m1.Unit);
        }

        public static PointM operator *(Maths.Vector value, Measure m)
        {
            m.EnsureIsNotNaN();
            return new PointM(m * value.X, m * value.Y);
        }

        public static Measure operator /(Measure m1, PreciseDouble value)
        {
            m1.EnsureIsNotNaN();
            return new Measure(m1.Value / value, m1.Unit);
        }

        public static PreciseDouble operator /(Measure m1, Measure m2)
        {
            m1.EnsureIsNotNaN();
            m2.EnsureIsNotNaN();
            return m1.normalizedValue / m2.normalizedValue;
        }

        //public static Measure operator /(PreciseDouble value, Measure m1)
        //{
        //    m1.EnsureIsNotNaN();
        //    return new Measure(m1.Value / value, m1.Unit);
        //}

        public static Measure operator +(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                ThrowNaNOperationException();
            return FromNormalizedValue(m1.normalizedValue + m2.normalizedValue, m1.Unit ?? m2.Unit);
        }

        public static Measure operator -(Measure m1, Measure m2)
        {
            if (m1.IsEmpty || m2.IsEmpty)
                ThrowNaNOperationException();
            return FromNormalizedValue(m1.normalizedValue - m2.normalizedValue, m1.Unit ?? m2.Unit);
        }

        #endregion

        #region Functions

        public static Measure Abs(Measure value)
        {
            value.EnsureIsNotNaN();
            return FromNormalizedValue(Math.Abs(value.normalizedValue.DoubleValue), value.Unit);
        }

        public static Measure Avg(Measure value1, Measure value2)
        {
            value1.EnsureIsNotNaN();
            value2.EnsureIsNotNaN();
            return FromNormalizedValue((value1.normalizedValue + value2.normalizedValue) / 2d, value1.Unit ?? value2.Unit);
        }

        public static Measure Min(Measure value1, Measure value2)
        {
            if (value1.IsEmpty)
                return value2;
            else if (value2.IsEmpty)
                return value1;
            return value1 < value2 ? value1 : value2;
        }

        public static Measure Max(Measure value1, Measure value2)
        {
            if (value1.IsEmpty)
                return value2;
            else if (value2.IsEmpty)
                return value1;
            return value1 > value2 ? value1 : value2;
        }

        //public static double SmartConvert(double value, double conv, bool mult)
        //{
        //    double res1 = mult ? value * conv : value / conv;
        //    double res2 = (double)(mult ? (decimal)value * (decimal)conv : (decimal)value / (decimal)conv);
        //    var res1Str = res1.ToString();
        //    var res2Str = res2.ToString();
        //    if (res1Str.Contains("."))
        //        res1Str = res1Str.Substring(res1Str.IndexOf(".") + 1);
        //    else
        //        res1Str = string.Empty;
        //    if (res2Str.Contains("."))
        //        res2Str = res2Str.Substring(res2Str.IndexOf(".") + 1);
        //    else
        //        res2Str = string.Empty;
        //    if (res1Str.Length < res2Str.Length)
        //        return res1;
        //    else
        //        return res2;
        //}

        public static Measure Round(Measure value)
        {
            return new Measure(Math.Round((double)value.Value), value.Unit);
        }

        public static Measure Round(Measure value, double step)
        {
            return new Measure(Math.Round(value.Value / step) * step, value.Unit);
        }

        #endregion

        private void EnsureIsNotNaN()
        {
            if (IsEmpty)
                ThrowNaNOperationException();
        }

        private static void ThrowNaNOperationException()
        {
            throw new InvalidOperationException("You cannot perform any operation on an empty measure.");
        }

        #region ToString

        public class MeasureFormat
        {
            public UnitOfMeasure OverrideUnit { get; set; }
            public int MinimumDecimals { get; set; }
            public int MaximumDecimals { get; set; }

            public bool ShowFractions { get; set; }
            public bool AllowApproximation { get; set; }
            public bool ShowUnitOfMeasure { get; set; }

            public static MeasureFormat DefaultFormat
            {
                get { return new MeasureFormat(); }
            }

            public MeasureFormat()
            {
                MinimumDecimals = 0;
                MaximumDecimals = 3;
                ShowUnitOfMeasure = true;
                ShowFractions = true;
                AllowApproximation = true;
                OverrideUnit = null;
            }

            internal MeasureFormat Clone()
            {
                return new MeasureFormat()
                {
                    AllowApproximation = AllowApproximation,
                    MaximumDecimals = MaximumDecimals,
                    MinimumDecimals = MinimumDecimals,
                    ShowFractions = ShowFractions,
                    ShowUnitOfMeasure = ShowUnitOfMeasure
                };
            }
        }

        public override string ToString()
        {
            return ToString(MeasureFormat.DefaultFormat);
        }

        public string ToString(UnitOfMeasure unit)
        {
            return ToString(new MeasureFormat() { OverrideUnit = unit });
        }

        public string ToString(MeasureFormat format)
        {
            if (IsEmpty)
                return "N/A";

            var usedUnit = format.OverrideUnit ?? Unit;

            string digitFormat = string.Empty;
            if (format.MinimumDecimals > 0 || format.MaximumDecimals > 0)
                digitFormat = ":0.";

            if (format.MinimumDecimals > 0)
                digitFormat = digitFormat.PadRight(3 + format.MinimumDecimals, '0');
            if (format.MaximumDecimals > 0)
                digitFormat = digitFormat.PadRight(3 + format.MaximumDecimals, '#');

            if (usedUnit == null)
                return string.Format("{0" + digitFormat + "}", Value);

            var displayedUnit = format.ShowUnitOfMeasure ? usedUnit.Symbol : string.Empty;

            const double sixtyfourth = 0.015625d;

            if (usedUnit == UnitOfMeasure.Inches && format.ShowFractions)
            {
                var value = this[usedUnit];
                int whole = (int)MathP.Floor(value);
                var remain = value - whole;

                if (remain >= sixtyfourth && remain + sixtyfourth < 1d)
                {
                    int sixtyFourCount = 0;
                    while (remain >= sixtyfourth || Math.Abs(remain - sixtyfourth) < 0.000001)
                    {
                        sixtyFourCount++;
                        remain -= sixtyfourth;
                    }
                    int baseFrac = 64;
                    while ((sixtyFourCount / 2d) % 2d == 0d || (sixtyFourCount / 2d) % 2d == 1d)
                    {
                        baseFrac /= 2;
                        sixtyFourCount /= 2;
                    }

                    if((whole > 0 || remain > sixtyfourth / 2) && (format.AllowApproximation || remain < 0.002))
                    {
                        if (whole == 0)
                            return string.Format("{3}{0}/{1}{2}", sixtyFourCount, baseFrac, displayedUnit, remain > 0.002 ? "~" : string.Empty);
                        else
                            return string.Format("{0} {4}{1}/{2}{3}", whole, sixtyFourCount, baseFrac, displayedUnit, remain > 0.002 ? "~" : string.Empty);
                    }
                }
                else if (remain > 0.002 && remain < sixtyfourth && whole > 0 && format.AllowApproximation)
                    return string.Format("~{0}{1}", whole, displayedUnit);
            }
            else if (usedUnit == UnitOfMeasure.Feets && format.ShowFractions)
            {
                var value = this[usedUnit];
                int whole = (int)MathP.Floor(value);
                var remain = value - whole;
                if (remain >= sixtyfourth / 12d && format.ShowUnitOfMeasure)
                    return string.Format("{0}{1} {2}", whole, usedUnit.Symbol, Inches(remain * 12d).ToString(format.Clone()));
                else if (whole > 0 && remain > 0 && remain < sixtyfourth / 12d && format.AllowApproximation)
                    return string.Format("~{0}{1}", whole, displayedUnit);
            }

            return string.Format("{0" + digitFormat + "}{1}", this[usedUnit], displayedUnit);
        }

        #endregion

        #region Serialization

        private Measure(SerializationInfo info, StreamingContext context)
        {
            _Unit = UnitOfMeasure.GetUnitByName(info.GetString("Unit"));
            normalizedValue = info.GetDouble("Value") * _Unit.ConversionFactor;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Unit", Unit.Name);
            info.AddValue("Value", Value);
        }

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            var value = double.Parse(reader.GetAttribute("Value"));
            _Unit = UnitOfMeasure.GetUnitByName(reader.GetAttribute("Unit"));
            if (_Unit == null)
                normalizedValue = value;
            else
                normalizedValue = _Unit.ConversionFactor * value;
        }

        public void WriteXml(XmlWriter writer)
        {
            if (writer.WriteState == WriteState.Element)
            {
                writer.WriteStartAttribute("Value");
                writer.WriteValue(Value);
                writer.WriteEndAttribute();
                writer.WriteStartAttribute("Unit");
                writer.WriteValue(Unit.Name);
                writer.WriteEndAttribute();
            }
        }

        public System.Xml.Linq.XAttribute SerializeAsAttribute(string name)
        {
            if(IsEmpty)
                return new System.Xml.Linq.XAttribute(name, "N/A");
            return new System.Xml.Linq.XAttribute(name, string.Format(NumberFormatInfo.InvariantInfo, "{0}{1}", Value, Unit != null ? Unit.Abbreviation : string.Empty));
        }

        public static Measure Parse(string value)
        {
            return Utilities.MeasureParser.Parse(value);
        }

		public static Measure ParseInvariant(string value)
		{
			return Parse(value, NumberFormatInfo.InvariantInfo);
		}

		public static Measure Parse(string value, IFormatProvider provider)
		{
			return Utilities.MeasureParser.Parse(value, provider);
		}

		public static bool TryParse(string value, out Measure measure)
        {
            return Utilities.MeasureParser.TryParse(value, out measure);
        }

		public static bool TryParse(string value, IFormatProvider provider, out Measure measure)
		{
			return Utilities.MeasureParser.TryParse(value, out measure);
		}

		public static Measure TryParse(string value, Measure fallback)
        {
			if (Utilities.MeasureParser.TryParse(value, out Measure result))
				return result;
			return fallback;
        }

		public static Measure TryParse(string value, IFormatProvider provider, Measure fallback)
		{
			if (Utilities.MeasureParser.TryParse(value, provider, out Measure result))
				return result;
			return fallback;
		}

		#endregion
	}
}
