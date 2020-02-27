using SiGen.Maths;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    static class StringHelper
    {
        public static void EstimateStringAction(SIString @string)
        {
            if (@string.Gauge.IsEmpty)
                return;

            //var fret1Pos = @string.ScaleLength - (@string.ScaleLength / Math.Pow(2, 1d / 12d));
            //var fret2Pos = @string.ScaleLength - (@string.ScaleLength / Math.Pow(2, 2d / 12d));
            //var dist = fret2Pos.NormalizedValue - fret1Pos.NormalizedValue;
            //var targetClearance = Measure.Mm(0.1) + (@string.Gauge / 2d);
            //var slope = targetClearance / dist;
            //var offset = slope * fret2Pos.NormalizedValue;
            //offset.Unit = UnitOfMeasure.Mm;
            //offset -= (@string.Gauge / 2d);
            //@string.ActionAtFirstFret = offset;

            var fretRatio = @string.Gauge[UnitOfMeasure.In].DoubleValue * 10;
            @string.ActionAtFirstFret = Measure.Mm(0.3 + fretRatio);

            @string.ActionAtTwelfthFret = Measure.Mm(2.5);
        }

        public static void EstimateStringGauge(SIString @string)
        {
            Measure estimatedGauge = Measure.Empty;

            if (@string.Previous != null && !@string.Previous.Gauge.IsEmpty)
            {
                estimatedGauge = @string.Previous.Gauge * 1.3d;
                if (@string.Previous.Previous != null)
                {
                    var gauge1 = @string.Previous.Gauge;
                    var gauge2 = @string.Previous.Previous.Gauge;
                    var gaugeDiff = gauge1 - gauge2;
                    estimatedGauge = Measure.Avg(estimatedGauge, gauge1 + gaugeDiff);
                }
            }
            else if (@string.Next != null && !@string.Next.Gauge.IsEmpty)
            {
                estimatedGauge = @string.Next.Gauge * 0.77d;
                if (@string.Next.Next != null)
                {
                    var gauge1 = @string.Next.Gauge;
                    var gauge2 = @string.Next.Next.Gauge;
                    var gaugeDiff = gauge1 - gauge2;
                    estimatedGauge = Measure.Avg(estimatedGauge, gauge1 + gaugeDiff);
                }
            }

            if (estimatedGauge != null)
            {
                var gaugeIn = estimatedGauge[UnitOfMeasure.In].DoubleValue;

                var roundedGauge = Math.Truncate(gaugeIn * 1000d) / 1000d;
                var remainder = gaugeIn - roundedGauge;
                remainder = NumberHelper.Round(remainder * 10000d, 5d) / 10000d;
                roundedGauge += remainder;
                @string.Gauge = Measure.Inches(roundedGauge);
            }
        }
    }
}
