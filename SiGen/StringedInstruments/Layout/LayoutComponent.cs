using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class LayoutComponent : IDisposable, ILayoutComponent
    {
		private bool isDisposed;

		public SILayout Layout { get; private set; }

		public int NumberOfStrings { get { return Layout.NumberOfStrings; } }

		SILayout ILayoutComponent.Layout
        {
            get { return Layout; }
        }

        public LayoutComponent(SILayout layout)
        {
			if (!isDisposed)
			{
				Layout = layout;
				if (layout != null)
					Layout._Components.Add(this);
			}
        }

		internal void SetLayout(SILayout layout)
		{
            var oldLayout = Layout;
			Layout = layout;

            if (oldLayout != null && oldLayout._Components.Contains(this))
                oldLayout._Components.Add(this);

            if (layout != null && !layout._Components.Contains(this))
                layout._Components.Add(this);
        }

		~LayoutComponent()
        {
            if (!isDisposed)
                Dispose();
        }

        public void Dispose()
        {
            isDisposed = true;
			if(Layout != null)
			{
				Layout._Components.Remove(this);
				Layout = null;
			}
        }

        void ILayoutComponent.OnStringConfigurationChanged()
        {
            OnStringConfigurationChanged();
        }

        protected virtual void OnStringConfigurationChanged()
        {

        }

		protected virtual void NotifyLayoutChanged(PropertyChange change)
		{
			Layout?.NotifyLayoutChanged(change);
		}

		protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
				var propChange = new PropertyChange(this, propertyName, property, value);
				property = value;
				NotifyLayoutChanged(propChange);
				return true;
            }
			return false;
        }

        protected bool SetPropertyValue<T>(ref T property, T value, bool invalidateLayout, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(property, value))
            {
                var propChange = new PropertyChange(this, propertyName, property, value)
                {
                    AffectsLayout = invalidateLayout
                };
                property = value;
                NotifyLayoutChanged(propChange);
                return true;
            }
            return false;
        }

        protected bool SetFieldValue<T>(ref T field, T value, string fieldName)
		{
			if (!EqualityComparer<T>.Default.Equals(field, value))
			{
				var propChange = new PropertyChange(this, fieldName, field, value, true);
				field = value;
				NotifyLayoutChanged(propChange);
				return true;
			}
			return false;
		}

		protected bool SetFieldValue<T>(ref T[] field, int index, T value, string fieldName)
		{
			if (!EqualityComparer<T>.Default.Equals(field[index], value))
			{
				var propChange = new PropertyChange(this, fieldName, index, field[index], value, true);
				field[index] = value;
				NotifyLayoutChanged(propChange);
				return true;
			}
			return false;
		}

		protected void StartBatchChanges(string name = null)
		{
			Layout?.StartBatchChanges(name);
		}

		protected void FinishBatchChanges()
		{
			Layout?.FinishBatchChanges();
		}

		//protected IDisposable OpenBatchTrans(string name = null)
		//{
		//	return new TemporaryObject(
		//		() => StartBatchChanges(name), 
		//		() => FinishBatchChanges());
		//}
	}

	public abstract class ActivableLayoutComponent : LayoutComponent
	{
		public abstract bool IsActive { get; }

		public ActivableLayoutComponent(SILayout layout) : base(layout)
		{

		}

		protected override void NotifyLayoutChanged(PropertyChange change)
		{
			if (IsActive)
				base.NotifyLayoutChanged(change);
		}
	}
}
