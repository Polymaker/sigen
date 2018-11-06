using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
	public class FretPositionCollection : IList<FretPosition>
	{
		private List<FretPosition> Items;
		private FretManager Manager;

		internal FretPositionCollection(FretManager manager)
		{
			Manager = manager;
			Items = new List<FretPosition>();
		}

		public FretPosition this[int index] { get => ((IList<FretPosition>)Items)[index]; set => ((IList<FretPosition>)Items)[index] = value; }

		public int Count => ((IList<FretPosition>)Items).Count;

		public bool IsReadOnly => ((IList<FretPosition>)Items).IsReadOnly;

		public void Add(FretPosition item)
		{
			if (item.Layout == null)
				item.SetLayout(Manager.Layout);
			((IList<FretPosition>)Items).Add(item);
		}

		public void Clear()
		{
			((IList<FretPosition>)Items).Clear();
		}

		public bool Contains(FretPosition item)
		{
			return ((IList<FretPosition>)Items).Contains(item);
		}

		public void CopyTo(FretPosition[] array, int arrayIndex)
		{
			((IList<FretPosition>)Items).CopyTo(array, arrayIndex);
		}

		public IEnumerator<FretPosition> GetEnumerator()
		{
			return ((IList<FretPosition>)Items).GetEnumerator();
		}

		public int IndexOf(FretPosition item)
		{
			return ((IList<FretPosition>)Items).IndexOf(item);
		}

		public void Insert(int index, FretPosition item)
		{
			((IList<FretPosition>)Items).Insert(index, item);
		}

		public bool Remove(FretPosition item)
		{
			var result = Items.Remove(item);
			if (result)
				item.Dispose();
			return result;
		}

		public void RemoveAt(int index)
		{
			((IList<FretPosition>)Items).RemoveAt(index);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IList<FretPosition>)Items).GetEnumerator();
		}
	}
}
