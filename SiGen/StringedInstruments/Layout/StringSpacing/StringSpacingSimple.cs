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

        public new Measure StringSpreadAtNut
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

        public new Measure StringSpreadAtBridge
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

        public override Measure GetSpacing(int index, bool atNut)
        {
            if (atNut && NutSpacingMode == NutSpacingMode.BetweenStrings && AdjustedNutSlots.Length > 0)
                return AdjustedNutSlots[index];
            return atNut ? StringSpacingAtNut : StringSpacingAtBridge;
        }

        public override void SetSpacing(int index, Measure value, bool atNut)
        {
            if (atNut)
                _StringSpacingAtNut = value;
            else
                _StringSpacingAtBridge = value;
        }

        public override XElement Serialize(string elemName)
        {
            var elem = base.Serialize(elemName);
            elem.Add(StringSpacingAtNut.SerializeAsAttribute("StringSpacingAtNut"));
            elem.Add(StringSpacingAtBridge.SerializeAsAttribute("StringSpacingAtBridge"));
            return elem;
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
        }
    }
}
