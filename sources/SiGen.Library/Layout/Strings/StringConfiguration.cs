using SiGen.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Strings
{
    public class StringConfiguration : LayoutComponent
    {
		public int Index { get; }

		public override ComponentTypes Type => ComponentTypes.Strings;

		public StringConfiguration(int index)
		{
			Index = index;
		}


	}
}
