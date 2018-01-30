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

        public StringSpacingSimple(SILayout layout) : base(layout)
        {

        }

        public override Measure GetSpacing(int index, bool atNut)
        {
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
    }
}
