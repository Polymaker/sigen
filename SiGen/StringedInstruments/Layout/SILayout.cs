using SiGen.Common;
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
        public const int CURRENT_LAYOUT_VERSION = 2;

        #region Fields

        internal List<LayoutComponent> _Components;
        private Temperament _FretsTemperament;
        private FretInterpolationMethod _FretInterpolation;
        private bool _LeftHanded;
        private int _NumberOfStrings;
		private bool _CompensateFretPositions;
		private StringSpacingType _StringSpacingMode;
		private ScaleLengthType _ScaleLengthMode;
        //private InstrumentType _InstrumentType;
        private RectangleM _CachedBounds;
        private bool isLayoutDirty;
        private LayoutItemCollection<SIString> _Strings;

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
                {
                    //InitializeStrings(_NumberOfStrings, value);
                    ChangeNumberOfString(value);
                }
            }
        }

        //TODO: To implement to pave the way for config wizard (e.g. tuning, gauge, etc)
        //public InstrumentType InstrumentType
        //{
        //    get => _InstrumentType;
        //    set => SetPropertyValue(ref _InstrumentType, value, nameof(InstrumentType));
        //}

        /// <summary>
        /// Strings are ordered from Treble to Bass!!!
        /// </summary>
        public IList<SIString> Strings => _Strings.AsReadOnly();

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

                    if (value == ScaleLengthType.Multiple)
                        MultiScaleConfig.InitializeIfNeeded();

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
                    case ScaleLengthType.Dual:
                        return DualScaleConfig;
                    case ScaleLengthType.Multiple:
                        return MultiScaleConfig;
                }
            }
        }

		public SingleScaleManager SingleScaleConfig { get; }

		public DualScaleManager DualScaleConfig { get; }

		public MultipleScaleManager MultiScaleConfig { get; }

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
				StartBatchChanges(nameof(FretsTemperament));
                if (SetPropertyValue(ref _FretsTemperament, value) && !IsLoading)
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

					StartBatchChanges(nameof(CompensateFretPositions));
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

        public int LayoutVersion { get; set; }

        public List<VisualElement> VisualElements { get; }

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
            _Strings = new LayoutItemCollection<SIString>(this);
            _Strings.CollectionChanged += Strings_CollectionChanged;

            Margins = new FingerboardMargin(this);

            _StringSpacingMode = StringSpacingType.Simple;
            SimpleStringSpacing = new StringSpacingSimple(this);
            ManualStringSpacing = new StringSpacingManual(this);

            SingleScaleConfig = new SingleScaleManager(this);
            DualScaleConfig = new DualScaleManager(this);
            MultiScaleConfig = new MultipleScaleManager(this);

            VisualElements = new List<VisualElement>();

            _ScaleLengthMode = ScaleLengthType.Single;
            _FretsTemperament = Temperament.Equal;
            _FretInterpolation = FretInterpolationMethod.Spline;
            _CachedBounds = RectangleM.Empty;
            LayoutName = string.Empty;

			LayoutChanges = new List<ILayoutChange>();

            LayoutVersion = CURRENT_LAYOUT_VERSION;
            //_InstrumentType = InstrumentType.Guitar;
        }

        #region Strings Management

        private bool PreventStringChange;

        private void Strings_CollectionChanged(object sender, CollectionChange e)
        {
            if (!IsLoading && !PreventStringChange)
            {
                //re-index strings
                for (int i = 0; i < _Strings.Count; i++)
                    _Strings[i].Index = i;
                _NumberOfStrings = _Strings.Count;
                RebuildComponentStringData();
                CheckCanCompensateFretPositions();
                OnNumberOfStringsChanged();
            }
        }

        public SIString GetString(int index, FingerboardSide side)
        {
            if (index < 0 || index >= _Strings.Count)
                return null;

            if (side == FingerboardSide.Treble)
                return _Strings[index];
            else
                return _Strings[_Strings.Count - index - 1];
        }

        public void AddString(FingerboardSide side, int numberToAdd = 1)
        {
            if (numberToAdd <= 0)
                return;

            PreventStringChange = true;

            StartBatchChanges("AddStrings");
            NotifyStringsChanging();

            for (int i = 0; i < numberToAdd; i++)
                AddStringCore(side);

            _NumberOfStrings = _Strings.Count;

            //re-index strings
            for (int i = 0; i < _Strings.Count; i++)
                _Strings[i].Index = i;

            RebuildComponentStringData();

            CheckCanCompensateFretPositions();

            FinishBatchChanges();

            OnNumberOfStringsChanged();

            PreventStringChange = false;
        }

        private void AddStringCore(FingerboardSide side)
        {
            var newString = new SIString()
            {
                Index = side == FingerboardSide.Treble ? 0 : _Strings.Count
            };

            if (side == FingerboardSide.Treble)
                _Strings.Insert(0, newString);
            else
                _Strings.Add(newString);

            IsAssigningProperties = true;
            StringHelper.EstimateStringGauge(newString);
            StringHelper.EstimateStringAction(newString);
            IsAssigningProperties = false;
        }

        public void RemoveString(FingerboardSide side, int numberToRemove = 1)
        {
            if (numberToRemove <= 0)
                return;

            PreventStringChange = true;

            StartBatchChanges("RemoveStrings");
            NotifyStringsChanging();

            numberToRemove = Math.Min(numberToRemove, _Strings.Count - 1);

            if (side == FingerboardSide.Treble)
            {
                var stringsToRemove = _Strings.Take(numberToRemove).ToList();
                _Strings.Remove(stringsToRemove);
            }
            else
            {
                var stringsToRemove = _Strings.Reverse().Take(numberToRemove).ToList();
                _Strings.Remove(stringsToRemove);
            }

            _NumberOfStrings = _Strings.Count;

            RebuildComponentStringData();

            CheckCanCompensateFretPositions();

            FinishBatchChanges();

            OnNumberOfStringsChanged();

            PreventStringChange = false;
        }

        private void NotifyStringsChanging()
        {
            foreach (var comp in _Components)
                (comp as ILayoutComponent).BeforeChangingStrings();
        }

        private void RebuildComponentStringData()
        {
            foreach (var comp in _Components)
                (comp as ILayoutComponent).OnStringsChanged();
        }

        public void ChangeNumberOfString(int numberOfStrings, FingerboardSide sideToChange = FingerboardSide.Bass)
        {
            if (numberOfStrings > _NumberOfStrings)
                AddString(sideToChange, numberOfStrings - _NumberOfStrings);
            else if(numberOfStrings < _NumberOfStrings)
                RemoveString(sideToChange, _NumberOfStrings - numberOfStrings);
        }

        private void CheckCanCompensateFretPositions()
        {
            if (CompensateFretPositions && !_Strings.All(s => s.CanCalculateCompensation))
                SetPropertyValue(ref _CompensateFretPositions, false, nameof(CompensateFretPositions));
        }

        protected void OnNumberOfStringsChanged()
        {
            NumberOfStringsChanged?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        public void AdjustStringsTuning()
        {
            StartBatchChanges(nameof(AdjustStringsTuning));
            foreach (var str in Strings)
            {
                if (str.Tuning != null)
                    str.Tuning = GetTuningForNote(str.Tuning.Note, FretsTemperament);
            }
            FinishBatchChanges();
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

		private List<ILayoutChange> CurrentBatchChanges;
        private string CurrentBatchName;
		private int NestedBatches;

		internal void NotifyLayoutChanged(ILayoutChange change)
		{
			if (!(IsLoading || IsAssigningProperties))
			{
				if(CurrentBatchChanges != null)
					CurrentBatchChanges.Add(change);
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
                    CurrentBatchChanges = new List<ILayoutChange>();
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
            if (change is PropertyChange propertyChange)
                ApplyPropertyChange(propertyChange, setNewValue);
            else if (change is CollectionChange collectionChange)
                ApplyCollectionChange(collectionChange, setNewValue);
            else if (change is BatchChange batchChange)
            {
                var allChanges = batchChange.LayoutChanges.ToList();
                if (!setNewValue)
                    allChanges.Reverse();

                foreach (var changeInfo in allChanges)
                    ApplyLayoutChange(changeInfo, setNewValue);
            }
        }

        //TODO: move code into each Change type class and clean-up the structure
        private void ApplyPropertyChange(PropertyChange propChange, bool setNewValue)
        {
            var ownerType = propChange.Component?.GetType() ?? GetType();
            var ownerObject = (object)propChange.Component ?? this;
            var valueToSet = setNewValue ? propChange.NewValue : propChange.OldValue;

            if (propChange.IsField)
            {
                var fi = ownerType.GetField(propChange.Property, 
                    BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (propChange.Index.HasValue)
                {
                    var arrayValue = (IList)fi.GetValue(ownerObject);
                    arrayValue[propChange.Index.Value] = valueToSet;
                }
                else
                    fi.SetValue(ownerObject, valueToSet);

                if (ownerObject is LayoutComponent component)
                {
                    component.OnSetFieldValue(propChange.Name, fi.GetValue(ownerObject), propChange.Index, valueToSet);
                }

                NotifyLayoutChanged(new PropertyChange(propChange.Component, propChange.Property,
                            setNewValue ? propChange.OldValue : propChange.NewValue,
                            setNewValue ? propChange.NewValue : propChange.OldValue,
                            true));
            }
            else
            {
                PropertyInfo pi = null;
                bool isSubProperty = false;

                if (propChange.Property.Contains("."))
                {
                    string[] propNames = propChange.Property.Split('.');
                    pi = ownerType.GetProperty(propNames[0]);
                    ownerObject = pi.GetValue(ownerObject);
                    ownerType = ownerObject.GetType();
                    pi = ownerType.GetProperty(propNames[1]);
                    isSubProperty = true;
                }
                else
                    pi = ownerType.GetProperty(propChange.Property);

                pi.SetValue(ownerObject, valueToSet);

                if (isSubProperty)
                {
                    NotifyLayoutChanged(new PropertyChange(propChange.Component,
                        propChange.Property, propChange.Index,
                        propChange.NewValue, propChange.OldValue,
                        propChange.IsField));
                }
            }
        }

        private void ApplyCollectionChange(CollectionChange changeData, bool setNewValue)
        {
            if (setNewValue) //redo
            {
                if (changeData.Action == System.ComponentModel.CollectionChangeAction.Remove)
                {
                    foreach (var item in changeData.ChangedItems)
                        changeData.Collection.Remove(item.Item);
                }
                else
                {
                    foreach (var item in changeData.ChangedItems)
                        changeData.Collection.Insert(item.NewIndex, item.Item);
                }
            }
            else //Undo
            {
                if (changeData.Action == System.ComponentModel.CollectionChangeAction.Remove)
                {
                    foreach (var item in changeData.ChangedItems.Reverse())
                        changeData.Collection.Insert(item.OldIndex, item.Item);
                }
                else
                {
                    foreach (var item in changeData.ChangedItems)
                        changeData.Collection.Remove(item.Item);
                }
            }

            if (changeData.Collection == _Strings)
            {
                _NumberOfStrings = _Strings.Count;
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

        public T GetElement<T>(Predicate<T> predicate) where T : VisualElement
        {
            return VisualElements.OfType<T>().FirstOrDefault(x => predicate(x));
        }

        public IEnumerable<T> GetElements<T>(Predicate<T> predicate) where T : VisualElement
        {
            return VisualElements.OfType<T>().Where(x => predicate(x));
        }

        public IEnumerable<T> GetElements<T>() where T : VisualElement
        {
            return VisualElements.OfType<T>();
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

            LayoutVersion = CURRENT_LAYOUT_VERSION;
            root.Add(new XAttribute("Version", LayoutVersion));

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

            layout.LayoutVersion = root.ReadAttribute("Version", 1);
            EnumHelper.CurrentLayoutVersion = layout.LayoutVersion;

            layout._NumberOfStrings = root.Element("Strings").GetIntAttribute("Count");
            for (int i = 0; i < layout.NumberOfStrings; i++)
                layout._Strings.Add(new SIString(layout, i));
            layout.RebuildComponentStringData();

            if (root.ContainsAttribute("Name"))
                layout.LayoutName = root.Attribute("Name").Value;

            if (root.HasElement("LeftHanded", out XElement lhElem))
                layout.LeftHanded = lhElem.ReadAttribute("Value", false);

            if (root.HasElement("ScaleLength", out XElement slElem))
            {
                layout.ScaleLengthMode = slElem.ReadAttribute("Type", ScaleLengthType.Single);
                layout.CurrentScaleLength.Deserialize(slElem);
            }

            layout.Margins.Deserialize(root.Element("FingerboardMargins"));

            for (int i = 0; i < layout.NumberOfStrings; i++)
            {
                var stringElem = root.Element("Strings").Elements("String").First(s => s.Attribute("Index").Value == i.ToString());
                layout._Strings[i].Deserialize(stringElem);
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
                DualScaleConfig.Treble = SingleScaleConfig.Length;
                DualScaleConfig.Bass = SingleScaleConfig.Length + Measure.Inches(1);
            }
            else if (ScaleLengthMode == ScaleLengthType.Dual)
                SingleScaleConfig.Length = DualScaleConfig.Treble;
            else
            {
                DualScaleConfig.Treble = MultiScaleConfig.Lengths.Min();
                DualScaleConfig.Treble = Measure.Round(DualScaleConfig.Treble, GetRoundAmount(DualScaleConfig.Treble));

                DualScaleConfig.Bass = MultiScaleConfig.Lengths.Max();
                DualScaleConfig.Bass = Measure.Round(DualScaleConfig.Bass, GetRoundAmount(DualScaleConfig.Bass));

                if (DualScaleConfig.Bass == DualScaleConfig.Treble)
                    DualScaleConfig.Bass = DualScaleConfig.Bass + Measure.Inches(1);

                DualScaleConfig.PerpendicularFretRatio = Strings.Average(s => s.MultiScaleRatio);

                SingleScaleConfig.Length = MultiScaleConfig.Lengths.Average();
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

        //TODO: Revise the utility of this
        private double GetRoundAmount(Measure value)
        {
            if (value.Unit == UnitOfMeasure.Cm)
                return 0.01;
            else if (value.Unit == UnitOfMeasure.Mm)
                return 0.1;
            else if (value.Unit == UnitOfMeasure.In)
                return 0.1;
            else if (value.Unit == UnitOfMeasure.Feets)
                return 1d / 12d;
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
        }
    }
}
