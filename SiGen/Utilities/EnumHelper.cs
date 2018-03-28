using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace SiGen.Utilities
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
    }
}
