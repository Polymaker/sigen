using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.ScaleLength
{
	public interface IStringScaleConfig
	{
		Measure ScaleLength { get; set; }
		bool AutoCalculateLength { get; set; }
		double MultiScaleAlignment { get; set; }
		StringLengthMode LengthMode { get; set; }
	}
}
