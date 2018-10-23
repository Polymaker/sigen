using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class LayoutComponent : IDisposable, ILayoutComponent
    {
        private /*readonly*/ SILayout _Layout;
        private bool isDisposed;

        public SILayout Layout { get { return _Layout; } }

        public int NumberOfStrings { get { return Layout.NumberOfStrings; } }

        SILayout ILayoutComponent.Layout
        {
            get { return Layout; }
        }

        public LayoutComponent(SILayout layout)
        {
            _Layout = layout;
            _Layout._Components.Add(this);
        }

        ~LayoutComponent()
        {
            if (!isDisposed)
                Dispose();
        }

        public void Dispose()
        {
            isDisposed = true;
            _Layout._Components.Remove(this);
            _Layout = null;
        }

        void ILayoutComponent.OnStringConfigurationChanged()
        {
            OnStringConfigurationChanged();
        }

        protected virtual void OnStringConfigurationChanged()
        {

        }

        protected void NotifyLayoutChanged(string propertyName)
        {
            Layout?.NotifyLayoutChanged(this, propertyName);
        }

        protected void SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            var comparer = EqualityComparer<T>.Default;
            if (!comparer.Equals(property, value))
            {
                property = value;
                NotifyLayoutChanged(propertyName);
            }
        }
    }
}
