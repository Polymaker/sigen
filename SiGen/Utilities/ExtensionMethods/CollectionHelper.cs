using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace System.Collections.Generic
{
    public static class CollectionHelper
    {
        public static bool AllEqual<T>(this IEnumerable<T> list, Func<T, object> predicate)
        {
            bool isFirst = true;
            object value = null;
            foreach (T obj in list)
            {
                if (isFirst)
                {
                    value = predicate(obj);
                    isFirst = false;
                }
                else if (!Equals(value, predicate(obj)))
                    return false;
            }
            return true;
        }

        public static void MassAssign<T, V>(this IEnumerable<T> list, Expression<Func<T, V>> assign, params V[] values)
        {
            if (values.Length != list.Count())
                throw new InvalidOperationException("The number of values does not correspond to the number of item in the list.");

            if (assign.Body.NodeType == ExpressionType.MemberAccess)
            {
                var bodyExp = (MemberExpression)assign.Body;

                var memberInfo = bodyExp.Member;
                var propertyInfo = memberInfo as PropertyInfo;
                var fieldInfo = memberInfo as FieldInfo;
                int ctr = 0;

                foreach (T obj in list)
                {
                    if (propertyInfo != null)
                        propertyInfo.SetValue(obj, values[ctr++], null);
                    else if (fieldInfo != null)
                        fieldInfo.SetValue(obj, values[ctr++]);
                }
            }
        }

        public static void SetAll<T, V>(this IEnumerable<T> list, Expression<Func<T, V>> assign, V valueToSet)
        {
            if (assign.Body.NodeType == ExpressionType.MemberAccess)
            {
                var bodyExp = (MemberExpression)assign.Body;

                var memberInfo = bodyExp.Member;
                var propertyInfo = memberInfo as PropertyInfo;
                var fieldInfo = memberInfo as FieldInfo;

                foreach (T obj in list)
                {
                    if (propertyInfo != null)
                        propertyInfo.SetValue(obj, valueToSet, null);
                    else if (fieldInfo != null)
                        fieldInfo.SetValue(obj, valueToSet);
                }
            }
        }

        public static V Sum<T, V>(this IEnumerable<T> list, Func<T, V> predicate)
        {
            var addOpp = typeof(V).GetMethod("op_Addition", new Type[] { typeof(V), typeof(V) });
            if (addOpp != null)
            {
                V total = default(V);
                foreach (T obj in list)
                    total = (V)addOpp.Invoke(null, new object[] { total, predicate(obj) });
                return total;
            }
            else
                throw new NotSupportedException("The type does not support addition.");

        }

        public static V Max2<T, V>(this IEnumerable<T> list, Func<T, V> predicate)
        {
            var gtOpp = typeof(V).GetMethod("op_GreaterThan", new Type[] { typeof(V), typeof(V) });
            if (gtOpp != null)
            {
                V maxValue = default(V);
                bool isFirst = true;

                foreach (T obj in list)
                {
                    V curValue = predicate(obj);
                    if (isFirst)
                    {
                        maxValue = curValue;
                        isFirst = false;
                    }

                    if ((bool)gtOpp.Invoke(null, new object[] { curValue, maxValue }))
                        maxValue = curValue;
                }

                return maxValue;
            }
            else
                throw new NotSupportedException("The type does not support comparison.");
        }

        public static V Min2<T, V>(this IEnumerable<T> list, Func<T, V> predicate)
        {
            var ltOpp = typeof(V).GetMethod("op_LessThan", new Type[] { typeof(V), typeof(V) });
            if (ltOpp != null)
            {
                V minValue = default(V);
                bool isFirst = true;

                foreach (T obj in list)
                {
                    V curValue = predicate(obj);
                    if (isFirst)
                    {
                        minValue = curValue;
                        isFirst = false;
                    }
                    if ((bool)ltOpp.Invoke(null, new object[] { curValue, minValue }))
                        minValue = curValue;
                }

                return minValue;
            }
            else
                throw new NotSupportedException("The type does not support comparison.");
        }
    }
}
