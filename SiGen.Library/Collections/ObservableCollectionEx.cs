using System.Linq;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace System.Collections.Generic
{
    public class ObservableCollectionEx<T> : ObservableCollection<T>
    {
        private bool _RaiseListChangedEvents;

        public bool RaiseListChangedEvents
        {
            get { return _RaiseListChangedEvents; }
            set
            {
                if (value != _RaiseListChangedEvents)
                    _RaiseListChangedEvents = value;
            }
        }

        public ObservableCollectionEx()
        {
            _RaiseListChangedEvents = true;
        }

        public ObservableCollectionEx(List<T> list)
            : base(list)
        {
            _RaiseListChangedEvents = true;
        }

        public ObservableCollectionEx(IEnumerable<T> collection)
            : base(collection)
        {
            _RaiseListChangedEvents = true;
        }

        public void AddRange(IEnumerable<T> elements)
        {
            CheckReentrancy();
            var raiseEvent = _RaiseListChangedEvents;
            _RaiseListChangedEvents = false;
            var items = elements.ToArray();
            foreach (var item in items)
                Add(item);
            _RaiseListChangedEvents = raiseEvent;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, items));
        }

        public void Reverse()
        {
            CheckReentrancy();
            var raiseEvent = _RaiseListChangedEvents;
            _RaiseListChangedEvents = false;
            int itemCount = Count;
            for (int i = 0; i < itemCount; i++)
            {
                var itm = this[0];
                RemoveAt(0);
                if (i == 0)
                    Add(itm);
                else
                    Insert(itemCount - i - 1, itm);
            }
            _RaiseListChangedEvents = raiseEvent;
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (RaiseListChangedEvents)
                base.OnCollectionChanged(e);
        }
    }
}
