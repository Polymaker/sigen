using SiGen.Measuring;
using SiGen.StringedInstruments.Fingerboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Strings
{
	public abstract class StringSpacingBase : ActivableLayoutComponent
	{
		public abstract StringSpacingMode SpacingMode { get; }

		public override ComponentTypes Type => ComponentTypes.Strings | ComponentTypes.Fingerboard;

		public override bool IsActive => throw new NotImplementedException();

		public Measure GetSpacingBetweenStrings(int index1, int index2, FingerboardEnd side)
		{
			if (index1 == index2)
				return Measure.Zero;

			Measure total = Measure.Zero;
			for (int i = Math.Min(index1, index2); i < Math.Max(index1, index2); i++)
				total += GetSpacing(i, side);
			return total;
		}

		public abstract Measure GetSpacing(int index, FingerboardEnd side);
	}
}
