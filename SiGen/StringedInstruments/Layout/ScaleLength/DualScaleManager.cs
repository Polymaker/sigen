using SiGen.Measuring;
using System.Globalization;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public class DualScaleManager : ScaleLengthManager
    {
        private Measure _Treble;
        private Measure _Bass;
        private double _PerpendicularFretRatio;

        public Measure Treble
        {
            get { return _Treble; }
            set
            {
                SetPropertyValue(ref _Treble, value);
            }
        }

        public Measure Bass
        {
            get { return _Bass; }
            set
            {
                SetPropertyValue(ref _Bass, value);
            }
        }

        public double PerpendicularFretRatio
        {
            get { return _PerpendicularFretRatio; }
            set
            {
                SetPropertyValue(ref _PerpendicularFretRatio, value);
            }
        }

        public override ScaleLengthType Type
        {
            get { return ScaleLengthType.Dual; }
        }

        public DualScaleManager(SILayout layout) : base(layout)
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
            //if (Layout.Strings.AllEqual(s => s.MultiScaleRatio))
            elem.Add(new XAttribute("MultiScaleRatio", PerpendicularFretRatio.ToString(NumberFormatInfo.InvariantInfo)));
            return elem;
        }

        internal override void Deserialize(XElement elem)
        {
            base.Deserialize(elem);
            Treble = Measure.ParseInvariant(elem.Attribute("Treble").Value);
            Bass = Measure.ParseInvariant(elem.Attribute("Bass").Value);
            if (elem.ContainsAttribute("MultiScaleRatio"))
                PerpendicularFretRatio = double.Parse(elem.Attribute("MultiScaleRatio").Value, NumberFormatInfo.InvariantInfo);
            else
                PerpendicularFretRatio = 0.5;
            //else if (Layout.Strings.AllEqual(s => s.MultiScaleRatio))
            //    _PerpendicularFretRatio = Layout.Strings[0].MultiScaleRatio;
        }
    }
}
