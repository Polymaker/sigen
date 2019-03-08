using SiGen.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout
{
	public class LayoutComponentCollection : OwnerCollectionBase<InstrumentLayout, LayoutComponent>
	{
		public LayoutComponentCollection(InstrumentLayout owner) : base(owner)
		{
		}

		public LayoutComponentCollection(InstrumentLayout owner, IEnumerable<LayoutComponent> source) : base(owner, source)
		{
		}

		//protected override bool OnRemovingItem(LayoutComponent item)
		//{
		//	if (item.MandatoryComponent)
		//		return false;
		//	return base.OnRemovingItem(item);
		//}

		protected override void SetChildOwner(LayoutComponent child)
		{
			child.SetLayout(Owner);
		}

		protected override void UnsetChildOwner(LayoutComponent child)
		{
			child.SetLayout(null);
		}
	}
}
