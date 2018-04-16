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

        internal List<LayoutComponent> _Components;
        private Temperament _FretsTemperament;
        private FretInterpolationMethod _FretInterpolation;
        private bool _LeftHanded;
        private int _NumberOfStrings;
        private SIString[] _Strings;
        private bool _CompensateFretPositions;
        
        private FingerboardMargin _Margins;
        private StringSpacingType _StringSpacingMode;
        private StringSpacingSimple _SimpleStringSpacing;
        private StringSpacingManual _ManualStringSpacing;
        private ScaleLengthType _ScaleLengthMode;
        private ScaleLengthManager.SingleScale _SingleScaleMgr;
        private ScaleLengthManager.MultiScale _MultiScaleMgr;
        private ScaleLengthManager.Individual _ManualScaleMgr;
        private List<VisualElement> _VisualElements;
        private RectangleM _CachedBounds;
        private bool isLayoutDirty;
        private bool isLoading;

        #endregion

        #region Properties

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

        #region Scale Length Management

        public ScaleLengthType ScaleLengthMode
        {
            get { return _ScaleLengthMode; }
            set
            {
                if (value != _ScaleLengthMode)
                {
                    if (NumberOfStrings == 1)
                        return;
                    if (value == ScaleLengthType.Individual)
                        _ManualScaleMgr.InitializeIfNeeded();
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

        #region String Spacing Management

        public StringSpacingType StringSpacingMode
        {
            get { return _StringSpacingMode; }
            set
            {
                if (value != _StringSpacingMode)
                {
                    _StringSpacingMode = value;
                    NotifyLayoutChanged(this, "StringSpacingMode");
                }
            }
        }

        public StringSpacingManager StringSpacing
        {
            get
            {
                switch (StringSpacingMode)
                {
                    default:
                    case StringSpacingType.Simple:
                        return _SimpleStringSpacing;
                    case StringSpacingType.Manual:
                        return _ManualStringSpacing;
                }
            }
        }

        public StringSpacingSimple SimpleStringSpacing
        {
            get { return _SimpleStringSpacing; }
        }

        public StringSpacingManual ManualStringSpacing
        {
            get { return _ManualStringSpacing; }
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
                    NotifyLayoutChanged(this, "FretsTemperament");
                }
            }
        }

        public FretInterpolationMethod FretInterpolation
        {
            get { return _FretInterpolation; }
            set
            {
                if (value != _FretInterpolation)
                {
                    _FretInterpolation = value;
                    NotifyLayoutChanged(this, "FretInterpolation");
                }
            }
        }

        public bool CompensateFretPositions
        {
            get { return _CompensateFretPositions; }
            set
            {
                if (value != _CompensateFretPositions)
                {
                    if (value && !Strings.All(s => s.CanCalculateCompensation))
                        return;
                    _CompensateFretPositions = value;
                    if (value)
                        FretInterpolation = FretInterpolationMethod.NotchedSpline;
                    else
                        FretInterpolation = FretInterpolationMethod.Spline;
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

        #endregion


        #region Events

        public event EventHandler LayoutChanged;
        public event EventHandler LayoutUpdated;
        public event EventHandler NumberOfStringsChanged;

        #endregion

        public SILayout()
        {
            _Components = new List<LayoutComponent>();
            _Margins = new FingerboardMargin(this);
            _StringSpacingMode = StringSpacingType.Simple;
            _SimpleStringSpacing = new StringSpacingSimple(this);
            _ManualStringSpacing = new StringSpacingManual(this);
            _SingleScaleMgr = new ScaleLengthManager.SingleScale(this);
            _MultiScaleMgr = new ScaleLengthManager.MultiScale(this);
            _ManualScaleMgr = new ScaleLengthManager.Individual(this);
            _VisualElements = new List<VisualElement>();
            _ScaleLengthMode = ScaleLengthType.Single;
            _FretsTemperament = Temperament.Equal;
            _FretInterpolation = FretInterpolationMethod.Spline;
            _CachedBounds = RectangleM.Empty;
            LayoutName = string.Empty;
            ChangedProperties = new List<string>();
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
                if (oldStrings != null && i < oldStrings.Length)
                    _Strings[i] = oldStrings[i];
                else
                {
                    _Strings[i] = new SIString(this, i);
                    if (i >= 2 && !_Strings[i - 2].Gauge.IsEmpty && !_Strings[i - 1].Gauge.IsEmpty)
                    {
                        var gaugeDiff = _Strings[i - 1].Gauge - _Strings[i - 2].Gauge;
                        var estGauge = _Strings[i - 1].Gauge * 1.3;
                        _Strings[i].Gauge = Measure.Avg(_Strings[i - 1].Gauge + gaugeDiff, estGauge);
                    }
                    if (i >= 1 && !_Strings[i - 1].Gauge.IsEmpty)
                        _Strings[i].Gauge = _Strings[i - 1].Gauge * 1.3;
                }
            }

            foreach (var comp in _Components)
                (comp as ILayoutComponent).OnStringConfigurationChanged();

            if (CompensateFretPositions && !Strings.All(s => s.CanCalculateCompensation))
                CompensateFretPositions = false;

            NotifyLayoutChanged(this, "Strings");

            if (oldStrings != null)
                OnNumberOfStringsChanged();
        }

        protected void OnNumberOfStringsChanged()
        {
            var handler = NumberOfStringsChanged;
            if (handler != null)
                handler(this, EventArgs.Empty);
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
            if (!isLoading)
            {
                isLayoutDirty = true;
                if (!ChangedProperties.Contains(propname))
                    ChangedProperties.Add(propname);
                OnLayoutChanged();
            }
        }

        protected void OnLayoutChanged()
        {
            LayoutChanged?.Invoke(this, EventArgs.Empty);
        }

        public RectangleM GetLayoutBounds()
        {
            if (VisualElements.Count == 0)
                return RectangleM.Empty;

            if (!_CachedBounds.IsEmpty)
                return _CachedBounds;

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
            _CachedBounds = RectangleM.FromLTRB(minX, maxY, maxX, minY);
            return _CachedBounds;
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

            root.Add(new XComment("Temperaments: " + Enum.GetNames(typeof(Temperament)).Aggregate((i, j) => i + ", " + j)));
            root.Add(SerializeProperty("Temperament", FretsTemperament));
            
            root.Add(SerializeProperty("LeftHanded", LeftHanded));
            root.Add(SerializeProperty("FretCompensation", CompensateFretPositions));

            root.Add(new XComment("LengthFunctions: " + Enum.GetNames(typeof(LengthFunction)).Aggregate((i, j) => i + ", " + j)));
            root.Add(CurrentScaleLength.Serialize("ScaleLength"));
            root.Add(Margins.Serialize("FingerboardMargins"));

            var stringsElem = new XElement("Strings", new XAttribute("Count", NumberOfStrings));
            for (int i = 0; i < NumberOfStrings; i++)
            {
                //stringsElem.Add(SerializationHelper.GenericSerialize(Strings[i], "String"));
                stringsElem.Add(Strings[i].Serialize("String"));
            }
            root.Add(stringsElem);

            if(NumberOfStrings > 1)
            {
                root.Add(new XComment(string.Format("StringSpacingAlignment: {0}",
                    Enum.GetNames(typeof(StringSpacingAlignment)).Aggregate((i, j) => i + ", " + j)
                    )
                ));
                root.Add(StringSpacing.Serialize("StringSpacings"));
            }

            var doc = new XDocument(root);
            doc.Save(stream);
        }
        
        public static SILayout Load(string path)
        {
            using (var fs = File.Open(path, FileMode.Open, FileAccess.Read))
                return Load(fs);
        }

        public static SILayout Load(Stream stream)
        {
            var doc = XDocument.Load(stream);
            if (doc != null)
                return Load(doc);
            return null;
        }

        public static SILayout Load(XDocument document)
        {
            var root = document.Root;

            var layout = new SILayout() { isLoading = true };
            layout.NumberOfStrings = root.Element("Strings").GetIntAttribute("Count");
            layout.ScaleLengthMode = DeserializeProperty<ScaleLengthType>(root.Element("ScaleLength").Attribute("Type"));
            layout.CurrentScaleLength.Deserialize(root.Element("ScaleLength"));

            layout.Margins.Deserialize(root.Element("FingerboardMargins"));

            if (root.ContainsAttribute("Name"))
                layout.LayoutName = root.Attribute("Name").Value;

            if (root.ContainsElement("LeftHanded"))
                layout.LeftHanded = DeserializeProperty<bool>(root.Element("LeftHanded"));

            for (int i = 0; i < layout.NumberOfStrings; i++)
            {
                var stringElem = root.Element("Strings").Elements("String").First(s => s.Attribute("Index").Value == i.ToString());
                layout.Strings[i].Deserialize(stringElem);
            }

            if (root.ContainsElement("Temperament"))
                layout.FretsTemperament = DeserializeProperty<Temperament>(root.Element("Temperament"));

            if (root.ContainsElement("FretCompensation"))
                layout.CompensateFretPositions = DeserializeProperty<bool>(root.Element("FretCompensation"));

            if (root.ContainsElement("StringSpacings"))
            {
                layout.StringSpacingMode = DeserializeProperty<StringSpacingType>(root.Element("StringSpacings").Attribute("Mode"));
                layout.StringSpacing.Deserialize(root.Element("StringSpacings"));
            }
            
            layout.FillDefaultValues();
            layout.isLoading = false;
            layout.isLayoutDirty = true;

            return layout;
        }

        private void FillDefaultValues()
        {
            //set default values for other scale length managers
            if (ScaleLengthMode == ScaleLengthType.Single)
            {
                MultiScaleConfig.Treble = SingleScaleConfig.Length;
                MultiScaleConfig.Bass = SingleScaleConfig.Length + Measure.Inches(1);
            }
            else if (ScaleLengthMode == ScaleLengthType.Multiple)
                SingleScaleConfig.Length = MultiScaleConfig.Treble;
            else
            {
                MultiScaleConfig.Treble = ManualScaleConfig.Lengths.Min();
                MultiScaleConfig.Treble = Measure.Round(MultiScaleConfig.Treble, GetRoundAmount(MultiScaleConfig.Treble));

                MultiScaleConfig.Bass = ManualScaleConfig.Lengths.Max();
                MultiScaleConfig.Bass = Measure.Round(MultiScaleConfig.Bass, GetRoundAmount(MultiScaleConfig.Bass));

                if (MultiScaleConfig.Bass == MultiScaleConfig.Treble)
                    MultiScaleConfig.Bass = MultiScaleConfig.Bass + Measure.Inches(1);

                MultiScaleConfig.PerpendicularFretRatio = Strings.Average(s => s.MultiScaleRatio);

                SingleScaleConfig.Length = ManualScaleConfig.Lengths.Average();
                SingleScaleConfig.Length = Measure.Round(SingleScaleConfig.Length, GetRoundAmount(SingleScaleConfig.Length));
            }
            
            if(StringSpacingMode == StringSpacingType.Simple)
            {
                for (int i = 0; i < NumberOfStrings - 1; i++)
                {
                    ManualStringSpacing.SetSpacing(FingerboardEnd.Nut, i, SimpleStringSpacing.GetSpacing(i, FingerboardEnd.Nut));
                    ManualStringSpacing.SetSpacing(FingerboardEnd.Bridge, i, SimpleStringSpacing.GetSpacing(i, FingerboardEnd.Bridge));
                }
            }
            else
            {
                SimpleStringSpacing.StringSpreadAtNut = ManualStringSpacing.StringSpreadAtNut;
                SimpleStringSpacing.StringSpreadAtBridge = ManualStringSpacing.StringSpreadAtBridge;
            }
        }

        private double GetRoundAmount(Measure value)
        {
            if (value.Unit == UnitOfMeasure.Cm)
                return 0.01;
            else if (value.Unit == UnitOfMeasure.Mm)
                return 0.1;
            else if (value.Unit == UnitOfMeasure.In)
                return 1d / 16d;
            return 1d;
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
