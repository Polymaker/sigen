using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class StringsSpacingBase : LayoutComponent, IStringsSpacing
    {
        private StringSpacingAlignment _BridgeAlignment;
        private StringSpacingAlignment _NutAlignment;

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
            get { return GetSpacingBetweenStrings(0, Layout.NumberOfStrings - 1, false); }
        }

        public virtual Measure StringSpreadAtNut
        {
            get { return GetSpacingBetweenStrings(0, Layout.NumberOfStrings - 1, true); }
        }

        public StringsSpacingBase(SILayout layout) : base(layout)
        {
            layout.NumberOfStringsChanged += Layout_NumberOfStringsChanged;
        }

        private void Layout_NumberOfStringsChanged(object sender, EventArgs e)
        {
            OnNumberOfStringsChanged();
        }

        protected virtual void OnNumberOfStringsChanged() { }

        public Measure GetSpacingBetweenStrings(int index1, int index2, bool atNut)
        {
            Measure total = Measure.Zero;
            for (int i = Math.Min(index1, index2); i < Math.Max(index1, index2); i++)
                total += GetSpacing(i, atNut);
            return total;
        }

        public abstract Measure GetSpacing(int index, bool atNut);

        public abstract void SetSpacing(int index, Measure value, bool atNut);

        public Measure[] GetStringPositions(bool atNut, out Measure center)
        {
            var stringPos = new Measure[NumberOfStrings];
            for (int i = 0; i < NumberOfStrings; i++)
            {
                if (i == 0)
                    stringPos[i] = Measure.Zero;
                else
                    stringPos[i] = stringPos[i - 1] + GetSpacingBetweenStrings(i - 1, i, atNut);
            }
            if ((atNut ? NutAlignment : BridgeAlignment) == StringSpacingAlignment.StringCenter)
                center = (atNut ? StringSpreadAtNut : StringSpreadAtBridge) / 2d;
            else if (NumberOfStrings % 2 == 1)//odd number of strings
                center = stringPos[(NumberOfStrings - 1) / 2];
            else//even number of strings
            {
                int mid = NumberOfStrings / 2;
                center = ((stringPos[mid] - stringPos[mid - 1]) / 2) + stringPos[mid - 1];
            }

            for (int i = 0; i < NumberOfStrings; i++)
                stringPos[i] -= center;//offset each strings position so they are centered relative to the layout
            return stringPos;
        }

        public StringsSpacingManual ConvertToManual()
        {
            if (this is StringsSpacingManual)
                return (StringsSpacingManual)this;
            var newSpacing = new StringsSpacingManual(Layout);
            for (int i = 0; i < Layout.NumberOfStrings - 1; i++)
            {
                newSpacing.SetSpacing(i, GetSpacing(i, true), true);
                newSpacing.SetSpacing(i, GetSpacing(i, false), false);
            }
            return newSpacing;
        }

        public StringsSpacingSimple ConvertToSimple()
        {
            if (this is StringsSpacingSimple)
                return (StringsSpacingSimple)this;
            var newSpacing = new StringsSpacingSimple(Layout);
            newSpacing.StringSpacingAtNut = StringSpreadAtNut / (Layout.NumberOfStrings - 1);
            newSpacing.StringSpreadAtBridge = StringSpreadAtBridge / (Layout.NumberOfStrings - 1);
            return newSpacing;
        }
    }
}
