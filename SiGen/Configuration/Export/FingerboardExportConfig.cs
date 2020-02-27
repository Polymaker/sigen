using Newtonsoft.Json;

namespace SiGen.Export
{
    public class FingerboardExportConfig : LineExportConfig
    {
        private bool _ContinueLines;

        [JsonProperty]
        public bool ContinueLines
        {
            get => _ContinueLines;
            set => SetPropertyValue(ref _ContinueLines, value);
        }
    }
}
