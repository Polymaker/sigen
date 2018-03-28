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
        public bool IsLayoutDirty { get { return isLayoutDirty; } }

        public void RebuildLayout()
        {
            VisualElements.Clear();
            _CachedBounds = RectangleM.Empty;

            if (StringSpacing is StringSpacingSimple)
                (StringSpacing as StringSpacingSimple).CalculateNutSlotPositions();

            Measure nutCenter = Measure.Zero;
            Measure bridgeCenter = Measure.Zero;
            Measure[] nutStringPos = StringSpacing.GetStringPositions(FingerboardEnd.Nut, out nutCenter);
            Measure[] bridgeStringPos = StringSpacing.GetStringPositions(FingerboardEnd.Bridge, out bridgeCenter);

            LayoutStrings(nutStringPos, bridgeStringPos);
            CreateFingerboardEdges();
            PlaceFrets();
            FinishFingerboardShape();
            if (LeftHanded)
                VisualElements.ForEach(e => e.FlipHandedness());
            isLayoutDirty = false;
            OnLayoutUpdated();
        }

        protected void OnLayoutUpdated()
        {
            LayoutUpdated?.Invoke(this, EventArgs.Empty);
        }

        private T AddVisualElement<T>(T elem) where T : VisualElement
        {
            VisualElements.Add(elem);
            elem.Layout = this;
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
                    Strings[i].RecalculateLengths();
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
                    var startingFretPos = str.LayoutLine.P2 + (stringVector * str.CalculatedLength * startingFretPosRatio);

                    if (!str.PlaceFretsRelativeToString)
                        startingFretPos = str.LayoutLine.SnapToLine(startingFretPos, true);

                    str.LayoutLine.P1 = startingFretPos;
                    str.RecalculateLengths();
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
            var offsetY = (maxPerpHeight - stringLine.Bounds.Height) * (0.5 - stringLine.String.MultiScaleRatio);
            stringLine.P1 += new PointM(Measure.Zero, offsetY);
            stringLine.P2 += new PointM(Measure.Zero, offsetY);
            stringLine.FretZero = stringLine.P1;//keep pos of fret 0 usefull because starting fret can be negative
            stringLine.String.RecalculateLengths();
        }
        
        internal LayoutLine GetStringBoundaryLine(SIString str, FingerboardSide dir)
        {
            LayoutLine boundary = null;
            if (dir == FingerboardSide.Bass)
                boundary = VisualElements.OfType<StringCenter>().FirstOrDefault(c => c.Right.Index == str.Index);
            else
                boundary = VisualElements.OfType<StringCenter>().FirstOrDefault(c => c.Left.Index == str.Index);

            if (boundary == null)
                return VisualElements.OfType<FingerboardSideEdge>().First(e => e.Side == dir);

            return boundary;
        }

        internal StringCenter GetStringsCenter(SIString left, SIString right)
        {
            return VisualElements.OfType<StringCenter>().FirstOrDefault(c => 
            (left.Index == c.Right.Index || left.Index == c.Left.Index) && 
            (right.Index == c.Right.Index || right.Index == c.Left.Index));
        }

        /// <summary>
        /// Treble side string
        /// </summary>
        public SIString FirstString { get { return Strings[0]; } }
        /// <summary>
        /// Bass side string
        /// </summary>
        public SIString LastString { get { return Strings[NumberOfStrings - 1]; } }

        #endregion

        #region Fingerboard

        private void CreateFingerboardEdges()
        {
            var trebleLine = FirstString.LayoutLine;
            var trebleFretboardEdge = AddVisualElement(new FingerboardSideEdge(
               trebleLine.GetPerpendicularPoint(trebleLine.P1, Margins.TrebleMargins[FingerboardEnd.Nut]),
               trebleLine.GetPerpendicularPoint(trebleLine.P2, Margins.TrebleMargins[FingerboardEnd.Bridge]),
               FingerboardSide.Treble));
            var bassLine = LastString.LayoutLine;
            var bassFretboardEdge = AddVisualElement(new FingerboardSideEdge(
               bassLine.GetPerpendicularPoint(bassLine.P1, Margins.BassMargins[FingerboardEnd.Nut] * -1), //*-1 to offset towards left
               bassLine.GetPerpendicularPoint(bassLine.P2, Margins.BassMargins[FingerboardEnd.Bridge] * -1),
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

        private void FinishFingerboardShape()
        {
            var trebleSideEdge = VisualElements.OfType<FingerboardSideEdge>().First(e => e.Side == FingerboardSide.Treble);
            var bassSideEdge = VisualElements.OfType<FingerboardSideEdge>().First(e => e.Side == FingerboardSide.Bass);
            var trebleNutFret = VisualElements.OfType<FretLine>().First(f => f.IsNut && f.Strings.Contains(FirstString));
            var bassNutFret = VisualElements.OfType<FretLine>().First(f => f.IsNut && f.Strings.Contains(LastString));

            //create inward fingerboard edges from nut
            if (!Strings.AllEqual(s => s.StartingFret))
            {
                foreach (var str in Strings)
                {
                    if (str.Next != null && str.StartingFret != str.Next.StartingFret)
                    {
                        var center = GetStringBoundaryLine(str, FingerboardSide.Bass);
                        AddVisualElement(new FingerboardEdge(center.SnapToLine(str.LayoutLine.P1, true), center.SnapToLine(str.Next.LayoutLine.P1, true)));
                    }
                }
            }

            var trebSideNutInter = trebleNutFret.GetIntersection(trebleSideEdge);
            if (!trebSideNutInter.IsEmpty)
                trebleSideEdge.P1 = trebSideNutInter;

            var bassSideNutInter = bassNutFret.GetIntersection(bassSideEdge);
            if (!bassSideNutInter.IsEmpty)
                bassSideEdge.P1 = bassSideNutInter;

            var trebleLastFret = VisualElements.OfType<FretLine>().First(f => f.FretIndex == FirstString.NumberOfFrets && f.Strings.Contains(FirstString));
            var bassLastFret = VisualElements.OfType<FretLine>().First(f => f.FretIndex == LastString.NumberOfFrets && f.Strings.Contains(LastString));

            PointM trebleEndPoint = trebleLastFret.Points.Last();
            PointM bassEndPoint = bassLastFret.Points.First();

            if (!Margins.LastFret.IsEmpty)
            {
                trebleEndPoint += trebleSideEdge.Direction * Margins.LastFret;
                bassEndPoint += bassSideEdge.Direction * Margins.LastFret;
            }

            var virtualTrebleEdge = AddVisualElement(new LayoutLine(trebleEndPoint, trebleSideEdge.P2, VisualElementType.GuideLine));
            var virtualBassEdge = AddVisualElement(new LayoutLine(bassEndPoint, bassSideEdge.P2, VisualElementType.GuideLine));
            trebleSideEdge.P2 = virtualTrebleEdge.P1;
            bassSideEdge.P2 = virtualBassEdge.P1;

            if (trebleLastFret.Strings.Count() == NumberOfStrings && trebleLastFret.IsStraight && trebleLastFret.FretIndex == MaximumFret)
            {
                AddVisualElement(new FingerboardEdge(bassSideEdge.P2, trebleSideEdge.P2));
            }
            else if (!Margins.LastFret.IsEmpty)
            {
                var fretLines = VisualElements.OfType<FretLine>();
                var edgePoints = new List<PointM>();

                for (int i = NumberOfStrings - 1; i >= 0; i--)
                {
                    var strLastFret = fretLines.First(fl => fl.FretIndex == Strings[i].NumberOfFrets && fl.Strings.Contains(Strings[i]));
                    var trebleSide = GetStringBoundaryLine(Strings[i], FingerboardSide.Treble);
                    var bassSide = GetStringBoundaryLine(Strings[i], FingerboardSide.Bass);
                    var pt1 = strLastFret.GetIntersection(bassSide);
                    var pt2 = strLastFret.GetIntersection(trebleSide);

                    if (pt1.IsEmpty)
                        pt1 = strLastFret.Points.First();
                    if (pt2.IsEmpty)
                        pt2 = strLastFret.Points.Last();

                    pt1 += bassSide.Direction * Margins.LastFret;
                    pt2 += trebleSide.Direction * Margins.LastFret;

                    edgePoints.Add(pt1);
                    edgePoints.Add(pt2);
                }

                edgePoints.RemoveAll(p => p.IsEmpty);
                edgePoints = edgePoints.Distinct().ToList();
                for (int i = 0; i < edgePoints.Count - 1; i++)
                    AddVisualElement(new FingerboardEdge(edgePoints[i], edgePoints[i + 1]));
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

        private int MinimumFret { get { return Strings.Min(s => s.StartingFret); } }
        private int MaximumFret { get { return Strings.Max(s => s.NumberOfFrets); } }

        private List<FretPosition> CalculateFretsForString(SIString str)
        {
            var frets = new List<FretPosition>();
            if (!CompensateFretPositions)
            {
                for (int i = MinimumFret; i <= MaximumFret; i++)
                    frets.Add(CalculateFretPosition(str, i));
            }
            else
            {
                var positions = FretCompensationCalculator.CalculateFretsCompensatedPositions(
                    str.PhysicalProperties, str.StringLength,
                    str.Tuning.FinalPitch, FretsTemperament,
                    Measure.Mm(0.5), str.ActionAtTwelfthFret, Measure.Mm(1.2), str.TotalNumberOfFrets);
                for (int i = 0; i < positions.Length; i++)
                {
                    double fretPosRatio = (str.StringLength - positions[i]).NormalizedValue / str.StringLength.NormalizedValue;
                    var fretPos = str.LayoutLine.P2 + (str.LayoutLine.Direction * -1) * (str.StringLength * fretPosRatio);
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

            var fretPos = str.LayoutLine.P2 + (dir * str.CalculatedLength * fretPosRatio);
            if (!str.PlaceFretsRelativeToString)
                fretPos = str.LayoutLine.SnapToLine(fretPos, true);

            return new FretPosition() { FretIndex = fret, Position = fretPos, StringIndex = str.Index, PositionRatio = fretPosRatio };
        }

        private void PlaceFrets()
        {
            var stringFrets = new Dictionary<int, List<FretPosition>>();
            foreach (var str in Strings)
                stringFrets.Add(str.Index, CalculateFretsForString(str));
            var fretSegments = new List<FretSegment>();
            foreach (var str in Strings)
            {
                for (int i = MinimumFret; i <= MaximumFret; i++)
                {
                    if (str.HasFret(i) || stringFrets[str.Index].Any(f=>f.FretIndex == i))
                    {
                        var fretPos = stringFrets[str.Index].First(f => f.FretIndex == i);
                        var perpLine = str.LayoutLine.Equation.GetPerpendicular(fretPos.Position.ToVector());
                        var leftLine = GetStringBoundaryLine(str, FingerboardSide.Bass);
                        var rightLine = GetStringBoundaryLine(str, FingerboardSide.Treble);
                        var p1 = PointM.FromVector(perpLine.GetIntersection(leftLine.Equation), fretPos.Position.Unit);
                        var p2 = PointM.FromVector(perpLine.GetIntersection(rightLine.Equation), fretPos.Position.Unit);
                        //var seg = AddVisualElement(new FretSegment(i, str, fretPos.Position, p1, p2));
                        var seg = new FretSegment(i, str, fretPos.Position, p1, p2);
                        fretSegments.Add(seg);
                        if (!str.HasFret(i))
                            seg.IsVirtual = true;
                    }
                }
            }
            //var fretSegments = VisualElements.OfType<FretSegment>();
            for (int f = MinimumFret; f <= MaximumFret; f++)
            {
                for(int s = 0; s < NumberOfStrings; s++)
                {
                    if (!Strings[s].HasFret(f))
                        continue;
                    var followingStrings = Strings.Skip(s).TakeWhile(x => x.HasFret(f));
                    var followingFrets = fretSegments.Where(fs => fs.FretIndex == f && followingStrings.Contains(fs.String)).ToList();

                    followingFrets.AddRange(fretSegments.Where(fs => fs.IsVirtual && fs.FretIndex == f 
                    && (fs.String == followingFrets.Last().String.Next || 
                    fs.String == followingFrets.First().String.Previous)).Select(fs=>fs.Clone()));

                    var line = AddVisualElement(new FretLine(followingFrets));
                    line.BuildLayout();
                    s += followingStrings.Count() - 1;
                }
            }
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

        public bool ShouldHaveStraightFrets()
        {
            if (NumberOfStrings == 1)
                return true;

            if (FretsTemperament != Temperament.Equal || CompensateFretPositions)
                return false;

            if(ScaleLengthMode == ScaleLengthType.Multiple)
            {
                return Strings.AllEqual(s => s.MultiScaleRatio);
            }
            else if(ScaleLengthMode == ScaleLengthType.Individual)
            {
                if(Strings.AllEqual(s => s.MultiScaleRatio))
                {
                    var scaleDiff = Measure.Abs(LastString.ScaleLength - FirstString.ScaleLength) / (NumberOfStrings - 1);
                    for(int i = 0; i < NumberOfStrings - 1; i++)
                    {
                        var diff = Measure.Abs(Strings[i + 1].ScaleLength - Strings[i].ScaleLength);
                        if (1 - (diff.NormalizedValue / scaleDiff.NormalizedValue) > 0.1)
                            return false;
                    }
                    return true;
                }
            }
            return ScaleLengthMode == ScaleLengthType.Single;
        }

        private StringTuning GetDefaultTuning()
        {
            var note = FretsTemperament == Temperament.Just ? MusicalNote.JustNote(NoteName.E, 2) : MusicalNote.EqualNote(NoteName.E, 2);
            return new StringTuning(note);
        }

        private FretSegment GetFretForString(SIString str, int fret)
        {
            var fretLine = VisualElements.OfType<FretLine>().FirstOrDefault(fl => fl.FretIndex == 0 && fl.Segments.Any(s => s.String == str));
            if(fretLine != null)
                return fretLine.Segments.First(s => s.String == str);
            return null;
        }

        #endregion
    }
}
