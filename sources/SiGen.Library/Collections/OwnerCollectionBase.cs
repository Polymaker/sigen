using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Collections
{
	public abstract class OwnerCollectionBase<O, C> : IList<C>, ICollection<C>, IEnumerable<C>, IEnumerable, IList, ICollection
	{
		private O _Owner;
		private ObservableCollection<C> innerList;
		private bool SuppressCollectionChanged;
		private bool DelayedCollectionChange;
		private NotifyCollectionChangedEventArgs LastChange;

		public O Owner
		{
			get { return _Owner; }
		}

		public C this[int index]
		{
			get { return innerList[index]; }
			set
			{
				innerList[index] = value;
			}
		}

		public int Count
		{
			get { return innerList.Count; }
		}

		public bool IsReadOnly
		{
			get { return false; }
		}

		public object SyncRoot
		{
			get
			{
				return ((ICollection)innerList).SyncRoot;
			}
		}

		public bool IsSynchronized
		{
			get
			{
				return ((ICollection)innerList).IsSynchronized;
			}
		}

		public bool IsFixedSize
		{
			get
			{
				return ((IList)innerList).IsFixedSize;
			}
		}

		object IList.this[int index]
		{
			get
			{
				return ((IList)innerList)[index];
			}

			set
			{
				((IList)innerList)[index] = value;
			}
		}

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		public OwnerCollectionBase(O owner)
		{
			_Owner = owner;
			LastChange = null;
			innerList = new ObservableCollection<C>();
			innerList.CollectionChanged += InnerCollectionChanged;
		}

		public OwnerCollectionBase(O owner, IEnumerable<C> source)
		{
			_Owner = owner;
			LastChange = null;
			innerList = new ObservableCollection<C>(source);
			foreach (var item in innerList)
				SetChildOwner(item);
			innerList.CollectionChanged += InnerCollectionChanged;
		}

		#region CollectionChanged

		private void InnerCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (SuppressCollectionChanged)
				return;
			if (DelayedCollectionChange)
				LastChange = e;
			else
			{
				OnCollectionChanged(e);
				LastChange = null;
			}
		}

		private void PushLastCollectionChange()
		{
			if (LastChange != null)
			{
				OnCollectionChanged(LastChange);
				LastChange = null;
			}
			DelayedCollectionChange = false;
		}

		protected void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
		{
			CollectionChanged?.Invoke(this, e);
		}

		#endregion

		protected abstract void SetChildOwner(C child);

		protected abstract void UnsetChildOwner(C child);

		protected virtual bool OnAddingItem(C item)
		{
			return true;
		}

		protected virtual bool OnRemovingItem(C item)
		{
			return true;
		}

		public int IndexOf(C element)
		{
			return innerList.IndexOf(element);
		}

		public void Add(C item)
		{
			if (OnAddingItem(item))
			{
				SetChildOwner(item);
				innerList.Add(item);
			}
		}

		public void Insert(int index, C item)
		{
			if (OnAddingItem(item))
			{
				DelayedCollectionChange = true;
				innerList.Insert(index, item);
				SetChildOwner(item);
				PushLastCollectionChange();
			}
		}

		public void Clear()
		{
			foreach (var elem in innerList)
				SetChildOwner(elem);
			innerList.Clear();
		}

		public bool Contains(C item)
		{
			return innerList.Contains(item);
		}

		public void CopyTo(C[] array, int arrayIndex)
		{
			innerList.CopyTo(array, arrayIndex);
		}

		public bool Remove(C item)
		{
			if (innerList.Contains(item) && OnRemovingItem(item))
			{
				DelayedCollectionChange = true;
				innerList.Remove(item);
				UnsetChildOwner(item);
				PushLastCollectionChange();
				return true;
			}

			return false;
		}

		public void RemoveAt(int index)
		{
			if (index >= 0 && index < Count)
			{
				var item = this[index];
				if (OnRemovingItem(item))
					UnsetChildOwner(item);
			}
			innerList.RemoveAt(index);
		}

		public int RemoveAll(Predicate<C> match)
		{
			if (match == null)
				throw new ArgumentNullException("match");

			if (innerList.Count == 0)
				return 0;

			SuppressCollectionChanged = true;

			var removedItems = new List<C>();

			for (int i = 0; i < innerList.Count; i++)
			{
				var currentItem = innerList[i];
				if (match(currentItem))
				{
					if (OnRemovingItem(currentItem))
					{
						removedItems.Add(currentItem);
						RemoveAt(i--);
					}
				}
			}

			SuppressCollectionChanged = false;
			InnerCollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems));

			return removedItems.Count;
		}

		public IEnumerator<C> GetEnumerator()
		{
			return innerList.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#region IList/ICollection

		public void CopyTo(Array array, int index)
		{
			((ICollection)innerList).CopyTo(array, index);
		}

		public int Add(object value)
		{
			Add((C)value);
			return Count - 1;
		}

		public bool Contains(object value)
		{
			return ((IList)innerList).Contains(value);
		}

		public int IndexOf(object value)
		{
			return ((IList)innerList).IndexOf(value);
		}

		public void Insert(int index, object value)
		{
			Insert(index, (C)value);
		}

		public void Remove(object value)
		{
			Remove((C)value);
		}

		#endregion

		public ReadOnlyCollection<C> AsReadOnly()
		{
			return new ReadOnlyCollection<C>(this);
		}
	}
}
