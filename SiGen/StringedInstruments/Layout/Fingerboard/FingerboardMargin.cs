using SiGen.Measuring;
using SiGen.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace SiGen.StringedInstruments.Layout
{
    public class FingerboardMargin : LayoutComponent
    {
        //private Measure _Treble;
        //private Measure _Bass;
        private Measure _NutTrebleMargin;
        private Measure _NutBassMargin;
        private Measure _BridgeTrebleMargin;
        private Measure _BridgeBassMargin;
        private Measure _LastFret;
        private bool _CompensateStringGauge;

        private ArrayProperty<Measure, FingerboardSide> _NutMargins;
        private ArrayProperty<Measure, FingerboardSide> _BridgeMargins;
        private ArrayProperty<Measure, FingerboardEnd> _TrebleMargins;
        private ArrayProperty<Measure, FingerboardEnd> _BassMargins;

        #region Edges (Nut/Bridge) margins accessors

        public Measure Treble
        {
            get { return _NutTrebleMargin != _BridgeTrebleMargin ? Measure.Empty : _NutTrebleMargin; }
            set
            {
				Layout.StartBatchChanges(nameof(Treble));
				SetFieldValue(ref _NutTrebleMargin, value, nameof(_NutTrebleMargin));
				SetFieldValue(ref _BridgeTrebleMargin, value, nameof(_BridgeTrebleMargin));
				Layout.FinishBatchChanges();
            }
        }

        public Measure Bass
        {
            get { return _NutBassMargin != _BridgeBassMargin ? Measure.Empty : _NutBassMargin; }
            set
            {
				Layout.StartBatchChanges(nameof(Bass));
				SetFieldValue(ref _NutBassMargin, value, nameof(_NutBassMargin));
				SetFieldValue(ref _BridgeBassMargin, value, nameof(_BridgeBassMargin));
				Layout.FinishBatchChanges();
            }
        }

        public Measure MarginAtNut
        {
            get { return _NutBassMargin != _NutTrebleMargin ? Measure.Empty : _NutBassMargin; }
            set
            {
				Layout.StartBatchChanges(nameof(MarginAtNut));
				SetFieldValue(ref _NutBassMargin, value, nameof(_NutBassMargin));
				SetFieldValue(ref _NutTrebleMargin, value, nameof(_NutTrebleMargin));
				Layout.FinishBatchChanges();
            }
        }

        public Measure MarginAtBridge
        {
            get { return _BridgeBassMargin != _BridgeTrebleMargin ? Measure.Empty : _BridgeBassMargin; }
            set
            {
				Layout.StartBatchChanges(nameof(MarginAtBridge));
                SetFieldValue(ref _BridgeBassMargin, value, nameof(_BridgeBassMargin));
				SetFieldValue(ref _BridgeTrebleMargin, value, nameof(_BridgeTrebleMargin));
				Layout.FinishBatchChanges();
            }
        }

        public ArrayProperty<Measure, FingerboardSide> NutMargins
        {
            get { return _NutMargins; }
        }

        public ArrayProperty<Measure, FingerboardSide> BridgeMargins
        {
            get { return _BridgeMargins; }
        }

        public ArrayProperty<Measure, FingerboardEnd> TrebleMargins
        {
            get { return _TrebleMargins; }
        }

        public ArrayProperty<Measure, FingerboardEnd> BassMargins
        {
            get { return _BassMargins; }
        }

        public Measure Edges
        {
            get { return Treble != Bass ? Measure.Empty : Treble; }
            set
            {
                Layout.StartBatchChanges(nameof(Edges));
                Treble = Bass = value;
                Layout.FinishBatchChanges();
            }
        }

        #endregion

        public Measure LastFret
        {
            get { return _LastFret; }
            set
            {
				SetPropertyValue(ref _LastFret, value);
			}
        }

        public bool CompensateStringGauge
        {
            get { return _CompensateStringGauge; }
            set
            {
				SetPropertyValue(ref _CompensateStringGauge, value);
			}
        }

        public FingerboardMargin(SILayout layout) : base(layout)
        {
            _LastFret = Measure.Empty;
            _NutMargins = new ArrayProperty<Measure, FingerboardSide>(
                (s) => s == FingerboardSide.Treble ? _NutTrebleMargin : _NutBassMargin,
                (s,m) => {
					if (s == FingerboardSide.Treble)
						SetFieldValue(ref _NutTrebleMargin, m, nameof(_NutTrebleMargin));
					else
						SetFieldValue(ref _NutBassMargin, m, nameof(_NutBassMargin));
				}
            );
            _BridgeMargins = new ArrayProperty<Measure, FingerboardSide>(
                (s) => s == FingerboardSide.Treble ? _BridgeTrebleMargin : _BridgeBassMargin,
                (s, m) => {
					if (s == FingerboardSide.Treble)
						SetFieldValue(ref _BridgeTrebleMargin, m, nameof(_BridgeTrebleMargin));
					else
						SetFieldValue(ref _BridgeBassMargin, m, nameof(_BridgeBassMargin));
                }
            );
            _TrebleMargins = new ArrayProperty<Measure, FingerboardEnd>(
                (s) => s == FingerboardEnd.Nut ? _NutTrebleMargin : _BridgeTrebleMargin,
                (s, m) => {
					if (s == FingerboardEnd.Nut)
						SetFieldValue(ref _NutTrebleMargin, m, nameof(_NutTrebleMargin));
					else
						SetFieldValue(ref _BridgeTrebleMargin, m, nameof(_BridgeTrebleMargin));
				}
            );
            _BassMargins = new ArrayProperty<Measure, FingerboardEnd>(
                (s) => s == FingerboardEnd.Nut ? _NutBassMargin : _BridgeBassMargin,
                (s, m) => {
					if (s == FingerboardEnd.Nut)
						SetFieldValue(ref _NutBassMargin, m, nameof(_NutBassMargin));
					else
						SetFieldValue(ref _BridgeBassMargin, m, nameof(_BridgeBassMargin));
                }
            );
        }

        public XElement Serialize(string elemName)
        {
            var elem = new XElement(elemName);
            if (!Edges.IsEmpty)
            {
                elem.Add(Edges.SerializeAsAttribute("Edges"));
            }
            else if (!Treble.IsEmpty && !Bass.IsEmpty)
            {
                elem.Add(Treble.SerializeAsAttribute("Treble"));
                elem.Add(Bass.SerializeAsAttribute("Bass"));
            }
            else if (!MarginAtNut.IsEmpty && !MarginAtBridge.IsEmpty)
            {
                elem.Add(MarginAtNut.SerializeAsAttribute("Nut"));
                elem.Add(MarginAtBridge.SerializeAsAttribute("Bridge"));
            }
            else
            {
                elem.Add(_NutTrebleMargin.SerializeAsAttribute("NutTreble"));
                elem.Add(_NutBassMargin.SerializeAsAttribute("NutBass"));
                elem.Add(_BridgeTrebleMargin.SerializeAsAttribute("BridgeTreble"));
                elem.Add(_BridgeBassMargin.SerializeAsAttribute("BridgeBass"));
            }
            
            elem.Add(new XAttribute("Compensated", CompensateStringGauge));
            elem.Add(LastFret.SerializeAsAttribute("LastFret"));

            return elem;
        }

        public void Deserialize(XElement elem)
        {
            if(elem.ContainsAttribute("LastFret"))
                _LastFret = Measure.ParseInvariant(elem.Attribute("LastFret").Value);
            if (elem.ContainsAttribute("Edges"))
            {
                _NutTrebleMargin = _NutBassMargin = _BridgeTrebleMargin = _BridgeBassMargin = Measure.ParseInvariant(elem.Attribute("Edges").Value);
            }
            else if (elem.ContainsAttribute("Treble") && elem.ContainsAttribute("Bass"))
            {
                _NutTrebleMargin = _BridgeTrebleMargin = Measure.ParseInvariant(elem.Attribute("Treble").Value);
                _NutBassMargin = _BridgeBassMargin = Measure.ParseInvariant(elem.Attribute("Bass").Value);
            }
            else if (elem.ContainsAttribute("Nut") && elem.ContainsAttribute("Bridge"))
            {
                _NutTrebleMargin = _NutBassMargin = Measure.ParseInvariant(elem.Attribute("Nut").Value);
                _BridgeTrebleMargin = _BridgeBassMargin = Measure.ParseInvariant(elem.Attribute("Bridge").Value);
            }
            else
            {
                if (elem.ContainsAttribute("NutTreble"))
                    _NutTrebleMargin = Measure.ParseInvariant(elem.Attribute("NutTreble").Value);
                else
                    _NutTrebleMargin = Measure.Zero;
                if (elem.ContainsAttribute("NutBass"))
                    _NutBassMargin = Measure.ParseInvariant(elem.Attribute("NutBass").Value);
                else
                    _NutBassMargin = Measure.Zero;
                if (elem.ContainsAttribute("BridgeTreble"))
                    _BridgeTrebleMargin = Measure.ParseInvariant(elem.Attribute("BridgeTreble").Value);
                else
                    _BridgeTrebleMargin = Measure.Zero;
                if (elem.ContainsAttribute("BridgeBass"))
                    _BridgeBassMargin = Measure.ParseInvariant(elem.Attribute("BridgeBass").Value);
                else
                    _BridgeBassMargin = Measure.Zero;
            }

            if (elem.ContainsAttribute("Compensated"))
                CompensateStringGauge = bool.Parse(elem.Attribute("Compensated").Value);
        }
    }
}
