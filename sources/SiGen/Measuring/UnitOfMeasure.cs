﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Measuring
{
    [System.ComponentModel.TypeConverter(typeof(UnitOfMeasureConverter))]
    public class UnitOfMeasure
    {
        public const double CmToInch = 1d / 2.54d;
        //0.3937007874015748d
        //0.3937007874015748031496062992126d

        #region Fields

        private string _Name;
        private string _Abreviation;
        private string _Symbol;
        private double _ConversionFactor;

        #endregion

        #region Properties

        public string Name { get { return _Name; } }

        public string Abreviation { get { return _Abreviation; } }

        public string Symbol { get { return _Symbol; } }

        public double ConversionFactor { get { return _ConversionFactor; } }

        #endregion

        private UnitOfMeasure(string name, string symbol, string abv, double norm)
        {
            _Name = name;
            _Abreviation = abv;
            _Symbol = symbol;
            _ConversionFactor = norm;
        }

        public static readonly UnitOfMeasure Millimeters = new UnitOfMeasure("Millimeters", "mm", "mm", 0.1d);
        public static readonly UnitOfMeasure Centimeters = new UnitOfMeasure("Centimeters", "cm", "cm", 1d);

        public static readonly UnitOfMeasure Inches = new UnitOfMeasure("Inches", "\"", "in", 2.54d);
        public static readonly UnitOfMeasure Feets = new UnitOfMeasure("Feets", "'", "ft", (2.54d) * 12d);

        public static UnitOfMeasure Mm { get { return Millimeters; } }
        public static UnitOfMeasure Cm { get { return Centimeters; } }
        public static UnitOfMeasure In { get { return Inches; } }
        public static UnitOfMeasure Ft { get { return Feets; } }

        public static double ConvertTo(double value, UnitOfMeasure from, UnitOfMeasure to)
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
        }

        #endregion
    }
}