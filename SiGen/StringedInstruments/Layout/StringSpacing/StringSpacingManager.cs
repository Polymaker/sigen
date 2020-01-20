using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;
using System.Xml.Linq;
using System.Globalization;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class StringSpacingManager : ActivableLayoutComponent//, IStringsSpacing
	{
        private StringSpacingAlignment _BridgeAlignment;
        private StringSpacingAlignment _NutAlignment;

        public abstract StringSpacingType Type { get; }

        public StringSpacingAlignment BridgeAlignment
        {
            get => _BridgeAlignment;
            set => SetPropertyValue(ref _BridgeAlignment, value);
        }

        public StringSpacingAlignment NutAlignment
        {
            get => _NutAlignment;
            set => SetPropertyValue(ref _NutAlignment, value);
        }

        public virtual Measure StringSpreadAtBridge
        {
            get { return GetSpacingBetweenStrings(0, Layout.NumberOfStrings - 1,  FingerboardEnd.Bridge); }
            set { throw new NotSupportedException(); }
        }

        public virtual Measure StringSpreadAtNut
        {
            get { return GetSpacingBetweenStrings(0, Layout.NumberOfStrings - 1, FingerboardEnd.Nut); }
            set { throw new NotSupportedException(); }
        }

		public override bool IsActive => Layout.StringSpacing == this;

		public StringSpacingManager(SILayout layout) : base(layout)
        {
        }

        public Measure GetSpacingBetweenStrings(int index1, int index2, FingerboardEnd side)
        {
            if (index1 == index2)
                return Measure.Zero;

            Measure total = Measure.Zero;
            for (int i = Math.Min(index1, index2); i < Math.Max(index1, index2); i++)
                total += GetSpacing(i, side);
            return total;
        }

        public abstract Measure GetSpacing(int index, FingerboardEnd side);

        public abstract void SetSpacing(FingerboardEnd side, int index, Measure value);

        public Measure[] GetStringPositions(FingerboardEnd side, out Measure center)
        {
            var stringPos = new Measure[NumberOfStrings];
            for (int i = 0; i < NumberOfStrings; i++)
            {
                if (i == 0)
                    stringPos[i] = Measure.Zero;
                else
                    stringPos[i] = stringPos[i - 1] + GetSpacingBetweenStrings(i - 1, i, side);
            }

            var centerAlign = (side == FingerboardEnd.Nut) ? NutAlignment : BridgeAlignment;
            var stringSpread = (side == FingerboardEnd.Nut) ? StringSpreadAtNut : StringSpreadAtBridge;

            switch (centerAlign)
            {
                default:
                case StringSpacingAlignment.FingerboardEdges:
                    {
                        if (Layout.Margins.CompensateStringGauge && 
                            !(Layout.FirstString.Gauge.IsEmpty || Layout.LastString.Gauge.IsEmpty))
                        {
                            var bassMargin = Layout.Margins.BassMargins[side] + Layout.LastString.Gauge / 2d;
                            var trebleMargin = Layout.Margins.TrebleMargins[side] + Layout.FirstString.Gauge / 2d;
                            
                            var totalWidth = bassMargin + stringSpread + trebleMargin;
                            center = totalWidth / 2d;
                            center -= trebleMargin;
                        }
                        else
                            center = stringSpread / 2d;
                        break;
                    }
                case StringSpacingAlignment.OuterStrings:
                    center = stringSpread / 2d;
                    break;
                case StringSpacingAlignment.MiddleString:
                    {
                        if (NumberOfStrings % 2 == 1)//odd number of strings
                        {
                            center = stringPos[(NumberOfStrings - 1) / 2];
                        }
                        else
                        {
                            //even number of strings
                            int mid = NumberOfStrings / 2;
                            center = ((stringPos[mid] - stringPos[mid - 1]) / 2) + stringPos[mid - 1];
                        }
                        break;
                    }
            }

            for (int i = 0; i < NumberOfStrings; i++)
            {
                //offset each strings position so they are centered relative to the layout
                //multiply by -1 to match string order (treble->bass = right->left)
                stringPos[i] = (stringPos[i] - center) * -1;
            }
            return stringPos;
        }

        //public StringSpacingManual ConvertToManual()
        //{
        //    if (this is StringSpacingManual)
        //        return (StringSpacingManual)this;
        //    var newSpacing = new StringSpacingManual(Layout);
        //    for (int i = 0; i < Layout.NumberOfStrings - 1; i++)
        //    {
        //        newSpacing.SetSpacing(FingerboardEnd.Nut, i, GetSpacing(i, FingerboardEnd.Nut));
        //        newSpacing.SetSpacing(FingerboardEnd.Bridge, i, GetSpacing(i, FingerboardEnd.Bridge));
        //    }
        //    return newSpacing;
        //}

        //public StringSpacingSimple ConvertToSimple()
        //{
        //    if (this is StringSpacingSimple)
        //        return (StringSpacingSimple)this;
        //    var newSpacing = new StringSpacingSimple(Layout);
        //    if(Layout.NumberOfStrings > 1)
        //    {
        //        newSpacing.StringSpacingAtNut = StringSpreadAtNut / (Layout.NumberOfStrings - 1);
        //        newSpacing.StringSpreadAtBridge = StringSpreadAtBridge / (Layout.NumberOfStrings - 1);
        //    }
        //    return newSpacing;
        //}

        public virtual XElement Serialize(string elemName)
        {
            var elem = new XElement(elemName,
                new XAttribute("Mode", (this is StringSpacingSimple) ? "Simple" : "Manual"),
                new XAttribute("NutAlignment", NutAlignment),
                StringSpreadAtNut.SerializeAsAttribute("StringSpreadAtNut"),
                new XAttribute("BridgeAlignment", BridgeAlignment),
                StringSpreadAtBridge.SerializeAsAttribute("StringSpreadAtBridge"));

            for (int i = 0; i < NumberOfStrings - 1; i++)
            {
                elem.Add(new XElement("Spacing",
                    new XAttribute("Index", i),
                    GetSpacing(i, FingerboardEnd.Nut).SerializeAsAttribute("Nut"),
                    GetSpacing(i, FingerboardEnd.Bridge).SerializeAsAttribute("Bridge")));
            }

            return elem;
        }

        public virtual void Deserialize(XElement elem)
        {
            NutAlignment = elem.ReadAttribute("NutAlignment", StringSpacingAlignment.OuterStrings);
            BridgeAlignment = elem.ReadAttribute("BridgeAlignment", StringSpacingAlignment.OuterStrings);

            foreach (var spacingElem in elem.Elements("Spacing"))
            {
                int spacingIndex = spacingElem.ReadAttribute<int>("Index");
                SetSpacing(FingerboardEnd.Nut, spacingIndex,
                    spacingElem.ReadAttribute("Nut", Measure.Zero));

                SetSpacing(FingerboardEnd.Bridge, spacingIndex,
                    spacingElem.ReadAttribute("Bridge", Measure.Zero));
            }
        }
    }
}
