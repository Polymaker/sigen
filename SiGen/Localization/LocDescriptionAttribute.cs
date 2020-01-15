using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace System.ComponentModel
{
    public class LocDescriptionAttribute : DescriptionAttribute
    {
        public override string Description => GetDescription();

        public string LocKey { get; set; }

        public LocDescriptionAttribute(string locKey)
        {
            LocKey = locKey;
        }

        public string GetDescription()
        {
            return GetDescription(CultureInfo.CurrentCulture);
        }

        public string GetDescription(CultureInfo culture)
        {
            var resMan = SiGen.Resources.Localizations.ResourceManager;
            return resMan.GetString(LocKey, culture);
        }
    }
}
