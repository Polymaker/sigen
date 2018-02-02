using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout.Visual;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public partial class SILayout// : XSerializable
    {
        #region Fields

        internal List<LayoutComponent> _Component;
        private Temperament _FretsTemperament;
        private bool _LeftHanded;
        private int _NumberOfStrings;
        private SIString[] _Strings;
        private bool _CompensateFretPositions;
        private StringSpacingManager _StringSpacing;
        private ScaleLengthType _ScaleLengthMode;
        private FingerboardMargin _Margins;
        private ScaleLengthManager.SingleScale _SingleScaleMgr;
        private ScaleLengthManager.MultiScale _MultiScaleMgr;
        private ScaleLengthManager.Individual _ManualScaleMgr;
        private List<VisualElement> _VisualElements;
        private bool isLayoutDirty;
        private bool isLoading;

        #endregion

        public int NumberOfStrings
        {
            get { return _NumberOfStrings; }
            set
            {
                if (value != _NumberOfStrings)
                    InitializeStrings(_NumberOfStrings, value);
            }
        }

        /// <summary>
        /// Strings are ordered from Treble to Bass!!!
        /// </summary>
        public SIString[] Strings { get { return _Strings; } }

        public string LayoutName { get; set; }

        #region Scale length management

        public ScaleLengthType ScaleLengthMode
        {
            get { return _ScaleLengthMode; }
            set
            {
                if (value != _ScaleLengthMode)
                {
                    if (NumberOfStrings == 1)
                        return;
                    _ScaleLengthMode = value;
                    NotifyLayoutChanged(this, "ScaleLengthMode");
                }
            }
        }

        public ScaleLengthManager CurrentScaleLength
        {
            get
            {
                switch (ScaleLengthMode)
                {
                    default:
                    case ScaleLengthType.Single:
                        return _SingleScaleMgr;
                    case ScaleLengthType.Multiple:
                        return _MultiScaleMgr;
                    case ScaleLengthType.Individual:
                        return _ManualScaleMgr;
                }
            }
        }

        public ScaleLengthManager.SingleScale SingleScaleConfig
        {
            get { return _SingleScaleMgr; }
        }

        public ScaleLengthManager.MultiScale MultiScaleConfig
        {
            get { return _MultiScaleMgr; }
        }

        public ScaleLengthManager.Individual ManualScaleConfig
        {
            get { return _ManualScaleMgr; }
        }

        #endregion

        public FingerboardMargin Margins { get { return _Margins; } }

        public Temperament FretsTemperament
        {
            get { return _FretsTemperament; }
            set
            {
                if (value != _FretsTemperament)
                {
                    _FretsTemperament = value;
                    AdjustStringsTuning();
                    //AdjustStringsTuning();
                    NotifyLayoutChanged(this, "FretsTemperament");
                }
            }
        }

        public StringSpacingManager StringSpacing
        {
            get { return _StringSpacing; }
            set
            {
                if (value != _StringSpacing)
                {
                    _StringSpacing = value;
                    NotifyLayoutChanged(this, "StringSpacing");
                }
            }
        }

        public bool CompensateFretPositions
        {
            get { return _CompensateFretPositions; }
            set
            {
                if(value != _CompensateFretPositions)
                {
                    if (value && !Strings.All(s => s.CanCalculateCompensation))
                        return;
                    _CompensateFretPositions = value;
                    NotifyLayoutChanged(this, "CompensateFretPositions");
                }
            }
        }

        public bool LeftHanded
        {
            get { return _LeftHanded; }
            set
            {
                if (value != _LeftHanded)
                {
                    _LeftHanded = value;
                    NotifyLayoutChanged(this, "LeftHanded");
                }
            }
        }

        public List<VisualElement> VisualElements { get { return _VisualElements; } }

        #region Events

        //public event EventHandler NumberOfStringsChanged;
        public event EventHandler LayoutChanged;

        #endregion

        public SILayout()
        {
            _Component = new List<LayoutComponent>();
            _Margins = new FingerboardMargin(this);
            _StringSpacing = new StringSpacingSimple(this);
            _SingleScaleMgr = new ScaleLengthManager.SingleScale(this);
            _MultiScaleMgr = new ScaleLengthManager.MultiScale(this);
            _ManualScaleMgr = new ScaleLengthManager.Individual(this);
            _VisualElements = new List<VisualElement>();
            _ScaleLengthMode = ScaleLengthType.Single;
            _FretsTemperament = Temperament.Equal;
            LayoutName = string.Empty;
        }

        private void InitializeStrings(int oldValue, int newValue)
        {
            if (newValue < 0)
                return;
            if (newValue == 1)
                _ScaleLengthMode = ScaleLengthType.Single;
            _NumberOfStrings = newValue;
            var oldStrings = _Strings;
            _Strings = new SIString[NumberOfStrings];
            for(int i = 0; i < _NumberOfStrings; i++)
            {
                if (oldStrings != null && i < oldStrings.Length - 1)
                    _Strings[i] = oldStrings[i];
                else
                    _Strings[i] = new SIString(this, i);
            }
            foreach (var comp in _Component)
                (comp as ILayoutComponent).OnStringConfigurationChanged();
            //if (NumberOfStringsChanged != null)
            //    NumberOfStringsChanged(this, EventArgs.Empty);
        }

        public void SetStringsTuning(params MusicalNote[] tunings)
        {
            if (tunings.Length != NumberOfStrings)
                throw new InvalidOperationException("Number of string mistmatch.");
            for (int i = 0; i < NumberOfStrings; i++)
                Strings[i].Tuning = new StringTuning(tunings[i]);
        }

        private void AdjustStringsTuning()
        {
            foreach (var str in Strings)
            {
                if (str.Tuning != null)
                {
                    MusicalNote newNote;
                    switch (FretsTemperament)
                    {
                        default:
                        case Temperament.Equal:
                            newNote = MusicalNote.EqualNote(str.Tuning.Note.NoteName, str.Tuning.Note.Octave);
                            break;
                        case Temperament.Just:
                            newNote = MusicalNote.JustNote(str.Tuning.Note.NoteName, str.Tuning.Note.Octave);
                            break;
                    }
                    var offset = new PitchValue();
                    if (FretsTemperament == Temperament.ThidellFormula)
                    {
                        if (newNote.NoteName == NoteName.E && newNote.Octave == 2)
                            offset = PitchValue.FromCents(-2);
                        else if (newNote.NoteName == NoteName.D && newNote.Octave == 3)
                            offset = PitchValue.FromCents(2);
                        else if (newNote.NoteName == NoteName.G && newNote.Octave == 3)
                            offset = PitchValue.FromCents(4);
                        else if (newNote.NoteName == NoteName.B && newNote.Octave == 3)
                            offset = PitchValue.FromCents(-1);
                        else if (newNote.NoteName == NoteName.E && newNote.Octave == 4)
                            offset = PitchValue.FromCents(-1);
                    }
                    else if (FretsTemperament == Temperament.DieWohltemperirte)
                    {
                        if (newNote.NoteName == NoteName.E && newNote.Octave == 2)
                            offset = PitchValue.FromCents(-2);
                        else if (newNote.NoteName == NoteName.D && newNote.Octave == 3)
                            offset = PitchValue.FromCents(2);
                        else if (newNote.NoteName == NoteName.G && newNote.Octave == 3)
                            offset = PitchValue.FromCents(3.9);
                        else if (newNote.NoteName == NoteName.E && newNote.Octave == 4)
                            offset = PitchValue.FromCents(-2);
                    }
                    str.Tuning = new StringTuning(newNote, offset);
                }
            }
        }

        internal void NotifyLayoutChanged(object sender, string propname)
        {
            if(!isLoading)
                isLayoutDirty = true;
        }

        public bool VerifyFretboardHasStraightFrets()
        {
            if (FretsTemperament != Temperament.Equal || _CompensateFretPositions)
                return false;

            if (!Strings.AllEqual(s => s.RelativeScaleLengthOffset))
                return false;
            if (Strings.Length > 2 && !Strings.AllEqual(s => s.ScaleLength))
            {
                var diff = Measure.Abs(Strings[0].ScaleLength - Strings[1].ScaleLength);
                for (int i = 1; i < NumberOfStrings - 1; i++)
                {
                    var scaleDiff = Measure.Abs(Strings[i].ScaleLength - Strings[i + 1].ScaleLength);
                    if (!Measure.EqualOrClose(diff, scaleDiff, Measure.Mm(0.0001)))
                        return false;
                }
            }
            return true;
        }

        public RectangleM GetBounds()
        {
            if (VisualElements.Count == 0)
                return RectangleM.Empty;

            Measure minX = Measure.Zero;
            Measure maxX = Measure.Zero;
            Measure minY = Measure.Zero;
            Measure maxY = Measure.Zero;

            foreach(var elem in VisualElements)
            {
                if (elem.Bounds.IsEmpty)
                    continue;
                if (elem.Bounds.Left < minX)
                    minX = elem.Bounds.Left;
                if (elem.Bounds.Bottom < minY)
                    minY = elem.Bounds.Bottom;
                if (elem.Bounds.Right > maxX)
                    maxX = elem.Bounds.Right;
                if (elem.Bounds.Top > maxY)
                    maxY = elem.Bounds.Top;
            }

            return RectangleM.FromLTRB(minX, maxY, maxX, minY);
        }

        #region XML serialization

        public void Save(string path)
        {
            using (var fs = File.Open(path, FileMode.Create))
                Save(fs);
        }
        
        public void Save(Stream stream)
        {
            var root = new XElement("Layout");

            //*** BASIC PARAMETERS ***
            if (!string.IsNullOrEmpty(LayoutName))
                root.Add(new XAttribute("Name", LayoutName));

            root.Add(SerializeProperty("Temperament", FretsTemperament));
            root.Add(SerializeProperty("LeftHanded", LeftHanded));
            root.Add(SerializeProperty("FretCompensation", CompensateFretPositions));
            root.Add(CurrentScaleLength.Serialize("ScaleLength"));
            root.Add(Utilities.SerializationHelper.GenericSerialize(Margins, "FingerboardMargins"));

            var stringsElem = new XElement("Strings", new XAttribute("Count", NumberOfStrings));
            for (int i = 0; i < NumberOfStrings; i++)
            {
                stringsElem.Add(Utilities.SerializationHelper.GenericSerialize(Strings[i], "String"));
                //stringsElem.Add(Strings[i].Serialize(ScaleLengthMode == ScaleLengthType.Individual));
            }
            root.Add(stringsElem);
            if(NumberOfStrings > 1)
                root.Add(StringSpacing.Serialize("StringSpacings"));

            var doc = new XDocument(root);
            doc.Save(stream);
        }
        
        public static SILayout Load(string path)
        {
            using (var fs = File.Open(path, FileMode.Open))
                return Load(fs);
        }

        public static SILayout Load(Stream stream)
        {
            var doc = XDocument.Load(stream);
            var root = doc.Root;

            var layout = new SILayout() { isLoading = true };
            layout.NumberOfStrings = root.Element("Strings").GetIntAttribute("Count");
            layout.ScaleLengthMode = DeserializeProperty<ScaleLengthType>(root.Element("ScaleLength").Attribute("Type"));
            layout.CurrentScaleLength.Deserialize(root.Element("ScaleLength"));
            SerializationHelper.GenericDeserialize(layout.Margins, root.Element("FingerboardMargins"));

            if (root.ContainsElement("Temperament"))
                layout.FretsTemperament = DeserializeProperty<Temperament>(root.Element("Temperament"));

            if (root.ContainsElement("LeftHanded"))
                layout.LeftHanded = DeserializeProperty<bool>(root.Element("LeftHanded"));

            for(int i = 0;i < layout.NumberOfStrings; i++)
            {
                var stringElem = root.Element("Strings").Elements("String").First(s => s.Attribute("Index").Value == i.ToString());
                SerializationHelper.GenericDeserialize(layout.Strings[i], stringElem);
                //layout.Strings[i].Deserialize(stringElem);
            }

            if (root.ContainsElement("FretCompensation"))
                layout.CompensateFretPositions = DeserializeProperty<bool>(root.Element("FretCompensation"));

            layout.isLoading = false;
            layout.isLayoutDirty = true;

            return layout;
        }

        private static XElement SerializeProperty(string name, object value)
        {
            return new XElement(name, new XAttribute("Value", value));
        }

        private static T DeserializeProperty<T>(XElement elem)
        {
            return DeserializeProperty<T>(elem.Attribute("Value"));
        }

        private static T DeserializeProperty<T>(XAttribute attr)
        {
            var strValue = attr.Value;
            if (typeof(T) == typeof(string))
                return (T)(object)strValue;
            if (typeof(T) == typeof(bool))
                return (T)(object)bool.Parse(strValue);
            if (typeof(T).IsEnum)
                return (T)Enum.Parse(typeof(T), strValue);

            return default(T);
        }

        #endregion

    }
}
