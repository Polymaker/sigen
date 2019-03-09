using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class PropertyChange : ILayoutChange
	{
		public LayoutComponent Component { get; }
		public string Property { get; }
		public int? Index { get; }
		public object OldValue { get; }
		public object NewValue { get; }
		public bool IsField { get; }

        public string Name => Property;

        public PropertyChange(LayoutComponent component, string property, object oldValue, object newValue)
		{
			Component = component;
			Property = property;
			OldValue = oldValue;
			NewValue = newValue;
		}

		public PropertyChange(LayoutComponent component, string property, int index, object oldValue, object newValue)
		{
			Component = component;
			Property = property;
			OldValue = oldValue;
			NewValue = newValue;
			Index = index;
		}

		public PropertyChange(LayoutComponent component, string property, object oldValue, object newValue, bool isField)
		{
			Component = component;
			Property = property;
			OldValue = oldValue;
			NewValue = newValue;
			IsField = isField;
		}

		public PropertyChange(LayoutComponent component, string property, int index, object oldValue, object newValue, bool isField)
		{
			Component = component;
			Property = property;
			OldValue = oldValue;
			NewValue = newValue;
			IsField = isField;
			Index = index;
		}

		public PropertyChange(LayoutComponent component, string property, int? index, object oldValue, object newValue, bool isField)
		{
			Component = component;
			Property = property;
			OldValue = oldValue;
			NewValue = newValue;
			IsField = isField;
			Index = index;
		}

		public PropertyChange[] GetChanges()
		{
			return new PropertyChange[] { this };
		}
	}
}
