using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiGen.Configuration
{
    public class DisplayPreferences
    {
        private List<ScreenConfiguration> _Screens;

        [XmlArray("MonitorsConfiguration"), XmlArrayItem("Screen")]
        public List<ScreenConfiguration> Screens { get { return _Screens; } }

        public DisplayPreferences()
        {
            _Screens = new List<ScreenConfiguration>();
        }
    }
}
