using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;
using SiGen.StringedInstruments.Fingerboard;

namespace SiGen.Layout.ScaleLength
{
	public class DualScaleManager : ScaleLengthManager
	{
		private Measure _Treble;
		private Measure _Bass;
		private double _PerpendicularAlignment;
		private StringLengthMode _LengthMode;

		public Measure Treble
		{
			get { return _Treble; }
			set
			{
				SetPropertyValue(ref _Treble, value);
			}
		}

		public Measure Bass
		{
			get { return _Bass; }
			set
			{
				SetPropertyValue(ref _Bass, value);
			}
		}

		public double PerpendicularAlignment
		{
			get => _PerpendicularAlignment;
			set => SetPropertyValue(ref _PerpendicularAlignment, value);
		}

		public StringLengthMode LengthMode
		{
			get => _LengthMode;
			set => SetPropertyValue(ref _LengthMode, value);
		}

		internal DualScaleManager(InstrumentLayout layout) : base(layout)
		{
			_Treble = Measure.Inches(25.5);
			_Bass = Measure.Inches(27);
			_PerpendicularAlignment = 0.5;
			_LengthMode = StringLengthMode.AlongString;
		}

		#region Get/Set Properties

		public override bool GetAutoCalculateLength(int stringIndex)
		{
			if (stringIndex == 0 || stringIndex == NumberOfStrings - 1)
				return false;
			return true;
		}

		public override StringLengthMode GetLengthMode(int stringIndex)
		{
			return LengthMode;
		}

		public override double GetMultiScaleAlignment(int stringIndex)
		{
			return PerpendicularAlignment;
		}

		public override Measure GetScaleLength(int stringIndex)
		{
			if (stringIndex == 0)
			{
				return Layout.FirstStringSide == FingerboardSide.Treble ? Treble : Bass;
			}
			else if (stringIndex == NumberOfStrings - 1)
			{
				return Layout.FirstStringSide == FingerboardSide.Bass ? Treble : Bass;
			}

			return Measure.Empty;
		}

		public override void SetAutoCalculateLength(int stringIndex, bool value)
		{
			throw new NotSupportedException();
		}

		public override void SetLengthMode(int stringIndex, StringLengthMode value)
		{
			LengthMode = value;
		}

		public override void SetMultiScaleAlignment(int stringIndex, double value)
		{
			PerpendicularAlignment = value;
		}

		public override void SetScaleLength(int stringIndex, Measure value)
		{
			if (stringIndex == 0)
			{
				if (Layout.FirstStringSide == FingerboardSide.Treble)
					Treble = value;
				else
					Bass = value;
			}
			else if (stringIndex == NumberOfStrings - 1)
			{
				if (Layout.FirstStringSide != FingerboardSide.Treble)
					Treble = value;
				else
					Bass = value;
			}
			else
			{
				throw new NotSupportedException();
			}
		}

		#endregion
	}
}
