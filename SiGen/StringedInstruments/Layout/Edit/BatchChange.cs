using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
	public class BatchChange : ILayoutChange
	{
		private List<PropertyChange> _ChangedProperties;

		public IList<PropertyChange> ChangedProperties => _ChangedProperties.AsReadOnly();

		public IEnumerable<LayoutComponent> ChangedComponents => _ChangedProperties.Select(p => p.Component).Distinct();

		public BatchChange(List<PropertyChange> changedProperties)
		{
			_ChangedProperties = changedProperties;
		}

		public PropertyChange[] GetChanges()
		{
			return _ChangedProperties.ToArray();
		}
	}
}
