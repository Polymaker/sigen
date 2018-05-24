using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Sound
{
    public struct PitchValue
    {
        #region Fields

        private double _Cents;
        private double _Ratio;

        #endregion

        #region Properties

        public double Cents { get { return _Cents; } set { _Cents = value; _Ratio = NoteConverter.CentsToIntonationRatio(value); } }
        public double Ratio { get { return _Ratio; } set { _Ratio = value; _Cents = NoteConverter.IntonationRatioToCents(value); } }

        public double Frequency
        {
            get { return NoteConverter.CalculateFrequency(this); }
        }

        #endregion

        #region Ctors

        public PitchValue(double cents, double ratio)
        {
            _Cents = cents;
            _Ratio = ratio;
        }

        #endregion

        #region Static Ctors

        public static PitchValue FromCents(double cents)
        {
            return new PitchValue(cents, NoteConverter.CentsToIntonationRatio(cents));
        }

        public static PitchValue FromRatio(double ratio)
        {
            return new PitchValue(NoteConverter.IntonationRatioToCents(ratio), ratio);
        }

        #endregion

        #region Operators

        public static PitchValue operator +(PitchValue p1, PitchValue p2)
        {
            return FromCents(p1.Cents + p2.Cents);
        }

        public static PitchValue operator -(PitchValue p1, PitchValue p2)
        {
            return FromCents(p1.Cents - p2.Cents);
        }

        #endregion
    }
}
