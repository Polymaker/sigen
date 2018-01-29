using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class ScaleLengthManager : LayoutComponent
    {
        public ScaleLengthManager(SILayout layout) : base(layout)
        {

        }
        public abstract void SetLength(int index, Measure value);
        public abstract Measure GetLength(int index);

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

            public SingleScale(SILayout layout) : base(layout)
            {

            }

            public override Measure GetLength(int index)
            {
                return Length;
            }

            public override void SetLength(int index, Measure value)
            {
                Length = value;
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
        }

        public class Individual : ScaleLengthManager
        {
            private Measure[] _Lengths;

            public Measure[] Lengths { get { return _Lengths; } }

            public Individual(SILayout layout) : base(layout)
            {
                layout.NumberOfStringsChanged += Layout_NumberOfStringsChanged;
            }

            private void Layout_NumberOfStringsChanged(object sender, EventArgs e)
            {
                if (_Lengths != null && _Lengths.Length > 0)
                {
                    var oldLengths = _Lengths;
                    _Lengths = new Measure[Layout.NumberOfStrings - 1];
                    for (int i = 0; i < Layout.NumberOfStrings - 1; i++)
                    {
                        if (i < oldLengths.Length)
                            _Lengths[i] = oldLengths[i];
                        else
                            _Lengths[i] = oldLengths[oldLengths.Length - 1];
                    }
                }
                else
                    _Lengths = new Measure[Layout.NumberOfStrings - 1];
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
        }
    }
    
}
