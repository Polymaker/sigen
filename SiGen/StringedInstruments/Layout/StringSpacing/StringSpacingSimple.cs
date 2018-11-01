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
        private StringSpacingMethod _NutSpacingMode;
        private StringSpacingMethod _BridgeSpacingMode;

        private Measure[] AdjustedNutSpacings;
        private Measure[] AdjustedBridgeSpacings;

        public override StringSpacingType Type
        {
            get { return StringSpacingType.Simple; }
        }

        public Measure StringSpacingAtNut
        {
            get => _StringSpacingAtNut;
            set => SetPropertyValue(ref _StringSpacingAtNut, value);
        }

        public Measure StringSpacingAtBridge
        {
            get => _StringSpacingAtBridge;
            set => SetPropertyValue(ref _StringSpacingAtBridge, value);
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

        public StringSpacingMethod NutSpacingMode
        {
            get => _NutSpacingMode;
            set => SetPropertyValue(ref _NutSpacingMode, value);
        }

        public StringSpacingMethod BridgeSpacingMode
        {
            get => _BridgeSpacingMode;
            set => SetPropertyValue(ref _BridgeSpacingMode, value);
        }

        public StringSpacingSimple(SILayout layout) : base(layout)
        {
            _NutSpacingMode = StringSpacingMethod.StringsCenter;
            _BridgeSpacingMode = StringSpacingMethod.StringsCenter;
            AdjustedNutSpacings = new Measure[0];
            AdjustedBridgeSpacings = new Measure[0];
        }

        public override Measure GetSpacing(int index, FingerboardEnd side)
        {
            if (side == FingerboardEnd.Nut && NutSpacingMode == StringSpacingMethod.BetweenStrings && AdjustedNutSpacings.Length > 0)
                return AdjustedNutSpacings[index];
            else if (side == FingerboardEnd.Bridge && BridgeSpacingMode == StringSpacingMethod.BetweenStrings && AdjustedBridgeSpacings.Length > 0)
                return AdjustedBridgeSpacings[index];

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
            if(NutSpacingMode == StringSpacingMethod.BetweenStrings && AdjustedNutSpacings != null && AdjustedNutSpacings.Length > 0)
                elem.AddFirst(new XComment("Nut slot positions are adjusted in consideration of the strings gauge"));
            elem.Add(new XAttribute("NutSpacingMode", NutSpacingMode));
            elem.Add(new XAttribute("BridgeSpacingMode", BridgeSpacingMode));
            elem.Add(StringSpacingAtNut.SerializeAsAttribute("StringSpacingAtNut"));
            elem.Add(StringSpacingAtBridge.SerializeAsAttribute("StringSpacingAtBridge"));
            return elem;
        }

        public override void Deserialize(XElement elem)
        {
            base.Deserialize(elem);
            if (elem.ContainsAttribute("NutSpacingMode"))
            {
                NutSpacingMode = (StringSpacingMethod)Enum.Parse(typeof(StringSpacingMethod), elem.Attribute("NutSpacingMode").Value);

                if (NutSpacingMode == StringSpacingMethod.BetweenStrings)
                {
                    StringSpacingAtNut = Measure.ParseInvariant(elem.Attribute("StringSpacingAtNut").Value);
                    AdjustedNutSpacings = new Measure[elem.Elements("Spacing").Count()];
                    foreach (var spacingElem in elem.Elements("Spacing"))
                    {
                        int spacingIndex = spacingElem.GetIntAttribute("Index");
                        AdjustedNutSpacings[spacingIndex] = Measure.ParseInvariant(spacingElem.Attribute("Nut").Value);
                    }
                }
            }
            if (elem.ContainsAttribute("BridgeSpacingMode"))
            {
                BridgeSpacingMode = (StringSpacingMethod)Enum.Parse(typeof(StringSpacingMethod), elem.Attribute("BridgeSpacingMode").Value);

                if (BridgeSpacingMode == StringSpacingMethod.BetweenStrings)
                {
                    StringSpacingAtBridge = Measure.ParseInvariant(elem.Attribute("StringSpacingAtBridge").Value);
                    AdjustedBridgeSpacings = new Measure[elem.Elements("Spacing").Count()];
                    foreach (var spacingElem in elem.Elements("Spacing"))
                    {
                        int spacingIndex = spacingElem.GetIntAttribute("Index");
                        AdjustedBridgeSpacings[spacingIndex] = Measure.ParseInvariant(spacingElem.Attribute("Nut").Value);
                    }
                }
            }
        }

        public void CalculateAdjustedPositions()
        {
            AdjustedNutSpacings = new Measure[0];
            AdjustedBridgeSpacings = new Measure[0];

            if (Layout.Strings.All(s => s.Gauge != Measure.Empty) && NumberOfStrings >= 2)
            {
                if(NutSpacingMode == StringSpacingMethod.BetweenStrings)
                {
                    AdjustedNutSpacings = new Measure[NumberOfStrings - 1];
                    var spacing = StringSpreadAtNut - Layout.Strings.Sum(s => s.Gauge);
                    spacing += (Layout.FirstString.Gauge / 2) + (Layout.LastString.Gauge / 2);
                    spacing /= NumberOfStrings - 1;
                    for (int i = 0; i < NumberOfStrings - 1; i++)
                    {
                        AdjustedNutSpacings[i] = (Layout.Strings[i].Gauge / 2) + spacing + (Layout.Strings[i + 1].Gauge / 2);
                        AdjustedNutSpacings[i].Unit = StringSpacingAtNut.Unit;
                    }
                }

                if (BridgeSpacingMode == StringSpacingMethod.BetweenStrings)
                {
                    AdjustedBridgeSpacings = new Measure[NumberOfStrings - 1];
                    var spacing = StringSpreadAtBridge - Layout.Strings.Sum(s => s.Gauge);
                    spacing += (Layout.FirstString.Gauge / 2) + (Layout.LastString.Gauge / 2);
                    spacing /= NumberOfStrings - 1;
                    for (int i = 0; i < NumberOfStrings - 1; i++)
                    {
                        AdjustedBridgeSpacings[i] = (Layout.Strings[i].Gauge / 2) + spacing + (Layout.Strings[i + 1].Gauge / 2);
                        AdjustedBridgeSpacings[i].Unit = StringSpacingAtBridge.Unit;
                    }
                }
            }
        }
    }
}
