using Newtonsoft.Json;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Export
{

    [Serializable]
    public class LayoutExportOptions : INotifyPropertyChanged
    {
        [JsonProperty("Name")]
        public string ConfigurationName { get; set; }

        [JsonProperty]
        public UnitOfMeasure ExportUnit { get; set; }

        [JsonProperty]
        public bool InkscapeCompatible { get; set; }

        [JsonProperty]
        public int TargetDPI { get; set; }

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
        public LayoutLineExportConfig StringCenters { get; set; }

        [JsonIgnore]
        public bool ExportStringCenters
        {
            get => StringCenters.Enabled;
            set => StringCenters.Enabled = value;
        }

        [JsonProperty]
        public LayoutLineExportConfig CenterLine { get; set; }

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
        public LayoutLineExportConfig FingerboardEdges { get; set; }

        [JsonIgnore]
        public bool ExportFingerboardEdges
        {
            get => FingerboardEdges.Enabled;
            set => FingerboardEdges.Enabled = value;
        }

        [JsonProperty]
        public LayoutLineExportConfig FingerboardMargins { get; set; }

        [JsonIgnore]
        public bool ExportFingerboardMargins
        {
            get => FingerboardMargins.Enabled;
            set => FingerboardMargins.Enabled = value;
        }

        [JsonProperty]
        public LayoutLineExportConfig GuideLines { get; set; }

        [JsonIgnore]
        public bool ExportGuideLines
        {
            get => GuideLines.Enabled;
            set => GuideLines.Enabled = value;
        }

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

        public void DettachPropertyChangedEvent()
        {
            var configObjs = new LayoutLineExportConfig[]
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
            var configObjs = new LayoutLineExportConfig[]
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
