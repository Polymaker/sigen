using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Measuring
{
    public class MeasureFormat
    {
        public UnitOfMeasure OverrideUnit { get; set; }
        public int MinimumDecimals { get; set; }
        public int MaximumDecimals { get; set; }

        public bool ShowFractions { get; set; }
        public bool AllowApproximation { get; set; }
        public bool ShowUnitOfMeasure { get; set; }

        public static MeasureFormat DefaultFormat
        {
            get { return new MeasureFormat(); }
        }

        public MeasureFormat()
        {
            MinimumDecimals = 0;
            MaximumDecimals = 3;
            ShowUnitOfMeasure = true;
            ShowFractions = true;
            AllowApproximation = true;
            OverrideUnit = null;
        }

        internal MeasureFormat Clone()
        {
            return new MeasureFormat()
            {
                AllowApproximation = AllowApproximation,
                MaximumDecimals = MaximumDecimals,
                MinimumDecimals = MinimumDecimals,
                ShowFractions = ShowFractions,
                ShowUnitOfMeasure = ShowUnitOfMeasure
            };
        }
    }
}
