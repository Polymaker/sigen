using Newtonsoft.Json;
using SiGen.Export;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiGen.Configuration
{
    [XmlRoot("SiGen")]
    public class AppConfig
    {
        [JsonProperty]
        public int MaxRecentFileHistory { get; set; }

        [JsonProperty]
        public List<RecentFile> RecentFiles { get; set; }

        [JsonIgnore]
        public List<LayoutExportOptions> ExportConfigs { get; set; }


        public AppConfig()
        {
            ExportConfigs = new List<LayoutExportOptions>();
            RecentFiles = new List<RecentFile>();
        }

        public static AppConfig CreateDefault()
        {
            return new AppConfig()
            {
                MaxRecentFileHistory = 10,
            };
        }
    }


}
