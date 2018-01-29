using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class StringsSpacingManual : StringsSpacingBase
    {
        private Measure[] _NutSpacing;
        private Measure[] _BridgeSpacing;

        public Measure[] NutSpacing { get { return _NutSpacing; } }
        public Measure[] BridgeSpacing { get { return _BridgeSpacing; } }

        public StringsSpacingManual(SILayout layout) : base(layout)
        {

        }

        protected override void OnNumberOfStringsChanged()
        {
            if(_NutSpacing != null && _NutSpacing.Length > 0)
            {
                var oldNut = _NutSpacing;
                var oldBridge = _BridgeSpacing;
                _NutSpacing = new Measure[Layout.NumberOfStrings - 1];
                _BridgeSpacing = new Measure[Layout.NumberOfStrings - 1];
                for (int i = 0; i < Layout.NumberOfStrings - 1; i++)
                {
                    if(i< oldNut.Length)
                    {
                        _NutSpacing[i] = oldNut[i];
                        _BridgeSpacing[i] = oldBridge[i];
                    }
                    else
                    {
                        _NutSpacing[i] = oldNut[oldNut.Length - 1];
                        _BridgeSpacing[i] = oldBridge[oldBridge.Length - 1];
                    }
                }
            }
            else
            {
                _NutSpacing = new Measure[Layout.NumberOfStrings - 1];
                _BridgeSpacing = new Measure[Layout.NumberOfStrings - 1];
            }
        }

        public override Measure GetSpacing(int index, bool atNut)
        {
            return atNut ? _NutSpacing[index] : _BridgeSpacing[index];
        }

        public override void SetSpacing(int index, Measure value, bool atNut)
        {
            if (atNut)
                _NutSpacing[index] = value;
            else
                _BridgeSpacing[index] = value;
        }
    }
}
