﻿using SiGen.Measuring;
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

        private readonly int _Index;
        private int _NumberOfFrets;
        private int _StartingFret;
        private StringTuning _Tuning;
        private StringProperties _PhysicalProperties;
        private Measure _ActionAtTwelfthFret;
        private double _MultiScaleRatio;
        private FretManager _Frets;

        #endregion

        #region Properties

        [XmlAttribute("Index")]
        public int Index { get { return _Index; } }

        public Measure ScaleLength
        {
            get { return Layout.CurrentScaleLength.GetLength(Index)/*_ScaleLength*/; }
        }

        /// <summary>
        /// The string's real length, taking into accounts the neck taper and measured at the starting fret (nut)
        /// </summary>
        public Measure StringLength { get; private set; }

        /// <summary>
        /// The string's real length, taking into accounts the neck taper and measured at the fret 0 (virtual nut)
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
        public double MultiScaleRatio
        {
            get { return _MultiScaleRatio; }
            set
            {
                if (value != _MultiScaleRatio)
                {
                    _MultiScaleRatio = value;
                    Layout.NotifyLayoutChanged(this, "MultiScaleRatio");
                }
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
            _MultiScaleRatio = 0.5;
            _NumberOfFrets = 24;
            _Frets = new FretManager(this);
        }

        public bool HasFret(int fretNo)
        {
            return fretNo >= StartingFret && fretNo <= NumberOfFrets;
        }

        internal void RecalculateLengths()
        {
            StringLength = LayoutLine.Length;
            if (LayoutLine.FretZero != PointM.Empty)
                RealScaleLength = PointM.Distance(LayoutLine.FretZero, LayoutLine.P2);
            else
                RealScaleLength = StringLength;
        }

        #region XML serialization

        internal XElement Serialize(string elemName)
        {
            
            var stringElem = new XElement(elemName, new XAttribute("Index", Index));

            //if (includeScale)
            //    stringElem.Add(ScaleLength.SerializeAsAttribute("ScaleLength"));

            if (!Layout.Strings.AllEqual(s => s.MultiScaleRatio))
                stringElem.Add(new XAttribute("MultiScaleRatio", MultiScaleRatio));

            var fretElem = new XElement("Frets",
                    new XAttribute("StartingFret", StartingFret),
                    new XAttribute("NumberOfFrets", NumberOfFrets));

            stringElem.Add(fretElem);

            if (!ActionAtTwelfthFret.IsEmpty)
            {
                var actionElem = new XElement("Action",
                    Measure.Empty.SerializeAsAttribute("AtFirstFret"),
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

            if(elem.ContainsAttribute("MultiScaleRatio"))
                _MultiScaleRatio = double.Parse(elem.Attribute("MultiScaleRatio").Value);
            else
                _MultiScaleRatio = 0.5d;

            if (elem.ContainsElement("Tuning"))
                _Tuning = StringTuning.Deserialize(elem.Element("Tuning"));

            if (elem.ContainsElement("Action"))
            {
                var actionElem = elem.Element("Action");
                _ActionAtTwelfthFret = Measure.Parse(actionElem.Attribute("AtTwelfthFret").Value);
            }
        }

        #endregion
    }
}