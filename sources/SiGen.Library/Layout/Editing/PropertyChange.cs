using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Editing
{
	public class PropertyChange : ILayoutChange
	{
		public LayoutComponent Component { get; }
		public string Property { get; }
		public int? Index { get; }
		public object OldValue { get; }
		public object NewValue { get; }
		public bool IsField { get; }
		public ComponentTypes AffectedComponents { get; }

		public PropertyChange(LayoutComponent component, ComponentTypes affectedComponents, string property, object oldValue, object newValue, bool isField = false, int? index = null)
		{
			Component = component;
			AffectedComponents = affectedComponents;
			Property = property;
			Index = index;
			OldValue = oldValue;
			NewValue = newValue;
			IsField = isField;
		}

		public PropertyChange[] GetChanges()
		{
			return new PropertyChange[] { this };
		}
	}
}
