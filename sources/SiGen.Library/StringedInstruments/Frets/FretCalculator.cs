using SiGen.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Frets
{
    public static class FretCalculator
    {
        public static readonly double[] ThidellFormulaChromaticOffsets = new double[] { 2, -4, 2, -4, -2, 0, -4, 4, -4, 0, -4, -1 };
        public static readonly double[] DieWohltemperirteChromaticOffsets = new double[] { 5.9, 1.4, 2, 0.6, -2, 7.8, -1.4, 3.9, 0.2, 0, 3.9, 0 };

        public static List<FretPosition> CalculateFrets(List<PitchValue> pitchOffsets)
        {
            var frets = new List<FretPosition>();
            for (int i = 0; i <= pitchOffsets.Count; i++)
                frets.Add(new FretPosition(i, pitchOffsets[i]));
            return frets;
        }

        public static List<FretPosition> CalculateFrets(PitchValue stringTuning, int numberOfFrets, Temperament temperament)
        {
            var pitchOffsets = new List<PitchValue>();
            var baseNote = MusicalNote.FromPitch(stringTuning, temperament == Temperament.Just ? IntonationMethod.Just : IntonationMethod.EqualTempered);

            for (int i = 0; i <= numberOfFrets; i++)
            {
                var fretNote = baseNote.AddSteps(i);
                var pitchOffset = fretNote.Pitch - baseNote.Pitch;

                ////consider (or not?) the open string pitch offset 
                //pitchOffset += (stringTuning - baseNote.Pitch);


                if (i > 0 && temperament == Temperament.DieWohltemperirte)
                    pitchOffset += PitchValue.FromCents(DieWohltemperirteChromaticOffsets[(int)fretNote.NoteName]);
                else if (i > 0 && temperament == Temperament.ThidellFormula)
                    pitchOffset += PitchValue.FromCents(ThidellFormulaChromaticOffsets[(int)fretNote.NoteName]);

                pitchOffsets.Add(fretNote.Pitch - baseNote.Pitch);
            }
            return CalculateFrets(pitchOffsets);
        }
    }
}
