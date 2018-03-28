using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public class StringSpacingSimple : StringSpacingManager
    {
        private Measure _StringSpacingAtNut;
        private Measure _StringSpacingAtBridge;
        private NutSpacingMode _NutSpacingMode;
        private Measure[] AdjustedNutSlots;

        public override StringSpacingType Type
        {
            get { return StringSpacingType.Simple; }
        }

        public Measure StringSpacingAtNut
        {
            get { return _StringSpacingAtNut; }
            set
            {
                if (value != _StringSpacingAtNut)
                {
                    _StringSpacingAtNut = value;
                    Layout.NotifyLayoutChanged(this, "StringSpacingAtNut");
                }
            }
        }

        public Measure StringSpacingAtBridge
        {
            get { return _StringSpacingAtBridge; }
            set
            {
                if (value != _StringSpacingAtNut)
                {
                    _StringSpacingAtBridge = value;
                    Layout.NotifyLayoutChanged(this, "StringSpacingAtBridge");
                }
            }
        }

        public override Measure StringSpreadAtNut
        {
            get
            {
                return StringSpacingAtNut * (Layout.NumberOfStrings - 1);
            }
            set
            {
                StringSpacingAtNut = value / (Layout.NumberOfStrings - 1);
            }
        }

        public override Measure StringSpreadAtBridge
        {
            get
            {
                return StringSpacingAtBridge * (Layout.NumberOfStrings - 1);
            }
            set
            {
                StringSpacingAtBridge = value / (Layout.NumberOfStrings - 1);
            }
        }

        public NutSpacingMode NutSpacingMode
        {
            get { return _NutSpacingMode; }
            set
            {
                if(value != NutSpacingMode)
                {
                    _NutSpacingMode = value;
                    Layout.NotifyLayoutChanged(this, "NutSpacingMode");
                }
            }
        }

        public StringSpacingSimple(SILayout layout) : base(layout)
        {
            _NutSpacingMode = NutSpacingMode.StringsCenter;
            AdjustedNutSlots = new Measure[0];
        }

        public override Measure GetSpacing(int index, FingerboardEnd side)
        {
            if (side == FingerboardEnd.Nut && NutSpacingMode == NutSpacingMode.BetweenStrings && AdjustedNutSlots.Length > 0)
                return AdjustedNutSlots[index];
            return side == FingerboardEnd.Nut ? StringSpacingAtNut : StringSpacingAtBridge;
        }

        public override void SetSpacing(FingerboardEnd side, int index, Measure value)
        {
            if (side == FingerboardEnd.Nut)
                _StringSpacingAtNut = value;
            else
                _StringSpacingAtBridge = value;
        }

        public override XElement Serialize(string elemName)
        {
            var elem = base.Serialize(elemName);
            elem.Add(new XAttribute("NutSpacingMode", NutSpacingMode));
            elem.Add(StringSpacingAtNut.SerializeAsAttribute("StringSpacingAtNut"));
            elem.Add(StringSpacingAtBridge.SerializeAsAttribute("StringSpacingAtBridge"));
            return elem;
        }

        public override void Deserialize(XElement elem)
        {
            base.Deserialize(elem);
            NutSpacingMode = (NutSpacingMode)Enum.Parse(typeof(NutSpacingMode), elem.Attribute("NutSpacingMode").Value);

            if(NutSpacingMode == NutSpacingMode.BetweenStrings)
            {
                StringSpacingAtNut = Measure.Parse(elem.Attribute("StringSpacingAtNut").Value);
                StringSpacingAtBridge = Measure.Parse(elem.Attribute("StringSpacingAtBridge").Value);
                AdjustedNutSlots = new Measure[elem.Elements("Spacing").Count()];
                foreach (var spacingElem in elem.Elements("Spacing"))
                {
                    int spacingIndex = spacingElem.GetIntAttribute("Index");
                    AdjustedNutSlots[spacingIndex] = Measure.Parse(spacingElem.Attribute("Nut").Value);
                }
            }
        }

        public void CalculateNutSlotPositions()
        {
            if (NutSpacingMode != NutSpacingMode.BetweenStrings || NumberOfStrings < 2)
            {
                AdjustedNutSlots = new Measure[0];
                return;
            }

            if(Layout.Strings.All(s=>s.Gauge != Measure.Empty))
            {
                AdjustedNutSlots = new Measure[NumberOfStrings - 1];
                var spacing = StringSpreadAtNut - Layout.Strings.Sum(s => s.Gauge);
                spacing += (Layout.FirstString.Gauge / 2) + (Layout.LastString.Gauge / 2);
                spacing /= NumberOfStrings - 1;
                for(int i = 0; i < NumberOfStrings - 1; i++)
                {
                    AdjustedNutSlots[i] = (Layout.Strings[i].Gauge / 2) + spacing + (Layout.Strings[i + 1].Gauge / 2);
                }
            }
            else
                AdjustedNutSlots = new Measure[0];
        }
    }
}
