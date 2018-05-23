using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Physics
{
    public struct MusicalNote
    {
        #region Fields

        private int _Octave;
        private NoteName _NoteName;
        private PitchValue _Pitch;
        private IntonationMethod _BaseIntonation;

        #endregion

        #region Properties

        public IntonationMethod BaseIntonation { get { return _BaseIntonation; } }

        public NoteName NoteName
        {
            get { return _NoteName; }
        }

        public int Octave
        {
            get { return _Octave; }
        }

        public PitchValue Pitch
        {
            get { return _Pitch; }
        }

        #endregion

        #region Static Ctors

        public static MusicalNote CreateNote(NoteName note, int octave, IntonationMethod intonation = IntonationMethod.EqualTempered)
        {
            return new MusicalNote
            {
                _NoteName = note,
                _Octave = octave,
                _BaseIntonation = intonation,
                _Pitch = NoteConverter.GetPitchFromNote(note, octave, intonation)
            };
        }

        public static MusicalNote FromPitch(PitchValue pitch)
        {
            return new MusicalNote()
            {
                _NoteName = (NoteName)Math.Round((pitch.Cents % 1200d) / 100d),//guess the note name from equal temperament
                _Octave = (int)Math.Floor(pitch.Cents / 1200d),
                _Pitch = pitch,
                _BaseIntonation = IntonationMethod.Computed
            };
        }

        public static MusicalNote FromCents(double cents)
        {
            return FromPitch(PitchValue.FromCents(cents));
        }

        public static MusicalNote JustNote(NoteName tone, int octave)
        {
            return CreateNote(tone, octave, IntonationMethod.Just);
        }

        public static MusicalNote EqualNote(NoteName tone, int octave)
        {
            return CreateNote(tone, octave, IntonationMethod.EqualTempered);
        }

        #endregion

        #region Functions

        public static int GetHalfStepsDifference(NoteName note1, int octave1, NoteName note2, int octave2)
        {
            return (int)note2 - (int)note1 + (octave2 - octave1) * 12;
        }

        public static int GetHalfStepsDifference(MusicalNote n1, MusicalNote n2)
        {
            return (int)n2.NoteName - (int)n1.NoteName + (n2.Octave - n1.Octave) * 12;
        }

        public MusicalNote AddSteps(int steps)
        {
            var baseSteps = (Octave * 12 + (int)NoteName) + steps;
            return CreateNote((NoteName)(baseSteps % 12), (int)Math.Floor(baseSteps / 12d), BaseIntonation);
        }

        public MusicalNote AddSteps(int steps, IntonationMethod newIntonation)
        {
            var baseSteps = (Octave * 12 + (int)NoteName) + steps;
            return CreateNote((NoteName)(baseSteps % 12), (int)Math.Floor(baseSteps / 12d), newIntonation);
        }

        public static NoteName ParseNoteName(string value)
        {
            NoteName note = NoteName.A;

            if (Enum.TryParse<NoteName>(value, out note))
                return note;

            const string NOTES = "ABCDEFG";

            if (value.Length == 1 && NOTES.Contains(value.ToUpper()))
                return (NoteName)Enum.Parse(typeof(NoteName), value.ToUpper());
            else if (NOTES.Contains(char.ToUpper(value[0])))
            {
                if (value.Contains("#"))
                {
                    int noteIdx = NOTES.IndexOf(char.ToUpper(value[0]));
                    return (NoteName)Enum.Parse(typeof(NoteName), NOTES[(noteIdx + 1) % NOTES.Length] + "b");
                }
                else if (value.ToUpper().IndexOf("B", 1) > 0)
                {
                    return (NoteName)Enum.Parse(typeof(NoteName), char.ToUpper(value[0]) + "b");
                }
            }

            throw new InvalidCastException("Invalid note name");
        }

        //public static MusicalNote Parse(string value)
        //{
        //    return null;
        //}

        #endregion

        public override string ToString()
        {
            return string.Format("{0}{1}", NoteName, Octave);
        }
    }
}
