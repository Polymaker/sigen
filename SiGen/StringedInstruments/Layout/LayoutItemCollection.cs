using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public interface ILayoutItemCollection
    {
        SILayout Layout { get; }
        LayoutComponent ParentComponent { get; }
    }

    public class LayoutItemCollection<T> : IList<T>, IList, ILayoutItemCollection
    {
        private SILayout _Layout;
        private List<T> InnerList;

        public LayoutComponent ParentComponent { get; }

        public SILayout Layout => ParentComponent?.Layout ?? _Layout;

        public int Count => ((IList<T>)InnerList).Count;

        public bool IsReadOnly => ((IList<T>)InnerList).IsReadOnly;

        public bool IsFixedSize => ((IList)InnerList).IsFixedSize;

        public object SyncRoot => ((IList)InnerList).SyncRoot;

        public bool IsSynchronized => ((IList)InnerList).IsSynchronized;

        object IList.this[int index] { get => ((IList)InnerList)[index]; set => ((IList)InnerList)[index] = value; }
        
        public T this[int index] { get => InnerList[index]; set => InnerList[index] = value; }

        public event EventHandler<CollectionChange> CollectionChanged;

        public LayoutItemCollection(LayoutComponent parentComponent)
        {
            ParentComponent = parentComponent;
            InnerList = new List<T>();
        }

        public LayoutItemCollection(SILayout layout)
        {
            _Layout = layout;
            InnerList = new List<T>();
        }

        protected void OnCollectionChanged(CollectionChange e)
        {
            if (Layout != null)
                Layout.NotifyLayoutChanged(e);
            CollectionChanged?.Invoke(this, e);
        }

        protected void RaiseCollectionChanged(
           CollectionChangeAction action,
           IEnumerable<CollectionItemChangeInfo> changedItems)
        {
            OnCollectionChanged(new CollectionChange(this, action, changedItems.ToArray()));
        }

        protected void RaiseCollectionChanged(
           CollectionChangeAction action,
           CollectionItemChangeInfo changedItem)
        {
            OnCollectionChanged(new CollectionChange(this, action,
                new CollectionItemChangeInfo[] { changedItem }));
        }

        private void UpdateItemParent(LayoutComponent item, bool adding)
        {
            if (item != null && Layout != null)
                item.SetLayout(adding ? Layout : null);
        }

        protected CollectionItemChangeInfo InsertItem(int index, T item)
        {
            if (index > Count)
                index = Count;
            UpdateItemParent(item as LayoutComponent, true);
            InnerList.Insert(index, item);
            return new CollectionItemChangeInfo(item, -1, index);
        }

        protected CollectionItemChangeInfo AddItem(T item)
        {
            UpdateItemParent(item as LayoutComponent, true);
            InnerList.Add(item);
            return new CollectionItemChangeInfo(item, -1, Count - 1);
        }

        public int IndexOf(T item)
        {
            return InnerList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            var addedItem = InsertItem(index, item);
            RaiseCollectionChanged(CollectionChangeAction.Add, addedItem);
        }

        public void Add(T item)
        {
            var addedItem = AddItem(item);
            RaiseCollectionChanged(CollectionChangeAction.Add, addedItem);
        }

        public void AddRange(IEnumerable<T> items)
        {
            var addedItems = new List<CollectionItemChangeInfo>();

            try
            {
                foreach (var item in items)
                    addedItems.Add(AddItem(item));
            }
            finally
            {
                RaiseCollectionChanged(CollectionChangeAction.Add, addedItems);
            }
        }

        #region Remove items

        protected CollectionItemChangeInfo RemoveItem(T item, bool runDry = false)
        {
            int itemIndex = IndexOf(item);

            if (itemIndex >= 0)
            {
                if (!runDry)
                {
                    InnerList.RemoveAt(itemIndex);
                    UpdateItemParent(item as LayoutComponent, false);
                }
                return new CollectionItemChangeInfo(item, itemIndex, -1);
            }
            return null;
        }

        protected CollectionItemChangeInfo RemoveItemAt(int index, bool runDry = false)
        {
            var item = InnerList[index];
            if (item != null)
            {
                if (!runDry)
                {
                    InnerList.RemoveAt(index);
                    UpdateItemParent(item as LayoutComponent, false);
                }

                return new CollectionItemChangeInfo(item, index, -1);
            }
            return null;
        }

        public void Remove(IEnumerable<T> elements)
        {
            var removedItems = new List<CollectionItemChangeInfo>();

            foreach (var item in elements)
            {
                var removedItem = RemoveItem(item, true);

                if (removedItem != null)
                    removedItems.Add(removedItem);

                InnerList.Remove(item);
            }

            if (removedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItems);

            foreach (var item in removedItems)
                UpdateItemParent(item.Item as LayoutComponent, false);
        }

        public void RemoveAll(Func<T, bool> predicate)
        {
            var removedItems = new List<CollectionItemChangeInfo>();

            foreach (var item in InnerList.ToArray())
            {
                if (predicate(item))
                {
                    removedItems.Add(RemoveItem(item, true));
                    InnerList.Remove(item);
                }
            }

            if (removedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItems);

            foreach (var item in removedItems)
                UpdateItemParent(item.Item as LayoutComponent, false);
        }

        public bool Remove(T item)
        {
            var removedItem = RemoveItem(item, true);

            if (removedItem != null)
            {
                InnerList.Remove((T)removedItem.Item);
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItem);
                UpdateItemParent(removedItem.Item as LayoutComponent, false);
            }

            return false;
        }

        public void RemoveAt(int index)
        {
            var removedItem = RemoveItemAt(index, true);

            if (removedItem != null)
            {
                InnerList.Remove((T)removedItem.Item);
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItem);
                UpdateItemParent(removedItem.Item as LayoutComponent, false);
            }
        }

        public void Clear()
        {
            var removedItems = new List<CollectionItemChangeInfo>();

            for (int i = 0; i < Count; i++)
                removedItems.Add(new CollectionItemChangeInfo(InnerList[i], i, -1));

            if (removedItems.Any())
                RaiseCollectionChanged(CollectionChangeAction.Remove, removedItems);

            InnerList.Clear();

            foreach (var item in removedItems)
                UpdateItemParent(item.Item as LayoutComponent, false);
        }

        #endregion

        public bool Contains(T item)
        {
            return InnerList.Contains(item);
        }

        public ReadOnlyCollection<T> AsReadOnly()
        {
            return InnerList.AsReadOnly();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((IList<T>)InnerList).CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IList<T>)InnerList).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<T>)InnerList).GetEnumerator();
        }

        int IList.Add(object value)
        {
            if (value is T item)
                Add(item);
            return -1;
        }

        bool IList.Contains(object value)
        {
            return ((IList)InnerList).Contains(value);
        }

        int IList.IndexOf(object value)
        {
            return ((IList)InnerList).IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            if (value is T item)
                Insert(index, item);
        }

        void IList.Remove(object value)
        {
            if (value is T item)
                Remove(item);
        }

        public void CopyTo(Array array, int index)
        {
            ((IList)InnerList).CopyTo(array, index);
        }
    }
}
