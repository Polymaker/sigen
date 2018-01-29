using SiGen.Measuring;
using SiGen.Physics;
using SiGen.StringedInstruments.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        private Measure _ScaleLength;
        private Measure _ActionAtTwelfthFret;
        private ScaleLengthMethod _LengthCalculationMethod;
        private double _RelativeScaleLengthOffset;

        #endregion

        #region Properties

        public int Index { get { return _Index; } }

        public Measure ScaleLength
        {
            get { return Layout.CurrentScaleLength.GetLength(Index)/*_ScaleLength*/; }
            //set
            //{
            //    if(value != _ScaleLength)
            //    {
            //        _ScaleLength = value;
            //        Layout.NotifyLayoutChanged(this, "ScaleLength");
            //    }
            //}
        }

        public Measure FinalLength { get; set; }

        /// <summary>
        /// Gets or sets the number of fret. This value d
        /// </summary>
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

        public ScaleLengthMethod LengthCalculationMethod
        {
            get { return _LengthCalculationMethod; }
            set
            {
                if (value != _LengthCalculationMethod)
                {
                    _LengthCalculationMethod = value;
                    Layout.NotifyLayoutChanged(this, "LengthCalculationMethod");
                }
            }
        }

        public bool PlaceFretsRelativeToString
        {
            get
            {
                return /*Layout.CompensateFretPositions || */LengthCalculationMethod == ScaleLengthMethod.AlongString;
            }
        }

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

        public SIString Previous
        {
            get { return Index > 0 ? Layout.Strings[Index - 1] : null; }
        }

        public SIString Next
        {
            get { return Index < Layout.NumberOfStrings - 1 ? Layout.Strings[Index + 1] : null; }
        }

        #endregion

        public SIString(SILayout layout, int stringIndex) : base(layout)
        {
            _Index = stringIndex;
            _ActionAtTwelfthFret = Measure.Empty;
        }

        #region XML serialization

        internal XElement Serialize()
        {
            var elem = new XElement("String", 
                new XAttribute("Index", Index),
                new XAttribute("StartingFret", StartingFret),
                new XAttribute("NumberOfFrets", NumberOfFrets),
                new XAttribute("LengthMethod", LengthCalculationMethod),
                ScaleLength.SerializeAsAttribute("ScaleLength"),
                ActionAtTwelfthFret.SerializeAsAttribute("ActionAtTwelfthFret"));

            return elem;
        } 

        #endregion
    }
}
