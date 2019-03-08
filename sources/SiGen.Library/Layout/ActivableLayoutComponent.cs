using SiGen.Layout.Editing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout
{
	public abstract class ActivableLayoutComponent : LayoutComponent
	{
		public abstract bool IsActive { get; }

		public ActivableLayoutComponent() { }

		internal ActivableLayoutComponent(InstrumentLayout layout) : base(layout)
		{
		}

		protected override void NotifyLayoutChanged(PropertyChange change)
		{
			if (IsActive)
				base.NotifyLayoutChanged(change);
		}
	}
}
