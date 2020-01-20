using SiGen.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public StringTuning()
        {
            Note = MusicalNote.CreateNote(NoteName.A, 4, IntonationMethod.EqualTempered);
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

        public XElement Serialize(string elemName)
        {
            var elem = new XElement(elemName, 
                new XAttribute("Note", Note.NoteName),
                new XAttribute("Octave", Note.Octave),
                new XAttribute("Intonation", Note.BaseIntonation));
            if (PitchOffset.Cents != 0)
                elem.Add(new XAttribute("CentsOffset", PitchOffset.Cents));
            return elem;
        }

        public static StringTuning Deserialize(XElement elem)
        {
            var tuning = new StringTuning(
                MusicalNote.CreateNote(
                    //(NoteName)Enum.Parse(typeof(NoteName), elem.Attribute("Note").Value),
                    //elem.GetIntAttribute("Octave"),
                    //(IntonationMethod)Enum.Parse(typeof(IntonationMethod), elem.Attribute("Intonation").Value)
                    elem.ReadAttribute("Note", NoteName.C),
                    elem.ReadAttribute("Octave", 4),
                    elem.ReadAttribute("Intonation", IntonationMethod.EqualTempered)
                )
            );

            tuning.PitchOffset = PitchValue.FromCents(elem.ReadAttribute("CentsOffset", 0d));

            //if (elem.HasAttribute("CentsOffset", out XAttribute centsAttr))
            //    tuning.PitchOffset = PitchValue.FromCents(double.Parse(centsAttr.Value));

            return tuning;
        }
    }
}
