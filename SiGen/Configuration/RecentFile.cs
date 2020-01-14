using Newtonsoft.Json;
using System.Xml.Serialization;

namespace SiGen.Configuration
{
    public class RecentFile
    {
        [XmlAttribute("Filename")]
        [JsonProperty("Path")]
        public string Filename { get; set; }

        [XmlIgnore]
        [JsonProperty("Name", NullValueHandling = NullValueHandling.Ignore)]
        public string LayoutName { get; set; }

        public RecentFile() { Filename = string.Empty; }
        public RecentFile(string filename) { Filename = filename; }

        public RecentFile(string filename, string layoutName)
        {
            Filename = filename;
            LayoutName = layoutName;
        }
    }
}
