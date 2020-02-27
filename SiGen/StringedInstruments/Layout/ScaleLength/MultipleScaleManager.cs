using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public class MultipleScaleManager : ScaleLengthManager
    {
        private Measure[] _Lengths;
        private Dictionary<SIString, Measure> StringLengthCache;

        public Measure[] Lengths { get { return _Lengths; } }


        public override ScaleLengthType Type
        {
            get { return ScaleLengthType.Multiple; }
        }

        public MultipleScaleManager(SILayout layout) : base(layout)
        {
            StringLengthCache = new Dictionary<SIString, Measure>();
        }

        internal void InitializeIfNeeded()
        {
            if (_Lengths == null || _Lengths.Length != NumberOfStrings)
                CopyValuesFromCurrentLayout();
        }

        public void CopyValuesFromCurrentLayout()
        {
            if (_Lengths == null || _Lengths.Length != NumberOfStrings)
                _Lengths = new Measure[Layout.NumberOfStrings];

            for (int i = 0; i < Layout.NumberOfStrings; i++)
            {
                var str = Layout.Strings[i];
                if (!str.RealScaleLength.IsEmpty)
                    _Lengths[i] = str.RealScaleLength;
                else if (Layout.ScaleLengthMode != ScaleLengthType.Multiple)
                    _Lengths[i] = Layout.CurrentScaleLength.GetLength(str.Index);
                else
                    _Lengths[i] = Measure.Inches(25.5);
            }
        }

        private void UpdateStringCache()
        {
            if (_Lengths != null)
            {
                foreach (var str in Layout.Strings)
                {
                    if (str.Index < _Lengths.Length)
                        StringLengthCache[str] = _Lengths[str.Index];
                }
            }
        }

        protected override void BeforeChangingStrings()
        {
            UpdateStringCache();
        }

        protected override void OnStringsChanged()
        {
            if (StringLengthCache.Count > 0 && _Lengths != null)
            {
                var oldLengths = _Lengths;
                _Lengths = new Measure[Layout.NumberOfStrings];

                for (int i = 0; i < Layout.NumberOfStrings; i++)
                {
                    var str = Layout.Strings[i];

                    if (StringLengthCache.ContainsKey(str))
                        _Lengths[i] = StringLengthCache[str];
                    else if (StringLengthCache.TryGetValue(str.Previous, out Measure prevLength))
                        _Lengths[i] = prevLength;
                    else if (StringLengthCache.TryGetValue(str.Next, out Measure nextLength))
                        _Lengths[i] = nextLength;
                    else
                        _Lengths[i] = Measure.Inches(25.5);
                }

                NotifyLayoutChanged(new PropertyChange(this, "_Lengths", oldLengths, _Lengths, true));
            }

            UpdateStringCache();
        }

        public override Measure GetLength(int index)
        {
            return Lengths[index];
        }

        public override void SetLength(int index, Measure value)
        {
            SetFieldValue(ref _Lengths, index, value, nameof(_Lengths));
        }

        internal override void OnSetFieldValue(string fieldName, object field, int? index, object value)
        {
            if (fieldName == nameof(_Lengths))
                UpdateStringCache();
        }

        public override XElement Serialize(string elemName)
        {
            //Lengths are serialized in the string element so nothing to do here
            return base.Serialize(elemName);
        }

        internal override void Deserialize(XElement elem)
        {
            base.Deserialize(elem);
            //Lengths are serialized in the string element so nothing to do here (except initialize Array)
            _Lengths = new Measure[NumberOfStrings];
        }
    }
}
