using SiGen.Maths;
using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout.Visual;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public partial class SILayout
    {
        public void RebuildLayout()
        {
            VisualElements.Clear();

            Measure nutCenter = Measure.Zero;
            Measure bridgeCenter = Measure.Zero;
            Measure[] nutStringPos = StringSpacing.GetStringPositions(true, out nutCenter);
            Measure[] bridgeStringPos = StringSpacing.GetStringPositions(false, out bridgeCenter);

            LayoutStrings(nutStringPos, bridgeStringPos);
            CreateFingerboardEdges();
            PlaceFrets();

            isLayoutDirty = false;
        }

        private T AddVisualElement<T>(T elem) where T : VisualElement
        {
            VisualElements.Add(elem);
            return elem;
        }

        #region Strings

        public StringLine ConstructString(SIString str, Measure nutPos, Measure bridgePos)
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

            return AddVisualElement(new StringLine(str, p1, p2));
        }

        private void LayoutStrings(Measure[] nutStringPos, Measure[] bridgeStringPos)
        {
            var stringLines = VisualElements.OfType<StringLine>();

            if (NumberOfStrings == 1)
            {
                ConstructString(Strings[0], nutStringPos[0], bridgeStringPos[0]);
            }
            else if (ScaleLengthMode != ScaleLengthType.Individual)
            {
                var trebleStr = ConstructString(Strings[0], nutStringPos[0], bridgeStringPos[0]);
                var bassStr = ConstructString(Strings[NumberOfStrings - 1], nutStringPos[NumberOfStrings - 1], bridgeStringPos[NumberOfStrings - 1]);

                var maxHeight = Measure.Max(trebleStr.Bounds.Height, bassStr.Bounds.Height);
                AdjustStringVerticalPosition(trebleStr, maxHeight);
                AdjustStringVerticalPosition(bassStr, maxHeight);

                var nutLine = Line.FromPoints(trebleStr.P1.ToVector(), bassStr.P1.ToVector());
                var bridgeLine = Line.FromPoints(trebleStr.P2.ToVector(), bassStr.P2.ToVector());

                for (int i = 1; i < NumberOfStrings - 1; i++)
                {
                    var nutPos = nutLine.GetPointForX(nutStringPos[i].NormalizedValue);
                    var bridgePos = bridgeLine.GetPointForX(bridgeStringPos[i].NormalizedValue);
                    AddVisualElement(new StringLine(Strings[i],
                        PointM.FromVector(nutPos, nutStringPos[i].Unit),
                        PointM.FromVector(bridgePos, bridgeStringPos[i].Unit)));
                    Strings[i].UpdateFinalLength();
                }
            }
            else
            {
                for (int i = 0; i < NumberOfStrings; i++)
                    ConstructString(Strings[i], nutStringPos[i], bridgeStringPos[i]);
                //*** Adjust strings position for multiscale
                var maxPerpHeight = Measure.FromNormalizedValue(stringLines.Max(l => l.Bounds.Height.NormalizedValue), UnitOfMeasure.Mm);
                foreach (var strLine in stringLines)
                    AdjustStringVerticalPosition(strLine, maxPerpHeight);
            }

            //calculate starting fret position if different from 0 (nut)
            foreach(var str in Strings)
            {
                if (str.StartingFret != 0)
                {
                    var startingFretPosRatio = GetEqualTemperedFretPosition(str.StartingFret);
                    var stringVector = str.PlaceFretsRelativeToString ? str.LayoutLine.Direction * -1 : new Vector(0, 1);
                    var startingFretPos = str.LayoutLine.P2 + (stringVector * str.FinalLength * startingFretPosRatio);

                    if (!str.PlaceFretsRelativeToString)
                        startingFretPos = str.LayoutLine.SnapToLine(startingFretPos, true);

                    str.LayoutLine.P1 = startingFretPos;
                    str.UpdateFinalLength();
                }
            }

            for(int i = 0; i < NumberOfStrings - 1; i++)
                AddVisualElement(new StringCenter(Strings[i + 1].LayoutLine, Strings[i].LayoutLine));
        }

        /// <summary>
        /// Adjust the string's position relative to the longest string.
        /// </summary>
        /// <param name="stringLine"></param>
        /// <param name="maxPerpHeight"></param>
        private void AdjustStringVerticalPosition(StringLine stringLine, Measure maxPerpHeight)
        {
            var offsetY = (maxPerpHeight - stringLine.Bounds.Height) * (0.5 - stringLine.String.RelativeScaleLengthOffset);
            stringLine.P1 += new PointM(Measure.Zero, offsetY);
            stringLine.P2 += new PointM(Measure.Zero, offsetY);
            stringLine.FretZero = stringLine.P1;//keep pos of fret 0 usefull because starting fret can be negative
            stringLine.String.UpdateFinalLength();
        }
        
        private LayoutLine GetStringBoundaryLine(SIString str, FingerboardSide dir)
        {
            LayoutLine boundary = null;
            if (dir == FingerboardSide.Bass)
                boundary = VisualElements.OfType<StringCenter>().FirstOrDefault(c => c.Right.Index == str.Index);
            else
                boundary = VisualElements.OfType<StringCenter>().FirstOrDefault(c => c.Left.Index == str.Index);

            if (boundary == null)
                return VisualElements.OfType<FingerboardEdge>().First(e => e.Side == dir);

            return boundary;
        }

        /// <summary>
        /// Treble side string
        /// </summary>
        private SIString FirstString { get { return Strings[0]; } }
        /// <summary>
        /// Bass side string
        /// </summary>
        private SIString LastString { get { return Strings[NumberOfStrings - 1]; } }

        #endregion

        #region Fingerboard

        private void CreateFingerboardEdges()
        {
            var trebleLine = FirstString.LayoutLine;
            var trebleFretboardEdge = AddVisualElement(new FingerboardEdge(
               trebleLine.GetPerpendicularPoint(trebleLine.P1, Margins.Treble),
               trebleLine.GetPerpendicularPoint(trebleLine.P2, Margins.Treble),
               FingerboardSide.Treble));
            var bassLine = LastString.LayoutLine;
            var bassFretboardEdge = AddVisualElement(new FingerboardEdge(
               bassLine.GetPerpendicularPoint(bassLine.P1, Margins.Bass * -1), //*-1 to offset towards left
               bassLine.GetPerpendicularPoint(bassLine.P2, Margins.Bass * -1),
               FingerboardSide.Bass));

            if(ScaleLengthMode != ScaleLengthType.Individual && NumberOfStrings > 1)
            {
                var nutLine = new LayoutLine(trebleLine.FretZero, bassLine.FretZero);
                var bridgeLine = new LayoutLine(trebleLine.P2, bassLine.P2);

                if (FirstString.StartingFret == 0)
                    trebleFretboardEdge.P1 = nutLine.GetIntersection(trebleFretboardEdge);
                else
                    trebleFretboardEdge.P1 = trebleFretboardEdge.SnapToLine(trebleLine.P1, true);

                if (LastString.StartingFret == 0)
                    bassFretboardEdge.P1 = nutLine.GetIntersection(bassFretboardEdge);
                else
                    bassFretboardEdge.P1 = bassFretboardEdge.SnapToLine(bassLine.P1, true);

                trebleFretboardEdge.P2 = bridgeLine.GetIntersection(trebleFretboardEdge);
                bassFretboardEdge.P2 = bridgeLine.GetIntersection(bassFretboardEdge);
            }
            else
            {
                trebleFretboardEdge.P1 = trebleFretboardEdge.SnapToLine(trebleLine.P1, true);
                trebleFretboardEdge.P2 = trebleFretboardEdge.SnapToLine(trebleLine.P2, true);
                bassFretboardEdge.P1 = bassFretboardEdge.SnapToLine(bassLine.P1, true);
                bassFretboardEdge.P2 = bassFretboardEdge.SnapToLine(bassLine.P2, true);
            }
        }

        #endregion

        #region Frets

        private class FretPosition
        {
            public int FretIndex { get; set; }
            public int StringIndex { get; set; }
            public double PositionRatio { get; set; }
            public PointM Position { get; set; }
        }

        private List<FretPosition> CalculateFretsForString(SIString str)
        {
            var frets = new List<FretPosition>();
            if (!CompensateFretPositions)
            {
                for (int i = Strings.Min(s => s.StartingFret); i <= Strings.Max(s => s.NumberOfFrets); i++)
                    frets.Add(CalculateFretPosition(str, i));
            }
            else
            {
                var positions = FretCompensationCalculator.CalculateFretsCompensatedPositions(
                    str.PhysicalProperties, str.FinalLength,
                    str.Tuning.FinalPitch, FretsTemperament,
                    Measure.Inches(0.030), str.ActionAtTwelfthFret, Measure.Mm(1.2), str.TotalNumberOfFrets);
                for (int i = 0; i < positions.Length; i++)
                {
                    double fretPosRatio = (str.FinalLength - positions[i]).NormalizedValue / str.FinalLength.NormalizedValue;
                    var fretPos = str.LayoutLine.P2 + (str.LayoutLine.Direction * -1) * (str.FinalLength * fretPosRatio);
                    frets.Add(new FretPosition() { FretIndex = i - str.StartingFret, Position = fretPos, StringIndex = str.Index, PositionRatio = fretPosRatio });
                }
            }
            return frets;
        }

        private FretPosition CalculateFretPosition(SIString str, int fret)
        {
            var stringTuning = str.Tuning ?? GetDefaultTuning();
            double fretPosRatio = GetRelativeFretPosition(stringTuning, fret - str.StartingFret, FretsTemperament);
            var dir = str.PlaceFretsRelativeToString ? str.LayoutLine.Direction * -1 : new Vector(0, 1);

            var fretPos = str.LayoutLine.P2 + (dir * str.FinalLength * fretPosRatio);
            if (!str.PlaceFretsRelativeToString)
                fretPos = str.LayoutLine.SnapToLine(fretPos, true);

            return new FretPosition() { FretIndex = fret, Position = fretPos, StringIndex = str.Index, PositionRatio = fretPosRatio };
        }

        private void PlaceFrets()
        {
            var stringFrets = new Dictionary<int, List<FretPosition>>();
            foreach (var str in Strings)
                stringFrets.Add(str.Index, CalculateFretsForString(str));

        }

        /// <summary>
        /// Returns the fret position as the distance from the bridge.
        /// </summary>
        /// <param name="fret"></param>
        /// <returns></returns>
        public static double GetEqualTemperedFretPosition(int fret)
        {
            return 1d / Math.Pow(2, fret / 12d);
        }

        public static double GetRelativeFretPosition(StringTuning openString, int fret, Temperament temperament)
        {
            var openCents = openString.FinalPitch.Cents;
            var fretNote = openString.Note.AddSteps(fret);
            double fretCents = fretNote.Pitch.Cents + openString.PitchOffset.Cents;
            
            if (fret != 0)
            {
                if (temperament == Temperament.ThidellFormula)
                    fretCents += NoteConverter.ThidellFormulaChromaticOffsets[(int)fretNote.NoteName];
                else if (temperament == Temperament.DieWohltemperirte)
                    fretCents += NoteConverter.DieWohltemperirteChromaticOffsets[(int)fretNote.NoteName];
            }

            var fretRatio = NoteConverter.CentsToIntonationRatio(fretCents - openCents);
            return 1d / fretRatio;
        }

        private StringTuning GetDefaultTuning()
        {
            var note = FretsTemperament == Temperament.Just ? MusicalNote.JustNote(NoteName.E, 2) : MusicalNote.EqualNote(NoteName.E, 2);
            return new StringTuning(note);
        }

        #endregion
    }
}
