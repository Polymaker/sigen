using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System
{
    public static class EnumHelper
    {
        public static string[] GetEnumDescriptions(Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);

            for(int i = 0; i < enumNames.Length; i++)
            {
                var enumValMember = enumType.GetMember(enumNames[i])[0];
                var descAttr = enumValMember.GetAttribute<DescriptionAttribute>();
                if (descAttr != null)
                    enumNames[i] = descAttr.Description;
            }
            return enumNames;
        }

        public static string GetEnumDescription(Type enumType, object value)
        {
            var enumName = Enum.GetName(enumType, value);

            var enumValMember = enumType.GetMember(enumName)[0];
            var descAttr = enumValMember.GetAttribute<DescriptionAttribute>();
            if (descAttr != null)
                return descAttr.Description;

            return enumName;
        }

        public class EnumItem
        {
            public const string ValueMember = "Value";
            public const string DisplayMember = "Description";

            private readonly object _Value;
            private readonly string _Name;
            private readonly string _Description;
            private readonly Type _EnumType;

            public object Value { get { return _Value; } }
            public string Name { get { return _Name; } }
            public string Description { get { return _Description; } }

            public EnumItem(Type enumType, object value)
            {
                _EnumType = enumType;
                _Name = value.ToString();
                _Value = value;
                _Description = GetEnumDescription(enumType, value);
            }

            public EnumItem(Type enumType, object value, string description)
            {
                _EnumType = enumType;
                _Name = value.ToString();
                _Value = value;
                _Description = description;
            }

            public EnumItem(object value, string description)
            {
                _EnumType = value.GetType();
                _Name = value.ToString();
                _Value = value;
                _Description = description;
            }
        }

        public static List<EnumItem> GetEnumItemCollection(Type enumType)
        {
            var enumValues = Enum.GetValues(enumType);
            var itemList = new List<EnumItem>();
            for (int i = 0; i < enumValues.Length; i++)
                itemList.Add(new EnumItem(enumType, enumValues.GetValue(i), GetEnumDescription(enumType, enumValues.GetValue(i))));
            return itemList;
        }
    }
}
