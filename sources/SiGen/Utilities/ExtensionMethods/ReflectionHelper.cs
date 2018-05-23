using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace System.Reflection
{
    public static class ReflectionHelper
    {
        public static T GetAttribute<T>(this MemberInfo mem, bool inherit = true) where T : Attribute
        {
            var attrs = (T[])mem.GetCustomAttributes(typeof(T), inherit);
            if (attrs.Length > 0)
                return attrs[0];
            return default(T);
        }

        public static T[] GetAttributes<T>(this MemberInfo mem, bool inherit = true) where T : Attribute
        {
            var attrs = (T[])mem.GetCustomAttributes(typeof(T), inherit);
            if (attrs.Length > 0)
                return attrs;
            return new T[0];
        }
    }
}
