using SiGen.Measuring;
using SiGen.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Frets
{
    public class /*struct*/ FretPosition
    {
        private readonly int _StringIndex;
        private readonly int _FretIndex;
        private double _DistanceRatio;
        private PitchValue _PitchOffset;
        private PointM _Position;
        private bool _IsVirtual;

        /// <summary>
        /// Gets the fret string's index.
        /// </summary>
        public int StringIndex { get { return _StringIndex; } }

        /// <summary>
        /// Gets the index of the fret.
        /// </summary>
        public int FretIndex { get { return _FretIndex; } }

        /// <summary>
        /// Gets the fret's distance from the bridge.
        /// The value is relative to the scale length.
        /// </summary>
        public double DistanceRatio { get { return _DistanceRatio; } }

        /// <summary>
        /// Gets the fret's pitch distance from the string's open note.
        /// </summary>
        public PitchValue PitchOffset { get { return _PitchOffset; } set { _PitchOffset = value; } }

        /// <summary>
        /// Gets the fret 2D position on the layout.
        /// </summary>
        public PointM Position { get { return _Position; } set { _Position = value; } }

        /// <summary>
        /// Gets a value indicating if the string actually possess this fret or not.
        /// Virtual frets are used for computation.
        /// </summary>
        public bool IsVirtual { get { return _IsVirtual; } }

        public FretPosition(int stringIndex, int fretIndex, double distanceRatio, bool isVirtual)
        {
            _StringIndex = stringIndex;
            _FretIndex = fretIndex;
            _IsVirtual = isVirtual;
            _DistanceRatio = distanceRatio;
        }
    }
}
