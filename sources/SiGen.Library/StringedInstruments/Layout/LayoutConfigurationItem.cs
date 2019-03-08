using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout
{
    public class LayoutConfigurationItem
    {
        private InstrumentLayout _Layout;

        public InstrumentLayout Layout { get { return _Layout; } }

        public int NumberOfStrings { get { return Layout != null ? Layout.NumberOfStrings : -1; } }

        protected virtual void OnStringNumberChanged() { }

        protected void NotifyLayoutChanged(string propertyName, LayoutProcessElement affectedElement)
        {
            if(Layout != null)
            {

            }
        }
    }
}
