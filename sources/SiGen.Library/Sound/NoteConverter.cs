using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Sound
{
    public static class NoteConverter
    {
        #region Static Consts

        public const double A4HZ = 440d;
        public const double C4HZ = 261.63d;
        public static readonly double TwelfthRoot = Math.Pow(2d, 1d / 12d);
        public static readonly double[] JustScaleRatios = new double[]
        {
            1,//C
            16d/15d,//C#
            9d/8d,//D
            6d/5d,//Eb
            5d/4d,//E
            4d/3d,//F
            7d/5d,//F#
            3d/2d,//G
            8d/5d,//Ab
            5d/3d,//A
            16d/9d,//Bb
            15d/8d,//B
        };

        public static readonly double[] ThidellFormulaChromaticOffsets = new double[] { 2, -4, 2, -4, -2, 0, -4, 4, -4, 0, -4, -1 };
        public static readonly double[] DieWohltemperirteChromaticOffsets = new double[] { 5.9, 1.4, 2, 0.6, -2, 7.8, -1.4, 3.9, 0.2, 0, 3.9, 0 };

        #endregion

        #region Cents <-> Ratio Conversion

        public static double IntonationRatioToCents(double ratio)
        {
            return Math.Log(ratio, 2) * 1200d;
        }

        public static double CentsToIntonationRatio(double cents)
        {
            return Math.Pow(2d, cents / 1200d);
        }

        #endregion

        #region Pitch <-> Note

        public static PitchValue GetPitchFromNote(NoteName note, int octave, IntonationMethod intonation)
        {
            var noteCents = octave * 1200d;
            switch (intonation)
            {
                default:
                case IntonationMethod.EqualTempered:
                    noteCents += IntonationRatioToCents(Math.Pow(TwelfthRoot, (int)note));
                    break;
                case IntonationMethod.Just:
                    noteCents += IntonationRatioToCents(JustScaleRatios[(int)note]);
                    break;
            }
            return PitchValue.FromCents(noteCents);
        }

        #endregion

        #region Frequency Calculation

        public static double CalculateFrequency(PitchValue referenceNote, double referenceFrequency, PitchValue note)
        {
            return referenceFrequency * (note.Ratio / referenceNote.Ratio);
        }

        public static double CalculateFrequency(MusicalNote referenceNote, double referenceFrequency, MusicalNote note)
        {
            return CalculateFrequency(referenceNote.Pitch, referenceFrequency, note.Pitch);
        }

        public static double CalculateFrequency(MusicalNote note)
        {
            return CalculateFrequency(MusicalNote.EqualNote(NoteName.A, 4), A4HZ, note);
        }

        public static double CalculateFrequency(PitchValue note)
        {
            return CalculateFrequency(MusicalNote.EqualNote(NoteName.A, 4).Pitch, A4HZ, note);
        }

        #endregion
    }
}
