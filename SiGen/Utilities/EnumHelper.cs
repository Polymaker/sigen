﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System
{
    public class OldValueAttribute : Attribute
    {
        public string Value { get; set; }

        public int BeforeVersion { get; set; } = 1;

        public OldValueAttribute(string value)
        {
            Value = value;
        }
    }

    public static class EnumHelper
    {
        public static int CurrentLayoutVersion { get; set; }

        public static T Parse<T>(string value) where T : Enum
        {
            var enumType = typeof(T);
            var enumNames = Enum.GetNames(enumType);

            for (int i = 0; i < enumNames.Length; i++)
            {
                if (enumNames[i].ToLower() == value.ToLower())
                    return (T)Enum.Parse(enumType, enumNames[i]);

                //var enumValMember = enumType.GetMember(enumNames[i])[0];

                //var oldValAttr = enumValMember.GetCustomAttribute<OldValueAttribute>();
                //if (oldValAttr != null && oldValAttr.Value.ToLower() == value.ToLower())
                //    return (T)Enum.Parse(enumType, enumNames[i]);
            }

            return default(T);
        }

        public static object Parse(Type enumType, string value)
        {
            var enumNames = Enum.GetNames(enumType);

            for (int i = 0; i < enumNames.Length; i++)
            {
                if (enumNames[i].ToLower() == value.ToLower())
                    return Enum.Parse(enumType, enumNames[i], true);

                //var enumValMember = enumType.GetMember(enumNames[i])[0];
                //var oldValAttr = enumValMember.GetCustomAttribute<OldValueAttribute>();

                //if (oldValAttr != null && oldValAttr.Value.ToLower() == value.ToLower())
                //    return Enum.Parse(enumType, enumNames[i], true);
            }

            throw new ArgumentException();
        }

        public static string[] GetEnumDescriptions(Type enumType)
        {
            var enumNames = Enum.GetNames(enumType);

            for(int i = 0; i < enumNames.Length; i++)
            {
                var enumValMember = enumType.GetMember(enumNames[i])[0];
                var descAttr = enumValMember.GetCustomAttribute<DescriptionAttribute>();
                if (descAttr != null)
                    enumNames[i] = descAttr.Description;
            }
            return enumNames;
        }

        public static string GetEnumDescription(Type enumType, object value)
        {
            var enumName = Enum.GetName(enumType, value);

            var enumValMember = enumType.GetMember(enumName)[0];
            var descAttr = enumValMember.GetCustomAttribute<DescriptionAttribute>();
            if (descAttr != null)
                return descAttr.Description;

            return enumName;
        }

        public class EnumItem
        {
            public const string ValueMember = "Value";
            public const string DisplayMember = "Description";

            public object Value { get; }
            public string Name { get; }
            public string Description { get; set; }

            public Type EnumType { get; }

            public EnumItem(Type enumType, object value)
            {
                EnumType = enumType;
                Name = value.ToString();
                Value = value;
                Description = GetEnumDescription(enumType, value);
            }

            public EnumItem(Type enumType, object value, string description)
            {
                EnumType = enumType;
                Name = value.ToString();
                Value = value;
                Description = description;
            }

            public EnumItem(object value, string description)
            {
                EnumType = value.GetType();
                Name = value.ToString();
                Value = value;
                Description = description;
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
