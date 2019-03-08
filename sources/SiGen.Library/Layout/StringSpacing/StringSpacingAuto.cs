using SiGen.Measuring;
using SiGen.StringedInstruments.Fingerboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Strings
{
	public class StringSpacingAuto : StringSpacingBase
	{
		public override StringSpacingMode SpacingMode => StringSpacingMode.Automatic;

		private Measure _StringSpacingAtNut;
		private Measure _StringSpacingAtBridge;

		private Measure[] AdjustedNutSpacings;
		private Measure[] AdjustedBridgeSpacings;

		private StringDistributionMode _NutDistributionMode;
		private StringDistributionMode _BridgeDistributionMode;

		public Measure StringSpacingAtNut
		{
			get => _StringSpacingAtNut;
			set => SetPropertyValue(ref _StringSpacingAtNut, value);
		}

		public Measure StringSpacingAtBridge
		{
			get => _StringSpacingAtBridge;
			set => SetPropertyValue(ref _StringSpacingAtBridge, value);
		}

		public StringDistributionMode NutDistributionMode
		{
			get => _NutDistributionMode;
			set => SetPropertyValue(ref _NutDistributionMode, value);
		}

		public StringDistributionMode BridgeDistributionMode
		{
			get => _BridgeDistributionMode;
			set => SetPropertyValue(ref _BridgeDistributionMode, value);
		}

		public override Measure GetSpacing(int index, FingerboardEnd side)
		{
			if (side == FingerboardEnd.Nut && NutDistributionMode == StringDistributionMode.BetweenStrings && AdjustedNutSpacings.Length > 0)
				return AdjustedNutSpacings[index];
			else if (side == FingerboardEnd.Bridge && BridgeDistributionMode == StringDistributionMode.BetweenStrings && AdjustedBridgeSpacings.Length > 0)
				return AdjustedBridgeSpacings[index];

			return side == FingerboardEnd.Nut ? StringSpacingAtNut : StringSpacingAtBridge;
		}
	}
}
