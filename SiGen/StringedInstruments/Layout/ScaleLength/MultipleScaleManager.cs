using SiGen.Measuring;
using System;
using System.Linq;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public class MultipleScaleManager : ScaleLengthManager
    {
        private Measure[] _Lengths;

        public Measure[] Lengths { get { return _Lengths; } }

        public override ScaleLengthType Type
        {
            get { return ScaleLengthType.Multiple; }
        }

        public MultipleScaleManager(SILayout layout) : base(layout)
        {

        }

        internal void InitializeIfNeeded()
        {
            if (_Lengths == null || _Lengths.Length != NumberOfStrings)
            {
                _Lengths = new Measure[Layout.NumberOfStrings];

                for (int i = 0; i < Layout.NumberOfStrings; i++)
                {
                    _Lengths[i] = !Layout.Strings[i].RealScaleLength.IsEmpty ? Layout.Strings[i].RealScaleLength : Layout.Strings[i].ScaleLength;
                    _Lengths[i].Unit = Layout.Strings[i].ScaleLength.Unit;
                }
            }
        }

        public void CopyValuesFromCurrentLayout()
        {
            if (Layout.Strings.All(s => s.LayoutLine != null))
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

                NotifyLayoutChanged(new PropertyChange(this, "_Lengths", oldLengths, _Lengths, true));
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
            SetFieldValue(ref _Lengths, index, value, nameof(_Lengths));
        }

        public void SetLengths(params Measure[] lengths)
        {
            if (lengths.Length != NumberOfStrings)
                throw new InvalidOperationException();

            SetFieldValue(ref _Lengths, lengths, nameof(_Lengths));
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
