using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SiGen.Layout
{
	[Flags]
	public enum ComponentTypes
	{
		General,
		Strings,
		Frets,
		Fingerboard = 4,
		Visuals = 8
	}
}
