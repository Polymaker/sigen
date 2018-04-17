using SiGen.Measuring;
using System;
using System.Xml.Serialization;

namespace SiGen.StringedInstruments.Data
{
    [Serializable]
    public class StringProperties
    {
        /// <summary>
        /// The diameter of the string (core wire for wound strings).
        /// Used for fret compensation calculations.
        /// </summary>
        [XmlAttribute("CoreWireDiameter")]
        public Measure CoreWireDiameter { get; set; }
        /// <summary>
        /// The outer diameter of a wound string.
        /// Used mainly for display.
        /// </summary>
        [XmlAttribute("StringDiameter")]
        public Measure StringDiameter { get; set; }
        /// <summary>
        /// The weight per unit volume of the string's material (core wire for wound strings) in lbs./ inch.
        /// Used for fret compensation calculations.
        /// </summary>
        [XmlAttribute("UW")]
        public double UnitWeight { get; set; }
        /// <summary>
        /// The modulus of elasticity of the string's material (core wire for wound strings; if defined).
        /// Used for fret compensation calculations.
        /// </summary>
        [XmlAttribute("MOE")]
        public double ModulusOfElasticity { get; set; }
        /// <summary>
        /// The string's material name.
        /// Used for display purpose only.
        /// </summary>
        [XmlAttribute("Material")]
        public string Material { get; set; }
        /// <summary>
        /// Gets the area of the core wire in inch².
        /// Used for fret compensation calculations.
        /// </summary>
        public double CoreWireArea
        {
            get
            {
                if (CoreWireDiameter.IsEmpty)
                    return 0;
                return Math.Pow(CoreWireDiameter[UnitOfMeasure.In] / 2d, 2) * Math.PI;
            }
        }
        /// <summary>
        /// Gets the area of the string in inch².
        /// Used for fret compensation calculations.
        /// </summary>
        public double StringArea
        {
            get
            {
                if (StringDiameter.IsEmpty)
                    return 0;
                return Math.Pow(StringDiameter[UnitOfMeasure.In] / 2d, 2) * Math.PI;
            }
        }

        public bool CanCalculateCompensation
        {
            get { return (CoreWireArea != 0 || StringArea != 0) && ModulusOfElasticity > 0 && UnitWeight > 0; }
        }

        public StringProperties()
        {
            Material = string.Empty;
        }

        public StringProperties(double uw, Measure diam, double moe)
        {
            Material = string.Empty;
            CoreWireDiameter = diam;
            UnitWeight = uw;
            ModulusOfElasticity = moe;
        }

        public StringProperties(Measure coreDiam, Measure gauge, double uw, double moe)
        {
            Material = string.Empty;
            
            CoreWireDiameter = coreDiam;
            StringDiameter = gauge;
            UnitWeight = uw;
            ModulusOfElasticity = moe;
        }

        public bool ShouldSerializeMaterial()
        {
            return !string.IsNullOrEmpty(Material);
        }
    }
}
