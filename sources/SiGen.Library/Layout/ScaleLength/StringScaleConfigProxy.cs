using SiGen.Layout.Strings;
using SiGen.Measuring;

namespace SiGen.Layout.ScaleLength
{
	public class StringScaleConfigProxy : StringPartialConfig<ScaleLengthManager>, IStringScaleConfig
	{
		public Measure ScaleLength
		{
			get => Owner.GetScaleLength(StringIndex);
			set => Owner.SetScaleLength(StringIndex, value);
		}

		public bool AutoCalculateLength
		{
			get => Owner.GetAutoCalculateLength(StringIndex);
			set => Owner.SetAutoCalculateLength(StringIndex, value);
		}

		public double MultiScaleAlignment
		{
			get => Owner.GetMultiScaleAlignment(StringIndex);
			set => Owner.SetMultiScaleAlignment(StringIndex, value);
		}

		public StringLengthMode LengthMode
		{
			get => Owner.GetLengthMode(StringIndex);
			set => Owner.SetLengthMode(StringIndex, value);
		}

		public StringScaleConfigProxy(int stringIndex, ScaleLengthManager owner) : base(stringIndex, owner)
		{
		}
	}
}
