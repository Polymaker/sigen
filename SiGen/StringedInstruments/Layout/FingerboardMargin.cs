using SiGen.Measuring;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SiGen.StringedInstruments.Layout
{
    public class FingerboardMargin : LayoutComponent
    {
        private Measure _Treble;
        private Measure _Bass;
        //private Measure _TrebleAtNut;
        //private Measure _BassAtNut;
        //private Measure _TrebleAtBridge;
        //private Measure _BassAtBridge;
        private Measure _LastFret;
        


        [XmlAttribute("Treble")]
        public Measure Treble
        {
            get { return _Treble; }
            set
            {
                if(value != _Treble)
                {
                    _Treble = value;
                    Layout.NotifyLayoutChanged(this, "FingerboardMargin");
                }
            }
        }

        [XmlAttribute("Bass")]
        public Measure Bass
        {
            get { return _Bass; }
            set
            {
                if (value != _Bass)
                {
                    _Bass = value;
                    Layout.NotifyLayoutChanged(this, "FingerboardMargin");
                }
            }
        }

        [XmlAttribute("LastFret")]
        public Measure LastFret
        {
            get { return _LastFret; }
            set
            {
                if (value != _LastFret)
                {
                    _LastFret = value;
                    Layout.NotifyLayoutChanged(this, "FingerboardMargin");
                }
            }
        }

        public Measure Edges
        {
            get { return Treble != Bass ? Measure.Empty : Treble; }
            set
            {
                Treble = Bass = value;
            }
        }

        public FingerboardMargin(SILayout layout) : base(layout)
        {
            _LastFret = Measure.Empty;

        }

        //public class 
    }
}
