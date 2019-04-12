using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Reflection;

namespace SiGen.Utilities
{
    public static class SerializationHelper
    {
        public static XAttribute SerializeAsAttribute<T>(string name, T value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if(converter != null && converter.CanConvertTo(typeof(string)))
                return new XAttribute(name, converter.ConvertTo(value, typeof(string)));

            return new XAttribute(name, value.ToString());
        }

        public static T Parse<T>(XAttribute attribute)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            if (converter != null && converter.CanConvertFrom(typeof(string)))
                return (T)converter.ConvertFrom(attribute.Value);

            return default(T);
        }

        //public static XElement GenericSerialize<T>(T obj, string name)
        //{
        //    var elem = new XElement(name);
        //    foreach(var propInfo in typeof(T).GetProperties())
        //    {
        //        var xmlAttr = (XmlAttribute[])propInfo.GetCustomAttributes(typeof(XmlAttribute), true);
        //        var xmlElem = (XmlAttribute[])propInfo.GetCustomAttributes(typeof(XmlAttribute), true);
        //        if (xmlAttr.Length > 0)
        //            elem.Add(new XAttribute(xmlAttr[0].Name, SerializeValue(propInfo.GetValue(obj))));
        //        if (xmlElem.Length > 0)
        //            elem.Add(new XAttribute(xmlAttr[0].Name, SerializeValue(propInfo.GetValue(obj))));
        //    }
        //    return elem;
        //}

        public static XElement GenericSerialize(object obj, string name)
        {
            var elem = new XElement(name);
            foreach (var propInfo in obj.GetType().GetProperties())
            {
                var xmlAttr = propInfo.GetCustomAttribute<XmlAttributeAttribute>(true);
                var xmlElem = propInfo.GetCustomAttribute<XmlElementAttribute>(true);

                if (xmlAttr != null)
                    elem.Add(new XAttribute(xmlAttr.AttributeName, SerializeValue(propInfo.GetValue(obj))));

                if (xmlElem != null)
                {
                    object propVal = propInfo.GetValue(obj);
                    if(propVal != null)
                        elem.Add(GenericSerialize(propVal, xmlElem.ElementName));
                }
            }
            return elem;
        }

        public static string SerializeValue(object value)
        {
            if (value is Measuring.Measure)
            {
                var measure = (Measuring.Measure)value;
                if (measure.IsEmpty)
                    return "N/A";
                return string.Format(NumberFormatInfo.InvariantInfo,"{0}{1}", measure.Value, measure.Unit != null ? measure.Unit.Abreviation : string.Empty);
            }
            else if(value is double dblvalue)
            {
                return dblvalue.ToString(NumberFormatInfo.InvariantInfo);
            }
            else if (value is decimal decValue)
            {
                return decValue.ToString(NumberFormatInfo.InvariantInfo);
            }

            if (value == null)
                return string.Empty;

            return value.ToString();
        }

        public static object DeserializeValue(Type type, string value)
        {
            var converter = TypeDescriptor.GetConverter(type);
            if (converter != null && converter.CanConvertFrom(typeof(string)))
                return converter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
            return value.ToString();
        }

        //private static 

        public static void GenericDeserialize(object obj, XElement elem)
        {
            foreach (var propInfo in obj.GetType().GetProperties())
            {
                if (!propInfo.CanWrite)
                    continue;

                var xmlAttr = propInfo.GetCustomAttribute<XmlAttributeAttribute>(true);
                var xmlElem = propInfo.GetCustomAttribute<XmlElementAttribute>(true);

                if (xmlAttr != null && elem.ContainsAttribute(xmlAttr.AttributeName))
                    propInfo.SetValue(obj, DeserializeValue(propInfo.PropertyType, elem.Attribute(xmlAttr.AttributeName).Value));

                if (xmlElem != null && elem.ContainsElement(xmlElem.ElementName))
                {
                    object propVal = Activator.CreateInstance(propInfo.PropertyType);
                    GenericDeserialize(propVal, elem.Element(xmlElem.ElementName));
                    propInfo.SetValue(obj, propVal);
                }
            }
        }

        public static T GenericDeserialize<T>(XElement elem)
        {
            T obj = Activator.CreateInstance<T>();
            foreach (var propInfo in typeof(T).GetProperties())
            {
                if (!propInfo.CanWrite)
                    continue;

                var xmlAttr = (XmlAttributeAttribute[])propInfo.GetCustomAttributes(typeof(XmlAttributeAttribute), true);
                var xmlElem = (XmlElementAttribute[])propInfo.GetCustomAttributes(typeof(XmlElementAttribute), true);

                if (xmlAttr.Length > 0 && elem.ContainsAttribute(xmlAttr[0].AttributeName))
                    propInfo.SetValue(obj, DeserializeValue(propInfo.PropertyType, elem.Attribute(xmlAttr[0].AttributeName).Value));

                if (xmlElem.Length > 0 && elem.ContainsElement(xmlElem[0].ElementName))
                {
                    object propVal = Activator.CreateInstance(propInfo.PropertyType);
                    GenericDeserialize(propVal, elem.Element(xmlElem[0].ElementName));
                    propInfo.SetValue(obj, propVal);
                }
            }
            return obj;
        }

    }
}
