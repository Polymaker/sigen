using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
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

        private readonly int _Index;
        private int _NumberOfFrets;
        private int _StartingFret;
        private StringTuning _Tuning;
        private StringProperties _PhysicalProperties;
        private Measure _ActionAtTwelfthFret;
        private double _RelativeScaleLengthOffset;

        #endregion

        #region Properties

        [XmlAttribute("Index")]
        public int Index { get { return _Index; } }

        public Measure ScaleLength
        {
            get { return Layout.CurrentScaleLength.GetLength(Index)/*_ScaleLength*/; }
        }

        /// <summary>
        /// Length used for fret positions
        /// </summary>
        public Measure FinalLength { get; set; }

        /// <summary>
        /// Gets or sets the number of fret. This value d
        /// </summary>
        [XmlAttribute("NumberOfFrets")]
        public int NumberOfFrets
        {
            get { return _NumberOfFrets; }
            set
            {
                if(value != _NumberOfFrets)
                {
                    _NumberOfFrets = value;
                    //Layout.OnStringConfigChanged(this, "NumberOfFrets");
                    Layout.NotifyLayoutChanged(this, "NumberOfFrets");
                }
            }
        }

        /// <summary>
        /// Gets or sets the starting fret. The default value is 0 (nut). 
        /// A positive value places the starting fret (or nut) further down the fingerboard (eg: a banjo's first string starts at the fifth fret).
        /// A negative value places the starting fret (or nut) behind the nut (a.k.a. negative fret).
        /// </summary>
        [XmlAttribute("StartingFret")]
        public int StartingFret
        {
            get { return _StartingFret; }
            set
            {
                if (value != _StartingFret)
                {
                    _StartingFret = value;
                    //Layout.OnStringConfigChanged(this, "StartingFret");
                    Layout.NotifyLayoutChanged(this, "StartingFret");
                }
            }
        }

        /// <summary>
        /// Gets or sets the total number of frets taking into account the starting fret.
        /// E.g. the first string of a 22 frets banjo starts at the fifth fret so in total that string has 17 frets.
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
        /// This value affects the placement of this string relative to the longest scale length.</para>
        /// </summary>
        [XmlAttribute("MultiScaleRatio")]
        public double RelativeScaleLengthOffset
        {
            get { return _RelativeScaleLengthOffset; }
            set
            {
                if (value != _RelativeScaleLengthOffset)
                {
                    _RelativeScaleLengthOffset = value;
                    Layout.NotifyLayoutChanged(this, "RelativeScaleLengthOffset");
                }
            }
        }

        public LengthFunction LengthCalculationMethod
        {
            get { return Layout.CurrentScaleLength.LengthCalculationMethod; }
            set
            {
                Layout.CurrentScaleLength.LengthCalculationMethod = value;
            }
            //get { return _LengthCalculationMethod; }
            //set
            //{
            //    if (value != _LengthCalculationMethod)
            //    {
            //        _LengthCalculationMethod = value;
            //        Layout.NotifyLayoutChanged(this, "LengthCalculationMethod");
            //    }
            //}
        }

        public bool PlaceFretsRelativeToString
        {
            get
            {
                return Layout.CompensateFretPositions || LengthCalculationMethod == LengthFunction.AlongString;
            }
        }

        [XmlElement("Properties")]
        public StringProperties PhysicalProperties
        {
            get { return _PhysicalProperties; }
            set
            {
                if (value != _PhysicalProperties)
                {
                    _PhysicalProperties = value;
                    Layout.NotifyLayoutChanged(this, "PhysicalProperties");
                    //EnsureCanStillCalculateCompentation();
                }
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
                    PhysicalProperties = new StringProperties();
                PhysicalProperties.StringDiameter = value;
            }
        }

        [XmlElement("Tuning")]
        public StringTuning Tuning
        {
            get { return _Tuning; }
            set
            {
                if (value != _Tuning)
                {
                    _Tuning = value;
                    if (Layout.FretsTemperament != Temperament.Equal)
                        Layout.NotifyLayoutChanged(this, "Tuning");
                    //EnsureCanStillCalculateCompentation();
                }
            }
        }

        /// <summary>
        /// Gets or sets the string action at the twelfth fret, measured above the fret.
        /// Used only for fret compensation calculation.
        /// </summary>
        [XmlAttribute("ActionAtTwelfthFret")]
        public Measure ActionAtTwelfthFret
        {
            get { return _ActionAtTwelfthFret; }
            set
            {
                if (_ActionAtTwelfthFret != value)
                {
                    _ActionAtTwelfthFret = value;
                    //EnsureCanStillCalculateCompentation();
                }
            }
        }

        public bool CanCalculateCompensation
        {
            get { return Tuning != null && 
                    PhysicalProperties != null && 
                    PhysicalProperties.CanCalculateCompensation && 
                    ActionAtTwelfthFret != Measure.Zero; }
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

        public SIString(SILayout layout, int stringIndex) : base(layout)
        {
            _Index = stringIndex;
            _ActionAtTwelfthFret = Measure.Empty;
            _RelativeScaleLengthOffset = 0.5;
            _NumberOfFrets = 24;
        }

        public bool HasFret(int fretNo)
        {
            return fretNo >= StartingFret && fretNo <= NumberOfFrets;
        }

        internal void UpdateFinalLength()
        {
            if (PlaceFretsRelativeToString)
                FinalLength = LayoutLine.Length;
            else
                FinalLength = LayoutLine.Bounds.Height;
        }

        #region XML serialization

        internal XElement Serialize(bool includeScale = false)
        {
            var elem = new XElement("String", 
                new XAttribute("Index", Index),
                new XAttribute("StartingFret", StartingFret),
                new XAttribute("NumberOfFrets", NumberOfFrets),
                new XAttribute("MultiScaleRatio", RelativeScaleLengthOffset),
                //new XAttribute("LengthMethod", LengthCalculationMethod),
                //ScaleLength.SerializeAsAttribute("ScaleLength"),
                ActionAtTwelfthFret.SerializeAsAttribute("ActionAtTwelfthFret"));
            if (includeScale)
                elem.Add(ScaleLength.SerializeAsAttribute("ScaleLength"));
            if (Tuning != null)
                elem.Add(new XElement("Tuning"));
            if (PhysicalProperties != null)
                elem.Add(new XElement("Properties"));
            return elem;
        }

        internal void Deserialize(XElement elem)
        {
            _StartingFret = elem.GetIntAttribute("StartingFret");
            _NumberOfFrets = elem.GetIntAttribute("NumberOfFrets");
            _RelativeScaleLengthOffset = double.Parse(elem.Attribute("MultiScaleRatio").Value);
            _ActionAtTwelfthFret = Measure.Parse(elem.Attribute("ActionAtTwelfthFret").Value);
        }

        #endregion
    }
}
