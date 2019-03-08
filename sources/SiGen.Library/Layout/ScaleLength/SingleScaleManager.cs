using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.ScaleLength
{
	public class SingleScaleManager : ScaleLengthManager
	{
		private Measure _Length;

		public Measure Length
		{
			get => _Length;
			set => SetPropertyValue(ref _Length, value);
		}

		internal SingleScaleManager(InstrumentLayout layout) : base(layout)
		{
			_Length = Measure.Inches(25.5);
		}

		#region Get/Set Properties

		public override void SetScaleLength(int index, Measure value)
		{
			Length = value;
		}

		public override Measure GetScaleLength(int stringIndex)
		{
			return Length;
		}

		public override bool GetAutoCalculateLength(int stringIndex)
		{
			return false;
		}

		public override StringLengthMode GetLengthMode(int stringIndex)
		{
			return StringLengthMode.AlongFingerboard;
		}

		public override double GetMultiScaleAlignment(int stringIndex)
		{
			return 1;
		}

		public override void SetAutoCalculateLength(int index, bool value)
		{
			throw new NotSupportedException();
		}

		public override void SetMultiScaleAlignment(int index, double value)
		{
			throw new NotSupportedException();
		}

		public override void SetLengthMode(int index, StringLengthMode value)
		{
			throw new NotSupportedException();
		}

		#endregion

	}
}
