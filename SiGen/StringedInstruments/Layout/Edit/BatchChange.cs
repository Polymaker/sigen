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

        public string Name { get; }

		public IList<PropertyChange> ChangedProperties => _ChangedProperties.AsReadOnly();

		public IEnumerable<LayoutComponent> ChangedComponents => _ChangedProperties.Select(p => p.Component).Distinct();

        public LayoutComponent Component { get; }

        public BatchChange(List<PropertyChange> changedProperties)
		{
            Name = string.Empty;
            _ChangedProperties = changedProperties;
            Component = ChangedComponents.Count() == 1 ? ChangedComponents.First() : null;
        }

        public BatchChange(string name, List<PropertyChange> ChangedProperties)
        {
            Name = name;
            _ChangedProperties = ChangedProperties;
            Component = ChangedComponents.Count() == 1 ? ChangedComponents.First() : null;
        }

        public PropertyChange[] GetChanges()
		{
			return _ChangedProperties.ToArray();
		}
	}
}
