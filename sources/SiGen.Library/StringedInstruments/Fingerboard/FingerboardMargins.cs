using SiGen.Measuring;
using SiGen.StringedInstruments.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Fingerboard
{
    public class FingerboardMargins : LayoutConfigurationItem
    {
        #region Fields

        private Measure _NutTrebleMargin;
        private Measure _NutBassMargin;
        private Measure _BridgeTrebleMargin;
        private Measure _BridgeBassMargin;
        private Measure _LastFretOverhang;
        private bool _CompensateStringGauge;
        private ArrayProperty<Measure, FingerboardSide> _NutMargins;
        private ArrayProperty<Measure, FingerboardSide> _BridgeMargins;
        private ArrayProperty<Measure, FingerboardEnd> _TrebleMargins;
        private ArrayProperty<Measure, FingerboardEnd> _BassMargins;

        #endregion

        #region Properties

        public Measure Treble
        {
            get { return _NutTrebleMargin != _BridgeTrebleMargin ? Measure.Empty : _NutTrebleMargin; }
            set
            {
                if (_NutTrebleMargin != value || _BridgeTrebleMargin != value)
                {
                    _NutTrebleMargin = _BridgeTrebleMargin = value;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            }
        }

        public Measure Bass
        {
            get { return _NutBassMargin != _BridgeBassMargin ? Measure.Empty : _NutBassMargin; }
            set
            {
                if (_NutBassMargin != value || _BridgeBassMargin != value)
                {
                    _NutBassMargin = _BridgeBassMargin = value;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            }
        }

        public Measure MarginAtNut
        {
            get { return _NutBassMargin != _NutTrebleMargin ? Measure.Empty : _NutBassMargin; }
            set
            {
                if (_NutBassMargin != value || _NutTrebleMargin != value)
                {
                    _NutBassMargin = _NutTrebleMargin = value;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            }
        }

        public Measure MarginAtBridge
        {
            get { return _BridgeBassMargin != _BridgeTrebleMargin ? Measure.Empty : _BridgeBassMargin; }
            set
            {
                if (_BridgeBassMargin != value || _BridgeTrebleMargin != value)
                {
                    _BridgeBassMargin = _BridgeTrebleMargin = value;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
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
                Treble = Bass = value;
            }
        }

        public Measure LastFretOverhang
        {
            get { return _LastFretOverhang; }
            set
            {
                if (value != _LastFretOverhang)
                {
                    _LastFretOverhang = value;
                    NotifyLayoutChanged("LastFretOverhang", LayoutProcessElement.FingerboardShape);
                }
            }
        }

        public bool CompensateStringGauge
        {
            get { return _CompensateStringGauge; }
            set
            {
                if (_CompensateStringGauge != value)
                {
                    _CompensateStringGauge = value;
                    NotifyLayoutChanged("CompensateStringGauge", LayoutProcessElement.FingerboardShape);
                }
            }
        }

        #endregion

        #region Ctor

        public FingerboardMargins()
        {
            _LastFretOverhang = Measure.Empty;
            _NutMargins = new ArrayProperty<Measure, FingerboardSide>(
                (s) => s == FingerboardSide.Treble ? _NutTrebleMargin : _NutBassMargin,
                (s, m) => {
                    if (s == FingerboardSide.Treble) _NutTrebleMargin = m;
                    else _NutBassMargin = m;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            );
            _BridgeMargins = new ArrayProperty<Measure, FingerboardSide>(
                (s) => s == FingerboardSide.Treble ? _BridgeTrebleMargin : _BridgeBassMargin,
                (s, m) => {
                    if (s == FingerboardSide.Treble) _BridgeTrebleMargin = m;
                    else _BridgeBassMargin = m;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            );
            _TrebleMargins = new ArrayProperty<Measure, FingerboardEnd>(
                (s) => s == FingerboardEnd.Nut ? _NutTrebleMargin : _BridgeTrebleMargin,
                (s, m) => {
                    if (s == FingerboardEnd.Nut) _NutTrebleMargin = m;
                    else _BridgeTrebleMargin = m;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            );
            _BassMargins = new ArrayProperty<Measure, FingerboardEnd>(
                (s) => s == FingerboardEnd.Nut ? _NutBassMargin : _BridgeBassMargin,
                (s, m) => {
                    if (s == FingerboardEnd.Nut) _NutBassMargin = m;
                    else _BridgeBassMargin = m;
                    NotifyLayoutChanged("FingerboardMargins", LayoutProcessElement.FingerboardShape);
                }
            );
        }

        #endregion

    }
}
