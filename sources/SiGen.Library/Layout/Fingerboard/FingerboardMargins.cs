using SiGen.Measuring;
using SiGen.Layout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Fingerboard
{
    public class FingerboardMargins : LayoutComponent
	{
        #region Fields

        private Measure _NutTrebleMargin;
        private Measure _NutBassMargin;
        private Measure _BridgeTrebleMargin;
        private Measure _BridgeBassMargin;
        private Measure _LastFretOverhang;
        private bool _CompensateStringGauge;

		#endregion

		#region Properties

		public override ComponentTypes Type => ComponentTypes.Fingerboard;

		public Measure Treble
        {
            get { return _NutTrebleMargin != _BridgeTrebleMargin ? Measure.Empty : _NutTrebleMargin; }
            set
            {
				SetMultipleValues(() =>
				{
					SetFieldValue(ref _NutTrebleMargin, value, nameof(_NutTrebleMargin));
					SetFieldValue(ref _BridgeTrebleMargin, value, nameof(_BridgeTrebleMargin));
				});
            }
        }

        public Measure Bass
        {
            get { return _NutBassMargin != _BridgeBassMargin ? Measure.Empty : _NutBassMargin; }
            set
            {
				SetMultipleValues(() =>
				{
					SetFieldValue(ref _NutBassMargin, value, nameof(_NutBassMargin));
					SetFieldValue(ref _BridgeBassMargin, value, nameof(_BridgeBassMargin));
				});
            }
        }

        public Measure MarginAtNut
        {
            get { return _NutBassMargin != _NutTrebleMargin ? Measure.Empty : _NutBassMargin; }
            set
            {
				SetMultipleValues(() =>
				{
					SetFieldValue(ref _NutBassMargin, value, nameof(_NutBassMargin));
					SetFieldValue(ref _NutTrebleMargin, value, nameof(_NutTrebleMargin));
				});
            }
        }

        public Measure MarginAtBridge
        {
            get { return _BridgeBassMargin != _BridgeTrebleMargin ? Measure.Empty : _BridgeBassMargin; }
            set
            {
				SetMultipleValues(() =>
				{
					SetFieldValue(ref _BridgeBassMargin, value, nameof(_BridgeBassMargin));
					SetFieldValue(ref _BridgeTrebleMargin, value, nameof(_BridgeTrebleMargin));
				});
            }
        }

		public ArrayProperty<Measure, FingerboardSide> NutMargins { get; }

		public ArrayProperty<Measure, FingerboardSide> BridgeMargins { get; }

		public ArrayProperty<Measure, FingerboardEnd> TrebleMargins { get; }

		public ArrayProperty<Measure, FingerboardEnd> BassMargins { get; }

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
				SetPropertyValue(ref _LastFretOverhang, value);
			}
        }

        public bool CompensateStringGauge
        {
			get => _CompensateStringGauge;
			set => SetPropertyValue(ref _CompensateStringGauge, value);
		}

        #endregion

        #region Ctor

        internal FingerboardMargins(InstrumentLayout layout) : base(layout)
        {
            _LastFretOverhang = Measure.Empty;
            NutMargins = new ArrayProperty<Measure, FingerboardSide>(
                (s) => s == FingerboardSide.Treble ? _NutTrebleMargin : _NutBassMargin,
                (s, m) => {
					if (s == FingerboardSide.Treble)
						SetFieldValue(ref _NutTrebleMargin, m, nameof(_NutTrebleMargin));
					else
						SetFieldValue(ref _NutBassMargin, m, nameof(_NutBassMargin));
                }
            );
            BridgeMargins = new ArrayProperty<Measure, FingerboardSide>(
                (s) => s == FingerboardSide.Treble ? _BridgeTrebleMargin : _BridgeBassMargin,
                (s, m) => {
					if (s == FingerboardSide.Treble)
						SetFieldValue(ref _BridgeTrebleMargin, m, nameof(_BridgeTrebleMargin));
					else
						SetFieldValue(ref _BridgeBassMargin, m, nameof(_BridgeBassMargin));
				}
            );
            TrebleMargins = new ArrayProperty<Measure, FingerboardEnd>(
                (s) => s == FingerboardEnd.Nut ? _NutTrebleMargin : _BridgeTrebleMargin,
                (s, m) => {
					if (s == FingerboardEnd.Nut)
						SetFieldValue(ref _NutTrebleMargin, m, nameof(_NutTrebleMargin));
					else
						SetFieldValue(ref _BridgeTrebleMargin, m, nameof(_BridgeTrebleMargin));
				}
            );
            BassMargins = new ArrayProperty<Measure, FingerboardEnd>(
                (s) => s == FingerboardEnd.Nut ? _NutBassMargin : _BridgeBassMargin,
                (s, m) => {
					if (s == FingerboardEnd.Nut)
						SetFieldValue(ref _NutBassMargin, m, nameof(_NutBassMargin));
					else
						SetFieldValue(ref _BridgeBassMargin, m, nameof(_BridgeBassMargin));
				}
            );
        }

        #endregion

    }
}
