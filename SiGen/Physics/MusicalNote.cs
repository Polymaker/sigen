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

        #endregion
    }
}
