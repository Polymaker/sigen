using SiGen.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Data
{
    public class StringTuning
    {
        public MusicalNote Note { get; set; }
        public PitchValue PitchOffset { get; set; }

        public PitchValue FinalPitch
        {
            get { return Note.Pitch + PitchOffset; }
        }

        public StringTuning(MusicalNote note)
        {
            Note = note;
        }

        public StringTuning(MusicalNote note, PitchValue pitchOffset)
        {
            Note = note;
            PitchOffset = pitchOffset;
        }

        public StringTuning(NoteName noteName, int octave)
            : this(MusicalNote.CreateNote(noteName, octave, IntonationMethod.EqualTempered)) { }

        public StringTuning(NoteName noteName, int octave, PitchValue pitchOffset)
            : this(MusicalNote.CreateNote(noteName, octave, IntonationMethod.EqualTempered), pitchOffset) { }
    }
}
