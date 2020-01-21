using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
	public class LayoutChangedEventArgs : EventArgs
	{
		public ILayoutChange Change { get; }

		//public PropertyChange[] ChangedProperties => Change.GetChanges();

		//public IEnumerable<LayoutComponent> ChangedComponents => ChangedProperties.Select(p => p.Component).Distinct();

		public LayoutChangedEventArgs(ILayoutChange change)
		{
			Change = change;
		}
	}
}
