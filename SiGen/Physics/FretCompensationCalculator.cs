using SiGen.Maths;
using SiGen.Measuring;
using SiGen.StringedInstruments.Data;
using System;
using System.Collections.Generic;
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

            //Calculate the fret positions
            PreciseDouble flatLength = stringLength[UnitOfMeasure.Inches];
            for (int i = 1; i <= numberOfFrets; i++)//i=1 to skip the nut
            {
                var fretRatio = Math.Pow(2, i / 12d);
                fretPositions[i] = flatLength - (flatLength / fretRatio);
            }

            var fret1Action = new Vector(fretPositions[1], actionAtFirstFret[UnitOfMeasure.Inches] + fretHeight[UnitOfMeasure.Inches]);
            var fret12Action = new Vector(fretPositions[12], actionAtTwelfthFret[UnitOfMeasure.Inches] + fretHeight[UnitOfMeasure.Inches]);
            var actionLineEq = Line.FromPoints(fret1Action, fret12Action);
            var nutPos = actionLineEq.GetPointForX(0);
            var saddlePos = actionLineEq.GetPointForX(flatLength);
            var saddleToNutDir = (nutPos - saddlePos).Normalized;
            frettedLengths[0] = (nutPos - saddlePos).Length;//actual string length
            finalFretPositions[0] = Measure.Zero;

            PreciseDouble L = frettedLengths[0];// stringLength[UnitOfMeasure.Inches];
            PreciseDouble f = openTuning.Frequency;//Frequency, Hz.
            PreciseDouble E = properties.ModulusOfElasticity;//Modulus of Elasticity, core wire, psi
            PreciseDouble A = properties.CoreWireArea != 0 ? properties.CoreWireArea : properties.StringArea;//Area, core wire, in²
            PreciseDouble mul = properties.UnitWeight;//String mass per unit length, lbs./ inch
			PreciseDouble g = 386.089; //Gravity, 386.089 in./ sec²

            //Calculate the open string tension: T = (mul * (2* L* f )^2) / g
            double T = (mul * Math.Pow((2 * L * f), 2)) / g;

            //Calculate "unstressed" string length: Lor = L / ((T / (E * A)) + 1)
            double Lor = L / ((T / (E * A)) + 1);

            for (int i = 1; i <= numberOfFrets; i++)//i=1 to skip the nut
            {
                var fretTopPos = new Vector(fretPositions[i], fretHeight[UnitOfMeasure.Inches]);
                var fretCenterPos = new Vector(fretPositions[i - 1], i == 1 ? nutPos.Y : fretHeight[UnitOfMeasure.Inches]);

                //Calculate the fretted string length : Ls(n) = The sum of the distances between the top of the fret from the nut and saddle
                //var Lsn = frettedLengths[i] = (nutPos - fretTopPos).Length + (saddlePos - fretTopPos).Length;
                var Lsn = frettedLengths[i] = (nutPos - fretCenterPos).Length + (fretCenterPos - fretTopPos).Length + (fretTopPos - saddlePos).Length;

                //Calculate the fretted string tension : Ts(n) = ((Lsn - Lor) / Lor) * E * A
                var Tsn = frettedTensions[i] = ((Lsn - Lor) / Lor) * E * A;
                //Calculate fret compensated position: Lfret(n) = SQRT((g * Tsn )/ mul ) / (2 * fn )
                var Lfretn = MathP.Sqrt((g * Tsn) / mul) / (2 * GetPitchAtFret(openTuning, i, temperament).Frequency);
                var pos2D = saddlePos + (saddleToNutDir * Lfretn);
                finalFretPositions[i] = Measure.Inches(pos2D.X);
            }

            return finalFretPositions;
        }

        public static PitchValue GetPitchAtFret(PitchValue openStringPitch, int fret, Temperament temperament)
        {
            var intonation = temperament == Temperament.Just ? IntonationMethod.Just : IntonationMethod.EqualTempered;
            var openStringNote = MusicalNote.FromPitch(openStringPitch).AddSteps(0, intonation);
            var fretNote = openStringNote.AddSteps(fret);

            double fretCents = fretNote.Pitch.Cents;

            ////consider (or not?) the open string pitch offset 
            //fretCents += openStringPitch.Cents - openStringNote.Pitch.Cents;

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

    }
}
