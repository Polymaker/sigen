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
		public bool IsLayoutDirty => LayoutChanges.Any() || isLayoutDirty;

        public void RebuildLayout()
        {
			if(LayoutChanges.Count == 1 && LayoutChanges[0] is PropertyChange pc && pc.Property == nameof(LeftHanded))
			{
				if(VisualElements.Count > 0)
				{
					VisualElements.ForEach(e => e.FlipHandedness());
					isLayoutDirty = false;
					LayoutChanges.Clear();
					OnLayoutUpdated();
					return;
				}
			}

            VisualElements.Clear();
            for (int i = 0; i < NumberOfStrings; i++)
                Strings[i].ClearLayoutData();

            _CachedBounds = RectangleM.Empty;

            if (StringSpacing is StringSpacingSimple)
                (StringSpacing as StringSpacingSimple).CalculateAdjustedPositions();

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
			LayoutChanges.Clear();
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
                var theta = MathP.Asin(opp.NormalizedValue / str.ScaleLength.NormalizedValue);
                adj = MathP.Cos(theta) * str.ScaleLength;
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
                var twelfthFret = new LayoutLine(PointM.Average(trebleStr.P1, trebleStr.P2), PointM.Average(bassStr.P1, bassStr.P2));

                //create the remaining strings by distributing them equally between the outer strings
                for (int i = 1; i < NumberOfStrings - 1; i++)
                {
                    var nutPos = nutLine.GetPointForX(nutStringPos[i].NormalizedValue);
                    var bridgePos = bridgeLine.GetPointForX(bridgeStringPos[i].NormalizedValue);
                    var createdString = AddVisualElement(new StringLine(Strings[i],
                        PointM.FromVector(nutPos, nutStringPos[i].Unit),
                        PointM.FromVector(bridgePos, bridgeStringPos[i].Unit)));

                    //strings distributed equally between the outer strings (which are tapered/angled) do not have their centers aligned
                    //so we correct the string length (at bridge) so that its center is aligned with the twelfth fret

                    var middle = createdString.GetIntersection(twelfthFret);
                    var distFromNut = PointM.Distance(createdString.P1, middle);
                    var distFromBridge = PointM.Distance(createdString.P2, middle);

                    if (!Measure.EqualOrClose(distFromNut, distFromBridge, Measure.Mm(0.05)))
                        createdString.P2 = createdString.P1 + (createdString.Direction * distFromNut * 2);

                    Strings[i].RecalculateLengths();//store the physical length of the string
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
                        startingFretPos = str.LayoutLine.SnapToLine(startingFretPos, LineSnapDirection.Horizontal);

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
            var ratio = 0.5;

            if (ScaleLengthMode == ScaleLengthType.Individual)
                ratio = stringLine.String.MultiScaleRatio;
            else if (ScaleLengthMode == ScaleLengthType.Multiple)
                ratio = MultiScaleConfig.PerpendicularFretRatio;

            var offsetY = (maxPerpHeight - stringLine.Bounds.Height) * (0.5 - ratio);
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
            //apply margin perpendicularly to the treble string
            var trebleLine = FirstString.LayoutLine;
            var trebleOffset = Measure.Zero;
            if (Margins.CompensateStringGauge && !FirstString.Gauge.IsEmpty)
                trebleOffset += FirstString.Gauge / 2;
            var trebleFretboardEdge = AddVisualElement(new FingerboardSideEdge(
               trebleLine.GetPerpendicularPoint(trebleLine.P1, Margins.TrebleMargins[FingerboardEnd.Nut] + trebleOffset),
               trebleLine.GetPerpendicularPoint(trebleLine.P2, Margins.TrebleMargins[FingerboardEnd.Bridge] + trebleOffset),
               FingerboardSide.Treble));

            //apply margin perpendicularly to the bass string
            var bassLine = LastString.LayoutLine;
            var bassOffset = Measure.Zero;
            if (Margins.CompensateStringGauge && !LastString.Gauge.IsEmpty)
                bassOffset += LastString.Gauge / 2;
            var bassFretboardEdge = AddVisualElement(new FingerboardSideEdge(
               bassLine.GetPerpendicularPoint(bassLine.P1, (Margins.BassMargins[FingerboardEnd.Nut] + bassOffset) * -1), //*-1 to offset towards left
               bassLine.GetPerpendicularPoint(bassLine.P2, (Margins.BassMargins[FingerboardEnd.Bridge] + bassOffset) * -1),
               FingerboardSide.Bass));

            //adjust the end points of each edges so that they are "inline" with the strings
            if(NumberOfStrings >= 2)
            {
                var trebOpp = GetStringBoundaryLine(FirstString, FingerboardSide.Bass) as StringCenter;
                var tp = trebOpp.GetRelativePoint(trebleLine, trebleLine.P1);
                var trebNutLine = new LayoutLine(tp, trebleLine.P1);
                var trebBriLine = new LayoutLine(trebOpp.P2, trebleLine.P2);
                trebleFretboardEdge.P1 = trebleFretboardEdge.GetIntersection(trebNutLine);
                trebleFretboardEdge.P2 = trebleFretboardEdge.GetIntersection(trebBriLine);

                var bassOpp = GetStringBoundaryLine(LastString, FingerboardSide.Treble) as StringCenter;
                var bp = bassOpp.GetRelativePoint(bassLine, bassLine.P1);
                var bassNutLine = new LayoutLine(bp, bassLine.P1);
                var bassBriLine = new LayoutLine(bassOpp.P2, bassLine.P2);
                bassFretboardEdge.P1 = bassFretboardEdge.GetIntersection(bassNutLine);
                bassFretboardEdge.P2 = bassFretboardEdge.GetIntersection(bassBriLine);
            }
            //else if(/*ScaleLengthMode != ScaleLengthType.Individual && */NumberOfStrings > 1)
            //{
            //    var nutLine = new LayoutLine(trebleLine.FretZero, bassLine.FretZero);
            //    var bridgeLine = new LayoutLine(trebleLine.P2, bassLine.P2);

            //    if (FirstString.StartingFret == 0)
            //        trebleFretboardEdge.P1 = nutLine.GetIntersection(trebleFretboardEdge);
            //    else
            //        trebleFretboardEdge.P1 = trebleFretboardEdge.SnapToLine(trebleLine.P1, true);

            //    if (LastString.StartingFret == 0)
            //        bassFretboardEdge.P1 = nutLine.GetIntersection(bassFretboardEdge);
            //    else
            //        bassFretboardEdge.P1 = bassFretboardEdge.SnapToLine(bassLine.P1, true);

            //    trebleFretboardEdge.P2 = bridgeLine.GetIntersection(trebleFretboardEdge);
            //    bassFretboardEdge.P2 = bridgeLine.GetIntersection(bassFretboardEdge);
            //}
            else
            {
                trebleFretboardEdge.P1 = trebleFretboardEdge.SnapToLine(trebleLine.P1, LineSnapDirection.Horizontal);
                trebleFretboardEdge.P2 = trebleFretboardEdge.SnapToLine(trebleLine.P2, LineSnapDirection.Horizontal);
                bassFretboardEdge.P1 = bassFretboardEdge.SnapToLine(bassLine.P1, LineSnapDirection.Horizontal);
                bassFretboardEdge.P2 = bassFretboardEdge.SnapToLine(bassLine.P2, LineSnapDirection.Horizontal);
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
                        var nut1 = VisualElements.OfType<FretLine>().First(f => f.IsNut && f.Strings.Contains(str));
                        var nut2 = VisualElements.OfType<FretLine>().First(f => f.IsNut && f.Strings.Contains(str.Next));
                        AddVisualElement(new FingerboardEdge(nut1.Points.First(), nut2.Points.Last()) { IsSideEdge = true });

                        //var center = GetStringBoundaryLine(str, FingerboardSide.Bass) as StringCenter;
                        //var p1 = center.GetRelativePoint(str.LayoutLine, str.LayoutLine.P1);
                        //var p2 = center.GetRelativePoint(str.Next.LayoutLine, str.Next.LayoutLine.P1);
                        //AddVisualElement(new FingerboardEdge(p1, p2) { IsSideEdge = true });
                    }
                }
            }

            var trebSideNutInter = PointM.Empty;
            if (trebleNutFret.Intersects(trebleSideEdge, out trebSideNutInter))
                trebleSideEdge.P1 = trebSideNutInter;

            var bassSideNutInter = PointM.Empty;
            if (bassNutFret.Intersects(bassSideEdge, out bassSideNutInter))
                bassSideEdge.P1 = bassSideNutInter;

            trebleSideEdge.RealEnd = trebleSideEdge.P2;
            bassSideEdge.RealEnd = bassSideEdge.P2;

            var trebleLastFret = VisualElements.OfType<FretLine>().First(f => f.FretIndex == FirstString.NumberOfFrets && f.Strings.Contains(FirstString));
            var bassLastFret = VisualElements.OfType<FretLine>().First(f => f.FretIndex == LastString.NumberOfFrets && f.Strings.Contains(LastString));

            PointM trebleEndPoint = trebleLastFret.Points.Last();
            PointM bassEndPoint = bassLastFret.Points.First();

            if (!Margins.LastFret.IsEmpty)
            {
                trebleEndPoint += trebleSideEdge.Direction * Margins.LastFret;
                bassEndPoint += bassSideEdge.Direction * Margins.LastFret;
            }

            //split physical from virtual fingerboard
            var virtualTrebleEdge = AddVisualElement(new LayoutLine(trebleEndPoint, trebleSideEdge.P2, VisualElementType.GuideLine));
            var virtualBassEdge = AddVisualElement(new LayoutLine(bassEndPoint, bassSideEdge.P2, VisualElementType.GuideLine));
            trebleSideEdge.P2 = virtualTrebleEdge.P1;
            bassSideEdge.P2 = virtualBassEdge.P1;

            var bridgeLine = new LayoutPolyLine(Strings.Select(s => s.LayoutLine.P2));
            
            var bridgeTrebleInter = PointM.Empty;
            if (bridgeLine.Intersects(virtualTrebleEdge, out bridgeTrebleInter))
                virtualTrebleEdge.P2 = bridgeTrebleInter;

            var bridgeBassInter = PointM.Empty;
            if (bridgeLine.Intersects(virtualBassEdge, out bridgeBassInter))
                virtualBassEdge.P2 = bridgeBassInter;

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
                    var pt1 = PointM.Empty;
                    var pt2 = PointM.Empty;

                    if (!strLastFret.Intersects(bassSide, out pt1))
                        pt1 = strLastFret.Points.First();

                    if (!strLastFret.Intersects(trebleSide, out pt2))
                        pt2 = strLastFret.Points.Last();

                    pt1 += bassSide.Direction * Margins.LastFret;
                    pt2 += trebleSide.Direction * Margins.LastFret;

                    edgePoints.Add(pt1);
                    edgePoints.Add(pt2);
                }

                //edgePoints.RemoveAll(p => p.IsEmpty);
                edgePoints = edgePoints.Distinct().ToList();
                var fretboardEdge = AddVisualElement(new FingerboardEdge(edgePoints));
                //fretboardEdge.InterpolateSpline();
            }
        }

        #endregion

        #region Frets

        private class FretPosition
        {
            public int FretIndex { get; set; }
            public int StringIndex { get; set; }
            public PreciseDouble PositionRatio { get; set; }
            public PointM Position { get; set; }
        }

        public int MinimumFret { get { return Strings.Min(s => s.StartingFret); } }
        public int MaximumFret { get { return Strings.Max(s => s.NumberOfFrets); } }

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
                    PreciseDouble fretPosRatio = (str.StringLength - positions[i]).NormalizedValue / str.StringLength.NormalizedValue;
                    var fretPos = str.LayoutLine.P2 + (str.LayoutLine.Direction * -1) * (str.StringLength * fretPosRatio);
                    frets.Add(new FretPosition() { FretIndex = i - str.StartingFret, Position = fretPos, StringIndex = str.Index, PositionRatio = fretPosRatio });
                }
            }
            return frets;
        }

        private FretPosition CalculateFretPosition(SIString str, int fret)
        {
            var stringTuning = str.Tuning ?? GetDefaultTuning();
            var fretPosRatio = GetRelativeFretPosition(stringTuning, fret - str.StartingFret, FretsTemperament);
            var dir = str.PlaceFretsRelativeToString ? str.LayoutLine.Direction * -1 : new Vector(0, 1);

            var fretPos = str.LayoutLine.P2 + (dir * str.CalculatedLength * fretPosRatio);
            if (!str.PlaceFretsRelativeToString)
                fretPos = str.LayoutLine.SnapToLine(fretPos, LineSnapDirection.Horizontal);

            return new FretPosition() { FretIndex = fret, Position = fretPos, StringIndex = str.Index, PositionRatio = fretPosRatio };
        }

        private void PlaceFrets()
        {
            var stringFrets = new Dictionary<int, List<FretPosition>>();
            foreach (var str in Strings)
                stringFrets.Add(str.Index, CalculateFretsForString(str));

            var fretSegments = new List<FretSegment>();
            //create fret "segments"; store each fret position for each strings in a list
            foreach (var str in Strings)
            {
                for (int i = MinimumFret; i <= MaximumFret; i++)
                {
                    if (stringFrets[str.Index].Any(f=>f.FretIndex == i))
                    {
                        var fretPos = stringFrets[str.Index].First(f => f.FretIndex == i);
                        var perpLine = str.LayoutLine.Equation.GetPerpendicular(fretPos.Position.ToVector());
                        var leftLine = GetStringBoundaryLine(str, FingerboardSide.Bass);
                        var rightLine = GetStringBoundaryLine(str, FingerboardSide.Treble);
                        var p1 = PointM.FromVector(perpLine.GetIntersection(leftLine.Equation), fretPos.Position.Unit);
                        var p2 = PointM.FromVector(perpLine.GetIntersection(rightLine.Equation), fretPos.Position.Unit);

                        var fretSeg = new FretSegment(i, str, fretPos.Position, p1, p2) { IsVirtual = !str.HasFret(i) };
                        fretSegments.Add(fretSeg);
                    }
                }
            }

            //regroup fret segments that are connected to form a line/curve
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
                    line.ComputeFretShape();
                    s += followingStrings.Count() - 1;
                }
            }
        }

        /// <summary>
        /// Returns the fret position as the distance from the bridge.
        /// </summary>
        /// <param name="fret"></param>
        /// <returns></returns>
        public static PreciseDouble GetEqualTemperedFretPosition(int fret)
        {
            return (PreciseDouble)1d / MathP.Pow(2, fret / 12d);
        }

        public static PreciseDouble GetRelativeFretPosition(StringTuning tuning, int fret, Temperament temperament)
        {
            if (fret == 0)
                return 1d;

            var pitchAtFret = FretCompensationCalculator.GetPitchAtFret(tuning.FinalPitch, fret, temperament);
			PreciseDouble fretRatio = NoteConverter.CentsToIntonationRatio(pitchAtFret.Cents - tuning.FinalPitch.Cents);
            return (PreciseDouble)1d / fretRatio;
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

        #endregion
    }
}
