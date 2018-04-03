using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class ScaleLengthManager : LayoutComponent
    {
        protected LengthFunction _LengthCalculationMethod;
        /// <summary>
        /// Determine if the scale length is applied along each strings (taking into account the neck taper) or straight along the fingerboard.
        /// </summary>
        /// <remarks>Originally this value was assignable by string, but that complicated considerably the layout calculation and it is not really usefull anyway.</remarks>
        public LengthFunction LengthCalculationMethod
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

        public abstract ScaleLengthType Type { get; }
        public ScaleLengthManager(SILayout layout) : base(layout)
        {
            _LengthCalculationMethod = LengthFunction.AlongString;
        }
        public abstract void SetLength(int index, Measure value);
        public abstract Measure GetLength(int index);

        public virtual XElement Serialize(string elemName)
        {
            return new XElement(elemName, new XAttribute("Type", Type), new XAttribute("LengthFunction", LengthCalculationMethod));
        }

        internal virtual void Deserialize(XElement elem)
        {
            LengthCalculationMethod = (LengthFunction)Enum.Parse(typeof(LengthFunction), elem.Attribute("LengthFunction").Value);
        }

        public class SingleScale : ScaleLengthManager
        {
            private Measure _Length;
            
            public Measure Length
            {
                get { return _Length; }
                set
                {
                    if (value != _Length)
                    {
                        _Length = value;
                        if (Layout.CurrentScaleLength == this)
                            Layout.NotifyLayoutChanged(this, "ScaleLength");
                    }
                }
            }

            public override ScaleLengthType Type
            {
                get { return ScaleLengthType.Single; }
            }

            public SingleScale(SILayout layout) : base(layout)
            {
                _LengthCalculationMethod = LengthFunction.AlongFingerboard;
                _Length = Measure.Inches(25.5);
            }

            public override Measure GetLength(int index)
            {
                return Length;
            }

            public override void SetLength(int index, Measure value)
            {
                Length = value;
            }

            public override XElement Serialize(string elemName)
            {
                var elem = base.Serialize(elemName);
                elem.Add(Length.SerializeAsAttribute("Value"));
                return elem;
            }

            internal override void Deserialize(XElement elem)
            {
                base.Deserialize(elem);
                Length = Measure.Parse(elem.Attribute("Value").Value);
            }
        }

        public class MultiScale : ScaleLengthManager
        {
            private Measure _Treble;
            private Measure _Bass;
            private double _PerpendicularFretRatio;

            public Measure Treble
            {
                get { return _Treble; }
                set
                {
                    if (value != _Treble)
                    {
                        _Treble = value;
                        if (Layout.CurrentScaleLength == this)
                            Layout.NotifyLayoutChanged(this, "ScaleLength");
                    }
                }
            }

            public Measure Bass
            {
                get { return _Bass; }
                set
                {
                    if (value != _Bass)
                    {
                        _Bass = value;
                        if(Layout.CurrentScaleLength == this)
                            Layout.NotifyLayoutChanged(this, "ScaleLength");
                    }
                }
            }

            public double PerpendicularFretRatio
            {
                get
                {
                    //if (!Layout.Strings.AllEqual(s => s.MultiScaleRatio))
                    //    return -1;
                    return _PerpendicularFretRatio;
                }
                set
                {
                    _PerpendicularFretRatio = value;
                    if (Layout.CurrentScaleLength == this)
                        Layout.Strings.SetAll(s => s.MultiScaleRatio, value);
                }
            }

            public override ScaleLengthType Type
            {
                get { return ScaleLengthType.Multiple; }
            }

            public MultiScale(SILayout layout) : base(layout)
            {
                _Treble = Measure.Inches(25.5);
                _Bass = Measure.Inches(27);
                _PerpendicularFretRatio = 0.5;
            }

            public override Measure GetLength(int index)
            {
                if (index == 0)
                    return Treble;
                else if (index == Layout.NumberOfStrings - 1)
                    return Bass;
                var step = (Bass - Treble) / (Layout.NumberOfStrings - 1);
                return Treble + (step * index);
            }

            public override void SetLength(int index, Measure value)
            {
                if (index == 0)
                    Treble = value;
                else if (index == Layout.NumberOfStrings - 1)
                    Bass = value;
            }

            public override XElement Serialize(string elemName)
            {
                var elem = base.Serialize(elemName);
                elem.Add(Treble.SerializeAsAttribute("Treble"));
                elem.Add(Bass.SerializeAsAttribute("Bass"));
                if (Layout.Strings.AllEqual(s => s.MultiScaleRatio))
                    elem.Add(new XAttribute("MultiScaleRatio", PerpendicularFretRatio));
                return elem;
            }

            internal override void Deserialize(XElement elem)
            {
                base.Deserialize(elem);
                Treble = Measure.Parse(elem.Attribute("Treble").Value);
                Bass = Measure.Parse(elem.Attribute("Bass").Value);
                if (elem.ContainsAttribute("MultiScaleRatio"))
                    PerpendicularFretRatio = double.Parse(elem.Attribute("MultiScaleRatio").Value);
                //else if (Layout.Strings.AllEqual(s => s.MultiScaleRatio))
                //    _PerpendicularFretRatio = Layout.Strings[0].MultiScaleRatio;
            }
        }

        public class Individual : ScaleLengthManager
        {
            private Measure[] _Lengths;

            public Measure[] Lengths { get { return _Lengths; } }

            public override ScaleLengthType Type
            {
                get { return ScaleLengthType.Individual; }
            }

            public Individual(SILayout layout) : base(layout)
            {

            }

            internal void InitializeIfNeeded()
            {
                if(_Lengths == null || _Lengths.Length != NumberOfStrings)
                {
                    _Lengths = new Measure[Layout.NumberOfStrings];
                    for (int i = 0; i < Layout.NumberOfStrings; i++)
                    {
                        _Lengths[i] = !Layout.Strings[i].RealScaleLength.IsEmpty ? Layout.Strings[i].RealScaleLength : Layout.Strings[i].ScaleLength;
                        _Lengths[i].Unit = Layout.Strings[i].ScaleLength.Unit;
                    }
                }
            }

            protected override void OnStringConfigurationChanged()
            {
                if (_Lengths != null && _Lengths.Length > 0)
                {
                    var oldLengths = _Lengths;
                    _Lengths = new Measure[Layout.NumberOfStrings];
                    for (int i = 0; i < Layout.NumberOfStrings; i++)
                    {
                        if (i < oldLengths.Length)
                            _Lengths[i] = oldLengths[i];
                        else
                            _Lengths[i] = oldLengths[oldLengths.Length - 1];
                    }
                }
                //else
                //{
                //    _Lengths = new Measure[Layout.NumberOfStrings];
                //    for (int i = 0; i < Layout.NumberOfStrings; i++)
                //        _Lengths[i] = Measure.Inches(25.5);
                //}
            }

            public override Measure GetLength(int index)
            {
                return Lengths[index];
            }

            public override void SetLength(int index, Measure value)
            {
                _Lengths[index] = value;
                if (Layout.CurrentScaleLength == this)
                    Layout.NotifyLayoutChanged(this, "ScaleLength");
            }

            public void SetLengths(params Measure[] lengths)
            {
                if (lengths.Length != NumberOfStrings)
                    throw new InvalidOperationException();
                _Lengths = lengths;
                if (Layout.CurrentScaleLength == this)
                    Layout.NotifyLayoutChanged(this, "ScaleLength");
            }

            public override XElement Serialize(string elemName)
            {
                return base.Serialize(elemName);
            }

            internal override void Deserialize(XElement elem)
            {
                base.Deserialize(elem);
                _Lengths = new Measure[NumberOfStrings];
            }
        }
    } 
}
