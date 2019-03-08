using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Editing
{
	public interface ILayoutChange
	{
		ComponentTypes AffectedComponents { get; }
		PropertyChange[] GetChanges();
	}
}
