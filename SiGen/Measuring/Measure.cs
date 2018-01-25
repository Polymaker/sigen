using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace SiGen.Measuring
{
    [Serializable]
    public struct Measure : ISerializable, IXmlSerializable
    {

        #region Static Consts

        public static readonly Measure Zero;
        public static readonly Measure Empty = new Measure(double.NaN, null);

        #endregion

        #region Fields

        private double normalizedValue;
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
        public double Value
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
        public double NormalizedValue { get { return normalizedValue; } }

        [XmlIgnore]
        public double this[UnitOfMeasure unit]
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

        [XmlIgnore, Obsolete("Use IsEmpty")]
        public bool IsNaN
        {
            get { return double.IsNaN(normalizedValue); }
        }

        [XmlIgnore]
        public bool IsEmpty
        {
            get { return double.IsNaN(normalizedValue); }
        }

        #endregion

        #region Ctors

        public Measure(double value, UnitOfMeasure unit)
        {
            if (unit != null)
                normalizedValue = value * unit.ConversionFactor;
            else
                normalizedValue = value;
            _Unit = unit;
        }

        #endregion

        #region Static Ctors

        public static Measure FromNormalizedValue(double value, UnitOfMeasure displayUnit)
        {
            return new Measure() { Unit = displayUnit, normalizedValue = value };
        }

        public static Measure Cm(double value) { return new Measure(value, UnitOfMeasure.Cm); }

        public static Measure Mm(double value) { return new Measure(value, UnitOfMeasure.Mm); }

        public static Measure Inches(double value) { return new Measure(value, UnitOfMeasure.Inches); }

        public static Measure Feets(double value) { return new Measure(value, UnitOfMeasure.Feets); }

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
            return m1.NormalizedValue > m2.NormalizedValue;
        }

        public static bool operator <(Measure m1, Measure m2)
        {
            return m1.NormalizedValue < m2.NormalizedValue;
        }

        public static bool operator >=(Measure m1, Measure m2)
        {
            return m1.NormalizedValue >= m2.NormalizedValue;
        }

        public static bool operator <=(Measure m1, Measure m2)
        {
            return m1.NormalizedValue <= m2.NormalizedValue;
        }

        #endregion

        #region Arithmetic operators

        public static Measure operator *(Measure m1, double value)
        {
            m1.EnsureIsNotNaN();
            return new Measure(m1.Value * value, m1.Unit);
        }

        public static Measure operator *(double value, Measure m1)
        {
            m1.EnsureIsNotNaN();
            return new Measure(m1.Value * value, m1.Unit);
        }

        public static Measure operator /(Measure m1, double value)
        {
            m1.EnsureIsNotNaN();
            return new Measure(m1.Value / value, m1.Unit);
        }

        //public static Measure operator /(double value, Measure m1)
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
            return FromNormalizedValue(Math.Abs(value.normalizedValue), value.Unit);
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

        public override string ToString()
        {
            return ToString(Unit);
        }

        public string ToString(UnitOfMeasure unit, bool forceDecimal = false)
        {
            if (unit == null)
                return Value.ToString();

            if (forceDecimal)
                return string.Format("{0:0.###}{1}", this[unit], unit.Symbol);

            const double sixtyfourth = 0.015625d;

            if (unit == UnitOfMeasure.Inches)
            {
                double value = this[unit];
                int whole = (int)Math.Floor(value);
                double remain = value - whole;
                if (remain >= sixtyfourth)
                {
                    int sixtyFourCount = 0;
                    while (remain >= 0.015625d)
                    {
                        sixtyFourCount++;
                        remain -= 0.015625d;
                    }
                    int baseFrac = 64;
                    while ((sixtyFourCount / 2d) % 2d == 0d || (sixtyFourCount / 2d) % 2d == 1d)
                    {
                        baseFrac /= 2;
                        sixtyFourCount /= 2;
                    }

                    if (whole == 0)
                        return string.Format("{3}{0}/{1}{2}", sixtyFourCount, baseFrac, unit.Symbol, remain > 0.002 ? "~" : string.Empty);
                    return string.Format("{0} {4}{1}/{2}{3}", whole, sixtyFourCount, baseFrac, unit.Symbol, remain > 0.002 ? "~" : string.Empty);
                }
                else if (remain > 0 && whole > 0)
                    return string.Format("~{0}{1}", whole, unit.Symbol);
            }
            else if (unit == UnitOfMeasure.Feets)
            {
                double value = this[unit];
                int whole = (int)Math.Floor(value);
                double remain = value - whole;
                if (remain > sixtyfourth / 12d)
                {
                    return string.Format("{0}{1} {2}", whole, unit.Symbol, Inches(remain * 12d));
                }
                else if (remain > 0 && whole > 0)
                    return string.Format("~{0}{1}", whole, unit.Symbol);
            }
            return string.Format("{0:0.##}{1}", this[unit], unit.Symbol);
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

        #endregion
    }
}
