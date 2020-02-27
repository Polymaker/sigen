using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace System.Xml.Linq
{
    public static class XmlLinqExtensions
    {
        public static XElement SortAttributes(this XElement element, Func<XAttribute, int> predicate)
        {
            var elemAttrs = element.Attributes().ToArray();
            element.RemoveAttributes();
            elemAttrs = elemAttrs.OrderBy(x => predicate(x)).ToArray();
            element.Add(elemAttrs);
            return element;
        }

        public static int GetIntAttribute(this XElement element, XName attributeName)
        {
            var attribute = element.Attribute(attributeName);
            return int.Parse(attribute.Value);
        }

        public static bool ContainsElement(this XElement element, XName childElementName)
        {
            return element.Element(childElementName) != null;
        }

        public static bool ContainsAttribute(this XElement element, XName attributeName)
        {
            return element.Attribute(attributeName) != null;
        }

        //public static T GetAttributeValue<T>(this XElement element, XName attributeName)
        //{

        //}

        #region Attributes

        public static XAttribute GetAttribute(this XElement element, string name)
        {
            return element.Attributes().FirstOrDefault(x => x.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool HasAttribute(this XElement element, string attributeName, out XAttribute attribute)
        {
            attribute = GetAttribute(element, attributeName);
            return attribute != null;
        }

        public static bool HasAttribute(this XElement element, string attributeName)
        {
            return HasAttribute(element, attributeName, out _);
        }

        public static bool TryReadAttribute<T>(this XElement element, string attributeName, out T result)
        {
            result = default;
            var attr = GetAttribute(element, attributeName);
            if (attr == null)
                return false;

            if (typeof(T) == typeof(int) &&
                int.TryParse(attr.Value, out int intVal))
            {
                result = (T)(object)intVal;
                return true;
            }
            else if (typeof(T) == typeof(float) &&
                float.TryParse(attr.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                result = (T)(object)floatVal;
                return true;
            }
            else if (typeof(T) == typeof(double) &&
                double.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out double dblVal))
            {
                result = (T)(object)dblVal;
                return true;
            }
            else if (typeof(T) == typeof(decimal) &&
                decimal.TryParse(attr.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decVal))
            {
                result = (T)(object)decVal;
                return true;
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T)(object)attr.Value;
                return true;
            }
            else if (typeof(T) == typeof(Measure))
            {
                if (Measure.TryParse(attr.Value, NumberFormatInfo.InvariantInfo, out Measure measure))
                {
                    result = (T)(object)measure;
                    return true;
                }
                return false;
            }
            else if (typeof(T) == typeof(bool))
            {
                switch (attr.Value.Trim().ToLower())
                {
                    case "1":
                    case "true":
                    case "yes":
                        result = (T)(object)true;
                        return true;
                    case "0":
                    case "false":
                    case "no":
                        result = (T)(object)false;
                        return true;
                }
            }
            else if (typeof(T).IsEnum)
            {
                if (int.TryParse(attr.Value, out int intEnumVal) &&
                    Enum.IsDefined(typeof(T), intEnumVal))
                {
                    result = (T)Enum.ToObject(typeof(T), intEnumVal);
                    return true;
                }
                try
                {
                    result = (T)EnumHelper.Parse(typeof(T), attr.Value);
                    return true;
                }
                catch { }
            }

            return false;
        }

        public static T ReadAttribute<T>(this XElement element, string attributeName, T defaultValue)
        {
            if (TryReadAttribute(element, attributeName, out T result))
                return result;
            return defaultValue;
        }

        public static T ReadAttribute<T>(this XElement element, string attributeName)
        {
            if (TryReadAttribute(element, attributeName, out T result))
                return result;

            if (element.HasAttribute(attributeName))
                throw new InvalidCastException($"The value '{element.Attribute(attributeName).Value}' could not be converted to {typeof(T).Name}");

            throw new KeyNotFoundException($"The attribute '{attributeName}' was not found");
        }

        #endregion

        #region Elements

        public static XElement GetElement(this XElement element, string name)
        {
            return element.Elements().FirstOrDefault(x => x.Name.LocalName.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public static bool HasElement(this XElement parentElem, string elementName, out XElement element)
        {
            element = GetElement(parentElem, elementName);
            return element != null;
        }

        public static bool HasElement(this XElement parentElem, string elementName)
        {
            return HasElement(parentElem, elementName, out _);
        }

        public static bool TryReadElement<T>(this XElement element, string elementName, out T result)
        {
            result = default;
            var elem = GetElement(element, elementName);
            if (elem == null)
                return false;

            if (typeof(T) == typeof(int) &&
                int.TryParse(elem.Value, out int intVal))
            {
                result = (T)(object)intVal;
                return true;
            }
            else if (typeof(T) == typeof(float) &&
                float.TryParse(elem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatVal))
            {
                result = (T)(object)floatVal;
                return true;
            }
            else if (typeof(T) == typeof(double) &&
                double.TryParse(elem.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out double dblVal))
            {
                result = (T)(object)dblVal;
                return true;
            }
            else if (typeof(T) == typeof(decimal) &&
                decimal.TryParse(elem.Value, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal decVal))
            {
                result = (T)(object)decVal;
                return true;
            }
            else if (typeof(T) == typeof(string))
            {
                result = (T)(object)elem.Value;
                return true;
            }
            else if (typeof(T) == typeof(Measure))
            {
                if (Measure.TryParse(elem.Value, NumberFormatInfo.InvariantInfo, out Measure measure))
                {
                    result = (T)(object)measure;
                    return true;
                }
                return false;
            }
            else if (typeof(T) == typeof(bool))
            {
                switch (elem.Value.Trim().ToLower())
                {
                    case "1":
                    case "true":
                    case "yes":
                        result = (T)(object)true;
                        return true;
                    case "0":
                    case "false":
                    case "no":
                        result = (T)(object)false;
                        return true;
                }
            }
            else if (typeof(T).IsEnum)
            {
                if (int.TryParse(elem.Value, out int intEnumVal) &&
                    Enum.IsDefined(typeof(T), intEnumVal))
                {
                    result = (T)Enum.ToObject(typeof(T), intEnumVal);
                    return true;
                }
                try
                {
                    result = (T)EnumHelper.Parse(typeof(T), elem.Value);
                    return true;
                }
                catch { }
            }

            return false;
        }

        public static T ReadElement<T>(this XElement element, string elementName, T defaultValue)
        {
            if (TryReadElement(element, elementName, out T result))
                return result;
            return defaultValue;
        }

        public static T ReadElementAttribute<T>(this XElement element, string elementName, string attributeName, T defaultValue)
        {
            var elem = GetElement(element, elementName);
            if (TryReadAttribute(elem, attributeName, out T result))
                return result;
            return defaultValue;
        }

        #endregion
    }
}
