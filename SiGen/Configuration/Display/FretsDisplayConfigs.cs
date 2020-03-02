using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration.Display
{
    public class FretsDisplayConfigs : LineDisplayConfig
    {
        private Measure _RenderWidth;
        private LineRenderMode _RenderMode;
        private bool _DisplayAccuratePositions;
        private bool _DisplayBridgeLine;

        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public LineRenderMode RenderMode
        {
            get => _RenderMode;
            set => SetPropertyValue(ref _RenderMode, value);
        }

        [Editor(typeof(UI.Controls.Designers.MeasureEditor), typeof(System.Drawing.Design.UITypeEditor))]
        [JsonProperty, JsonConverter(typeof(MeasureJsonConverter))]
        public Measure RenderWidth
        {
            get => _RenderWidth;
            set => SetPropertyValue(ref _RenderWidth, value);
        }

        [JsonProperty]
        public bool DisplayAccuratePositions
        {
            get => _DisplayAccuratePositions;
            set => SetPropertyValue(ref _DisplayAccuratePositions, value);
        }

        [JsonProperty]
        public bool DisplayBridgeLine
        {
            get => _DisplayBridgeLine;
            set => SetPropertyValue(ref _DisplayBridgeLine, value);
        }

        #region Winform Designer Specifics

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetRenderMode()
        {
            RenderMode = (DefaultValues as FretsDisplayConfigs)?.RenderMode ?? LineRenderMode.PlainLine;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeRenderMode()
        {
            return RenderMode != ((DefaultValues as FretsDisplayConfigs)?.RenderMode ?? LineRenderMode.PlainLine);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetRenderWidth()
        {
            RenderWidth = (DefaultValues as FretsDisplayConfigs)?.RenderWidth ?? Measure.Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeRenderWidth()
        {
            return RenderWidth != ((DefaultValues as FretsDisplayConfigs)?.RenderWidth ?? Measure.Empty);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetDisplayAccuratePositions()
        {
            DisplayAccuratePositions = (DefaultValues as FretsDisplayConfigs)?.DisplayAccuratePositions ?? false;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeDisplayAccuratePositions()
        {
            return DisplayAccuratePositions != ((DefaultValues as FretsDisplayConfigs)?.DisplayAccuratePositions ?? false);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetDisplayBridgeLine()
        {
            DisplayBridgeLine = (DefaultValues as FretsDisplayConfigs)?.DisplayBridgeLine ?? false;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeDisplayBridgeLine()
        {
            return DisplayBridgeLine != ((DefaultValues as FretsDisplayConfigs)?.DisplayBridgeLine ?? false);
        }

        public override bool ShouldSerializeConfig()
        {
            return base.ShouldSerializeConfig() || 
                ShouldSerializeRenderMode() ||
                ShouldSerializeRenderWidth() ||
                ShouldSerializeDisplayAccuratePositions() ||
                ShouldSerializeDisplayBridgeLine();
        }

        public override void ResetConfig()
        {
            base.ResetConfig();
            ResetRenderMode();
            ResetRenderWidth();
            ResetDisplayAccuratePositions();
            ResetDisplayBridgeLine();
        }

        public override void InitDefaultValues()
        {
            DefaultValues = new FretsDisplayConfigs
            {
                Color = Color,
                Visible = Visible,
                RenderMode = RenderMode,
                RenderWidth = RenderWidth,
                DisplayAccuratePositions = DisplayAccuratePositions,
                DisplayBridgeLine = DisplayBridgeLine
            };
        }

        public override void CopyValues(LineDisplayConfig displayConfig)
        {
            base.CopyValues(displayConfig);
            if (displayConfig is FretsDisplayConfigs fretsDisplay)
            {
                RenderMode = fretsDisplay.RenderMode;
                RenderWidth = fretsDisplay.RenderWidth;
                DisplayAccuratePositions = fretsDisplay.DisplayAccuratePositions;
                DisplayBridgeLine = fretsDisplay.DisplayBridgeLine;
            }
        }

        #endregion
    }
}
