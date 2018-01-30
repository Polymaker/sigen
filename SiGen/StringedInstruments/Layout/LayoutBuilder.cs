using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class LayoutBuilder
    {

        public static void BuildLayout(SILayout layout)
        {
            layout.VisualElements.Clear();

            Measure nutCenter = Measure.Zero;
            Measure bridgeCenter = Measure.Zero;
            Measure[] nutStringPos = layout.StringSpacing.GetStringPositions(true, out nutCenter);
            Measure[] bridgeStringPos = layout.StringSpacing.GetStringPositions(false, out bridgeCenter);

            //***** STRINGS *****//
            var stringLines = new List<StringLine>();

            for (int i = 0; i < layout.NumberOfStrings; i++)
                stringLines.Add(ConstructString(layout.Strings[i], nutStringPos[i], bridgeStringPos[i]));

            Measure maxPerpHeight = Measure.Zero;
            if (layout.NumberOfStrings > 1)
                maxPerpHeight = Measure.FromNormalizedValue(stringLines.Max(l => l.Bounds.Height.NormalizedValue), UnitOfMeasure.Mm);

            foreach (var stringLine in stringLines)
                AdjustString(maxPerpHeight, stringLine);

            layout.VisualElements.AddRange(stringLines);

            

            //***** Fretboard Edges *****//
            

            //***** Frets *****//

        }

        #region Strings

        public static StringLine ConstructString(SIString str, Measure nutPos, Measure bridgePos)
        {
            var opp = Measure.Abs(nutPos - bridgePos);
            Measure adj = str.ScaleLength;

            if (str.LengthCalculationMethod == LengthFunction.AlongString && opp > Measure.Zero)
            {
                var theta = Math.Asin(opp.NormalizedValue / str.ScaleLength.NormalizedValue);
                adj = Math.Cos(theta) * str.ScaleLength;
            }

            var p1 = new PointM(nutPos, (adj / 2d));
            var p2 = new PointM(bridgePos, (adj / 2d) * -1d);

            return new StringLine(str, p1, p2);
        }

        private static void SetStringRealLength(SIString stringInfo, StringLine stringLine)
        {
            if (stringInfo.Layout.CompensateFretPositions || stringInfo.LengthCalculationMethod == LengthFunction.AlongString)
                stringInfo.FinalLength = stringLine.Length;
            else
                stringInfo.FinalLength = stringLine.Bounds.Height;
        }

        private static void AdjustString(Measure maxPerpHeight , StringLine stringLine)
        {
            var stringInfo = stringLine.String;
            var offsetY = (maxPerpHeight - stringLine.Bounds.Height) * (0.5 - stringInfo.RelativeScaleLengthOffset);
            stringLine.P1 += new PointM(Measure.Zero, offsetY);
            stringLine.FretZero = stringLine.P1;//keep pos of fret 0 usefull because starting fret can be negative
            stringLine.P2 += new PointM(Measure.Zero, offsetY);
            SetStringRealLength(stringInfo, stringLine);

            if (stringInfo.StartingFret != 0)
            {
                var startingFretPosRatio = GetEqualTemperedFretPosition(stringInfo.StartingFret);
                var lineDir = stringInfo.PlaceFretsRelativeToString ? stringLine.Direction * -1 : new Vector(0, 1);
                var fretPos = stringLine.P2 + (lineDir * stringInfo.FinalLength * startingFretPosRatio);

                if (!stringInfo.PlaceFretsRelativeToString)
                    fretPos = stringLine.SnapToLine(fretPos, true);

                stringLine.P1 = fretPos;
                SetStringRealLength(stringInfo, stringLine);
            }
        }

        #endregion
        /// <summary>
        /// Returns the fret position as the distance from the bridge.
        /// </summary>
        /// <param name="fret"></param>
        /// <returns></returns>
        public static double GetEqualTemperedFretPosition(int fret)
        {
            return 1d / Math.Pow(2, fret / 12d);
        }

        #region Update Scheduler



        #endregion
    }
}
