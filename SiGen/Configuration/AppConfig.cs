using Newtonsoft.Json;
using SiGen.Export;
using SiGen.UI;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace SiGen.Configuration
{
    [XmlRoot("SiGen")]
    public class AppConfig
    {
        public static AppConfig Current => AppConfigManager.Current;

        [JsonProperty]
        public int MaxRecentFileHistory { get; set; }

        [JsonProperty]
        public List<RecentFile> RecentFiles { get; set; }

        //[JsonIgnore]
        //public List<LayoutExportConfig> ExportConfigs { get; set; }

        [JsonProperty]
        public LayoutExportConfig ExportConfig { get; set; }

        [JsonProperty]
        public ViewerDisplayConfig DisplayConfig { get; set; }

        public AppConfig()
        {
            //ExportConfigs = new List<LayoutExportConfig>();
            RecentFiles = new List<RecentFile>();
        }

        public static AppConfig CreateDefault()
        {
            return new AppConfig()
            {
                MaxRecentFileHistory = 10,
                ExportConfig = LayoutExportConfig.CreateDefault(),
                DisplayConfig = ViewerDisplayConfig.CreateDefault()
            };
        }

        public AppConfig Clone()
        {
            return new AppConfig()
            {
                MaxRecentFileHistory = MaxRecentFileHistory,
                ExportConfig = ExportConfig.Clone(),
                DisplayConfig = DisplayConfig.Clone()
            };
        }
    }


}
