using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Strings
{
	public class StringPartialConfig<T> where T : LayoutComponent
	{
		public int StringIndex { get; }
		public T Owner { get; }


		public StringPartialConfig(int stringIndex, T owner)
		{
			StringIndex = stringIndex;
			Owner = owner;
		}
	}
}
