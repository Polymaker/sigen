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

            public Measure Treble
            {
                get { return _Treble; }
                set
                {
                    if (value != _Treble)
                    {
                        _Treble = value;
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
                        Layout.NotifyLayoutChanged(this, "ScaleLength");
                    }
                }
            }

            public override ScaleLengthType Type
            {
                get { return ScaleLengthType.Multiple; }
            }

            public MultiScale(SILayout layout) : base(layout)
            {

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
                return elem;
            }

            internal override void Deserialize(XElement elem)
            {
                base.Deserialize(elem);
                Treble = Measure.Parse(elem.Attribute("Treble").Value);
                Bass = Measure.Parse(elem.Attribute("Bass").Value);
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
                else
                    _Lengths = new Measure[Layout.NumberOfStrings];
            }

            public override Measure GetLength(int index)
            {
                return Lengths[index];
            }

            public override void SetLength(int index, Measure value)
            {
                _Lengths[index] = value;
                Layout.NotifyLayoutChanged(this, "ScaleLength");
            }

            public void SetLengths(params Measure[] lengths)
            {
                if (lengths.Length != NumberOfStrings)
                    throw new InvalidOperationException();
                _Lengths = lengths;
            }
        }
    } 
}
