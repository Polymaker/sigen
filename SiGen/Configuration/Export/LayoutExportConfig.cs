using Newtonsoft.Json;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{

    [Serializable]
    public class LayoutExportConfig : INotifyPropertyChanged
    {
        //[JsonProperty("Name")]
        //public string ConfigurationName { get; set; }

        [JsonProperty]
        public UnitOfMeasure ExportUnit { get; set; }

        [JsonProperty]
        public bool InkscapeCompatible { get; set; }

        [JsonProperty]
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

        [JsonProperty]
        public LineExportConfig StringCenters { get; set; }

        [JsonIgnore]
        public bool ExportStringCenters
        {
            get => StringCenters.Enabled;
            set => StringCenters.Enabled = value;
        }

        [JsonProperty]
        public LineExportConfig CenterLine { get; set; }

        [JsonIgnore]
        public bool ExportCenterLine
        {
            get => CenterLine.Enabled;
            set => CenterLine.Enabled = value;
        }

        [JsonProperty]
        public FretsExportConfig Frets { get; set; }

        [JsonIgnore]
        public bool ExportFrets
        {
            get => Frets.Enabled;
            set => Frets.Enabled = value;
        }

        [JsonIgnore]
        public bool ExtendFretSlots => Frets.ExtendFretSlots;

        [JsonProperty]
        public FingerboardExportConfig FingerboardEdges { get; set; }

        [JsonIgnore]
        public bool ExportFingerboardEdges
        {
            get => FingerboardEdges.Enabled;
            set => FingerboardEdges.Enabled = value;
        }

        [JsonProperty]
        public LineExportConfig FingerboardMargins { get; set; }

        [JsonIgnore]
        public bool ExportFingerboardMargins
        {
            get => FingerboardMargins.Enabled;
            set => FingerboardMargins.Enabled = value;
        }

        [JsonProperty]
        public LineExportConfig GuideLines { get; set; }

        [JsonIgnore]
        public bool ExportGuideLines
        {
            get => GuideLines.Enabled;
            set => GuideLines.Enabled = value;
        }

        public LayoutExportConfig()
        {
            Strings = new StringsExportConfig();
            StringCenters = new LineExportConfig();
            CenterLine = new LineExportConfig();
            Frets = new FretsExportConfig();
            FingerboardEdges = new FingerboardExportConfig();
            FingerboardMargins = new LineExportConfig();
            GuideLines = new LineExportConfig();
            ExportFrets = true;
            ExportFingerboardEdges = true;
            ExportUnit = UnitOfMeasure.Mm;
        }

        public LayoutExportConfig Clone()
        {
            string json = JsonConvert.SerializeObject(this);
            JsonSerializer serializer = new JsonSerializer();
            using (var sr = new StringReader(json))
                return (LayoutExportConfig)serializer.Deserialize(sr, typeof(LayoutExportConfig));
        }

        public void DettachPropertyChangedEvent()
        {
            var configObjs = new LineExportConfig[]
            {
                Strings,
                StringCenters,
                CenterLine,
                Frets,
                FingerboardEdges,
                FingerboardMargins,
                GuideLines
            };

            for (int i = 0; i < configObjs.Length; i++)
                configObjs[i].PropertyChanged -= ConfigObj_PropertyChanged;
        }

        public void AttachPropertyChangedEvent()
        {
            var configObjs = new LineExportConfig[]
            {
                Strings,
                StringCenters,
                CenterLine,
                Frets,
                FingerboardEdges,
                FingerboardMargins,
                GuideLines
            };

            for (int i = 0; i < configObjs.Length; i++)
            {
                configObjs[i].PropertyChanged -= ConfigObj_PropertyChanged;
                configObjs[i].PropertyChanged += ConfigObj_PropertyChanged;
            }
        }

        private void ConfigObj_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender == Strings)
                OnPropertyChanged($"{nameof(Strings)}.{e.PropertyName}");

            else if (sender == StringCenters)
                OnPropertyChanged($"{nameof(StringCenters)}.{e.PropertyName}");

            else if (sender == CenterLine)
                OnPropertyChanged($"{nameof(CenterLine)}.{e.PropertyName}");

            else if (sender == Frets)
                OnPropertyChanged($"{nameof(Frets)}.{e.PropertyName}");

            else if (sender == FingerboardEdges)
                OnPropertyChanged($"{nameof(FingerboardEdges)}.{e.PropertyName}");

            else if (sender == FingerboardMargins)
                OnPropertyChanged($"{nameof(FingerboardMargins)}.{e.PropertyName}");

            else if (sender == GuideLines)
                OnPropertyChanged($"{nameof(GuideLines)}.{e.PropertyName}");
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static LayoutExportConfig CreateDefault()
        {
            var exportConfig = new LayoutExportConfig()
            {
                //ConfigurationName = "Default",
                ExportFrets = true,
                ExportFingerboardEdges = true,
                ExportCenterLine = true,
                ExportUnit = UnitOfMeasure.Mm,
                InkscapeCompatible = true
            };
            exportConfig.Frets.Color = Color.Red;
            exportConfig.Strings.Color = Color.Black;
            exportConfig.StringCenters.Color = Color.LightGray;
            exportConfig.StringCenters.IsDashed = true;
            exportConfig.FingerboardEdges.Color = Color.Blue;
            exportConfig.FingerboardEdges.ContinueLines = true;
            exportConfig.FingerboardMargins.Color = Color.Gray;
            exportConfig.GuideLines.Color = Color.LightGray;
            exportConfig.GuideLines.IsDashed = true;
            return exportConfig;
        }
    }
}
