using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SiGen.StringedInstruments.Layout
{
    public abstract class ScaleLengthManager : ActivableLayoutComponent
	{
        protected LengthFunction _LengthCalculationMethod;
        /// <summary>
        /// Determine if the scale length is applied along each strings (taking into account the neck taper) or straight along the fingerboard.
        /// </summary>
        /// <remarks>Originally this value was assignable by string, but that complicated considerably the layout calculation and it is not really usefull anyway.</remarks>
        public LengthFunction LengthCalculationMethod
        {
            get { return _LengthCalculationMethod; }
            set
            {
				SetPropertyValue(ref _LengthCalculationMethod, value);
            }
        }

        public abstract ScaleLengthType Type { get; }

		public override bool IsActive => Layout.CurrentScaleLength == this;

		public ScaleLengthManager(SILayout layout) : base(layout)
        {
            _LengthCalculationMethod = LengthFunction.AlongString;
        }

        public abstract void SetLength(int index, Measure value);

        public abstract Measure GetLength(int index);

        public virtual XElement Serialize(string elemName)
        {
            return new XElement(elemName, new XAttribute("Type", Type), new XAttribute("LengthFunction", LengthCalculationMethod));
        }

        internal virtual void Deserialize(XElement elem)
        {
            LengthCalculationMethod = (LengthFunction)Enum.Parse(typeof(LengthFunction), elem.Attribute("LengthFunction").Value);
        }
    }
}
