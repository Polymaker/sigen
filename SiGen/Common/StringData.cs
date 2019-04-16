using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiGen.Common
{
    public class StringData
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }
        [XmlAttribute("Brand")]
        public string Brand { get; set; }
        [XmlAttribute("CD")]
        public Measure? CoreDiameter { get; set; }
        [XmlAttribute("OD")]
        public Measure OuterDiameter { get; set; }
        [XmlAttribute("UW")]
        public double? UnitWeight { get; set; }
        [XmlAttribute("MoE")]
        public double? ModulusOfElasticity { get; set; }
        [XmlAttribute("Type")]
        public StringType Type { get; set; }
    }
}
