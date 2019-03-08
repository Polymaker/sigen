using SiGen.StringedInstruments.Fingerboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiGen.Layout.Strings
{
	public class StringManager
	{
		public static FingerboardSide DefaultSide = FingerboardSide.Bass;

		private int _NumberOfStrings;
		private StringConfiguration[] _Strings;

		public int NumberOfStrings
		{
			get => _NumberOfStrings;
			set
			{
				if (value > 0 && value != _NumberOfStrings)
				{
					if (value > _NumberOfStrings)
					{
						for (int i = _Strings.Length; i < value; i++)
							AddString();
						_NumberOfStrings = value;
					}
					else
					{

					}
				}
			}
		}

		public void AddString()
		{
			AddString(DefaultSide);
		}

		public void AddString(FingerboardSide side)
		{

		}

		public void RemoveString(FingerboardSide side)
		{

		}

		public void RemoveString(int index)
		{

		}
	}
}
