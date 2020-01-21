﻿using SiGen.Common;
using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using SiGen.StringedInstruments.Layout.Visual;
using SiGen.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
		private bool _CompensateFretPositions;
		private StringSpacingType _StringSpacingMode;
		private ScaleLengthType _ScaleLengthMode;
        private InstrumentType _InstrumentType;
		private List<VisualElement> _VisualElements;
        private RectangleM _CachedBounds;
        private bool isLayoutDirty;

		/// <summary>
		/// Layout changes since last rebuilt
		/// </summary>
		private List<ILayoutChange> LayoutChanges;

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

        public InstrumentType InstrumentType
        {
            get => _InstrumentType;
            set => SetPropertyValue(ref _InstrumentType, value, nameof(InstrumentType));
        }

		/// <summary>
		/// Strings are ordered from Treble to Bass!!!
		/// </summary>
		public SIString[] Strings { get; private set; }

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
                        ManualScaleConfig.InitializeIfNeeded();

					SetPropertyValue(ref _ScaleLengthMode, value);
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
                        return SingleScaleConfig;
                    case ScaleLengthType.Multiple:
                        return MultiScaleConfig;
                    case ScaleLengthType.Individual:
                        return ManualScaleConfig;
                }
            }
        }

		public ScaleLengthManager.SingleScale SingleScaleConfig { get; }

		public ScaleLengthManager.MultiScale MultiScaleConfig { get; }

		public ScaleLengthManager.Individual ManualScaleConfig { get; }

		#endregion

		#region String Spacing Management

		public StringSpacingType StringSpacingMode
        {
            get { return _StringSpacingMode; }
            set
            {
				SetPropertyValue(ref _StringSpacingMode, value);
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
                        return SimpleStringSpacing;
                    case StringSpacingType.Manual:
                        return ManualStringSpacing;
                }
            }
        }

		public StringSpacingSimple SimpleStringSpacing { get; }

		public StringSpacingManual ManualStringSpacing { get; }

		#endregion

		public FingerboardMargin Margins { get; }

		public Temperament FretsTemperament
        {
            get { return _FretsTemperament; }
            set
            {
				StartBatchChanges();
                if (SetPropertyValue(ref _FretsTemperament, value))
					AdjustStringsTuning();
				FinishBatchChanges();
			}
        }

        public FretInterpolationMethod FretInterpolation
        {
            get { return _FretInterpolation; }
            set
            {
				SetPropertyValue(ref _FretInterpolation, value);
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

					StartBatchChanges();
                    if (value)
                        FretInterpolation = FretInterpolationMethod.NotchedSpline;
                    else
                        FretInterpolation = FretInterpolationMethod.Spline;

					NotifyLayoutChanged(new PropertyChange(null, "CompensateFretPositions", !value, value));

					FinishBatchChanges();
                }
            }
        }

        public bool LeftHanded
        {
            get { return _LeftHanded; }
            set
            {
				SetPropertyValue(ref _LeftHanded, value);
            }
        }

        public List<VisualElement> VisualElements { get { return _VisualElements; } }

		public bool IsLoading { get; private set; }

		internal bool IsAssigningProperties { get; private set; }

		#endregion

		#region Events
		
		public event EventHandler<LayoutChangedEventArgs> LayoutChanged;

		public event EventHandler LayoutUpdated;
        public event EventHandler NumberOfStringsChanged;

        #endregion

        public SILayout()
        {
            _Components = new List<LayoutComponent>();
            Margins = new FingerboardMargin(this);
            _StringSpacingMode = StringSpacingType.Simple;
            SimpleStringSpacing = new StringSpacingSimple(this);
            ManualStringSpacing = new StringSpacingManual(this);
            SingleScaleConfig = new ScaleLengthManager.SingleScale(this);
            MultiScaleConfig = new ScaleLengthManager.MultiScale(this);
            ManualScaleConfig = new ScaleLengthManager.Individual(this);
            _VisualElements = new List<VisualElement>();
            _ScaleLengthMode = ScaleLengthType.Single;
            _FretsTemperament = Temperament.Equal;
            _FretInterpolation = FretInterpolationMethod.Spline;
            _CachedBounds = RectangleM.Empty;
            LayoutName = string.Empty;
			LayoutChanges = new List<ILayoutChange>();
            _InstrumentType = InstrumentType.Guitar;
		}

        private void InitializeStrings(int oldValue, int newValue)
        {
            if (newValue < 0)
                return;

			StartBatchChanges();

            if (newValue == 1)
				SetPropertyValue(ref _ScaleLengthMode, ScaleLengthType.Single, nameof(ScaleLengthMode));

			NotifyLayoutChanged(new PropertyChange(null, nameof(NumberOfStrings), _NumberOfStrings, newValue));
			//NotifyLayoutChanged(new PropertyChange(null, nameof(_NumberOfStrings), null, _NumberOfStrings, newValue, true));

			_NumberOfStrings = newValue;

			var oldStrings = Strings;
            Strings = new SIString[NumberOfStrings];

            for(int i = 0; i < _NumberOfStrings; i++)
            {
                if (oldStrings != null && i < oldStrings.Length)
                    Strings[i] = oldStrings[i];
                else
                {
                    Strings[i] = new SIString(this, i);
                    if (i >= 2 && !Strings[i - 2].Gauge.IsEmpty && !Strings[i - 1].Gauge.IsEmpty)
                    {
                        var gaugeDiff = Strings[i - 1].Gauge - Strings[i - 2].Gauge;
                        var estGauge = Strings[i - 1].Gauge * 1.3;
                        Strings[i].Gauge = Measure.Avg(Strings[i - 1].Gauge + gaugeDiff, estGauge);
                    }
                    if (i >= 1 && !Strings[i - 1].Gauge.IsEmpty)
                        Strings[i].Gauge = Strings[i - 1].Gauge * 1.3;
                }
            }

            foreach (var comp in _Components)
                (comp as ILayoutComponent).OnStringConfigurationChanged();

            if (CompensateFretPositions && !Strings.All(s => s.CanCalculateCompensation))
				SetPropertyValue(ref _CompensateFretPositions, false, nameof(CompensateFretPositions));

			//if (CurrentBatchChanges != null)
			//{
			//	CurrentBatchChanges.Insert(1, new PropertyChange(null, nameof(Strings), oldStrings, Strings));
			//	CurrentBatchChanges.RemoveAll(c => c.Component is SIString);
			//}

			FinishBatchChanges();

			if (oldStrings != null)
				OnNumberOfStringsChanged();
		}

        /*
        public void AddString(FingerboardSide side)
        {
            StartBatchChanges();

            _NumberOfStrings++;

            var oldStrings = Strings;
            Strings = new SIString[NumberOfStrings];

            if (side == FingerboardSide.Treble)
            {
                Strings[0] = new SIString(this, 0);

            }

            for (int i = 0; i < oldStrings.Length; i++)
            {

            }

            for (int i = 0; i < _NumberOfStrings; i++)
            {

                if (oldStrings != null && i < oldStrings.Length)
                    Strings[i] = oldStrings[i];
                else
                {
                    Strings[i] = new SIString(this, i);
                    if (i >= 2 && !Strings[i - 2].Gauge.IsEmpty && !Strings[i - 1].Gauge.IsEmpty)
                    {
                        var gaugeDiff = Strings[i - 1].Gauge - Strings[i - 2].Gauge;
                        var estGauge = Strings[i - 1].Gauge * 1.3;
                        Strings[i].Gauge = Measure.Avg(Strings[i - 1].Gauge + gaugeDiff, estGauge);
                    }
                    if (i >= 1 && !Strings[i - 1].Gauge.IsEmpty)
                        Strings[i].Gauge = Strings[i - 1].Gauge * 1.3;
                }
            }

            OnNumberOfStringsChanged();
        }
        */
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
                    str.Tuning = GetTuningForNote(str.Tuning.Note, FretsTemperament);
            }
        }

        public static StringTuning GetTuningForNote(MusicalNote note, Temperament temperament)
        {
            MusicalNote newNote;
            switch (temperament)
            {
                default:
                case Temperament.Equal:
                    newNote = MusicalNote.EqualNote(note.NoteName, note.Octave);
                    break;
                case Temperament.Just:
                    newNote = MusicalNote.JustNote(note.NoteName, note.Octave);
                    break;
            }
            var offset = new PitchValue();
            if (temperament == Temperament.ThidellFormula)
            {
                offset = PitchValue.FromCents(NoteConverter.ThidellFormulaChromaticOffsets[(int)newNote.NoteName]);
                //if (newNote.NoteName == NoteName.E && newNote.Octave == 2)
                //    offset = PitchValue.FromCents(-2);
                //else if (newNote.NoteName == NoteName.D && newNote.Octave == 3)
                //    offset = PitchValue.FromCents(2);
                //else if (newNote.NoteName == NoteName.G && newNote.Octave == 3)
                //    offset = PitchValue.FromCents(4);
                //else if (newNote.NoteName == NoteName.B && newNote.Octave == 3)
                //    offset = PitchValue.FromCents(-1);
                if (newNote.NoteName == NoteName.E && newNote.Octave == 4)
                    offset = PitchValue.FromCents(-1);
            }
            else if (temperament == Temperament.DieWohltemperirte)
            {
                offset = PitchValue.FromCents(NoteConverter.DieWohltemperirteChromaticOffsets[(int)newNote.NoteName]);
                //if (newNote.NoteName == NoteName.E && newNote.Octave == 2)
                //    offset = PitchValue.FromCents(-2);
                //else if (newNote.NoteName == NoteName.D && newNote.Octave == 3)
                //    offset = PitchValue.FromCents(2);
                //else if (newNote.NoteName == NoteName.G && newNote.Octave == 3)
                //    offset = PitchValue.FromCents(3.9);
                //else if (newNote.NoteName == NoteName.E && newNote.Octave == 4)
                //    offset = PitchValue.FromCents(-2);
            }
            return new StringTuning(newNote, offset);
        }

		#region Layout Change Tracking & Notification

		private List<PropertyChange> CurrentBatchChanges;
        private string CurrentBatchName;
		private int NestedBatches;

		internal void NotifyLayoutChanged(ILayoutChange change)
		{
			if (!(IsLoading || IsAssigningProperties))
			{
				if(change is PropertyChange propChange && CurrentBatchChanges != null)
					CurrentBatchChanges.Add(propChange);
				else
					OnLayoutChanged(new LayoutChangedEventArgs(change));
			}
		}
		
		protected void OnLayoutChanged(LayoutChangedEventArgs args)
		{
			LayoutChanges.Add(args.Change);
			LayoutChanged?.Invoke(this, args);
		}

		internal void StartBatchChanges(string name = null)
		{
			if (!(IsLoading || IsAssigningProperties))
			{
				if(CurrentBatchChanges == null)
                {
                    CurrentBatchChanges = new List<PropertyChange>();
                    CurrentBatchName = name ?? string.Empty;
                }
				NestedBatches++;
			}
		}

		internal void FinishBatchChanges()
		{
			if(!(IsLoading || IsAssigningProperties) && CurrentBatchChanges != null)
			{
				if(--NestedBatches == 0)
				{
					if (CurrentBatchChanges.Count == 1)
						OnLayoutChanged(new LayoutChangedEventArgs(CurrentBatchChanges[0]));
					else if (CurrentBatchChanges.Count > 1)
                        OnLayoutChanged(new LayoutChangedEventArgs(new BatchChange(CurrentBatchName, CurrentBatchChanges)));
					CurrentBatchChanges = null;
				}
			}
		}

		protected bool SetPropertyValue<T>(ref T property, T value, [CallerMemberName] string propertyName = null)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				var propChange = new PropertyChange(null, propertyName, property, value);
				property = value;
				NotifyLayoutChanged(propChange);
				return true;
			}
			return false;
		}

		public void UndoChange(ILayoutChange change)
		{
			ApplyLayoutChange(change, false);
		}

		public void RedoChange(ILayoutChange change)
		{
			ApplyLayoutChange(change, true);
		}

		private void ApplyLayoutChange(ILayoutChange change, bool setNewValue)
		{
			//IsAssigningProperties = true;
			try
			{
				var allChanges = change.GetChanges().ToList();
				if (!setNewValue)
					allChanges.Reverse();

				foreach (var changedProp in allChanges)
				{
					if (changedProp.IsField)
					{
						FieldInfo fi = null;
						if (changedProp.Component == null)
							fi = GetType().GetField(changedProp.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
						else
							fi = changedProp.Component.GetType().GetField(changedProp.Property, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

						if (changedProp.Index.HasValue)
						{
							var arrayValue = (IList)fi.GetValue((object)changedProp.Component ?? this);
							arrayValue[changedProp.Index.Value] = setNewValue ? changedProp.NewValue : changedProp.OldValue;
						}
						else
							fi.SetValue((object)changedProp.Component ?? this, setNewValue ? changedProp.NewValue : changedProp.OldValue);

                        NotifyLayoutChanged(new PropertyChange(changedProp.Component, changedProp.Property,
                            setNewValue ? changedProp.OldValue : changedProp.NewValue,
                            setNewValue ? changedProp.NewValue : changedProp.OldValue, 
                            true));
					}
					else
					{
						PropertyInfo pi = null;
						object ownerObject = (object)changedProp.Component ?? this;

						if (changedProp.Property.Contains("."))
						{
							string[] propNames = changedProp.Property.Split('.');
							pi = ownerObject.GetType().GetProperty(propNames[0]);
							ownerObject = pi.GetValue(ownerObject);
							pi = ownerObject.GetType().GetProperty(propNames[1]);
						}
						else
							pi = ownerObject.GetType().GetProperty(changedProp.Property);

						pi.SetValue(ownerObject, setNewValue ? changedProp.NewValue : changedProp.OldValue);

                        if (changedProp.Property.Contains("."))
                            NotifyLayoutChanged(new PropertyChange(changedProp.Component, changedProp.Property, changedProp.Index, changedProp.NewValue, changedProp.OldValue, changedProp.IsField));
					}

				}
			}
			finally
			{
				//IsAssigningProperties = false;
			}
		}

		#endregion

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
                root.Add(new XComment(string.Format("StringSpacingMethod: {0}",
                    Enum.GetNames(typeof(StringSpacingMethod)).Aggregate((i, j) => i + ", " + j)
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

            var layout = new SILayout() { IsLoading = true };
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
            layout.IsLoading = false;
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

        public static SILayout GenerateDefaultLayout()
        {
            var stream = typeof(SILayout).Assembly.GetManifestResourceStream("SiGen.Resources.DefaultLayout.sil");
            return Load(stream);
            //var layout = new SILayout
            //{
            //    LayoutName = "6 Strings Fingerboard Layout",
            //    IsLoading = true
            //};

            //layout.InitializeStrings(0, 6);
            //layout.FretsTemperament = Temperament.Equal;
            //layout.StringSpacingMode = StringSpacingType.Simple;
            //layout.ScaleLengthMode = ScaleLengthType.Single;
            //layout.SingleScaleConfig.Length = Measure.Inches(25.5);
            //layout.SimpleStringSpacing.StringSpacingAtNut = Measure.Mm(7.3);
            //layout.SimpleStringSpacing.StringSpreadAtBridge = Measure.Mm(10.5);
            //layout.IsLoading = false;
            //return layout;
        }
    }
}
