using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class StringSpacingManual : StringSpacingManager
    {
        private Measure[] _NutSpacing;
        private Measure[] _BridgeSpacing;

        public override StringSpacingType Type
        {
            get { return StringSpacingType.Manual; }
        }

        public Measure[] NutSpacing { get { return _NutSpacing; } }
        public Measure[] BridgeSpacing { get { return _BridgeSpacing; } }

        public StringSpacingManual(SILayout layout) : base(layout)
        {

        }

        protected override void OnStringConfigurationChanged()
        {
            if (_NutSpacing != null && _NutSpacing.Length > 0)
            {
                var oldNut = _NutSpacing;
                var oldBridge = _BridgeSpacing;
                _NutSpacing = new Measure[Layout.NumberOfStrings - 1];
                _BridgeSpacing = new Measure[Layout.NumberOfStrings - 1];

                for (int i = 0; i < Layout.NumberOfStrings - 1; i++)
                {
                    if (i < oldNut.Length)
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

        public override Measure GetSpacing(int index, FingerboardEnd end)
        {
            return end == FingerboardEnd.Nut ? _NutSpacing[index] : _BridgeSpacing[index];
        }

        public override void SetSpacing(FingerboardEnd end, int index, Measure value)
        {
			if (end == FingerboardEnd.Nut)
				SetFieldValue(ref _NutSpacing, index, value, nameof(_NutSpacing));
			else
				SetFieldValue(ref _BridgeSpacing, index, value, nameof(_BridgeSpacing));
        }
    }
}
