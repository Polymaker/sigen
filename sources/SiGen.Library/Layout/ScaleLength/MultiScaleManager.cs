using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;

namespace SiGen.Layout.ScaleLength
{
	public class MultiScaleManager : ScaleLengthManager
	{
		internal MultiScaleManager(InstrumentLayout layout) : base(layout)
		{
		}

		public override bool GetAutoCalculateLength(int stringIndex)
		{
			throw new NotImplementedException();
		}

		public override StringLengthMode GetLengthMode(int stringIndex)
		{
			throw new NotImplementedException();
		}

		public override double GetMultiScaleAlignment(int stringIndex)
		{
			throw new NotImplementedException();
		}

		public override Measure GetScaleLength(int stringIndex)
		{
			throw new NotImplementedException();
		}

		public override void SetAutoCalculateLength(int stringIndex, bool value)
		{
			throw new NotImplementedException();
		}

		public override void SetLengthMode(int stringIndex, StringLengthMode value)
		{
			throw new NotImplementedException();
		}

		public override void SetMultiScaleAlignment(int stringIndex, double value)
		{
			throw new NotImplementedException();
		}

		public override void SetScaleLength(int stringIndex, Measure value)
		{
			throw new NotImplementedException();
		}
	}
}
