using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
	public interface ILayoutChange
	{
        string Name { get; }
        LayoutComponent Component { get; }
        bool AffectsLayout { get; }

    }
}
