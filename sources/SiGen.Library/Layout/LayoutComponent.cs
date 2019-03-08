using SiGen.Layout.Editing;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout
{
	public abstract class LayoutComponent
	{
		public InstrumentLayout Layout { get; private set; }

		public int NumberOfStrings => Layout?.NumberOfStrings ?? 1;

		public abstract ComponentTypes Type { get; }

		public bool MandatoryComponent { get; }

		public LayoutComponent() { }

		internal LayoutComponent(InstrumentLayout layout)
		{
			MandatoryComponent = true;
			layout.Components.Add(this);
		}

		internal void SetLayout(InstrumentLayout layout)
		{
			Layout = layout;
		}

		#region Properties & Fields value management

		protected virtual void NotifyLayoutChanged(PropertyChange change)
		{
			Layout?.NotifyLayoutChanged(change);
		}

		protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
		{
			return SetPropertyValue(ref property, value, Type, propertyName);
		}

		protected bool SetPropertyValue<T>(ref T property, T value, ComponentTypes changeType, [CallerMemberName] string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				var propChange = new PropertyChange(component: this, affectedComponents: changeType, property: propertyName, oldValue: property, newValue: value);
				property = value;
				NotifyLayoutChanged(propChange);
				return true;
			}
			return false;
		}

		protected bool SetFieldValue<T>(ref T field, T value, string fieldName)
		{
			return SetFieldValue(ref field, value, fieldName, Type);
		}

		protected bool SetFieldValue<T>(ref T field, T value, string fieldName, ComponentTypes changeType)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				var propChange = new PropertyChange(component: this, affectedComponents: changeType, property: fieldName, oldValue: field, newValue: value, isField: true);
				field = value;
				NotifyLayoutChanged(propChange);
				return true;
			}
			return false;
		}

		protected bool SetFieldValue<T>(ref T[] field, int index, T value, string fieldName, ComponentTypes changeType = ComponentTypes.General)
		{
			if (!EqualityComparer<T>.Default.Equals(field[index], value))
			{
				var propChange = new PropertyChange(component: this, affectedComponents: changeType, property: fieldName, index: index, oldValue: field[index], newValue: value, isField: true);
				field[index] = value;
				NotifyLayoutChanged(propChange);
				return true;
			}
			return false;
		}

		protected void StartBatchChanges()
		{
			Layout?.StartBatchChanges();
		}

		protected void FinishBatchChanges()
		{
			Layout?.FinishBatchChanges();
		}

		protected void SetMultipleValues(Action action)
		{
			try
			{
				StartBatchChanges();
				action();
			}
			finally
			{
				FinishBatchChanges();
			}
		}

		#endregion

		#region Undo/Redo

		//protected virtual void UndoLayoutChange(PropertyChange change)
		//{
		//	if (change.IsField)
		//	{
		//		FieldInfo fi = GetValueField(change.Property);

		//		//if (change.Index.HasValue)
		//		//{
		//		//	var arrayValue = (IList)fi.GetValue((object)change.Component ?? this);
		//		//	arrayValue[change.Index.Value] = setNewValue ? changedProp.NewValue : changedProp.OldValue;
		//		//}
		//		//else
		//		//	fi.SetValue((object)change.Component ?? this, setNewValue ? changedProp.NewValue : changedProp.OldValue);
		//	}
		//	else
		//	{
		//		PropertyInfo pi = GetValueProperty(change.Property);

		//	}
		//}

		//protected virtual FieldInfo GetValueField(string fieldName)
		//{
		//	return GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		//}

		//protected virtual PropertyInfo GetValueProperty(string propertyName)
		//{
		//	return GetType().GetProperty(propertyName);
		//}

		#endregion
	}
}
