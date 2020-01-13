using Newtonsoft.Json;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{

    [Serializable]
    public class LayoutExportOptions
    {
        public string ConfigurationName { get; set; }

        public StringsExportConfig Strings { get; set; }

        [JsonIgnore]
        public bool ExportStrings
        {
            get => Strings.Enabled;
            set => Strings.Enabled = value;
        }

        [JsonIgnore]
        public bool UseStringGauge
        {
            get => Strings.UseStringGauge;
            set => Strings.UseStringGauge = value;
        }

        public LayoutLineExportConfig StringCenters { get; set; }

        [JsonIgnore]
        public bool ExportStringCenters
        {
            get => StringCenters.Enabled;
            set => StringCenters.Enabled = value;
        }

        public LayoutLineExportConfig CenterLine { get; set; }

        [JsonIgnore]
        public bool ExportCenterLine
        {
            get => CenterLine.Enabled;
            set => CenterLine.Enabled = value;
        }

        public FretsExportConfig Frets { get; set; }

        [JsonIgnore]
        public bool ExportFrets
        {
            get => Frets.Enabled;
            set => Frets.Enabled = value;
        }

        public bool ExtendFretSlots => Frets.ExtendFretSlots;

        public LayoutLineExportConfig FingerboardEdges { get; set; }

        [JsonIgnore]
        public bool ExportFingerboardEdges
        {
            get => FingerboardEdges.Enabled;
            set => FingerboardEdges.Enabled = value;
        }

        public LayoutLineExportConfig FingerboardMargins { get; set; }

        [JsonIgnore]
        public bool ExportFingerboardMargins
        {
            get => FingerboardMargins.Enabled;
            set => FingerboardMargins.Enabled = value;
        }

        public LayoutLineExportConfig GuideLines { get; set; }

        [JsonIgnore]
        public bool ExportGuideLines
        {
            get => GuideLines.Enabled;
            set => GuideLines.Enabled = value;
        }

        public bool InkscapeCompatible { get; set; }

        public int TargetDPI { get; set; }

        public UnitOfMeasure ExportUnit { get; set; }

        public LayoutExportOptions()
        {
            Strings = new StringsExportConfig();
            StringCenters = new LayoutLineExportConfig();
            CenterLine = new LayoutLineExportConfig();
            Frets = new FretsExportConfig();
            FingerboardEdges = new LayoutLineExportConfig();
            FingerboardMargins = new LayoutLineExportConfig();
            GuideLines = new LayoutLineExportConfig();
            ExportFrets = true;
            ExportFingerboardEdges = true;
            ExportUnit = UnitOfMeasure.Mm;
        }

        public static LayoutExportOptions CreateDefault()
        {
            var exportConfig = new LayoutExportOptions()
            {
                ConfigurationName = "Default",
                ExportFrets = true,
                ExportFingerboardEdges = true,
                ExportCenterLine = true,
                ExportUnit = UnitOfMeasure.Mm,
                InkscapeCompatible = true,
                TargetDPI = 90
            };
            exportConfig.Frets.Color = Color.Red;
            exportConfig.Strings.Color = Color.Black;
            exportConfig.StringCenters.Color = Color.LightGray;
            exportConfig.FingerboardEdges.Color = Color.Blue;
            exportConfig.FingerboardMargins.Color = Color.Gray;
            exportConfig.GuideLines.Color = Color.LightGray;
            return exportConfig;
        }
    }
}
