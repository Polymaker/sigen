using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.ScaleLength
{
	public abstract partial class ScaleLengthManager : ActivableLayoutComponent
	{

		protected IStringScaleConfig[] StringConfigs;


		public override ComponentTypes Type => ComponentTypes.Strings/* | ComponentTypes.Fingerboard*/;

		public override bool IsActive => throw new NotImplementedException();


		internal ScaleLengthManager(InstrumentLayout layout) : base(layout)
		{
		}

		#region Get/Set Properties

		public abstract void SetScaleLength(int stringIndex, Measure value);

		public abstract void SetAutoCalculateLength(int stringIndex, bool value);

		public abstract void SetMultiScaleAlignment(int stringIndex, double value);

		public abstract void SetLengthMode(int stringIndex, StringLengthMode value);

		public abstract bool GetAutoCalculateLength(int stringIndex);

		public abstract Measure GetScaleLength(int stringIndex);

		public abstract double GetMultiScaleAlignment(int stringIndex);

		public abstract StringLengthMode GetLengthMode(int stringIndex);

		#endregion

		protected virtual IStringScaleConfig InitializeNewString(int stringIndex)
		{
			return new StringScaleConfigProxy(stringIndex, this);
		}

		public IStringScaleConfig GetStringConfig(int stringIndex)
		{
			return StringConfigs[stringIndex];
		}
	}
}
