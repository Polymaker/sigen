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
    public class StringsDisplayConfig : LineDisplayConfig
    {
        private LineRenderMode _RenderMode;

        [JsonProperty, JsonConverter(typeof(StringEnumConverter))]
        public LineRenderMode RenderMode
        {
            get => _RenderMode;
            set => SetPropertyValue(ref _RenderMode, value);
        }

        public override bool Equals(object obj)
        {
            return obj is StringsDisplayConfig config &&
                   base.Equals(obj) &&
                   RenderMode == config.RenderMode;
        }

        public override int GetHashCode()
        {
            var hashCode = 850630672;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + RenderMode.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(StringsDisplayConfig left, StringsDisplayConfig right)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return ReferenceEquals(left, null) && ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(StringsDisplayConfig left, StringsDisplayConfig right)
        {
            return !(left == right);
        }

        #region Winform Designer Specifics

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetRenderMode()
        {
            RenderMode = (DefaultValues as StringsDisplayConfig)?.RenderMode ?? LineRenderMode.PlainLine;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeRenderMode()
        {
            return RenderMode != ((DefaultValues as StringsDisplayConfig)?.RenderMode ?? LineRenderMode.PlainLine);
        }

        public override bool ShouldSerializeConfig()
        {
            return base.ShouldSerializeConfig() || ShouldSerializeRenderMode();
        }

        public override void ResetConfig()
        {
            base.ResetConfig();
            ResetRenderMode();
        }

        public override void InitDefaultValues()
        {
            DefaultValues = new StringsDisplayConfig
            {
                Color = Color,
                Visible = Visible,
                RenderMode = RenderMode
            };
        }

        #endregion
    }
}
