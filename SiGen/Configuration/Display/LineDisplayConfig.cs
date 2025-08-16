using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration.Display
{
    public class LineDisplayConfig : ConfigObjectBase
    {
        internal LineDisplayConfig DefaultValues;

        private bool _Visible;
        private bool _Dashed;
        //private float _Thickness;
        private Color _Color;

        [JsonProperty]
        public bool Visible
        {
            get => _Visible;
            set => SetPropertyValue(ref _Visible, value);
        }

        [JsonIgnore]
        public bool Dashed
        {
            get => _Dashed;
            set => SetPropertyValue(ref _Dashed, value);
        }

        //[JsonIgnore]
        //public float Thickness
        //{
        //    get => _Thickness;
        //    set => SetPropertyValue(ref _Thickness, value);
        //}

        [JsonProperty, JsonConverter(typeof(ColorJsonConverter))]
        public Color Color
        {
            get => _Color;
            set => SetPropertyValue(ref _Color, value);
        }

        public override bool Equals(object obj)
        {
            return obj is LineDisplayConfig config &&
                   Visible == config.Visible &&
                   Color == config.Color;
        }

        public override int GetHashCode()
        {
            var hashCode = 1868664161;
            hashCode = hashCode * -1521134295 + Visible.GetHashCode();
            hashCode = hashCode * -1521134295 + Color.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(LineDisplayConfig left, LineDisplayConfig right)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
                return ReferenceEquals(left, null) && ReferenceEquals(right, null);
            return left.Equals(right);
        }

        public static bool operator !=(LineDisplayConfig left, LineDisplayConfig right)
        {
            return !(left == right);
        }

        public void ApplyExportConfig(SiGen.Export.LineExportConfig config)
        {
            Color = config.Color;
            Dashed = config.IsDashed;
            Visible = config.Enabled;
            //if (config.LineUnit == Export.LineUnit.Pixels)
        }


        #region Winform Designer Specifics


        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetVisible()
        {
            Visible = DefaultValues?.Visible ?? false;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeVisible()
        {
            return Visible != (DefaultValues?.Visible ?? false);
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetColor()
        {
            Color = DefaultValues?.Color ?? Color.Empty;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeColor()
        {
            return Color != (DefaultValues?.Color ?? Color.Empty);
        }

        public virtual bool ShouldSerializeConfig()
        {
            return ShouldSerializeColor() || ShouldSerializeVisible();
        }

        public virtual void ResetConfig()
        {
            ResetColor();
            ResetVisible();
        }

        public virtual void InitDefaultValues()
        {
            DefaultValues = new LineDisplayConfig
            {
                Color = Color,
                Visible = Visible
            };
        }

        public void SetDefaultValues(LineDisplayConfig displayConfig)
        {
            DefaultValues = displayConfig;
        }

        public virtual void CopyValues(LineDisplayConfig displayConfig)
        {
            Color = displayConfig.Color;
            Visible = displayConfig.Visible;
        }

        #endregion


    }
}
