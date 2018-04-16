using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Utilities
{
    /// <summary>
    /// A simple class that enables to make dynamic indexed propterties.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="K"></typeparam>
    public class ArrayProperty<T,K>
    {
        private Func<K, T> getPredicate;
        private Action<K, T> setPredicate;

        public T this[K key]
        {
            get
            {
                if (getPredicate == null)
                    throw new NotImplementedException();
                else
                    return getPredicate(key);
            }
            set
            {
                if (setPredicate == null)
                    throw new NotImplementedException();
                else
                    setPredicate(key, value);
            }
        }

        public ArrayProperty(Func<K, T> getMethod, Action<K, T> setMethod)
        {
            getPredicate = getMethod;
            setPredicate = setMethod;
        }

        public ArrayProperty(Func<K, T> getMethod) : this(getMethod, null) { }
    }
}
