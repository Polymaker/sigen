﻿using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Physics
{
    public static class FretCompensationCalculator
    {
        /// <summary>
        /// Calculates the compensated fret positions (takes into account the pitch offset from pressing the string along the fretboard).
        /// </summary>
        /// <param name="properties">The string's physical properties.</param>
        /// <param name="stringLength">The real string length (not the scale length).</param>
        /// <param name="openTuning">The tuning of the open string (not fretted).</param>
        /// <param name="temperament">The temperament to use.</param>
        /// <param name="actionAtFirstFret">Action at the first fret, measured above the fret.</param>
        /// <param name="actionAtTwelfthFret">Action at the twelfth fret, measured above the fret.</param>
        /// <param name="fretHeight">Fret height. Set to zero for fretless.</param>
        /// <param name="numberOfFrets">Number of fret to calculate.</param>
        /// <returns>Returns an array of fret positions (distances from the bridge) starting at fret 0 (nut)</returns>
        public static Measure[] CalculateFretsCompensatedPositions(StringProperties properties, Measure stringLength, PitchValue openTuning, Temperament temperament,
            Measure actionAtFirstFret, Measure actionAtTwelfthFret, Measure fretHeight, int numberOfFrets)
        {
            var fretPositions = new PreciseDouble[numberOfFrets + 1];//+1 to include nut and improve readability (e.g. fretPositions[1] = fret #1 instead of fretPositions[0])
            var frettedLengths = new PreciseDouble[numberOfFrets + 1];//+1 to include nut and improve readability (e.g. frettedLengths[1] = fret #1 instead of frettedLengths[0])
            var frettedTensions = new PreciseDouble[numberOfFrets + 1];//+1 to include nut and improve readability (e.g. frettedTensions[1] = fret #1 instead of frettedTensions[0])
            var finalFretPositions = new Measure[numberOfFrets + 1];
            var fretHeightIn = fretHeight[UnitOfMeasure.Inches];
            //Calculate the fret positions
            PreciseDouble flatLength = stringLength[UnitOfMeasure.Inches];
            for (int i = 1; i <= numberOfFrets; i++)//i=1 to skip the nut
            {
                var fretRatio = (PreciseDouble)Math.Pow(2, i / 12d);
                //fretRatio = GetAdjustedFretRatio(openTuning, i, temperament);
                fretPositions[i] = flatLength - (flatLength / fretRatio);
            }

            var gaugeOffset = (properties.StringDiameter / 2d)[UnitOfMeasure.Inches];
            var fret1Action = new Vector(fretPositions[1], 
                actionAtFirstFret[UnitOfMeasure.Inches] + 
                fretHeight[UnitOfMeasure.Inches]);

            var fret12Action = new Vector(fretPositions[12], 
                actionAtTwelfthFret[UnitOfMeasure.Inches] + 
                fretHeight[UnitOfMeasure.Inches]);
            
            var actionLineEq = Line.FromPoints(fret1Action, fret12Action);
            var nutPos = actionLineEq.GetPointForX(0);
            var saddlePos = actionLineEq.GetPointForX(flatLength);
            var saddleToNutDir = (nutPos - saddlePos).Normalized;
            frettedLengths[0] = (nutPos - saddlePos).Length;//actual string length
            finalFretPositions[0] = Measure.Zero;

            PreciseDouble L = frettedLengths[0];// stringLength[UnitOfMeasure.Inches];
            PreciseDouble f = openTuning.Frequency;//Frequency, Hz.
            PreciseDouble E = properties.ModulusOfElasticity;//Modulus of Elasticity, core wire, GPa
            E *= 145038d;//GPa to PSI

            PreciseDouble A = properties.CoreWireArea != 0 ? properties.CoreWireArea : properties.StringArea;//Area, core wire, in²
            PreciseDouble mul = properties.UnitWeight;//String mass per unit length, lbs./ inch
			PreciseDouble g = 386.089; //Gravity, 386.089 in./ sec²

            //Calculate the open string tension: T = (mul * (2* L* f )^2) / g
            double T = (mul * Math.Pow((2 * L * f), 2)) / g;

            //Calculate "unstressed" string length: Lor = L / ((T / (E * A)) + 1)
            double Lor = L / ((T / (E * A)) + 1);

            for (int i = 1; i <= numberOfFrets; i++)//i=1 to skip the nut
            {
                var fretFreq = GetPitchAtFret(openTuning, i, temperament).Frequency;
                var fretTopPos = new Vector(fretPositions[i], fretHeightIn);
                var prevFretPos = new Vector(fretPositions[i - 1], fretHeightIn);
                
                if (i == 1)
                    prevFretPos.Y = nutPos.Y;

                var fingerPressPos = new Vector();
                fingerPressPos.X = prevFretPos.X;// (fretTopPos.X + prevFretPos.X) / 2d;
                if (i == 1)
                    fingerPressPos.X = (fretTopPos.X + prevFretPos.X) / 2d;
                fingerPressPos.Y = fretHeightIn;
                //fingerPressPos.Y = PreciseDouble.Max(fretHeightIn - gaugeOffset, gaugeOffset);

                //var fretCenterPos = new Vector(fretPositions[i - 1], i == 1 ? nutPos.Y : fretHeight[UnitOfMeasure.Inches]);

                //Calculate the fretted string length : Ls(n) = The sum of the distances between the top of the fret from the nut and saddle
                //var Lsn = frettedLengths[i] =
                //    (saddlePos - fretTopPos).Length +
                //    (fretTopPos - fingerPressPos).Length +
                //    (fingerPressPos - nutPos).Length;

                var Lsn = frettedLengths[i] =
                    (saddlePos - fretTopPos).Length +
                    (fretTopPos - nutPos).Length;

                //var Lsn = frettedLengths[i] = (nutPos - fretCenterPos).Length + (fretCenterPos - fretTopPos).Length + (fretTopPos - saddlePos).Length;

                //Calculate the fretted string tension : Ts(n) = ((Lsn - Lor) / Lor) * E * A
                var Tsn = frettedTensions[i] = ((Lsn - Lor) / Lor) * E * A;
                //Calculate fret compensated position: Lfret(n) = SQRT((g * Tsn )/ mul ) / (2 * fn )

                var Lfretn = MathP.Sqrt((g * Tsn) / mul) / (2d * fretFreq);

                //var opp = saddlePos.Y - fretHeightIn;
                //var theta = MathP.Asin(opp / Lfretn);
                //var fretDist = MathP.Cos(theta) * Lfretn;
                //var pos2D = saddlePos + (saddleToNutDir * Lfretn);
                //var testPos = flatLength - fretDist;

                var finalPos = flatLength - Lfretn;
                fretPositions[i] = finalPos;
                finalFretPositions[i] = Measure.Inches(finalPos);
            }


            return finalFretPositions;
        }

        public static PitchValue GetPitchAtFret(PitchValue openStringPitch, int fret, Temperament temperament)
        {
            var intonation = temperament == Temperament.Just ? IntonationMethod.Just : IntonationMethod.EqualTempered;
            
            var openStringNote = MusicalNote.FromPitch(openStringPitch).AddSteps(0, intonation);
            var fretNote = openStringNote.AddSteps(fret);

            double fretCents = fretNote.Pitch.Cents;

            if (fret != 0)
            {
                //Add the chromatic offset
                if (temperament == Temperament.ThidellFormula)
                    fretCents += NoteConverter.ThidellFormulaChromaticOffsets[(int)fretNote.NoteName];
                else if (temperament == Temperament.DieWohltemperirte)
                    fretCents += NoteConverter.DieWohltemperirteChromaticOffsets[(int)fretNote.NoteName];
            }

            return PitchValue.FromCents(fretCents);
        }

        public static PreciseDouble GetAdjustedFretRatio(PitchValue tuning, int fret, Temperament temperament)
        {
            if (fret == 0)
                return 0d;

            var pitchAtFret = GetPitchAtFret(tuning, fret, temperament);
            PreciseDouble fretRatio = NoteConverter.CentsToIntonationRatio(pitchAtFret.Cents - tuning.Cents);
            return fretRatio;
        }
    }
}
