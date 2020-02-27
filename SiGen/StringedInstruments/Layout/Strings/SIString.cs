using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SiGen.StringedInstruments.Layout
{
    public class SIString : LayoutComponent
    {

		#region Fields
		private int _NumberOfFrets;
        private int _StartingFret;
        private StringTuning _Tuning;
        private StringProperties _PhysicalProperties;
        private Measure _ActionAtFirstFret;
        private Measure _ActionAtTwelfthFret;
        private double _MultiScaleRatio;
        //private FretManager _Frets;

		#endregion

		#region Properties

		[XmlAttribute("Index")]
		public int Index { get; internal set; }

		public Measure ScaleLength
        {
            get { return Layout.CurrentScaleLength.GetLength(Index)/*_ScaleLength*/; }
            set
            {
                Layout.CurrentScaleLength.SetLength(Index, value);
            }
        }

        /// <summary>
        /// The string's real length, taking into accounts the neck taper and measured at the starting fret (nut)
        /// </summary>
        public Measure StringLength { get; private set; }

        /// <summary>
        /// The string's real scale length, taking into accounts the neck taper and measured at the fret 0 (virtual nut)
        /// </summary>
        public Measure RealScaleLength { get; private set; }

        /// <summary>
        /// The length used to calculate fret positions.
        /// Depending on the value of <see cref="PlaceFretsRelativeToString"/>, it will either return the string's real length or vertical length.
        /// </summary>
        public Measure CalculatedLength
        {
            get
            {
                if (LayoutLine == null)
                    return Measure.Empty;
                return PlaceFretsRelativeToString ? LayoutLine.Length : LayoutLine.Bounds.Height;
            }
        }

        /// <summary>
        /// Gets or sets the number of fret.
        /// </summary>
        public int NumberOfFrets
        {
            get { return _NumberOfFrets; }
            set
            {
				SetPropertyValue(ref _NumberOfFrets, value);
			}
        }

        /// <summary>
        /// Gets or sets the starting fret. The default value is 0 (nut). 
        /// <para>A positive value places the starting fret (or nut) further down the fingerboard (eg: a banjo's first string starts at the fifth fret).
        /// A negative value places the starting fret (or nut) behind the nut (a.k.a. negative fret).</para>
        /// </summary>
        /// <example>test</example>
        public int StartingFret
        {
            get { return _StartingFret; }
            set
            {
				SetPropertyValue(ref _StartingFret, value);
			}
        }

        /// <summary>
        /// Gets or sets the total number of frets taking into account the starting fret.
        /// <para>E.g. the first string of a 22 frets banjo starts at the fifth fret so in total that string has 17 frets.</para>
        /// </summary>
        public int TotalNumberOfFrets
        {
            get { return NumberOfFrets - StartingFret; }
            set
            {
                NumberOfFrets = value + StartingFret;
            }
        }

        /// <summary>
        /// Gets or sets the relative offset of this string for a mutli scale length layout.
        /// <para>This value affects the placement of this string relative to the longest scale length.</para>
        /// </summary>
        /// <remarks>This value is now only used when the scale length mode is individual (manual)</remarks>
        public double MultiScaleRatio
        {
            get { return _MultiScaleRatio; }
            set
            {
				SetPropertyValue(ref _MultiScaleRatio, value, Layout.ScaleLengthMode != ScaleLengthType.Single);
			}
        }

        /// <summary>
        /// Determine if the scale length is applied along the string (taking into account the neck taper) or straight along the fingerboard.
        /// </summary>
        /// <remarks>Originally this value was assignable by string, but that complicated considerably the layout calculation and it is not really usefull anyway.</remarks>
        public LengthFunction LengthCalculationMethod
        {
            get { return Layout.CurrentScaleLength.LengthCalculationMethod; }
            set
            {
                Layout.CurrentScaleLength.LengthCalculationMethod = value;
            }
        }

        public bool PlaceFretsRelativeToString
        {
            get
            {
                return Layout.CompensateFretPositions || LengthCalculationMethod == LengthFunction.AlongString;
            }
        }

        public StringProperties PhysicalProperties
        {
            get { return _PhysicalProperties; }
            set
            {
				SetPropertyValue(ref _PhysicalProperties, value);
			}
        }

        public Measure Gauge
        {
            get
            {
                if (PhysicalProperties != null)
                    return PhysicalProperties.StringDiameter;
                return Measure.Empty;
            }
            set
            {
                if (PhysicalProperties == null)
                    PhysicalProperties = new StringProperties() { StringDiameter = value };
                else if (PhysicalProperties.StringDiameter != value)
                {
					var oldValue = PhysicalProperties.StringDiameter;
					//SetSubPropertyValue(PhysicalProperties, p => p.StringDiameter, value);
					PhysicalProperties.StringDiameter = value;
                    NotifyLayoutChanged(new PropertyChange(this, "PhysicalProperties.StringDiameter", oldValue, value));
				}
            }
        }

        public StringTuning Tuning
        {
            get => _Tuning;
            set => SetPropertyValue(ref _Tuning, value, Layout.FretsTemperament != Temperament.Equal || Layout.CompensateFretPositions);
        }

        /// <summary>
        /// Gets or sets the string action at the first fret, measured from the top of the fret to the bottom of the string.
        /// <br/>Used only for fret compensation calculation.
        /// </summary>
        public Measure ActionAtFirstFret
        {
            get { return _ActionAtFirstFret; }
            set => SetPropertyValue(ref _ActionAtFirstFret, value, false);
        }

        /// <summary>
        /// Gets or sets the string action at the twelfth fret, measured from the top of the fret to the bottom of the string.
        /// <br/>Used only for fret compensation calculation.
        /// </summary>
        public Measure ActionAtTwelfthFret
        {
            get { return _ActionAtTwelfthFret; }
            set => SetPropertyValue(ref _ActionAtTwelfthFret, value, false);
        }

        public bool CanCalculateCompensation
        {
            get { return Tuning != null && 
                    PhysicalProperties != null && 
                    PhysicalProperties.CanCalculateCompensation && 
                    ActionAtFirstFret > Measure.Zero &&
                    ActionAtTwelfthFret > Measure.Zero; }
        }

        public SIString Previous
        {
            get { return Index > 0 ? Layout.Strings[Index - 1] : null; }
        }

        public SIString Next
        {
            get { return Index < Layout.NumberOfStrings - 1 ? Layout.Strings[Index + 1] : null; }
        }

        public Visual.StringLine LayoutLine
        {
            get { return Layout.VisualElements.OfType<Visual.StringLine>().FirstOrDefault(l => l.String == this); }
        }

        #endregion

        internal SIString() : base(null)
        {
            _ActionAtTwelfthFret = Measure.Empty;
            _ActionAtFirstFret = Measure.Empty;
            _MultiScaleRatio = 0.5;
            _NumberOfFrets = 24;
            RealScaleLength = Measure.Empty;
        }

        public SIString(SILayout layout, int stringIndex) : base(layout)
        {
            Index = stringIndex;
            _ActionAtTwelfthFret = Measure.Empty;
            _ActionAtFirstFret = Measure.Empty;
            _MultiScaleRatio = 0.5;
            _NumberOfFrets = 24;
            RealScaleLength = Measure.Empty;
        }

        public bool HasFret(int fretNo)
        {
            return fretNo >= StartingFret && fretNo <= NumberOfFrets;
        }

        internal void RecalculateLengths()
        {
            if (LayoutLine != null)
            {
                StringLength = LayoutLine.Length.Convert(ScaleLength.Unit);

                if (LayoutLine.FretZero != PointM.Empty && LayoutLine.FretZero != LayoutLine.P1)
                    RealScaleLength = PointM.Distance(LayoutLine.FretZero, LayoutLine.P2);
                else
                    RealScaleLength = StringLength;

                RealScaleLength = RealScaleLength.Convert(ScaleLength.Unit);
            }
        }

        internal void ClearLayoutData()
        {
            StringLength = Measure.Empty;
            RealScaleLength = Measure.Empty;
        }

        #region XML serialization

        internal XElement Serialize(string elemName)
        {
            
            var stringElem = new XElement(elemName, new XAttribute("Index", Index));

            if (Layout.ScaleLengthMode == ScaleLengthType.Multiple)
            {
                stringElem.Add(ScaleLength.SerializeAsAttribute("ScaleLength"));
                stringElem.Add(new XAttribute("MultiScaleRatio", MultiScaleRatio));
            }

            var fretElem = new XElement("Frets",
                    new XAttribute("StartingFret", StartingFret),
                    new XAttribute("NumberOfFrets", NumberOfFrets));

            stringElem.Add(fretElem);

            if (!ActionAtTwelfthFret.IsEmpty)
            {
                var actionElem = new XElement("Action",
                    ActionAtFirstFret.SerializeAsAttribute("AtFirstFret"),
                    ActionAtTwelfthFret.SerializeAsAttribute("AtTwelfthFret"));
                stringElem.Add(actionElem);
            }

            if (Tuning != null)
                stringElem.Add(Tuning.Serialize("Tuning"));
            if (PhysicalProperties != null)
                stringElem.Add(SerializationHelper.GenericSerialize(PhysicalProperties, "Properties"));
            return stringElem;
        }

        internal void Deserialize(XElement elem)
        {
            var fretElem = elem.Element("Frets");

            _StartingFret = fretElem.GetIntAttribute("StartingFret");
            _NumberOfFrets = fretElem.GetIntAttribute("NumberOfFrets");

            if (elem.ContainsAttribute("ScaleLength"))
                ScaleLength = Measure.ParseInvariant(elem.Attribute("ScaleLength").Value);

            if(elem.ContainsAttribute("MultiScaleRatio"))
                _MultiScaleRatio = double.Parse(elem.Attribute("MultiScaleRatio").Value, System.Globalization.NumberFormatInfo.InvariantInfo);
            //else
            //    _MultiScaleRatio = 0.5d;

            if (elem.ContainsElement("Tuning"))
                _Tuning = StringTuning.Deserialize(elem.Element("Tuning"));

            if (elem.ContainsElement("Action"))
            {
                var actionElem = elem.Element("Action");
                _ActionAtFirstFret = actionElem.ReadAttribute("AtFirstFret", Measure.Empty);
                _ActionAtTwelfthFret = actionElem.ReadAttribute("AtTwelfthFret", Measure.Empty);
            }

            if (elem.ContainsElement("Properties"))
                _PhysicalProperties = SerializationHelper.GenericDeserialize<StringProperties>(elem.Element("Properties"));
        }

        #endregion
    }
}
