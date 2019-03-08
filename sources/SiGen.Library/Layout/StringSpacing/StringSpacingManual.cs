using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;
using SiGen.StringedInstruments.Fingerboard;

namespace SiGen.Layout.Strings
{
	public class StringSpacingManual : StringSpacingBase
	{
		public override StringSpacingMode SpacingMode => StringSpacingMode.Manual;

		public override Measure GetSpacing(int index, FingerboardEnd side)
		{
			throw new NotImplementedException();
		}
	}
}
