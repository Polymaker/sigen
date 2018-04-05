using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Collections.Generic
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private bool suppressChange;

        public void AddRange(IEnumerable<T> elements)
        {
            suppressChange = true;
            var items = elements.ToArray();
            foreach (var item in items)
                Add(item);
            suppressChange = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void Reverse()
        {
            suppressChange = true;
            int itemCount = Count;
            for(int i = 0; i < itemCount; i++)
            {
                var itm = this[0];
                RemoveAt(0);
                if (i == 0)
                    Add(itm);
                else
                    Insert(itemCount - i - 1, itm);
            }
            suppressChange = false;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if(!suppressChange)
                base.OnCollectionChanged(e);
        }
    }
}
