using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
	public class BatchChange : ILayoutChange
	{
        private List<ILayoutChange> _LayoutChanges;

        public string Name { get; }

        public IList<ILayoutChange> LayoutChanges => _LayoutChanges.AsReadOnly();

        public IEnumerable<PropertyChange> ChangedProperties => LayoutChanges.OfType<PropertyChange>();

		public IEnumerable<LayoutComponent> ChangedComponents => LayoutChanges.Select(p => p.Component).Distinct();

        public bool AffectsLayout => ChangedProperties.Any(x => x.AffectsLayout);

        public LayoutComponent Component { get; }

        public BatchChange(IEnumerable<ILayoutChange> layoutChanges)
        {
            Name = string.Empty;
            _LayoutChanges = layoutChanges.ToList();
            Component = ChangedComponents.FirstOrDefault();
        }


        public BatchChange(string name, IEnumerable<ILayoutChange> layoutChanges)
        {
            Name = name;
            _LayoutChanges = layoutChanges.ToList();
            Component = ChangedComponents.FirstOrDefault();
        }
	}
}
