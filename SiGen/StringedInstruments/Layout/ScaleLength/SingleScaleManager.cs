using SiGen.Measuring;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public class SingleScaleManager : ScaleLengthManager
    {
        private Measure _Length;

        public Measure Length
        {
            get { return _Length; }
            set
            {
                SetPropertyValue(ref _Length, value);
            }
        }

        public override ScaleLengthType Type
        {
            get { return ScaleLengthType.Single; }
        }

        public SingleScaleManager(SILayout layout) : base(layout)
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
            Length = Measure.ParseInvariant(elem.Attribute("Value").Value);
        }
    }
}
