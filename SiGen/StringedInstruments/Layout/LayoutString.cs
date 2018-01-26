using SiGen.StringedInstruments.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class LayoutString
    {
        #region Fields

        private readonly SILayout _Layout;
        private readonly int _Index;
        private int _NumberOfFrets;
        private int _StartingFret;

        //private StringProperties _PhysicalProperties;

        #endregion

        #region Properties

        public SILayout Layout { get { return _Layout; } }

        /// <summary>
        /// Gets or sets the number of fret. This value d
        /// </summary>
        public int NumberOfFrets
        {
            get { return _NumberOfFrets; }
            set
            {
                if(value != _NumberOfFrets)
                {
                    _NumberOfFrets = value;
                    Layout.OnStringConfigChanged(this, "NumberOfFrets");
                }
            }
        }

        /// <summary>
        /// Gets or sets the starting fret. The default value is 0 (nut). 
        /// A positive value places the starting fret (or nut) further down the fingerboard (eg: a banjo's first string starts at the fifth fret).
        /// A negative value places the starting fret (or nut) behind the nut (a.k.a. negative fret).
        /// </summary>
        public int StartingFret
        {
            get { return _StartingFret; }
            set
            {
                if (value != _StartingFret)
                {
                    _StartingFret = value;
                    Layout.OnStringConfigChanged(this, "StartingFret");
                }
            }
        }

        /// <summary>
        /// Gets or sets the total number of frets taking into account the starting fret.
        /// E.g. the first string of a 22 frets banjo starts at the fifth fret so in total that string has 17 frets.
        /// </summary>
        public int TotalNumberOfFrets
        {
            get { return NumberOfFrets - StartingFret; }
            set
            {
                NumberOfFrets = value + StartingFret;
            }
        }

        #endregion

        public LayoutString(SILayout layout, int stringIndex)
        {
            _Layout = layout;
            _Index = stringIndex;
        }
    }
}
