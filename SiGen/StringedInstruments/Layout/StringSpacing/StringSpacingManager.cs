using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class StringSpacingManager : LayoutComponent//, IStringsSpacing
    {
        private StringSpacingAlignment _BridgeAlignment;
        private StringSpacingAlignment _NutAlignment;

        public abstract StringSpacingType Type { get; }

        public StringSpacingAlignment BridgeAlignment
        {
            get { return _BridgeAlignment; }
            set
            {
                if(value != _BridgeAlignment)
                {
                    _BridgeAlignment = value;
                    Layout.NotifyLayoutChanged(this, "BridgeAlignment");
                }
            }
        }

        public StringSpacingAlignment NutAlignment
        {
            get { return _NutAlignment; }
            set
            {
                if (value != _NutAlignment)
                {
                    _NutAlignment = value;
                    Layout.NotifyLayoutChanged(this, "NutAlignment");
                }
            }
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
            if ((side == FingerboardEnd.Nut ? NutAlignment : BridgeAlignment) == StringSpacingAlignment.SpacingMiddle)
                center = (side == FingerboardEnd.Nut ? StringSpreadAtNut : StringSpreadAtBridge) / 2d;
            else if (NumberOfStrings % 2 == 1)//odd number of strings
                center = stringPos[(NumberOfStrings - 1) / 2];
            else//even number of strings
            {
                int mid = NumberOfStrings / 2;
                center = ((stringPos[mid] - stringPos[mid - 1]) / 2) + stringPos[mid - 1];
            }

            for (int i = 0; i < NumberOfStrings; i++)
            {
                //offset each strings position so they are centered relative to the layout
                //multiply by -1 to match string order (treble->bass = right->left)
                stringPos[i] = (stringPos[i] - center) * -1;
            }
            return stringPos;
        }

        public StringSpacingManual ConvertToManual()
        {
            if (this is StringSpacingManual)
                return (StringSpacingManual)this;
            var newSpacing = new StringSpacingManual(Layout);
            for (int i = 0; i < Layout.NumberOfStrings - 1; i++)
            {
                newSpacing.SetSpacing(FingerboardEnd.Nut, i, GetSpacing(i, FingerboardEnd.Nut));
                newSpacing.SetSpacing(FingerboardEnd.Bridge, i, GetSpacing(i, FingerboardEnd.Bridge));
            }
            return newSpacing;
        }

        public StringSpacingSimple ConvertToSimple()
        {
            if (this is StringSpacingSimple)
                return (StringSpacingSimple)this;
            var newSpacing = new StringSpacingSimple(Layout);
            if(Layout.NumberOfStrings > 1)
            {
                newSpacing.StringSpacingAtNut = StringSpreadAtNut / (Layout.NumberOfStrings - 1);
                newSpacing.StringSpreadAtBridge = StringSpreadAtBridge / (Layout.NumberOfStrings - 1);
            }
            return newSpacing;
        }

        public virtual XElement Serialize(string elemName)
        {
            var elem = new XElement(elemName,
                new XAttribute("Mode", (this is StringSpacingSimple) ? "Simple" : "Manual"),
                new XAttribute("NutAlignment", NutAlignment),
                StringSpreadAtNut.SerializeAsAttribute("StringSpreadAtNut"),
                new XAttribute("BridgeAlignment", BridgeAlignment),
                StringSpreadAtBridge.SerializeAsAttribute("StringSpreadAtBridge"));
            for (int i = 0; i < NumberOfStrings - 1; i++)
                elem.Add(new XElement("Spacing", 
                    new XAttribute("Index", i), 
                    GetSpacing(i, FingerboardEnd.Nut).SerializeAsAttribute("Nut"),
                    GetSpacing(i, FingerboardEnd.Bridge).SerializeAsAttribute("Bridge")));
            return elem;
        }

        public virtual void Deserialize(XElement elem)
        {
            NutAlignment = (StringSpacingAlignment)Enum.Parse(typeof(StringSpacingAlignment), elem.Attribute("NutAlignment").Value);
            BridgeAlignment = (StringSpacingAlignment)Enum.Parse(typeof(StringSpacingAlignment), elem.Attribute("BridgeAlignment").Value);
            foreach(var spacingElem in elem.Elements("Spacing"))
            {
                SetSpacing(FingerboardEnd.Nut, spacingElem.GetIntAttribute("Index"), Measure.TryParse(spacingElem.Attribute("Nut").Value, Measure.Zero));
                SetSpacing(FingerboardEnd.Bridge, spacingElem.GetIntAttribute("Index"), Measure.TryParse(spacingElem.Attribute("Bridge").Value, Measure.Zero));
            }
        }
    }
}
