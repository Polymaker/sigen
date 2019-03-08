using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Editing
{
	public class BatchChange : ILayoutChange
	{
		private List<PropertyChange> _ChangedProperties;

		public IList<PropertyChange> ChangedProperties => _ChangedProperties.AsReadOnly();

		public IEnumerable<LayoutComponent> ChangedComponents => _ChangedProperties.Select(p => p.Component).Distinct();

		public ComponentTypes AffectedComponents { get; }

		public BatchChange(List<PropertyChange> changedProperties)
		{
			_ChangedProperties = changedProperties;
			foreach (var prop in changedProperties)
				AffectedComponents |= prop.AffectedComponents;
		}

		public PropertyChange[] GetChanges()
		{
			return _ChangedProperties.ToArray();
		}
	}
}
