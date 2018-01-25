using SiGen.Measuring;
using System;

namespace SiGen.StringedInstruments.Data
{
    public class StringProperties
    {
        /// <summary>
        /// The diameter of the string (core wire for wound strings).
        /// Used for fret compensation calculations.
        /// </summary>
        public Measure CoreWireDiameter { get; set; }
        /// <summary>
        /// The outer diameter of a wound string.
        /// Used mainly for display.
        /// </summary>
        public Measure StringDiameter { get; set; }
        /// <summary>
        /// The weight per unit volume of the string's material (core wire for wound strings) in lbs./ inch.
        /// Used for fret compensation calculations.
        /// </summary>
        public double UnitWeight { get; set; }
        /// <summary>
        /// The modulus of elasticity of the string's material (core wire for wound strings).
        /// Used for fret compensation calculations.
        /// </summary>
        public double ModulusOfElasticity { get; set; }
        /// <summary>
        /// The string's material name.
        /// Used for display purpose only.
        /// </summary>
        public string Material { get; set; }
        /// <summary>
        /// Gets the area of the string (core wire for wound strings).
        /// </summary>
        public Measure CoreWireArea
        {
            get
            {
                if (CoreWireDiameter.IsEmpty)
                    return Measure.Empty;
                return Measure.FromNormalizedValue(Math.Pow(CoreWireDiameter.NormalizedValue / 2d, 2) * Math.PI, CoreWireDiameter.Unit);
            }
        }
    }
}
