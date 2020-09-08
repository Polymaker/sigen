using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Configuration.Display
{
    public class FingerboardDisplayConfig : LineDisplayConfig
    {
        private bool _ContinueLines;

        [JsonProperty]
        public bool ContinueLines
        {
            get => _ContinueLines;
            set => SetPropertyValue(ref _ContinueLines, value);
        }

        #region Winform Designer Specifics

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public void ResetContinueLines()
        {
            ContinueLines = (DefaultValues as FingerboardDisplayConfig)?.ContinueLines ?? false;
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public bool ShouldSerializeContinueLines()
        {
            return ContinueLines != ((DefaultValues as FingerboardDisplayConfig)?.ContinueLines ?? false);
        }

        public override void InitDefaultValues()
        {
            DefaultValues = new FingerboardDisplayConfig()
            {
                Color = Color,
                Visible = Visible,
                ContinueLines = ContinueLines
            };
        }

        public override void CopyValues(LineDisplayConfig displayConfig)
        {
            base.CopyValues(displayConfig);
            if (displayConfig is FingerboardDisplayConfig fingerboard)
            {
                ContinueLines = fingerboard.ContinueLines;
            }
        }

        #endregion
    }
}
