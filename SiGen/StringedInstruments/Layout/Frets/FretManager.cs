using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.StringedInstruments.Layout
{
    public class FretManager : LayoutComponent
    {
		public SIString String { get; }

		private int _StartingFret;
		private int _NumberOfFrets;
		private FretPositionCollection _Frets;

		/// <summary>
		/// Gets or sets the number of fret.
		/// </summary>
		public int NumberOfFrets
		{
			get { return _NumberOfFrets; }
			set
			{
				SetPropertyValue(ref _NumberOfFrets, value);
			}
		}

		/// <summary>
		/// Gets or sets the starting fret. The default value is 0 (nut). 
		/// <para>A positive value places the starting fret (or nut) further down the fingerboard (eg: a banjo's first string starts at the fifth fret).
		/// A negative value places the starting fret (or nut) behind the nut (a.k.a. negative fret).</para>
		/// </summary>
		/// <example>test</example>
		public int StartingFret
		{
			get { return _StartingFret; }
			set
			{
				SetPropertyValue(ref _StartingFret, value);
			}
		}

		/// <summary>
		/// Gets or sets the total number of frets taking into account the starting fret.
		/// <para>E.g. the first string of a 22 frets banjo starts at the fifth fret so in total that string has 17 frets.</para>
		/// </summary>
		public int TotalNumberOfFrets
		{
			get { return NumberOfFrets - StartingFret; }
			set
			{
				NumberOfFrets = value + StartingFret;
			}
		}

		internal FretManager(SIString str) : base(str.Layout)
        {
			String = str;
			_Frets = new FretPositionCollection(this);
		}

		public void GenerateFretPositions()
		{
			for (int i = Layout.MinimumFret; i <= Layout.MaximumFret; i++)
			{

			}
		}
    }
}
