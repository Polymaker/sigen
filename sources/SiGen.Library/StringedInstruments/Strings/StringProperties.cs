using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiGen.Measuring;

namespace SiGen.StringedInstruments.Strings
{
    public class StringProperties
    {
        /// <summary>
        /// The diameter of the string (core wire for wound strings).
        /// Used for fret compensation calculations.
        /// </summary>
        public Measure CoreWireDiameter { get; set; }

        /// <summary>
        /// Gets the area of the core wire.
        /// Used for fret compensation calculations.
        /// </summary>
        public MeasureSquared CoreWireArea
        {
            get
            {
                if (CoreWireDiameter.IsEmpty)
                    return MeasureSquared.Empty;
                return (CoreWireDiameter / 2).Squared() * Math.PI;
            }
        }

        /// <summary>
        /// The outer diameter of a wound string.
        /// Used mainly for display.
        /// </summary>
        public Measure StringDiameter { get; set; }

        /// <summary>
        /// Gets the area of the core wire.
        /// Used for fret compensation calculations.
        /// </summary>
        public MeasureSquared StringArea
        {
            get
            {
                if (StringDiameter.IsEmpty)
                    return MeasureSquared.Empty;
                return (StringDiameter / 2).Squared() * Math.PI;
            }
        }

        /// <summary>
        /// The weight per unit volume of the string's material (core wire for wound strings) in lbs./ inch.
        /// Used for fret compensation calculations.
        /// </summary>
        public double UnitWeight { get; set; }

        /// <summary>
        /// The modulus of elasticity of the string's material (core wire for wound strings; if defined).
        /// Used for fret compensation calculations.
        /// </summary>
        public double ModulusOfElasticity { get; set; }


    }
}
